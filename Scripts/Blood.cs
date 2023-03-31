using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    private ParticleSystem PS;
    void Start()
    {
        PS = GetComponent<ParticleSystem>();
    }
    void Update()
    {
        //当粒子效果播放完毕后，返回对象池
        if (!PS.IsAlive())
        {
            ObjectPool.Instance.ReturnPool(this.gameObject);
        }
    }
}
