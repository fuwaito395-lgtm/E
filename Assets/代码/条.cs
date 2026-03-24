using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 条 : MonoBehaviour
{
   
    public static 条 instance;
    private float x=0;
    private void Awake()
    {
        
        instance = this;
    }
    public void whenworkdone()
    {
        x = this.transform.localScale.x+1;
        if (x == 2)
        {
            收容系统.Instance.条满();
        }
        else
        {
            this.transform.localScale = new Vector3(x, 1, 1);
        }
        
        
    }
}
