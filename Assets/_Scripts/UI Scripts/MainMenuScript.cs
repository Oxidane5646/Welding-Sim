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
        [SerializeField] private Button exitButton;

        private void Start()
        {
            InitializeButtonFunction();
        }

        private void InitializeButtonFunction()
        {
            exprimentCanvasButton.onClick.AddListener(() => OnExperimentButtonSelected());
            tutorialCanvasButton.onClick.AddListener(() => OnTutorialButtonSelected());
            exitButton.onClick.AddListener(() => OnExitButtonSelected());
        }

        private void OnExperimentButtonSelected()
        {
            exprimentCanvas.GameObject().SetActive(true);
            this.gameObject.SetActive(false);
        }

        private void OnTutorialButtonSelected()
        {
            tutorialCanvas.GameObject().SetActive(true);
            this.gameObject.SetActive(false);
        }

        private void OnExitButtonSelected()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}
