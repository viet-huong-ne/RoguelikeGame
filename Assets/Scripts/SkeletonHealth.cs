using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonHealth : MonoBehaviour, IHealth
{
    public GameObject damageText;
    [SerializeField] private int health = 20;
    private Animator animator;  
    private bool isDead = false;  // Biến kiểm tra tình trạng chết

    private SpriteRenderer spriteRenderer;  // Thêm biến lưu SpriteRenderer
    private Color originalColor;  // Lưu màu gốc

    [SerializeField] private float damageEffectDuration = 0.2f; // Thời gian hiệu ứng đổi màu

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();  // Lấy SpriteRenderer

        // Lưu màu gốc của sprite
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void TakeDamage(int amount)
    {
        if(amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative damage");
        }

        this.health -= amount;

        if (spriteRenderer != null)
        {
            StartCoroutine(ShowDamageEffect());
        }

        
        RectTransform textTransform = Instantiate(damageText).GetComponent<RectTransform>();
        textTransform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        Canvas canvas = GameObject.FindObjectOfType<Canvas>();
        textTransform.SetParent(canvas.transform);
        PopUpDamage popup = textTransform.GetComponent<PopUpDamage>();
        if (popup != null)
        {
            // Gửi giá trị damage cho popup
            popup.textMesh.text = amount.ToString();
        }

        if(health <= 0 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Skeleton is dead!");

        if (animator != null)
        {
            animator.SetTrigger("Death");
            // Lấy thời gian dài của animation "Death" và hủy đối tượng sau đó
            float deathAnimationTime = animator.GetCurrentAnimatorStateInfo(0).length;
            Destroy(gameObject, deathAnimationTime); // Hủy sau khi animation hoàn tất
        }
    }

    // Phương thức trả về trạng thái sống
    public bool IsDead()
    {
        return isDead;
    }

    // Coroutine để thay đổi màu sắc tạm thời khi nhận sát thương
    private IEnumerator ShowDamageEffect()
    {
        // Đổi màu thành trắng
        spriteRenderer.color = Color.red;

        // Chờ trong thời gian hiệu ứng
        yield return new WaitForSeconds(damageEffectDuration);

        // Khôi phục lại màu gốc
        spriteRenderer.color = originalColor;
    }
}