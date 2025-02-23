using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    #region timer

    [Header("Timer Logic")]
    [SerializeField] private float _startTime;
    public float _timer;
    [SerializeField] private TextMeshProUGUI timerText;

    public event Action Last30SecondsStart;

    #endregion

    #region fear
    [SerializeField] private float fearGap;

    [SerializeField] private float generalFear = 0;
    [SerializeField] private Slider generalFearSlider;

    #endregion

    #region Ending Screens

    [Header("Ending screens")]
    
    [SerializeField] private GameObject winScreenPanel;
    [SerializeField] private GameObject loseScreenPanel;
    [SerializeField] private TextMeshProUGUI buyersInfoText;

    #endregion

    #region buyers spawning

    [Header("Buyer and ghost spawning settings")]

    //[SerializeField] private int _buyersIncrementPerLevel;
    
    [SerializeField] private GameObject _buyerPrefab;
    
    [SerializeField] private int howManyGhostToSpawn = 1;
    [SerializeField] private GameObject ghostPrefab;

    private int _amountOfBuyersToSpawn = 3;

    private List<GameObject> _ghosts = new();
    public List<GameObject> _buyers = new();
    [SerializeField] private AudioSource knockAudioSource;
    [SerializeField] private AudioSource slamAudioSource;
    #endregion

    #region rabbit Icons
    [SerializeField] private GameObject[] rabbitsIconList;
    [SerializeField] private GameObject[] winIconList;
    [SerializeField] private GameObject[] loseIconList;
    [SerializeField] private Sprite runAwayIcon;
    [SerializeField] private Sprite didntShowUpIcon;
    [SerializeField] private Sprite baseIcon;
    [SerializeField] private Sprite soldIcon;

    #endregion
    
    private bool isGameOver = false;

    void Start()
    {
        if (instance == null){
            instance = this;
        }
        else if (instance != this){
            Destroy(gameObject);
        }

        Initialize();
    }

    private IEnumerator ReduceTime()
    {
        while (_timer > 0){
            _timer--;
            UpdateTimeText();

            if (_timer <= 30){
                Last30SecondsStart?.Invoke();
            }

            yield return new WaitForSeconds(1);
        }

        Time.timeScale = 0;
        Debug.Log("Selling finished");
        LevelComplete();
    }

    private void UpdateTimeText(){
        int minutes = (int)_timer / 60;
        int seconds = (int)_timer % 60;

        timerText.text = $"{minutes}m {seconds}s";
    }

    private void Initialize()
    {
        Time.timeScale = 1;
        generalFear = 0;
        _timer = _startTime;

        // Destroy deactivated ghosts from last time
        foreach (GameObject ghost in _ghosts){
            if (ghost == null) continue;

            Destroy(ghost);
        }
        _ghosts.Clear();

        // Destroy deactivated buyers from last time
        foreach (GameObject buyer in _buyers){
            if (buyer == null) continue;

            Destroy(buyer);
        }
        _buyers.Clear();
        
        howManyGhostToSpawn = SceneManager.GetActiveScene().buildIndex;
        _amountOfBuyersToSpawn = 3;

        SpawnGhosts(howManyGhostToSpawn);
        //SpawnBuyers(_amountOfBuyersToSpawn);
        StartCoroutine(SpawnForBuyers(_amountOfBuyersToSpawn));
        StartCoroutine(ReduceTime());
    }

    private void SpawnGhosts(int amount)
    {
        for (int i = 0; i < amount; i++){
            GameObject newGhost = Instantiate(ghostPrefab, transform.position, Quaternion.identity);
            _ghosts.Add(newGhost);
        }
    }

    /*private void SpawnBuyers(int amount){
        for (int i = 0; i < amount; i++){
            GameObject newBuyer = Instantiate(_buyerPrefab, transform.position, Quaternion.identity);
            _buyers.Add(newBuyer);
        }
    }*/

    private IEnumerator SpawnForBuyers(int amount)
    {
        for (int i = 0; i < amount; i++){
            GameObject newBuyer = Instantiate(_buyerPrefab, new Vector3(0,1,-9), Quaternion.identity);
            rabbitsIconList[i].SetActive(true);
            knockAudioSource.Play();
            Image winIconImage = winIconList[i].GetComponent<Image>();
            winIconImage.sprite = soldIcon;
            _buyers.Add(newBuyer);
            yield return new WaitForSeconds(60f);
        }
    }
    
    public void RemoveBuyer(GameObject buyer)
    {
        if (_buyers.Count > 0)
        {
            // Always start updating from index 0 (the first buyer that escapes)
            int index = _buyers.IndexOf(buyer);

            if (index >= 0)
            {
                // Access the Image components directly, even if the GameObjects are inactive
                Image rabbitIconImage = rabbitsIconList[index].GetComponent<Image>();
                Image winIconImage = winIconList[index].GetComponent<Image>();
                Image loseIconImage = loseIconList[index].GetComponent<Image>();

                if (rabbitIconImage != null)
                    rabbitIconImage.sprite = runAwayIcon;

                if (winIconImage != null)
                    winIconImage.sprite = runAwayIcon;

                if (loseIconImage != null)
                    loseIconImage.sprite = runAwayIcon;

                // Remove the buyer from the list
                slamAudioSource.Play();
                _buyers.RemoveAt(index);
            }
        }

        // Check if all buyers have escaped and trigger Game Over
        if (_buyers.Count == 0 && !isGameOver)
        {
            GameOver();
        }
    }


    private void LevelComplete()
    {
        howManyGhostToSpawn += _buyers.Count;

        PlayerPrefs.SetInt("howManyGhostToSpawn", howManyGhostToSpawn);

        winScreenPanel.SetActive(true);

        /*if (_buyers.Count > 0) {
            buyersInfoText.text =  _buyers.Count.ToString() + " stayed, there are all ghosts now...";
        }*/
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
            buyer.GetComponent<BuyersMovement>().SetState(new EscapingState(buyer.GetComponent<BuyersMovement>()));
        }
    }
}