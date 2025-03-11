using UnityEngine;

public class CoinItem : MonoBehaviour
{
    [SerializeField] private float pickUpDistance = 3f; // Khoảng cách để bắt đầu di chuyển về phía người chơi
    [SerializeField] private float moveSpeed = 5f;     // Tốc độ di chuyển về phía người chơi
    private CoinCounter coinCounter;
    [SerializeField] private float fastMoveSpeed = 20f; // Tốc độ di chuyển khi ở chế độ hút nhanh
    public static bool isFastAttractMode = false;
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
            if (coinCounter != null)
            {
                coinCounter.AddCoin();
                SoundEffectManager.Instance.PlaySoundEffect(Resources.Load<AudioClip>("SoundEffects/CoinReceived"), 1f);
                Destroy(gameObject);
            }
            else{
                Debug.Log("Coint counter null");
            }
        }
    }
}
