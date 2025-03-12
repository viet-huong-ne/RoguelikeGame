using UnityEngine;

public class SwordBullet : MonoBehaviour
{
    public float bulletLife = 1f;  // Defines how long before the bullet is destroyed
    public float speed = 5f;        // Speed of the bullet
    private Vector2 spawnPoint;     // Where the bullet was spawned
    private float timer = 0f;       // Timer to track bullet lifetime
    private Vector2 direction;       // Direction of the bullet movement

    [SerializeField] private GameObject VFX;
    [SerializeField] private int damageAmount;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = transform.position; // Store the initial spawn point
        direction = transform.right; // Set the initial direction based on rotation
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > bulletLife)
            Destroy(this.gameObject);

        timer += Time.deltaTime;
        transform.position += (Vector3)(direction * speed * Time.deltaTime); // Move bullet
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            // Instantiate the VFX at the collision point
            HeroHealth playerHealth = col.gameObject.GetComponent<HeroHealth>();
            //Instantiate(VFX, transform.position, Quaternion.identity);
            Destroy(this.gameObject); // Optionally destroy the bullet
            playerHealth.TakeDamage(damageAmount);

        }

    }
}
