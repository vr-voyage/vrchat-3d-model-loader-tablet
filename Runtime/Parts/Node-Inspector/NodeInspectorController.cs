
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class NodeInspectorController : UdonSharpBehaviour
{
    GameObject displaying = null;
    public void Display(GameObject go)
    {
        displaying = go;
        var components = GetComponentsInChildren<INodeInspectorComponent>(true);
        int nComponents = components.Length;
        for (int i = 0; i < nComponents; i++)
        {
            var component = components[i];
            if (component == null) continue;
            component.Display(go);
        }
    }

    public void RefreshIfDisplaying(GameObject go)
    {
        if (go == null) return;
        if (displaying == go || displaying == null)
        {
            Display(go);
        }
        else
        {
            string objectName = displaying != null ? displaying.name : "NULL";
            Debug.Log($"<color=orange>Not displaying that object. Current is {objectName} </color>");
        }
    }

    public void Clear()
    {
        var components = GetComponentsInChildren<INodeInspectorComponent>(true);
        int nComponents = components.Length;
        for (int i = 0; i < nComponents; i++)
        {
            var component = components[i];
            if (component == null) continue;
            component.Clear();
        }
        displaying = null;
    }

}
