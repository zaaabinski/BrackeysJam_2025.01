using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    public AudioSource footstepSource; // Assign in Inspector
    public AudioClip[] defaultFootsteps;
    public AudioClip[] stoneFootsteps;

    public float footstepInterval = 0.5f; // Time between footsteps
    private float footstepTimer = 0f;

    private Rigidbody playerRigidbody;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Check if the player is moving
        if (playerRigidbody.linearVelocity.magnitude > 0.1f)
        {
            footstepTimer += Time.deltaTime;
            if (footstepTimer >= footstepInterval)
            {
                PlayFootstep();
                footstepTimer = 0f; // Reset timer
            }
        }
        else
        {
            footstepTimer = 0f; // Reset if player stops moving
        }
    }

    void PlayFootstep()
    {
        AudioClip[] footstepSounds = GetFootstepSounds();

        if (footstepSounds.Length > 0)
        {
            int index = Random.Range(0, footstepSounds.Length);
            footstepSource.PlayOneShot(footstepSounds[index]);
        }
    }

    AudioClip[] GetFootstepSounds()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
        {
            string surfaceTag = hit.collider.tag;

            if (surfaceTag == "Stone") return stoneFootsteps;
        }

        return defaultFootsteps;
    }
}
