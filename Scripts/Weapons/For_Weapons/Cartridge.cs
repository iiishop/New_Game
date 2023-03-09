using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cartridge : MonoBehaviour
{
    private Rigidbody rb;
    public float deathTime =3f;
    public float Force;

    void Start()
    {
        //随机取一个范围在-40到-60之间的数
        var force2 = Random.Range(-50f, -10f);
        if(Program.Weapon_Rotation is > 90 and < 270)
            Force = -Force;

        rb = this.GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(Mathf.Cos((Program.Weapon_Rotation+90) / 180 * Mathf.PI) * Force,
            Mathf.Sin((Program.Weapon_Rotation+90) / 180 * Mathf.PI) * Force, force2)); //*f.localEulerAngles.z);
        Invoke("destory",deathTime);
    }
    public virtual void destory()
    {
        Destroy(this.gameObject);
    }
}