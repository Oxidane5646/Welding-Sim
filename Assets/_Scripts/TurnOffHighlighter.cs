using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TurnOffHighlighter : MonoBehaviour
{
     XRSocketInteractor socketInteractor;

    private void Start()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();
        socketInteractor.selectEntered.AddListener(OnSelectEntered);
        socketInteractor.selectExited.AddListener(OnSelectExited);
    }

    void OnSelectEntered(SelectEnterEventArgs args)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    void OnSelectExited(SelectExitEventArgs args)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
    }
}
