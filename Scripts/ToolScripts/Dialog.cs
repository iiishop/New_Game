using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    /***
     * this is the dialog class
     * it is used to display text on the screen
     * 使用方法：
     * 在需要使用的时候调用Dialog.Instance.setDialog(自定义的对话框内容)
     */
    public static Dialog Instance;
    public GameObject Dialog_Box;
    public Text Dialog_Text;
    public Image Speaker_Picture;
    private string[] Dialogs;
    public int index = 0;
    string Path = "Dialog/";

    public void SetDialog(TextAsset textFile)
    {
        Dialogs = textFile.text.Split('\n');
        Dialog_Box.SetActive(true);
        index = 0;
    }

    private void ChangeImage(string Name)
    {
        //Speaker_Picture是一个Image组件，用来显示图片，这里需要改变Speaker_Picture的源图片
        Speaker_Picture.sprite = Resources.Load<Sprite>(Path + Name) as Sprite;
    }

    private void Start()
    {
        Dialog_Box.SetActive(false);
    }

    private void Update()
    {
        if (Dialog_Box.activeSelf)
        {
            //这里需要根据对话内容来改变图片
            //通过检测Dialogs[index]中<>中的内容来改变图片
            //比如<张三/1.png>，那么就需要改变图片为Assets/Dialog/下的张三文件夹中的1.png文件
            var temp = Dialogs[index].Split('<', '>');
            //当temp[1]不为空时，并且当前的图片不是temp[1]时，才需要改变图片
            
            if (temp.Length > 1 )
            {
                if (Speaker_Picture.sprite.name != temp[1])
                {
                    //删除对话中的图片信息
                    Dialogs[index] = Dialogs[index].Replace("<" + temp[1] + ">", "");
                    ChangeImage(temp[1]);
                }
            }
            Dialog_Text.text = Dialogs[index];
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                index++;
                if (index >= Dialogs.Length)
                {
                    Dialog_Box.SetActive(false);
                }
            }
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}