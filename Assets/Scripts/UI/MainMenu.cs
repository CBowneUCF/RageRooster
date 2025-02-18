using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MenuSingleton<MainMenu>
{
    public GameObject optionsMenuPanel;
    public GameObject creditsPanel;
    public EventReference musicRef;
    //public RectTransform creditsContent;

    private void Start()
    {
        optionsMenuPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    public void PlayGame()
    {
        Gameplay.BeginMainMenu(0);
        Close();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowCredits()
    {
        //creditsContent.anchoredPosition = new Vector2(creditsContent.anchoredPosition.x, 0);
        creditsPanel.SetActive(true);
    }

    public void HideCredits()
    {
        creditsPanel.SetActive(false);
    }
}
