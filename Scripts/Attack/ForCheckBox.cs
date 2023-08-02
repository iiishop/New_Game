using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Tools;

public class ForCheckBox : MonoBehaviour
{
    public GameObject Enemy;
    private EnemyController EC;
    public bool Is_Player_In = false;
    private List<ForAttackCheck> FACs = new List<ForAttackCheck>();
    private void Start()
    {
        EC = Enemy.GetComponent<EnemyController>();
        FACs.AddRange(Enemy.GetComponentsInChildren<ForAttackCheck>());
    }
    //这个脚本的碰撞箱被触发时，即主角进入了敌人的索敌范围，需要：
    //DONE1.让敌人朝向主角
    //DONE2.将敌人的攻击模式为远程的武器，枪口转动直到朝向主角
    //DONE3.敌人向主角方向运动直到进入攻击范围
    //DONE4.敌人开始攻击
    //DONE5.运动直到进入最后一个攻击范围，结束运动
    //DONE6.主角离开最后一个攻击范围，向主角运动；主角靠近，远离主角运动
    //TODO:敌人的动画系统的完善
    //TODO:敌人的跳跃
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Is_Player_In = true;
            if (EC.Can_Move)
            {
                //默认敌人方向为右
                if (other.transform.position.x > Enemy.transform.position.x && EC.IsFlip)
                {
                    EC.CharacterFlip();
                }
                else if (other.transform.position.x < Enemy.transform.position.x && !EC.IsFlip)
                {
                    EC.CharacterFlip();
                }
                var t = FACs.Select(x => x.Is_Player_In).ToList();
                //DONE:这里需要判断是否所有的武器都可以开火，如果不是，则需要移动
                var playerInAllCheckBoxes = t.IsAllBool(true);
                if (playerInAllCheckBoxes)
                {
                    EC.Stop();
                }
                else
                {
                    EC.Move();
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Is_Player_In = false;
        }
    }
}
