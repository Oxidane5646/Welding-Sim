using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Button parallelButton;
    [SerializeField] Button lJointButton;
    [SerializeField] Button tJointButton;

    public static event Action<weldObjectType> onWeldObjectSelected;

    private void Start()
    {
        parallelButton.onClick.AddListener(() => SelectWeldObjectType(weldObjectType.parallelJoint));
        lJointButton.onClick.AddListener(() => SelectWeldObjectType(weldObjectType.lJoint));
        tJointButton.onClick.AddListener(() => SelectWeldObjectType(weldObjectType.tJoint));
    }

    private void SelectWeldObjectType(weldObjectType weldObjectType)
    {
        onWeldObjectSelected.Invoke(weldObjectType);
    }
}
