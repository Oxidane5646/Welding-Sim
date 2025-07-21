using System;
using UnityEngine;

namespace UI_Scripts
{
    public class InstructionManager : MonoBehaviour
    {
        [Serializable]
        public class InstructionPanel
        {
            public GameObject panel;
            bool isCompleted = false;
        }

        [SerializeField] InstructionPanel[] instructionPanels;
    }
}
