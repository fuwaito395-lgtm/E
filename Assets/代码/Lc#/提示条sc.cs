using TMPro;
using UnityEngine;

public class 怪物攻击提示物体 : MonoBehaviour
{
    [Header("SpriteRenderer")]
    public SpriteRenderer 背景;
    public SpriteRenderer 进度条;

    [Header("文字")]
    public TextMeshPro 攻击文字;

    [Header("大招抖动参数")]
    public float 最大旋转角度 = 12f;
    public float 最大位移 = 0.06f;
    public float 抖动频率 = 10f;

    private Vector3 进度条初始缩放;

    private bool 正在抖动;
    private bool 需要重建缓存 = true;

    private Vector3[][] 原始顶点缓存;
    private float[] 字符种子;

    private void Awake()
    {
        if (进度条 != null)
        {
            进度条初始缩放 = 进度条.transform.localScale;
        }

        gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (!正在抖动) return;
        if (攻击文字 == null || !攻击文字.gameObject.activeInHierarchy) return;

        更新字符抖动();
    }

    public void 显示(string 攻击名, bool 是大招)
    {
        gameObject.SetActive(true);

        if (攻击文字 != null)
        {
            攻击文字.text = 攻击名;
            攻击文字.color = 是大招 ? Color.red : Color.white;
            攻击文字.ForceMeshUpdate();

            需要重建缓存 = true;
            正在抖动 = 是大招;
            if (!是大招)
            {
                恢复文字原状();
            }
        }

        if (进度条 != null)
        {
            var s = 进度条初始缩放;
            s.x = Mathf.Max(0.001f, 进度条初始缩放.x * 0.001f);
            进度条.transform.localScale = s;
        }

        if (!是大招)
        {
            正在抖动 = false;
        }
    }

    public void 设置进度(float p)
    {
        if (进度条 == null) return;

        p = Mathf.Clamp01(p);

        var s = 进度条初始缩放;
        s.x = Mathf.Max(0.001f, 进度条初始缩放.x * p);
        进度条.transform.localScale = s;
    }

    public void 隐藏()
    {
        正在抖动 = false;
        恢复文字原状();
        gameObject.SetActive(false);
    }

    private void 更新字符抖动()
    {
        if (攻击文字 == null) return;

        if (需要重建缓存 || 原始顶点缓存 == null)
        {
            重建缓存();
        }

        if (原始顶点缓存 == null) return;

        var textInfo = 攻击文字.textInfo;
        int meshCount = textInfo.meshInfo.Length;

        for (int i = 0; i < meshCount; i++)
        {
            if (textInfo.meshInfo[i].mesh == null) continue;
            if (原始顶点缓存[i] == null) continue;

            var verts = textInfo.meshInfo[i].vertices;
            var src = 原始顶点缓存[i];

            int len = Mathf.Min(verts.Length, src.Length);
            for (int v = 0; v < len; v++)
            {
                verts[v] = src[v];
            }
        }

        int characterCount = textInfo.characterCount;

        for (int i = 0; i < characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            int matIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;

            var verts = textInfo.meshInfo[matIndex].vertices;
            if (verts == null || verts.Length < vertexIndex + 4) continue;

            float seed = 字符种子[i];
            float t = Time.unscaledTime * 抖动频率;

            float noiseX = (Mathf.PerlinNoise(seed, t) - 0.5f) * 2f;
            float noiseY = (Mathf.PerlinNoise(seed + 37.2f, t) - 0.5f) * 2f;
            float noiseA = (Mathf.PerlinNoise(seed + 91.7f, t) - 0.5f) * 2f;

            Vector3 offset = new Vector3(
                noiseX * 最大位移,
                noiseY * 最大位移 * 0.35f,
                0f
            );

            float angle = noiseA * 最大旋转角度;

            Vector3 center = (verts[vertexIndex + 0] + verts[vertexIndex + 2]) / 2f;
            Matrix4x4 matrix = Matrix4x4.TRS(offset, Quaternion.Euler(0f, 0f, angle), Vector3.one);

            for (int j = 0; j < 4; j++)
            {
                Vector3 local = verts[vertexIndex + j] - center;
                verts[vertexIndex + j] = matrix.MultiplyPoint3x4(local) + center;
            }

            textInfo.meshInfo[matIndex].mesh.vertices = verts;
            攻击文字.UpdateGeometry(textInfo.meshInfo[matIndex].mesh, matIndex);
        }
    }

    private void 重建缓存()
    {
        if (攻击文字 == null) return;

        攻击文字.ForceMeshUpdate();
        var textInfo = 攻击文字.textInfo;

        if (textInfo == null || textInfo.meshInfo == null)
        {
            原始顶点缓存 = null;
            字符种子 = null;
            需要重建缓存 = false;
            return;
        }

        原始顶点缓存 = new Vector3[textInfo.meshInfo.Length][];
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            var meshInfo = textInfo.meshInfo[i];
            if (meshInfo.vertices == null)
            {
                原始顶点缓存[i] = null;
                continue;
            }

            原始顶点缓存[i] = new Vector3[meshInfo.vertices.Length];
            System.Array.Copy(meshInfo.vertices, 原始顶点缓存[i], meshInfo.vertices.Length);
        }

        字符种子 = new float[textInfo.characterCount];
        for (int i = 0; i < 字符种子.Length; i++)
        {
            字符种子[i] = Random.Range(0f, 1000f);
        }

        需要重建缓存 = false;
    }

    private void 恢复文字原状()
    {
        if (攻击文字 == null) return;

        if (原始顶点缓存 == null) return;

        var textInfo = 攻击文字.textInfo;
        if (textInfo == null || textInfo.meshInfo == null) return;

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            if (原始顶点缓存[i] == null) continue;
            if (textInfo.meshInfo[i].vertices == null) continue;

            var verts = textInfo.meshInfo[i].vertices;
            var src = 原始顶点缓存[i];

            int len = Mathf.Min(verts.Length, src.Length);
            for (int v = 0; v < len; v++)
            {
                verts[v] = src[v];
            }

            textInfo.meshInfo[i].mesh.vertices = verts;
            攻击文字.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
}