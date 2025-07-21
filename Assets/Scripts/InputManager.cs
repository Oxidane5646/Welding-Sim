using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActions;
    private InputAction weld;

    public event Action OnWeldPressed;
    public event Action OnEquipKeyPressed;

    private void Awake()
    {
        weld = inputActions.FindActionMap("XRI RightHand Interaction").FindAction("weld");
        weld.Enable();
    }

    private void OnDestroy()
    {
        weld.Disable();
    }


    private void Update()
    {
        WeldCheck();
    }

    public void WeldCheck()
    {
        if (weld == null) return;

        if( weld.ReadValue<float>() > 0.1f)
        {
            OnWeldPressed?.Invoke();
        }
    }

}
