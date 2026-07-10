
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class NodeInspectorMaterialsController : INodeInspectorComponent
{
    public GameObject filamentedPrefab;
    public GameObject unknownPrefab;

    public RectTransform listParent;
    public override void Clear()
    {
        Transform t = listParent;
        int childCount = t.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = t.GetChild(i);
            if (child == null) continue;
            GameObject childGo = child.gameObject;
            if (childGo == null) continue;
            Destroy(childGo);
        }
    }

    GameObject GetPrefabForShader(string shaderName)
    {
        switch (shaderName)
        {
            case "GLBLoader/Silent/Filamented":
                return filamentedPrefab;
        }

        return unknownPrefab;
    }

    void DisplayMaterial(Material m)
    {
        if (m.shader == null) return;
        string shaderName = m.shader.name;
        GameObject prefab = GetPrefabForShader(shaderName);
        Transform t = listParent.transform;
        GameObject instantiatedMaterial = Instantiate(prefab, t);
        instantiatedMaterial.name = m.name;
        var controller = instantiatedMaterial.GetComponent<INodeInspectorMaterialController>();
        if (controller == null)
        {
            Destroy(instantiatedMaterial);
            return;
        }
        controller.DisplayMaterial(m);
    }

    void DisplayMaterials(Material[] materials)
    {
        int nMaterials = materials.Length;
        for (int m = 0; m < nMaterials; m++)
        {
            Material mat = materials[m];
            if (mat == null) continue;
            DisplayMaterial(mat);
        }
    }

    public override void Display(GameObject go)
    {
        Clear();
        var meshRenderer = go.GetComponent<MeshRenderer>();
        bool hasRenderer = meshRenderer != null;
        gameObject.SetActive(hasRenderer);
        if (!hasRenderer) { return; }
        DisplayMaterials(meshRenderer.sharedMaterials);
    }
}
