using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;

public class WeldSpawner : MonoBehaviour
{
    [SerializeField] GameObject weldBeadPrefab;
    [SerializeField] Transform rayReference;
    [SerializeField] float maxHitDistance = 10f;
    GameObject currentBead = null;

    RaycastHit weldHit;

    public bool CanSpawnWeld(float resolution)
    {
        weldHit = GetPositionRaycastVR(rayReference , maxHitDistance).Value;

        Vector3 spawnPoint = weldHit.point;
        string tag = weldHit.collider.tag;

        if (spawnPoint == Vector3.zero) return false; 

        if (currentBead == null)
        {
            return true;
        }
        if (tag == "weldable" || tag == "weldPoint") 
        {
            return Vector3.Distance(currentBead.transform.position, spawnPoint) > resolution;
        }
        return false; 
    }

    public void SpawnWeld(Vector3 spawnPoint)
    {
        currentBead = Instantiate(weldBeadPrefab, spawnPoint, Quaternion.identity);
    }

    public RaycastHit? GetPositionRaycastVR(Transform rayReference, float maxHitDistance)
    {
        RaycastHit hit;
        if (Physics.Raycast(rayReference.position, rayReference.forward, out hit))
        {
            Debug.DrawRay(rayReference.position, rayReference.forward, Color.green);
            return hit;
        }
        return hit;
    }
}
