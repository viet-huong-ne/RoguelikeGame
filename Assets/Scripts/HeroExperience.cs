using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class HeroExperience : Singleton<HeroExperience>
{
    [SerializeField] private GameObject experienceBarPrefab; // Prefab của thanh EXP
    [SerializeField] private Canvas canvas; // Canvas để chứa thanh EXP
    [SerializeField] private GameObject knife;
    [SerializeField] private GameObject garlic;
    private TMP_Text levelText;
    private GameObject experienceBarInstance; // Instance của thanh EXP
    private Image experienceBarImage; // Biến lưu trữ Image của thanh "Fill"
    [SerializeField] private int currentLevel = 1; // Cấp độ hiện tại
    [SerializeField] private int currentExperience = 0; // EXP hiện tại
    [SerializeField] private int experienceToNextLevel = 100; // EXP cần để lên cấp
    [SerializeField] private SkillSelectionManager skillSelectionManager;
    private Queue<Action> skillSelectionQueue = new Queue<Action>();
    private bool isWaitingForNextSkill = false;
    private bool isFirstSkillSelection = true;

    private void Start()
    {
        if (knife != null)
        {
            knife.SetActive(false);
        }

        if (garlic != null)
        {
            garlic.SetActive(false);
        }
        
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

    public int GetHeroLevel()
    {
        return currentLevel;
    }

    public void AddExperience(int amount)
    {
        currentExperience += amount;

        // Kiểm tra nếu đủ EXP để lên cấp
        while (currentExperience >= experienceToNextLevel)
        {
            LevelUp();
        }

        UpdateExperienceBar();

        // Nếu không chờ bảng kỹ năng tiếp theo và không có bảng kỹ năng đang hiển thị, xử lý tiếp
        if (skillSelectionQueue.Count > 0 && !isWaitingForNextSkill && !SkillSelectionManager.Instance.IsSkillSelectionActive())
        {
            ProcessNextSkillSelection();
        }
    }

    private void LevelUp()
    {
        
        currentLevel++; // Tăng cấp
        currentExperience -= experienceToNextLevel; // Trừ EXP đã tiêu tốn

        // Tăng yêu cầu EXP cho cấp tiếp theo
        experienceToNextLevel = Mathf.FloorToInt(experienceToNextLevel * 1.2f);

        // Thêm bảng chọn kỹ năng vào hàng đợi
        skillSelectionQueue.Enqueue(() =>
        {
            skillSelectionManager.ShowSkillSelectionPanel();
        });

        UpdateWeapons();
        UpdateLevelText();
    }

    public void ProcessNextSkillSelection()
    {
        if (skillSelectionQueue.Count > 0)
        {
            if (isFirstSkillSelection)
            {
                // Hiển thị ngay lập tức lần đầu tiên
                isFirstSkillSelection = false;
                skillSelectionQueue.Dequeue()?.Invoke();
            }
            else
            {
                // Các lần sau thì chờ 0.5 giây
                StartCoroutine(ShowNextSkillSelectionWithDelay());
            }
        }
    }

    private IEnumerator ShowNextSkillSelectionWithDelay()
    {
        SoundEffectManager.Instance.PlaySoundEffect(Resources.Load<AudioClip>("SoundEffects/LevelUp"), 1f);
        isWaitingForNextSkill = true;

        yield return new WaitForSeconds(0.5f); // Chờ 0.5 giây

        skillSelectionQueue.Dequeue()?.Invoke();
        isWaitingForNextSkill = false;
    }

    public bool HasSkillSelectionInQueue()
    {
        return skillSelectionQueue.Count > 0;
    }

    void UpdateWeapons()
    {
        bool knifeShouldBeActive = currentLevel >= 4;
        if (knife.activeSelf != knifeShouldBeActive)
        {
            knife.SetActive(knifeShouldBeActive);
            if (knifeShouldBeActive)
            {
                SoundEffectManager.Instance.PlaySoundEffect(Resources.Load<AudioClip>("SoundEffects/Attach"), 1f);
            }
        }

        bool garlicShouldBeActive = currentLevel >= 8;
        if (garlic.activeSelf != garlicShouldBeActive)
        {
            garlic.SetActive(garlicShouldBeActive);
            if (garlicShouldBeActive)
            {
                SoundEffectManager.Instance.PlaySoundEffect(Resources.Load<AudioClip>("SoundEffects/Attach"), 1f);
            }
        }
    }
}
