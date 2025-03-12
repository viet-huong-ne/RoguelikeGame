using TMPro; // Import TextMeshPro namespace
using UnityEngine;

public class SurvivedTime : MonoBehaviour
{
    private TextMeshProUGUI survivedTimeText;
    private Timer timerPrefab;

    void Start()
    {
        survivedTimeText = GetComponent<TextMeshProUGUI>();

        if (survivedTimeText != null)
        {
            survivedTimeText.text = "0:00";
        }

        // Tìm Timer từ canvas trong scene
        timerPrefab = FindObjectOfType<Timer>();
        if (timerPrefab == null)
        {
            Debug.LogError("Timer not found in the scene! Please ensure a Timer component exists in the canvas.");
        }
    }

    void Update()
    {
        if (timerPrefab != null && survivedTimeText != null)
        {
            float elapsedTime = timerPrefab.GetElapsedTime();

            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);

            survivedTimeText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}
