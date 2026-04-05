using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class 员工setatt : MonoBehaviour
{
    [SerializeField] private 员工perfer Datapfset;
    [SerializeField] private SpriteRenderer sr;

    public 武器SO 起始武器;
    public 武器实例 当前武器;

    public 怪物实例 targetl = null;
    public 武器攻击实例 当前攻击 = null;

    public 员工perfer OwnerData => Datapfset;

    private void Awake()
    {
        if (Datapfset == null)
            Datapfset = GetComponent<员工perfer>();

        if (sr == null)
            sr = GetComponent<SpriteRenderer>();
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
            Debug.LogError("[员工setatt.装备武器] 找不到员工perfer");
            return;
        }

        当前武器 = new 武器实例(so, Datapfset);
        当前攻击 = null;
    }

    public void attupdate()
    {
        if (Datapfset == null || Datapfset.Data == null) return;
        if (tct.Instance == null) return;
        if (tct.Instance.timec == 0) return;
        if (当前武器 == null) return;

        float dt = Time.deltaTime * tct.Instance.timec;

        // 只要当前攻击还在跑，就只推进它，不重新判定本次攻击
        if (当前攻击 != null && 当前攻击.IsRunning)
        {
            var next = 当前攻击.Tick(dt, this);

            if (next != null)
            {
                当前攻击 = next;
                return;
            }

            if (!当前攻击.IsRunning)
                当前攻击 = null;

            return;
        }

        刷新锁定目标();

        if (!目标有效(targetl))
        {
            清除目标();
            Datapfset.Data.currentState = 人数据列表.ren.state.idie;
            return;
        }

        // 跨房间：进入待机，不清目标
        if (targetl.lnowroom != Datapfset.Data.CurrentRoomId)
        {
            清除目标();
            Datapfset.Data.currentState = 人数据列表.ren.state.idie;
            return;
        }

        Datapfset.Data.currentState = 人数据列表.ren.state.att;

        if (targetl.关联对象 == null)
        {
            清除目标();
            Datapfset.Data.currentState = 人数据列表.ren.state.idie;
            return;
        }

        朝向目标();

        var atk = 当前武器.选择攻击();
        if (atk == null) return;

        if (!atk.CanStart(this, targetl))
            return;

        if (!atk.IsTargetInRange(this, targetl))
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.MoveTowards(
                pos.x,
                targetl.关联对象.transform.position.x,
                Datapfset.Data.movesp * dt
            );
            transform.position = pos;
            return;
        }

        当前攻击 = atk;
        当前攻击.StartAttack(this, targetl);

        // 前摇为 0 的攻击，直接推进一帧
        var nextAtk = 当前攻击.Tick(dt, this);
        if (nextAtk != null)
            当前攻击 = nextAtk;
    }

    private void 刷新锁定目标()
    {
        if (目标有效(targetl))
        {
            Datapfset.Data.ltarget = targetl;
            return;
        }

        if (目标有效(Datapfset.Data.ltarget))
        {
            targetl = Datapfset.Data.ltarget;
            return;
        }

        targetl = null;
    }

    private bool 目标有效(怪物实例 m)
    {
        if (m == null) return false;
        if (m.关联对象 == null) return false;
        if (m.nowhpl <= 0) return false;
        return true;
    }

    private void 清除目标()
    {
        targetl = null;
        if (Datapfset != null && Datapfset.Data != null)
            Datapfset.Data.ltarget = null;
    }

    private void 朝向目标()
    {
        if (sr == null || targetl == null || targetl.关联对象 == null) return;

        if (transform.position.x > targetl.关联对象.transform.position.x)
            sr.flipX = false;
        else
            sr.flipX = true;
    }
}