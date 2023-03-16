using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class StateManager : MonoBehaviour
{
    public float Lose_Speed;
    public float HP_Max;
    public float HP_Now;
    public float HP_Rec;
    

    private void Update()
    {
        if (HP_Rec != HP_Now)
        {
            ChangeHP(HP_Rec);
            Debug.Log(HP_Now / HP_Max);
            this.transform.GetChild(0).GetComponent<Image>().fillAmount = HP_Now / HP_Max;
        }
    }

    public void ChangeHP(float HP_New)
    {
        //让血条线性平滑地改变到指定值，HP的值不确定，fillAmount的值在0~1之间
        //这里的Lose_Speed是一个常数，可以在Inspector面板中调整
        //这里的HP是一个变量，可以在其他脚本中调用这个函数来改变
        
        
        
        if(HP_New!=HP_Now)
            HP_Now = Mathf.Lerp(HP_Now, HP_New, Lose_Speed * Time.unscaledDeltaTime);
        if (HP_Now <= HP_New)
        {
            HP_Now = HP_New;
        }
        //对HP_New和HP_Now取整，防止出现小数点后面的数值
        HP_New = (int) HP_New;
        HP_Now = (int) HP_Now;
        if (HP_New == HP_Now)
        {
            HP_Rec=HP_Now;
        }

        if (HP_New > HP_Max)
        {
            HP_Rec=HP_Max;
            HP_Now=HP_Max;
        }
            
    }
}
