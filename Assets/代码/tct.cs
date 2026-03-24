using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tct : MonoBehaviour
{
    public float timec;
    public static tct Instance;
    private void Awake()
    {
        timec = 1;
        Instance = this;
    }
    public void changetct0()
    {
        timec = 0;
    }
    public void changetct1()
    {
        timec = 1;
    }
    public void changetct15()
    {
        timec = 1.5f;
    }
    public void changetct2()
    {
        timec = 2;
    }
}
