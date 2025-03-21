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

        socketInteractor = SetupSocketInteractor(weldTorchHead);

    }

    public override void ExitState(WeldStateManager stateManager)
    {
        panel.SetActive(false);

        RemoveSocketInteractor(socketInteractor);
    }

    public override void UpdateState(WeldStateManager stateManager)
    {
        bool objectsAreGrabbed = stateManager.grabObjectsState.GetObjectsAreGrabbed();

        if (CheckIfObjectIsInserted(electrode)) electrodeInserted = true;
        if (CheckIfObjectIsRemoved(electrode)) electrodeInserted = false;

        if (electrodeInserted)
        {
            stateManager.TransitionToState(stateManager.groundClampState);
        }

        else if (!objectsAreGrabbed)
        {
           stateManager.TransitionToState(stateManager.grabObjectsState);
        }
    }

    public bool GetElectrodeInserted()
    {
        return electrodeInserted;
    }   
}
