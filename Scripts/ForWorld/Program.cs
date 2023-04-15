using System;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
//using UnityEditor.Animations;
using UnityEngine;
using Weapons;

/// <summary>
/// 这个类是用来存储一些全局变量以及一些全局方法的类。
/// </summary>
public class Program : MonoBehaviour
{
    public static Program Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// 当前的时间缩放比例
    /// </summary>
    public float Time_Scale = 1.0f;
    /// <summary>
    /// 缩放时间技能会缩放的时间比率
    /// </summary>
    public float Scaled_Time = 0.1f;
    public Weapon Weapon_Now;
    public Weapon[] Weapon_Bag = new Weapon[2];
    public float Weapon_Rotation;
    public GameObject Main_Character;
    public Program_Save Program_Save_Object;

    private Rigidbody Main_Character_Rigidbody;
    private Animator Main_Character_Animator;
    private PlayerController Main_Character_Controller;

    private void Start()
    {
        Program_Save_Object = Program_Save.Instance;

        Main_Character = this.gameObject;
        Main_Character_Rigidbody = Main_Character.GetComponent<Rigidbody>();
        Main_Character_Animator = Main_Character.GetComponent<Animator>();
        Main_Character_Controller = Main_Character.GetComponent<PlayerController>();
    }
    /// <summary>
    /// 改变时间
    /// </summary>
    /// <param name="time">改变后的时间</param>
    public void TimeChange(float time)
    {
        Time_Scale = time;
        Time.timeScale = Time_Scale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    /// <summary>
    /// 用于改变时间的方法。
    /// 与TimeChange方法的区别在于，这个方法可以是考虑到游戏主角的运动方式，使得主角在时间缩放时仍然能够保持原来的运动方式。
    /// 而TimeChange方法则是直接改变时间，并未关心其他的细节。
    /// 这个方法应用了TimeChange方法。
    /// </summary>
    /// <param name="time">缩放后的时间</param>
    public void ChangeTheTime(float time)
    {
        if (Input.GetKeyDown(KeyCode.T) && Time_Scale == 1.0f)
        {
            TimeChange(time);
            Main_Character.transform.position = new Vector3(this.transform.position.x,
                this.transform.position.y + 0.02f, this.transform.position.z);
            //锁定主角的旋转和y轴的移动
            Main_Character_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY;
            Main_Character_Animator.SetBool("IsGrounded", true);
        }
        else if (Input.GetKeyDown(KeyCode.T) && Time_Scale == time)
        {
            TimeChange(1.0f);
            Main_Character_Controller.Can_Move = true;
            //解锁主角的y轴的移动，但仍然锁定旋转
            Main_Character_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ;
        }
    }

    /// <summary>
    /// 用于让物体线性运动到目标点的方法
    /// </summary>
    /// <param name="self">需要运动的物体的起始坐标</param>
    /// <param name="target">目标坐标</param>
    /// <param name="offset">目标坐标的偏移值</param>
    /// <param name="speed">运动速度</param>
    /// <param name="is_unscaled_time">是否使用unscaledDeltaTime</param>
    /// <returns>下一帧self需要到达的位置</returns>
    public Vector3 Lerp2Tagert(Vector3 self, Vector3 target, Vector3 offset, float speed, bool is_unscaled_time = true)
    {
        Vector3 target_position = target + offset;
        Vector3 new_position;
        if (is_unscaled_time)
        {
            new_position = Vector3.Lerp(self, target_position, speed * Time.unscaledDeltaTime);
        }
        else
        {
            new_position = Vector3.Lerp(self, target_position, speed * Time.deltaTime);
        }

        return new_position;
    }

    /// <summary>
    /// 获得animator下某个动画片段的时长方法
    /// </summary>
    /// <param animator="animator">Animator组件</param> 
    /// <param name="name">要获得的动画片段名字</param>
    /// <returns>这个动画的原始长度（与播放速度无关）</returns>
    public float GetAnimatorLength(Animator animator, string name)
    {
        //动画片段时间长度
        float length = 0;
        float speed = 1;
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name.Equals(name))
            {
                length = clip.length;
                break;
            }
        }
        //这一部分因为编译后脱离了编辑器，所以无法获取到动画片段的播放速度，所以删去
        //获取当前动画的播放速度
        //var animController = animator.runtimeAnimatorController as AnimatorController;
        //if (animController)
        //{
        //    for (int i = 0; i < animController.layers[0].stateMachine.states.Length; i++)
        //    {
        //        if(animController.layers[0].stateMachine.states[i].state.nameHash == Animator.StringToHash(name))
        //            speed = animController.layers[0].stateMachine.states[i].state.speed;
        //    }
        //}
        return length / speed;
    }

    /// <summary>
    /// 一个方法，用于在t秒内，每次调用返回一个数，数值从a到b。如果b大于a，那么返回的数值会从小到大，反之dd则从大到小。如果t为0，那么返回的数值为b。
    /// </summary>
    /// <param name="a">起始的数值</param>
    /// <param name="b">结束的数值</param>
    /// <param name="t">变换的时间</param>
    /// <returns></returns>
    public float NumberLerp(float a, float b, ref float t)
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
        ChangeTheTime(Scaled_Time);
    }

}