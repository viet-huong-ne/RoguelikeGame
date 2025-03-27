using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel;
    private bool isMuted = false;
    
    public void PlayClickSound()
    {
        SoundEffectManager.Instance.PlaySoundEffect(Resources.Load<AudioClip>("SoundEffects/Click"), 1f);
    }

    public void Pause()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Continue()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void LoadMainMenu()
    {
        // Load scene ngay lập tức
        SceneManager.LoadScene("MenuScene");

        // Khi scene mới đã load, tiến hành hủy các đối tượng
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Hàm chạy sau khi scene mới đã load
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DestroyDontDestroyOnLoadObjects();
        Destroy(gameObject);

        // Gỡ bỏ sự kiện để tránh gọi lại sau này
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Time.timeScale = 1f;
    }

    private void DestroyDontDestroyOnLoadObjects()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.scene.name == "DontDestroyOnLoad")
            {
                Destroy(obj);
            }
        }

        Debug.Log("All DontDestroyOnLoad objects have been destroyed.");
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0 : 1;

        Debug.Log("Audio " + (isMuted ? "Muted" : "Unmuted"));
    }
}
