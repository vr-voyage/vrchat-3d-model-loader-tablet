
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class NodeInspectorTransformController : INodeInspectorComponent
{
    public TMPro.TMP_Text[] position;
    public TMPro.TMP_Text[] rotation;
    public TMPro.TMP_Text[] scale;

    void DisplayVector3(Vector3 v, TMPro.TMP_Text[] output)
    {
        if (output[0] != null) output[0].text = v[0].ToString("F3");
        if (output[1] != null) output[1].text = v[1].ToString("F3");
        if (output[2] != null) output[2].text = v[2].ToString("F3");
    }

    public override void Display(GameObject go)
    {
        Transform t = go.transform;
        DisplayVector3(t.localPosition, position);
        DisplayVector3(t.localEulerAngles, rotation);
        DisplayVector3(t.localScale, scale);
    }
}
