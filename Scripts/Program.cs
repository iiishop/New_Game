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
    
    private Rigidbody Main_Character_Rigidbody;
    private Animator Main_Character_Animator;
    private PlayerController Main_Character_Controller;

    private void Start()
    {
        Main_Character = this.gameObject;
        Main_Character_Rigidbody = Main_Character.GetComponent<Rigidbody>();
        Main_Character_Animator = Main_Character.GetComponent<Animator>();
        Main_Character_Controller = Main_Character.GetComponent<PlayerController>();
    }

    public static void TimeChange(float time)
    {
        Time_Scale = time;
        Time.timeScale = Time_Scale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        //Q:在Unity修改Time.timeScale后，如何让物理引擎的碰撞检测和刚体的运动速度也跟着改变？
        //A:https://blog.csdn.net/qq_41855420/article/details/107100100
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && Time_Scale ==1.0f)
        {
            TimeChange(0.001f);
            Main_Character.transform.position = new Vector3(this.transform.position.x,
                this.transform.position.y + 0.02f, this.transform.position.z);
            //锁定主角的旋转和y轴的移动
            Main_Character_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY;
            Main_Character_Animator.SetBool("IsGrounded", true);
        }
        else if (Input.GetKeyDown(KeyCode.T) && Time_Scale == 0.001f)
        {
            TimeChange(1.0f);
            Main_Character_Controller.Can_Move = true;
            //解锁主角的y轴的移动，但仍然锁定旋转
            Main_Character_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ;
        }
    }
    
}