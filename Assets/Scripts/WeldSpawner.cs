using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeldSpawner : MonoBehaviour
{
    [SerializeField] GameObject weldBeadPrefab;
    GameObject currentBead = null;

    public bool CanSpawnWeld(float resolution, Vector3 spawnPoint , string tag)
    {
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
}
