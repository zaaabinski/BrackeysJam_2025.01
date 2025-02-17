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
    
    [SerializeField] private GameObject winScreenPanel;
    [SerializeField] private GameObject loseScreenPanel;
    [SerializeField] private TextMeshProUGUI buyersInfoText;

    #region buyers spawning

    private int _amountOfBuyersToSpawn = 1;

    [SerializeField] private int _buyersIncrementPerLevel;
    [SerializeField] private GameObject _buyerPrefab;
    [SerializeField] private GameObject buyerHolder;
    
    [SerializeField] private int howManyGhostToSpawn = 1;
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private GameObject _ghostHolder;

    private List<GameObject> _ghosts = new();
    private List<GameObject> _buyers = new();

    #endregion

    private bool isGameOver = false;

    void Start()
    {
        if (instance == null){
            instance = this;
        }
        else {
            Destroy(gameObject); // Prevent duplicates in case we're gonna add more scenes
        }

        Initialize();
    }

    private IEnumerator ReduceTime()
    {
        while (timer > 0){
            timer--;
            timerText.text = timer.ToString();

            yield return new WaitForSeconds(1);
        }

        Time.timeScale = 0;
        Debug.Log("Selling finished");
        LevelComplete();
    }

    private void Initialize()
    {
        Time.timeScale = 1;
        generalFear = 0;
        timer = 60;

        // Destroy deactivated ghosts from last time
        foreach (GameObject ghost in _ghosts){
            if (ghost == null) continue;

            Destroy(ghost);
        }
        _ghosts.Clear();
        
        // Add already existing buyers into their list
        foreach (Transform child in buyerHolder.transform)
        {
            if (child.GetComponent<BuyersMovement>()){
                _buyers.Add(child.gameObject);
            }
            else{
                Debug.LogWarning("Something else than a buyer detected in the buyer holder");
            }
        }
        

        howManyGhostToSpawn = PlayerPrefs.GetInt("howManyGhostToSpawn", 1);
        _amountOfBuyersToSpawn = PlayerPrefs.GetInt("amountOfBuyersToSpawn", 1);

        SpawnGhosts(howManyGhostToSpawn);
        SpawnBuyers(_amountOfBuyersToSpawn);
        
        StartCoroutine(ReduceTime());
    }

    private void SpawnGhosts(int amount)
    {
        for (int i = 0; i < amount; i++){
            GameObject newGhost = Instantiate(ghostPrefab, transform.position, Quaternion.identity);
            _ghosts.Add(newGhost);
        }
    }

    private void SpawnBuyers(int amount){
        for (int i = 0; i < amount; i++){
            GameObject newBuyer = Instantiate(_buyerPrefab, transform.position, Quaternion.identity);
            _buyers.Add(newBuyer);
        }
    }

    public void RemoveBuyer(GameObject buyer)
    {
        _buyers.Remove(buyer);

        if(_buyers.Count == 0 && !isGameOver){
            GameOver();
        }
    }

    private void LevelComplete()
    {
        howManyGhostToSpawn += _buyers.Count;
        _amountOfBuyersToSpawn += _buyersIncrementPerLevel;

        PlayerPrefs.SetInt("howManyGhostToSpawn", howManyGhostToSpawn);
        PlayerPrefs.SetInt("amountOfBuyersToSpawn", _amountOfBuyersToSpawn);

        winScreenPanel.SetActive(true);

        if (_buyers.Count > 0) {
            buyersInfoText.text =  _buyers.Count.ToString() + " stayed, there are all ghosts now...";
        }
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
        foreach (GameObject buyer in _buyers)
        {
            buyer.GetComponent<BuyersMovement>().CurrentState = PossibleStates.Escaping;
        }
    }
}