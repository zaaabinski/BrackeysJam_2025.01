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

    public void ReturnToMenu(){
        SceneManager.LoadScene(_mainMenuSceneName);
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
