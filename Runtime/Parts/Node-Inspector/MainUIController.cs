
using DG.Tweening;
using ThreeDModelLoader;
using UdonSharp;
using UnityEngine;
using VoyageVoyage;
using VRC.SDKBase;
using VRC.Udon;

public class MainUIController : UdonSharpBehaviour
{
    public MainDispatcher tabletDispatcher;
    public MainHandler loadingHandler;
    public NodeInspectorController nodeInspector;
    public NodeTreeController nodeTree;
    public TitleBarController titleBar;
    public LoadStatusController loadStatus;
    public StatsController statsController;
    public SidePanelLeftController leftPanelController;

    public void Awake()
    {
        FillHierarchy();
        nodeInspector.Display(tabletDispatcher.GetRoot());
    }

    public void RootHasMoved()
    {
        Debug.Log("<color=cyan>Refreshing display</color>");
        nodeInspector.RefreshIfDisplaying(tabletDispatcher.GetRoot());
    }

    public void LoadData(byte[] data, VRCUrl from)
    {
        titleBar.SetFilenameFromUrl(from);
        Transform spawnRoot = tabletDispatcher.objectRoot;
        spawnRoot.ClearChildren();
        loadingHandler.Load(data, spawnRoot);
    }

    public void NodeSelected(GameObject go)
    {
        nodeInspector.Display(go);
    }

    public void Loaded()
    {
        Debug.Log("Scene Loaded");

        SendCustomEventDelayedSeconds(nameof(ShowContent), 0.015f);
    }

    public void Error()
    {
        loadStatus.ShowError("Could not load 3D Model");
    }

    public void ShowContent()
    {
        loadStatus.ShowSuccess();
        ILoaderHandler handler = loadingHandler.GetCurrentHandler();
        if (handler != null)
        {
            statsController.ShowExtensions(handler.GetExtensionsUsed());
            statsController.SetStats(handler.GetStats());
            statsController.SetDimensions(handler.GetDimensions());
            
        }

        NodeSelected(tabletDispatcher.GetRoot());
        FillHierarchy();
    }

    public void FillHierarchy()
    {
        nodeTree.ShowHierarchy(tabletDispatcher.GetRoot());
    }

    public void ShowHierarchy()
    {
        leftPanelController.ShowPanel(SidePanelLeftController.hierarchyPanelIndex);
    }

    public void ShowStats()
    {
        leftPanelController.ShowPanel(SidePanelLeftController.statsPanelIndex);
    }


    public void DownloadSuccess(VRCUrl url)
    {
        titleBar.SetFilenameFromUrl(url);
        statsController.Clear();
        statsController.SetURL(url);
        nodeTree.Clear();
        nodeInspector.Clear();
    }

    public void DownloadFailure(string message)
    {
        loadStatus.ShowError(message);
    }
}
