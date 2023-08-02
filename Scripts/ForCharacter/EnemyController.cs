using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Spine.Unity;
using Tools;

public class EnemyController : MonoBehaviour
{
    private Rigidbody Rb;
    private Animator Ani;
    private SkeletonAnimation SA;
    private SpriteRenderer SR;
    private CharacterState CS;
    public bool IsSpine = false;
    public bool IsFlip = false;
    [Header("行为模式管理")]
    /// <summary>
    /// 用以判断角色是可移动的还是固定式的
    /// </summary>
    public bool Can_Move = false;
    public float Move_Speed;
    /// <summary>
    /// 创建检测碰撞箱的参数，分别是左右上下的大小
    /// </summary>
    public Vector4 Check_Collider_Size;
    /// <summary>
    /// 创建检测碰撞箱的参数，分别是前方深度与后方深度，默认为0.3f
    /// </summary>
    public Vector2 Check_Collider_Offset = new Vector2(0.3f, 0.3f);
    public List<Attack_Mode> AttackModes = new List<Attack_Mode>();
    private Collider CheckCollider;
    private GameObject CheckBox;



    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        Ani = GetComponent<Animator>();
        SA = GetComponent<SkeletonAnimation>();
        SR = GetComponent<SpriteRenderer>();
        CS = GetComponent<CharacterState>();

        CheckBox = new GameObject("CheckBox");
        var t = CheckBox.AddComponent<ForCheckBox>();
        t.Enemy = this.gameObject;
        CheckBox.transform.SetParent(this.transform);
        CheckCollider = CheckBox.AddComponent<BoxCollider>();
        CheckCollider.isTrigger = true;
        CheckBox.transform.localPosition = Vector3.zero;
        CheckBox.transform.localScale = new Vector3(Check_Collider_Size.x + Check_Collider_Size.y, Check_Collider_Size.z + Check_Collider_Size.w, Check_Collider_Offset.x + Check_Collider_Offset.y);
        CheckBox.transform.localPosition = new Vector3((Check_Collider_Size.y - Check_Collider_Size.x) / 2, (Check_Collider_Size.z - Check_Collider_Size.w) / 2, (Check_Collider_Offset.y - Check_Collider_Offset.x) / 2);

        foreach (var attackMode in AttackModes)
        {
            attackMode.Start();
        }

    }

    void Update()
    {

    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            if (!IsSpine)
            {
                Ani.SetBool("IsGrounded", false);
            }
            else
            {
                SA.PlaySpineAnimation("Stalling", true, SA.AnimationName != "Jump");
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            if (!IsSpine)
            {
                Ani.SetBool("IsGrounded", true);
            }
            else
            {
                SA.PlaySpineAnimation("Idle", true);
            }
        }

        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -0.6f);
    }
    /// <summary>
    /// 用以让角色左右翻转
    /// </summary>
    public void CharacterFlip()
    {
        //DONE:只翻转角色会导致攻击模式的枪口朝向错误
        IsFlip = !IsFlip;
        if (!IsSpine)
        {
            SR.flipX = IsFlip;
        }
        else
        {
            SA.skeleton.ScaleX = IsFlip ? -1 : 1;
        }
        foreach (var attackMode in AttackModes)
        {
            attackMode.Gun.transform.Rotate(0, 0, 180 - 2 * attackMode.Gun.transform.rotation.eulerAngles.z);
            attackMode.attack_position.transform.Rotate(0, 0, 180 - 2 * attackMode.attack_position.transform.rotation.eulerAngles.z);
            attackMode.Gun.transform.localPosition = attackMode.Gun.transform.localPosition.Flip();

            attackMode.AttackCheck.transform.localPosition = attackMode.AttackCheck.transform.localPosition.Flip();
        }
        //翻转CheckBox
        CheckBox.transform.localPosition = CheckBox.transform.localPosition.Flip();
    }
    public void Move()
    {
        //TODO:向当前面朝方向进行移动操作
        Rb.velocity = new Vector3(IsFlip ? -Move_Speed : Move_Speed, Rb.velocity.y, 0);
    }
    public void Stop()
    {
        Rb.velocity = new Vector3(0, Rb.velocity.y, 0);
    }
}

[System.Serializable]
public class Attack_Mode
{
    public int id;
    /// <summary>
    /// 攻击模式是否为远程
    /// </summary>
    public bool Is_Shoot = true;
    public GameObject Gun;
    public float Shoot_Wait_Time;
    /// <summary>
    /// 枪口转动的速度rad/s
    /// </summary>
    public float Gun_Rot_Speed;
    public float[] Rot_Range = new float[2];
    /// <summary>
    /// 攻击模式的父物体，用于管理，即角色
    /// </summary>
    public GameObject Father;
    /// <summary>
    /// 需要生成的子弹物体
    /// </summary>
    public GameObject attack;
    /// <summary>
    /// 生成子弹的位置
    /// </summary>
    public GameObject attack_position;
    /// <summary>
    /// 触发此攻击的范围的触发器
    /// </summary>
    private Collider attack_collider;
    public Vector4 attack_collider_size;
    public Vector2 attack_collider_offset;
    /// <summary>
    /// 挂载攻击检测的物体
    /// </summary>
    public GameObject AttackCheck;
    public void Start()
    {
        AttackCheck = new GameObject($"AttackCheck{id}");
        var t = AttackCheck.AddComponent<ForAttackCheck>();
        t.AM = this;
        t.FCB = Father.GetComponentInChildren<ForCheckBox>();
        t.SA = Father.GetComponentInChildren<SkeletonAnimation>();

        AttackCheck.transform.SetParent(Father.transform);
        attack_collider = AttackCheck.AddComponent<BoxCollider>();
        attack_collider.isTrigger = true;
        AttackCheck.transform.localPosition = Vector3.zero;
        AttackCheck.transform.localScale = new Vector3(attack_collider_size.x + attack_collider_size.y, attack_collider_size.z + attack_collider_size.w, attack_collider_offset.x + attack_collider_offset.y);
        AttackCheck.transform.localPosition = new Vector3((attack_collider_size.y - attack_collider_size.x) / 2, (attack_collider_size.z - attack_collider_size.w) / 2, (attack_collider_offset.y - attack_collider_offset.x) / 2);
    }
}