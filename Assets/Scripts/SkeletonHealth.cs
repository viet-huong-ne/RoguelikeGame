using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonHealth : MonoBehaviour, IHealth
{
    [SerializeField] private int health = 20;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int amount)
    {
        if(amount <0){
            throw new System.ArgumentOutOfRangeException("Cannot have negative damage");
        }

        this.health -= amount;

        if(health <= 0){
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Skeleton is dead!");
        Destroy(gameObject);
    }
}
