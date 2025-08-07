using System;
using UI_Scripts;
using UnityEngine;

public class GameManager : MonoBehaviour
{
     
    [SerializeField] private UIManager uiManager; // not needed 
    [SerializeField] private WeldObjectsInitializer weldObjectsInitializer;
    [SerializeField] private ExperimentMenuCanvas experimentMenu;
    [SerializeField] private MainMenuScript mainMenu;
    [SerializeField] private InstructionsManager instructionsManager; // not needed i guess
    [SerializeField] private ParametersUI parametersUI;

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
        if (experimentMenu)
        {
            
        }
    }
    
    private void HandleGameModeSelection(GameMode gameMode)
    {
        if(gameMode == GameMode.Expriment)
        {
            //instructionsManager.gameObject.SetActive(true);
            parametersUI.gameObject.SetActive(true);
        }

        else if (gameMode == GameMode.Tutorial)
        {
            weldObjectsInitializer.InitializeWeldData(WeldObjectType.Basic, WeldSetupType.Basic);
            parametersUI.gameObject.SetActive(true);
        }
    }
    #region Parameter Data transfer stupid code
    
    //worst code i have ever written in my life 
    private void SetParameters(ParameterCalculator parameterCalculator)
    {
        parameterCalculator.GetParameters(out float distance, out float  angle, out float  speed);
        parametersUI.Setparameters(distance, angle, speed);
    }

    private void Update()
    {
        ParameterCalculator parameterCalculator =  weldObjectsInitializer.GetParametersCalculator();
        if (parameterCalculator)
        {
            SetParameters(parameterCalculator);
        }
    }

    #endregion
}
