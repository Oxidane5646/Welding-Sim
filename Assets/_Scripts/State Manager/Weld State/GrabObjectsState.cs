using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace State_Manager.Weld_State
{
    public class GrabObjectsState : BaseWeldState
    {
        private GameObject weldTorchBody;
        private GameObject electrode;

        public bool objectsAreGrabbed = false;

        public GrabObjectsState(GameObject panel , GameObject weldTorchBody , GameObject electrode) : base(panel)
        {
            this.weldTorchBody = weldTorchBody;
            this.electrode = electrode;
        }

        public override void EnterState(WeldStateManager stateManager) // what happens when we enter the state
        {
            panel.SetActive(true);
        }

        public override void ExitState(WeldStateManager stateManager) // what happens when we exit the state
        {
            panel.SetActive(false);
        }

        public override void UpdateState(WeldStateManager stateManager) // what happend when we are in the state
        {
            objectsAreGrabbed = ObejctsAreGrabbed();

            if (objectsAreGrabbed)
            {
                stateManager.TransitionToState(stateManager.insertElectrodeState);
            }
        }

        public bool ObejctsAreGrabbed()
        {
            bool weldTorchGrabbed = weldTorchBody.GetComponent<XRGrabInteractable>().isSelected;
            bool electrodeGrabbed = electrode.GetComponent<XRGrabInteractable>().isSelected;

            if (weldTorchGrabbed && electrodeGrabbed)
            {
                return true;
            }
            return  false;
        }

        public bool GetObjectsAreGrabbed()
        {
            return objectsAreGrabbed;
        }



    }
}
