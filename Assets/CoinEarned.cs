using TMPro; // Import TextMeshPro namespace
using UnityEngine;

public class CoinEarned : MonoBehaviour
{
    private TextMeshProUGUI coinEarnedText;
    private int earnedCoins;
    private CoinCounter coinCounter;

    void Start()
    {
        coinEarnedText = GetComponent<TextMeshProUGUI>();
        if (coinEarnedText != null)
        {
            coinEarnedText.text = "0";
        }

        coinCounter = FindObjectOfType<CoinCounter>();
        if (coinCounter == null)
        {
            Debug.LogError("Coin not found in the scene! Please ensure a Coin component exists in the canvas.");
        }
    }

    public void Update()
    {
        if (coinCounter != null)
        {
            earnedCoins = coinCounter.GetCoins();

            if (coinEarnedText != null)
            {
                coinEarnedText.text = $"{earnedCoins}";
            }
        }
    }
}
