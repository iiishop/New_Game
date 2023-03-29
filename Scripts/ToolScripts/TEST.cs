using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TEST : MonoBehaviour
{
    // Start is called before the first frame update
    
    public TextAsset textAsset;
    public bool start = false;

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            Dialog.Instance.SetDialog(textAsset); 
        } 
    }
}
