using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager Instance;

    private List<AudioSource> audioSources = new List<AudioSource>();
    private GameObject audioSourceContainer;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }

        // Create a container for all sound effect sources
        audioSourceContainer = new GameObject("AudioSourceContainer");
        audioSourceContainer.transform.parent = transform;
    }

    public void PlaySoundEffect(AudioClip clip, float volume = 1f, bool loop = false)
    {
        if (clip == null)
        {
            Debug.LogWarning("SoundEffectManager: Tried to play a null AudioClip.");
            return;
        }

        // Get or create an AudioSource
        AudioSource audioSource = GetAvailableAudioSource();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.Play();

        // If the sound is not looping, stop and recycle the AudioSource after it finishes
        if (!loop)
        {
            StartCoroutine(StopAndRecycleAudioSource(audioSource, clip.length));
        }
    }

    public void StopAllSoundEffects()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    private AudioSource GetAvailableAudioSource()
    {
        // Find an available (idle) AudioSource
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        // If no available source, create a new one
        AudioSource newSource = audioSourceContainer.AddComponent<AudioSource>();
        audioSources.Add(newSource);
        return newSource;
    }

    private IEnumerator StopAndRecycleAudioSource(AudioSource audioSource, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!audioSource.loop)
        {
            audioSource.Stop();
            audioSource.clip = null;
        }
    }
}
