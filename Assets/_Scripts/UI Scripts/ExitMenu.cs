using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitMenu : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    [SerializeField] private Button restartButton;

    private void OnEnable()
    {
        exitButton.onClick.AddListener(ExitButtonFunction);
        restartButton.onClick.AddListener(RestartButtonFunction);
    }

    private void RestartButtonFunction()
    {
        // Get the current scene name and reload it
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    private void ExitButtonFunction()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
