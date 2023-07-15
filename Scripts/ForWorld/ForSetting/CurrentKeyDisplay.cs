using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentKeyDisplay : MonoBehaviour
{
    public TextMeshPro Text;
    public string KeyName;
    private void Start()
    {
        Text = GetComponent<TextMeshPro>();
        GetKey();
        KeyConfirmate.Key_Confirmate.AddListener(GetKey);
    }
    public void GetKey(params object[] args)
    {
        Text.text = Program.Instance.Program_Save_Object.Key_Settings[KeyName].ToString();
    }
}
