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
    public float damage;
    Rigidbody rb;
    public float deathTime = 3f;
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        Move();
    }

    public virtual void Move()
    {
        //rb2d.AddForce(new Vector2(Mathf.Cos((Program.Weapon_Rotation) / 180 * Mathf.PI) , 
        //    Mathf.Sin((Program.Weapon_Rotation) / 180 * Mathf.PI) )*speed);
        rb.velocity = new Vector2(Mathf.Cos((Program.Weapon_Rotation) / 180 * Mathf.PI) * speed,
            Mathf.Sin((Program.Weapon_Rotation) / 180 * Mathf.PI) * speed);
        
        Invoke("destory",deathTime);
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("碰撞"+other.gameObject.name);
        if (other.gameObject.CompareTag("Enemy"))
        {
            Hurt(other);
            destory();
        }
        
    }

    public virtual void Hurt(Collider other)
    {
        //other.gameObject.GetComponent<>()
        //伤害和其他特殊效果
        var HP = other.gameObject.GetComponent<CharacterState>().HP;
        
        if(HP-damage<0)
            other.gameObject.GetComponent<CharacterState>().ChangeHP(0);
        else
            other.gameObject.GetComponent<CharacterState>().ChangeHP(HP-damage);
    }

    public virtual void destory()
    {
        Destroy(this.gameObject);
    }

}
