using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using Unity.AI.Navigation;
using System.Linq;

public class BuyersMovement : MonoBehaviour
{
    #region components

    private NavMeshAgent _agent;
    [SerializeField] private NavmeshUtilities _navmeshUtilities;
    [SerializeField] private NavMeshSurface _navmeshSurface;

    #endregion

    #region settings
    [Header("Settings")]
    
    [SerializeField] private float _anomalyDetectionRadius;
    [SerializeField] private LayerMask _anomalyDetectionMask;

    [SerializeField] private float _resumeMovingDelay;

    #endregion

    #region 

    public bool IsScared { get; private set; }

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
        // Check for any nearby anomalies
        Collider[] surroundingColliders = Physics.OverlapSphere(transform.position, _anomalyDetectionRadius, _anomalyDetectionMask, QueryTriggerInteraction.Collide);
        Collider[] detectedAnomalies = surroundingColliders.Where(x => x.CompareTag("Anomaly")).ToArray();

        if (detectedAnomalies.Length > 0){
            // Make sure we aren't setting the destination to the same thing
            if (_agent.destination != detectedAnomalies[0].transform.position){
                _agent.SetDestination(detectedAnomalies[0].transform.position);
            }

            IsScared = true;

            return; // prevent it from going away from the anomaly before the player removes it
        }

        IsScared = false;

        // If it reached its destination we reset it to another random point
        if (_agent.remainingDistance <= _agent.stoppingDistance){
            StartCoroutine(RandomAnomolyDestination());
        }
    }

    private IEnumerator RandomAnomolyDestination(){
        yield return new WaitForSeconds(_resumeMovingDelay);

        Vector3 point = _navmeshUtilities.GetRandomPointOnNavmesh(_navmeshSurface);

        _agent.SetDestination(point);
    }
}
