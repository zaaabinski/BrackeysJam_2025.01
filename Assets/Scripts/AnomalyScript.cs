using UnityEngine;
using UnityEngine.InputSystem;

public class AnomalyScript : MonoBehaviour
{
    #region Settings

    [Header("Settings")]

    [SerializeField] private InputActionReference _interactionReference;

    #endregion

    #region Instances / Components

    [Header("Instances / Components")]

    [SerializeField] private GameObject infoText;
    [SerializeField] private GameObject anomalyGoneParticles;

    #endregion

    #region private variables

    private bool anomalyActive = true;
    private bool isPlayerInRange = false;

    #endregion

    private void Awake()
    {
        _interactionReference.action.Enable();
    }

    private void Update()
    {
        bool keyPressed = _interactionReference.action.ReadValue<float>() == 1;

        if (keyPressed && isPlayerInRange)
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