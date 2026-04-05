#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class 空引用扫描器
{
    [MenuItem("Tools/扫描场景空引用")]
    public static void 扫描场景空引用()
    {
        int count = 0;

        var monos = Object.FindObjectsByType<MonoBehaviour>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        foreach (var mono in monos)
        {
            if (mono == null) continue;

            ScanSerializedObject(mono, ref count);
        }

        Debug.Log($"[扫描完成] 场景空引用数量: {count}");
    }

    [MenuItem("Tools/扫描选中物体空引用")]
    public static void 扫描选中物体空引用()
    {
        var go = Selection.activeGameObject;
        if (go == null)
        {
            Debug.LogWarning("请先选中一个 GameObject");
            return;
        }

        int count = 0;
        var monos = go.GetComponentsInChildren<MonoBehaviour>(true);

        foreach (var mono in monos)
        {
            if (mono == null) continue;
            ScanSerializedObject(mono, ref count);
        }

        Debug.Log($"[扫描完成] 选中物体空引用数量: {count}");
    }

    [MenuItem("Tools/扫描Prefab空引用")]
    public static void 扫描Prefab空引用()
    {
        int count = 0;
        string[] guids = AssetDatabase.FindAssets("t:Prefab");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) continue;

            var monos = prefab.GetComponentsInChildren<MonoBehaviour>(true);
            foreach (var mono in monos)
            {
                if (mono == null)
                {
                    Debug.LogError($"[Prefab缺失脚本] {path}", prefab);
                    count++;
                    continue;
                }

                ScanSerializedObject(mono, ref count, path);
            }
        }

        Debug.Log($"[扫描完成] Prefab 空引用数量: {count}");
    }

    private static void ScanSerializedObject(Object target, ref int count, string extraPath = null)
    {
        SerializedObject so;

        try
        {
            so = new SerializedObject(target);
        }
        catch
        {
            return;
        }

        SerializedProperty prop = so.GetIterator();
        bool enterChildren = true;

        while (prop.NextVisible(enterChildren))
        {
            enterChildren = true;

            if (prop.propertyType == SerializedPropertyType.ObjectReference)
            {
                if (prop.objectReferenceValue == null)
                {
                    string path = BuildPath(target);
                    if (!string.IsNullOrEmpty(extraPath))
                        path = extraPath + " | " + path;

                    Debug.LogError($"[空引用] {path} -> {prop.propertyPath}", target);
                    count++;
                }
            }
        }
    }

    private static string BuildPath(Object target)
    {
        if (target is Component c)
            return GetFullPath(c.gameObject);

        if (target is GameObject go)
            return GetFullPath(go);

        return target.name;
    }

    private static string GetFullPath(GameObject obj)
    {
        if (obj == null) return "(null)";

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
#endif