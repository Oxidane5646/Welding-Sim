using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActions;
    private InputAction weld;
    private InputAction equip;

    public event Action OnWeldPressed;
    public event Action OnEquipKeyPressed;

    private void Awake()
    {
        weld = inputActions.FindActionMap("XRI RightHand Interaction").FindAction("weld");
        equip = inputActions.FindActionMap("XRI RightHand Interaction").FindAction("equip");
        weld.Enable();
        equip.Enable();
        equip.performed += OnEquipPressed;
    }
    
    private void OnEquipPressed(InputAction.CallbackContext context)
    {
        OnEquipKeyPressed?.Invoke();
    }

    private void OnDestroy()
    {
        weld.Disable();
        equip.Disable();
    }


    private void Update()
    {
        WeldCheck();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void WeldCheck()
    {
        if (weld == null) return;

        if( weld.ReadValue<float>() > 0.1f)
        {
            OnWeldPressed?.Invoke();
        }
    }
    
    

}
