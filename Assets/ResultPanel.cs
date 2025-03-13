using UnityEngine;
using UnityEngine.SceneManagement;
public class ResultPanel : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Change background music
        GameObject backgroundMusicObject = GameObject.Find("BackgroundMusicManager");
        if (backgroundMusicObject != null)
        {
            BackgroundMusicController musicController = backgroundMusicObject.GetComponent<BackgroundMusicController>();
            if (musicController != null)
            {
                // Correct Resources.Load path
                AudioClip newMusicClip = Resources.Load<AudioClip>("Music/GameOver_Theme");
                if (newMusicClip != null)
                {
                    musicController.ChangeMusic(newMusicClip);
                }
                else
                {
                    Debug.LogWarning("New music clip not found in Resources/Music/GameOver_Theme!");
                }
            }
            else
            {
                Debug.LogWarning("BackgroundMusicController component not found on BackgroundMusic object!");
            }
        }
        else
        {
            Debug.LogWarning("BackgroundMusic object not found in the scene!");
        }
    }

    public void LoadMainMenu()
    {
        // Hủy tất cả các đối tượng DontDestroyOnLoad trước khi chuyển sang MenuScene
        DestroyDontDestroyOnLoadObjects();
        Destroy(gameObject);
        // Tải scene chính
        SceneManager.LoadScene("MenuScene");
    }

    private void DestroyDontDestroyOnLoadObjects()
    {
        // Tìm tất cả các đối tượng
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // Kiểm tra nếu đối tượng nằm trong "DontDestroyOnLoad"
            if (obj.scene.name == "DontDestroyOnLoad")
            {
                Destroy(obj);
            }
        }

        Debug.Log("All DontDestroyOnLoad objects have been destroyed.");
    }
}
