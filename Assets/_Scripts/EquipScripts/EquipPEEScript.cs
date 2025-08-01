using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EquipPEEScript : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Transform raycastPoint;
    [SerializeField] private float raycastDistance = 2f; // Made raycast distance configurable
    [SerializeField] private XRDirectInteractor directInteractor;
 

    // Use a dictionary to manage equipment state and associated tags
    private SerializableDictionary<string, bool> equippedState = new SerializableDictionary<string, bool>
    {
        {"WeldMask", false},
        {"WeldGloves", false},
        {"WeldApron", false}
    };

    // Event to notify when a specific item is equipped
    public event Action<string> OnItemEquipped;

    private bool isEquipKeyPressed;

    private void Awake()
    {
        if (inputManager == null)
        {
            Debug.LogError("InputManager not assigned to EquipPEEScript.", this);
            enabled = false; // Disable the script if InputManager is missing
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
        isEquipKeyPressed = true;
    }

    private void Update()
    {
        // Only attempt to equip if the key was pressed
        if (isEquipKeyPressed)
        {
            AttemptEquip();
            isEquipKeyPressed = false; // Reset the key press after attempting to equip
        }
    }

    private void AttemptEquip()
    {
        // Check if all essential items are already equipped (can be customized)
        if (equippedState["WeldMask"] && equippedState["WeldGloves"] && equippedState["WeldApron"])
        {
            return; // All required items equipped, no need to proceed
        }

        if (Physics.Raycast(raycastPoint.position, raycastPoint.forward, out RaycastHit hit, raycastDistance))
        {
            string hitTag = hit.collider.tag;

            // Check if the hit object is a recognizable equipment item and not already equipped
            if (equippedState.ContainsKey(hitTag) && !equippedState[hitTag])
            {
                EquipItem(hit.transform.gameObject, hitTag);
            }
        }
    }

    private void EquipItem(GameObject itemObject, string itemTag)
    {
        equippedState[itemTag] = true;
        Debug.Log($"{itemTag} equipped!");
        Destroy(itemObject);
        OnItemEquipped?.Invoke(itemTag); // Notify listeners
    }

    /// <summary>
    /// Checks if a specific item is equipped.
    /// </summary>
    /// <param name="itemTag">The tag of the item to check.</param>
    /// <returns>True if the item is equipped, false otherwise.</returns>
    public bool IsItemEquipped(string itemTag)
    {
        return equippedState.ContainsKey(itemTag) && equippedState[itemTag];
    }

    public Dictionary<string, bool> GetEquippedState()
    {
        Dictionary<string, bool> equippedItems = new Dictionary<string, bool>();
        return equippedItems;
    }

    public bool IsWeldTorchEquipped()
    {
        return false;
    }
}