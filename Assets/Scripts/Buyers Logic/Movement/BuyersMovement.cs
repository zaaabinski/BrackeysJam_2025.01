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

    #endregion

    #region settings

    [Header("Settings")]
    
    [SerializeField] private float _scareTimeBeforeRunning;

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
    }

    private void Start()
    {
        SetState(new NormalState(this));
    }

    public void Update(){
        _currentState?.UpdateState();
    }

    public void SetState(IBuyerState newState)
    {
        _currentState?.ExitState();
        _currentState = newState;
        _currentState.EnterState();
    }


    // Behavior functions (called by states)
    public bool CheckForAnomalies() {
         return false; 
    }

    public void PickRandomDestination() { 
        Vector3 randomDestination = _navmeshUtilities.GetRandomPointOnNavmesh(_navmeshSurface);

        _agent.SetDestination(randomDestination);
    }

    public bool HasReachedDestination() { 
        return !_agent.pathPending && _agent.remainingDistance <= 2f; 
    }

    public void MoveToAnomaly(Transform anomaly) {
         _agent.SetDestination(anomaly.position);
    }

    public bool IsScared() {
        return CurrentState == BuyerStateType.Scared;
    }

    public bool ShouldEscape() { 
        return _scaredTimer > _scareTimeBeforeRunning;
    }
}