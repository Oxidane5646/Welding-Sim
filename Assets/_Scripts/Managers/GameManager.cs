using System.Collections;
using System.Collections.Generic;
using UI_Scripts;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] WeldDatabase weldDatabase;
    [SerializeField] UIManager uiManager; 
    [SerializeField] newWeldManager newWeldManager;
    [SerializeField] private ExperimentMenuCanvas experimentMenu;
    
    [Header("Spawn Points")]
    [SerializeField] Transform weldObjectSpawn;
    [SerializeField] Transform weldSetupSpawn;

    private WeldSpawner currentWeldSpawner;
    private JoinPlates currentJoinPlates;
    private ParticleSystem weldParticles;
    
    private void Awake()
    {
        if (experimentMenu != null)
        {
            experimentMenu.OnOnWeldSelectionComplete += InitializeWeldData;
        }
    }
    
    private void InitializeWeldData(WeldObjectType weldObjectType, WeldSetupType weldSetupType)
    {
        if (weldDatabase == null) return;

        // Destroy old objects before spawning new ones
        DestroyExistingObjects();

        // Spawn and configure the weld object
        GameObject currentWeldObject = BuildObject(weldObjectType);
        currentJoinPlates = currentWeldObject?.GetComponent<JoinPlates>();

        // Spawn and configure the weld setup
        GameObject currentWeldSetup = BuildSetup(weldSetupType);
        
        if (currentWeldSetup != null)
        {
            currentWeldSpawner = currentWeldSetup.GetComponent<WeldSpawner>();
            weldParticles = currentWeldSetup.GetComponentInChildren<ParticleSystem>();
        }
        
        newWeldManager.SetScriptReferences(currentWeldSpawner, currentJoinPlates , weldParticles);
    }
    
    private GameObject BuildObject(WeldObjectType weldObjectType)
    {
        GameObject weldObjectPrefab = weldDatabase?.GetWeldObject(weldObjectType);
        return weldObjectPrefab != null ? Instantiate(weldObjectPrefab, weldObjectSpawn.position, Quaternion.identity) : null;
    }

    private GameObject BuildSetup(WeldSetupType weldSetupType)
    {
        GameObject weldSetupPrefab = weldDatabase?.GetWeldSetup(weldSetupType);
        return weldSetupPrefab != null ? Instantiate(weldSetupPrefab, weldSetupSpawn.position, Quaternion.identity) : null;
    }

    private void DestroyExistingObjects()
    {
        if (currentWeldSpawner != null) Destroy(currentWeldSpawner.gameObject);
        if (currentJoinPlates != null) Destroy(currentJoinPlates.gameObject);
    }
    
    private void OnDestroy()
    {
        if (experimentMenu != null)
        {
            experimentMenu.OnOnWeldSelectionComplete -= InitializeWeldData;
        }
    }
    
    
}
