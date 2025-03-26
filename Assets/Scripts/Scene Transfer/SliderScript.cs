using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicSliderScript : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Đảm bảo slider nhận giá trị volume ban đầu
        if (slider != null)
        {
            AudioSource bgMusic = FindObjectOfType<BackgroundMusicController>()?.GetComponent<AudioSource>();
            if (bgMusic != null)
            {
                slider.value = bgMusic.volume * 100f; // Chuyển từ khoảng 0-1 về 0-100
            }
        }

        // Lắng nghe sự kiện thay đổi giá trị của slider
        slider.onValueChanged.AddListener((v) =>
        {
            text.text = v.ToString("0");
            Debug.Log("Slider value changed to: " + v);
            FindObjectOfType<BackgroundMusicController>()?.SetMusicVolume(v);
        });

        // Cập nhật giá trị hiển thị ban đầu
        text.text = slider.value.ToString("0");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
