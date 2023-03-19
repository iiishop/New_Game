using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;

/// <summary>
/// 放在血条等的父物体上，用于管理状态。现在只有血量，后续可以加入更多的状态
/// </summary>
public class StateManager : MonoBehaviour
{
    public float Lose_Speed;
    public float HP_Max;
    public float HP_Now;
    public float HP_Rec;
    public Image HP_Bar;

    private void Start()
    {
        HP_Bar = this.transform.GetChild(0).GetComponent<Image>();
    }

    private void Update()
    {
        if (HP_Rec != HP_Now)
        {
            ChangeHP(HP_Rec);
            HP_Bar.fillAmount = HP_Now / HP_Max;
        }
    }

    public void ChangeHP(float HP_New)
    {
        if(HP_New!=HP_Now)
            HP_Now = Mathf.Lerp(HP_Now, HP_New, Lose_Speed * Time.unscaledDeltaTime);
        if (HP_Now <= HP_New)
            HP_Now = HP_New;
            //对HP_New和HP_Now取整，防止出现小数点后面的数值
        if ((int)HP_New == (int)HP_Now)
            HP_Rec=HP_Now;
        if (HP_New > HP_Max)
        {
            HP_Rec = HP_Max;
            HP_Now = HP_Max;
        }

        if (HP_New < 1)
        {
            HP_Now = 0;
        }
    }
}
