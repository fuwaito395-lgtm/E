using System.Collections.Generic;
using UnityEngine;

public class 武器库 : MonoBehaviour
{
    public static 武器库 Instance;

    public List<武器SO> 所有武器 = new List<武器SO>();

    private Dictionary<int, 武器SO> id字典 = new Dictionary<int, 武器SO>();

    private void Awake()
    {
        Instance = this;
        id字典.Clear();

        foreach (var w in 所有武器)
        {
            if (w == null) continue;
            if (!id字典.ContainsKey(w.武器Id))
                id字典.Add(w.武器Id, w);
        }
    }

    public 武器SO 根据Id取武器(int id)
    {
        if (id字典.TryGetValue(id, out var so))
            return so;

        return null;
    }
}