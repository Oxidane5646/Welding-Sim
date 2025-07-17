using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;

public class WeldSpawner : MonoBehaviour
{
    [SerializeField] GameObject weldBeadPrefab;
    [SerializeField] Transform rayReference;
    [SerializeField] float weldResolution = 0.1f;
    [SerializeField] float maxHitDistance = 10f;

    GameObject currentBead = null;
    RaycastHit weldHit;
    Vector3 spawnPoint;

    public bool CanSpawnWeld()
    {
        // Early return if required components are missing
        if (rayReference == null)
        {
            Debug.LogWarning("Ray reference is null!");
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
        if (weldHit.collider == null)
        {
            return false;
        }

        string tag = weldHit.collider.tag;

        // If no current bead exists, we can spawn
        if (currentBead == null)
        {
            return tag == "weldable" || tag == "weldPoint";
        }

        // Check distance from current bead if it exists
        if (tag == "weldable" || tag == "weldPoint")
        {
            return Vector3.Distance(currentBead.transform.position, spawnPoint) > weldResolution;
        }

        return false;
    }

    public void SpawnWeld()
    {
        if (weldBeadPrefab == null)
        {
            Debug.LogError("Weld bead prefab is not assigned!");
            return;
        }

        if (CanSpawnWeld())
        {
            currentBead = Instantiate(weldBeadPrefab, spawnPoint, Quaternion.identity);
        }
    }

    public RaycastHit? GetPositionRaycastVR(Transform rayReference, float maxHitDistance)
    {
        if (rayReference == null)
        {
            Debug.LogWarning("Ray reference is null in GetPositionRaycastVR!");
            return null;
        }

        RaycastHit hit;
        if (Physics.Raycast(rayReference.position, rayReference.forward, out hit, maxHitDistance))
        {
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
