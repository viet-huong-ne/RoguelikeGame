using UnityEngine;

public class SackOfGoldItem : MonoBehaviour
{
    [SerializeField] private float pickUpDistance = 1f; // Khoảng cách để bắt đầu di chuyển về phía người chơi
    [SerializeField] private float moveSpeed = 5f;     // Tốc độ di chuyển về phía người chơi
    private CoinCounter coinCounter;
    private Transform player; // Lưu trữ tham chiếu đến vị trí người chơi

    private void Start()
    {
        coinCounter = GameObject.Find("CCO").GetComponent<CoinCounter>();
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
        // Kiểm tra nếu va chạm với người chơi
        if (other.CompareTag("Player"))
        {
            if (coinCounter != null)
            {
                coinCounter.AddSackOfGold();
                Destroy(gameObject);
            }
            else{
                Debug.Log("Coint counter null");
            }
        }
    }
}
