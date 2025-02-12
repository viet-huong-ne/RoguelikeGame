using UnityEngine;

public class ExpItem : MonoBehaviour
{
    [SerializeField] private float pickUpDistance = 3f; // Khoảng cách để bắt đầu di chuyển về phía người chơi
    [SerializeField] private float moveSpeed = 5f;     // Tốc độ di chuyển về phía người chơi

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
        // Kiểm tra nếu va chạm với người chơi
        if (other.CompareTag("Player"))
        {
            HeroExperience heroExp = other.GetComponent<HeroExperience>();
            if (heroExp != null)
            {
                heroExp.AddExperience(10); // Thêm EXP
                Destroy(gameObject);      // Xóa viên EXP
            }
        }
    }
}
