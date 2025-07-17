using System;
using System.Collections.Generic;
using UnityEngine;

public class WeldBeadSpawner : MonoBehaviour
{
    [Header("Weld Bead Settings")]
    [SerializeField] private GameObject weldBeadPrefab;
    [SerializeField] private Transform rayReference;
    [SerializeField] private float maxHitDistance = 10f;
    [SerializeField] private float weldResolution = 0.1f;
    [SerializeField] private Material weldMaterial;
    [SerializeField] private float beadWidth = 0.02f;
    [SerializeField] private float beadHeight = 0.01f;

    // Events for other systems
    public static event Action<Vector3, Vector3> OnWeldPointCreated; // position, normal
    public static event Action<Vector3> OnWeldStarted; // start position
    public static event Action OnWeldStopped;
    public static event Action OnWeldCleared;
    public static event Action<bool> OnWeldValidityChanged; // can weld or not

    // Private variables
    private List<Vector3> weldPoints = new List<Vector3>();
    private LineRenderer weldLineRenderer;
    private RaycastHit currentHit;
    private bool isWelding = false;
    private Vector3 lastWeldPosition;

    #region Unity Lifecycle

    private void Start()
    {
        SetupWeldLineRenderer();
    }

    private void OnEnable()
    {
        InputManagerV2.OnWeldStarted += HandleWeldStarted;
        InputManagerV2.OnWeldStopped += HandleWeldStopped;
    }

    private void OnDisable()
    {
        InputManagerV2.OnWeldStarted -= HandleWeldStarted;
        InputManagerV2.OnWeldStopped -= HandleWeldStopped;
    }

    private void Update()
    {
        if (isWelding)
        {
            ContinueWelding();
        }
    }

    #endregion

    #region Setup

    private void SetupWeldLineRenderer()
    {
        weldLineRenderer = gameObject.AddComponent<LineRenderer>();
        if (weldMaterial != null)
        {
            weldLineRenderer.material = weldMaterial;
        }
        weldLineRenderer.startWidth = beadWidth;
        weldLineRenderer.endWidth = beadWidth;
        weldLineRenderer.positionCount = 0;
        weldLineRenderer.useWorldSpace = true;
    }

    #endregion

    #region Event Handlers

    private void HandleWeldStarted()
    {
        if (CanStartWelding())
        {
            StartWelding();
        }
    }

    private void HandleWeldStopped()
    {
        StopWelding();
    }

    #endregion

    #region Welding Logic

    private bool CanStartWelding()
    {
        RaycastHit? hitResult = GetWeldPosition();
        if (!hitResult.HasValue) return false;

        currentHit = hitResult.Value;
        string surfaceTag = currentHit.collider.tag;

        return surfaceTag == "weldable" || surfaceTag == "weldPoint";
    }

    private void StartWelding()
    {
        if (isWelding) return;

        isWelding = true;
        Vector3 startPosition = currentHit.point;
        lastWeldPosition = startPosition;

        // Add first point
        AddWeldPoint(startPosition, currentHit.normal);

        // Notify other systems
        OnWeldStarted?.Invoke(startPosition);

        Debug.Log("[WeldBeadSpawner] Welding started at position: " + startPosition);
    }

    private void ContinueWelding()
    {
        if (!CanStartWelding()) return;

        Vector3 currentPosition = currentHit.point;

        // Check if we should add a new weld point
        if (Vector3.Distance(lastWeldPosition, currentPosition) >= weldResolution)
        {
            AddWeldPoint(currentPosition, currentHit.normal);
            lastWeldPosition = currentPosition;
        }
    }

    private void StopWelding()
    {
        if (!isWelding) return;

        isWelding = false;
        OnWeldStopped?.Invoke();

        Debug.Log("[WeldBeadSpawner] Welding stopped. Total points: " + weldPoints.Count);
    }

    private void AddWeldPoint(Vector3 position, Vector3 normal)
    {
        // Add to weld points list
        weldPoints.Add(position);

        // Update line renderer
        UpdateWeldLine();

        // Spawn individual bead if prefab is assigned
        if (weldBeadPrefab != null)
        {
            SpawnWeldBead(position, normal);
        }

        // Notify other systems
        OnWeldPointCreated?.Invoke(position, normal);
    }

    private void SpawnWeldBead(Vector3 position, Vector3 normal)
    {
        Quaternion rotation = Quaternion.LookRotation(normal);
        GameObject bead = Instantiate(weldBeadPrefab, position, rotation);
        bead.transform.localScale = new Vector3(beadWidth, beadHeight, beadWidth);
    }

    private void UpdateWeldLine()
    {
        weldLineRenderer.positionCount = weldPoints.Count;
        if (weldPoints.Count > 0)
        {
            weldLineRenderer.SetPositions(weldPoints.ToArray());
        }
    }

    #endregion

    #region Utility Methods

    private RaycastHit? GetWeldPosition()
    {
        if (rayReference == null) return null;

        RaycastHit hit;
        if (Physics.Raycast(rayReference.position, rayReference.forward, out hit, maxHitDistance))
        {
            Debug.DrawRay(rayReference.position, rayReference.forward * hit.distance, Color.green);
            return hit;
        }

        Debug.DrawRay(rayReference.position, rayReference.forward * maxHitDistance, Color.red);
        return null;
    }

    #endregion

    #region Public API

    public void ClearWeld()
    {
        weldPoints.Clear();
        weldLineRenderer.positionCount = 0;

        // Destroy existing weld beads
        GameObject[] weldBeads = GameObject.FindGameObjectsWithTag("WeldBead");
        foreach (GameObject bead in weldBeads)
        {
            Destroy(bead);
        }

        OnWeldCleared?.Invoke();
        Debug.Log("[WeldBeadSpawner] Weld cleared");
    }

    public bool IsWelding => isWelding;
    public int WeldPointCount => weldPoints.Count;
    public Vector3 LastWeldPosition => lastWeldPosition;

    #endregion

    #region Debug

    private void OnDrawGizmos()
    {
        if (rayReference != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(rayReference.position, rayReference.forward * maxHitDistance);
        }

        // Draw weld points
        Gizmos.color = Color.red;
        foreach (Vector3 point in weldPoints)
        {
            Gizmos.DrawSphere(point, 0.01f);
        }
    }

    #endregion
}