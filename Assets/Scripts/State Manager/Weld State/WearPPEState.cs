using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WearPPEState : BaseWeldState
{
    private GameObject HelmetSocket;
    private GameObject GlovesSocket;

    private XRSocketInteractor HelmetSocketInteractor;
    private XRSocketInteractor GlovesSocketInteractor;

    public bool helmetWeared;
    public bool glovesWeared;

    public WearPPEState(GameObject panel) : base(panel)
    {

    }

    public override void EnterState(WeldStateManager stateManager)
    {
        panel.SetActive(true);

        HelmetSocketInteractor = SetupSocketInteractor(HelmetSocket);
        GlovesSocketInteractor = SetupSocketInteractor(panel);
    }

    public override void ExitState(WeldStateManager stateManager)
    {
        panel.SetActive(false);

        RemoveSocketInteractor(HelmetSocketInteractor);
        RemoveSocketInteractor(GlovesSocketInteractor);
    }

    public override void UpdateState(WeldStateManager stateManager)
    {

    }
}
