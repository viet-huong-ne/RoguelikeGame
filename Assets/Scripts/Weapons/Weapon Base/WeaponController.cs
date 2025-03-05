using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Stats")]
    public WeaponScriptableObject weaponData ;
    float currentCooldown = 0.5f;

    protected HeroKnight pm;
    private HeroHealth heroHealth; // Reference to HeroHealth

    protected virtual void Start()
    {
        if (weaponData == null)
        {
            Debug.LogError("WeaponController: weaponData chưa được gán! Hãy kiểm tra Inspector.");
            return;
        }
        pm = FindObjectOfType<HeroKnight>();
        heroHealth = GetComponentInParent<HeroHealth>();
        currentCooldown = weaponData.cooldownDuration;
    }

    protected virtual void Update()
    {
        if (heroHealth != null && heroHealth.IsDead())
        {
            return; // Do nothing if the hero is dead
        }

        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0f)
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        currentCooldown = weaponData.cooldownDuration;
    }
}
