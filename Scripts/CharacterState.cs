using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 放在每个角色身上，用于管理角色的状态。现在只有血量，后续可以加入更多的状态
/// </summary>
public class CharacterState : MonoBehaviour
{
    public float HP;
    public bool Died = false;
    public StateManager StateManager;
    public Vector3 Offset;
    public float Follow_Speed;

    private SpriteRenderer SR;
    private Animator Ani;
    private Rigidbody Rb;

    private void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        Ani = GetComponent<Animator>();
        Rb = GetComponent<Rigidbody>();
        if (this.tag != "Player")
        {
            var State = ObjectPool.Instance.GetPool(ObjectPool.Instance.State_Prefab);
            //让状态管理器的父物体为场景中名字为Canvas的物体
            State.transform.SetParent(GameObject.Find("Canvas").transform);
            State.GetComponent<RectTransform>().localScale = Vector3.one;
            
            StateManager = State.GetComponent<StateManager>();
        }
        
        StateManager.HP_Max = HP;
        StateManager.HP_Now = HP;
        StateManager.HP_Rec = HP;
    }

    private void Update()
    {
        Follow();
        Ani.SetFloat("AirSpeed", Rb.velocity.y);
    }
    
    private void Follow()
    {
        StateManager.transform.position = Vector3.Lerp(StateManager.transform.position, new Vector3(
                Offset.x * (SR.flipX ? -1f : 1f),
                Offset.y, Offset.z) + this.transform.position,
            Follow_Speed * Time.unscaledDeltaTime);
    }

    public void ChangeHP(float HP)
    {
        if (HP > StateManager.HP_Max)
            this.HP = StateManager.HP_Max;
        else
            this.HP = HP;
        if (HP > 1)
        {
            StateManager.HP_Rec = HP;
            Hurted();
        }
        else if(!Died)
        {
            StateManager.HP_Rec = 0;
            Die();
        }
    }

    public virtual void Hurted()
    {
        Ani.SetTrigger("Hurted");
    }

    public virtual void Die()
    {
        Ani.SetTrigger("Die");
        Died = true;
    }
}