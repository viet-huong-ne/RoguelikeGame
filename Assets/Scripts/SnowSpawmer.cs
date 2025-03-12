using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class SnowSpawmer : MonoBehaviour
{
    public ParticleSystem snowParticle;
    public float spawnInterval = 5f;
    private float slowSpeed = 2f;
    private float normalSpeed = 5f;
    private bool isSnowing = false;
    public HeroKnight player;
    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<HeroKnight>();
        }
        //SpawnSnow();
        InvokeRepeating("SpawnSnow", 0f, spawnInterval);
    }

    void SpawnSnow()
    {
        snowParticle.Play(); // Chạy hiệu ứng tuyết
        isSnowing = true;
        AdjustPlayerSpeed();

        // Dừng tuyết sau 3 giây (có thể chỉnh)
        Invoke("StopSnow", 5f);

    }
    void StopSnow()
    {        
        snowParticle.Stop();
        isSnowing = false;
        AdjustPlayerSpeed();
    }

    void AdjustPlayerSpeed()
    {
        if (player != null)
        {
            Debug.Log("isSnowing:" + isSnowing);
            player.SetSpeed(isSnowing ? slowSpeed : normalSpeed);
        }
    }
}
