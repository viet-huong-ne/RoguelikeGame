using UnityEngine;
using UnityEngine.EventSystems;

public class SlashController : WeaponController
{
    // Biến lưu giá trị gốc
    private float originalDamage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        // Lưu giá trị gốc ban đầu
        originalDamage = weaponData.damage;
    }

    public void ApplyEffect(SkillScriptableObject skill)
    {
        // Chỉ thay đổi giá trị trong game, không thay đổi ScriptableObject
        if (skill.effectDirection == EffectDirection.Increase)
        {
            SoundEffectManager.Instance.PlaySoundEffect(Resources.Load<AudioClip>("SoundEffects/DrawSword"), 1f);
            if (skill.valueType == ValueType.Flat)
            {
                weaponData.damage += skill.value;
            }
            else if (skill.valueType == ValueType.Percentage)
            {
                weaponData.damage += weaponData.damage * (skill.value / 100f);
            }
        }
        else if (skill.effectDirection == EffectDirection.Decrease)
        {
            SoundEffectManager.Instance.PlaySoundEffect(Resources.Load<AudioClip>("SoundEffects/SwordDrop"), 1f);
            if (skill.valueType == ValueType.Flat)
            {
                weaponData.damage -= skill.value;
            }
            else if (skill.valueType == ValueType.Percentage)
            {
                weaponData.damage -= weaponData.damage * (skill.value / 100f);
            }
        }

        // Debug log để kiểm tra kết quả
        Debug.Log($"Updated Damage: {weaponData.damage}");
    }

    // Phương thức để reset lại giá trị gốc khi kết thúc game
    public void ResetDamage()
    {
        weaponData.damage = originalDamage;
        Debug.Log($"Reset Damage to Original: {weaponData.damage}");
    }

    protected override void Attack()
    {
        SoundEffectManager.Instance.PlaySoundEffect(Resources.Load<AudioClip>("SoundEffects/Slash"), 1f);
        base.Attack();
        Vector3 attackDirection = pm.lastMovedVector.normalized;
        GameObject spawnSlash = Instantiate(weaponData.Prefab);
        spawnSlash.transform.SetParent(transform, true);
        spawnSlash.transform.localPosition = Vector3.zero;
        spawnSlash.transform.rotation = transform.rotation;
        spawnSlash.GetComponent<SlashBehaviour>().DirectionChecker(pm.lastMovedVector);
    }
}
