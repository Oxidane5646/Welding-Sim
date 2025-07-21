using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Test_Scripts
{
    public class JoinObjects : MonoBehaviour
    {
        [SerializeField] List<Rigidbody> connectBodies;
        [SerializeField] Rigidbody centralBody;

        [SerializeField] InputActionAsset inputActions;
        private InputAction weld;

        private bool isConnected = false;

        private void Awake()
        {
            weld = inputActions.FindActionMap("XRI RightHand Interaction").FindAction("weld");
        }

        private void OnEnable()
        {
            weld?.Enable();
        }

        private void OnDisable()
        {
            weld?.Disable();
        }

        private void Update()
        {
            WeldCheck();
        }

        private void WeldCheck()
        {
            if (weld == null) return;

            if (weld.ReadValue<float>() > 0.1f)
            {
                Connect();
            }
        }

        private void Connect()
        {
            if (isConnected) return;

            foreach (Rigidbody obj in connectBodies)
            {
                if (obj.GetComponent<FixedJoint>() == null)
                {
                    FixedJoint joint = obj.AddComponent<FixedJoint>();
                    joint.connectedBody = centralBody;
                }
            }

            isConnected = true;
        }
    }
}
