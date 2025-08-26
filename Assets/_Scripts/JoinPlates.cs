using System;
using UnityEngine;

public class JoinPlates : MonoBehaviour
{
    /*Note:
     * implement the fixed joint system properly
     */

    private bool isJoined = false;
    private const float JoinThreshold = 60f;

    public event Action<float> OnCompletionPercentageChanged;

    public class WeldPoint
    {
        public  BoxCollider Collider;
        public bool IsHit = false;

        public WeldPoint(BoxCollider collider)
        {
            this.Collider = collider;
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
        weldPoints = new WeldPoint[jointColliders.Length];
        for (int i = 0; i < jointColliders.Length; i++)
        {
            weldPoints[i] = new WeldPoint(jointColliders[i]);
        }
    }

    public void UpdateWeldPoints(RaycastHit hit)
    {
        if (hit.collider == null || !hit.collider.CompareTag("weldPoint"))
            return;

        Debug.Log("Weld Points Updated");

        foreach (WeldPoint weldPoint in weldPoints)
        {
            if (weldPoint.Collider == hit.collider)
            {
                weldPoint.IsHit = true;
                break; // Stop searching after finding the match
            }
        }
    }


    public float GetCompletionPercentage()
    {
        if (weldPoints.Length == 0) return 0f;
        
        int hitCount = 0;
        foreach (WeldPoint weldPoint in weldPoints)
        {
            if (weldPoint.IsHit) hitCount++;
        }

        return (float)hitCount / weldPoints.Length * 100f;
    }

    public bool CanJoinPlates()
    {
        if (weldPoints.Length == 0)
        {
            Debug.Log("No weld points to check.");
            return false;
        }

        float completionPercentage = GetCompletionPercentage();
        bool canJoin = completionPercentage >= JoinThreshold;

        Debug.Log($"Weld Summary: {completionPercentage:0.##}% complete - {(canJoin ? "CAN JOIN" : "CANNOT JOIN")}");

        // Fire the completion percentage event
        OnCompletionPercentageChanged?.Invoke(completionPercentage);

        return canJoin;
    }

    public void ConnectPlates()
    {
        if (isJoined) return;

        Debug.Log("Plates are connected");

        foreach (GameObject obj in objectsToConnect)
        {
            obj.transform.SetParent(centralBody.transform);
        }

        isJoined = true;
    }

    public void TryJoinWeldObject(RaycastHit hit)
    {
        UpdateWeldPoints(hit);

        if (CanJoinPlates())
        {
            ConnectPlates();
        }
    }

    #region Unity LifeCycle

    private void OnDisable()
    {
        if (weldSpawner != null)
        {
            weldSpawner.OnWeldRaycastHit -= TryJoinWeldObject;
        }
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