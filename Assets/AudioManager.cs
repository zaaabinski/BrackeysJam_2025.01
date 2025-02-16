using UnityEngine;

public class AudioManager : MonoBehaviour //The video I used as reference for this was https://www.youtube.com/watch?v=IxHPzrEq1Tc
{
   [Header("---------- Audio Source ----------")]
   [SerializeField] AudioSource musicSource;
   [SerializeField] AudioSource sfxSource;

   [Header("---------- Audio Clip ----------")]
   public AudioClip background;
   public AudioClip menu_click; //He makes one of these for every sound. It doesn't look like the best way. I didn't continue with the tutorial to not mess with more than what I can see
   public AudioClip menu_hover;

   private void Start()
   {
	   musicSource.clip = background;
	   musicSource.Play();
   }
}
