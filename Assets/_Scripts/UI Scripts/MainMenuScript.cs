using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI_Scripts
{
    public class MainMenuScript : MonoBehaviour
    {
        [Header("Canvas References ")]
        [SerializeField] private Canvas exprimentCanvas;
        [SerializeField] private Canvas tutorialCanvas;
        
        [Header("Button References ")]
        [SerializeField] private Button exprimentCanvasButton;
        [SerializeField] private Button tutorialCanvasButton;

        public event Action<GameMode> OnGameModeSelected;

        private void Start()
        {
            InitializeButtonFunction();
        }

        private void InitializeButtonFunction()
        {
            exprimentCanvasButton.onClick.AddListener(() => OnExperimentButtonSelected());
            tutorialCanvasButton.onClick.AddListener(() => OnTutorialButtonSelected());
        }

        private void OnExperimentButtonSelected()
        {
            exprimentCanvas.GameObject().SetActive(true);
            OnGameModeSelected?.Invoke(GameMode.Expriment);
            this.gameObject.SetActive(false);
        }

        private void OnTutorialButtonSelected()
        {
            tutorialCanvas.GameObject().SetActive(true);
            OnGameModeSelected?.Invoke(GameMode.Tutorial);
            this.gameObject.SetActive(false);
        }
        
    }
}
