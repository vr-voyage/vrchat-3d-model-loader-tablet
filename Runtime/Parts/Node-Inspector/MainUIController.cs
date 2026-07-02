
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

        SendCustomEventDelayedSeconds(nameof(ShowContent), 1.0f);
    }

    public void Error()
    {
        loadStatus.ShowError("Could not load 3D Model");
    }



    public void ShowContent()
    {
        loadStatus.ShowSuccess();
        loadStatus.ShowExtensions(loadingHandler.GetExtensionsUsed());
        ShowHierarchy();
    }

    public void ShowHierarchy()
    {
        nodeTree.ShowHierarchy(tabletDispatcher.objectRoot.gameObject);
    }

    public void DownloadSuccess(VRCUrl url)
    {
        titleBar.SetFilenameFromUrl(url);
    }

    public void DownloadFailure(string message)
    {
        loadStatus.ShowError(message);
    }
}
