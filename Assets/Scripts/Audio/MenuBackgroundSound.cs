using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBackgroundSound : MonoBehaviour
{
    public static MenuBackgroundSound instance;
    public string _gameSceneName;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == _gameSceneName)
        {
            Destroy(gameObject);
        }
    }
}
