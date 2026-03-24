using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 通用攻击子弹 : MonoBehaviour
{
    [Header("运行时参数")]
    public float 伤害值;
    public class_damage.damegatype Damage;
    public int 结算次数;
    public float 间隔;

    public SpriteRenderer nowsprite;
    public Sprite[] spriteatt;
    public LayerMask 目标层;

    private Collider2D col;
    private ContactFilter2D filter;
    private Collider2D[] buffer = new Collider2D[32];

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        nowsprite = GetComponent<SpriteRenderer>();

        filter = new ContactFilter2D();
        filter.useLayerMask = true;
        filter.useTriggers = true;
    }

    public void 初始化(
        float dmg,
        class_damage.damegatype _Damage,
        怪物实例 movedata,
        int 次数,
        float interval,
        LayerMask layer,
        Sprite[] _spriteatt,
        float x = 1f,
        float y = 1f
        
    )
    {
        伤害值 = dmg;
        结算次数 = 次数;
        间隔 = interval;
        目标层 = layer;
        Damage = _Damage;
        spriteatt = _spriteatt;

        filter.SetLayerMask(目标层);

        BoxCollider2D box = GetComponent<BoxCollider2D>();
        if (box != null)
        {
            box.size = new Vector2(x, y);
        }
       

        var flip = movedata.关联对象.GetComponent<SpriteRenderer>();
        if(flip.flipX==true)
        {
            nowsprite.flipX = true;
            Vector3 pos = transform.position;
            pos.x -= x / 2f + 1f;
            transform.position = pos;
        }else
        {
            nowsprite.flipX = false;
            Vector3 pos = transform.position;
            pos.x += x / 2f + 1f;
            transform.position = pos;
        }
            StartCoroutine(伤害循环());
    }

    private IEnumerator 伤害循环()
    {
        for (int i = 0; i < 结算次数; i++)
        {
            nowsprite.sprite = spriteatt[i];
            if (i < 结算次数 )
            {
                yield return 等待可变时间(间隔);
            }
            扫描并伤害();
        }

        Destroy(gameObject);
    }
    private IEnumerator 等待可变时间(float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (tct.Instance != null && tct.Instance.timec > 0f)
            {
                elapsed += Time.unscaledDeltaTime * tct.Instance.timec;
            }

            yield return null;
        }
    }
    private void 扫描并伤害()
    {
        int count = col.OverlapCollider(filter, buffer);
        HashSet<员工perfer> set = new HashSet<员工perfer>();

        for (int i = 0; i < count; i++)
        {
            var c = buffer[i];
            if (c == null) continue;
            if (!c.CompareTag("Employee")) continue;

            var emp = c.GetComponentInParent<员工perfer>();
            if (emp == null) continue;

            if (set.Add(emp))
            {
                emp.calfinaldamega(伤害值, Damage, emp, null, null);
            }
        }
    }
}