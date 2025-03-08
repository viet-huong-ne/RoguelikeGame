using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public GameObject Player;

    private void Awake()
    {
        // Đảm bảo player tồn tại qua các scene
        DontDestroyOnLoad(this.gameObject);
    }
}
