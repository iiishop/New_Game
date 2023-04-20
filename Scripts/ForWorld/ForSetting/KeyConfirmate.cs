using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class KeyConfirmate : MonoBehaviour
{
    public static Signal Key_Confirmate = new Signal();
    public static string KeyName = "";
    public TextMeshPro Text;
    bool Is_Abled = false;
    private void OnEnable()
    {
        Is_Abled = true;
        Text.text = "按下任意键";
    }

    private void OnDisable()
    {
        Is_Abled = false;
    }
    private void Update()
    {
        if (!Is_Abled) return;
        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(vKey))
            {
                Debug.Log("KeyCode down: " + vKey);
                var t = Program.Instance.Program_Save_Object.SetKey(KeyName, vKey);
                if (t == "Success")
                {
                    Key_Confirmate.Call();
                    gameObject.SetActive(false);
                }
                else
                {
                    Text.text = "按键: " + vKey.ToString() + " 被功能: " + t + " 占用";
                }
            }
        }
    }
}
