using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WeldManager : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private WeldSpawner weldSpawner;
    [SerializeField] private JoinPlates joinPlates;
    [SerializeField] private FillerRodCollision fillerRodCollision;
    [SerializeField] private WeldDatabase weldDatabase;

    [SerializeField] private Camera cam;
    [SerializeField] private float resolution;
    [SerializeField] private float maxWeldDistance;
    [SerializeField] private Transform rayReference;

    private RaycastHit hit;
    private Vector3 spawnPoint;

    private void Start()
    {
        

        // Ensure InputManager is assigned
        if (inputManager == null)
        {
            Debug.LogError("InputManager is not assigned in WeldManager!");
            return;
        }
    }

    private void Update()
    {

    }

    private void NormalVRTesting()
    {
        (spawnPoint, hit) = inputManager.GetPositionRaycastVR(rayReference, maxWeldDistance);
        checkingNull(spawnPoint, hit, resolution);

        if (hit.collider == null || hit.transform == null) return;

        bool canSpawnWeld = weldSpawner.CanSpawnWeld(resolution, spawnPoint, hit.transform.tag);

        //if (inputManager.isWeldingVR() && canSpawnWeld)
        {
            weldSpawner.SpawnWeld(spawnPoint);
        }

        joinPlates.UpdateWeldPoints(hit);

        if (spawnPoint == Vector3.zero) return;

        if (joinPlates.CanJoinPlates())
        {
            joinPlates.ConnectPlates();
        }
    }

    private void checkingNull(Vector3 spawnPoint, RaycastHit hit1, float resolution)
    {
        if (spawnPoint == Vector3.zero)
        {
            Debug.LogWarning("SpawnPoint is zero (not valid).");
        }
        if (hit1.collider == null)
        {
            Debug.LogWarning("Raycast hit nothing.");
        }
        if (resolution == 0)
        {
            Debug.LogWarning("Resolution is zero, check your settings.");
        }
    }

}


