using UnityEngine;

public class SwordSpawner : MonoBehaviour
{
    enum SpawnerType { Straight, Spin }

    [Header("Bullet Attributes")]
    public GameObject bullet;
    public float bulletLife = 1f;
    public float speed = 1f;

    [Header("Spawner Attributes")]
    [SerializeField] private SpawnerType spawnerType;
    [SerializeField] private float attackRange;   // Attack range for shooting

    private GameObject spawnedBullet;

    // Start is called before the first frame update
    void Start()
    {
        // You can add initialization logic if necessary
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnerType == SpawnerType.Spin)
        {
            transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z + 1f);
        }
    }

    // Fire method is public so the SpawnerManager can call it
    public void Fire()
    {
        Vector2 playerPosition = HeroKnight.Instance.transform.position; // Get the player's position
        Vector2 targetPosition = new Vector2(playerPosition.x, playerPosition.y - 0.5f); // Adjust to center (modify offset as needed)

        if (Vector2.Distance(transform.position, targetPosition) <= attackRange)
        {
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            Quaternion bulletRotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            spawnedBullet = Instantiate(bullet, transform.position, bulletRotation);
            spawnedBullet.GetComponent<SwordBullet>().speed = speed;
            spawnedBullet.GetComponent<SwordBullet>().bulletLife = bulletLife;
        }
    }
}
