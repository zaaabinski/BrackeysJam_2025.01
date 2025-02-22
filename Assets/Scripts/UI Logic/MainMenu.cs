using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame(){
        SceneManager.LoadScene("Level");
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(sceneBuildIndex: SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void ReturnToMenu(){
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartGameWhenOver()
    {
        SceneManager.LoadScene("Level1");
    }
    
    public void QuitGame(){
        Application.Quit();
    }

    /*public void OpenSettings(){
        _settingsContainer.SetActive(true);
    }

    public void CloseSettings(){
        _settingsContainer.SetActive(false);
    }*/
    
}
