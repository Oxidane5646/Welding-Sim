using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI_Scripts
{
    public class ExperimentMenuCanvas : MonoBehaviour
    {
        [Header("WeldObject Buttons")]
        [SerializeField] private Button parallelButton;
        [SerializeField] private Button lJointButton;
        [SerializeField] private Button tJointButton;

        [FormerlySerializedAs("MigButton")]
        [Header("WeldSetup Buttons")]
        [SerializeField] private Button migButton;
        [SerializeField] private Button tigButton;
        [SerializeField] private Button stickButton;

        [Header("Panels")]
        [SerializeField] private GameObject weldObjectPanel;
        [SerializeField] private GameObject weldSetupPanel;

        private WeldObjectType selectedWeldObjectType;

        // Event now sends both weldObjectType and weldSetupType
        public event Action<WeldObjectType, WeldSetupType> OnOnWeldSelectionComplete;

        private void Start()
        {
            // Map weld object buttons to types
            Dictionary<Button, WeldObjectType> weldObjectButtons = new()
            {
                { parallelButton, WeldObjectType.ParallelJoint },
                { lJointButton, WeldObjectType.LJoint },
                { tJointButton, WeldObjectType.TJoint }
            };

            // Assign listeners dynamically
            foreach (var entry in weldObjectButtons)
            {
                entry.Key.onClick.AddListener(() => SelectWeldObjectType(entry.Value));
            }

            // Map weld setup buttons to types
            Dictionary<Button, WeldSetupType> weldSetupButtons = new()
            {
                { migButton, WeldSetupType.Mig },
                { tigButton, WeldSetupType.Tig },
                { stickButton, WeldSetupType.Stick }
            };

            // Assign listeners dynamically
            foreach (var entry in weldSetupButtons)
            {
                entry.Key.onClick.AddListener(() => SelectWeldSetupType(entry.Value));
            }
        }

        private void SelectWeldObjectType(WeldObjectType weldObjectType)
        {
            selectedWeldObjectType = weldObjectType;
            weldSetupPanel.SetActive(true);
            weldObjectPanel.SetActive(false);
        }

        private void SelectWeldSetupType(WeldSetupType weldSetupType)
        {
            OnOnWeldSelectionComplete?.Invoke(selectedWeldObjectType, weldSetupType);
            weldSetupPanel.SetActive(false);
        }
    }
}

