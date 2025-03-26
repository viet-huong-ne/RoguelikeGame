using UnityEngine;

using System.Collections;
using UnityEngine;

public class BatAttackArea : MonoBehaviour
{
	public EnemyScriptableObject enemyData;
	[SerializeField] private float currentDamage;
	private bool canDamage = true;
	private BatStats batStats;
	public Vector3 attackOffset;

	void Awake()
	{
		currentDamage = enemyData.Damage;
		batStats = GetComponentInParent<BatStats>();
	}

	protected virtual void OnTriggerStay2D(Collider2D col)
	{
		if (col.CompareTag("Player") && canDamage)
		{


			StartCoroutine(DamageCooldown());

			batStats.TriggerAttackAnimation();
		}
	}

	private IEnumerator DamageCooldown()
	{
		canDamage = false;
		yield return new WaitForSeconds(0.3f);
		canDamage = true;
	}

	public void PerformDamage(float damageAmount)
	{
		float radius = 1f; // Adjust based on attack range
		Vector3 pos = transform.position;
		pos += transform.right * attackOffset.x;
		pos += transform.up * attackOffset.y;

		Collider2D col = Physics2D.OverlapCircle(
			pos,
			radius,
			LayerMask.GetMask("Player")
		);
		Debug.Log(col == null);
		if (col != null && canDamage)
		{
			HeroHealth hero = col.GetComponent<HeroHealth>();
			HeroKnight heroKnight = col.GetComponent<HeroKnight>();
			if (hero != null)
			{
				hero.TakeDamage((int)damageAmount);
				StartCoroutine(DamageCooldown());
			}
		}
	}


	public void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, 2f);
	}
}

