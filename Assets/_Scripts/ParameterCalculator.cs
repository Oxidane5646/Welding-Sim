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
    }

    public void GetParameters(out float distance, out float angle, out float speed)
    {
        distance = currentDistance;
        angle = currentAngle;
        speed = currentSpeed;
    }
}
