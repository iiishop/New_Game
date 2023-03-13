using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public float Speed = 3.0f;

    [Header("冲刺设置")] public float Dash_Time;
    private float Dash_Time_Left;
    public float Dash_Speed;
    private float Dash_CD_Start;
    public float Dash_CD;
    private bool Is_Dashing;
    private float One_Dash_Time;
    private float One_Dash_Start;

    private float x;
    private bool Can_Move = true;

    private void Start()
    {
        One_Dash_Time = Dash_Time / ObjectPool.Instance.ShadowPoolSize;
    }

    private void Update()
    {
        //获取玩家输入,并将其转换为int类型,当其大于0时,将其设置为1,当其小于0时,将其设置为-1
        x = Input.GetAxisRaw("Horizontal");


        if (Input.GetKey(KeyCode.A))
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        if (Input.GetKey(KeyCode.D))
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }


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
    }

    private void FixedUpdate()
    {
        if (Can_Move)
        {
            Move();
            Dash();
        }

        this.GetComponent<Rigidbody>().velocity = new Vector3(0, this.GetComponent<Rigidbody>().velocity.y);
    }

    private void Move()
    {
        if (x != 0)
        {
            GetComponent<Animator>().SetBool("IsMove", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("IsMove", false);
        }

        //使用transform来控制玩家的移动
        transform.Translate(new Vector3(x, this.GetComponent<Rigidbody>().velocity.y) * Speed * Time.unscaledDeltaTime);
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

                ObjectPool.Instance.GetPool();
            }
            else
            {
                Dash_Time_Left = 0;
                Is_Dashing = false;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ground"))
        {
            var dis = collision.transform.position - this.transform.position;
            GetComponent<Animator>().SetBool("IsMove", false);
            if (Is_Dashing)
            {
                transform.Translate(
                    new Vector3(dis.x > 0 ? -1 : 1, this.GetComponent<Rigidbody>().velocity.y) *
                    Dash_Speed *
                    Time.unscaledDeltaTime);
            }

            if (x != 0)
            {
                Can_Move = dis.x switch
                {
                    > 0 when x == 1 => false,
                    > 0 => true,
                    < 0 when x == -1 => false,
                    < 0 => true,
                    _ => Can_Move
                };
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        Can_Move = true;
    }
}