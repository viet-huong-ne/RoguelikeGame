using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonHealth : MonoBehaviour, IHealth
{
    public GameObject damageText;
    [SerializeField] private int health = 20;
    private Animator animator;
    private bool isDead = false;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    [SerializeField] private float damageEffectDuration = 0.2f;

    // Thêm biến để chứa prefab của viên kinh nghiệm
    [SerializeField] private GameObject experiencePrefab;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

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

        // Rơi ra viên kinh nghiệm khi Skeleton chết
        DropExperience();

        if (animator != null)
        {
            animator.SetTrigger("Death");
            float deathAnimationTime = animator.GetCurrentAnimatorStateInfo(0).length;
            Destroy(gameObject, deathAnimationTime); // Hủy sau khi animation hoàn tất
        }
    }

    public bool IsDead()
    {
        return isDead;
    }

    private IEnumerator ShowDamageEffect()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(damageEffectDuration);
        spriteRenderer.color = originalColor;
    }

    private void DropExperience()
    {
        if (experiencePrefab != null)
        {
            // Tạo viên kinh nghiệm ở dưới Skeleton (giảm y một chút)
            Vector3 spawnPosition = transform.position + new Vector3(0, -1, 0);

            // Tạo viên kinh nghiệm tại vị trí tính toán
            Instantiate(experiencePrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Experience prefab not assigned in the Inspector.");
        }
    }
}
