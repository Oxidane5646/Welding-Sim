using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace State_Manager.Weld_State
{
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
}
