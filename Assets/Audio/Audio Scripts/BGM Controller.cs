using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioSource audioSource; // Assign in Inspector
    public AudioClip introClip; // Intro clip (plays once)
    public AudioClip loopClip; // Loop clip (plays after intro)
    
    public float crossfadeDuration = 0.4f; // Duration for crossfade

    private bool introPlayed = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
        PlayIntro();
    }

    void PlayIntro()
    {
        audioSource.clip = introClip; // Set the intro clip
        audioSource.loop = false; // Ensure the intro does not loop
        audioSource.Play(); // Play the intro clip

        // Start crossfade only after the intro clip finishes
        Invoke("StartCrossfade", introClip.length); // Start crossfade after intro finishes
    }

    void StartCrossfade()
    {
        StartCoroutine(CrossfadeMusic()); // Begin crossfade coroutine
    }

    System.Collections.IEnumerator CrossfadeMusic()
    {
        // Set up loop clip
        AudioSource loopSource = gameObject.AddComponent<AudioSource>(); // Create a new AudioSource for the loop
        loopSource.clip = loopClip; // Set the loop clip
        loopSource.loop = true; // Enable looping for the loop clip
        loopSource.volume = 0f; // Start at zero volume for smooth transition
        loopSource.Play(); // Start playing the loop clip

        // Gradually fade in the loop clip and fade out the intro
        float elapsedTime = 0f;
        while (elapsedTime < crossfadeDuration)
        {
            // Gradually fade in loop and fade out intro
            loopSource.volume = Mathf.Lerp(0f, 1f, elapsedTime / crossfadeDuration); // Fade in loop
            audioSource.volume = Mathf.Lerp(1f, 0f, elapsedTime / crossfadeDuration); // Fade out intro
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Final adjustments
        loopSource.volume = 1f; // Ensure loop is fully audible
        audioSource.volume = 0f; // Stop intro sound gradually

        // Cleanup: Remove the intro audio source once the intro is faded out
        Destroy(audioSource);
    }
}
