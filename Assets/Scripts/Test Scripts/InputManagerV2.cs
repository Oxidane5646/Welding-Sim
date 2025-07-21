using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Test_Scripts
{
    public class InputManagerV2 : MonoBehaviour
    {

        [Header("Input Configuration")]
        [SerializeField] private InputActionAsset inputActions;
        [SerializeField] private string actionMapName = "XRI RightHand Interaction";
        [SerializeField] private string weldActionName = "weld";
        [SerializeField] private float vrInputThreshold = 0.1f;

        // Input Actions
        private InputAction vrWeldAction;
        private InputAction mouseWeldAction;

        //Events 
        public static event Action OnWeldStarted;
        public static event Action OnWeldStopped;
        public static event Action<bool> OnWeldStateChanged;

        private bool isCurrentlyWelding = false;

        #region Unity LifeCycle

        private void Awake()
        {
            InitializeInputActions();
        }

        private void OnEnable()
        {
            EnableInputActions();
        }

        private void OnDisable()
        {
            DiaableInputActions();
        }

        private void OnDestroy()
        {
            CleanUpInputActions();
        }

        #endregion

        #region Input Initialization

        private void InitializeInputActions()
        {
            if (inputActions == null)
            {
                Debug.LogError($"[{nameof(InputManagerV2)}] InpuAction Asset is not assigned!");
                return;
            }

            InputActionMap actionMap = inputActions.FindActionMap(actionMapName);

            if (actionMap == null)
            {
                Debug.LogError($"[{nameof(InputManagerV2)}] Action Map '{actionMapName}' not found in Input Action Asset!");
                return;
            }

            vrWeldAction = actionMap.FindAction(weldActionName);

            if (vrWeldAction == null)
            {
                Debug.LogError($"[{nameof(InputManagerV2)}] Weld Action '{weldActionName}' not found in Action Map '{actionMapName}'!");
                return;
            }

            mouseWeldAction = new InputAction("MouseWeld", InputActionType.Button, "<Mouse>/leftButton");

            vrWeldAction.performed += OnWeldActionPerformed;
            vrWeldAction.canceled += OnWeldActionCanceled;

            mouseWeldAction.performed += OnWeldActionPerformed;
            mouseWeldAction.performed += OnWeldActionCanceled;
        }



        private void EnableInputActions()
        {
            vrWeldAction?.Enable();
            mouseWeldAction?.Enable();
        }
        private void DiaableInputActions()
        {
            vrWeldAction?.Disable();
            mouseWeldAction?.Disable();
        }
        private void CleanUpInputActions()
        {
            if (vrWeldAction != null)
            {
                vrWeldAction.performed -= OnWeldActionPerformed;
                vrWeldAction.canceled -= OnWeldActionCanceled;
                vrWeldAction.Dispose();
            }

            if (mouseWeldAction != null)
            {
                mouseWeldAction.performed -= OnWeldActionPerformed;
                mouseWeldAction.canceled -= OnWeldActionCanceled;
                mouseWeldAction.Dispose();
            }
        }

        #endregion

        #region Input Event Handlers

        private void OnWeldActionPerformed(InputAction.CallbackContext context)
        {
            if (context.action == vrWeldAction)
            {
                float value = context.ReadValue<float>();
                if (value > vrInputThreshold)
                {
                    StartWelding();
                }
            }
            else if (context.action == mouseWeldAction)
            {
                StartWelding();
            }
        }

        private void OnWeldActionCanceled(InputAction.CallbackContext context)
        {
            StopWelding();
        }

        #endregion

        #region Welding State Management

        private void StartWelding()
        {
            if (isCurrentlyWelding) return;
            isCurrentlyWelding = true;
            OnWeldStarted?.Invoke();
            OnWeldStateChanged?.Invoke(true);

            Debug.Log("[Input Manager] Welding started.");
        }

        private void StopWelding()
        {
            if (isCurrentlyWelding)
            {
                isCurrentlyWelding = false;
                OnWeldStopped?.Invoke();
                OnWeldStateChanged?.Invoke(false);

                Debug.Log("[Input Manager] Welding stopped.");
            }
        }

        #endregion

        #region Public API

        ///<summary>
        ///Returns the current welding state
        ///</summary>

        public bool isWelding => isCurrentlyWelding;

        ///<summary>
        ///Manually Start welding 
        ///</summary>

        public void ForceStartWelding()
        {
            StartWelding();
        }

        ///<summary>
        ///Manually stop welding
        ///</summary>

        public void ForceStopWelding()
        {
            StopWelding();
        }

        #endregion
    }
}


