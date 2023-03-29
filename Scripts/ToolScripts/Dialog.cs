using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
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
    //public Image Head_Picture;
    private string[] Dialogs;
    public int index = 0;

    public void SetDialog(TextAsset textFile)
    {
        Dialogs = textFile.text.Split('\n');
        Dialog_Box.SetActive(true);
        index = 0;
    }

    private void Start()
    {
        Dialog_Box.SetActive(false);
    }

    private void Update()
    {
        if (Dialog_Box.activeSelf)
        {
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