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
    private float speed_now;

    private void Start()
    {
        target = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (target)
        {
            speed_now = speed*(transform.position - target.transform.position).magnitude; 
            transform.position = Vector3.Lerp(transform.position,
                Quaternion.Euler(target.transform.eulerAngles) * offset + target.transform.position,
                speed * Time.unscaledDeltaTime);
        }
    }
}