using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SFXSliderScript : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI text;

    void Start()
    {
        SoundEffectManager sfxManager = FindObjectOfType<SoundEffectManager>();
        if (sfxManager != null)
        {
            slider.value = sfxManager.GetSFXVolume(); // Lấy volume hiện tại (0-100)
        }
        else
        {
            Debug.LogError("SoundEffectManager NOT found!");
        }

        text.text = slider.value.ToString("0");

        slider.onValueChanged.AddListener((v) =>
        {
            text.text = v.ToString("0"); // Hiển thị số nguyên
            Debug.Log("Slider value changed to: " + v);
            sfxManager?.SetSFXVolume(v);
        });
    }
}
