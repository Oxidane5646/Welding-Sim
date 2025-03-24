using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("WeldObject Buttons")]
    [SerializeField] private Button parallelButton;
    [SerializeField] private Button lJointButton;
    [SerializeField] private Button tJointButton;

    [Header("WeldSetup Buttons")]
    [SerializeField] private Button MigButton;
    [SerializeField] private Button TigButton;
    [SerializeField] private Button StickButton;

    [Header("Panels")]
    [SerializeField] private GameObject weldObjectPanel;
    [SerializeField] private GameObject weldSetupPanel;

    private weldObjectType selectedWeldObjectType;

    // Event now sends both weldObjectType and weldSetupType
    public event Action<weldObjectType, weldSetupType> onWeldSelectionComplete;

    private void Start()
    {
        // Map weld object buttons to types
        Dictionary<Button, weldObjectType> weldObjectButtons = new()
        {
            { parallelButton, weldObjectType.parallelJoint },
            { lJointButton, weldObjectType.lJoint },
            { tJointButton, weldObjectType.tJoint }
        };

        // Assign listeners dynamically
        foreach (var entry in weldObjectButtons)
        {
            entry.Key.onClick.AddListener(() => SelectWeldObjectType(entry.Value));
        }

        // Map weld setup buttons to types
        Dictionary<Button, weldSetupType> weldSetupButtons = new()
        {
            { MigButton, weldSetupType.MIG },
            { TigButton, weldSetupType.TIG },
            { StickButton, weldSetupType.Stick }
        };

        // Assign listeners dynamically
        foreach (var entry in weldSetupButtons)
        {
            entry.Key.onClick.AddListener(() => SelectWeldSetupType(entry.Value));
        }
    }

    private void SelectWeldObjectType(weldObjectType weldObjectType)
    {
        selectedWeldObjectType = weldObjectType;
        weldSetupPanel.SetActive(true);
        weldObjectPanel.SetActive(false);
    }

    private void SelectWeldSetupType(weldSetupType weldSetupType)
    {
        onWeldSelectionComplete?.Invoke(selectedWeldObjectType, weldSetupType);
        weldSetupPanel.SetActive(false);
    }
}

