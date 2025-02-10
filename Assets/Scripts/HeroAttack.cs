using System.Collections;
using UnityEngine;

public class HeroAttack : MonoBehaviour
{
    private GameObject attackArea = default;

    private bool attacking = false;
    private bool canAttack = true;  // Kiểm tra khả năng tấn công

    private float timeToAttack = 0.25f; // Thời gian vùng tấn công tồn tại
    private float attackInterval = 1.2f; // Khoảng thời gian giữa các đợt tấn công
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        attackArea = transform.GetChild(0).gameObject;

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
                attackArea.SetActive(attacking);
            }
        }
    }

    private void Attack()
    {
        if (canAttack)  // Kiểm tra xem có thể tấn công hay không
        {
            attacking = true;
            attackArea.SetActive(attacking);
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
}
