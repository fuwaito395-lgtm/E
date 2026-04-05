using System.Reflection;
using UnityEditor;
using UnityEngine;
public class 查Missing : MonoBehaviour
{
    [ContextMenu("扫描场景 Missing Script")]
    void Scan()
    {
        GameObject[] all = FindObjectsOfType<GameObject>(true);

        int count = 0;

        foreach (var go in all)
        {
            var comps = go.GetComponents<Component>();

            for (int i = 0; i < comps.Length; i++)
            {
                if (comps[i] == null)
                {
                    Debug.LogError($"❌ Missing Script 在物体: {GetFullPath(go)}", go);
                    count++;
                }
            }
        }

        Debug.Log($"扫描完成，共发现 {count} 个 Missing Script");
    }

    string GetFullPath(GameObject obj)
    {
        string path = obj.name;
        Transform t = obj.transform;

        while (t.parent != null)
        {
            t = t.parent;
            path = t.name + "/" + path;
        }

        return path;
    }
}
public class 查PrefabMissing
{
    [MenuItem("Tools/扫描所有Prefab Missing")]
    static void ScanPrefabs()
    {
        string[] guids = AssetDatabase.FindAssets("t:Prefab");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            var comps = prefab.GetComponentsInChildren<Component>(true);

            foreach (var c in comps)
            {
                if (c == null)
                {
                    Debug.LogError($"❌ Prefab 有 Missing: {path}");
                    break;
                }
            }
        }

        Debug.Log("Prefab 扫描完成");
    }
}

public class 查空引用 : MonoBehaviour
{
    [ContextMenu("扫描空引用字段")]
    void Scan()
    {
        var all = FindObjectsOfType<MonoBehaviour>(true);

        foreach (var mono in all)
        {
            var fields = mono.GetType().GetFields(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var f in fields)
            {
                if (f.FieldType.IsSubclassOf(typeof(Object)))
                {
                    var value = f.GetValue(mono);

                    if (value == null)
                    {
                        Debug.LogWarning($"⚠️ 空引用: {mono.name} → {f.Name}", mono);
                    }
                }
            }
        }
    }
}