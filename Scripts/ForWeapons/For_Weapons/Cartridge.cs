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
        var force2 = Random.Range(-2.5f, -1.5f);
        if(Program.Weapon_Rotation is > 90 and < 270)
            Force = -Force;

        rb = this.GetComponent<Rigidbody>();

        rb.velocity = new Vector3(Mathf.Cos((Program.Weapon_Rotation+90) / 180 * Mathf.PI) * force2,
            Mathf.Sin((Program.Weapon_Rotation + 90) / 180 * Mathf.PI) * Force, force2);
        
        Invoke("Destroy",deathTime);
    }
    public virtual void Destroy()
    {
        Destroy(this.gameObject);
    }
}