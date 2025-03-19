using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeldData", menuName = "Weld Sim / WeldData", order = 0)]
public class WeldDatabase : ScriptableObject
{
    [Serializable]
    public class WeldObject
    {
        public weldObjectType weldObjectType;
        public GameObject prefab;
    }

    [Serializable]

    public class WeldSetup
    {
        public weldSetupType weldSetupType;
        public GameObject prefab;
    }

    [SerializeField] List<WeldObject> weldObjects = new List<WeldObject>();
    [SerializeField] List<WeldSetup> weldSetups = new List<WeldSetup>();

    public GameObject GetWeldObject(weldObjectType weldObjectType)
    {
        foreach (WeldObject weldObject in weldObjects)
        {
            if (weldObject.weldObjectType == weldObjectType)
            {
                return weldObject.prefab;
            }  
        }
        Debug.Log("WeldObject not found");
        return null;
    }

    public GameObject GetWeldSetup(weldSetupType weldSetupType)
    {
        foreach (WeldSetup weldSetup in weldSetups)
        {
            if (weldSetup.weldSetupType == weldSetupType)
            {
                return weldSetup.prefab;
            }
        }
        Debug.Log("WeldSetup not found");
        return null;
    }   
}
