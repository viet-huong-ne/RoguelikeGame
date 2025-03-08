using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// Base script of all projectile behaviours [to be placed on a prefab of a weapon that is a projectile]
/// </summary>
public class ProjectileWeaponBehaviour : MonoBehaviour
{
    public WeaponScriptableObject weaponData;
    protected Vector3 direction;
    public float destroyAfterSeconds;

    //current stats
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
    public void DirectionChecker(Vector3 dir)
    {
        direction = dir;
        float dirx = direction.x;
        float diry = direction.y;
        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.rotation.eulerAngles;

        if (dirx < 0 && diry == 0) //left
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
        }
        else if (dirx == 0 && diry < 0) //down
        {
            //scale.y *= -1;
            rotation.z = 0f;
        }
        else if (dirx == 0 && diry > 0) //top
        {
            rotation.z = 180f;
        }
        else if (dirx > 0 && diry > 0) //cross right top
        {
            rotation.z = 135f;
        }
        else if (dirx > 0 && diry < 0) //cross right bot
        {
            rotation.z = 45f;
        }
        else if (dirx < 0 && diry > 0) //cross left top
        {
            rotation.z = -135f;
        }
        else if (dirx < 0 && diry < 0) // left bot
        {

            rotation.z = -45f;
        }
        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(rotation);
    }
    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(currentDamage);
            ReducePierce();
        }
        if (col.CompareTag("BringerOfDeath"))
        {
            BODStats enemy = col.GetComponent<BODStats>();
            enemy.TakeDamage(currentDamage);
            ReducePierce();
        }
    }
    void ReducePierce()
    {
        currentPierce--;
        if (currentPierce <= 0)
        {
            Destroy(gameObject);
        }
    }
}
