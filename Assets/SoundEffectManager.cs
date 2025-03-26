using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : Singleton<SoundEffectManager>
{
    private List<AudioSource> audioSources = new List<AudioSource>();
    private GameObject audioSourceContainer;
    private float globalSFXVolume = 1f; // Mặc định 100%

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // Tạo GameObject chứa AudioSource
        audioSourceContainer = new GameObject("AudioSourceContainer");
        audioSourceContainer.transform.SetParent(transform);

        // Thêm một AudioSource mặc định để tránh lỗi
        AudioSource defaultSource = audioSourceContainer.AddComponent<AudioSource>();
        audioSources.Add(defaultSource);
    }

    public void PlaySoundEffect(AudioClip clip, float volume = 1f, bool loop = false)
    {
        if (clip == null)
        {
            Debug.LogWarning("SoundEffectManager: Tried to play a null AudioClip.");
            return;
        }

        AudioSource audioSource = GetAvailableAudioSource();
        if (audioSource == null)
        {
            Debug.LogError("SoundEffectManager: No available AudioSource!");
            return;
        }

        audioSource.clip = clip;
        audioSource.volume = Mathf.Clamp01(volume * globalSFXVolume); // Áp dụng âm lượng toàn cục
        audioSource.loop = loop;
        audioSource.Play();

        if (!loop)
        {
            StartCoroutine(StopAndRecycleAudioSource(audioSource, clip.length));
        }
    }

    public void SetSFXVolume(float value)
    {
        globalSFXVolume = Mathf.Clamp(value / 100f, 0f, 1f); // Chuyển từ 0-100 về 0-1

        foreach (AudioSource source in audioSources)
        {
            if (source.isPlaying)
            {
                source.volume = Mathf.Clamp01(source.volume * globalSFXVolume);
            }
        }

        Debug.Log("SFX Volume Set To: " + globalSFXVolume);
    }

    public float GetSFXVolume()
    {
        return globalSFXVolume * 100f; // Trả về theo khoảng 0-100
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        // Tạo mới AudioSource nếu tất cả đang bận
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
