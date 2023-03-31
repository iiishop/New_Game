using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Weapons;

[System.Serializable]
public class Bullet : MonoBehaviour//缺少碰撞后销毁、伤害计算
{
    //成员变量
    public float speed;
    public float damage;
    Rigidbody rb;
    public float deathTime = 3f;
    public bool isPlayerBullet = true;
    
    public bool canPassCharacter = false;
    public bool canPassWall = false;
    
    private List<GameObject> HitList = new List<GameObject>();
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        Move();
    }

    public virtual void Move()
    {
        rb.velocity = new Vector2(Mathf.Cos((Program.Weapon_Rotation) / 180 * Mathf.PI) * speed,
            Mathf.Sin((Program.Weapon_Rotation) / 180 * Mathf.PI) * speed);
        
        Invoke("Destroy",deathTime);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")&&other.gameObject.GetComponent<CharacterState>().HP>0&&isPlayerBullet&&!HitList.Contains(other.gameObject))
        {
            Hurt(other);
            if(!canPassCharacter)
                Destroy();
        }
        else if (other.gameObject.CompareTag("Player")&&other.gameObject.GetComponent<CharacterState>().HP>0&&!isPlayerBullet&&!HitList.Contains(other.gameObject))
        {
            Hurt(other);
            if(!canPassCharacter)
                Destroy();
        }
        else if (!canPassWall && !HitList.Contains(other.gameObject))
        {
            if (other.gameObject.GetComponent<CharacterState>() == null&&other.gameObject.GetComponent<Collider>().isTrigger==false)
            {
                Destroy();
            }
        }
        
        
    }

    public virtual void Hurt(Collider other)
    {
        HitList.Add(other.gameObject);
        ShowBloodBroad(other);
        ShowBlood(other);
        
        //伤害和其他特殊效果
        var HP = other.gameObject.GetComponent<CharacterState>().HP;
        
        if(HP-damage<0)
            other.gameObject.GetComponent<CharacterState>().ChangeHP(0);
        else
            other.gameObject.GetComponent<CharacterState>().ChangeHP(HP-damage);
    }
    
    public virtual void ShowBloodBroad(Collider other)
    {
        var hit = other.ClosestPointOnBounds(transform.position)-other.gameObject.transform.position;
        var BloodBroad = ObjectPool.Instance.GetPool(ObjectPool.Instance.Prefabs[2]);
        BloodBroad.GetComponent<BloodBroad>().Other = other.gameObject;
        BloodBroad.transform.SetParent(GameObject.Find("Canvas").transform);
        BloodBroad.GetComponent<BloodBroad>().OnTextChange(damage.ToString());
        BloodBroad.GetComponent<BloodBroad>().Offset_Now = BloodBroad.GetComponent<BloodBroad>().Offset + hit;
    }
    
    public virtual void ShowBlood(Collider other)
    {
        var Blood = ObjectPool.Instance.GetPool(ObjectPool.Instance.Prefabs[3]);
        //Blood的父物体是世界
        Blood.transform.SetParent(null);
        //Blood的位置是碰撞点
        Blood.transform.position = other.ClosestPointOnBounds(transform.position);

        if (transform.rotation.eulerAngles.y == 0)
        {
            //面向右
            Blood.transform.rotation = Quaternion.Euler(-transform.rotation.z/math.PI*180f,90f,transform.rotation.x);
        }
        else
        {
            //面向左
            if (transform.rotation.y > 0)
            {
                Blood.transform.rotation = Quaternion.Euler(-transform.rotation.x/math.PI*180f,270f,transform.rotation.z);
            }
            else
            {
                Blood.transform.rotation = Quaternion.Euler(transform.rotation.x/math.PI*180f,270f,transform.rotation.z);
            }
        }
        //Blood的z轴位置-0.1f
        Blood.transform.position = new Vector3(Blood.transform.position.x, Blood.transform.position.y, Blood.transform.position.z - 0.1f);
        
        
    }

    public virtual void Destroy()
    {
        Destroy(this.gameObject);
    }

}
