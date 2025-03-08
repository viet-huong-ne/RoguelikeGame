using UnityEngine;
using UnityEngine.UI;

public class Timer : Singleton<Timer>
{
    public Text timerText;
    private float elapsedTime = 0f; 
    private bool isRunning = true;

    void Update()
    {
        if (isRunning)
        {
            // Increment elapsed time
            elapsedTime += Time.deltaTime;

            // Format the time as minutes and seconds
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);

            // Update the text UI
            timerText.text = $"{minutes:0}:{seconds:00}";
        }        
            DontDestroyOnLoad(this.gameObject);
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        timerText.text = "00:00";
    }
}
