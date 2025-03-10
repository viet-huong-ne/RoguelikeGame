using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class EnemiesDefeated : MonoBehaviour
{
    private TextMeshProUGUI enemiesDefeatedText;
    private int enemiesDefeated;
    private KillCounter killCounter;

    void Start()
    {
        enemiesDefeatedText = GetComponent<TextMeshProUGUI>();
        if (enemiesDefeatedText != null)
        {
            enemiesDefeatedText.text = "0";
        }
        // Tìm Timer từ canvas trong scene
        killCounter = FindObjectOfType<KillCounter>();
    }

    public void Update()
    {
        if (killCounter != null)
        {
            enemiesDefeated = killCounter.GetKill();

            if (enemiesDefeatedText != null)
            {
                enemiesDefeatedText.text = $"{enemiesDefeated}";
            }
        }
    }
}
