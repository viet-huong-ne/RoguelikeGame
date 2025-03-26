using UnityEngine;

public class ImpalerMovement : MonoBehaviour
{
	[SerializeField] private EnemyScriptableObject enemyData; // ScriptableObject containing enemy stats
	[SerializeField] private Animator animator; // Animator to control enemy animations

	private Transform player;
	private bool canMove = true; // Flag to control movement

	private void Start()
	{
		HeroKnight heroKnight = FindObjectOfType<HeroKnight>();
		if (heroKnight != null)
		{
			player = heroKnight.transform;
		}
		else
		{
			Debug.LogError("No HeroKnight found in the scene.");
		}
	}

	private void Update()
	{
		if (player == null || !canMove) return; // Exit if no player found or movement is disabled

		MoveTowardsPlayer();
	}

	public void MoveTowardsPlayer()
    {
        if (player == null || !canMove) return;
        //Debug.Log($"Current Move Speed: {enemyData.moveSpeed}");

        // Move towards the player
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, enemyData.moveSpeed * Time.deltaTime);

        // Flip the enemy to face the player
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // Update animator's speed parameter
        //if (animator != null)
        //{
        //    animator.SetFloat("Speed", enemyData.moveSpeed);
        //}
    }

	// Public method to stop movement
	public void StopMovement()
	{
		Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
		if (rigidbody2D != null)
		{
			rigidbody2D.linearVelocity = Vector2.zero; // Stop all velocity
		}
		Debug.Log("STOP");
		canMove = false;
	}

	public void ResumeMovement()
	{
		canMove = true;

		Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
		if (rigidbody2D != null)
		{
			rigidbody2D.linearVelocity = Vector2.zero;
		}

		Debug.Log("RESUME");
	}

	// Optionally set the player target dynamically
	public void SetPlayer(GameObject playerObject)
	{
		player = playerObject.transform;
	}
}

