using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 出逃父物体 : MonoBehaviour
{
    public static 出逃父物体 instance;
    private void Awake()
    {
        instance = this;
    }
}
