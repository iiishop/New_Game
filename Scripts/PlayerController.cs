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
    public bool Can_Move = true;

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

        if (Input.GetKeyUp(KeyCode.A)||Input.GetKeyUp(KeyCode.D))
        {
            GetComponent<Animator>().SetBool("IsMove", false);
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
            //使用transform来控制玩家的移动
            transform.Translate(new Vector3(x, this.GetComponent<Rigidbody>().velocity.y) * Speed *
                                Time.unscaledDeltaTime);
        }
        else
        {
            GetComponent<Animator>().SetBool("IsMove", false);
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
        
        if (!collision.gameObject.CompareTag("Ground")||(Program.Time_Scale!=1))
        {
            //获取玩家与碰撞体接触的点的坐标
            var ClosestPoint = collision.GetContact(0).point;
            var Left = ClosestPoint.x > this.transform.position.x;
            Debug.Log(Left);
            Debug.Log(ClosestPoint);
            Debug.Log(this.transform.position);
            //GetComponent<Animator>().SetBool("IsMove", false);
            /*if (Is_Dashing)
            {
                transform.Translate(
                    new Vector3(!Left ? -1 : 1, this.GetComponent<Rigidbody>().velocity.y) *
                    Dash_Speed *
                    Time.unscaledDeltaTime);
            }*/
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
    }
}