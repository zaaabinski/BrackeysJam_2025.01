using UnityEngine;

public class FlashScreen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1;
        Invoke("SetOff", 2.5f);
    }

    private void SetOff()
    {
        gameObject.SetActive(false);
    }
}
