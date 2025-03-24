using TMPro.Examples;
using UnityEngine;

public class EntranceArea : MonoBehaviour
{
    [SerializeField] private string transitionName;

    private void Start()
    {
        Debug.Log("SceneTransitionName:" + SceneManagement.Instance.SceneTransitionName);
        if (transitionName == SceneManagement.Instance.SceneTransitionName)
        {
            HeroKnight playerController = HeroKnight.Instance;
            if (playerController != null)
            {
                // Move the player to this entrance's position
                playerController.transform.position = this.transform.position;

                // Set the camera to follow the player
                CameraController.Instance.SetPlayerCameraFollow();
                UIFade.Instance.FadeToClear();
                MonsterSpawner spawner = FindObjectOfType<MonsterSpawner>();
                spawner.StartNewLevel();
                // Assign the player and Timer to MonsterSpawner
                AssignToMonsterSpawner(playerController);
            }
        }
    }

    private void AssignToMonsterSpawner(HeroKnight player)
    {
        // Find MonsterSpawner in the scene
        MonsterSpawner spawner = FindObjectOfType<MonsterSpawner>();
        if (spawner != null)
        {
            // Assign the player to MonsterSpawner
            spawner.player = player.gameObject;
            Debug.Log("Player assigned to MonsterSpawner.");

            // Find the existing Timer from DontDestroyOnLoad
            Timer existingTimer = FindObjectOfType<Timer>();
            if (existingTimer != null)
            {
                // Assign the existing Timer to the MonsterSpawner
                spawner.timer = existingTimer;
                Debug.Log("Existing Timer has been assigned to MonsterSpawner.");
            }
    }
        else
        {
            Debug.LogWarning("MonsterSpawner not found in the scene.");
        }
    }
}
