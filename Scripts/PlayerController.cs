using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private void Start()
    {
        One_Dash_Time = Dash_Time / ObjectPool.Instance.ShadowPoolSize;
    }

    private void Update()
    {
        //获取玩家输入,并将其转换为int类型,当其大于0时,将其设置为1,当其小于0时,将其设置为-1
        int x = 0, z = 0;
        if (Input.GetKey(KeyCode.A))
        {
            GetComponent<SpriteRenderer>().flipX = true;
            x = -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            GetComponent<SpriteRenderer>().flipX = false;
            x = 1;
        }

        if (Input.GetKey(KeyCode.W))
            z = 1;
        if (Input.GetKey(KeyCode.S))
            z = -1;


        //当检测到wasd没有被按下时，将x和z的值设置为0
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            x = 0;
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            z = 0;


        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Time.realtimeSinceStartup >= Dash_CD_Start + Dash_CD)//CD时间结束
            {
                One_Dash_Start = Time.realtimeSinceStartup;
                Dash_CD_Start = Time.realtimeSinceStartup;
                Dash_Time_Left = Dash_Time;
                Is_Dashing = true;
            }
        }


        Move(x, z);

        Dash(x, z);
    }

    private void Move(int x, int z) //后期会去掉z
    {
        if (x != 0 || z != 0)
        {
            GetComponent<Animator>().SetBool("IsMove", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("IsMove", false);
        }


        //使用transform来控制玩家的移动
        transform.Translate(new Vector3(x, 0, z) * Speed, Space.World);
    }

    private void Dash(int x, int z)
    {
        Dash_Time_Left -= Time.unscaledDeltaTime;
        if (Is_Dashing)
        {
            transform.Translate(new Vector3(x, 0, z) * Dash_Speed, Space.World);
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
}