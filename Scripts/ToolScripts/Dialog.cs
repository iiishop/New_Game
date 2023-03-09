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
    public GameObject dialogBox;
    public Text dialogText;
    public Image headPicture;
    public string[] dialogs;
    public int index = 0;
    
    public void SetDialog(TextAsset textFile)
    {
        dialogs=textFile.text.Split('\n');
        dialogBox.SetActive(true);
        index = 0;
    }

    private void Start()
    {
        dialogBox.SetActive(false);
    }

    private void Update()
    {
        if (dialogBox.activeSelf)
        {
            dialogText.text = dialogs[index];
            if (Input.GetKeyDown(KeyCode.Space)||Input.GetMouseButtonDown(0))
            {
                index++;
                if (index >= dialogs.Length)
                {
                    dialogBox.SetActive(false);
                }
            }
        }
    }

    private void Awake()
    {
        if(Instance!=null && Instance!=this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}
