using System.Collections.Generic;
using UnityEngine;

public class SkillSelectionManager : Singleton<SkillSelectionManager>
{
    [SerializeField] private GameObject skillSelectionPanelPrefab;
    private GameObject skillSelectionPanelInstance;
    [SerializeField]
    private GameObject heroKnight;
    private bool isSkillSelectionActive = false;
    // Tham chiếu tới Canvas
    private Canvas canvas;

    // Tham chiếu tới script Cooldown
    private Cooldown cooldownUI;

    // Danh sách để theo dõi các kỹ năng đã chọn
    private List<SkillScriptableObject> selectedSkills = new List<SkillScriptableObject>();
    [SerializeField] private HeroExperience heroExperience;

    public bool IsSkillSelectionActive()
    {
        return isSkillSelectionActive;
    }

    public void ShowSkillSelectionPanel()
    {
        if (isSkillSelectionActive)
        {
            Debug.Log("Skill selection panel is already active.");
            return;
        }

        // Tạm dừng game
        Time.timeScale = 0f;

        // Tạm dừng tất cả âm thanh
        PauseAllSoundEffects();

        // Tạo panel chọn kỹ năng
        skillSelectionPanelInstance = Instantiate(skillSelectionPanelPrefab, FindObjectOfType<Canvas>().transform);
        isSkillSelectionActive = true;

        Debug.Log("Skill selection panel created successfully.");

        // Lấy các nút SkillButton trong panel
        SkillButton[] skillButtons = skillSelectionPanelInstance.GetComponentsInChildren<SkillButton>();
        Debug.Log($"Found {skillButtons.Length} skill buttons.");

        if (skillButtons == null || skillButtons.Length != 3)
        {
            Debug.LogError($"Skill selection panel must have exactly 3 skill buttons. Found: {skillButtons?.Length ?? 0}");
            return;
        }

        // Lấy danh sách kỹ năng từ SkillManager
        int skillCount = SkillManager.Instance.GetSkillCount();

        for (int i = 0; i < skillButtons.Length; i++)
        {
            if (i < skillCount)
            {
                // Lấy một kỹ năng ngẫu nhiên mà chưa được chọn
                var skill = SkillManager.Instance.GetSkill();
                
                // Nếu kỹ năng đã được chọn trước đó, tiếp tục tìm kỹ năng khác
                while (selectedSkills.Contains(skill))
                {
                    skill = SkillManager.Instance.GetSkill();
                }

                // Gán dữ liệu kỹ năng
                skillButtons[i].Setup(skill, this);
                selectedSkills.Add(skill); // Thêm kỹ năng vào danh sách đã chọn
                Debug.Log($"Skill button {i} set up with skill: {skill.skillName}");
            }
            else
            {
                // Không đủ kỹ năng, nút dư thừa sẽ bị vô hiệu hóa
                skillButtons[i].Clear();
                Debug.Log($"Skill button {i} has no skill assigned.");
            }
        }
    }

    public void HideSkillSelectionPanel()
    {
        if (skillSelectionPanelInstance != null)
        {
            Destroy(skillSelectionPanelInstance);
        }

        // Tiếp tục game
        Time.timeScale = 1f;
        isSkillSelectionActive = false;
        selectedSkills.Clear(); // Xóa danh sách kỹ năng đã chọn khi ẩn panel
    }

    public void SelectSkill(SkillScriptableObject skill)
    {
        Debug.Log($"Selected skill: {skill.skillName}");

        ApplySkillEffect(skill);

        HideSkillSelectionPanel();

        ResumeAllSoundEffects();

        // Tiếp tục trò chơi
        Time.timeScale = 1f;

        // Gọi bảng kỹ năng tiếp theo nếu có
        if (HeroExperience.Instance.HasSkillSelectionInQueue())
        {
            HeroExperience.Instance.ProcessNextSkillSelection();
        }
    }

    public void ApplySkillEffect(SkillScriptableObject skill)
    {
        // Kiểm tra kỹ năng có hiệu lực hay không
        if (skill == null)
        {
            Debug.LogWarning("Invalid skill selected");
            return;
        }

        // Lấy kiểu hiệu ứng
        switch (skill.effectType)
        {
            case SkillEffectType.CurrentHP:
                ApplyHPEffect(skill);
                break;

            case SkillEffectType.MaxHP:
                ApplyMaxHPEffect(skill);
                break;

            case SkillEffectType.Attack:
                ActivateSkill(skill);
                break;

            case SkillEffectType.Speed:
                ApplySpeedEffect(skill);
                break;

            case SkillEffectType.AttachObject:  // Gắn GameObject vào player
                AttachObjectToPlayer(skill);
                break;
        }
    }

    private void AttachObjectToPlayer(SkillScriptableObject skill)
    {
        SoundEffectManager.Instance.PlaySoundEffect(Resources.Load<AudioClip>("SoundEffects/Attach"), 1f);

        // Kiểm tra nếu chưa gán canvas
        if (canvas == null)
        {
            canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("Canvas không tồn tại trong scene!");
                return;
            }
        }

        // Kiểm tra nếu chưa gán CooldownUI
        if (cooldownUI == null)
        {
            cooldownUI = canvas.GetComponentInChildren<Cooldown>();
            if (cooldownUI == null)
            {
                Debug.LogError("CooldownUI không tồn tại trong Canvas!");
                return;
            }
        }

        // Kiểm tra HeroKnight và object của skill
        if (heroKnight != null && skill.objectToAttach != null)
        {
            // Kiểm tra nếu kỹ năng đã được gắn
            if (SkillManager.Instance.IsSkillAttached(skill))
            {
                Debug.LogWarning($"Skill {skill.skillName} đã được gắn. Không thể gắn lại.");
                return;
            }

            // Instantiate object và gắn vào HeroKnight
            GameObject obj = Instantiate(skill.objectToAttach, heroKnight.transform.position, Quaternion.identity);

            // Gắn đối tượng vào HeroKnight
            obj.transform.SetParent(heroKnight.transform);
            obj.transform.localPosition = Vector3.zero;

            // Thêm kỹ năng vào danh sách attachedSkills
            SkillManager.Instance.AddAttachedSkill(skill);

            // Hiển thị icon cooldown
            cooldownUI.ShowCooldownIcon(canvas.transform);

            Debug.Log($"Skill {skill.skillName} đã được gắn thành công vào HeroKnight.");
        }
        else
        {
            Debug.LogWarning("No HeroKnight or no object to attach in skill: " + skill.skillName);
        }
    }

    private void ApplyHPEffect(SkillScriptableObject skill)
    {
        // Kiểm tra hướng tác động (tăng hay giảm)
        if (skill.effectDirection == EffectDirection.Increase)
        {
            SoundEffectManager.Instance.PlaySoundEffect(Resources.Load<AudioClip>("SoundEffects/Heal"), 1f);
            // Kiểm tra giá trị là Flat hay Percentage
            if (skill.valueType == ValueType.Flat)
            {
                HeroHealth.Instance.Heal((int)skill.value); // Thêm máu cố định
            }
            else if (skill.valueType == ValueType.Percentage)
            {
                int amount = (int)(HeroHealth.Instance.MAX_HEALTH * (skill.value / 100f));
                HeroHealth.Instance.Heal(amount);
            }
        }
        else if (skill.effectDirection == EffectDirection.Decrease)
        {
            SoundEffectManager.Instance.PlaySoundEffect(Resources.Load<AudioClip>("SoundEffects/Hurt"), 1f);
            // Kiểm tra giá trị là Flat hay Percentage
            if (skill.valueType == ValueType.Flat)
            {
                HeroHealth.Instance.TakeDamage((int)skill.value);
            }
            else if (skill.valueType == ValueType.Percentage)
            {
                int amount = (int)(HeroHealth.Instance.health * (skill.value / 100f));
                HeroHealth.Instance.TakeDamage(amount);
            }
        }
    }

    private void ApplyMaxHPEffect(SkillScriptableObject skill)
    {
        // Tăng/giảm MaxHP
        if (skill.effectDirection == EffectDirection.Increase)
        {
            SoundEffectManager.Instance.PlaySoundEffect(Resources.Load<AudioClip>("SoundEffects/IncreaseMaxHealth"), 1f);
            float previousMaxHealth = HeroHealth.Instance.MAX_HEALTH; // Lưu MaxHP trước khi thay đổi

            if (skill.valueType == ValueType.Flat)
            {
                HeroHealth.Instance.MAX_HEALTH += (int)skill.value; // Tăng MaxHP cố định
            }
            else if (skill.valueType == ValueType.Percentage)
            {
                HeroHealth.Instance.MAX_HEALTH += (int)(HeroHealth.Instance.MAX_HEALTH * (skill.value / 100f)); // Tăng MaxHP theo phần trăm
            }

            // Nếu MaxHP tăng, cập nhật Current HP nếu cần
            if (HeroHealth.Instance.health == previousMaxHealth) 
            {
                // Tăng Current HP tương ứng với tỷ lệ thay đổi của MaxHP
                HeroHealth.Instance.health = (int)(HeroHealth.Instance.health * (HeroHealth.Instance.MAX_HEALTH / previousMaxHealth));
            }

            // Cập nhật lại Current HP nếu cần, đảm bảo không vượt quá MaxHP mới
            if (HeroHealth.Instance.health > HeroHealth.Instance.MAX_HEALTH)
            {
                HeroHealth.Instance.health = HeroHealth.Instance.MAX_HEALTH;
            }
        }
        else if (skill.effectDirection == EffectDirection.Decrease)
        {
            SoundEffectManager.Instance.PlaySoundEffect(Resources.Load<AudioClip>("SoundEffects/DecreaseMaxHealth"), 1f);
            if (skill.valueType == ValueType.Flat)
            {
                HeroHealth.Instance.MAX_HEALTH -= (int)skill.value; // Giảm MaxHP cố định
            }
            else if (skill.valueType == ValueType.Percentage)
            {
                HeroHealth.Instance.MAX_HEALTH -= (int)(HeroHealth.Instance.MAX_HEALTH * (skill.value / 100f)); // Giảm MaxHP theo phần trăm
            }

            // Cập nhật lại Current HP nếu cần
            if (HeroHealth.Instance.health > HeroHealth.Instance.MAX_HEALTH)
            {
                HeroHealth.Instance.health = HeroHealth.Instance.MAX_HEALTH; // Đảm bảo Current HP không vượt quá MaxHP mới
            }
        }
    }

    void ActivateSkill(SkillScriptableObject skill)
    {
        // Giả sử SlashController, ShieldController, KnifeController được tham chiếu
        SlashController slashController = FindObjectOfType<SlashController>();
        GarlicController garlicController = FindObjectOfType<GarlicController>();
        KnifeController knifeController = FindObjectOfType<KnifeController>();

        // Kiểm tra TargetType và gọi phương thức thích hợp
        if (skill.targetType == AttackTargetType.Slash && slashController != null)
        {
            slashController.ApplyEffect(skill);
        }
        else if (skill.targetType == AttackTargetType.Shield && garlicController != null)
        {
            garlicController.ApplyEffect(skill);
        }
        else if (skill.targetType == AttackTargetType.Knife && knifeController != null)
        {
            knifeController.ApplyEffect(skill);
        }
        else
        {
            Debug.LogWarning("No suitable controller found for the selected target type.");
        }
    }

    private void ApplySpeedEffect(SkillScriptableObject skill)
    {
        if (skill.effectDirection == EffectDirection.Increase)
        {
            SoundEffectManager.Instance.PlaySoundEffect(Resources.Load<AudioClip>("SoundEffects/SpeedUp"), 1f);
            // Tăng tốc độ (cố định hay phần trăm)
            if (skill.valueType == ValueType.Flat)
            {
                HeroKnight.Instance.speed += skill.value; // Tăng tốc độ cố định
            }
            else if (skill.valueType == ValueType.Percentage)
            {
                HeroKnight.Instance.speed += HeroKnight.Instance.speed * (skill.value / 100f); // Tăng tốc độ theo phần trăm
            }
        }
        else if (skill.effectDirection == EffectDirection.Decrease)
        {
            SoundEffectManager.Instance.PlaySoundEffect(Resources.Load<AudioClip>("SoundEffects/SlowDown"), 1f);
            if (skill.valueType == ValueType.Flat)
            {
                HeroKnight.Instance.speed -= skill.value;
            }
            else if (skill.valueType == ValueType.Percentage)
            {
                HeroKnight.Instance.speed -= HeroKnight.Instance.speed * (skill.value / 100f); // Giảm tốc độ theo phần trăm
            }
        }
    }

    // Tạm dừng tất cả hiệu ứng âm thanh, trừ nhạc nền
    private void PauseAllSoundEffects()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in allAudioSources)
        {
            // Giả sử nhạc nền được gắn tag "BackgroundMusic"
            if (audioSource.CompareTag("BackgroundMusic"))
            {
                continue; // Bỏ qua AudioSource của nhạc nền
            }

            if (audioSource.isPlaying) // Chỉ tạm dừng các AudioSource đang phát
            {
                audioSource.Pause();
                Debug.Log($"Paused sound effect: {audioSource.clip?.name ?? "Unnamed Clip"}");
            }
        }
    }

    private void ResumeAllSoundEffects()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in allAudioSources)
        {
            if (!audioSource.isPlaying) // Chỉ tiếp tục các AudioSource bị tạm dừng
            {
                audioSource.UnPause();
                Debug.Log($"Resumed sound effect: {audioSource.clip?.name ?? "Unnamed Clip"}");
            }
        }
    }
}
