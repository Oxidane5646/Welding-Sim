using UI_Scripts;
using UnityEngine;

public class GameManager : MonoBehaviour
{
     
    [SerializeField] private UIManager uiManager; 
    [SerializeField] private WeldObjectsInitializer weldObjectsInitializer;
    [SerializeField] private ExperimentMenuCanvas experimentMenu;
    [SerializeField] private MainMenuScript mainMenu;
    [SerializeField] private InstructionsManager instructionsManager;

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
        experimentMenu.OnWeldSelectionComplete += weldObjectsInitializer.InitializeWeldData;
        mainMenu.OnGameModeSelected += HandleGameModeSelection;

    }

    private void DisableEventListenrs()
    {
        //Note :- check if these need null checks 
        if (!experimentMenu) { return; }
        if (!mainMenu) { return; }
    
        //Note:- Onweldselectioncomplete only need to be subscribed to when GameMode.experiment is selected
        experimentMenu.OnWeldSelectionComplete -= weldObjectsInitializer.InitializeWeldData;

        mainMenu.OnGameModeSelected -= HandleGameModeSelection;
    }
    

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
            //instructionsManager.gameObject.SetActive(true);
        }

        else if (gameMode == GameMode.Tutorial)
        {
            weldObjectsInitializer.InitializeWeldData(WeldObjectType.Basic, WeldSetupType.Basic);
        }
    }
    
}
