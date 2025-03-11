using UnityEngine;
using UnityEngine.UI;

public class Cooldown : MonoBehaviour
{
    [SerializeField]
    private Image abilityImage;
    private float cooldownTime;
    private float currentCooldown;
    private bool isCooldown;

    public KeyCode ability;

    void Start()
    {
        abilityImage.fillAmount = 0;
    }

    void Update()
    {
        if (isCooldown)
        {
            // Làm đầy thanh cooldown UI dựa trên thời gian trôi qua
            currentCooldown -= Time.deltaTime;
            abilityImage.fillAmount = Mathf.Clamp01(currentCooldown / cooldownTime);  // Đảm bảo giá trị fillAmount từ 0 đến 1

            // Khi cooldown hết, reset trạng thái UI
            if (currentCooldown <= 0)
            {
                isCooldown = false;
                abilityImage.fillAmount = 0;
            }
        }
    }

    // Phương thức này được gọi từ FastFoot script để bắt đầu cooldown
    public void StartCooldown(float cooldownDuration)
    {
        cooldownTime = cooldownDuration;
        currentCooldown = cooldownTime;
        isCooldown = true;  // Bắt đầu thời gian cooldown
    }
}
