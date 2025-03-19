using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WearPPEState : BaseWeldState ,Isg
{
    private GameObject HelmetSocket;
    private GameObject GlovesSocket;

    private XRSocketInteractor HelmetSocketInteractor;
    private XRSocketInteractor GlovesSocketInteractor;

    public WearPPEState(GameObject panel) : base(panel)
    {

    }

    public override void EnterState(WeldStateManager stateManager)
    {
        panel.SetActive(true);

        HelmetSocketInteractor = HelmetSocket.GetComponent<XRSocketInteractor>();
        GlovesSocketInteractor = GlovesSocket.GetComponent<XRSocketInteractor>();



    }

    public override void ExitState(WeldStateManager stateManager)
    {
        panel.SetActive(false);
    }

    public override void UpdateState(WeldStateManager stateManager)
    {
        
    }
}
