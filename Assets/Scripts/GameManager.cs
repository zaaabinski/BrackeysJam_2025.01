using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private float fearGap;
    
    [SerializeField] private float generalFear = 0;
    [SerializeField] private Slider generalFearSlider;
    [SerializeField] private List<GameObject> buyersList = new List<GameObject>();
    [SerializeField] private GameObject buyerHolder;


    private bool isGameOver = false;

    void Start()
    {
        if (instance == null)
            instance = this;
        Initialize();
    }

    private void Initialize()
    {
        generalFear = 0;
        foreach (Transform child in buyerHolder.transform)
        {
            buyersList.Add(child.gameObject);
        }
    }

    public void AddFear(float amount)
    {
        generalFear += amount;
        generalFearSlider.value = generalFear;
        if (generalFear >= fearGap && isGameOver == false)
        {
            AllEscape();
            isGameOver = true;
        }
    }

    private void AllEscape()
    {
        foreach (GameObject buyer in buyersList)
        {
            buyer.GetComponent<BuyersMovement>().isEscaping = true;
        }
    }
}