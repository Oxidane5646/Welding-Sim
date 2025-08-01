using System.Collections;
using System.Collections.Generic;
using UI_Scripts;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] WeldDatabase weldDatabase; // to new script 
    [SerializeField] UIManager uiManager; 
    [SerializeField] newWeldManager newWeldManager; // tp mew script (bad referencing)
    [SerializeField] private ExperimentMenuCanvas experimentMenu;
    [SerializeField] private MainMenuScript mainMenu;
    
    [Header("Spawn Points")]
    [SerializeField] Transform weldObjectSpawn; // to new script 
    [SerializeField] Transform weldSetupSpawn; // to new script 

    private WeldSpawner currentWeldSpawner;
    private JoinPlates currentJoinPlates;
    private ParticleSystem weldParticles;

    private void OnEnable()
    {
        InitializeEventListeners();
    }

    private void OnDisable()
    {
        DisableEventListenrs();
    }

    private void InitializeEventListeners()
    {
        if(experimentMenu == null) { return; }
        if(mainMenu == null) { return; }

        //Note:- Onweldselectioncomplete only need to be subscribed to when GameMode.experiment is selected
        experimentMenu.OnWeldSelectionComplete += InitializeWeldData;
        mainMenu.OnGameModeSelected += HandleGameModeSelection;

    }

    private void DisableEventListenrs()
    {
        //Note :- check if these need null checks 
        if (experimentMenu == null) { return; }
        if (mainMenu == null) { return; }
    
        //Note:- Onweldselectioncomplete only need to be subscribed to when GameMode.experiment is selected
        experimentMenu.OnWeldSelectionComplete -= InitializeWeldData;

        mainMenu.OnGameModeSelected -= HandleGameModeSelection;
    }

    #region Spawn Weld Setup And Object

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

        newWeldManager.SetScriptReferences(currentWeldSpawner, currentJoinPlates, weldParticles);
        currentJoinPlates.SetScriptReferences(currentWeldSpawner);
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

    #endregion



    private void OnDestroy()
    {
        if (experimentMenu != null)
        {
            
        }
    }
    
    private void HandleGameModeSelection(GameMode gameMode)
    {
        if(gameMode == GameMode.Expriment)
        {
            
        }

        else if (gameMode == GameMode.Tutorial)
        {
            InitializeWeldData(WeldObjectType.Basic, WeldSetupType.Basic);
        }
    }
    
}
