using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3.0f;
    
    private void Update()
    {
        
        //获取玩家输入,并将其转换为int类型,当其大于0时,将其设置为1,当其小于0时,将其设置为-1
        int x=0, z=0;
        if (Input.GetKey(KeyCode.A))
        {
            GetComponent<SpriteRenderer>().flipX = true;
            x=-1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            GetComponent<SpriteRenderer>().flipX = false;
            x=1;
        }
        if(Input.GetKey(KeyCode.W))
            z=1;
        if(Input.GetKey(KeyCode.S))
            z=-1;
        

        //当检测到wasd没有被按下时，将x和z的值设置为0
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            x = 0;
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            z = 0;

        if (x != 0 || z != 0)
        {
            GetComponent<Animator>().SetBool("IsMove", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("IsMove", false);
        }
        

        //使用transform来控制玩家的移动
        transform.Translate(new Vector3(x, 0, z) * speed, Space.World);

    }
}