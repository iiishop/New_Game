using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons
{
    public class SingleGun : Weapon
    {
        bool Is_Fire = false;

        public override void Shot()
        {
            base.Shot();
            Is_Fire = true;
        }


        public override bool CheckFire()
        {
            var info = Program.Instance.Weapon_Now.Ani.GetCurrentAnimatorStateInfo(0);
            return !Is_Reloading && Now_Magazing > 0 && !Ani.GetBool("Fire") && !Is_Fire && !info.IsName("Fire");
        }

        private void MouseUp(params object[] args)
        {
            Is_Fire = false;
        }

        private void Start()
        {
            WeaponManager.Signal.AddListener(MouseUp);
        }
    }
}