
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class TestFilamentedController : UdonSharpBehaviour
{
    public NodeInspectorMaterialController_Filamented filamentedController;
    public Material filamentedMaterial;
    void Start()
    {
        filamentedController.DisplayMaterial(filamentedMaterial);
    }
}
