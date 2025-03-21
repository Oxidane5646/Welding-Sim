using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

public class GroundClampState : BaseWeldState
{
    private GameObject GroundClampSocket;
    private GameObject GroundClamp;

    private XRSocketInteractor socketInteractor;

    private bool clampInserted;
    
    public GroundClampState(GameObject panel) : base(panel) { }

    public override void EnterState(WeldStateManager stateManager)
    {
        panel.SetActive(true);

        socketInteractor = SetupSocketInteractor(GroundClampSocket);
    }

    public override void ExitState(WeldStateManager stateManager)
    {
        panel.SetActive(false);

        RemoveSocketInteractor(socketInteractor);       

    }

    public override void UpdateState(WeldStateManager stateManager)
    {
        if (CheckIfObjectIsInserted(GroundClamp))
        {
            clampInserted = true;
        }
        if (CheckIfObjectIsRemoved(GroundClamp))
        {
            clampInserted = false;
        }

        if (clampInserted)
        {
            stateManager.TransitionToState(stateManager.wearPPEState);
        }
    }
}
