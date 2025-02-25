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
    private GameObject player;  // Reference to the player
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
    private int waveCount = 0; // Số wave đã qua
    private float elapsedTime = 0f; // Track elapsed time for gradual increase
    private int skeletonDensityMultiplier = 1; // Density multiplier to control skeleton count
    private bool canSpawnWave = false; // Điều kiện để có thể spawn wave to
    private List<Vector3> spawnPositions = new List<Vector3>(); // Lưu trữ các vị trí đã spawn
    private Camera mainCamera;
    [SerializeField]
    private Timer timer;
    private bool isTheLastSpawned = false;
    private bool canSpawnTheLast = false;
    private float keyPickedTime = -1f;
    private bool isPortalActivated = false;
    [SerializeField]
    private float decisionTime = 30f;
    [SerializeField]
    private float theLastDropRate = 0.2f;
    void Start()
    {
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
        SpawnInitialSkeletons();

        // Continue with the regular spawning
        StartCoroutine(SpawnSkeletons());
        StartCoroutine(CheckSpawnTheLast());
    }

    private IEnumerator CheckSpawnTheLast()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // Kiểm tra mỗi giây

            float elapsedTime = timer.GetElapsedTime(); // Lấy thời gian từ Timer

            if (!isTheLastSpawned && !isPortalActivated && timer.GetElapsedTime() > decisionTime)
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
        monster.GetComponent<EnemyMovement>().SetPlayer(player);
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

    private void SpawnInitialSkeletons()
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

    private IEnumerator SpawnSkeletons()
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
        // Tạo phạm vi spawn ngoài camera để không bị spawn gần người chơi
        Vector3 screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        float safeDistance = 2f; // Đảm bảo rằng quái spawn ngoài vùng camera

        // Tạo một phạm vi spawn rộng hơn camera
        Vector2 randomDirection = Random.insideUnitCircle * spawnRadius;

        // Vị trí spawn thử
        Vector3 spawnPosition = player.transform.position + new Vector3(randomDirection.x, randomDirection.y, 0f);

        // Kiểm tra xem vị trí spawn có nằm trong phạm vi an toàn không
        if (Mathf.Abs(spawnPosition.x) > screenBounds.x + safeDistance && Mathf.Abs(spawnPosition.y) > screenBounds.y + safeDistance)
        {
            // Kiểm tra xem vị trí spawn có quá gần các quái đã spawn trước đó không
            int maxRetries = 10;  // Giới hạn số lần thử
            int retries = 0;

            while (retries < maxRetries)
            {
                bool isValidPosition = true;

                foreach (var pos in spawnPositions)
                {
                    if (Vector3.Distance(spawnPosition, pos) < 2f) // Điều chỉnh khoảng cách này theo nhu cầu
                    {
                        isValidPosition = false;
                        break;
                    }
                }

                if (isValidPosition)
                {
                    return spawnPosition; // Nếu tìm được vị trí hợp lý, trả về
                }

                // Nếu không hợp lý, thử lại với một vị trí ngẫu nhiên mới
                randomDirection = Random.insideUnitCircle * spawnRadius;
                spawnPosition = player.transform.position + new Vector3(randomDirection.x, randomDirection.y, 0f);
                retries++;
            }

            // Nếu không tìm được vị trí hợp lý sau số lần thử, trả về vị trí ngẫu nhiên gần player
            return spawnPosition; 
        }
        else
        {
            // Nếu nằm gần camera, tạo lại vị trí spawn
            return GetRandomSpawnPosition();
        }
    }
}

