using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject skeleton;
    [SerializeField]
    private float skeletonInterval = 3.5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnSekeleton(skeletonInterval, skeleton));
    }

    private IEnumerator spawnSekeleton(float interval, GameObject skeleton){
        yield return new WaitForSeconds(interval);
        GameObject newSkeleton = Instantiate(skeleton, new Vector3(Random.Range(-5f,5), Random.Range(-6f,6f),0), Quaternion.identity);
        StartCoroutine(spawnSekeleton(interval, skeleton));
    }
}
