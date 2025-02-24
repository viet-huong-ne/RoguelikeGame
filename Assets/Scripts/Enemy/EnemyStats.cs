using System.Collections;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    [SerializeField] private float damageCooldown = 1f;
    public GameObject hero;
    //show dame of skill
    public GameObject damageText;
    // current stats 
    float currentMoveSpeed;
    float currentHealth;
    float currentDamage;
    void Awake()
    {
        currentMoveSpeed = enemyData.MoveSpeed;
        currentDamage = enemyData.Damage;
        currentHealth = enemyData.MaxHealth;
    }
    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            Kill();
        }
        RectTransform textTransform = Instantiate(damageText).GetComponent<RectTransform>();
        textTransform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        Canvas canvas = GameObject.FindObjectOfType<Canvas>();
        textTransform.SetParent(canvas.transform);
        PopUpDamage popup = textTransform.GetComponent<PopUpDamage>();
        if (popup != null)
        {
            popup.textMesh.text = dmg.ToString();
        }
    }
    private bool canDamage = true;      // Kiểm soát trạng thái sẵn sàng gây sát thương

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            HeroHealth hero = col.GetComponent<HeroHealth>();
            hero.TakeDamage((int)currentDamage);
        }
    }
    public void TryDealDamage(GameObject target)
    {
        if (canDamage)
        {
            StartCoroutine(DealDamageCoroutine(target));
        }
    }

    private IEnumerator DealDamageCoroutine(GameObject target)
    {
        // Gây sát thương nếu đối tượng có "PlayerHealth"
        HeroHealth heroHealth = target.GetComponent<HeroHealth>();
        if (heroHealth != null)
        {
            heroHealth.TakeDamage(((int)enemyData.Damage));
        }

        // Chờ thời gian hồi
        canDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canDamage = true;
    }
    public void Kill()
    {
        Destroy(gameObject);  
    }


}
