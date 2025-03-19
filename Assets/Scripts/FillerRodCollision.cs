using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillerRodCollision : MonoBehaviour
{
    bool isColliding = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "weldable")
        {
            isColliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "weldable")
        {
            isColliding = false;
        }
    }

    public bool GetIsColliding()
    {
        return isColliding; 
    }
}
