using System.Collections;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class HeroAttack : MonoBehaviour
{
    private GameObject attackArea = default;
    private GameObject secondaryAttackArea = null;
    private bool doubleAttackEnabled = false;
    private bool attacking = false;
    private bool canAttack = true;  // Kiểm tra khả năng tấn công

    private float timeToAttack = 0.25f; // Thời gian vùng tấn công tồn tại
    private float attackInterval = 1.2f; // Khoảng thời gian giữa các đợt tấn công
    private float timer = 0f;

    // Start is called before the first frame update
    public int damage = 10;   // Base damage
    public int level = 1;     // Starting level

    // Store the original scale of the attack area for scaling purposes
    private Vector3 originalAttackAreaScale;

    void Start()
    {
        attackArea = transform.GetChild(0).gameObject;

        // Save the original scale of the attack area
        originalAttackAreaScale = attackArea.transform.localScale;

        // Đảm bảo tắt vùng tấn công ban đầu
        attackArea.SetActive(false);

        // Bắt đầu coroutine tự động tấn công
        StartCoroutine(AutoAttack());
    }

    // Update is called once per frame
    void Update()
    {
        if (attacking)
        {
            timer += Time.deltaTime;

            if (timer >= timeToAttack)
            {
                timer = 0;
                attacking = false;
                attackArea.SetActive(false);
                if (doubleAttackEnabled && secondaryAttackArea != null)
                {
                    secondaryAttackArea.SetActive(false);
                }
            }
        }
    }

    private void Attack()
    {
        if (canAttack)
        {
            attacking = true;
            attackArea.SetActive(true);
            if (doubleAttackEnabled && secondaryAttackArea != null)
            {
                secondaryAttackArea.SetActive(true);
            }
        }
    }

    private IEnumerator AutoAttack()
    {
        while (true)
        {
            if (canAttack)  // Kiểm tra nếu có thể tấn công
            {
                // Gọi hàm Attack mỗi 1.5 giây
                Attack();
            }
            yield return new WaitForSeconds(attackInterval);
        }
    }

    // Phương thức để vô hiệu hóa tấn công khi nhân vật chết
    public void DisableAttack()
    {
        canAttack = false;
    }

    // Phương thức để kích hoạt lại tấn công nếu cần
    public void EnableAttack()
    {
        canAttack = true;
    }

    public void LevelUp()
    {
        level++;

        if (level == 2 && !doubleAttackEnabled)
        {
            // Enable double attack by instantiating a secondary attack area.
            doubleAttackEnabled = true;
            // Duplicate the primary attack area as a child of the hero.
            secondaryAttackArea = Instantiate(attackArea, transform);
            // Mirror its position horizontally (assumes original offset on x-axis)
            Vector3 secPos = attackArea.transform.localPosition;
            secPos.x = -secPos.x;
            secondaryAttackArea.transform.localPosition = secPos;
            // Flip the scale on the X-axis so that its animation appears flipped
            Vector3 flippedScale = originalAttackAreaScale;
            flippedScale.x = -Mathf.Abs(flippedScale.x);
            secondaryAttackArea.transform.localScale = flippedScale;
            // Ensure it is inactive initially.
            secondaryAttackArea.SetActive(false);

            // Increase damage by 20%
            damage = Mathf.RoundToInt(damage * 1.2f);

            Debug.Log("Leveled up to " + level + " with double attack enabled | Damage: " + damage);
        }
        else if (level > 2)
        {
            // For levels 3 and above, further scale the attack areas and increase damage.
            damage = Mathf.RoundToInt(damage * 1.2f);
            // Calculate scale factor based on levels beyond 2.
            float scaleFactor = 1f + 0.1f * (level - 2);
            // Scale primary attack area normally.
            attackArea.transform.localScale = originalAttackAreaScale * scaleFactor;
            // For the secondary area, ensure its X scale remains negative for a flip effect.
            if (secondaryAttackArea != null)
            {
                Vector3 newScale = originalAttackAreaScale * scaleFactor;
                newScale.x = -Mathf.Abs(newScale.x);
                secondaryAttackArea.transform.localScale = newScale;
            }
            Debug.Log("Leveled up to " + level + " | Damage: " + damage + " | Attack Area Scale: " + attackArea.transform.localScale);
        }
    }
}
