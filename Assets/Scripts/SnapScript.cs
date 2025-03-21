using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapScript : MonoBehaviour
{
    [SerializeField] Transform SnapTransform;
    [SerializeField] Collider SnapCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other == SnapCollider)
        {
            transform.position = SnapTransform.position;
            transform.rotation = SnapTransform.rotation;
        }
    }
}
