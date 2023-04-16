using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;
using System.IO;
using Newtonsoft.Json;

public class Program_Save : MonoBehaviour
{
    public static Program_Save Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    [System.Serializable]
    /// <summary>
    /// 这个类用于存储一些需要保存的数据，是一个中间类，用于将数据保存到本地
    /// </summary>
    class SaveData
    {
        public int[] Player_Weapon_Inventory;
        public Dictionary<string, KeyCode> Key_Settings;
    }

    SaveData Save_Data = new SaveData();

    public List<Weapon> Player_Weapon_Inventory = new List<Weapon>();
    public Dictionary<string, KeyCode> Key_Settings = new Dictionary<string, KeyCode>();
    public Dictionary<KeyCode, string> Key_Settings_Reverse = new Dictionary<KeyCode, string>();


    private void Start()
    {
        Key_Init();
        //LoadDatas();
        SaveDatas();
    }

    /// <summary>
    /// 这个方法用于将需要保存的数据处理成可以转换成为json的形式
    /// </summary>
    public void ProcessDatas()
    {
        //处理需要保存的数据
        Save_Data.Key_Settings = Key_Settings;
        Save_Data.Player_Weapon_Inventory = new int[Player_Weapon_Inventory.Count];
        int i = 0;
        foreach (var item in Player_Weapon_Inventory)
        {
            Save_Data.Player_Weapon_Inventory[i] = item.ID;
            i++;
        }
    }


    /// <summary>
    /// 这个方法用于将处理完的数据保存到本地
    /// </summary>
    /// <param name="path">为储存的文件名称，例如"\\Datas.json"表示存储到streamingAssetsPath/Datas.json中</param>
    public void SaveDatas(string path = "\\Datas.json")
    {
        ProcessDatas();


        //将数据保存到本地
        string json = JsonConvert.SerializeObject(Save_Data);
        string fileUrl = Application.streamingAssetsPath + path;
        using (StreamWriter sw = new StreamWriter(fileUrl))
        {
            //清空文档
            sw.Flush();
            //保存数据
            sw.WriteLine(json);
            //关闭文档
            sw.Close();
            sw.Dispose();
        }
    }
    /// <summary>
    /// 这个方法用于从本地加载数据，并将数据应用到游戏中
    /// </summary>
    /// <param name="path">为读取的文件名称，例如"\\Datas.json"表示从streamingAssetsPath/Datas.json读取</param>
    public void LoadDatas(string path = "\\Datas.json")
    {
        string fileUrl = Application.streamingAssetsPath + path;
        using (StreamReader sr = new StreamReader(fileUrl))
        {
            string json = sr.ReadToEnd();
            Save_Data = JsonConvert.DeserializeObject<SaveData>(json);
            sr.Close();
            sr.Dispose();
        }
        ApplyDatas();
    }

    /// <summary>
    /// 这个方法用于将数据应用到游戏中
    /// </summary>
    public void ApplyDatas()
    {
        Key_Settings = Save_Data.Key_Settings;
        foreach (var item in Save_Data.Player_Weapon_Inventory)
        {
            Player_Weapon_Inventory.Add(WeaponManager.Instance.Weapon_Inventory[item]);
        }
    }

    /// <summary>
    /// 这个方法用于初始化按键设置
    /// </summary>
    public void Key_Init()
    {
        Key_Settings.Add("Dash", KeyCode.LeftShift);
        Key_Settings.Add("Left", KeyCode.A);
        Key_Settings.Add("Right", KeyCode.D);
        Key_Settings.Add("Jump", KeyCode.Space);
        Key_Settings.Add("Aim", KeyCode.Mouse1);
        Key_Settings.Add("Shoot", KeyCode.Mouse0);
        Key_Settings.Add("Reload", KeyCode.R);
        Key_Settings.Add("WeaponChange", KeyCode.Q);
        Key_Settings.Add("Trigger", KeyCode.E);
        Key_Settings.Add("SpecialSkill", KeyCode.T);
        Key_Settings.Add("DialogNext1", KeyCode.Z);
        Key_Settings.Add("DialogNext2", KeyCode.Return);

        //将Key_Settings中的键值对反转，存储到Key_Settings_Reverse中
        foreach (var item in Key_Settings)
        {
            Key_Settings_Reverse.Add(item.Value, item.Key);
        }

    }

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
        SaveDatas();
        return "Success";
    }
}
