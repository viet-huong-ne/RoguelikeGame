using UnityEngine;

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

    public SkillScriptableObject GetSkill(int index)
    {
        if (index >= 0 && index < skills.Length)
        {
            return skills[index];
        }

        return null;
    }

    public int GetSkillCount()
    {
        return skills.Length;
    }
}
