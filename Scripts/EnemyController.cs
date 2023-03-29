using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody Rb;
    private Animator Ani;
    private SpriteRenderer SR;
    private CharacterState CS;

    

    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        Ani = GetComponent<Animator>();
        SR = GetComponent<SpriteRenderer>();
        CS = GetComponent<CharacterState>();
    }

    void Update()
    {
        
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Ani.SetBool("IsGrounded", false);
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Ani.SetBool("IsGrounded", true);
        }
    }
}
