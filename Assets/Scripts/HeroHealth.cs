using System.Collections;
using UnityEngine;

public class HeroHealth : MonoBehaviour, IHealth
{
    [SerializeField] private int health = 200;
    [SerializeField] private int MAX_HEALTH = 200;

    [SerializeField] private float damageEffectDuration = 0.2f; // Thời gian hiệu ứng đỏ
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private Animator animator;
    private HeroKnight heroKnight;
    private HeroAttack heroAttack; 

    private void Start()
    {
        // Lấy SpriteRenderer và lưu màu gốc
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>(); 
        heroKnight = GetComponent<HeroKnight>(); 
        heroAttack = GetComponent<HeroAttack>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void TakeDamage(int amount)
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative damage");
        }

        // Kiểm tra nếu nhân vật đã chết, không nhận thêm sát thương
        if (health <= 0)
        {
            return;
        }

        health -= amount;

        // Kích hoạt hiệu ứng bị đánh
        if (spriteRenderer != null)
        {
            StartCoroutine(ShowDamageEffect());
        }

        if (health <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative healing");
        }

        bool wouldBeOverMaxHealth = health + amount > MAX_HEALTH;

        if (wouldBeOverMaxHealth)
        {
            health = MAX_HEALTH;
        }
        else
        {
            health += amount;
        }
    }

    private void Die()
    {
        Debug.Log("I am dead!");

        // Gọi trigger để phát animation chết
        if (animator != null)
        {
            animator.SetTrigger("Death");
        }

        // Tạm dừng di chuyển của nhân vật khi chết
        if (heroKnight != null)
        {
            heroKnight.DisableMovement();
        }

        // Vô hiệu hóa tấn công khi chết
        if (heroAttack != null)
        {
            heroAttack.DisableAttack();
        }

        // Đợi animation chết trước khi hủy nhân vật
        StartCoroutine(WaitForDeathAnimation());
    }

    private IEnumerator WaitForDeathAnimation()
    {
        // Lấy thời gian dài của animation "Death"
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = stateInfo.length;

        // Chờ cho animation chết hoàn tất
        yield return new WaitForSecondsRealtime(animationDuration + 1f);

        Destroy(gameObject);
    }

    private IEnumerator ShowDamageEffect()
    {
        // Đổi màu nhân vật sang đỏ
        spriteRenderer.color = Color.red;

        // Chờ trong thời gian hiệu ứng
        yield return new WaitForSeconds(damageEffectDuration);

        // Khôi phục màu gốc
        spriteRenderer.color = originalColor;
    }
}
