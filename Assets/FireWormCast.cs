using System.Collections;
using UnityEngine;

public class FireWormCast : MonoBehaviour
{
    [SerializeField] private GameObject fireBallPrefab;
    [SerializeField] private float singleFireBallCooldown = 6f; // Thời gian hồi cho bắn 1 quả cầu
    [SerializeField] private float multiFireBallCooldown = 15f; // Thời gian hồi cho bắn 3 quả cầu
    [SerializeField] private int numberOfCasts = 3; // Số quả cầu lửa khi bắn liên tục
    [SerializeField] private float castInterval = 1f; // Thời gian giữa các lần bắn liên tục

    private Animator animator;
    private bool isCasting = false;
    private Transform player;

    private FireWormMovement fireWormMovement;

    void Start()
    {
        animator = GetComponent<Animator>();

        HeroKnight heroKnight = FindObjectOfType<HeroKnight>();
        if (heroKnight != null)
        {
            player = heroKnight.transform;
        }
        else
        {
            Debug.LogError("No HeroKnight found in the scene.");
        }

        fireWormMovement = GetComponent<FireWormMovement>();

        StartCoroutine(CastRoutine());
    }

    void Update()
    {
        if (isCasting)
        {
            fireWormMovement.StopMovement();

            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Attack") && stateInfo.normalizedTime >= 1f)
            {
                FinishCasting();
            }
            return;
        }
    }

    IEnumerator CastRoutine()
    {
        while (true)
        {
            // Bắn một quả cầu mỗi 6 giây
            yield return new WaitForSeconds(singleFireBallCooldown);
            if (!isCasting)
            {
                CastFireBall();
            }

            // Bắn 3 quả cầu liên tục mỗi 15 giây
            yield return new WaitForSeconds(multiFireBallCooldown - singleFireBallCooldown);
            if (!isCasting)
            {
                StartCoroutine(CastFireBallMultipleTimes());
            }
        }
    }

    IEnumerator CastFireBallMultipleTimes()
    {
        isCasting = true;

        // Stop movement during casting
        if (fireWormMovement != null)
        {
            fireWormMovement.StopMovement();
        }
        else
        {
            Debug.LogWarning("FireWormMovement is missing!");
        }

        animator.SetTrigger("Attack");

        for (int i = 0; i < numberOfCasts; i++)
        {
            CastFireBall();

            yield return new WaitForSeconds(castInterval);
        }

        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            yield return null;
        }

        FinishCasting();
    }

    void CastFireBall()
    {
        if (fireBallPrefab == null || player == null)
        {
            Debug.LogWarning("FireBallPrefab or player is missing!");
            return;
        }

        // Tạo quả cầu lửa tại vị trí của FireWorm
        GameObject fireBall = Instantiate(fireBallPrefab, transform.position, Quaternion.identity);

        // Tính toán hướng di chuyển từ FireWorm đến người chơi
        Vector2 direction = (player.position - transform.position).normalized;

        // Gắn tốc độ di chuyển (velocity) cho quả cầu lửa
        Rigidbody2D rb = fireBall.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float fireBallSpeed = 10f; // Tốc độ di chuyển của quả cầu lửa
            rb.linearVelocity = direction * fireBallSpeed;
        }
        else
        {
            Debug.LogWarning("FireBallPrefab is missing Rigidbody2D!");
        }

        // Quay quả cầu lửa để hướng về người chơi
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        fireBall.transform.rotation = Quaternion.Euler(0, 0, angle);

        Debug.Log("FireBall spawned and launched towards the player.");
    }

    void FinishCasting()
    {
        if (!isCasting) return;

        isCasting = false;

        if (fireWormMovement != null)
        {
            fireWormMovement.ResumeMovement();
        }
        else
        {
            Debug.LogWarning("FireWormMovement is missing!");
        }

        Debug.Log("Casting finished. Movement resumed.");
    }
}
