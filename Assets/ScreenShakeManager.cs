using System.Collections;
using Cinemachine;
using UnityEngine;

public class ScreenShakeManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noise;
    private float shakeTimer;

    private void Start()
    {
        if (virtualCamera != null)
        {
            noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }

    public void TriggerShake(float amplitude, float frequency, float duration)
    {
        if (noise != null)
        {
            noise.m_AmplitudeGain = amplitude;
            noise.m_FrequencyGain = frequency;
            shakeTimer = duration;
            StartCoroutine(StopShakeAfterDuration());
        }
    }

    private IEnumerator StopShakeAfterDuration()
    {
        yield return new WaitForSeconds(shakeTimer);
        if (noise != null)
        {
            noise.m_AmplitudeGain = 0f;
            noise.m_FrequencyGain = 0f;
        }
    }
}
