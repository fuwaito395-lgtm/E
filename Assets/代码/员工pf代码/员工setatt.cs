using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class 员工setatt : MonoBehaviour
{
    [SerializeField] private 员工perfer Datapfset;
    [SerializeField] private SpriteRenderer sr;
    public bool attcd = true;

    public 武器SO 起始武器;
    public 武器实例 当前武器;

    public 怪物实例 targetl = null;
    public float timer = 0f;

    private void Awake()
    {
        if (Datapfset == null)
            Datapfset = GetComponent<员工perfer>();
        
        if (sr == null)
            sr = GetComponent<SpriteRenderer>();

        Debug.Log($"[员工setatt.Awake] Datapfset={(Datapfset == null ? "null" : "ok")} , sr={(sr == null ? "null" : "ok")}");
    }

    private void Start()
    {
        if (Datapfset == null)
            Datapfset = GetComponent<员工perfer>();

        if (sr == null)
            sr = GetComponent<SpriteRenderer>();

        if (Datapfset == null || Datapfset.Data == null)
        {
            Debug.LogError("员工数据还没初始化");
            return;
        }

        起始武器 = 武器库.Instance != null
            ? 武器库.Instance.根据Id取武器(Datapfset.Data.egoatt)
            : null;

        if (起始武器 == null)
        {
            Debug.LogError($"没有找到武器，egoatt = {Datapfset.Data.egoatt}");
            return;
        }

        装备武器(起始武器);
    }

    
       
    

    public void 装备武器(武器SO so)
    {
        if (so == null)
        {
            Debug.LogError("[员工setatt.装备武器] so == null");
            return;
        }

        if (Datapfset == null)
            Datapfset = GetComponent<员工perfer>();

        if (Datapfset == null)
        {
            Debug.LogError("[员工setatt.装备武器] 找不到员工perfer，说明这脚本不在员工身上，或组件没挂对");
            return;
        }

        当前武器 = new 武器实例(so, Datapfset);
        timer = 0f;

        Debug.Log($"[员工setatt.装备武器] 已装备武器：{so.显示名}");
    }

    public void attupdate()
    {
        if (Datapfset == null)
        {
            Debug.LogError("[attupdate] Datapfset == null");
            return;
        }

        if (Datapfset.Data == null)
        {
            Debug.LogError("[attupdate] Datapfset.Data == null，员工数据没有初始化");
            return;
        }

        if (tct.Instance == null)
        {
            Debug.LogError("[attupdate] tct.Instance == null");
            return;
        }

        if (tct.Instance.timec == 0)
        {
            return;
        }

        if (当前武器 == null)
        {
            Debug.LogError("[attupdate] 当前武器 == null，说明武器没有装备成功");
            return;
        }

        if (当前武器.数据 == null)
        {
            Debug.LogError("[attupdate] 当前武器.数据 == null");
            return;
        }

        if (timer > 0f)
        {
            timer -= Time.deltaTime * tct.Instance.timec;
            return;
        }

       
        targetl = Datapfset.Data.ltarget;
        

        if (targetl == null)
        {
            Datapfset.Data.currentState = 人数据列表.ren.state.idie;
            return;
        }

        if (targetl.关联对象 == null)
        {
            Debug.LogError("[attupdate] targetl.关联对象 == null");
            
            return;
        }

        if (targetl.nowhpl <= 0)
        {
            targetl = null;
            Datapfset.Data.ltarget = null;
            return;
        }

        if (Datapfset.Data.CurrentRoomId != targetl.lnowroom)
        {
            Datapfset.Data.currentState = 人数据列表.ren.state.idie;
        }
        //Debug.Log("di");
        if(transform.position.x > targetl.关联对象.transform.position.x)
        {
            Datapfset.sr.flipX = false;
        }else
        {
            Datapfset.sr.flipX = true;
        }

        float 距离 = Mathf.Abs(transform.position.x - targetl.关联对象.transform.position.x);
        if (距离 > 当前武器.数据.攻击范围)
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.MoveTowards(pos.x, targetl.关联对象.transform.position.x, Datapfset.Data.movesp * tct.Instance.timec * Time.deltaTime);//水平移动
            transform.position = pos;
            return;
        }
        if (attcd == true)
        {
            attcd = false;
            timer =当前武器.数据.攻击间隔;
            return;
        }
        //Debug.Log("cdi");
        开始攻击();
    }

    private void 开始攻击()
    {
        if (当前武器 == null || 当前武器.数据 == null) return;

        attcd = true;
        当前武器.OnAttackStart();

        int hitCount = 0;
        //Debug.Log("stat");
        if (当前武器.数据.是否范围伤害)
        {
            hitCount = 范围攻击();
        }
        else
        {
            hitCount = 单体攻击(targetl) ? 1 : 0;
        }

        当前武器.OnAttackEnd(hitCount);
    }

    private int 范围攻击()
    {
        int hitCount = 0;
        if (Datapfset.Data == null) return 0;

        float 范围 = 当前武器.数据.范围半径;
        int roomId = Datapfset.Data.CurrentRoomId;

        if (Datapfset.Data.linroom != null)
        {
            foreach (var m in Datapfset.Data.linroom)
            {
                if (m == null || m.关联对象 == null || m.nowhpl <= 0) continue;
                if (m.lnowroom != roomId) continue;

                float dx = Mathf.Abs(transform.position.x - m.关联对象.transform.position.x);
                if (dx > 范围) continue;

                if (攻击怪物(m))
                    hitCount++;
            }
        }

        if (当前武器.数据.是否误伤友方 && 人数据列表.intance != null)
        {
            var roomEmployees = Datapfset.当前房间.当前员工;
            foreach (var r in roomEmployees)
            {
                if (r == null || r.Data.关联对象 == null) continue;
                if (r == Datapfset) continue;

                float dx = Mathf.Abs(transform.position.x - r.Data.关联对象.transform.position.x);
                if (dx > 范围) continue;

                if (攻击员工(r.Data))
                    hitCount++;
            }
        }

        return hitCount;
    }

    private bool 单体攻击(怪物实例 m)
    {
        if (m == null || m.关联对象 == null) return false;
        if (m.nowhpl <= 0) return false;
        if (m.lnowroom != Datapfset.Data.CurrentRoomId) return false;
        //Debug.Log("da");
        float 距离 = Mathf.Abs(transform.position.x - m.关联对象.transform.position.x);
        if (距离 > 当前武器.数据.实际攻击范围) return false;

        return 攻击怪物(m);
    }

    private bool 攻击怪物(怪物实例 m)
    {
        float damage = 当前武器.计算最终伤害(当前武器.数据.基础伤害);
        //Debug.Log("calda");
        m.calfinaldamega(
            damage,
            当前武器.数据.伤害类型,
            Datapfset,
            m,
            null
        );

        当前武器.通知命中(m.关联对象, damage, false);
        return true;
    }

    private bool 攻击员工(人数据列表.ren r)
    {
        if (r == null || r.关联对象 == null) return false;
        if (r.hp <= 0) return false;

        float damage = 当前武器.计算最终伤害(当前武器.数据.基础伤害);
        r.hp = Mathf.Max(0, r.hp - Mathf.CeilToInt(damage));

        if (r.hp <= 0)
        {
            r.currentState = 人数据列表.ren.state.die;
        }

        当前武器.通知命中(r.关联对象, damage, true);
        return true;
    }
}