using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L插件 : MonoBehaviour
{
    public static L插件 instance;
    private void Awake()
    {
        instance = this;
    }
    public GameObject blood;
    public GameObject 范围att;
    public GameObject tip条;
}
