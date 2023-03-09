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
    Rigidbody2D rb2d;
    public float deathTime = 3f;
    void Start()
    {
        rb2d = this.GetComponent<Rigidbody2D>();
        Move();
        deathTime = deathTime / Program.Time_Scale;
    }

    public virtual void Move()
    {
        rb2d.AddForce(new Vector2(Mathf.Cos((Program.Weapon_Rotation) / 180 * Mathf.PI) , 
            Mathf.Sin((Program.Weapon_Rotation) / 180 * Mathf.PI) ).normalized*speed);

        Invoke("destory",deathTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Hurt();
        destory();
    }

    public virtual void Hurt()
    {
        //other.gameObject.GetComponent<>()
        //伤害和其他特殊效果
    }

    public virtual void destory()
    {
        Destroy(this.gameObject);
    }
    
    //子弹运行前设定大小
    /*protected void ChangeSize(){
        transform.localScale = Vector3.one * size;
    }
    protected void Move(){
        transform.Translate(Vector3.forward *speed);//创建物体的角度的方向平移，需优化为实际方向
    }*/
}
