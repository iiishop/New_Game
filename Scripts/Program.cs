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
    public static Program_Save Program_Save_Object;

    private void Start()
    {
        Main_Character = this.gameObject;
    }

    public static void TimeChange(float time)
    {
        Time_Scale = time;
        Time.timeScale = Time_Scale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        //Main_Character.GetComponent<Rigidbody>().velocity = new Vector3(0,Main_Character.GetComponent<Rigidbody>().velocity.y/Time_Scale);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && Time_Scale ==1.0f)
        {
            TimeChange(0.01f);
            Main_Character.transform.position = new Vector3(this.transform.position.x,
                this.transform.position.y + 0.01f, this.transform.position.z);
            //锁定主角的旋转和y轴的移动
            Main_Character.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY;
            Main_Character.GetComponent<Animator>().SetBool("IsGrounded", true);
        }
        else if (Input.GetKeyDown(KeyCode.T) && Time_Scale == 0.01f)
        {
            TimeChange(1.0f);
            Main_Character.GetComponent<PlayerController>().Can_Move = true;
            //解锁主角的y轴的移动，但仍然锁定旋转
            Main_Character.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ;
        }
    }
    
}