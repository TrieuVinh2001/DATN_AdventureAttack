using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject shopPanel;

    public void CloseShop()
    {
        shopPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void ClosePause()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Continue()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Play()
    {
        SceneManager.LoadScene("Main");
    }

    public void Setting()
    {

    }

    public void Exit()
    {
        Application.Quit();
    }
}
