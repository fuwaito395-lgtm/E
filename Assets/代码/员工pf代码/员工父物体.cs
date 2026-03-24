using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 员工父物体 : MonoBehaviour
{
    public static 员工父物体 instance;
    private void Awake()
    {
        instance = this;
    }
  

    public List<员工perfer> allren = new List<员工perfer>();
    
}
