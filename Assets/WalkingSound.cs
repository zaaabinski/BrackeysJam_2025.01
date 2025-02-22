using UnityEngine;

public class WalkingSound : MonoBehaviour
{
    [SerializeField] AudioClip[] walkingSounds;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void FootStep()
    {
        AudioClip clip = walkingSounds[Random.Range(0, walkingSounds.Length)];
        audioSource.clip = clip;
        audioSource.Play();
    }
}
