using NUnit.Framework;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    public static SoundHandler Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
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