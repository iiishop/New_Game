using UnityEngine;
using Weapons;

public class Rife : Weapon
{

    public override bool CheckFire()
    {
        var info = Program.Instance.Weapon_Now.Ani.GetCurrentAnimatorStateInfo(0);
        return !Is_Reloading && Now_Magazing > 0 && !info.IsName("Fire");
    }
}