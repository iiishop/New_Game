using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (HP > 0)
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
