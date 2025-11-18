using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void SetEmiter(AudioClip clip, float volume = 1.0f, bool loop = false)
    {
        if (clip == null)
        {
            Destroy(gameObject);
            return;
        }

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.spatialBlend = 0.98f;

    }

    public void PlaySoundAndDie(AudioClip clip, float volume = 1.0f)
    {
        if (clip == null)
        {
            Destroy(gameObject);
            return;
        }

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = false;
        audioSource.spatialBlend = 0.9f;


        audioSource.Play();
        Destroy(gameObject, clip.length + 0.1f);
    }

    public void StopSound()
    {
        Destroy(gameObject);
    }

    public void PauseSound()
    {
       if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    public void ResumeSound()
    {
        if (audioSource.clip != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void ReplaySound()
    {
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
    }
}