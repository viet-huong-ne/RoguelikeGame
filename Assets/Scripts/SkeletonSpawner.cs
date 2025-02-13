using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject skeletonPrefab;
    [SerializeField]
    private GameObject player;  // Reference to the player
    [SerializeField]
    private float initialSkeletonInterval = 5f; // Tần suất spawn quái ban đầu (chậm hơn)
    [SerializeField]
    private float intervalDecreaseRate = 0.05f; // Tốc độ giảm tần suất spawn
    [SerializeField]
    private float spawnRadius = 15f; // Bán kính spawn xung quanh player
    [SerializeField]
    private float waveTimeInterval = 15f; // Thời gian tối thiểu giữa các wave (sau 30 giây sẽ có thể bắt đầu spawn wave)
    [SerializeField]
    private float minimumWaveInterval = 15f; // Thời gian tối thiểu giữa các wave (15 giây)

    private float skeletonInterval; // Thời gian giữa mỗi lần spawn quái
    private int waveCount = 0; // Số wave đã qua
    private float elapsedTime = 0f; // Track elapsed time for gradual increase
    private int skeletonDensityMultiplier = 1; // Density multiplier to control skeleton count
    private bool canSpawnWave = false; // Điều kiện để có thể spawn wave to
    private List<Vector3> spawnPositions = new List<Vector3>(); // Lưu trữ các vị trí đã spawn

    private Camera mainCamera;

    void Start()
    {
        if (skeletonPrefab == null || player == null)
        {
            Debug.LogError("Skeleton prefab or player is not assigned in the Inspector!");
            return;
        }

        skeletonInterval = initialSkeletonInterval;

        mainCamera = Camera.main; // Lấy camera chính

        // Immediately spawn a few skeletons when the game starts
        SpawnInitialSkeletons();

        // Continue with the regular spawning
        StartCoroutine(SpawnSkeletons()); // Spawn quái liên tục ngay từ đầu
        StartCoroutine(SpawnWave()); // Cũng spawn wave quái khi có điều kiện
    }

    private void SpawnInitialSkeletons()
    {
        int initialSkeletons = 3; // Number of skeletons to spawn immediately
        for (int i = 0; i < initialSkeletons; i++)
        {
            GameObject skeleton = Instantiate(skeletonPrefab);

            Vector3 spawnPosition = GetRandomSpawnPosition();
            skeleton.transform.position = spawnPosition;
            skeleton.SetActive(true);

            // Gán player cho AIChase để quái chase player
            skeleton.GetComponent<AIChase>().SetPlayer(player);

            // Lưu trữ vị trí spawn
            spawnPositions.Add(spawnPosition);
        }
    }


    private IEnumerator SpawnSkeletons()
    {
        while (true)
        {
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
            if (elapsedTime >= 30f) // Sau 30 giây
            {
                skeletonDensityMultiplier = 1; // Giữ số lượng quái spawn thấp
            }

            if (elapsedTime >= 60f) // Sau 60 giây
            {
                skeletonDensityMultiplier = 2; // Tăng nhẹ mật độ
            }
            else if (elapsedTime >= 90f) // Sau 90 giây
            {
                skeletonDensityMultiplier = 3; // Tăng mật độ một cách nhẹ nhàng
            }

            // Spawn quái với số lượng và mật độ tăng dần
            int skeletonsToSpawn = Mathf.Min(3 * skeletonDensityMultiplier, 15); // Giới hạn số lượng quái spawn trong mỗi lần spawn (giảm số lượng quái hơn)

            for (int i = 0; i < skeletonsToSpawn; i++)
            {
                // Tạo mới skeleton mỗi lần
                GameObject skeleton = Instantiate(skeletonPrefab);

                Vector3 spawnPosition = GetRandomSpawnPosition();
                skeleton.transform.position = spawnPosition;
                skeleton.SetActive(true);

                // Gán player cho AIChase để quái chase player
                skeleton.GetComponent<AIChase>().SetPlayer(player);

                // Lưu trữ vị trí spawn để kiểm tra sau
                spawnPositions.Add(spawnPosition);
            }

            // Giảm tần suất spawn theo thời gian (chậm dần)
            skeletonInterval = Mathf.Max(3f, skeletonInterval - intervalDecreaseRate);  // Không cho tần suất spawn thấp hơn 3s
        }
    }

    private IEnumerator SpawnWave()
    {
        // Chỉ cho phép spawn wave sau 30 giây đầu tiên
        yield return new WaitForSeconds(30f); // Đảm bảo không spawn wave trong 30 giây đầu

        canSpawnWave = true;

        while (true)
        {
            if (canSpawnWave)
            {
                waveCount++;

                // Tăng dần số lượng quái trong wave nhưng không quá nhiều
                int skeletonsInWave = Mathf.Min(waveCount * 2 * skeletonDensityMultiplier, 30);  // Điều chỉnh số lượng quái trong mỗi wave (giảm số lượng quái hơn)
                Debug.Log("Wave " + waveCount + " spawning " + skeletonsInWave + " skeletons!");

                // Spawn nhiều quái cùng lúc
                for (int i = 0; i < skeletonsInWave; i++)
                {
                    // Tạo mới skeleton mỗi lần
                    GameObject skeleton = Instantiate(skeletonPrefab);

                    Vector3 spawnPosition = GetRandomSpawnPosition();
                    skeleton.transform.position = spawnPosition;
                    skeleton.SetActive(true);

                    // Gán player cho AIChase để quái chase player
                    skeleton.GetComponent<AIChase>().SetPlayer(player);

                    // Lưu trữ vị trí spawn để kiểm tra sau
                    spawnPositions.Add(spawnPosition);
                }

                // Thời gian ngẫu nhiên giữa các wave, nhưng tối thiểu 15 giây
                float waveInterval = Random.Range(minimumWaveInterval, waveTimeInterval);
                yield return new WaitForSeconds(waveInterval);
            }
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

