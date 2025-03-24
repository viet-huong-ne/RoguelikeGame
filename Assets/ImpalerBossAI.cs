using System.Collections;
using UnityEngine;

public class ImpalerBossAI : MonoBehaviour
{
	public Animator animator;
	public Transform player;
	public float attackRange = 2f;
	public float moveSpeed = 3f;
	public int health = 100;

	private bool isAttacking;
	private bool isCountering;

	void Update()
	{
		if (health <= 0) return;

		float distance = Vector2.Distance(transform.position, player.position);
		if (distance <= attackRange && !isAttacking)
		{
			ChooseAttack();
		}
		else
		{
			MoveTowardsPlayer();
		}
	}

	void MoveTowardsPlayer()
	{
		if (isAttacking) return;
		transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
	}

	void ChooseAttack()
	{
		isAttacking = true;
		int attackType = Random.Range(1, 7); // 1-5 melee, 6 is counter

		if (attackType <= 5)
		{
			StartCoroutine(MeleeAttack(attackType));
		}
		else
		{
			StartCoroutine(CounterAttack());
		}
	}

	IEnumerator MeleeAttack(int attackType)
	{
		animator.SetTrigger($"attack{attackType}");
		yield return new WaitForSeconds(1f);
		isAttacking = false;
	}

	IEnumerator CounterAttack()
	{
		isCountering = true;
		animator.SetTrigger("counter");
		yield return new WaitForSeconds(1.5f);
		isCountering = false;
		isAttacking = false;
	}

	public void TakeDamage(int damage)
	{
		if (health <= 0) return;

		health -= damage;
		animator.SetTrigger("Hit");

		if (health <= 0)
		{
			StartCoroutine(Die());
		}
	}

	IEnumerator Die()
	{
		animator.SetTrigger("Death");
		yield return new WaitForSeconds(2f);
		Destroy(gameObject);
	}
}
