using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Tools;
using System.Linq;

public class ForAttackCheck : MonoBehaviour
{
    public Attack_Mode AM { set; get; }
    public ForCheckBox FCB { set; get; }
    //2023.07.25
    //NOTDO:计算远程武器枪口是否对准了主角| 我认为只要进入了攻击范围就可以开火，可以添加一个扇形的攻击范围检测器
    //若将来引入了使用射线进行检测是否对准，那么可以不用加入扇形的攻击范围检测器
    //当然，可想而知，使用射线检测会更加简单和便捷
    //因此，逻辑可以是： 进入checkbox-》Gun判断角度以进行顺时针或逆时针旋转-》使用射线检测是否可以碰撞到主角-》可以则停止旋转，不可以则继续旋转-》进入开火逻辑
    //2023.07.27 DONE:我忘记了checkbox和attachcheckbox在角色进行翻转或其他旋转操作后是否会跟随变化，希望在下一次可以测试并解决这个问题|并不会:w

    //DONE:开火频率
    public SkeletonAnimation SA;
    private float last_shoot_time;//上一次开火的缩放后的具体时间
    private Ray attack_check_ray;
    private bool can_attach_player = false;
    private GameObject player;
    public bool Is_Player_In { get; set; } = false;
    private float maxDistance;
    private void Start()
    {
        player = Program.Instance.Main_Character;

        var distance = new float[] { AM.attack_collider_offset.x, AM.attack_collider_offset.y, AM.attack_collider_size.x, AM.attack_collider_size.y, AM.attack_collider_size.z, AM.attack_collider_size.w };
        var max = Mathf.Max(distance);
        distance = distance.Where(x => x != max).ToArray();
        var second_max = Mathf.Max(distance);
        maxDistance = Mathf.Sqrt(max * max + second_max * second_max);
    }
    private void OnTriggerStay(Collider other)
    {
        var can_shoot = last_shoot_time + AM.Shoot_Wait_Time < Time.time;
        if (other.gameObject.CompareTag("Player"))
        {
            Is_Player_In = true;
            if (can_shoot && can_attach_player)
            {
                Attack();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            BackToIdle();
        }
    }
    private void FixedUpdate()
    {
        attack_check_ray = new Ray(AM.Gun.transform.position, (AM.attack_position.transform.position - AM.Gun.transform.position).normalized);
        if (FCB.Is_Player_In && !can_attach_player && AM.Gun_Rot_Speed != 0 && AM.Is_Shoot)
        {
            //DONE:用射线检测是否可以碰撞到主角
            ROT(AM.Rot_Range[0], AM.Rot_Range[1]);
        }
        if (FCB.Is_Player_In)
        {
            Ray_Check();
        }
    }
    private void Ray_Check()
    {
        Debug.DrawRay(attack_check_ray.origin, attack_check_ray.direction * maxDistance, Color.red);
        if (Physics.Raycast(attack_check_ray))
        {
            foreach (var item in Physics.RaycastAll(attack_check_ray, maxDistance))
            {
                if (item.collider.gameObject.CompareTag("Player"))
                {
                    can_attach_player = true;
                    return;
                }
            }
            //对于发射跟踪导弹的炮台使用
            if (AM.Gun_Rot_Speed == 0)
            {
                can_attach_player = true;
                return;
            }
            can_attach_player = false;
        }
    }


    private void Attack()
    {
        //TODO:去掉攻击模式的判断，攻击方式的选择以距离而定。这里会让每个攻击动画在其对应的层级里播放，以达到同时攻击的效果
        SA.PlaySpineAnimation($"Attack{AM.id}", false);
        last_shoot_time = Time.time;
        var t = AM.attack.GetComponent<Bullet>();
        t.is_Enemy_Bullet = true;
        t.Weapon_Rotation = AM.Gun.transform.rotation.eulerAngles.z;
        Instantiate(AM.attack, AM.attack_position.transform.position, AM.Gun.transform.rotation);
    }
    private void BackToIdle()
    {
        Is_Player_In = false;
        SA.PlaySpineAnimation("Idle", true);
    }

    //TODO:目前的ROT只能用于拥有固定基座的旋转炮塔，若需要使用例如基于骨骼节点对齐等方式的旋转，还需尝试改进，若无法改进，即在设计上避免
    public virtual void ROT(float angleUp = 180, float angleDown = 180)
    {
        //判断player和当前的旋转角度决定是顺时针还是逆时针旋转
        var player_pos = player.transform.position;
        var gun_pos = AM.Gun.transform.position;
        var player_angle = Mathf.Atan2(player_pos.y - gun_pos.y, player_pos.x - gun_pos.x) * Mathf.Rad2Deg;
        var gun_angle = AM.Gun.transform.rotation.eulerAngles.z;
        var angle = player_angle - gun_angle;

        angle = angle > 180 ? angle - 360 : angle < -180 ? angle + 360 : angle;
        #region 旋转限制测试
        //gun_angle = gun_angle > 180 ? gun_angle - 360 : gun_angle < -180 ? gun_angle + 360 : gun_angle;
        //DEBUG.Instance.Display_Info($"player_angle:{player_angle},gun_angle:{gun_angle},angle:{angle}");
        ////angleUP和angleDown是枪口的旋转角度限制
        //if ((gun_angle > angleUp && angle > gun_angle) || (gun_angle < -angleDown && angle < gun_angle))
        //{
        //    return;
        //}
        #endregion
        AM.Gun.transform.Rotate(0, 0, AM.Gun_Rot_Speed * Time.fixedDeltaTime * (angle > 0 ? 1 : angle < 0 ? -1 : 0));
    }
}
