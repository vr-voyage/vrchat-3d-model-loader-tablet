
using UdonSharp;
using UnityEngine;
using VoyageVoyage;
using VRC.SDKBase;
using VRC.Udon;

public class MainUIController : UdonSharpBehaviour
{
    public NodeInspectorController nodeInspector;
    public NodeTreeController nodeTree;
    public TitleBarController titleBar;
    public LoadStatusController loadStatus;
    public GLBLoader loader;

    bool shownAfterDownloaded = false;

    public void NodeSelected(GameObject go)
    {
        nodeInspector.Display(go);
    }

    public void SceneLoaded()
    {
        Debug.Log("Scene Loaded");
        if (shownAfterDownloaded) { Debug.LogError("ALREADY SHOWN !"); return; }
        shownAfterDownloaded = true;

        SendCustomEventDelayedSeconds(nameof(ShowContent), 1.0f);
    }

    public void ParseError()
    {
        loadStatus.ShowError("Could not load 3D Model");
    }



    public void ShowContent()
    {
        loadStatus.ShowSuccess();
        loadStatus.ShowExtensions(loader.extensionsUsed);
        ShowHierarchy();
    }

    public void ShowHierarchy()
    {
        nodeTree.ShowHierarchy(loader.mainParent.gameObject);
    }

    public void DownloadSuccess(VRCUrl url)
    {
        titleBar.SetFilenameFromUrl(url);
        shownAfterDownloaded = false;
    }

    public void DownloadFailure(string message)
    {
        loadStatus.ShowError(message);
        shownAfterDownloaded = false;
    }
}
