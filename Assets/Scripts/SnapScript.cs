using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapScript : MonoBehaviour
{
    [SerializeField] Transform SnapTransform;
    [SerializeField] Collider SnapCollider;

    private bool m_InSnapCollider = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other == SnapCollider)
        {
            transform.gameObject.GetComponent<Collider>().enabled = false;
            m_InSnapCollider = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == SnapCollider)
        {
            m_InSnapCollider = false;
            transform.gameObject.GetComponent<Collider>().enabled = true;
        }
    }

    private void Update()
    {
        if (m_InSnapCollider)
        {
            transform.position = SnapTransform.position;
            transform.rotation = SnapTransform.rotation;
        }
    }

    
}
