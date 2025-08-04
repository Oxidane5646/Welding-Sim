using TMPro;
using UnityEngine;

public class ParametersUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI angleText;
    [SerializeField] private TextMeshProUGUI speedText;

    float currentDistance;
    float currentAngle;
    float currentSpeed;
    
    public void Setparameters(float distance, float angle, float speed)
    {
        currentDistance = distance;
        currentAngle = angle;
        currentSpeed = speed;
    }

    public void Update()
    {
        distanceText.text = currentDistance.ToString("F2");
        angleText.text = currentAngle.ToString("F2");
        speedText.text = currentSpeed.ToString("F2");
    }
}
