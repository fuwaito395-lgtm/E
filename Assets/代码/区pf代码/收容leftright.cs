using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 收容leftright : MonoBehaviour
{
    public float LeftX;
    public float RightX;
    public float objecty;

    private void Awake()
    {
        var col = GetComponent<Collider2D>();
        if (col != null)
        {
            Bounds b = col.bounds;
            LeftX = b.min.x;
            RightX = b.max.x;
            objecty = b.min.y;
        }
        else
        {
            var sr = GetComponent<SpriteRenderer>();
            Bounds b = sr.bounds;
            LeftX = b.min.x;
            RightX = b.max.x;
            objecty = b.min.y;
        }
        var num = GetComponent<收容所button>();


        MapSystem.Instance.giveleftright(LeftX, RightX, objecty + 2, num.RoomId,this.gameObject);
    }
}
