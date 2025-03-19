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

        socketInteractor = GroundClampSocket.GetComponent<XRSocketInteractor>();
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
        
        
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactorObject.transform.gameObject == GroundClamp)
        {
            clampInserted = true;
        }
        
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        if (args.interactorObject.transform.gameObject == GroundClamp)
        {
            clampInserted = false;
        }
    }
}
