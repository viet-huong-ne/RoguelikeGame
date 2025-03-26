using System.Collections;
using UnityEngine;

public class ImpalerBossAI : MonoBehaviour
{
	[SerializeField] private Animator animator;
	[SerializeField] private float attackCooldown = 2f;
	[SerializeField] private float attackRange = 2f;
	[SerializeField] private int maxAttacks = 6;
	private Transform player;
	private bool isAttacking = false;
	private ImpalerMovement impalerMovement;

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		impalerMovement = GetComponent<ImpalerMovement>();
		StartCoroutine(AttackRoutine());
	}

	private IEnumerator AttackRoutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(attackCooldown);
			if (player == null || animator.GetBool("IsDead")) yield break;

			float distance = Vector2.Distance(transform.position, player.position);
			if (distance <= attackRange && !isAttacking)
			{
				StartCoroutine(PerformAttack());
			}
		}
	}

	private IEnumerator PerformAttack()
	{
		isAttacking = true;
		impalerMovement.StopMovement();

		int attackID = Random.Range(1, maxAttacks + 1);
		animator.SetInteger("AttackID", attackID);
		yield return new WaitForSeconds(1.5f); 

		animator.SetInteger("AttackID", 0);
		impalerMovement.ResumeMovement();
		isAttacking = false;
	}
}
