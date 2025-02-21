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
    public Animator anim;
    private void Awake()
    {
        _navmeshUtilities = FindAnyObjectByType<NavmeshUtilities>();
        _navmeshSurface = FindAnyObjectByType<NavMeshSurface>();
        _agent = GetComponent<NavMeshAgent>();
        _exit = FindAnyObjectByType<GetAwayIdentifier>().gameObject;
        anim = GetComponentInChildren<Animator>();
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
        anim.SetTrigger("Walking");
        _agent.SetDestination(randomDestination);
    }

    public bool HasReachedDestination() { 
        return !_agent.pathPending && _agent.remainingDistance <= 2f; 
    }

    public bool CheckForAnomalies()
    {
        return Physics.OverlapSphere(transform.position, _anomalyDetectionRadus)
                    .Any(collider => collider.CompareTag("Anomaly") && IsAnomalyVisible(collider) && collider.GetComponent<AnomalyScript>().anomalyActive);
    }


    public GameObject FindClosestAnomaly()
    {
        var anomalyColliders = Physics.OverlapSphere(transform.position, _anomalyDetectionRadus)
                                    .Where(x => x.CompareTag("Anomaly"))
                                    .ToArray();

        anomalyColliders = anomalyColliders.Where(x => x.GetComponent<AnomalyScript>().anomalyActive).ToArray();
        
        GameObject closestAnomaly = null;
        float closestDistance = float.MaxValue;

        foreach (var anomalyCollider in anomalyColliders)
        {
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

    [SerializeField] private LayerMask _visibilityCheckMask;

    private bool IsAnomalyVisible(Collider anomalyCollider)
    {
        Vector3 direction = anomalyCollider.transform.position - _visibilityStartObject.position;
        AnomalyScript anomalyScript = anomalyCollider.gameObject.GetComponent<AnomalyScript>();
        if (anomalyCollider.bounds.Contains(_visibilityStartObject.position))
        {
            return true; // If starting point is inside the anomaly, it's considered visible
            // THIS FIXED THE PROBLEM LETS FUCKIGN GO!!!!
        }

        if (Physics.SphereCast(_visibilityStartObject.position, 0.1f, direction, out RaycastHit hit, _anomalyDetectionRadus, _visibilityCheckMask))
        {
            return hit.collider == anomalyCollider && anomalyScript.anomalyActive;
        }

        return false;
    }


    public void MoveToAnomaly() {
        GameObject anomaly = FindClosestAnomaly();

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