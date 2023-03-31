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
        if(Input.GetKeyDown(KeyCode.E)&&In_Trigger)
        {
            if (!Is_Triggered)
            {
                Dialog.Instance.SetDialog(Dialog_Text);
            }
            Is_Triggered = true;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        In_Trigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
       In_Trigger = false; 
    }
}
