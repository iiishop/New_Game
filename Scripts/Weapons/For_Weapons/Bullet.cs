using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Weapons;

[System.Serializable]
public class Bullet : MonoBehaviour//缺少碰撞后销毁、伤害计算
{
    //成员变量
    public float speed;
    public double damage;
    public double gittest;
    Rigidbody2D rb2d;
    public float deathTime = 3f;
    void Start()
    {
        rb2d = this.GetComponent<Rigidbody2D>();
        Move();
    }

    public virtual void Move()
    {
        //rb2d.AddForce(new Vector2(Mathf.Cos((Program.Weapon_Rotation) / 180 * Mathf.PI) , 
        //    Mathf.Sin((Program.Weapon_Rotation) / 180 * Mathf.PI) )*speed);
        rb2d.velocity = new Vector2(Mathf.Cos((Program.Weapon_Rotation) / 180 * Mathf.PI) * speed,
            Mathf.Sin((Program.Weapon_Rotation) / 180 * Mathf.PI) * speed);
        
        Invoke("destory",deathTime);//完成后应去除这行
    }

    private void OnTriggerEnter(Collider other)//添加了碰撞敌人后才会触发伤害
    {   
        if (other.gameObject.tag == "Enemy") {
        Hurt();
        }
        destory(other.gameObject.tag);
    }

    public virtual void Hurt()
    {
        //other.gameObject.GetComponent<>()
        //伤害和其他特殊效果
    }

    public virtual void destory(string tag)//为实现双穿添加参数
    {
        Destroy(this.gameObject);
    }

}
