using UnityEngine;
using UnityEngine.Tilemaps;

public class HeroKnight : MonoBehaviour
{
    private Rigidbody2D rb;
    public Animator animator;

    float inputHorizontal;
    float inputVertical;
    [SerializeField] private float speed = 4f;
    bool facingRight = true;
    public Tilemap tilemap;
    private Vector2 minBounds;
    private Vector2 maxBounds;

    private bool canMove = true;  // Kiểm tra di chuyển

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Kiểm tra và cài đặt Tilemap để lấy các giới hạn của nó
        if (tilemap != null)
        {
            // Lấy giới hạn của Tilemap từ bounding box
            Bounds tilemapBounds = tilemap.localBounds;
            minBounds = tilemapBounds.min;
            maxBounds = tilemapBounds.max;
        }
        else
        {
            Debug.LogError("Tilemap reference is missing!");
        }
    }

    void FixedUpdate()
    {
        if (!canMove) return;  // Nếu không di chuyển, không xử lý input

        // Nhận input từ người chơi
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        // Tính toán hướng di chuyển
        Vector2 movement = new Vector2(inputHorizontal, inputVertical) * speed;

        // Tính toán vị trí mới của nhân vật
        Vector2 newPosition = rb.position + movement * Time.fixedDeltaTime;

        // Giới hạn vị trí mới trong phạm vi của Tilemap
        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);

        // Di chuyển nhân vật đến vị trí mới
        rb.MovePosition(newPosition);

        // Cập nhật Speed cho Animator
        float currentSpeed = movement.magnitude;
        animator.SetFloat("Speed", currentSpeed);

        // Xử lý hướng lật nhân vật (chỉ cần cho trục X)
        if (inputHorizontal > 0 && !facingRight)
        {
            Flip();
        }
        else if (inputHorizontal < 0 && facingRight)
        {
            Flip();
        }
    }

    public void DisableMovement()
    {
        canMove = false;  // Vô hiệu hóa di chuyển
    }

    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }
}
