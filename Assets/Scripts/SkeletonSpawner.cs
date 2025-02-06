using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject skeletonPrefab;
    [SerializeField]
    private GameObject player;  // Reference to the player
    [SerializeField]
    private float skeletonInterval = 3.5f;
    [SerializeField]
    private int maxSkeletons = 20;

    private Queue<GameObject> skeletonPool = new Queue<GameObject>();

    void Start()
    {
        if (skeletonPrefab == null || player == null)
        {
            Debug.LogError("Skeleton prefab or player is not assigned in the Inspector!");
            return;
        }

        for (int i = 0; i < maxSkeletons; i++)
        {
            GameObject skeleton = Instantiate(skeletonPrefab);
            skeleton.SetActive(false);
            skeletonPool.Enqueue(skeleton);
        }

        StartCoroutine(SpawnSkeletons(skeletonInterval));
    }

    private IEnumerator SpawnSkeletons(float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            if (skeletonPool.Count > 0)
            {
                GameObject skeleton = skeletonPool.Dequeue();
                skeleton.transform.position = new Vector3(Random.Range(-5f, 5f), Random.Range(-6f, 6f), 0);
                skeleton.SetActive(true);

                // Assign the player reference to the skeleton's AIChase script
                skeleton.GetComponent<AIChase>().SetPlayer(player);
            }
        }
    }

    public void ReturnSkeletonToPool(GameObject skeleton)
    {
        skeleton.SetActive(false);
        skeletonPool.Enqueue(skeleton);
    }
}
