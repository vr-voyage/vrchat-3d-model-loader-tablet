
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public abstract class INodeInspectorComponent : UdonSharpBehaviour
{
    public abstract void Display(GameObject go);
}
