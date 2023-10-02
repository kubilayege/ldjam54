using UnityEngine;

public class AudioManager : SingletonBehaviour<AudioManager>
{
    [SerializeField] private AudioSource audioSource;
    

    public void PlayOnce(AudioClip clip)
    {
        if (clip == null) return;
        
        audioSource.PlayOneShot(clip);
    }

    public void Play(AudioClip clip)
    {
        if (clip == null) return;

        audioSource.clip = clip;
        audioSource.Play();
    }
}