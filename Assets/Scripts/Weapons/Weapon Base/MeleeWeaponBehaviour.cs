using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// vu khi can chien
/// </summary>
public class MeleeWeaponBehaviour : MonoBehaviour
{
    public WeaponScriptableObject weaponData;
    public float destroyAfterSeconds;
    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCooldownDuration;
    protected int currentPierce;

    void Awake()
    {
        currentCooldownDuration = weaponData.CooldownDuration;
        currentDamage = weaponData.Damage;
        currentSpeed = weaponData.Speed;
        currentPierce = weaponData.Pierce;
    }
    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(currentDamage);
        }
        if (col.CompareTag("BringerOfDeath"))
        {
            BODStats enemy = col.GetComponent<BODStats>();
            enemy.TakeDamage(currentDamage);
        }
		if (col.CompareTag("Impaler"))
		{
			ImpalerStats enemy = col.GetComponent<ImpalerStats>();
			enemy.TakeDamage(currentDamage);
		}
	}
}
