using UnityEngine;
using UnityEngine.EventSystems;

public class SlashController : WeaponController
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }


    protected override void Attack()
    {
        base.Attack();
        Vector3 attackDirection = pm.lastMovedVector.normalized;
        GameObject spawnSlash = Instantiate(weaponData.Prefab);
            // Xoay đòn chém theo hướng ban đầu
        spawnSlash.transform.SetParent(transform, true);
        spawnSlash.transform.localPosition = Vector3.zero;
        spawnSlash.transform.rotation = transform.rotation;
        spawnSlash.GetComponent<SlashBehaviour>().DirectionChecker(pm.lastMovedVector);



    }
}
