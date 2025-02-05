using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChase : MonoBehaviour
{
    public GameObject player;          
    public float speed;                
    public float stopDistance = 1.5f;   
    public float attackDistance = 1.5f;
    public float attackCooldown = 1f;
    public Animator animator;

    private float distance;
    private bool isAttacking = false;

    void Update()
    {
        // Tính khoảng cách giữa quái và người chơi
        distance = Vector2.Distance(transform.position, player.transform.position);

        // Nếu quái nằm ngoài khoảng cách tấn công
        if (distance > attackDistance)
        {
            isAttacking = false;
            animator.SetBool("IsReadyToAttack", false);

            // Nếu khoảng cách lớn hơn stopDistance, quái tiếp tục di chuyển
            if (distance > stopDistance)
            {
                MoveTowardsPlayer();
            }
        }
        else
        {
            // Nếu quái trong phạm vi tấn công
            if (!isAttacking)
            {
                isAttacking = true;
                StartCoroutine(AttackPlayer());
            }
        }
    }

    void MoveTowardsPlayer()
    {
        // Di chuyển quái về phía người chơi
        Vector2 direction = (player.transform.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        // Lật trái/phải dựa trên hướng của người chơi
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // Cập nhật tham số Speed cho Animator
        animator.SetFloat("Speed", speed);
    }

    IEnumerator AttackPlayer()
    {
        while (isAttacking)
        {
            // Tấn công người chơi
            Debug.Log("Enemy attacks the player!");
            animator.SetBool("IsReadyToAttack", true);

            // Gây sát thương (nếu có logic gây sát thương)
            // player.GetComponent<PlayerHealth>().TakeDamage(damage);

            // Chờ thời gian hồi chiêu
            yield return new WaitForSeconds(attackCooldown);
        }

        // Reset trạng thái Animator khi dừng tấn công
        animator.SetBool("IsReadyToAttack", false);
    }
}
