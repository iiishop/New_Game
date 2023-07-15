using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Spine.Unity;

public class EnemyController : MonoBehaviour
{
    private Rigidbody Rb;
    private Animator Ani;
    private SkeletonAnimation SA;
    private SpriteRenderer SR;
    private CharacterState CS;
    public bool IsSpine = false;



    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        if (!IsSpine)
        {
            Ani = GetComponent<Animator>();
        }
        else
        {
            SA = GetComponent<SkeletonAnimation>();
        }
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
            if (!IsSpine)
            {
                Ani.SetBool("IsGrounded", false);
            }
            else
            {
                if (SA.AnimationName != "Jump")
                {
                    SA.AnimationState.SetAnimation(0, "Stalling", true);
                }
                else
                {
                    SA.AnimationState.AddAnimation(0, "Stalling", true, 0);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            if (!IsSpine)
            {
                Ani.SetBool("IsGrounded", true);
            }
        }

        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -0.6f);
    }
}
