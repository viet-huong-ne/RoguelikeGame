using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeController : WeaponController
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
        base.Attack();
        GameObject spawnKnife = Instantiate(weaponData.Prefab);
        spawnKnife.transform.position = transform.position;
        spawnKnife.GetComponent<KnifeBehaviour>().DirectionChecker(pm.lastMovedVector);
    }
}
