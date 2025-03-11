using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] private SpawnerManager spawnerManager; // Reference to the Swapner Manager
    [SerializeField] private float bossHealth = 100f; // Boss health


    private void Start()
    {
        //spawnerManager.OnActionComplete += HandleSwapnerManagerComplete; // Subscribe to the event
        StartCoroutine(ManageSpawners());
    }

    private IEnumerator ManageSpawners()
    {
        while (bossHealth > 0)
        {
            // Ensure the Spawner Manager is active
            if (!spawnerManager.gameObject.activeInHierarchy)
            {
                spawnerManager.gameObject.SetActive(true);
            }

            // Run the Spawner Manager
            spawnerManager.Run();

            // Wait until the Spawner Manager completes its action
            yield return new WaitUntil(() => spawnerManager.IsActionComplete);

            // Deactivate the Spawner Manager after it completes its action
            spawnerManager.gameObject.SetActive(false);
            // Ensure the SnowballSpawner is active

        }
    }
}
