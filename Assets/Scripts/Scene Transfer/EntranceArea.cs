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
            }
        }
    }
}
