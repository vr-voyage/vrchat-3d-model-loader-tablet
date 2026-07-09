
using System.Xml;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class StatsController : UdonSharpBehaviour
{
    public MainUIController uIController;

    public TMPro.TextMeshProUGUI nodesCount;
    public TMPro.TextMeshProUGUI materialsCount;
    public TMPro.TextMeshProUGUI texturesCount;
    public TMPro.TextMeshProUGUI meshesCount;
    public TMPro.TextMeshProUGUI trianglesCount;

    public TMPro.TextMeshProUGUI dimensionX;
    public TMPro.TextMeshProUGUI dimensionY;
    public TMPro.TextMeshProUGUI dimensionZ;

    public TMPro.TextMeshProUGUI urlText;


    public Transform extensionsListRoot;
    public GameObject extensionListElementPrefab;

    void SetText(TMPro.TextMeshProUGUI textMeshUGUI, string s)
    {
        if (textMeshUGUI == null || s == null) return;
        textMeshUGUI.text = s;
    }

    public void SetStats(long[] stats)
    {
        SetText(nodesCount, stats[ThreeDModelLoader.ILoaderHandler.statNodesIndex].ToString());
        SetText(materialsCount, stats[ThreeDModelLoader.ILoaderHandler.statMaterialsIndex].ToString());
        SetText(texturesCount, stats[ThreeDModelLoader.ILoaderHandler.statTexturesIndex].ToString());
        SetText(meshesCount, stats[ThreeDModelLoader.ILoaderHandler.statMeshesIndex].ToString());
        SetText(trianglesCount, stats[ThreeDModelLoader.ILoaderHandler.statTrianglesIndex].ToString());
    }

    public void SetDimensions(Vector3 v)
    {
        SetText(dimensionX, v.x.ToString());
        SetText(dimensionY, v.y.ToString());
        SetText(dimensionZ, v.z.ToString());
    }

    public void SetURL(VRCUrl url)
    {
        urlText.text = url.ToString();
    }

    public void Clear()
    {
        SetText(nodesCount, "");
        SetText(materialsCount, "");
        SetText(texturesCount, "");
        SetText(meshesCount, "");
        SetText(trianglesCount, "");
        SetText(dimensionX, "");
        SetText(dimensionY, "");
        SetText(dimensionZ, "");
        SetText(urlText, "");

        extensionsListRoot.ClearChildren();

    }

    void AddExtension(string extensionName)
    {
        if (extensionsListRoot == null || extensionListElementPrefab == null) { return; }

        GameObject listElement = Instantiate(extensionListElementPrefab, extensionsListRoot);
        TMPro.TextMeshProUGUI textElement = listElement.GetComponent<TMPro.TextMeshProUGUI>();
        textElement.text = extensionName;

    }
    public void ShowExtensions(string[] extensions)
    {
        extensionsListRoot.ClearChildren();
        int nExtensions = extensions.Length;
        for (int i = 0; i < nExtensions; i++)
        {
            AddExtension(extensions[i]);
        }
    }

    public void NodesClicked()
    {
        uIController.ShowHierarchy();
    }
}
