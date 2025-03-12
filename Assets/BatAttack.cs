using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAttack : MonoBehaviour
{
	[SerializeField] private int damage = 10;
	[SerializeField] private float damageCooldown = 1f;

	private bool canDamage = true;      // Kiểm soát trạng thái sẵn sàng gây sát thương

	public void TryDealDamage(GameObject target)
	{
		if (canDamage)
		{
			StartCoroutine(DealDamageCoroutine(target));
		}
	}

	private IEnumerator DealDamageCoroutine(GameObject target)
	{
		// Gây sát thương nếu đối tượng có "PlayerHealth"
		HeroHealth heroHealth = target.GetComponent<HeroHealth>();
		if (heroHealth != null)
		{
			heroHealth.TakeDamage(damage);
		}

		// Chờ thời gian hồi
		canDamage = false;
		yield return new WaitForSeconds(damageCooldown);
		canDamage = true;
	}
}
