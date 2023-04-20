using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;

/// <summary>
/// 这个脚本用于将渲染方式为Screen Space - Camera的UI元素，移动到鼠标的位置
/// </summary>
public class MousePoint : MonoBehaviour
{
    private CharacterState CS;
    private Camera Main_Camera;
    [Header("弹药环相关")]
    public GameObject Bullet_Ring;
    public Vector2 Offset;
    public float Follow_Speed;
    public float Rotate_Speed;
    public float Rotate_Speed_Reload;

    private void Start()
    {
        CS = Program.Instance.Main_Character.GetComponent<CharacterState>();
        Main_Camera = Camera.main;
        //隐藏鼠标
        Cursor.visible = false;
    }

    private void Update()
    {
        Vector3 Pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Pos.z);
        transform.position = Main_Camera.ScreenToWorldPoint(mousePos);
        transform.position = new Vector3(transform.position.x, transform.position.y, CS.transform.position.z - 3f);


        Bullet_Ring_Function();
    }

    private void Bullet_Ring_Function()
    {
        if (Program.Instance.Weapon_Now.Is_Reloading)
        {

            Bullet_Ring.transform.Rotate(0, 0, Rotate_Speed_Reload * Time.unscaledDeltaTime);
        }
        else
        {
            Bullet_Ring.transform.Rotate(0, 0, Rotate_Speed * Time.unscaledDeltaTime);
        }
        Bullet_Ring.transform.position = Program.Instance.Lerp2Tagert(Bullet_Ring.transform.position, this.transform.position, new Vector3(Offset.x, Offset.y, 0), Follow_Speed);

    }

}
