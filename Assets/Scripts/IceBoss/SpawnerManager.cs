using System;
using System.Collections;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [Header("Spawner Attributes")]
    public SwordSpawner[] spawners;  // List of all spawners in the scene
    public float delayBetweenSpawners = 2f;  // Time between each spawner firing
    public event Action OnActionComplete; // Event to notify when the sequence is complete
    public bool IsActionComplete { get; private set; } // Property to track if the action is complete


    public void Run() // Add this method
    {
        IsActionComplete = false; // Reset the action complete status
        StartCoroutine(SpawnerSequence());
    }


    private IEnumerator SpawnerSequence()
    {

        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i].Fire();  // Tell the spawner to fire
            yield return new WaitForSeconds(delayBetweenSpawners);  // Wait for the specified delay before moving to the next spawner
        }
        OnActionComplete?.Invoke();
        IsActionComplete = true; // Set the action complete status
    }
}
