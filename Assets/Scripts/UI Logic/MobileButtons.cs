using UnityEngine;

public class MobileButtons : MonoBehaviour
{
    [SerializeField] private GameObject[] _mobileUI;

    private void Awake()
    {
        if (Application.isMobilePlatform)
        {
            foreach (GameObject go in _mobileUI)
            {
                go.SetActive(true);
            }
        }
    }
}
