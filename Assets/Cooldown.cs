using UnityEngine;
using UnityEngine.UI;

public class Cooldown : MonoBehaviour
{
    private Image abilityImage;
    private GameObject currentCooldownIcon;

    [SerializeField]
    private GameObject dashIconPrefab; // Prefab cho background
    [SerializeField]
    private GameObject dashIconCDPrefab; // Prefab cho filled icon

    private float cooldownTime;
    private float currentCooldown;
    private bool isCooldown;

    void Start()
    {
        abilityImage = null; // Reset để đảm bảo không dùng dữ liệu cũ

        // Nếu cooldown icon đã tồn tại, đặt fillAmount về 0
        if (abilityImage != null)
            abilityImage.fillAmount = 0;
    }

    void Update()
    {
        if (isCooldown)
        {
            currentCooldown -= Time.deltaTime;
            abilityImage.fillAmount = Mathf.Clamp01(currentCooldown / cooldownTime);

            if (currentCooldown <= 0)
            {
                isCooldown = false;
                abilityImage.fillAmount = 0;
            }
        }
    }

    public void StartCooldown(float cooldownDuration)
    {
        // Đặt fillAmount về 0 trước khi bắt đầu cooldown
        if (abilityImage != null)
            abilityImage.fillAmount = 0;

        cooldownTime = cooldownDuration;
        currentCooldown = cooldownTime;
        isCooldown = true;
    }

    public void ShowCooldownIcon(Transform parentTransform)
    {
        if (dashIconPrefab != null && dashIconCDPrefab != null)
        {
            // Tạo icon background
            GameObject dashIcon = Instantiate(dashIconPrefab, parentTransform);

            // Tạo icon filled và gắn nó vào icon background
            currentCooldownIcon = Instantiate(dashIconCDPrefab, parentTransform);

            // Gắn Image component từ prefab để quản lý cooldown
            abilityImage = currentCooldownIcon.GetComponent<Image>();

            // Đặt fillAmount về 0 khi khởi tạo
            if (abilityImage != null)
                abilityImage.fillAmount = 0;
        }
        else
        {
            Debug.LogError("DashIconPrefab hoặc DashIconCDPrefab chưa được gán!");
        }
    }
}
