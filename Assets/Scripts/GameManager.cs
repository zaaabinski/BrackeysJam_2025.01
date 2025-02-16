using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    [SerializeField] private float timer;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float fearGap;

    [SerializeField] private float generalFear = 0;
    [SerializeField] private Slider generalFearSlider;
    public List<GameObject> buyersList = new List<GameObject>();
    [SerializeField] private GameObject buyerHolder;
    [SerializeField] private int howManyGhostToSpawn = 1;
    [SerializeField] private GameObject ghostPrefab;
    
    [SerializeField] private GameObject winScreenPanel;
    [SerializeField] private GameObject loseScreenPanel;
    [SerializeField] private TextMeshProUGUI buyersInfoText;

    private bool isGameOver = false;

    void Start()
    {
        if (instance == null)
            instance = this;
        Initialize();
    }

    private IEnumerator ReduceTime()
    {
        if (timer >= 0)
        {
            timer--;
            timerText.text = timer.ToString();
            yield return new WaitForSeconds(1);
            StartCoroutine(ReduceTime());
        }
        else if (timer <= 1)
        {
            Time.timeScale = 0;
            Debug.Log("Selling finished");
            LevelComplete();
            //function to call to calculate buyers left and ghosts ect
        }
    }

    private void Initialize()
    {
        Time.timeScale = 1;
        generalFear = 0;
        timer = 60;
        
        foreach (Transform child in buyerHolder.transform)
        {
            buyersList.Add(child.gameObject);
        }
        if(PlayerPrefs.HasKey("howManyGhostToSpawn"))
            howManyGhostToSpawn = PlayerPrefs.GetInt("howManyGhostToSpawn");
        else
        {
            PlayerPrefs.SetInt("howManyGhostToSpawn", 1);
            howManyGhostToSpawn = PlayerPrefs.GetInt("howManyGhostToSpawn");
        }
        SpawnGhosts(howManyGhostToSpawn);
        
        
        StartCoroutine(ReduceTime());
    }

    private void SpawnGhosts(int amount)
    {
        for(int i = 0; i < amount; i++)
            Instantiate(ghostPrefab, transform.position, Quaternion.identity);
    }

    public void RemoveBuyer(GameObject buyer)
    {
            buyersList.Remove(buyer);
            if(buyersList.Count == 0 && !isGameOver)
                GameOver();
    }

    private void LevelComplete()
    {
        howManyGhostToSpawn += buyersList.Count;
        PlayerPrefs.SetInt("howManyGhostToSpawn", howManyGhostToSpawn);
        winScreenPanel.SetActive(true);
        if(buyersList.Count >= 0)
             buyersInfoText.text =  buyersList.Count.ToString() + " stayed, there are all ghosts now...";
        
    }

    private void GameOver()
    {
        AllEscape();
        isGameOver = true;
        Time.timeScale = 0;
        loseScreenPanel.SetActive(true);
        Debug.Log("Game over");
    }
    
    public void AddFear(float amount)
    {
        generalFear += amount;
        generalFearSlider.value = generalFear;
        if (generalFear >= fearGap && !isGameOver)
        {
            GameOver();
        }
    }

    private void AllEscape()
    {
        foreach (GameObject buyer in buyersList)
        {
            buyer.GetComponent<BuyersMovement>().CurrentState = PossibleStates.Escaping;
        }
    }
}