
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class MainDispatcher : UdonSharpBehaviour
{
    public CurvedLaserEffect placementLaser;
    public Transform objectRoot;
    public FrameWithCamera cameraFrame;
    public MainUIController mainUi;

    public void ActivateLaser()
    {
        if (placementLaser != null) { placementLaser.gameObject.SetActive(true); }
    }

    public void Loaded()
    {
        if (cameraFrame == null || objectRoot == null) return;
        cameraFrame.Focus(objectRoot.gameObject);
    }

    public GameObject GetRoot()
    {
        if (objectRoot == null) return null;
        return objectRoot.gameObject;
    }

    public void ObjectTransformUpdated()
    {
        mainUi.RootHasMoved();
    }

}
