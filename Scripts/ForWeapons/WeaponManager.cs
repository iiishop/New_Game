using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;
public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;
    /// <summary>
    /// 表示游戏中现存的所有武器
    /// </summary>
    /// <typeparam name="Weapon"></typeparam>
    /// <returns></returns>
    public List<Weapon> Weapon_Inventory = new List<Weapon>();
    public Weapons.Weapon wp; //For test
    public static Signal Signal = new Signal();


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// 调用这个函数会更换武器
    /// </summary>
    private void ChangeWeapon()
    {
        Program.Instance.Weapon_Now =
            Program.Instance.Weapon_Now == Program.Instance.Weapon_Bag[0] ? Program.Instance.Weapon_Bag[1] : Program.Instance.Weapon_Bag[0];
    }

    private void Start()
    {
        Program.Instance.Weapon_Now = wp; //For test
    }

    private void Update()
    {
        CheckAni();
        if (Input.GetKey(Program.Instance.Program_Save_Object.Key_Settings["Shoot"]) && Input.GetKey(Program.Instance.Program_Save_Object.Key_Settings["Aim"]))
        {
            Program.Instance.Weapon_Now.Fire();
        }

        if (Input.GetKeyDown(Program.Instance.Program_Save_Object.Key_Settings["Reload"]))
        {
            Program.Instance.Weapon_Now.Reload();
        }

        if (Input.GetKeyUp(Program.Instance.Program_Save_Object.Key_Settings["Shoot"]))
        {
            Signal.Call();
        }
        if (Input.GetKey(Program.Instance.Program_Save_Object.Key_Settings["Aim"]))
        {
            Program.Instance.Weapon_Now.ROT();
        }
        else
        {
            Program.Instance.Weapon_Now.ROTM();
        }
    }

    /// <summary>
    /// 这个函数用来检测动画是否播放完毕，如果播放完毕就会关闭动画
    /// </summary>
    private void CheckAni()
    {
        AnimatorStateInfo info = Program.Instance.Weapon_Now.Ani.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 1.0f)
        {
            if (info.IsName("Fire"))
            {
                Program.Instance.Weapon_Now.Ani.SetBool("Fire", false);
            }

            if (info.IsName("Reload"))
            {
                Program.Instance.Weapon_Now.Ani.SetBool("Reload", false);
                Program.Instance.Weapon_Now.Is_Reloading = false;
                Program.Instance.Weapon_Now.Reload_Ani_Time_Left = Program.Instance.Weapon_Now.Reload_Ani_Time;
            }
        }
    }
}