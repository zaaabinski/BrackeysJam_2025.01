using UnityEngine;
using System.Collections;
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
    [SerializeField] private float stoppingRadius = 2f; // Buyer stops within this radius
    
    #endregion

    #region

    [SerializeField] private bool IsScared = false;
    [SerializeField] private GameObject scaredMark;
    #endregion

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        StartCoroutine(RandomAnomalyDestination());
    }

    private void Update()
    {
        // Check for any nearby anomalies
        Collider[] surroundingColliders = Physics.OverlapSphere(transform.position, _anomalyDetectionRadius, _anomalyDetectionMask, QueryTriggerInteraction.Collide);
        Collider[] detectedAnomalies = surroundingColliders.Where(x => x.CompareTag("Anomaly")).ToArray();

        if (detectedAnomalies.Length > 0)
        {
            Vector3 anomalyPosition = detectedAnomalies[0].transform.position;
            
            // Offset the destination by a random point within the stopping radius
            Vector3 offset = Random.insideUnitSphere * stoppingRadius;
            offset.y = 0; // Keep buyer on the ground
            Vector3 targetPosition = anomalyPosition + offset;

            // Make sure we aren't setting the destination to the same thing
            if (_agent.destination != targetPosition)
            {
                _agent.SetDestination(targetPosition);
            }

            IsScared = true;
            scaredMark.SetActive(true);

            return; // Prevent moving away before the anomaly is removed
        }

        IsScared = false;
        scaredMark.SetActive(false);

        // If it reached its destination within stopping radius, pick another destination
        if (!_agent.pathPending && _agent.remainingDistance <= stoppingRadius)
        {
            StartCoroutine(RandomAnomalyDestination());
        }
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
}
