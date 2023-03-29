using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public float Speed = 3.0f;

    [Header("冲刺设置")] 
    public float Dash_Time;
    private float Dash_Time_Left;
    public float Dash_Speed;
    private float Dash_CD_Start;
    public float Dash_CD;
    private bool Is_Dashing;
    private float One_Dash_Time;
    private float One_Dash_Start;
    public int Dash_Size;
    
    [Header("跳跃设置")]
    public float Jump_Force;
    public int Jump_Count = 2;
    private int Jump_Count_Now;
    
    [Header("组件")]
    public Animator Ani;
    public Rigidbody Rb;
    public SpriteRenderer SR;

    /// <summary>
    /// 是horizontal轴的值,用于控制玩家的移动
    /// </summary>
    private float x;
    
    [Header("变量")]
    public bool Can_Move = true;
    public bool Is_Grounded = true;

    private void Start()
    {
        Jump_Count_Now = Jump_Count;
        One_Dash_Time = Dash_Time / Dash_Size;
        Ani = GetComponent<Animator>();
        Rb = GetComponent<Rigidbody>();
        SR = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //获取玩家输入,并将其转换为int类型,当其大于0时,将其设置为1,当其小于0时,将其设置为-1
        x = Input.GetAxisRaw("Horizontal");


        if (Input.GetKey(KeyCode.A))
        {
            SR.flipX = true;
        }

        if (Input.GetKey(KeyCode.D))
        {
            SR.flipX = false;
        }

        if (Input.GetKeyUp(KeyCode.A)||Input.GetKeyUp(KeyCode.D))
        {
            Ani.SetBool("IsMove", false);
        }
        
        Jump();

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Time.realtimeSinceStartup >= Dash_CD_Start + Dash_CD) //CD时间结束
            {
                One_Dash_Start = Time.realtimeSinceStartup;
                Dash_CD_Start = Time.realtimeSinceStartup;
                Dash_Time_Left = Dash_Time;
                Is_Dashing = true;
            }
        }


        if (Can_Move)
        {
            Move();
            Dash();
        }

        this.Rb.velocity = new Vector3(0, this.Rb.velocity.y);
    }

    private void Jump()
    {
        if(Is_Grounded && Jump_Count_Now < Jump_Count && Program.Time_Scale == 1.0f && Rb.velocity.y <= 0)
        {
            Jump_Count_Now = Jump_Count;
        }
        if(Input.GetKeyDown(KeyCode.Space) && Jump_Count_Now > 0 && Program.Time_Scale == 1.0f)
        {
            Rb.velocity = new Vector3(Rb.velocity.x, Jump_Force);
            Ani.SetTrigger("Jump");
            Ani.SetBool("IsGrounded", false);
            Is_Grounded = false;
            Jump_Count_Now--;
        }
    }

    private void Move()
    {
        if (x != 0)
        {
            Ani.SetBool("IsMove", true);
            //使用transform来控制玩家的移动
            transform.Translate(new Vector3(x, 0) * Speed * Time.unscaledDeltaTime);
        }
        else
        {
            Ani.SetBool("IsMove", false);
        }
    }

    private void Dash()
    {
        Dash_Time_Left -= Time.unscaledDeltaTime;
        if (Is_Dashing)
        {
            transform.Translate(new Vector3(x, 0) * Dash_Speed * Time.unscaledDeltaTime);
        }

        if (Is_Dashing && One_Dash_Start + One_Dash_Time <= Time.realtimeSinceStartup)
        {
            if (Dash_Time_Left > 0)
            {
                One_Dash_Start = Time.realtimeSinceStartup;

                ObjectPool.Instance.GetPool(ObjectPool.Instance.Shadow_Prefab);
            }
            else
            {
                Dash_Time_Left = 0;
                Is_Dashing = false;
            }
        }
    }
    //当玩家与地面接触时,将Is_Grounded设置为true
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            Is_Grounded = true;
            Ani.SetBool("IsGrounded", true);
        }
    }


    private void OnCollisionStay(Collision collision)
    {

        if (!collision.gameObject.CompareTag("Ground")||(Program.Time_Scale!=1))
        {
            //获取玩家与碰撞体接触的点的坐标
            var ClosestPoint = collision.GetContact(0).point;
            var Left = ClosestPoint.x > this.transform.position.x;
            //对closestpoint.x保留两位小数
            ClosestPoint.x = (float) Math.Round(ClosestPoint.x, 2);
            //对玩家的x坐标保留两位小数
            var t = (float) Math.Round(this.transform.position.x, 2);
            if (ClosestPoint.x == t)
            {
                Can_Move = true;
                return;
            }

            if (x != 0)
            {
                if (Left)
                {
                    Can_Move = x != 1;
                }
                else
                {
                    Can_Move = x != -1;
                }
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        Can_Move = true;
        if (other.gameObject.CompareTag("Ground"))
        {
            Is_Grounded = false;
            Ani.SetBool("IsGrounded", false);
        }
    }
}