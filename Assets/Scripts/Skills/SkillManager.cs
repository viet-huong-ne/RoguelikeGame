using UnityEngine;
using System.Collections.Generic;

public class SkillManager : Singleton<SkillManager>
{
    public static SkillManager Instance;

    private SkillScriptableObject[] skills;

    private void Awake()
    {
        Instance = this;

        // Load tất cả SkillScriptableObjects từ Resources/Skills
        skills = Resources.LoadAll<SkillScriptableObject>("Skills");

        Debug.Log($"Loaded {skills.Length} skills from Resources/Skills.");
    }

    // Lấy một skill ngẫu nhiên và kiểm tra tỉ lệ xác suất
    public SkillScriptableObject GetSkill()
    {
        // Tạo danh sách các kỹ năng hợp lệ theo xác suất
        List<SkillScriptableObject> validSkills = new List<SkillScriptableObject>();

        foreach (SkillScriptableObject skill in skills)
        {
            float roll = Random.Range(0f, 1f);
            if (roll <= skill.probability)
            {
                // Kiểm tra xem kỹ năng có liên quan đến TargetType là Slash, Garlic, hoặc Knife
                if (skill.targetType == AttackTargetType.Slash)
                {
                    // Kiểm tra nếu SlashController đã được gắn vào HeroKnight
                    HeroKnight heroKnight = FindObjectOfType<HeroKnight>();
                    if (heroKnight != null && heroKnight.GetComponentInChildren<SlashController>() != null)
                    {
                        validSkills.Add(skill); // Thêm kỹ năng vào danh sách hợp lệ
                    }
                }
                else if (skill.targetType == AttackTargetType.Shield)
                {
                    // Kiểm tra nếu ShieldController đã được gắn vào HeroKnight
                    HeroKnight heroKnight = FindObjectOfType<HeroKnight>();
                    if (heroKnight != null && heroKnight.GetComponentInChildren<GarlicController>() != null)
                    {
                        validSkills.Add(skill); // Thêm kỹ năng vào danh sách hợp lệ
                    }
                }
                else if (skill.targetType == AttackTargetType.Knife)
                {
                    // Kiểm tra nếu KnifeController đã được gắn vào HeroKnight
                    HeroKnight heroKnight = FindObjectOfType<HeroKnight>();
                    if (heroKnight != null && heroKnight.GetComponentInChildren<KnifeController>() != null)
                    {
                        validSkills.Add(skill); // Thêm kỹ năng vào danh sách hợp lệ
                    }
                }
                else
                {
                    validSkills.Add(skill); // Thêm kỹ năng vào danh sách hợp lệ nếu không có target type đặc biệt
                }
            }
        }

        // Nếu có kỹ năng hợp lệ
        if (validSkills.Count > 0)
        {
            // Chọn một kỹ năng ngẫu nhiên từ danh sách các kỹ năng hợp lệ
            SkillScriptableObject selectedSkill = validSkills[Random.Range(0, validSkills.Count)];
            Debug.Log($"Skill {selectedSkill.skillName} selected with probability {selectedSkill.probability}");
            return selectedSkill;
        }

        // Nếu không có kỹ năng nào hợp lệ, chọn một kỹ năng ngẫu nhiên từ toàn bộ danh sách
        SkillScriptableObject fallbackSkill = skills[Random.Range(0, GetSkillCount())];
        Debug.LogWarning($"No valid skill found. Fallback to skill {fallbackSkill.skillName}");
        return fallbackSkill;
    }

    public int GetSkillCount()
    {
        return skills.Length;
    }
}
