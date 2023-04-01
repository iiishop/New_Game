using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowScript : MonoBehaviour
{
    private Transform Player;

    private SpriteRenderer This_Sprite;
    private SpriteRenderer Player_Sprite;

    private Color This_Color;
    

    [Header("不透明度控制")] 
    public float Alpha_Start;
    public float Alpha_Mutiplier;
    public float Alpha;
    

    private void OnEnable()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        This_Sprite = this.GetComponent<SpriteRenderer>();
        Player_Sprite = Player.GetComponent<SpriteRenderer>();
        
        this.transform.localScale = Player.localScale;

        Alpha = Alpha_Start;
        This_Color = new Color(1, 1, 1, Alpha);
        This_Sprite.color = This_Color;
        
        This_Sprite.sprite = Player_Sprite.sprite;
        This_Sprite.flipX = Player_Sprite.flipX;
        This_Sprite.flipY = Player_Sprite.flipY;
        transform.position = Player.position;
        
    }

    void FixedUpdate()
    {
        This_Color = new Color(1, 1, 1, Alpha);
        This_Sprite.color = This_Color;
        
        var t = Alpha_Mutiplier/Time.fixedDeltaTime;
        if (t >= 1)
            t = 0.99f;
        Alpha *= t;
        

        if (Alpha<=0.01f)
        {
            ObjectPool.Instance.ReturnPool(this.gameObject);
        }
    }
}
