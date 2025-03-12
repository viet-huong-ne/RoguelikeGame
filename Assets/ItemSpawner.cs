using UnityEngine;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner Instance;

    [SerializeField] private GameObject itemPrefab; // Prefab của item để spawn
    [SerializeField] private float mapWidth = 10f;  // Độ rộng bản đồ (x)
    [SerializeField] private float mapHeight = 10f; // Độ cao bản đồ (y)
    [SerializeField] private float maxSpawnInterval = 15f; // Thời gian spawn tối đa (giây)
    [SerializeField] private float minSpawnInterval = 5f; // Thời gian spawn tối thiểu (giây)
    [SerializeField] private LayerMask tilemapLayer; // Layer của Tilemap để kiểm tra va chạm
    [SerializeField] private int maxRetries = 10; // Số lần thử tìm vị trí spawn
    [SerializeField] private float spawnRadius = 0.5f; // Bán kính kiểm tra va chạm với Tilemap
    [SerializeField] private List<Vector3> spawnPositions; // Danh sách vị trí spawn đã sử dụng

    private float nextSpawnTime; // Thời điểm tiếp theo để spawn

    void Start()
    {
        spawnPositions = new List<Vector3>();
        ScheduleNextSpawn();
    }

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Kiểm tra nếu đã đến thời gian spawn tiếp theo
        if (Time.time >= nextSpawnTime)
        {
            SpawnItem();
            ScheduleNextSpawn();
        }
    }

    // Spawn item tại vị trí hợp lệ
    private void SpawnItem()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        if (spawnPosition != Vector3.zero) // Nếu tìm được vị trí hợp lệ
        {
            Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
            spawnPositions.Add(spawnPosition); // Lưu vị trí spawn
        }
    }

    // Lên lịch thời gian spawn tiếp theo
    private void ScheduleNextSpawn()
    {
        float randomInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
        nextSpawnTime = Time.time + randomInterval;
    }

    // Lấy vị trí spawn ngẫu nhiên tránh Tilemap
    private Vector3 GetRandomSpawnPosition()
    {
        for (int retries = 0; retries < maxRetries; retries++)
        {
            // Tạo vị trí ngẫu nhiên trong giới hạn bản đồ
            float randomX = Random.Range(-mapWidth / 2f, mapWidth / 2f);
            float randomY = Random.Range(-mapHeight / 2f, mapHeight / 2f);
            Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);

            // Kiểm tra va chạm với Tilemap
            Collider2D hitCollider = Physics2D.OverlapCircle(spawnPosition, spawnRadius, tilemapLayer);
            if (hitCollider == null) // Nếu không có va chạm
            {
                // Kiểm tra khoảng cách spawn không trùng với các vị trí đã spawn
                bool isValidPosition = true;
                foreach (var pos in spawnPositions)
                {
                    if (Vector3.Distance(spawnPosition, pos) < 2f) // Điều chỉnh khoảng cách tối thiểu
                    {
                        isValidPosition = false;
                        break;
                    }
                }

                if (isValidPosition)
                {
                    return spawnPosition;
                }
            }
        }

        // Nếu không tìm được vị trí hợp lệ, trả về Vector3.zero
        Debug.LogWarning("Không tìm được vị trí spawn hợp lệ sau " + maxRetries + " lần thử.");
        return Vector3.zero;
    }
}
