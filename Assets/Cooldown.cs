using UnityEngine;
using UnityEngine.UI;

public class Cooldown : MonoBehaviour
{
    private Image abilityImage;
    private GameObject currentCooldownIcon;
    private Transform canvas; // Thêm biến canvas

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

        // Tìm Canvas trong scene nếu chưa được gán
        canvas = GameObject.Find("Canvas")?.transform;
        if (canvas == null)
        {
            Debug.LogError("Canvas không tìm thấy trong Scene!");
        }

        // Nếu cooldown icon đã tồn tại, đặt fillAmount về 0
        if (abilityImage != null)
            abilityImage.fillAmount = 0;
    }

    void Update()
    {
        if (isCooldown)
        {
            currentCooldown -= Time.deltaTime;
            if (abilityImage != null)
                abilityImage.fillAmount = Mathf.Clamp01(currentCooldown / cooldownTime);

            if (currentCooldown <= 0)
            {
                isCooldown = false;
                if (abilityImage != null)
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

    public void ShowCooldownIcon()
    {
        if (dashIconPrefab != null && dashIconCDPrefab != null && canvas != null)
        {
            GameObject dashIcon = Instantiate(dashIconPrefab, canvas, false);
            currentCooldownIcon = Instantiate(dashIconCDPrefab, canvas, false);

            // Reset vị trí để đảm bảo nó xuất hiện đúng trên UI
            dashIcon.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            currentCooldownIcon.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            abilityImage = currentCooldownIcon.GetComponent<Image>();
            if (abilityImage != null)
                abilityImage.fillAmount = 0;
        }
        else
        {
            Debug.LogError("DashIconPrefab hoặc DashIconCDPrefab chưa được gán hoặc Canvas không tồn tại!");
        }
    }
}
