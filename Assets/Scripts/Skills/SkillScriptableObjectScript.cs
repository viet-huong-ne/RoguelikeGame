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

public enum SkillEffectType
{
    CurrentHP, 
    Attack,
    Speed,
    MaxHP
}

[CreateAssetMenu(fileName = "SkillScriptableObject", menuName = "ScriptableObjects/Skill")]
public class SkillScriptableObject : ScriptableObject
{
    public string skillName;
    public string skillDescription;
    public Sprite skillIcon;

    public SkillEffectType effectType;
    public float value;
    public ValueType valueType;
    public EffectDirection effectDirection;

    public float probability;
}
