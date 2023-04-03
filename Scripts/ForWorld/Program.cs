using System;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Animations;
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
    }
    
    public static Vector3 Lerp2Tagert(Vector3 self, Vector3 target, Vector3 offset, float speed)
    {
        Vector3 target_position = target + offset;
        Vector3 new_position = Vector3.Lerp(self, target_position, speed * Time.unscaledDeltaTime);
        return new_position;
    }

    /// <summary>
    /// 获得animator下某个动画片段的时长方法
    /// </summary>
    /// <param animator="animator">Animator组件</param> 
    /// <param name="name">要获得的动画片段名字</param>
    /// <returns></returns>
    public static float GetAnimatorLength(Animator animator ,string name)
    {
        //动画片段时间长度
        float length = 0;
        float speed = 0;
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name.Equals(name))
            {
                length = clip.length;
                break;
            }
        }
        //获取当前动画的播放速度
        var animController = animator.runtimeAnimatorController as AnimatorController;
        if (animController)
        {
            for (int i = 0; i < animController.layers[0].stateMachine.states.Length; i++)
            {
                if(animController.layers[0].stateMachine.states[i].state.nameHash == Animator.StringToHash(name))
                    speed = animController.layers[0].stateMachine.states[i].state.speed;
            }
        }
        return length/speed;
    }
    
    /// <summary>
    /// 一个方法，用于在t秒内，每次调用返回一个数，数值从a到b。如果b大于a，那么返回的数值会从小到大，反之dd则从大到小。如果t为0，那么返回的数值为b。
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static float NumberLerp(float a, float b, ref float t)
    {
        if (t <= 0)
        {
            return b;
        }
        else
        {
            var res = a + (b - a) * Time.unscaledDeltaTime / t * Time_Scale;
            t -= Time.unscaledDeltaTime * Time_Scale;
            return res;
        }
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