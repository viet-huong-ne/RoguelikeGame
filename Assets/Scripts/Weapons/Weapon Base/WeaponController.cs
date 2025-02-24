using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
/// <summary>
/// Base scripts for all weapon controller
/// </summary>
public class WeaponController : MonoBehaviour
{
    [Header("Weapon Stats")]
    public WeaponScriptableObject weaponData;
    float currentCooldown;

    protected HeroKnight pm;
    protected virtual void Start()
    {
        pm = FindObjectOfType<HeroKnight>();
        currentCooldown = weaponData.cooldownDuration; // at the start set the current cooldown to be the cooldown duration
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0f) {
            Attack();
        }
    }
    protected virtual void Attack() 
    {
        currentCooldown = weaponData.cooldownDuration;
    }
}
