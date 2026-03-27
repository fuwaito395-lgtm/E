using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 条 : MonoBehaviour
{
    public static 条 instance;
    SpriteRenderer sr;
    private float x=1;
    float spriteWidthLocal;
    float leftLocalX;
    Vector3 originalLocalScale;
    private void Awake()
    {
        instance = this;
        sr = GetComponent<SpriteRenderer>();
    }
    public void whenworkdone()
    {
        x = this.transform.localScale.x+1;
        if (x == 10)
        {
            收容系统.Instance.条满();
        }
        else
            Debug.Log("inc");
        {
            this.transform.localScale = new Vector3(x, 1, 1);
        }
        
        
    }
}
