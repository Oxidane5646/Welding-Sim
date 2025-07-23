using System;
using UnityEngine;

public class WeldSpawner : MonoBehaviour
{
    /*
     *Note:
     * create a more detailed way to simulate the welding process
     * make the onweldraycasthit event more effecient and flexible
     * make the canspawnweld method more flexible and efficient using variables for the tag references
     * 
     */

    [SerializeField] GameObject weldBeadPrefab;
    [SerializeField] Transform rayReference;
    [SerializeField] float weldResolution = 0.1f;
    [SerializeField] float maxHitDistance = 10f;

    GameObject currentBead;
    RaycastHit weldHit;
    Vector3 spawnPoint;

    public event Action<RaycastHit> OnWeldRaycastHit;
    // ReSharper disable Unity.PerformanceAnalysis
    public bool CanSpawnWeld()
    {
        // Early return if required components are missing
        if (!rayReference)
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
            return colliderTag == "weldable" || colliderTag == "weldPoint";
        }

        // Check distance from current bead if it exists
        if (colliderTag == "weldable" || colliderTag == "weldPoint")
        {
            return Vector3.Distance(currentBead.transform.position, spawnPoint) > weldResolution;
        }

        return false;
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
        if (weldBeadPrefab == null)
            Debug.LogError($"[{gameObject.name}] Weld bead prefab is not assigned!");

        if (rayReference == null)
            Debug.LogError($"[{gameObject.name}] Ray reference is not assigned!");
    }
}
