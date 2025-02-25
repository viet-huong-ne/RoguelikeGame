using UnityEngine;

public class KeyItem : MonoBehaviour
{
    [SerializeField] private float pickUpDistance = 1f; // Khoảng cách để bắt đầu di chuyển về phía người chơi
    [SerializeField] private float moveSpeed = 5f;     // Tốc độ di chuyển về phía người chơi
    [SerializeField] private GameObject portalPrefab;  // Prefab của Portal
    [SerializeField] private float portalOffset = 2f;  // Khoảng cách Portal xuất hiện trước mặt Player
    [SerializeField] private Timer timer;
    private Transform player; // Lưu trữ tham chiếu đến vị trí người chơi

    private void Start()
    {
        // Tìm đối tượng Player thông qua tag nếu chưa gán trong Inspector
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogError("Player not found! Ensure Player has the 'Player' tag.");
            }
        }
    }

    private void Update()
    {
        if (player == null) return;

        // Kiểm tra khoảng cách giữa viên EXP và Player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < pickUpDistance)
        {
            // Di chuyển viên EXP về phía người chơi
            Vector3 moveDir = (player.position - transform.position).normalized;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            timer.StopTimer();
            // Kích hoạt trạng thái hút nhanh
            ExpItem.isFastAttractMode = true;
            SackOfGoldItem.isFastAttractMode = true;
            CoinItem.isFastAttractMode = true;
            SummonPortal();
            Destroy(gameObject);
        }
    }

    private void SummonPortal()
    {
        if (portalPrefab != null && player != null)
        {
            Vector3 portalPosition = player.position
                               + player.transform.right * portalOffset // Dịch trước mặt
                               + Vector3.up * 1f;
            
            // Triệu hồi Portal
            Instantiate(portalPrefab, portalPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Portal Prefab or Player reference is missing!");
        }
    }
}
