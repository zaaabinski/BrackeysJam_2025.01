using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject creditsPanel;

    public void ShowControlsPanel()
    {
        controlsPanel.SetActive(true);
    }
    
    /*public void PlayGame(){
        SceneManager.LoadScene("Level");
    }*/

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

    public void OpenCredits(){
        creditsPanel.SetActive(true);
    }

    public void CloseCredits(){
        creditsPanel.SetActive(false);
    }
    
}
