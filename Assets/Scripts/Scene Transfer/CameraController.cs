using Cinemachine;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    public void SetPlayerCameraFollow()
    {
        // Find the Cinemachine camera and set it to follow the player
        cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        if (cinemachineVirtualCamera != null && HeroKnight.Instance != null)
        {
            cinemachineVirtualCamera.Follow = HeroKnight.Instance.transform;
        }
    }
}
