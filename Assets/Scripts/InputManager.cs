using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActions;
    private InputAction weld;

    public event Action OnWeldPressed;

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

    public (Vector3 , RaycastHit) GetPositionRaycastVR(Transform rayReference , float maxHitDistance)
    {
        RaycastHit hit;
        if (Physics.Raycast(rayReference.position, rayReference.forward, out hit))
        {
            Debug.DrawRay(rayReference.position , rayReference.forward  , Color.green);
            return (hit.point , hit);
        }
        
        return (Vector3.zero, default);
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
