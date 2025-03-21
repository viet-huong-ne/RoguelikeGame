using UnityEngine;
using UnityEngine.UI;

public class CoinCounter : Singleton<CoinCounter>
{
    int coins;
    public Text coinCountText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        ShowCoin();
    }

    private void ShowCoin(){
        coinCountText.text = coins.ToString();
    }

    public void AddCoin(){
        coins++;
    }

    public void AddSackOfGold(){
        coins += 10;
    }

    public int GetCoins()
    {
        return coins;
    }
}
