using System;
using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on BackgroundMusicController!");
        }
    }

    public void ChangeMusic(AudioClip newClip)
    {
        Debug.Log("BackgroundMusicController: Attempting to change music...");
        if (audioSource == null)
        {
            Debug.LogError("AudioSource is null!");
            return;
        }

        if (newClip == null)
        {
            Debug.LogError("New AudioClip is null!");
            return;
        }

        audioSource.clip = newClip;
        audioSource.Play();
        Debug.Log("Music successfully changed to: " + newClip.name);
    }

    public void StopMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
