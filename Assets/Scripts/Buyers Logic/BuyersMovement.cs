using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using Unity.AI.Navigation;
using System.Linq;
using UnityEditor.EditorTools;
using Unity.VisualScripting;

public enum PossibleStates{
    Normal,
    Investigating,
    Scared,
    Running
}

public class BuyersMovement : MonoBehaviour
{
    #region components

    private NavMeshAgent _agent;
    private MeshRenderer _renderer;

    [SerializeField] private NavmeshUtilities _navmeshUtilities;
    [SerializeField] private NavMeshSurface _navmeshSurface;

    #endregion

    #region settings
    [Header("Settings")]
    
    [SerializeField] private float _anomalyDetectionRadius;
    [SerializeField] private LayerMask _anomalyDetectionMask;

    [SerializeField] private float _resumeMovingDelay;


    [Header("Anomaly settings")]

    [SerializeField] private float _stoppingRadius = 2f; // Buyer stops within this radius


    [Tooltip("After investigating the anomaly it gets scared. After this many seconds it runs away if its still scared")]
    [SerializeField] private float _runAwayCooldown;

    [SerializeField] Transform _getAwayPoint;

    #endregion

    #region 

    public PossibleStates CurrentState { get; private set; }

    #endregion

    #region private variables

    private float _scaredTimer;

    #endregion

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        StartCoroutine(RandomAnomalyDestination());
    }

    private void Update()
    {
        if (CurrentState == PossibleStates.Scared){
            _scaredTimer += Time.deltaTime;
        }
        else{
            _scaredTimer = 0;
        }

        if (_scaredTimer > _runAwayCooldown){
            CurrentState = PossibleStates.Running;
            
            _renderer.material.color = Color.red;
        }

        // If its running then we set the destination to the getaway point
        if (CurrentState == PossibleStates.Running){
            if (_agent.destination != _getAwayPoint.position){
                _agent.destination = _getAwayPoint.position;
            }

            return;
        }

        // Check for any nearby anomalies
        Collider[] surroundingColliders = Physics.OverlapSphere(transform.position, _anomalyDetectionRadius, _anomalyDetectionMask, QueryTriggerInteraction.Collide);
        Collider[] detectedAnomalies = surroundingColliders.Where(x => x.CompareTag("Anomaly")).ToArray();

        if (detectedAnomalies.Length > 0){
            // Make sure we aren't setting the destination to the same thing
            if (_agent.destination != detectedAnomalies[0].transform.position){
                CurrentState = PossibleStates.Investigating;
                _agent.SetDestination(detectedAnomalies[0].transform.position);
            }

            if (ReachedDestination()){
                CurrentState = PossibleStates.Scared;
            }

            return; // prevent it from going away from the anomaly before the player removes it
        }

        CurrentState = PossibleStates.Normal;

        // Find another random point to set its destination to
        if (ReachedDestination()){
            StartCoroutine(RandomAnomalyDestination());
        }
    }

    private bool ReachedDestination(){
        if (_agent.remainingDistance <= _stoppingRadius){
            return true;
        }

        return false;
    }

    private IEnumerator RandomAnomalyDestination()
    {
        yield return new WaitForSeconds(_resumeMovingDelay);

        Vector3 point = _navmeshUtilities.GetRandomPointOnNavmesh(_navmeshSurface);
        
        // Offset the destination by a random point within the stopping radius
        Vector3 offset = Random.insideUnitSphere * _stoppingRadius;
        offset.y = 0; // Keep buyer on the ground
        point += offset;
        
        _agent.SetDestination(point);
    }
}
