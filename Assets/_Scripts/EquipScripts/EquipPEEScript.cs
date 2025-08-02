using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class EquipPEEScript : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Transform raycastPoint;
    [SerializeField] private float raycastDistance = 2f; // Made raycast distance configurable
    
    [SerializeField] private XRDirectInteractor leftHandInteractor;
    [SerializeField] private XRDirectInteractor rightHandInteractor;

    // Use a dictionary to manage equipment state and associated tags
    private SerializableDictionary<string, bool> equippedState = new SerializableDictionary<string, bool>
    {
        {"WeldMask", false},
        {"WeldGloves", false},
        {"WeldApron", false},
        {"WeldTorch", false},
    };

    // Event to notify when a specific item is equipped
    public event Action<string> OnItemEquipped;

    private void Awake()
    {
        if (!inputManager)
        {
            return;
        }
        inputManager.OnEquipKeyPressed += HandleEquipKeyPressed;
    }

    private void OnDestroy()
    {
        if (inputManager != null)
        {
            inputManager.OnEquipKeyPressed -= HandleEquipKeyPressed;
        }
    }

    private void HandleEquipKeyPressed()
    {
        AttemptEquip();
    }

    private void AttemptEquip()
    {
        // Check if all essential items are already equipped 
        if (equippedState["WeldMask"] && equippedState["WeldGloves"] && equippedState["WeldApron"])
        {
            return; 
        }

        if (Physics.Raycast(raycastPoint.position, raycastPoint.forward, out RaycastHit hit, raycastDistance))
        {
            string hitTag = hit.collider.tag;

            // Check if the hit object is a recognizable equipment item 
            if (equippedState.ContainsKey(hitTag))
            {
                EquipItem(hit.transform.gameObject, hitTag);
            }
        }
    }

    private void EquipItem(GameObject itemObject, string itemTag)
    {
        equippedState[itemTag] = true;
        Destroy(itemObject);
        OnItemEquipped?.Invoke(itemTag); 
    }
    
    
    public bool IsItemEquipped(string itemTag)
    {
        return equippedState.TryGetValue(itemTag, value: out bool equipped) && equipped;
    }

    private void OnEnable()
    {
        InitializeEventListeners();
    }
    
    private void OnDisable()
    {
        DisableEventListeners();
    }

    private void InitializeEventListeners()
    {
        rightHandInteractor.selectEntered.AddListener(OnGrab);
        leftHandInteractor.selectEntered.AddListener(OnGrab);
        
        rightHandInteractor.selectExited.AddListener(OnRelease);
        leftHandInteractor.selectExited.AddListener(OnRelease);
    }
    
    private void DisableEventListeners()
    {
        rightHandInteractor.selectEntered.RemoveListener(OnGrab);
        leftHandInteractor.selectEntered.RemoveListener(OnGrab);
        
        rightHandInteractor.selectExited.RemoveListener(OnRelease);
        leftHandInteractor.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        GameObject grabbedObject = args.interactableObject.transform.gameObject;
        if (grabbedObject.CompareTag("WeldTorch"))
        {
            equippedState["WeldTorch"] = true;
        }
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        GameObject releasedObject = args.interactableObject.transform.gameObject;
        if (releasedObject.CompareTag("WeldTorch"))
        {
            equippedState["WeldTorch"] = false;
        }
    }
    
}