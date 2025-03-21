using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public abstract class BaseWeldState 
{
    protected GameObject panel;

    private GameObject socketEnteredObject;
    private GameObject socketExitedObject;


    public BaseWeldState(GameObject panel)
    {
        this.panel = panel;
    }

    public abstract void EnterState(WeldStateManager stateManager);
    public abstract void ExitState(WeldStateManager stateManager);
    public abstract void UpdateState(WeldStateManager stateManager);
    
    protected void SetupSocketInteractor(GameObject socket, XRSocketInteractor socketInteractor)
    {
        socketInteractor = socket.GetComponent<XRSocketInteractor>();
        socketInteractor.selectEntered.AddListener(OnSelectEntered);
        socketInteractor.selectExited.AddListener(OnSelectExited);
    }

    protected void RemoveSocketInteractor(XRSocketInteractor socketInteractor)
    {
        socketInteractor.selectEntered.RemoveListener(OnSelectEntered);
        socketInteractor.selectExited.RemoveListener(OnSelectExited);
    }

    protected void OnSelectEntered(SelectEnterEventArgs args)
    {
        socketEnteredObject = args.interactorObject.transform.gameObject;
    }

    protected void OnSelectExited(SelectExitEventArgs args)
    {
        socketExitedObject = args.interactorObject.transform.gameObject;
    }

    protected void CheckIfObjectIsInserted(GameObject objectToCheck, bool objectInserted)
    {
        if (socketEnteredObject == objectToCheck)
        {
            objectInserted = true;
        }
    }

    protected void CheckIfObjectIsRemoved(GameObject objectToCheck, bool objectInserted)
    {
        if (socketExitedObject == objectToCheck)
        {
            objectInserted = false;
        }
    }

}
