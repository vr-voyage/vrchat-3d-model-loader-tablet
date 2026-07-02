
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class AttachToVRCPart : UdonSharpBehaviour
{
    public VRCPlayerApi.TrackingDataType vrTracker;
    public VRCPlayerApi.TrackingDataType nonVrTracker;

    VRCPlayerApi.TrackingDataType usedTracker;

    Transform t;

    private void Start()
    {
        VRCPlayerApi localPlayer = Networking.LocalPlayer;
        bool isInVr = localPlayer.IsUserInVR();
        usedTracker = isInVr ? vrTracker : nonVrTracker;
        t = gameObject.transform;
    }

    private void Update()
    {
        VRCPlayerApi localPlayer = Networking.LocalPlayer;
        VRCPlayerApi.TrackingData trackinData = localPlayer.GetTrackingData(usedTracker);
        t.position = trackinData.position;
        t.rotation = trackinData.rotation;
    }
}
