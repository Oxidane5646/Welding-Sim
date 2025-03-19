using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public abstract class BaseWeldState 
{
    protected GameObject panel;
    public BaseWeldState(GameObject panel)
    {
        this.panel = panel;
    }

    public abstract void EnterState(WeldStateManager stateManager);
    public abstract void ExitState(WeldStateManager stateManager);
    public abstract void UpdateState(WeldStateManager stateManager);

    public virtual void OnSelectEntered(SelectEnterEventArgs args)
    {
        
    }

}
