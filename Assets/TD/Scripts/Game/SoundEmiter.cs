using UnityEngine;
using System.Collections;

// Este script vai tocar um som e se autodestruir
[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        // Garante que o som não toque assim que for criado
        audioSource.playOnAwake = false;
    }

    // Método público para iniciar o som
    public void PlaySound(AudioClip clip, float volume = 1.0f)
    {
        if (clip == null)
        {
            Destroy(gameObject);
            return;
        }

        // Atribui as configurações
        audioSource.clip = clip;
        audioSource.volume = volume;

        // Toca o som
        audioSource.Play();

        // Agenda a destruição do objeto para quando o som terminar
        // Adicionamos 0.1s de margem para garantir
        Destroy(gameObject, clip.length + 0.1f);
    }
}