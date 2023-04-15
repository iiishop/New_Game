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
    public int Jump_Count_Now;
    public bool Is_Air;
    public float Jump_Up_Speed;
    public float Jump_Up_Time;
    public float Jump_Up_Time_Left;
    public float Up_Start_Velocity;

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

        //跳跃初始化。加入前第一次跳跃会有问题
        Is_Air = false;
        Jump_Up_Time_Left = Jump_Up_Time;
        Rb.useGravity = true;
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

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            Ani.SetBool("IsMove", false);
        }


        if (Input.GetKeyDown(KeyCode.LeftShift))
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
            Jump();
        }

        this.Rb.velocity = new Vector3(0, this.Rb.velocity.y);
    }

    private void Jump()
    {
        if (Is_Grounded && Jump_Count_Now < Jump_Count && Program.Time_Scale == 1.0f && Rb.velocity.y <= 0)
        {
            Jump_Count_Now = Jump_Count;
        }
        // 当起跳后仍然按住空格时,并且玩家的速度变成0，给予玩家一个稳定的向上速度，使玩家能够更好的控制跳跃高度
        if (Input.GetKey(KeyCode.Space) && !Is_Grounded && Rb.velocity.y <= Up_Start_Velocity && Program.Time_Scale == 1.0f && !Is_Air && Jump_Count_Now > 0)
        {
            DEBUG.Instance.Display_Info("Jump");
            Is_Air = true;
            Rb.useGravity = false;
            Rb.velocity = new Vector3(Rb.velocity.x, Jump_Up_Speed);
        }
        if (Input.GetKeyDown(KeyCode.Space) && Jump_Count_Now > 0 && Program.Time_Scale == 1.0f)
        {
            Rb.velocity = new Vector3(Rb.velocity.x, Jump_Force);
            Ani.SetTrigger("Jump");
            Ani.SetBool("IsGrounded", false);
            Is_Grounded = false;
            Jump_Count_Now--;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Is_Air = false;
            Jump_Up_Time_Left = Jump_Up_Time;
            Rb.useGravity = true;
        }
        //当Is_Air为true时,开始计时,当计时结束后将Rb的useGravity设置为true
        if (Is_Air)
        {
            Jump_Up_Time_Left -= Time.unscaledDeltaTime;
            if (Jump_Up_Time_Left <= 0)
            {
                Rb.useGravity = true;
            }
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

                ObjectPool.Instance.GetPool(ObjectPool.Instance.Prefabs[0]);
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
        if (collision.gameObject.CompareTag("Ground"))
        {
            Is_Grounded = true;

            Ani.SetBool("IsGrounded", true);
        }
    }


    private void OnCollisionStay(Collision collision)
    {
        if ((Program.Time_Scale != 1))
        {
            Collider Player_Collider = GetComponent<Collider>();
            Collider Collision_Collider = collision.gameObject.GetComponent<Collider>();
            Vector2 Collision_Center = Collision_Collider.bounds.center;
            Vector2 Player_Center = Player_Collider.bounds.center;
            var Collision_Radius = Collision_Collider.bounds.size.x / 2;
            var Player_Radius = Player_Collider.bounds.size.x / 2;
            var Collision_Height = Collision_Collider.bounds.size.y / 2;
            var Player_Height = Player_Collider.bounds.size.y / 2;
            var Collision_Top = Collision_Center.y + Collision_Height;
            var Collision_Bottom = Collision_Center.y - Collision_Height;
            var Player_Top = Player_Center.y + Player_Height;
            var Player_Bottom = Player_Center.y - Player_Height;
            var Collision_Left = Collision_Center.x - Collision_Radius;
            var Collision_Right = Collision_Center.x + Collision_Radius;
            var Player_Left = Player_Center.x - Player_Radius;
            var Player_Right = Player_Center.x + Player_Radius;
            var Left_Distance = Collision_Left - Player_Right;
            var Right_Distance = Collision_Right - Player_Left;
            //float Top_Distance = Collision_Top - Player_Bottom;
            //float Bottom_Distance = Collision_Bottom - Player_Top;

            if (Right_Distance > 0 && Collision_Center.x < Player_Center.x && Collision_Top > Player_Bottom && Collision_Bottom < Player_Top)
            {
                this.transform.position = new Vector3(this.transform.position.x + Right_Distance, this.transform.position.y, this.transform.position.z);
            }
            if (Left_Distance < 0 && Collision_Center.x > Player_Center.x && Collision_Top > Player_Bottom && Collision_Bottom < Player_Top)
            {
                this.transform.position = new Vector3(this.transform.position.x + Left_Distance, this.transform.position.y, this.transform.position.z);
            }
            /*
            //获取玩家与碰撞体接触的点的坐标
            //var ClosestPoint = collision.GetContact(0).point;
            //var Left = ClosestPoint.x > this.transform.position.x;
            ////对closestpoint.x保留两位小数
            //ClosestPoint.x = (float) Math.Round(ClosestPoint.x, 2);
            ////对玩家的x坐标保留两位小数
            //var t = (float) Math.Round(this.transform.position.x, 2);
            //Debug.Log(this.transform.position.y - ClosestPoint.y);
            //if (ClosestPoint.x == t && this.transform.position.y- ClosestPoint.y >=Grounded_Radius )
            //{
            //    Can_Move = true;
            //    return;
            //}

            //if (x != 0)
            //{
            //    if (Left)
            //    {
            //        Can_Move = x != 1;
            //    }
            //    else
            //    {
            //        Can_Move = x != -1;
            //    }
            //}*/
        }
    }

    private void OnCollisionExit(Collision other)
    {
        Can_Move = true;
        if (other.gameObject.CompareTag("Ground"))
        {
            Is_Grounded = false;
            Ani.SetBool("IsGrounded", false);
            if (Rb.velocity.y < 0)
            {
                Jump_Count_Now--;
            }
        }
    }
}