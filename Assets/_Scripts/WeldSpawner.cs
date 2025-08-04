using System;
using UnityEngine;

public class WeldSpawner : MonoBehaviour
{
    /*
     *Note:
     * create a more detailed way to simulate the welding process
     * make the onweldraycasthit event more effecient and flexible
     * make the canspawnweld method more flexible and efficient using variables for the tag references
     */
    
    [Header("Object References")]
    [SerializeField] private GameObject weldBeadPrefab;
    [SerializeField] private Transform rayReference;
    [SerializeField] private FillerRodCollision fillerRod;
    
    [Header("Weld Configuration")]
    [SerializeField] private float weldResolution = 0.1f;
    [SerializeField] private float maxHitDistance = 10f;

    [Header("Weld Type (select only one type)")] 
    [SerializeField] private bool isMig;
    [SerializeField] private bool isTig;
    [SerializeField] private bool isStick;

    GameObject currentBead;
    RaycastHit weldHit;
    Vector3 spawnPoint;

    public event Action<RaycastHit> OnWeldRaycastHit;
    
    public bool CanSpawnWeld()
    {
        // Early return if required components are missing
        if (!rayReference)
        {
            return false;
        }
        
        //Kinda stupid but it works good for now 
        if ((isTig || isMig) && !fillerRod.GetIsColliding())
        {
            return false;
        }
        
        var hitResult = GetPositionRaycastVR(rayReference, maxHitDistance);
        
        if (!hitResult.HasValue)
        {
            return false;
        }

        weldHit = hitResult.Value;
        spawnPoint = weldHit.point;

        // Check if we hit a valid collider with a tag
        if (!weldHit.collider)
        {
            return false;
        }

        string colliderTag = weldHit.collider.tag;

        // If no current bead exists, we can spawn
        if (!currentBead)
        {
            return CheckColliderTag(colliderTag);
        }

        // Check distance from current bead if it exists
        if (CheckColliderTag(colliderTag))
        {
            return Vector3.Distance(currentBead.transform.position, spawnPoint) > weldResolution;
        }

        return false;
    }

    private bool CheckColliderTag(string colliderTag)
    {
        return colliderTag == "weldable" || colliderTag == "weldPoint";
    }

    public void SpawnWeld()
    {
        if (!weldBeadPrefab)
        {
            return;
        }

        if (CanSpawnWeld())
        {
            currentBead = Instantiate(weldBeadPrefab, spawnPoint, Quaternion.identity);
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public RaycastHit? GetPositionRaycastVR(Transform rayReference, float maxHitDistance)
    {
        if (!rayReference)
        {
            return null;
        }

        RaycastHit hit;
        if (Physics.Raycast(rayReference.position, rayReference.forward, out hit, maxHitDistance))
        {
            OnWeldRaycastHit?.Invoke(hit);
            ChangeHighlightMaterail(hit);
            Debug.DrawRay(rayReference.position, rayReference.forward * hit.distance, Color.green);
            return hit;
        }

        // Draw red ray when no hit
        Debug.DrawRay(rayReference.position, rayReference.forward * maxHitDistance, Color.red);
        return null;
    }

    // Optional: Add validation in Start/Awake
    void Start()
    {
        ValidateComponents();
    }

    void ValidateComponents()
    {
        if (!weldBeadPrefab)
            Debug.LogError($"[{gameObject.name}] Weld bead prefab is not assigned!");

        if (!rayReference)
            Debug.LogError($"[{gameObject.name}] Ray reference is not assigned!");
    }

    #region Highlighting Material stupid code
    
    [Header("Highlighting Materail")]
    [SerializeField] private Material greenHighlightMaterial;
    [SerializeField] private Material redHighlightMaterial;

    private GameObject lastWeldableHitObject;

    private void ChangeHighlightMaterail(RaycastHit hit)
    {
        GameObject hitObject = hit.collider?.gameObject;
        if (!hitObject) return;

        string tag = hitObject.tag;

        // Only change material on last "weldable" object
        if (tag == "weldable")
        {
            if (lastWeldableHitObject != hitObject)
            {
                ResetLastWeldableObject(); // turn old one red
                SetHighlightMaterial(hitObject.transform, greenHighlightMaterial);
                lastWeldableHitObject = hitObject;
            }
        }
        else if (tag == "weldPoint")
        {
            // Only update material of lastWeldableHitObject if it's valid
            if (lastWeldableHitObject)
            {
                SetHighlightMaterial(lastWeldableHitObject.transform, greenHighlightMaterial);
            }
        }
        else
        {
            ResetLastWeldableObject();
        }
    }


    private void ResetLastWeldableObject()
    {
        if (lastWeldableHitObject)
        {
            SetHighlightMaterial(lastWeldableHitObject.transform, redHighlightMaterial);
            lastWeldableHitObject = null;
        }
    }

    private void SetHighlightMaterial(Transform target, Material material)
    {
        var renderer = target.GetComponent<MeshRenderer>();
        if (renderer) renderer.material = material;
    }

    public Vector3 GetSpawnPoint()
    {
        return spawnPoint;
    }
    
    #endregion
}
