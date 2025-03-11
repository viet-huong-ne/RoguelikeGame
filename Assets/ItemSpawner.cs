using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab; // Prefab của item để spawn
    [SerializeField] private float mapWidth = 10f;  // Độ rộng bản đồ (x)
    [SerializeField] private float mapHeight = 10f; // Độ cao bản đồ (y)
    [SerializeField] private float maxSpawnInterval = 15f; // Thời gian spawn tối đa (giây)

    private float nextSpawnTime; // Thời điểm tiếp theo để spawn

    void Start()
    {
        ScheduleNextSpawn();
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

    // Spawn item tại vị trí ngẫu nhiên
    private void SpawnItem()
    {
        // Tính vị trí ngẫu nhiên trong giới hạn bản đồ
        float randomX = Random.Range(-mapWidth / 2f, mapWidth / 2f);
        float randomY = Random.Range(-mapHeight / 2f, mapHeight / 2f);
        Vector2 spawnPosition = new Vector2(randomX, randomY);

        // Instantiate item tại vị trí ngẫu nhiên
        Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
    }

    // Lên lịch thời gian spawn tiếp theo
    private void ScheduleNextSpawn()
    {
        float randomInterval = Random.Range(0f, maxSpawnInterval);
        nextSpawnTime = Time.time + randomInterval;
    }
}
