using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InsertElectrodeState : BaseWeldState
{
    private GameObject electrode;
    private GameObject weldTorchHead;
    private XRSocketInteractor socketInteractor;

    public bool electrodeInserted = false;

    public InsertElectrodeState(GameObject panel , GameObject electrode, GameObject weldTorchHead) : base(panel)
    {
        this.electrode = electrode;
        this.weldTorchHead = weldTorchHead;
    }

    public override void EnterState(WeldStateManager stateManager)
    {
        panel.SetActive(true);

        socketInteractor = weldTorchHead.GetComponent<XRSocketInteractor>();
        socketInteractor.selectEntered.AddListener(OnSelectEntered);
        socketInteractor.selectExited.AddListener(OnSelectExited);

    }

    public override void ExitState(WeldStateManager stateManager)
    {
        panel.SetActive(false);

        socketInteractor.selectEntered.RemoveListener(OnSelectEntered);
        socketInteractor.selectExited.RemoveListener(OnSelectExited);
    }

    public override void UpdateState(WeldStateManager stateManager)
    {
        bool objectsAreGrabbed = stateManager.grabObjectsState.objectsAreGrabbed;

        if (electrodeInserted)
        {
            stateManager.TransitionToState(stateManager.groundClampState);
        }

        else if (!objectsAreGrabbed)
        {
           stateManager.TransitionToState(stateManager.grabObjectsState);
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if(args.interactableObject.transform.gameObject == electrode)
        {
            electrodeInserted = true;
        }
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        if (args.interactableObject.transform.gameObject == electrode)
        {
            electrodeInserted = false;
        }
    }
}
