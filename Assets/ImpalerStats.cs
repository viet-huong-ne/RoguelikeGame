using System.Collections;
using UnityEngine;

public class ImpalerStats : MonoBehaviour
{
	public EnemyScriptableObject enemyData;
	private BODMovement bodMovement;
	[SerializeField] private float damageCooldown = 2f;  // Set your cooldown duration
	public GameObject hero;
	public GameObject damageText;
	private Color originalColor;
	[SerializeField] private float currentMoveSpeed;
	[SerializeField] private float currentHealth;
	[SerializeField] private float currentDamage;
	[SerializeField] private float damageEffectDuration = 0.2f;
	private SpriteRenderer spriteRenderer;
	[SerializeField] private KillCounter killCounter;
	[SerializeField] public GameObject keyPrefab;
	private int dropCount = 0;
	private Animator animator;
	private bool isDead = false;

	void Start()
	{
		bodMovement = GetComponent<BODMovement>();
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		killCounter = GameObject.Find("KCO").GetComponent<KillCounter>();

		if (spriteRenderer != null)
		{
			originalColor = spriteRenderer.color;
		}

		// Change background music
		GameObject backgroundMusicObject = GameObject.Find("BackgroundMusicManager");
		if (backgroundMusicObject != null)
		{
			BackgroundMusicController musicController = backgroundMusicObject.GetComponent<BackgroundMusicController>();
			if (musicController != null)
			{
				// Correct Resources.Load path
				AudioClip newMusicClip = Resources.Load<AudioClip>("Music/BOD_Theme");
				if (newMusicClip != null)
				{
					musicController.ChangeMusic(newMusicClip);
				}
				else
				{
					Debug.LogWarning("New music clip not found in Resources/Music/BOD_Theme!");
				}
			}
			else
			{
				Debug.LogWarning("BackgroundMusicController component not found on BackgroundMusic object!");
			}
		}
		else
		{
			Debug.LogWarning("BackgroundMusic object not found in the scene!");
		}
	}

	void Awake()
	{
		currentMoveSpeed = enemyData.MoveSpeed;
		currentDamage = enemyData.Damage;
		currentHealth = enemyData.MaxHealth;
	}

	public void TakeDamage(float dmg)
	{
		currentHealth -= dmg;
		StartCoroutine(ShowDamageEffect());
		if (currentHealth <= 0)
		{
			Die();
		}
		RectTransform textTransform = Instantiate(damageText).GetComponent<RectTransform>();
		textTransform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		Canvas canvas = GameObject.FindObjectOfType<Canvas>();
		textTransform.SetParent(canvas.transform);
		PopUpDamage popup = textTransform.GetComponent<PopUpDamage>();
		if (popup != null)
		{
			popup.textMesh.text = dmg.ToString();
		}
	}

	private IEnumerator ShowDamageEffect()
	{
		spriteRenderer.color = Color.red;
		yield return new WaitForSeconds(damageEffectDuration);
		spriteRenderer.color = originalColor;
	}

	public void TriggerAttackAnimation()
	{
		if (isDead) return;
		int attackID = Random.Range(1, 7); 
		animator.SetInteger("AttackID", attackID);
		StartCoroutine(ResetAttack());
	}

	private IEnumerator ResetAttack()
	{
		yield return new WaitForSeconds(1.5f);
		animator.SetInteger("AttackID", 0);
	}
	public void Die()
	{
		SoundEffectManager.Instance.PlaySoundEffect(Resources.Load<AudioClip>("SoundEffects/BossDeath"), 1f);
		if (isDead) return; // Prevent multiple executions
		isDead = true;
		bodMovement.StopMovement();
		currentMoveSpeed = 0;
		animator.SetTrigger("Death");
		float deathAnimationTime = animator.GetCurrentAnimatorStateInfo(0).length;
		Rigidbody2D rb = GetComponent<Rigidbody2D>();
		if (rb != null)
		{
			rb.linearVelocity = Vector2.zero;
			rb.isKinematic = true;
		}
		DropKey(keyPrefab);

		killCounter.AddKill();
		Destroy(gameObject, deathAnimationTime);
	}

	private void DropKey(GameObject prefab)
	{
		if (prefab != null)
		{
			// Calculate spawn position with an offset to avoid overlap
			float offsetX = 0.5f * (dropCount % 3) - 0.5f;
			float offsetY = -0.5f * (dropCount / 3);

			Vector3 spawnPosition = transform.position + new Vector3(offsetX, offsetY, 0);

			Instantiate(prefab, spawnPosition, Quaternion.identity);

			dropCount++;
		}
		else
		{
			Debug.LogError("Prefab not assigned in the Inspector.");
		}
	}
}
