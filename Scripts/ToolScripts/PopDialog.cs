using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopDialog : MonoBehaviour
{
    public static PopDialog Instance;
    
    
    
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
