using UnityEngine;

public class UICanvasManager : MonoBehaviour
{
    private void Awake()
    {
        GameObject[] allCanvases = GameObject.FindGameObjectsWithTag("Canvas"); // Tìm tất cả Canvas
        foreach (GameObject canvas in allCanvases)
        {
            if (canvas != this.gameObject) // Nếu không phải Canvas của DontDestroyOnLoad
            {
                Destroy(canvas); // Xóa Canvas thừa
            }
        }

        DontDestroyOnLoad(gameObject); // Giữ lại Canvas này
    }
}
