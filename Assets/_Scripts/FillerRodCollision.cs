using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This class is only needed for TIG welding, where the filler rod needs to detect collisions with weldable objects.
public class FillerRodCollision : MonoBehaviour
{
    bool isColliding = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("weldable"))
        {
            isColliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("weldable"))
        {
            isColliding = false;
        }
    }

    public bool GetIsColliding()
    {
        return isColliding; 
    }
}
