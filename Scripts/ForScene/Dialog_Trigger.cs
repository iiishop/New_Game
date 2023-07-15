using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog_Trigger : MonoBehaviour
{
    private bool Is_Triggered;
    public TextAsset Dialog_Text;
    private bool In_Trigger;

    private void Update()
    {
        if (Input.GetKeyDown(Program.Instance.Program_Save_Object.Key_Settings["Trigger"]) && In_Trigger)
        {
            if (!Is_Triggered)
            {
                Dialog.Instance.SetDialog(Dialog_Text);
            }
            Is_Triggered = true;
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        In_Trigger = true;
        if (!Is_Triggered)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        In_Trigger = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
