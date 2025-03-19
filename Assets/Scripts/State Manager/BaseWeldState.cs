using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

}
