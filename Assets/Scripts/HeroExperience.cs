using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroExperience : Singleton<HeroExperience>
{
    [SerializeField] private GameObject experienceBarPrefab; // Prefab của thanh EXP
    [SerializeField] private Canvas canvas; // Canvas để chứa thanh EXP
    private TMP_Text levelText;
    private GameObject experienceBarInstance; // Instance của thanh EXP
    private Image experienceBarImage; // Biến lưu trữ Image của thanh "Fill"
    [SerializeField] private int currentLevel = 1; // Cấp độ hiện tại
    [SerializeField] private int currentExperience = 0; // EXP hiện tại
    [SerializeField] private int experienceToNextLevel = 100; // EXP cần để lên cấp
    [SerializeField] private SkillSelectionManager skillSelectionManager;
    private void Start()
    {
        // Instantiate thanh EXP và gắn nó vào Canvas
        if (experienceBarPrefab != null && canvas != null)
        {
            experienceBarInstance = Instantiate(experienceBarPrefab, canvas.transform, false);

            // Lấy Image của phần "Fill" trong thanh EXP
            experienceBarImage = experienceBarInstance.transform.Find("Experience").GetComponent<Image>();

            levelText = experienceBarInstance.transform.Find("LevelText").GetComponent<TMP_Text>();

            UpdateExperienceBar();
            UpdateLevelText();
        }
    }


    private void Update()
    {
        // Đảm bảo thanh kinh nghiệm được cập nhật mỗi frame
        UpdateExperienceBar();
        UpdateLevelText();
    }

    // Cập nhật tiến độ thanh EXP 
    private void UpdateExperienceBar()
    {
        if (experienceBarImage != null)
        {
            experienceBarImage.fillAmount = Mathf.Clamp((float)currentExperience / experienceToNextLevel, 0f, 1f);
        }
    }

    // Cập nhật văn bản hiển thị cấp độ
    private void UpdateLevelText()
    {
        if (levelText != null)
        {
            levelText.text = $"Level {currentLevel}";
        }
        else
        {
            Debug.LogWarning("LevelText is null! Check your prefab setup.");
        }
    }

    // Thêm EXP
    public void AddExperience(int amount)
    {
        currentExperience += amount;

        // Kiểm tra nếu đủ EXP để lên cấp
        while (currentExperience >= experienceToNextLevel)
        {
            LevelUp();
        }

        UpdateExperienceBar();
    }

    // Xử lý lên cấp
    private void LevelUp()
    {
        currentLevel++; // Tăng cấp
        currentExperience -= experienceToNextLevel; // Trừ EXP đã tiêu tốn

        // Tăng yêu cầu EXP cho cấp tiếp theo
        experienceToNextLevel = Mathf.FloorToInt(experienceToNextLevel * 1.2f);

        // Hiển thị bảng chọn kỹ năng
        skillSelectionManager.ShowSkillSelectionPanel();

        // Cập nhật hiển thị Level
        UpdateLevelText();
    }
}
