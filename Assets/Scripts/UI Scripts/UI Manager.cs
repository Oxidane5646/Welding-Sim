using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("WeldObject Buttons")]
    [SerializeField] Button parallelButton;
    [SerializeField] Button lJointButton;
    [SerializeField] Button tJointButton;


    [Header("WeldSetup Buttons")]
    [SerializeField] Button MigButton;
    [SerializeField] Button TigButton;
    [SerializeField] Button StickButton;

    [Header("Panels")]
    [SerializeField] GameObject weldObjectPanel;
    [SerializeField] GameObject weldSetupPanel;


    public  event Action<weldObjectType> onWeldObjectSelected;
    public  event Action<weldSetupType> onWeldSetupTypeSelected;

    private void Start()
    {
        parallelButton.onClick.AddListener(() => SelectWeldObjectType(weldObjectType.parallelJoint));
        lJointButton.onClick.AddListener(() => SelectWeldObjectType(weldObjectType.lJoint));
        tJointButton.onClick.AddListener(() => SelectWeldObjectType(weldObjectType.tJoint));

        MigButton.onClick.AddListener(() => SelectWeldSetupType(weldSetupType.MIG));
        TigButton.onClick.AddListener(() => SelectWeldSetupType(weldSetupType.TIG));
        StickButton.onClick.AddListener(() => SelectWeldSetupType(weldSetupType.Stick));
    }

    private void SelectWeldObjectType(weldObjectType weldObjectType)
    {
        onWeldObjectSelected?.Invoke(weldObjectType);
        weldSetupPanel.SetActive(true);
        weldObjectPanel.SetActive(false);
    }

    private void SelectWeldSetupType(weldSetupType weldSetupType)
    {
        onWeldSetupTypeSelected?.Invoke(weldSetupType);
        weldSetupPanel.SetActive(false);
    }
}
