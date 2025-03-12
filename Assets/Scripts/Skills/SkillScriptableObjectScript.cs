using UnityEngine;

public enum EffectDirection
{
    Increase, 
    Decrease 
}

public enum ValueType
{
    Flat, 
    Percentage
}

public enum AttackTargetType
{
    Slash,
    Shield,
    Knife,
    Not
}

public enum SkillEffectType
{
    CurrentHP, 
    Attack,
    Speed,
    MaxHP,
    AttachObject 
}

[CreateAssetMenu(fileName = "SkillScriptableObject", menuName = "ScriptableObjects/Skill")]
public class SkillScriptableObject : ScriptableObject
{
    public string skillName;
    public string skillDescription;
    public Sprite skillIcon;

    public SkillEffectType effectType;  // Chọn loại hiệu ứng (CurrentHP, Attack, v.v.)
    public float value;
    public ValueType valueType;
    public EffectDirection effectDirection;
    public AttackTargetType targetType;
    public GameObject objectToAttach;
    public float probability;
}
