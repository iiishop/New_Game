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
    /// 这个类用于存储一些游戏内容中需要保存的数据，是一个中间类，用于将数据保存到本地
    /// </summary>
    class SaveData
    {
        public List<int> Player_Weapon_Inventory = new List<int>();
    }
    /// <summary>
    /// 这个类用于存储设置部分需要保存的数据，是一个中间类，用于将数据保存到本地
    /// </summary>
    class SettingData
    {
        public Dictionary<string, KeyCode> Key_Settings;
        public bool Is_Full_Screen;
        public float Global_Volume;
    }

    SaveData Save_Data = new SaveData();
    SettingData Setting_Data = new SettingData();

    public List<Weapon> Player_Weapon_Inventory = new List<Weapon>();
    public Dictionary<string, KeyCode> Key_Settings = new Dictionary<string, KeyCode>();
    public Dictionary<KeyCode, string> Key_Settings_Reverse = new Dictionary<KeyCode, string>();




    private void Start()
    {
        if (LoadDatas(ref Setting_Data, "Setting_Data") == 0)
        {
            //如果没有读取到数据，则初始化数据
            Key_Init();


            //将数据保存到本地
            SaveDatas(Setting_Data, "Setting_Data");
        }
    }


    /// <summary>
    /// 这个方法用于将需要保存的数据处理成可以转换成为json的形式
    /// </summary>
    private void ProcessDatas()
    {
        //处理Save_Data部分

        Save_Data.Player_Weapon_Inventory.Clear();
        DEBUG.Instance.Display_Info(Player_Weapon_Inventory.Count.ToString());
        foreach (var item in Player_Weapon_Inventory)
        {
            DEBUG.Instance.Display_Info(item.ID.ToString());
            if (item != null)
            {
                Save_Data.Player_Weapon_Inventory.Add(item.ID);
            }
        }

        //处理Setting_Data部分
        Setting_Data.Key_Settings = Key_Settings;
        Setting_Data.Is_Full_Screen = Screen.fullScreen;
        Setting_Data.Global_Volume = AudioListener.volume;
    }


    /// <summary>
    /// 这个方法用于将处理完的数据保存到本地
    /// </summary>
    /// <param name="Data">为需要保存的数据</param>
    public void SaveDatas<T>(T Data, string Name)
    {
        var path = "\\" + Name + ".json";
        //把所有数据都处理成可以转换成为json的形式，不一定会全部保存
        ProcessDatas();


        //将数据保存到本地
        string json = JsonConvert.SerializeObject(Data);
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
    /// <returns>返回1表示读取成功，返回0表示读取失败</returns>
    public int LoadDatas<T>(ref T Data, string Name)
    {
        var path = "\\" + Name + ".json";
        string fileUrl = Application.streamingAssetsPath + path;
        //如果存在，则读取数据
        if (File.Exists(fileUrl))
        {
            using (StreamReader sr = new StreamReader(fileUrl))
            {
                string json = sr.ReadToEnd();
                Data = JsonConvert.DeserializeObject<T>(json);
                sr.Close();
                sr.Dispose();
            }
            ApplyDatas();
            return 1;
        }
        else
        {
            //如果不存在，则返回
            return 0;
        }
    }

    /// <summary>
    /// 这个方法用于将数据应用到游戏中
    /// </summary>
    private void ApplyDatas()
    {
        //Save_Data部分
        Player_Weapon_Inventory.Clear();
        foreach (var item in Save_Data.Player_Weapon_Inventory)
        {
            Player_Weapon_Inventory.Add(WeaponManager.Instance.Weapon_Inventory[item]);
        }

        //Setting_Data部分
        Key_Settings = Setting_Data.Key_Settings;
        Key_Settings_Reverse.Clear();
        foreach (var item in Key_Settings)
        {
            Key_Settings_Reverse.Add(item.Value, item.Key);
        }
        Screen.fullScreen = Setting_Data.Is_Full_Screen;
        AudioListener.volume = Setting_Data.Global_Volume;

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
        Key_Settings.Add("Crouch", KeyCode.LeftControl);
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
        if (Key_Settings.ContainsValue(Key_Code) && Key_Settings_Reverse[Key_Code] != Key_Name)
        {
            return Key_Settings_Reverse[Key_Code];
        }
        else
        {
            Key_Settings[Key_Name] = Key_Code;
            if (Key_Settings_Reverse.ContainsKey(Key_Code))
            {
                Key_Settings_Reverse[Key_Code] = Key_Name;
            }
            else
            {
                Key_Settings_Reverse.Add(Key_Code, Key_Name);
            }
        }
        //SaveDatas(Setting_Data, "Setting_Data");
        return "Success";
    }
}
