
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class NodeInspectorController : UdonSharpBehaviour
{
    public void Display(GameObject go)
    {
        var components = GetComponentsInChildren<INodeInspectorComponent>(true);
        int nComponents = components.Length;
        for (int i = 0; i < nComponents; i++)
        {
            var component = components[i];
            if (component == null) continue;
            component.Display(go);
        }
    }

}
