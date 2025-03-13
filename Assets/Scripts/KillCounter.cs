using UnityEngine;
using UnityEngine.UI;

public class KillCounter : Singleton<KillCounter>
{
    int kills;
    public Text enemyCountText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ShowKill();
    }

    private void ShowKill(){
        enemyCountText.text = kills.ToString();
    }

    public int GetKill(){
        return kills;
    }

    public void AddKill(){
        kills++;
    }
}
