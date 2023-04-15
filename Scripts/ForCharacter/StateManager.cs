using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
    public bool Is_Main_Character;
    [Header("管理的信息")]
    public Image HP_Bar;
    public Image Bullet_Ring;
    public TextMeshProUGUI Bullet_Value;


    private void Start()
    {
        HP_Bar = this.transform.Find("HP_Bar").GetComponent<Image>();
        transform.parent.GetComponent<Canvas>().worldCamera = Camera.main;
    }

    private void OnEnable()
    {
        HP_Now = HP_Max;
        HP_Rec = HP_Max;
    }

    private void Update()
    {
        if (HP_Rec != HP_Now)
        {
            HP_Bar.fillAmount = ChangeState(ref HP_Rec, ref HP_Now, HP_Max);
        }

        if (Is_Main_Character)
        {
            //如果program.weapon_now正在播放reload动画
            if (Program.Instance.Weapon_Now.Is_Reloading)
            {
                Bullet_Value.text = "Reloading";
                Bullet_Ring.fillAmount = Program.Instance.NumberLerp(Bullet_Ring.fillAmount, 1, ref Program.Instance.Weapon_Now.Reload_Ani_Time_Left);
            }
            else
            {
                Bullet_Value.text = Program.Instance.Weapon_Now.Now_Magazing.ToString();
                Bullet_Ring.fillAmount = (float)Program.Instance.Weapon_Now.Now_Magazing / (float)Program.Instance.Weapon_Now.Full_Magazing;
            }
        }
    }

    public float ChangeState(ref float State_New, ref float State_Now, float State_Max)
    {
        if (State_New != State_Now)
            State_Now -= Lose_Speed * Time.deltaTime;
        if (State_Now <= State_New)
            State_Now = State_New;
        //对HP_New和HP_Now取整，防止出现小数点后面的数值
        if ((int)State_New == (int)State_Now)
            State_Now = State_New;
        if (State_New > State_Max)
        {
            State_New = State_Max;
            State_Now = State_Max;
        }

        if (State_Now < 1)
            State_Now = 0;
        return State_Now / State_Max;
    }
}