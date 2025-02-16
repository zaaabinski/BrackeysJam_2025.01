using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class BuyerMovement : MonoBehaviour
{
    public List<Transform> anomolyPoints = new List<Transform>();

    #region components

    private NavMeshAgent _agent;

    #endregion

    #region settings
    [Header("Settings")]
    
    public float resumeMovingDelay;

    #endregion

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        StartCoroutine(RandomAnomolyDestination());
    }

    private void Update()
    {
        if (_agent.remainingDistance <= _agent.stoppingDistance){
            StartCoroutine(RandomAnomolyDestination());
        }
    }

    private IEnumerator RandomAnomolyDestination(){
        yield return new WaitForSeconds(resumeMovingDelay);

        Vector3 point = anomolyPoints[Random.Range(0, anomolyPoints.Count)].position;

        _agent.SetDestination(point);
    }
}
