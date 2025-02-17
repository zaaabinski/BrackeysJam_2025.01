using System;
using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Unity.AI.Navigation;
using System.Linq;
using Random = UnityEngine.Random;

public enum PossibleStates {
    Normal,
    Investigating,
    Scared,
    Escaping,
    HasEscaped
}

public class BuyersMovement : MonoBehaviour
{
    #region components

    private NavMeshAgent _agent;
    private MeshRenderer _renderer;
    [SerializeField] private NavmeshUtilities _navmeshUtilities;
    [SerializeField] private NavMeshSurface _navmeshSurface;

    [Header("Anomaly settings")]
    [Tooltip(
        "After investigating the anomaly it gets scared. After this many seconds it runs away if its still scared")]
    [SerializeField]
    private float _runAwayCooldown;

    [SerializeField] Transform _getAwayPoint;

    #endregion
    
    #region private variables

    [SerializeField] private float _scaredTimer;

    #endregion

    #region settings

    [Header("Settings")] [SerializeField] private float _anomalyDetectionRadius;
    [SerializeField] private LayerMask _anomalyDetectionMask;

    [SerializeField] private float _resumeMovingDelay;
    [SerializeField] private float stoppingRadius = 2f; // Buyer stops within this radius

    #endregion

    #region Properties

    private PossibleStates _currentState;

    public PossibleStates CurrentState {
        get => _currentState;

        set {
            _currentState = value;

            scaredMark.SetActive(value != PossibleStates.Normal);

            if (value == PossibleStates.HasEscaped){
                GameManager.instance.RemoveBuyer(this.gameObject);
            }
        }
    }

    [SerializeField] private GameObject scaredMark;

    #endregion

    private float timeAdder = 0;
    private GameObject _currentInvestigationTarget;

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
        if (CurrentState == PossibleStates.HasEscaped) {
            return;
        }

        if (CurrentState == PossibleStates.Scared) {
            timeAdder = Time.deltaTime;
            _scaredTimer += timeAdder;
            GameManager.instance.AddFear(timeAdder);
        }
        else {
            _scaredTimer = 0;
        }

        if (_scaredTimer >= _runAwayCooldown) {
            CurrentState = PossibleStates.Escaping;
        }
        
        if (CurrentState == PossibleStates.Escaping) {
            _renderer.material.color = Color.red;

            if (_agent.destination != _getAwayPoint.position) {
                _agent.destination = _getAwayPoint.position;
            }

            return;
        }

        // Check for any nearby anomalies
        Collider[] surroundingColliders = Physics.OverlapSphere(transform.position, _anomalyDetectionRadius,
            _anomalyDetectionMask, QueryTriggerInteraction.Collide);
        Collider[] detectedAnomalies = surroundingColliders.Where(x => x.CompareTag("Anomaly")).ToArray();

        if (detectedAnomalies.Length > 0)
        {
            Vector3 anomalyPosition = detectedAnomalies[0].transform.position;

            // Offset the destination by a random point within the stopping radius
            Vector3 offset = Random.insideUnitSphere * stoppingRadius;
            offset.y = 0; // Keep buyer on the ground
            Vector3 targetPosition = anomalyPosition + offset;

            _agent.SetDestination(targetPosition);

            // Make sure we aren't setting the destination to the same thing
            if (_currentInvestigationTarget != detectedAnomalies[0].gameObject)
            {
                CurrentState = PossibleStates.Investigating;

                _currentInvestigationTarget = detectedAnomalies[0].gameObject;
            }

            if (HasReachedDestination()){
                CurrentState = PossibleStates.Scared;
            }

            return; // Prevent moving away before the anomaly is removed
        }

        CurrentState = PossibleStates.Normal;

        // If it reached its destination within stopping radius, pick another destination
        if (HasReachedDestination())
        {
            StartCoroutine(RandomAnomalyDestination());
        }
    }

    private bool HasReachedDestination(){
        if (!_agent.pathPending && _agent.remainingDistance <= stoppingRadius)
        {
            return true;
        }

        return false;
    }

    private IEnumerator RandomAnomalyDestination()
    {
        yield return new WaitForSeconds(_resumeMovingDelay);

        Vector3 point = _navmeshUtilities.GetRandomPointOnNavmesh(_navmeshSurface);

        // Offset the destination by a random point within the stopping radius
        Vector3 offset = Random.insideUnitSphere * stoppingRadius;
        offset.y = 0; // Keep buyer on the ground
        point += offset;

        _agent.SetDestination(point);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GetAway")&& CurrentState == PossibleStates.Escaping)
        {
            CurrentState = PossibleStates.HasEscaped;    
            gameObject.SetActive(false);
        }
    }
}