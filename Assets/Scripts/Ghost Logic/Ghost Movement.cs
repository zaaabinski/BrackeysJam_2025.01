using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class GhostMovement : MonoBehaviour
{
    public List<Transform> anomolyPoints = new List<Transform>();

    #region components

    private NavMeshAgent _agent;

    #endregion

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        RandomAnomolyDestination();
    }

    private void Update()
    {
        if (_agent.remainingDistance <= _agent.stoppingDistance){
            RandomAnomolyDestination();
        }
    }

    private void RandomAnomolyDestination(){
        Vector3 point = anomolyPoints[Random.Range(0, anomolyPoints.Count)].position;

        _agent.SetDestination(point);
    }
}
