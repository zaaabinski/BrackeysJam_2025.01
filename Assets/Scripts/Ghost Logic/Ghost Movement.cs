using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AI;

public class GhostMovement : MonoBehaviour
{
    [SerializeField] private List<GameObject> anomalyPoints = new List<GameObject>();
    
    #region components / instances
    private NavMeshAgent _agent;
    private GameManager _gameManager;

    #endregion

    #region settings
    [Header("Settings")]
    public float resumeMovingDelay;
    public float stoppingRadius = 2f; // Ghost stops within this radius instead of exact point

    [Tooltip("The last 30 seconds the ghost speed gets multiplied by this value")]
    [SerializeField] private float _speedMultiplier;

    #endregion

    private void Awake()
    {
        anomalyPoints = GameObject.FindGameObjectsWithTag("AnomalyPoint").ToList();
        _agent = GetComponent<NavMeshAgent>();
        _gameManager = FindAnyObjectByType<GameManager>();
    }

    private void ApplySpeedMultiplier(){
        _agent.speed *= _speedMultiplier;
    }

    private void OnEnable()
    {
        _gameManager.Last30SecondsStart += ApplySpeedMultiplier;
    }

    private void OnDisable()
    {
        _gameManager.Last30SecondsStart -= ApplySpeedMultiplier;
    }

    private void Start()
    {
        StartCoroutine(RandomAnomalyDestination());
    }

    private void Update()
    {
        if (!_agent.pathPending && _agent.remainingDistance <= stoppingRadius)
        {
            StartCoroutine(RandomAnomalyDestination());
        }
    }

    private IEnumerator RandomAnomalyDestination()
    {
        yield return new WaitForSeconds(resumeMovingDelay);

        Vector3 targetPoint = anomalyPoints[Random.Range(0, anomalyPoints.Count)].transform.position;
        
        // Offset the destination by a random point within the stopping radius
        Vector3 offset = Random.insideUnitSphere * stoppingRadius;
        offset.y = 0; // Keep ghost on the ground
        targetPoint += offset;
        
        _agent.SetDestination(targetPoint);
    }
}