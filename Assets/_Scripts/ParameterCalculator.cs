using UnityEngine;

public class ParameterCalculator : MonoBehaviour
{
    [SerializeField] Transform rayOrigin;
    [SerializeField] WeldSpawner weldSpawner;
    [SerializeField] Transform speedTransform;
    
    [SerializeField] private float minWeldSpeed = 0.02f; 
    [SerializeField] private float maxWeldSpeed = 0.1f;

    private float currentDistance;
    private float currentAngle;
    private RaycastHit? weldHit;
    
    private void GetAngleAndDistance()
    { 
        Vector3 pointA = rayOrigin.position;
        Vector3 pointB = weldSpawner.GetSpawnPoint();
        
        currentDistance = Vector3.Distance(pointA, pointB);
        currentAngle= Vector3.Angle(rayOrigin.forward,(pointB - pointA).normalized);
    }
    
    public float currentSpeed { get; private set; }
    
    private Vector3 previousPosition;

    void Start()
    {
        previousPosition = speedTransform.position;
    }

    void Update()
    {
        GetAngleAndDistance();
        Vector3 currentPosition = speedTransform.position;
        float speedDistance = Vector3.Distance(currentPosition, previousPosition);
        currentSpeed = speedDistance / Time.deltaTime;

        previousPosition = currentPosition;
        weldHit = weldSpawner.GetPositionRaycastVR();
    }

    public void GetParameters(out float distance, out float angle, out float speed)
    {
        Transform weldTransform = null;
        if (weldHit.HasValue)
        {
            weldTransform = weldHit.Value.transform;
        }
        else
        {
            distance = 0;
            angle = 0;
            speed = 0;
        }
        
        if (weldHit == null || (!weldTransform.CompareTag("weldable") 
                                || !weldTransform.CompareTag("weldPoint") 
                                || !weldTransform.CompareTag("weldBead")))
        {
            distance = 0f;
            angle = 0f;
            speed = 0f;
        }
        else
        {
            distance = currentDistance;
            angle = currentAngle;
            speed = currentSpeed;
        }
    }
}
