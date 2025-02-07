using UnityEngine;

public class AttackArea : MonoBehaviour
{
    [SerializeField] private int damage = 20;
    [SerializeField] private int damageVariance = 2; // Giá trị ± cho sát thương
    private Animator animator;

    [SerializeField] private GameObject owner; // Chủ sở hữu (người chơi)

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Bỏ qua va chạm nếu collider thuộc về chính người chơi
        if (collider.gameObject == owner)
        {
            return;
        }

        IHealth health = collider.GetComponent<IHealth>();
        if (health != null)
        {
            // Tạo sát thương ngẫu nhiên trong khoảng [damage - damageVariance, damage + damageVariance]
            int randomDamage = Random.Range(damage - damageVariance, damage + damageVariance + 1);
            health.TakeDamage(randomDamage);

            // Áp dụng lực đẩy nếu đối tượng là AIChase
            AIChase aiChase = collider.GetComponent<AIChase>();
            if (aiChase != null)
            {
                Vector2 knockbackDirection = (collider.transform.position - transform.position).normalized;
                aiChase.ApplyKnockback(knockbackDirection, 0.5f); // Đẩy quái vật trong 0.5 giây
            }

            // Kích hoạt animation tấn công (nếu có)
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }
        }
    }

    public void SetOwner(GameObject newOwner)
    {
        owner = newOwner; // Cài đặt chủ sở hữu (người chơi)
    }
}
