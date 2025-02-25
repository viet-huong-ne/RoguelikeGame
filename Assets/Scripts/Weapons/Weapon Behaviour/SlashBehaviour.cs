using System.Collections.Generic;
using UnityEngine;

public class SlashBehaviour : MeleeWeaponBehaviour
{
    protected Vector3 direction;
    protected override void Start()
    {
        base.Start();
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
        }
        
        transform.localScale = scale;
        //transform.rotation = Quaternion.Euler(rotation);
    }
}
