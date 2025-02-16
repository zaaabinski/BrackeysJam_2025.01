using UnityEngine;

public class AnomalyScript : MonoBehaviour
{
    [SerializeField] private GameObject infoText;
    private bool anomalyActive = true;
    private bool isPlayerInRange = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isPlayerInRange)
            gameObject.SetActive(false);
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