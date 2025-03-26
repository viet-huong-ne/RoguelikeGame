using System.Collections;
using UnityEngine;

public class ImpalerAttackArea : MonoBehaviour
{
	public EnemyScriptableObject enemyData;
	[SerializeField] private float currentDamage;
	private bool canDamage = true;
	private ImpalerStats bossStats;
	public Vector3 attackOffset;

	void Awake()
	{
		currentDamage = enemyData.Damage;
		bossStats = GetComponentInParent<ImpalerStats>();
	}

	protected virtual void OnTriggerStay2D(Collider2D col)
	{
		if (col.CompareTag("Player") && canDamage)
		{
			//HeroHealth hero = col.GetComponent<HeroHealth>();
			//if (hero != null)
			//{
			//	hero.TakeDamage((int)currentDamage);
			//}

			StartCoroutine(DamageCooldown());

			bossStats.TriggerAttackAnimation();
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
		float radius = 10f; // Adjust based on attack range
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
				heroKnight.Stun();
				StartCoroutine(DamageCooldown()); 
			}
		}
	}


	// Debug to visualize the OverlapCircle
	public void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, 2f);
	}
}
