using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeldStateManager : MonoBehaviour
{
    private BaseWeldState currentState;

    //GameObject References and other references for the context 
    GameObject electrode;
    GameObject weldTorchBody;
    GameObject weldTorchHead;


    // UI panel references 
    [Header("UI Panels")]
    public GameObject grabstatePanel;
    public GameObject electrodestatePanel;
    public GameObject groundClampstatePanel;

    // State instances Declaration
    public GrabObjectsState grabObjectsState;
    public InsertElectrodeState insertElectrodeState;
    public GroundClampState groundClampState;

    private void Start()
    {
        // Initialize the state instances   
        grabObjectsState = new GrabObjectsState(grabstatePanel , weldTorchBody , electrode);
        insertElectrodeState = new InsertElectrodeState(electrodestatePanel , electrode , weldTorchHead);
        groundClampState = new GroundClampState(groundClampstatePanel);

        // Set the default state
        currentState = grabObjectsState;
        currentState.EnterState(this);
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    public void TransitionToState(BaseWeldState newState)
    {
        currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }


}
