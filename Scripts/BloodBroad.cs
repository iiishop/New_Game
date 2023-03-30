using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodBroad : MonoBehaviour
{
    public float Lose_Speed = 100f;
    public float Up_Speed = 0.1f;
    public Vector3 Offset;
    public Text _Text;
    public Color _Color;
    private RectTransform Rect_Trans;
    public GameObject Other;
    public Vector3 Offset_Now;

    private void OnEnable()
    {
        _Color.a = 1;
        Offset_Now = Offset;
    }

    void Start()
    {
        _Text = this.GetComponent<Text>();
        Rect_Trans = GetComponent<RectTransform>();
        Rect_Trans.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        this.GetComponent<Text>().color = _Color;
    }

    void Update()
    {
        //如果没有跟随对象则销毁
        if (Other == null)
        {
            ObjectPool.Instance.ReturnPool(this.gameObject);
            return;
        }
        Follow();
        Offset_Now += new Vector3(0, Up_Speed * Time.unscaledDeltaTime, 0);
        _Color.a -= Lose_Speed * Time.unscaledDeltaTime;
        _Text.color = _Color;
        if(_Color.a<=0.01f)
            ObjectPool.Instance.ReturnPool(this.gameObject);
    }

    void Follow()
    {
        this.transform.position = Other.transform.position + Offset_Now;
    }

    public void OnTextChange(string val)
    {
        //根据文字长度改变宽度
        this.GetComponent<Text>().text = val;
    }
}
