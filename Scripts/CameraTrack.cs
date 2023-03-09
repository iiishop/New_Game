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
    private bool isRoatating = false;
    void FixedUpdate()
    {
        PlayerController.signal.AddListener(Onsignal);

        if (target&&!isRoatating)
        {
            transform.position=Vector3.Lerp(transform.position, Quaternion.Euler(target.transform.eulerAngles) * offset + target.transform.position, speed);
        }
    }
    void Onsignal(params object[] args)
    {
        if ((bool)args[0])
        {
            isRoatating = true;
            transform.parent = target.transform;
        }
        else
        {
            transform.parent = null;
            isRoatating = false;
        }
    }
}