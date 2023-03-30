using System;
using System.Collections;
using System.Collections.Generic;
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
        Debug.Log("碰撞"+other.gameObject.name);
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
        else if(!canPassWall&& !HitList.Contains(other.gameObject))
            Destroy();
        
        
    }

    public virtual void Hurt(Collider other)
    {
        HitList.Add(other.gameObject);
        var BloodBroad = ObjectPool.Instance.GetPool(ObjectPool.Instance.Prefabs[2]);
        BloodBroad.GetComponent<BloodBroad>().Other = other.gameObject;
        BloodBroad.transform.SetParent(GameObject.Find("Canvas").transform);
        BloodBroad.GetComponent<BloodBroad>().OnTextChange(damage.ToString());
        var hit = other.ClosestPointOnBounds(transform.position)-other.gameObject.transform.position;
        BloodBroad.GetComponent<BloodBroad>().Offset_Now = BloodBroad.GetComponent<BloodBroad>().Offset + hit;
        //伤害和其他特殊效果
        var HP = other.gameObject.GetComponent<CharacterState>().HP;
        
        if(HP-damage<0)
            other.gameObject.GetComponent<CharacterState>().ChangeHP(0);
        else
            other.gameObject.GetComponent<CharacterState>().ChangeHP(HP-damage);
    }

    public virtual void Destroy()
    {
        Destroy(this.gameObject);
    }

}
