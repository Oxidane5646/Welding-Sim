using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

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
