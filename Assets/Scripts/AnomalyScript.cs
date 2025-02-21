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

    private bool isPlayerInRange = false;

    #endregion
    
    public bool anomalyActive = true;
    
    #region animation
    [SerializeField] private Animator anomalyAnimator;
    [SerializeField] private ParticleSystem anomalyParticles;
    #endregion
    
    private void Awake()
    {
        _interactionReference.action.Enable();
        anomalyAnimator = gameObject.GetComponentInChildren<Animator>();
        /*Invoke("StartAnomaly",1);
        anomalyActive = true;*/
    }

    private void Update()
    {
        bool keyPressed = _interactionReference.action.ReadValue<float>() == 1;

        if (keyPressed && isPlayerInRange && anomalyActive)
        {
            StopAnomaly();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && anomalyActive)
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

    public void StartAnomaly()
    {
        anomalyAnimator.SetBool("Start",true);
        anomalyParticles.Play();
    }

    private void StopAnomaly()
    {
        isPlayerInRange = false;
        infoText.SetActive(false);
        GameObject particles =  Instantiate(anomalyGoneParticles, transform.position, Quaternion.identity);
        Destroy(particles,1f);
        anomalyActive = false;
        anomalyAnimator.SetBool("Start", false);
        anomalyParticles.Stop();
    }
    
}