using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnomalyScript : MonoBehaviour
{
    #region Settings

    [Header("Settings")]

    [SerializeField] private InputActionReference _interactionReference;
    [SerializeField] private float _anomalyProtectionTime;

    #endregion

    #region Instances / Components

    [Header("Instances / Components")]

    [SerializeField] private GameObject infoText;
    [SerializeField] private GameObject anomalyGoneParticles;
    private Collider _anomalyCollider; // Added reference to the collider

    #endregion

    #region Private Variables

    private bool isPlayerInRange = false;

    #endregion

    public bool anomalyActive = true;
    public bool _anomalyProtected = false;

    #region Animation
    [SerializeField] private Animator anomalyAnimator;
    [SerializeField] private ParticleSystem anomalyParticles;
    #endregion

    private void Awake()
    {
        _interactionReference.action.Enable();
        anomalyAnimator = gameObject.GetComponentInChildren<Animator>();
        anomalyParticles = gameObject.GetComponentInChildren<ParticleSystem>();
        _anomalyCollider = GetComponent<Collider>(); // Get the collider reference
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
        if (_anomalyProtected) return;

        anomalyActive = true;
        anomalyAnimator.SetBool("Start", true);
        Invoke("StartParticles",0.5f);
        StartCoroutine(ProtectAnomaly());
        StartCoroutine(RefreshTrigger()); // Refresh the trigger collider
    }


    private void StartParticles()
    {
        anomalyParticles.Play();

    }
    
    private IEnumerator ProtectAnomaly()
    {
        _anomalyProtected = true;
        yield return new WaitForSeconds(_anomalyProtectionTime);
        _anomalyProtected = false;
    }

    private void StopAnomaly()
    {
        isPlayerInRange = false;
        infoText.SetActive(false);
       // GameObject particles = Instantiate(anomalyGoneParticles, transform.position, Quaternion.identity);
        //Destroy(particles, 1f);
        anomalyActive = false;
        anomalyAnimator.SetBool("Start", false);
        anomalyParticles.Stop();
    }

    private IEnumerator RefreshTrigger()
    {
        if (_anomalyCollider != null)
        {
            _anomalyCollider.enabled = false;
            yield return new WaitForSeconds(0.1f); // Small delay
            _anomalyCollider.enabled = true;
        }
    }
}
