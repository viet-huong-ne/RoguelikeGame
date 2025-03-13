using UnityEngine;

public class ExpItem : MonoBehaviour
{
    [SerializeField] private float pickUpDistance = 1f; // Khoảng cách để bắt đầu di chuyển về phía người chơi
    [SerializeField] private float moveSpeed = 5f;     // Tốc độ di chuyển về phía người chơi
    [SerializeField] private float fastMoveSpeed = 20f; // Tốc độ di chuyển khi ở chế độ hút nhanh

    private Transform player; // Lưu trữ tham chiếu đến vị trí người chơi

    // Biến tĩnh để kiểm soát chế độ hút nhanh
    public static bool isFastAttractMode = false;

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

        // Luôn di chuyển về phía người chơi nếu ở chế độ hút nhanh
        if (isFastAttractMode || Vector3.Distance(transform.position, player.position) < pickUpDistance)
        {
            Vector3 moveDir = (player.position - transform.position).normalized;

            // Chọn tốc độ dựa trên trạng thái hút nhanh
            float currentMoveSpeed = isFastAttractMode ? fastMoveSpeed : moveSpeed;
            transform.position += moveDir * currentMoveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Kiểm tra nếu va chạm với người chơi
        if (other.CompareTag("Player"))
        {
            HeroExperience heroExp = other.GetComponent<HeroExperience>();
            if (heroExp != null)
            {
                SoundEffectManager.Instance.PlaySoundEffect(Resources.Load<AudioClip>("SoundEffects/ExperienceReceived"), 1f);
                heroExp.AddExperience(10); // Thêm EXP
                Debug.Log("Picked up experience");
                Destroy(gameObject);      // Xóa viên EXP
            }
        }
    }
}
