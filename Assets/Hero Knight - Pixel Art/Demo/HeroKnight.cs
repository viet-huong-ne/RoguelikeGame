using UnityEngine;
using UnityEngine.Tilemaps;

public class HeroKnight : Singleton<HeroKnight>
{
    public Animator animator;

    float inputHorizontal;
    float inputVertical;
    [SerializeField] public float speed = 3f;
    bool facingRight = true;
    public Tilemap tilemap;
    public Vector2 moveDir;
    [HideInInspector]
    public Vector2 lastMovedVector;
    public Vector2 minBounds;
    public Vector2 maxBounds;
    private float currentSpeed = 3f;
    private bool canMove = true;  // Kiểm tra di chuyển
    // Reference
    Rigidbody2D rb;
    public CharacterScriptableObject characterData;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastMovedVector = new Vector2(1, 0f);
        animator = GetComponent<Animator>();

		// Kiểm tra và cài đặt Tilemap để lấy các giới hạn của nó
		if (tilemap != null)
		{
			// Convert tilemap cell bounds to world space
			BoundsInt cellBounds = tilemap.cellBounds;
			Vector3Int minCell = cellBounds.min;
			Vector3Int maxCell = cellBounds.max;

			// Convert tile positions to world positions
			Vector3 minWorld = tilemap.CellToWorld(minCell);
			Vector3 maxWorld = tilemap.CellToWorld(maxCell);

			// Adjust bounds slightly to keep the player inside
			minBounds = new Vector2(minWorld.x, minWorld.y);
			maxBounds = new Vector2(maxWorld.x, maxWorld.y);
		}
		else
		{
			Debug.LogError("Tilemap reference is missing!");
		}
		AssignStartingSkills();
	}

	public void SetSpeed(float boostSpeed){
        speed = boostSpeed;
    }

    void FixedUpdate()
    {
        if (!canMove) return;  // Nếu không di chuyển, không xử lý input

        // Nhận input từ người chơi
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
        // lấy địa điểm nhân vật
        moveDir = new Vector2(inputHorizontal, inputVertical).normalized;
        // lấy hướng mặt của nhân vật
        if (moveDir.x != 0)
        {
            lastMovedVector = new Vector2(moveDir.x, 0f); //Last moved X
        }
        if (moveDir.y != 0)
        {
            lastMovedVector = new Vector2(0f, moveDir.y); //Last moved Y
        }
        if (moveDir.x != 0 && moveDir.y != 0)
        {
            lastMovedVector = new Vector2(moveDir.x, moveDir.y);
        }
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
    public void ResetSpeed()
    {
        speed = currentSpeed;
    }

	private void AssignStartingSkills()
	{
		if (SkillManager.Instance != null)
		{
			Debug.Log("HeroKnight assigning default skills.");
		}
	}
}
