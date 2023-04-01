using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DEBUG : MonoBehaviour
{
    public static DEBUG Instance;

    private TextMeshProUGUI TMP;
    //单例模式
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Display_Info(string Info)
    {
        //换行显示内容
        TMP.text += Info + "\n";
    }
    private void Start()
    {
        TMP = transform.Find("DEBUG_INFO").GetComponent<TextMeshProUGUI>();
        TMP.gameObject.SetActive(false);
    }

    private void Update()
    {
       //按F11来切换显示
        if (Input.GetKeyDown(KeyCode.F11))
        {
            TMP.gameObject.SetActive(!TMP.gameObject.activeSelf);
        } 
    }
}
