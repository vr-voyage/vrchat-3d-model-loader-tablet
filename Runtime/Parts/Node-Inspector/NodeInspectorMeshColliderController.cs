
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class NodeInspectorMeshColliderController : INodeInspectorComponent
{
    public Toggle meshColliderCheck;

    public override void Display(GameObject go)
    {
        
        if (go == null) { gameObject.SetActive(false); return; }
        MeshCollider meshCollider = go.GetComponent<MeshCollider>();
        if (meshCollider == null) { gameObject.SetActive(false); return; }
        meshColliderCheck.isOn = meshCollider.enabled;
        gameObject.SetActive(true);
    }

    public override void Clear()
    {
        meshColliderCheck.isOn = false;
        gameObject.SetActive(false);
    }
}
