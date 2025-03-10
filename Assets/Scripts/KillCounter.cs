using UnityEngine;
using UnityEngine.UI;

public class KillCounter : MonoBehaviour
{
    int kills;
    public Text enemyCountText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject); 
    }
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
