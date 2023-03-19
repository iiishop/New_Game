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
    public StateManager StateManager;

    private void Start()
    {
        StateManager.HP_Max = HP;
        StateManager.HP_Now = HP;
        StateManager.HP_Rec = HP;
    }

    public void ChangeHP(float HP)
    {
        if(HP>StateManager.HP_Max)
            HP = StateManager.HP_Max;
        else
            this.HP = HP;
        if (HP > 1)
        {
            StateManager.HP_Rec = HP;
            Hurted();
        }
        else
        {
            StateManager.HP_Rec = 0;
            Die();
        }
    }

    public virtual void Hurted()
    {
        
    }

    public virtual void Die()
    {
        
    }
}
