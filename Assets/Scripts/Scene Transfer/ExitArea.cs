using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitArea : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;

    private float waitToLoadTime = 1f;
    private bool isTransitioning = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Va chạm với: " + other.gameObject.name + " | Layer: " + other.gameObject.layer);
        HeroKnight playerController = other.gameObject.GetComponent<HeroKnight>();
        Debug.Log("nhân vật:" + playerController);
        if (playerController != null)
        {
            //isTransitioning = true;
            // Set the transition name in the scene management system
            SceneManagement.Instance.SetTransitionName(sceneTransitionName);
            UIFade.Instance.FadeToBlack();
            StartCoroutine(LoadSceneRoutine());
        }
        //if (other.CompareTag("Player")) // Kiểm tra nếu nhân vật chạm vào portal
        //{
        //    SceneManager.LoadScene(sceneToLoad); // Chuyển sang màn khác
        //}
    }
    private IEnumerator LoadSceneRoutine()
    {
        while (waitToLoadTime >= 0f)
        {
            waitToLoadTime -= Time.deltaTime;
            yield return null;
        }
        Debug.Log("Đang load scene: " + sceneToLoad);
        SceneManager.LoadScene(sceneToLoad);
    }
}
