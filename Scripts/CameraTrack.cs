using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
public class CameraTrack : MonoBehaviour
{
    public GameObject target;
    public Vector3 offset;
    public float speed = 0.5f;
    void Update()
    {

        if (target)
        {
            transform.position=Vector3.Lerp(transform.position, Quaternion.Euler(target.transform.eulerAngles) * offset + target.transform.position, speed);
        }
    }
}