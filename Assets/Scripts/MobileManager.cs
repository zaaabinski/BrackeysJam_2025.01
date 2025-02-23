using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class MobileManager : MonoBehaviour
{
    public GameObject[] mobileStuff;

    private void Start()
    {
        if (IsMobileDevice())
        {
            foreach (GameObject go in mobileStuff)
            {
                go.SetActive(true);
            }
        }
    }

    bool IsMobileDevice()
    {
        return Application.isMobilePlatform;
    }
}
