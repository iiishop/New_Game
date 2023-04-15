using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

[CreateAssetMenu(fileName = "Program_Save", menuName = "Program_Save")]
public class Program_Save : ScriptableObject
{
    public List<Weapon> Weapon_Inventory = new List<Weapon>();
    //定义一个名为Key_Settings的字典，用于存储按键设置，并且重载添加方法，保证其中的键值对不会重复
    public Dictionary<string, KeyCode> Key_Settings = new Dictionary<string, KeyCode>()
    {
        {"Down", KeyCode.S},
        {"Left", KeyCode.A},
        {"Right", KeyCode.D},
        {"Jump", KeyCode.Space},
        {"Dash", KeyCode.LeftShift},
        {"Attack", KeyCode.Mouse0},
        {"Skill", KeyCode.Mouse1},
        {"Inventory", KeyCode.Tab},
        {"Pause", KeyCode.Escape},
        {"Map", KeyCode.M},
        {"Menu", KeyCode.Escape},
        {"Save", KeyCode.F5},
        {"Load", KeyCode.F9},
        {"Debug", KeyCode.F12},
    };
    public Dictionary<KeyCode, string> Key_Settings_Reverse = new Dictionary<KeyCode, string>();
    /// <summary>
    /// 这个方法用于更新按键设置
    /// </summary>
    /// <param name="Key_Name">按键名称</param>
    /// <param name="Key_Code">按键</param>
    /// <returns>是否成功。若为“Success”则成功更改，否则为冲突的按键名称</returns>
    public string SetKey(string Key_Name, KeyCode Key_Code)
    {
        //如果Key_Code已经存在于字典中，则返回此Key_Code对应的键名
        if (Key_Settings.ContainsValue(Key_Code))
        {
            return Key_Settings_Reverse[Key_Code];
        }
        else
        {
            //将Key_Name对应的值更新为Key_Code
            Key_Settings[Key_Name] = Key_Code;
            //如果Key_Settings_Reverse中已经存在Key_Code，则更新其对应的键名
            if (Key_Settings_Reverse.ContainsKey(Key_Code))
            {
                Key_Settings_Reverse[Key_Code] = Key_Name;
            }
            //否则，添加新的键值对
            else
            {
                Key_Settings_Reverse.Add(Key_Code, Key_Name);
            }
        }
        return "Success";
    }
}
