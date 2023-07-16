using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Spine.Unity;

public class EnemyController : MonoBehaviour
{
    private Rigidbody Rb;
    private Animator Ani;
    private SkeletonAnimation SA;
    private SpriteRenderer SR;
    private CharacterState CS;
    public bool IsSpine = false;
    private bool IsFlip = false;
    [Header("行为模式管理")]
    /// <summary>
    /// 用以判断角色是可移动的还是固定式的
    /// </summary>
    public bool IsMove = false;
    /// <summary>
    /// 创建检测碰撞箱的参数，分别是左右上下的大小
    /// </summary>
    public Vector4 CheckBox;
    /// <summary>
    /// 创建检测碰撞箱的参数，分别是前方深度与后方深度，默认为0.3f
    /// </summary>
    public Vector2 CheckBoxOffset = new Vector2(0.3f, 0.3f);
    public List<Attack_Mode> AttackModes = new List<Attack_Mode>();
    private Collider CheckCollider;
    private GameObject Manager;



    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        if (!IsSpine)
        {
            Ani = GetComponent<Animator>();
        }
        else
        {
            SA = GetComponent<SkeletonAnimation>();
        }
        SR = GetComponent<SpriteRenderer>();
        CS = GetComponent<CharacterState>();

        Manager = new GameObject("Manager");
        var t = Manager.AddComponent<ForCheckBox>();
        Manager.transform.SetParent(this.transform);
        CheckCollider = Manager.AddComponent<BoxCollider>();
        CheckCollider.isTrigger = true;
        Manager.transform.localPosition = Vector3.zero;
        Manager.transform.localScale = new Vector3(CheckBox.x + CheckBox.y, CheckBox.z + CheckBox.w, CheckBoxOffset.x + CheckBoxOffset.y);
        Manager.transform.localPosition = new Vector3((CheckBox.y - CheckBox.x) / 2, (CheckBox.z - CheckBox.w) / 2, (CheckBoxOffset.y - CheckBoxOffset.x) / 2);

        foreach (var attackMode in AttackModes)
        {
            attackMode.Start();
        }

    }

    void Update()
    {
        //IsFlip = IsSpine ? SA.skeleton.ScaleX < 0 : SR.flipX;

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
                if (SA.AnimationName != "Jump")
                {
                    SA.AnimationState.SetAnimation(0, "Stalling", true);
                }
                else
                {
                    SA.AnimationState.AddAnimation(0, "Stalling", true, 0);
                }
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
        }

        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -0.6f);
    }
    /// <summary>
    /// 用以让角色左右翻转
    /// </summary>
    private void CharacterFlip()
    {
        IsFlip = !IsFlip;
        if (!IsSpine)
        {
            SR.flipX = IsFlip;
        }
        else
        {
            SA.skeleton.ScaleX = IsFlip ? -1 : 1;
        }
    }
    private bool Check()
    {

        return true;
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
    public float Shoot_Speed;
    public float Gun_Rot_Speed;
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
    private GameObject AttackCheck;
    public void Start()
    {
        AttackCheck = new GameObject($"AttackCheck{id}");
        var t = AttackCheck.AddComponent<ForAttackCheck>();
        t.Attack_Prefab = attack;
        t.Attack_Position = attack_position;

        AttackCheck.transform.SetParent(Father.transform);
        attack_collider = AttackCheck.AddComponent<BoxCollider>();
        attack_collider.isTrigger = true;
        AttackCheck.transform.localPosition = Vector3.zero;
        AttackCheck.transform.localScale = new Vector3(attack_collider_size.x + attack_collider_size.y, attack_collider_size.z + attack_collider_size.w, attack_collider_offset.x + attack_collider_offset.y);
        AttackCheck.transform.localPosition = new Vector3((attack_collider_size.y - attack_collider_size.x) / 2, (attack_collider_size.z - attack_collider_size.w) / 2, (attack_collider_offset.y - attack_collider_offset.x) / 2);
    }
}