using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdatePpeCanvasScript : MonoBehaviour
{
    [SerializeField] private EquipPEEScript equipPpeScript;
    
    [SerializeField] private TextMeshProUGUI weldMaskText;
    [SerializeField] private TextMeshProUGUI weldGlovesText;
    [SerializeField] private TextMeshProUGUI weldApronText;

    // This will be your runtime dictionary. We populate this in Awake.
    private Dictionary<string, TextMeshProUGUI> ppeTextIndicatorsRuntime;

    [SerializeField] private string equippedCharacter = "Y";
    [SerializeField] private string notEquippedCharacter = "N";

    private void Awake()
    {
        if (equipPpeScript == null)
        {
            Debug.LogError("EquipPEEScript not assigned to UpdatePpeCanvasScript.", this);
            enabled = false;
            return;
        }

        // Initialize the runtime dictionary using the manually assigned Inspector fields.
        ppeTextIndicatorsRuntime = new Dictionary<string, TextMeshProUGUI>
        {
            {"WeldMask", weldMaskText},
            {"WeldGloves", weldGlovesText},
            {"WeldApron", weldApronText}
            // Add any other PPE items here if you extend your system
        };

        equipPpeScript.OnItemEquipped += HandleOnItemEquipped;

        // Set initial UI state based on current equipment
        InitializePpeText();
    }

    private void OnDestroy()
    {
        if (equipPpeScript != null)
        {
            equipPpeScript.OnItemEquipped -= HandleOnItemEquipped;
        }
    }

    private void HandleOnItemEquipped(string itemTag)
    {
        // Use the runtime dictionary to update the correct text element
        if (ppeTextIndicatorsRuntime.TryGetValue(itemTag, out TextMeshProUGUI textComponent))
        {
            textComponent.text = equippedCharacter;
            Debug.Log($"UI: {itemTag} text updated to '{equippedCharacter}'.");
        }
        else
        {
            Debug.LogWarning($"UI: No TextMeshProUGUI mapping found for item tag: {itemTag}.");
        }
    }

    /// <summary>
    /// Initializes the text indicators based on the current PPE equipped state.
    /// This should be called once in Awake or OnEnable.
    /// </summary>
    private void InitializePpeText()
    {
        // Iterate through all known PPE items in our runtime dictionary
        foreach (var entry in ppeTextIndicatorsRuntime)
        {
            string itemTag = entry.Key;
            TextMeshProUGUI textComponent = entry.Value;

            // Query the EquipPEEScript for the current state of each item
            if (equipPpeScript.IsItemEquipped(itemTag))
            {
                textComponent.text = equippedCharacter;
            }
            else
            {
                textComponent.text = notEquippedCharacter;
            }
        }
    }
}