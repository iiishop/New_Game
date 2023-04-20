using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DEBUG : MonoBehaviour
{
    public static DEBUG Instance;
    public GameObject Debug_Panel;

    public TMP_InputField TMP;
    private TextMeshProUGUI Text;
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
    public void Display_Info(string Info)
    {
        Debug.Log(Info);
        //换行显示内容
        TMP.text += Info + "\n";

    }
    private void Start()
    {
        Text = TMP.GetComponent<TextMeshProUGUI>();
        Debug_Panel.SetActive(false);
    }

    private void Update()
    {
        //按F11来切换显示
        if (Input.GetKeyDown(KeyCode.F11))
        {
            Debug_Panel.SetActive(!Debug_Panel.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.F12))
        {
            Display_Info("F12");
        }
    }
}
