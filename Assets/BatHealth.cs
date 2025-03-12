using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatHealth : MonoBehaviour, IHealth
{
	public GameObject damageText;
	[SerializeField] private int health = 20;
	private Animator animator;
	private bool isDead = false;

	private SpriteRenderer spriteRenderer;
	private Color originalColor;
	[SerializeField] private float experienceDropChance = 0.7f;
	[SerializeField] private float damageEffectDuration = 0.2f;

	// Thêm biến để chứa prefab của viên kinh nghiệm
	[SerializeField] private GameObject experiencePrefab;
	[SerializeField] private GameObject healthItemPrefab;  

	public float healthDropChance = 0.5f;
	void Start()
	{
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();

		if (spriteRenderer != null)
		{
			originalColor = spriteRenderer.color;
		}
	}

	public void TakeDamage(int amount)
	{
		if (amount < 0)
		{
			throw new System.ArgumentOutOfRangeException("Cannot have negative damage");
		}

		health -= amount;

		if (spriteRenderer != null)
		{
			StartCoroutine(ShowDamageEffect());
		}

		RectTransform textTransform = Instantiate(damageText).GetComponent<RectTransform>();
		textTransform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		Canvas canvas = FindFirstObjectByType<Canvas>();
		textTransform.SetParent(canvas.transform);
		PopUpDamage popup = textTransform.GetComponent<PopUpDamage>();
		if (popup != null)
		{
			popup.textMesh.text = amount.ToString();
		}

		if (health <= 0 && !isDead)
		{
			Die();
		}
	}

	private void Die()
	{
		isDead = true;

		DropExperience();

		DropHealthItem();


		if (animator != null)
		{
			animator.SetTrigger("Death");
			float deathAnimationTime = animator.GetCurrentAnimatorStateInfo(0).length;
			Destroy(gameObject, deathAnimationTime); // Hủy sau khi animation hoàn tất
		}
	}

	public bool IsDead()
	{
		return isDead;
	}

	private IEnumerator ShowDamageEffect()
	{
		spriteRenderer.color = Color.red;
		yield return new WaitForSeconds(damageEffectDuration);
		spriteRenderer.color = originalColor;
	}

	private void DropExperience()
	{
		if (Random.value <= experienceDropChance)
		{
			if (experiencePrefab != null)
			{
				Vector3 spawnPosition = transform.position + new Vector3(0, -1, 0);

				Instantiate(experiencePrefab, spawnPosition, Quaternion.identity);
			}
			else
			{
				Debug.LogError("Experience prefab not assigned in the Inspector.");
			}
		}
		else
		{
			Debug.Log("No experience dropped this time.");
		}
	}

	private void DropHealthItem()
	{
		if (healthItemPrefab != null && Random.value < healthDropChance)
		{
			Vector3 spawnPosition = transform.position + new Vector3(0, -1, 0);
			Instantiate(healthItemPrefab, spawnPosition, Quaternion.identity);
			Debug.Log("Health item dropped!");
		}
	}
}
