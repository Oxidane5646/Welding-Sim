using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class InputManager : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActions;
    private InputAction weld;
    private InputAction equip;

    public event Action OnWeldPressed;
    public event Action OnEquipKeyPressed;

    private const float weldThreshold = 0.1f;
    private Coroutine weldCoroutine;

    private void Awake()
    {
        var map = inputActions.FindActionMap("XRI RightHand Interaction");
        weld = map.FindAction("weld");
        equip = map.FindAction("equip");

        weld.Enable();
        equip.Enable();

        equip.performed += OnEquipPressed;
        weld.started += OnWeldStarted;
        weld.canceled += OnWeldCanceled;
    }

    private void OnEquipPressed(InputAction.CallbackContext context)
    {
        OnEquipKeyPressed?.Invoke();
    }

    private void OnWeldStarted(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        if (value > weldThreshold && weldCoroutine == null)
        {
            weldCoroutine = StartCoroutine(WeldHoldRoutine());
        }
    }

    private void OnWeldCanceled(InputAction.CallbackContext context)
    {
        if (weldCoroutine != null)
        {
            StopCoroutine(weldCoroutine);
            weldCoroutine = null;
        }
    }

    private IEnumerator WeldHoldRoutine()
    {
        while (true)
        {
            OnWeldPressed?.Invoke();
            yield return null; // Wait 1 frame
        }
    }

    private void OnDestroy()
    {
        weld.started -= OnWeldStarted;
        weld.canceled -= OnWeldCanceled;
        equip.performed -= OnEquipPressed;

        weld.Disable();
        equip.Disable();
    }
}

