using NUnit.Framework;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    public static SoundHandler Instance { get; private set; }

    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource UIAudioSource;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SetupAudioSources()
    {

        musicAudioSource.loop = true;
        musicAudioSource.spatialBlend = 0f;
        musicAudioSource.playOnAwake = false;

        UIAudioSource.loop = false;
        UIAudioSource.spatialBlend = 0f; 
        UIAudioSource.playOnAwake = false;
    }

    public void PlayMusic(AudioClip clip, float volume = 0.5f)
    {
        if (clip == null) return;

        if (musicAudioSource.isPlaying && musicAudioSource.clip == clip) return;

        musicAudioSource.Stop();
        musicAudioSource.clip = clip;
        musicAudioSource.volume = volume;
        musicAudioSource.Play();
    }

    public void StopMusic()
    {
        musicAudioSource.Stop();
    }


    public void PlayUISound(AudioClip clip, float volume = 1.0f)
    {
        if (clip == null) return;
        UIAudioSource.PlayOneShot(clip, volume);
    }

    public SoundEmitter PlaySoundAtPosition(AudioClip clip, Vector3 passedPosition, float volume = 1.0f, bool loop = false, bool singleExecution = false, Transform parent = null)
    {
        if (clip == null) return null;

        GameObject soundEmitterObject = new GameObject("SoundEmitter");
        soundEmitterObject.transform.position = passedPosition;

        if (parent != null)
        {
            soundEmitterObject.transform.SetParent(parent);
        }

        SoundEmitter soundEmitter = soundEmitterObject.AddComponent<SoundEmitter>();

        if(singleExecution)
        {
            soundEmitter.PlaySoundAndDie(clip, volume);
            return soundEmitter;
        }

        soundEmitter.SetEmiter(clip, volume, loop);

        return soundEmitter;
    }
}