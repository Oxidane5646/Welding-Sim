using System;
using UnityEngine;

public class WeldSpawner : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private GameObject weldBeadPrefab;
    [SerializeField] private Transform rayReference;
    [SerializeField] private FillerRodCollision fillerRod;
    
    [Header("Weld Configuration")]
    [SerializeField] private float weldResolution = 0.01f;
    [SerializeField] private float maxHitDistance = 10f;

    [Header("Weld Type (select only one type)")] 
    [SerializeField] private bool isMig;
    [SerializeField] private bool isTig;
    [SerializeField] private bool isStick;

    GameObject currentBead;
    RaycastHit weldHit;
    Vector3 spawnPoint;
    GameObject weldSurface;

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
        
        var hitResult = GetPositionRaycastVR();
        
        if (!hitResult.HasValue)
        {
            return false;
        }

        weldHit = hitResult.Value;
        spawnPoint = weldHit.point;

        //This is some stupid code
        bool isWeldSurfaceAssigned = false;
        
        if (weldHit.transform.CompareTag("weldable") && !isWeldSurfaceAssigned)
        {
            weldSurface = weldHit.transform.gameObject;
            isWeldSurfaceAssigned = true;
        }

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
            currentBead.transform.SetParent(weldSurface.transform);
            currentBead.tag = "weldBead";
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public RaycastHit? GetPositionRaycastVR()
    {
        if (!rayReference)
        {
            return null;
        }

        if (Physics.Raycast(rayReference.position, rayReference.forward, out RaycastHit hit, maxHitDistance))
        {
            OnWeldRaycastHit?.Invoke(hit);
            OnWeldHitChanged(hit);
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

    #region Some stupid highlightmaterial code

    [Header("Highlight Settings")]
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material redMaterial;

    private Renderer weldSurfaceRenderer;
    private GameObject currentWeldSurface;  

    // Call this whenever your raycast hits something new
    public void OnWeldHitChanged(RaycastHit newHit)
    {
        // Update weld surface if hitting a weldable object
        if (newHit.transform.CompareTag("weldable"))
        {
            if (currentWeldSurface != newHit.transform.gameObject)
            {
                currentWeldSurface = newHit.transform.gameObject;
                weldSurfaceRenderer = currentWeldSurface.GetComponent<Renderer>();
            }
        }

        // Skip if no weld surface exists
        if (weldSurfaceRenderer == null) return;

        // Determine if hit is valid (weldable, weldPoint, or weldBead)
        bool isValidHit = newHit.transform.CompareTag("weldable") || 
                          newHit.transform.CompareTag("weldPoint") || 
                          newHit.transform.CompareTag("weldBead");

        // Apply material
        weldSurfaceRenderer.material = isValidHit ? greenMaterial : redMaterial;
    }

    public Vector3 GetSpawnPoint()
    {
        return spawnPoint;
    }

    #endregion
}
