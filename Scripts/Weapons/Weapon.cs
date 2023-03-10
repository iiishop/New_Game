using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Weapons
{
    [System.Serializable]
    public class Weapon : MonoBehaviour
    {
        public GameObject Bullet;           //子弹的预制体
        public GameObject Muzzle;           //枪口
        public GameObject Cartridge;        //弹壳
        public GameObject Cartridge_Ejector;//弹壳弹出口
        public string Special_Mode;         //特殊模式，比如说追踪，穿透等等(由,分割)
        public Animator Ani;                //动画控制器
        public int Full_Magazing = 0;       //弹夹容量
        public int Now_Magazing = 0;        //当前弹夹内的子弹数量
        public bool Is_Reloading = false;   //是否正在装弹
        public ParticleSystem Fire_Particle;
        private GameObject New_Bullet;
        public Camera Main_Camera;
        private GameObject Father;
        [Header("跟随设置")]
        public float Follow_Speed;
        public Vector3 Follow_Offset;
        private int Follow_Face = 1;
        [Header("漂浮设置")]
        public float Float_Speed;
        public float Float_Amplitude;

        private void Start()
        {
            Father = Program.Main_Character;
        }

        public virtual void Reload()
        {
            Now_Magazing = Full_Magazing;
            Ani.SetBool("Reload", true);
            Is_Reloading = true;
        }
        /// <summary>
        /// 按下鼠标左键时调用
        /// </summary>
        public virtual void Fire()
        {
            if (CheckFire())
            {
                Shot();
            }
        }
        /// <summary>
        /// 确认可以开火时调用
        /// </summary>
        public virtual void Shot()
        {
            Program.Weapon_Rotation = this.gameObject.transform.localEulerAngles.z;
            
            Fire_Particle.Play();
            Ani.SetBool("Fire", true);
            Now_Magazing--;
            New_Bullet = Instantiate(Bullet, Muzzle.transform.position, Muzzle.transform.rotation);
            Instantiate(Cartridge, Cartridge_Ejector.transform.position, Cartridge_Ejector.transform.rotation);
            
        }
        public virtual bool CheckFire()
        {
            return true;
        }

        private void Update()
        {
            ROT();
            Follow();
        }

        private void ROT()
        {
            //获取鼠标的坐标，鼠标是屏幕坐标，Z轴为0，这里不做转换  
            Vector3 mouse = Input.mousePosition;
            //获取物体坐标，物体坐标是世界坐标，将其转换成屏幕坐标，和鼠标一直  
            Vector3 obj = Main_Camera.WorldToScreenPoint(transform.position);
            //屏幕坐标向量相减，得到指向鼠标点的目标向量，即黄色线段  
            Vector3 direction = mouse - obj;
            //转过中线左右翻转
            if (direction.x>=0)
            {
                
                GetComponent<SpriteRenderer>().flipY = false;
                this.gameObject.transform.GetChild(0).transform.localRotation = new Quaternion(0, 0, 0, 0);
            }
            else
            {
                
                GetComponent<SpriteRenderer>().flipY = true;
                this.gameObject.transform.GetChild(0).transform.localRotation = new Quaternion(180, 0, 0, 0);
            }
            Vector3 Player_Direction = mouse- Main_Camera.WorldToScreenPoint(Father.transform.position);
            if(Player_Direction.x>=0)
                Follow_Face = 1;
            else
                Follow_Face = -1;

            this.transform.localEulerAngles = new Vector3(0, this.transform.localEulerAngles.y, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        }

        private void Follow()
        {
            transform.position=Vector3.Lerp(transform.position, 
                Quaternion.Euler(Father.transform.eulerAngles) * new Vector3(Follow_Offset.x*Follow_Face, Follow_Offset.y,Follow_Offset.z) + Father.transform.position, Follow_Speed);
            //让物体在y轴上下浮动
            transform.position = new Vector3(transform.position.x,
                Follow_Offset.y + Mathf.Sin(Time.unscaledTime * Float_Speed) * Float_Amplitude, 
                transform.position.z);
        }
    }
}
