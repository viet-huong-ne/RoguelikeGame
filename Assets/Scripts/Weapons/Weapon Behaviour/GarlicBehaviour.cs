using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarlicBehaviour : MeleeWeaponBehaviour
{
    List<GameObject> makedEnemies;
    protected override void Start()
    {
        base.Start();
        makedEnemies = new List<GameObject>();
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy") && !makedEnemies.Contains(col.gameObject))
        {
            EnemyStats enemy =  col.GetComponent<EnemyStats>();
            enemy.TakeDamage(currentDamage);

            makedEnemies.Add(col.gameObject);
        }
		if (col.CompareTag("BatLava") && !makedEnemies.Contains(col.gameObject))
		{
			BatStats enemy = col.GetComponent<BatStats>();
			enemy.TakeDamage(currentDamage);

			makedEnemies.Add(col.gameObject);
		}
		if (col.CompareTag("BringerOfDeath"))
        {
            BODStats enemy = col.GetComponent<BODStats>();
            enemy.TakeDamage(currentDamage);
            makedEnemies.Add(col.gameObject);
        }
		if (col.CompareTag("Impaler"))
		{
			ImpalerStats enemy = col.GetComponent<ImpalerStats>();
			enemy.TakeDamage(currentDamage);
			makedEnemies.Add(col.gameObject);
		}
	}
}
