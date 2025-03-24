using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject monsterPrefab;
    [SerializeField]
    private GameObject theLastPrefab;
    [SerializeField] private GameObject keyPrefab;
    [SerializeField]
    public GameObject player;  // Reference to the player
    [SerializeField]
    private int initialSkeletons = 5;
    [SerializeField]
    private float initialMonsterInterval = 5f; // Tần suất spawn quái ban đầu (chậm hơn)
    [SerializeField]
    private float intervalDecreaseRate = 0.05f; // Tốc độ giảm tần suất spawn
    [SerializeField]
    private float spawnRadius = 15f; // Bán kính spawn xung quanh player
    private HeroHealth heroHealth; // Reference to HeroHealth
    private float skeletonInterval; // Thời gian giữa mỗi lần spawn quái
    private float elapsedTime = 0f; // Track elapsed time for gradual increase
    private int skeletonDensityMultiplier = 1; // Density multiplier to control skeleton count
    // private bool canSpawnWave = false;
    private List<Vector3> spawnPositions = new List<Vector3>(); // Lưu trữ các vị trí đã spawn
    private Camera mainCamera;
    [SerializeField]
    public Timer timer;
    private bool isTheLastSpawned = false;
    private bool canSpawnTheLast = false;
    private float keyPickedTime = -1f;
    private bool isPortalActivated = false;
    [SerializeField]
    private float decisionTime = 30f;
    [SerializeField]
    private float theLastDropRate = 0.2f;
    private float levelElapsedTime = 0f; // Đếm thời gian từ lúc vào màn mới
    private bool isNewLevel = true; // Đánh dấu nếu vừa vào màn mới

    void Awake(){
        Debug.Log("HIHIHI");
    }
    private void Update()
    {
        levelElapsedTime += Time.deltaTime; // Tính thời gian chỉ riêng màn hiện tại
    }
    void Start()
    {
        timer.StartTimer();
        Time.timeScale = 1f;
        if (player == null)
        {
            Debug.LogWarning("Player is not assigned to MonsterSpawner.");
        }
        else
        {
            Debug.Log("MonsterSpawner initialized with player: " + player.name);
        }
        
        if (timer != null)
        {
            timer.StartTimer();
        }

        heroHealth = GetComponentInParent<HeroHealth>();
        if (monsterPrefab == null || player == null)
        {
            Debug.LogError("Skeleton prefab or player is not assigned in the Inspector!");
            return;
        }

        skeletonInterval = initialMonsterInterval;

        mainCamera = Camera.main; // Lấy camera chính

        // Immediately spawn a few skeletons when the game starts
        SpawnInitialMonster();

        // Continue with the regular spawning
        StartCoroutine(SpawnMonsters());
        StartCoroutine(CheckSpawnTheLast());
    }

    private IEnumerator CheckSpawnTheLast()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // Kiểm tra mỗi giây

            float elapsedTime = timer.GetElapsedTime(); // Lấy thời gian từ Timer

            if (!isTheLastSpawned && !isPortalActivated && levelElapsedTime/*timer.GetElapsedTime()*/ > decisionTime)
            {
                canSpawnTheLast = true;

                if (Random.value <= theLastDropRate) // 20% xác suất
                {
                    SpawnLastMonster(theLastPrefab);
                    isTheLastSpawned = true;
                    canSpawnTheLast = false;
                }
            }

            if (keyPickedTime > 0 && elapsedTime >= keyPickedTime + 120f)
            {
                canSpawnTheLast = true;
            }
        }
    }

    private void SpawnLastMonster(GameObject prefab)
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject monster = Instantiate(prefab, spawnPosition, Quaternion.identity);
    }

    public void OnKeyPickedUp()
    {
        keyPickedTime = elapsedTime;
        canSpawnTheLast = false;
    }

    public void OnPortalActivated()
    {
        isPortalActivated = true;
        canSpawnTheLast = false;
    }

    private void SpawnInitialMonster()
    {
        if (heroHealth != null && heroHealth.IsDead())
        {
            return; // Do nothing if the hero is dead
        }

        for (int i = 0; i < initialSkeletons; i++)
        {
            GameObject skeleton = Instantiate(monsterPrefab);

            Vector3 spawnPosition = GetRandomSpawnPosition();
            skeleton.transform.position = spawnPosition;
            skeleton.SetActive(true);

            // Lưu trữ vị trí spawn
            spawnPositions.Add(spawnPosition);
        }
    }

    private IEnumerator SpawnMonsters()
    {
        while (true)
        {
            if (isTheLastSpawned) yield break;

            // Kiểm tra nếu player bị destroy, ngừng spawn
            if (player == null)
            {
                Debug.Log("Player is destroyed, stopping skeleton spawn.");
                yield break;
            }

            // Spawn quái sau khoảng thời gian tần suất
            yield return new WaitForSeconds(skeletonInterval);

            // Tăng mật độ quái sau mỗi mốc thời gian (ví dụ: sau 30 giây, 60 giây)
            elapsedTime += skeletonInterval;

            // Giảm tốc độ spawn từ từ và không quá nhanh
            if (elapsedTime >= 45f) // Sau 30 giây
            {
                skeletonDensityMultiplier = 1; // Giữ số lượng quái spawn thấp
            }

            if (elapsedTime >= 90f) // Sau 60 giây
            {
                skeletonDensityMultiplier = 2; // Tăng nhẹ mật độ
            }
            else if (elapsedTime >= 120f) // Sau 90 giây
            {
                skeletonDensityMultiplier = 3; // Tăng mật độ một cách nhẹ nhàng
            }

            // Spawn quái với số lượng và mật độ tăng dần
            int skeletonsToSpawn = Mathf.Min(3 * skeletonDensityMultiplier, 15); // Giới hạn số lượng quái spawn trong mỗi lần spawn (giảm số lượng quái hơn)

            for (int i = 0; i < skeletonsToSpawn; i++)
            {
                // Tạo mới skeleton mỗi lần
                GameObject skeleton = Instantiate(monsterPrefab);

                Vector3 spawnPosition = GetRandomSpawnPosition();
                skeleton.transform.position = spawnPosition;
                skeleton.SetActive(true);

                // Lưu trữ vị trí spawn để kiểm tra sau
                spawnPositions.Add(spawnPosition);
            }

            // Giảm tần suất spawn theo thời gian (chậm dần)
            skeletonInterval = Mathf.Max(3f, skeletonInterval - intervalDecreaseRate);  // Không cho tần suất spawn thấp hơn 3s
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        int maxRetries = 10; // Số lần thử vị trí spawn
        float safeDistance = 2f; // Đảm bảo spawn ngoài vùng camera
        Vector3 screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        for (int retries = 0; retries < maxRetries; retries++)
        {
            // Tạo vị trí spawn ngẫu nhiên trong bán kính xung quanh player
            Vector2 randomDirection = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = player.transform.position + new Vector3(randomDirection.x, randomDirection.y, 0f);

            // Kiểm tra spawnPosition có nằm ngoài phạm vi camera
            if (Mathf.Abs(spawnPosition.x) > screenBounds.x + safeDistance || Mathf.Abs(spawnPosition.y) > screenBounds.y + safeDistance)
            {
                // Kiểm tra vị trí spawn có giao với Tilemap Top không
                Collider2D hitCollider = Physics2D.OverlapCircle(spawnPosition, 0.5f, LayerMask.GetMask("Object"));
                if (hitCollider == null) // Nếu không có va chạm với Tilemap Top
                {
                    // Kiểm tra khoảng cách spawn so với các vị trí spawn trước đó
                    bool isValidPosition = true;

                    foreach (var pos in spawnPositions)
                    {
                        if (Vector3.Distance(spawnPosition, pos) < 2f) // Điều chỉnh khoảng cách này
                        {
                            isValidPosition = false;
                            break;
                        }
                    }

                    if (isValidPosition)
                    {
                        return spawnPosition; // Vị trí spawn hợp lệ
                    }
                }
            }
        }

        // Nếu sau số lần thử vẫn không tìm được, trả về vị trí mặc định gần player
        return player.transform.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0f);
    }
    public void StartNewLevel()
    {
        levelElapsedTime = 0f; // Reset bộ đếm cho màn mới
        isNewLevel = true; // Đánh dấu là màn mới
        isTheLastSpawned = false; // Reset trạng thái boss
    }


}

