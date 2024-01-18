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
    [SerializeField] private GameObject settingPanel;

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

    public void CloseSetting()
    {
        settingPanel.SetActive(false);
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
        pausePanel.SetActive(false);
        GameManager.instance.gameObject.SetActive(false);
        SceneManager.LoadScene("Menu");
    }

    public void Play()
    {
        if (GameManager.instance)
        {
            GameManager.instance.gameObject.SetActive(true);
        }
        
        SceneManager.LoadScene("Main");
    }

    public void Setting()
    {
        settingPanel.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
