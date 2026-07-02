using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class ExtensionsHelpers
{
    public static bool Contains<T>(this T[] arr, T value)
    {
        if (arr == null) return false;
        int nValues = arr.Length;
        for (int i = 0; i < nValues; i++)
        {
            T currentValue = arr[i];
            if (!currentValue.Equals(value)) continue;
            return true;
        }
        return false;
    }

    public static void ClearChildren(this Transform t)
    {
        int nChildren = t.childCount;
        for (int i = nChildren - 1; i >= 0; i--)
        {
            Transform childT = t.GetChild(i);
            if (childT == null) continue;
            GameObject go = childT.gameObject;
            if (go == null) continue;
            UnityEngine.Object.Destroy(go);
        }
    }
}
