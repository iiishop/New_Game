using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using TMPro;
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
    public TextMeshProUGUI Dialog_Text;
    public Image Speaker_Picture;
    private string[] Dialogs;
    public int index = 0;
    string Path = "Dialog/";


    public void SetDialog(TextAsset textFile)
    {
        Dialogs = textFile.text.Split('\n');
        Dialog_Box.SetActive(true);
        index = 0;
        Program.Instance.Main_Character.GetComponent<PlayerController>().Can_Move = false;
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
            if (temp.Length > 1)
            {
                if (Speaker_Picture.sprite.name != temp[1])
                {
                    //删除对话中的图片信息
                    Dialogs[index] = Dialogs[index].Replace("<" + temp[1] + ">", "");
                    ChangeImage(temp[1]);
                }
            }

            //获取对话中的命令，命令被[]包围，并且都是数字
            var command = Dialogs[index].Split('[', ']');
            if (command.Length > 1)
            {
                for (int i = 1; i < command.Length; i++)
                {
                    //删除对话中的命令信息
                    Dialogs[index] = Dialogs[index].Replace("[" + command[i] + "]", "");
                    //根据命令来执行相应的操作
                    switch (command[i])
                    {
                        case "1":
                            //这里是命令1的操作
                            //命令1是让speaker_picture消失
                            Speaker_Picture.gameObject.SetActive(false);
                            break;
                        case "2":
                            //这里是命令2的操作
                            //命令2是让speaker_picture出现
                            Speaker_Picture.gameObject.SetActive(true);
                            break;
                        case "3":
                            //这里是命令3的操作
                            //命令3是让屏幕变黑，但是留下对话框的部分
                            //实现方式是让子物体的BlackScreen子物体active
                            Dialog_Box.transform.Find("BlackScreen").gameObject.SetActive(true);
                            break;
                        case "4":
                            //这里是命令4的操作
                            //命令4是让屏幕变亮
                            //实现方式是让子物体的BlackScreen子物体deactive
                            Dialog_Box.transform.Find("BlackScreen").gameObject.SetActive(false);
                            break;


                        default:
                            break;
                    }
                }
            }

            Dialog_Text.text = Dialogs[index];
            if (Input.GetKeyDown(Program.Instance.Program_Save_Object.Key_Settings["DialogNext1"]) || Input.GetKeyDown(Program.Instance.Program_Save_Object.Key_Settings["DialogNext2"]))
            {
                index++;
                if (index >= Dialogs.Length)
                {
                    Program.Instance.Main_Character.GetComponent<PlayerController>().Can_Move = true;
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