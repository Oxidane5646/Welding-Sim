using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinPlates : MonoBehaviour
{
    /*Note:
     * implement the fixed joint system properly
     */

    private bool isJoined = false;

    public event Action<float> OnCompletionPercentageChanged;

    public class WeldPoint
    {
        public BoxCollider collider;
        public bool isHit = false;

        public WeldPoint(BoxCollider collider)
        {
            this.collider = collider;
        }
    }

    [SerializeField] BoxCollider[] jointColliders;
    [SerializeField] Rigidbody centralBody;
    [SerializeField] GameObject[] objectsToConnect;

    WeldPoint[] weldPoints;

    private WeldSpawner weldSpawner;

    public void SetScriptReferences(WeldSpawner currentWeldSpawner)
    {
        weldSpawner = currentWeldSpawner;
    }

    private void Start()
    {
        weldPoints = BuildWeldPoints(jointColliders);
    }

    private WeldPoint[] BuildWeldPoints(BoxCollider[] colliders)
    {
        WeldPoint[] weldPoints = new WeldPoint[colliders.Length];

        for (int i = 0; i < colliders.Length; i++)
        {
            weldPoints[i] = new WeldPoint(colliders[i]);
        }
        return weldPoints;
    }

    public void UpdateWeldPoints(RaycastHit hit)
    {
        if (!hit.collider || !hit.transform) return; // null reference check for the raycasthit

        if (hit.transform.CompareTag("weldPoint"))
        {

            Debug.Log("Weld Points Updated");

            foreach (WeldPoint weldPoint in weldPoints)
            {
                if (weldPoint.collider == hit.collider)
                {
                    weldPoint.isHit = true;
                }
            }
        }
    }

    // check if the vector3 spawnPoint is in 000 before using this fucntion
    public bool CanJoinPlates()
    {
        foreach (WeldPoint weldPoint in weldPoints)
        {
            if (!weldPoint.isHit) { return false; }
        }
        return true;
    }

    public void ConnectPlates()
    {
        //disabled just for debugging reasons
        //if (isJoined) { return; }

        Debug.Log("Plates are connected");

        foreach (GameObject obj in objectsToConnect)
        {
            FixedJoint joint = obj.AddComponent<FixedJoint>();
            
            joint.connectedBody = centralBody;
        }

        //isJoined = true;
    }

    public float GetCompletionPercentage()
    {
        if( weldPoints.Length == 0) return 0.0f;
        
        int hitcount = 0;
        foreach (WeldPoint weldPoint in weldPoints)
        {
            if (weldPoint.isHit) hitcount++;
        }

        return (float)hitcount / weldPoints.Length * 100f;
    }

    private void JoinWeldObject(RaycastHit hit)
    {
        UpdateWeldPoints(hit);

        if (CanJoinPlates())
        {
            ConnectPlates();
        }
    }

    #region Unity LifeCycle

    private void OnEnable()
    {
        if (!weldSpawner) return;
        weldSpawner.OnWeldRaycastHit += JoinWeldObject;
    }

    private void OnDisable()
    {
        if (!weldSpawner) return;
        weldSpawner.OnWeldRaycastHit -= JoinWeldObject;
    }

    #endregion
    
    #region Debugging

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ConnectPlates();
        }
    }
    
    #endregion
}