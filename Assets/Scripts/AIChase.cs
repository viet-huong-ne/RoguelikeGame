using UnityEngine;

public class AIChase : MonoBehaviour
{
    [SerializeField] private GameObject player;           
    [SerializeField] private float speed = 1.5f;                 
    [SerializeField] private float stopDistance = 1.5f;   
    [SerializeField] private Animator animator;
    private float distance;
    private SkeletonAttack skeletonAttack; // Tham chiếu đến lớp tấn công
    private SkeletonHealth skeletonHealth; // Thêm tham chiếu tới SkeletonHealth

    private bool isKnockedBack = false; // Trạng thái bị đẩy
    private Vector2 knockbackDirection; // Hướng bị đẩy
    private float knockbackDuration = 0.5f; // Thời gian bị đẩy
    private float knockbackTimeLeft; // Thời gian còn lại của lực đẩy

    private void Start()
    {
        // Lấy AttackHandler và SkeletonHealth từ cùng GameObject
        skeletonAttack = GetComponent<SkeletonAttack>();
        skeletonHealth = GetComponent<SkeletonHealth>();

        if (skeletonAttack == null)
        {
            Debug.LogError("AttackHandler is missing on " + gameObject.name);
        }

        if (skeletonHealth == null)
        {
            Debug.LogError("SkeletonHealth is missing on " + gameObject.name);
        }
    }

    private void Update()
    {
        // Nếu Skeleton chết, không thực hiện hành động gì
        if (skeletonHealth == null || skeletonHealth.IsDead())
        {
            return;
        }

        // Nếu quái vật đang bị đẩy, giảm thời gian lực đẩy
        if (isKnockedBack)
        {
            knockbackTimeLeft -= Time.deltaTime;

            // Quái vật sẽ bị đẩy trong một khoảng thời gian ngắn
            transform.position += (Vector3)knockbackDirection * speed * Time.deltaTime;

            // Khi hết thời gian lực đẩy, khôi phục trạng thái
            if (knockbackTimeLeft <= 0)
            {
                isKnockedBack = false;
            }
            return; // Ngừng các hành động khác trong khi bị đẩy
        }

        // Tính khoảng cách giữa quái và người chơi
        distance = Vector2.Distance(transform.position, player.transform.position);

        // Nếu khoảng cách lớn hơn stopDistance, quái vật di chuyển
        if (distance > stopDistance)
        {
            MoveTowardsPlayer();
        }
        else
        {
            // Nếu đã chạm, cố gắng gây sát thương
            skeletonAttack?.TryDealDamage(player);
        }
    }

    private void MoveTowardsPlayer()
    {
        // Di chuyển quái về phía người chơi
        Vector2 direction = (player.transform.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        // Lật trái/phải dựa trên hướng của người chơi
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // Cập nhật tham số Speed cho Animator
        animator.SetFloat("Speed", speed);
    }

    // Phương thức gọi từ bên ngoài để đẩy quái vật
    public void ApplyKnockback(Vector2 direction, float duration)
    {
        knockbackDirection = direction;
        knockbackDuration = duration;
        knockbackTimeLeft = duration;
        isKnockedBack = true;
    }

    // Method to assign the player reference
    public void SetPlayer(GameObject playerObject)
    {
        player = playerObject;
    }
}
