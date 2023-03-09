using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Weapons
{
    public class AccumulatorWeapon : Weapon
    {
        public bool Is_Fire = false;
        public float Max_AccumulatorTime = 0.5f;
        public float AccumulatorTime = 0;
        public float Percent_AccumulatorTime = 0;
        

        public override void Fire()
        {
            if (Is_Fire) return;
            if (!CheckFire()) return;
            AccumulatorTime = 0;
            Is_Fire = true;
            Shot();
        }

        private void FixedUpdate()
        {
            if (Is_Fire)
            {
                AccumulatorTime += Time.deltaTime;
                
                if (AccumulatorTime >= Max_AccumulatorTime)
                {
                    AccumulatorTime = Max_AccumulatorTime;
                }
            }
        }

        public override bool CheckFire()
        {
            return !Is_Reloading && Now_Magazing > 0 && !Ani.GetBool("Fire");
        }

        private void MouseUp(params object[] args)
        {
            Percent_AccumulatorTime = AccumulatorTime / Max_AccumulatorTime;
            Is_Fire = false;
            AccumulatorTime = 0;
        }

        private void Start()
        {
            WeaponManager.Signal.AddListener(MouseUp);
        }
    }
}