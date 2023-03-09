using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Weapons.Weapon wp; //For test
    public static Signal Signal = new Signal();

    /// <summary>
    /// 调用这个函数会更换武器
    /// </summary>
    private void ChangeWeapon()
    {
        Program.Weapon_Now =
            Program.Weapon_Now == Program.Weapon_Bag[0] ? Program.Weapon_Bag[1] : Program.Weapon_Bag[0];
    }

    private void Start()
    {
        Program.Weapon_Now = wp; //For test
    }

    private void Update()
    {
        CheckAni();
        if (Input.GetMouseButton(0))
        {
            Program.Weapon_Now.Fire();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Program.Weapon_Now.Reload();
        }

        if (Input.GetMouseButtonUp(0))
        {
            Signal.Call();
        }
    }

    /// <summary>
    /// 这个函数用来检测动画是否播放完毕，如果播放完毕就会关闭动画
    /// </summary>
    private void CheckAni()
    {
        AnimatorStateInfo info = Program.Weapon_Now.Ani.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 1.0f)
        {
            if (info.IsName("Fire"))
            {
                Program.Weapon_Now.Ani.SetBool("Fire", false);
            }

            if (info.IsName("Reload"))
            {
                Program.Weapon_Now.Ani.SetBool("Reload", false);
                Program.Weapon_Now.Is_Reloading = false;
            }
        }
    }
}