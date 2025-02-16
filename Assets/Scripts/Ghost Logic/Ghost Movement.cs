using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class GhostMovement : MonoBehaviour
{
    [SerializeField] private List<Transform> anomalyPoints = new List<Transform>();
    [SerializeField] private GameObject pointsList;
    
    #region components
    private NavMeshAgent _agent;
    #endregion

    #region settings
    [Header("Settings")]
    public float resumeMovingDelay;
    public float stoppingRadius = 2f; // Ghost stops within this radius instead of exact point
    #endregion

    private void Awake()
    {
        pointsList = GameObject.Find("PointsForGhost");
        _agent = GetComponent<NavMeshAgent>();
        foreach (Transform child in pointsList.transform)
        {
            anomalyPoints.Add(child);
        }
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

        Vector3 targetPoint = anomalyPoints[Random.Range(0, anomalyPoints.Count)].position;
        
        // Offset the destination by a random point within the stopping radius
        Vector3 offset = Random.insideUnitSphere * stoppingRadius;
        offset.y = 0; // Keep ghost on the ground
        targetPoint += offset;
        
        _agent.SetDestination(targetPoint);
    }
}