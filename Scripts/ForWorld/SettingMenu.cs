using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMenu : MonoBehaviour
{
    public GameObject KeyConfirmatation;
    public void KeyBind(string keyName)
    {
        KeyConfirmate.KeyName = keyName;
        KeyConfirmatation.SetActive(true);
    }
}
