using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    Transform player;
    private EnemyStats stats;
    public GameObject hero;
    void Start()
    {
        player = FindObjectOfType<HeroKnight>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, enemyData.moveSpeed * Time.deltaTime);
    }
}
