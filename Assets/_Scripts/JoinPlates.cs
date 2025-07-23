using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinPlates : MonoBehaviour
{
    /*Note:
     * Make this class more effecient and make dynamic adding of weldpoints 
     * implement the fixed joint system properly
     * This class needs the reference of the weldspawner to make it self sufficient and subcribe to the onweldraycasthit event action
     */

    private bool isJoined = false;

    public event Action<float> OnCompletionPercentageChanged;

    private void OnEnable()
    {
       
    }

    private void JoinPlatesFunction()
    {

    }

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
    [SerializeField] Rigidbody[] objectsToConnect;

    WeldPoint[] weldPoints;

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
        if (hit.collider == null || hit.transform == null) return; // null reference check for the raycasthit

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
        if (isJoined) { return; }

        Debug.Log("Plates are connected");

        foreach (Rigidbody obj in objectsToConnect)
        {
            FixedJoint joint = obj.gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = centralBody;
        }

        isJoined = true;
    }

    public float CompletionPercentage()
    {
        if( weldPoints.Length == 0) return 0.0f;
        
        int hitcount = 0;
        foreach (WeldPoint weldPoint in weldPoints)
        {
            if (weldPoint.isHit) hitcount++;
        }

        return (float)hitcount / weldPoints.Length * 100f;
    }

}