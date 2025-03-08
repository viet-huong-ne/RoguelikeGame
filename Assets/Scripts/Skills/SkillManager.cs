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
                validSkills.Add(skill);
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

        // Trả về null nếu không có kỹ năng nào hợp lệ
        return null;
    }

    public int GetSkillCount()
    {
        return skills.Length;
    }
}
