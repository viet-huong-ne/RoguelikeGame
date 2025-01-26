using UnityEngine;
using System.Collections;

public class HeroKnight : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 movementDirection;
    private Animator animator;

    float inputHorizontal;
    float inputVertical;
    float speed = 5f;
    bool facingRight = true;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        float currentSpeed = rb.velocity.magnitude;
        animator.SetFloat("Speed", currentSpeed);

        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector2(inputHorizontal * speed, inputVertical * speed);

        // Xử lý hướng lật (chỉ cần cho trục X)
        if (inputHorizontal > 0 && !facingRight)
        {
            Flip();
        }
        else if (inputHorizontal < 0 && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }
}
