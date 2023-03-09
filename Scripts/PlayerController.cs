using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3.0f;
    public float turnSpeed = 90f;
    float Rot=0f;
    bool isRotated = false;
    public static Signal signal = new Signal();
    //这个脚本使用rigidbody来控制玩家在3d空间中的移动
    private void Update()
    {
        //在移动时开启Move动画
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            GetComponent<Animator>().SetBool("IsMove", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("IsMove", false);
        }
        //当horizontal不等于0时，玩家的朝向为horizontal的正负
        if (Input.GetAxis("Horizontal") < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        
        //获取玩家输入
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        //获取玩家的rigidbody组件
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        
        //根据玩家的transform.eulerAngles设置玩家的速度
        rigidbody.velocity = transform.TransformDirection(new Vector3(x, 0, z) * speed);

        //当按下e时，线性改变玩家的朝向到逆时针旋转90度
        if (Input.GetKeyDown(KeyCode.E) && !isRotated)
        {
            isRotated = true;
            StartCoroutine(ROT(true));
        }
        
        //当按下q时，线性改变玩家的朝向到顺时针旋转90度
        if (Input.GetKeyDown(KeyCode.Q) && !isRotated)
        {
            isRotated = true;
            StartCoroutine(ROT(false));
        }
    }
    
    public IEnumerator ROT(bool isClockwise)
    {
        signal.Call(true);
        Rot = 0;
        while (Rot <= 90)
        {
            Rot += turnSpeed*Time.deltaTime;
            if (isClockwise)
            {
                transform.Rotate(0, turnSpeed*Time.deltaTime, 0);
            }
            else
            {
                transform.Rotate(0, -turnSpeed*Time.deltaTime, 0);
            }
            yield return 0;
        }
        if(Rot>90)
        {
            isRotated = false;
            float angle = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, Mathf.Round(angle / 90) * 90, 0);
            signal.Call(false);
        }
    }
}
