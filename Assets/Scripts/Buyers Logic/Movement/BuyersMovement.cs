using System;
using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Unity.AI.Navigation;
using System.Linq;
using Random = UnityEngine.Random;
using Unity.Collections;

public enum BuyerStateType
{
    Normal,
    Investigating,
    Scared,
    Escaping,
    HasEscaped
}

public interface IBuyerState
{
    BuyerStateType StateType { get; }
    void EnterState();
    void UpdateState();
    void ExitState();
}

public class BuyersMovement : MonoBehaviour
{
    #region components / instances

    [Header("Components / Instances")]
    private NavMeshAgent _agent;
    private NavmeshUtilities _navmeshUtilities;
    private NavMeshSurface _navmeshSurface;
    private GameObject _scaredMark;
    private Transform _visibilityStartObject;

    [SerializeField] private GameObject _exit;

    #endregion

    #region settings

    [Header("Settings")]
    
    [SerializeField] private float _scareTimeBeforeRunning;
    [SerializeField] private float _anomalyDetectionRadus;
    [SerializeField] private BuyersSettings _buyerSettings;

    #endregion

    #region state tracking variables

    private float _scaredTimer;

    #endregion

    private IBuyerState _currentState;
    public BuyerStateType CurrentState => _currentState.StateType;

    private void Awake()
    {
        _navmeshUtilities = FindAnyObjectByType<NavmeshUtilities>();
        _navmeshSurface = FindAnyObjectByType<NavMeshSurface>();
        _agent = GetComponent<NavMeshAgent>();
        _exit = FindAnyObjectByType<GetAwayIdentifier>().gameObject;
        _scaredMark = transform.Find("ScaredMark").gameObject;
        _visibilityStartObject = transform.Find("VisibilityStartObject");
    }

    private void Start()
    {
        SetState(new NormalState(this));
    }

    public void Update(){
        _currentState?.UpdateState();

        if (IsScared()){
            _scaredTimer += Time.deltaTime;
            GameManager.instance.AddFear(Time.deltaTime);
        }
        else{
            _scaredTimer = 0;
        }
    }

    public void SetState(IBuyerState newState)
    {
        _currentState?.ExitState();
        _currentState = newState;
        _currentState.EnterState();
    }


    // Behavior functions (called by states)

    public void PickRandomDestination() { 
        Vector3 randomDestination = _navmeshUtilities.GetRandomPointOnNavmesh(_navmeshSurface);

        _agent.SetDestination(randomDestination);
    }

    public bool HasReachedDestination() { 
        return !_agent.pathPending && _agent.remainingDistance <= 2f; 
    }

    public bool CheckForAnomalies() {
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, _anomalyDetectionRadus);
        Collider[] anomalyNearbyColliders = nearbyColliders.Where(x => x.CompareTag("Anomaly")).ToArray();

        return anomalyNearbyColliders.Length > 0;
    }

    public GameObject FindClosestAnomaly()
    {
        // Get all anomalies in detection radius
        var anomalyColliders = Physics.OverlapSphere(transform.position, _anomalyDetectionRadus)
                                    .Where(x => x.CompareTag("Anomaly"))
                                    .ToArray();

        GameObject closestAnomaly = null;
        float closestDistance = float.MaxValue;

        foreach (var anomalyCollider in anomalyColliders)
        {
            // Check if the anomaly is visible (not blocked by obstacles)
            if (IsAnomalyVisible(anomalyCollider))
            {
                float distance = Vector3.Distance(transform.position, anomalyCollider.transform.position);

                if (distance < closestDistance)
                {
                    closestAnomaly = anomalyCollider.gameObject;
                    closestDistance = distance;
                }
            }
        }

        return closestAnomaly;
    }

    // Helper method to check if anomaly is visible
    private bool IsAnomalyVisible(Collider anomalyCollider)
    {
        Vector3 direction = anomalyCollider.transform.position - _visibilityStartObject.position;

        if (Physics.Raycast(_visibilityStartObject.position, direction, out RaycastHit hit, _anomalyDetectionRadus))
        {
            return hit.collider == anomalyCollider;
        }

        return false;
    }

    public void MoveToAnomaly() {
        GameObject anomaly = FindClosestAnomaly();

        if (anomaly == null){
            return; // This means it wasnt visible via raycasting
        }

         _agent.SetDestination(anomaly.transform.position);
    }

    public bool IsScared() {
        return CurrentState == BuyerStateType.Scared;
    }

    public bool ShouldEscape() { 
        return _scaredTimer > _scareTimeBeforeRunning;
    }

    public void RetargetToExit(){
        _agent.SetDestination(_exit.transform.position);
    }

    public void ToggleScaredMark(bool status){
        _scaredMark.SetActive(status);
    }

    public void LerpScaredMarkColor(){
        float normalizedScaredTimer = _scaredTimer / _scareTimeBeforeRunning;
        _scaredMark.GetComponent<Renderer>().material.color = _buyerSettings.ScaredMarkGradient.Evaluate(normalizedScaredTimer);
    }
}