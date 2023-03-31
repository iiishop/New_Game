using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class TEST : MonoBehaviour
{
    // Start is called before the first frame update
    
    public TextAsset textAsset;
    public bool start = true;


    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            Dialog.Instance.SetDialog(textAsset); 
            start = false;
        } 
    }
}
