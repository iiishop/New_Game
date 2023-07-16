using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForAttackCheck : MonoBehaviour
{
    public GameObject Attack_Prefab;
    public GameObject Attack_Position;
    //TODO:计算远程武器枪口是否对准了主角
    //TODO:开火频率
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            Attack();
        }
    }
    public void Attack()
    {
        DEBUG.Instance.Display_Info($"Attack{Attack_Prefab.name}");
        Instantiate(Attack_Prefab, Attack_Position.transform.position, Attack_Position.transform.rotation);
    }
}
