using System;
using Unity.VisualScripting;
using UnityEngine;
using Weapons;

public class Program : MonoBehaviour
{
    public static float Time_Scale = 1.0f;
    public static Weapon Weapon_Now;
    public static Weapon[] Weapon_Bag = new Weapon[2];
    public static float Weapon_Rotation;
    public static GameObject Main_Character;
    public Program_Save Program_Save_Object;

    private void Start()
    {
        Main_Character = this.gameObject;
    }

    public static void TimeChange(float time)
    {
        Time_Scale = time;
        Time.timeScale = Time_Scale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        //Main_Character.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && Time_Scale ==1.0f)
        {
            TimeChange(0.01f);
            //锁定主角的旋转和y轴的移动
            Main_Character.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY;
        }
        else if (Input.GetKeyDown(KeyCode.T) && Time_Scale == 0.01f)
        {
            TimeChange(1.0f);
            //解锁主角的y轴的移动，但仍然锁定旋转
            Main_Character.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ;
        }
    }
}