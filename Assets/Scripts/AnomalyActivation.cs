using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class AnomalyActivation : MonoBehaviour
{
    [SerializeField] private int chanceForSpawn=25;
    [SerializeField] private float _delayBeforeApplyingAnomaly;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ghost"))
        {   
            StartCoroutine(ApplyAnomaly(other.gameObject));
        }
    }

    private IEnumerator ApplyAnomaly(GameObject ghost)
    {
        // Freeze ghost
        NavMeshAgent agent = ghost.GetComponent<NavMeshAgent>();

        float ghostSpeed = agent.speed;
        agent.speed = 0;

        yield return new WaitForSeconds(_delayBeforeApplyingAnomaly);

        agent.speed = ghostSpeed;

        // Apply anomaly
        Random random = new Random();

        if (random.Next(0, 100) < chanceForSpawn)
        {
            foreach (Transform child in transform)
            {
                AnomalyScript anomalyScript = child.GetComponent<AnomalyScript>();
                anomalyScript.StartAnomaly();
            }
        }
    }
}
