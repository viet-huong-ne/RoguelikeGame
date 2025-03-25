using System.Collections;
using UnityEngine;

public class ImpalerAttackArea : MonoBehaviour
{
	public EnemyScriptableObject enemyData;
	[SerializeField] private float currentDamage;
	private bool canDamage = true;
	private ImpalerStats bossStats;

	void Awake()
	{
		currentDamage = enemyData.Damage;
		bossStats = GetComponentInParent<ImpalerStats>();
	}

	protected virtual void OnTriggerStay2D(Collider2D col)
	{
		if (col.CompareTag("Player") && canDamage)
		{
			HeroHealth hero = col.GetComponent<HeroHealth>();
			if (hero != null)
			{
				hero.TakeDamage((int)currentDamage);
			}

			StartCoroutine(DamageCooldown());

			// Ensure boss only attacks when player is in range
			//if (!bossStats.IsAttacking)
			//{
			//	bossStats.PerformAttack();
			//}
		}
	}

	private IEnumerator DamageCooldown()
	{
		canDamage = false;
		yield return new WaitForSeconds(2f);
		canDamage = true;
	}
}
