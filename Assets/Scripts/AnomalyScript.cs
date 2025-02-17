using UnityEngine;

public class AnomalyScript : MonoBehaviour
{
    #region settings

    [SerializeField] private KeyCode _interactionKey;

    #endregion

    [SerializeField] private GameObject infoText;
    [SerializeField] private GameObject anomalyGoneParticles;
    private bool anomalyActive = true;
    private bool isPlayerInRange = false;

    private void Update()
    {
        if (Input.GetKeyDown(_interactionKey) && isPlayerInRange)
        {
            isPlayerInRange = false;
            infoText.SetActive(false);
            GameObject particles =  Instantiate(anomalyGoneParticles, transform.position, Quaternion.identity);
            Destroy(particles,1f);
            Debug.Log("Deactivating anomaly: " + gameObject.name);
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            infoText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            infoText.SetActive(false);
        }
    }
}