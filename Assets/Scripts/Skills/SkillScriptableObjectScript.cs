using UnityEngine;

[CreateAssetMenu(fileName = "SkillScriptableObject", menuName = "ScriptableObjects/Skill")]
public class SkillScriptableObject : ScriptableObject
{
    public string skillName;
    public string skillDescription;
    public Sprite skillIcon;
}
