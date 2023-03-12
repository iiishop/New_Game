using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pass_Enemy_Bullet : Bullet
{
    public override void destory(string tag)//为实现双穿添加参数
    {
        if (tag!="Enemy"){
        Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update

}
