using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Other stuff")]
    [SerializeField] GameObject _settingsContainer;

    [Header("Scene Loading")]
    [SerializeField] private string _GameSceneName;
    [SerializeField] private string _mainMenuSceneName;

    public void PlayGame(){
        SceneManager.LoadScene(_GameSceneName);
    }

    private void Awake()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu")
            PlayerPrefs.SetInt("howManyGhostToSpawn",1);
            PlayerPrefs.SetInt("amountOfBuyersToSpawn", 1);
    }

    public void ReturnToMenu(){
        PlayerPrefs.SetInt("howManyGhostToSpawn",1);
        PlayerPrefs.SetInt("amountOfBuyersToSpawn",1);
        
        SceneManager.LoadScene(_mainMenuSceneName);
    }

    public void RestartGameWhenOver()
    {
        PlayerPrefs.SetInt("howManyGhostToSpawn",1);
        PlayerPrefs.SetInt("amountOfBuyersToSpawn", 1);

        SceneManager.LoadScene(_GameSceneName);
    }
    
    public void QuitGame(){
        Application.Quit();
    }

    public void OpenSettings(){
        _settingsContainer.SetActive(true);
    }

    public void CloseSettings(){
        _settingsContainer.SetActive(false);
    }
}
