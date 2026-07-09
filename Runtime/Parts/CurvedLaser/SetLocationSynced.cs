
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class SetLocationSynced : UdonSharpBehaviour
{
    [UdonSynced]
    public Vector3 syncedPosition;
    [UdonSynced]
    public Vector3 syncedRotation;
    [UdonSynced]
    public Vector3 syncedScale;

    Vector3 lastDirtyPos;
    Vector3 lastDirtyRotation;
    Vector3 lastDirtyScale;

    Transform checkedTransform;

    public UdonSharpBehaviour[] objectTransformUpdatedReceivers;

    bool isDirty = false;
    double lastDirty = 0;

    private void Start()
    {
        checkedTransform = transform;
        syncedPosition = checkedTransform.position;
        syncedRotation = checkedTransform.eulerAngles;
        syncedScale = checkedTransform.localScale;

        lastDirtyPos = checkedTransform.localPosition;
        lastDirtyRotation = checkedTransform.localEulerAngles;
        lastDirtyScale = checkedTransform.localScale;

        enabled = true;
        SendCustomEventDelayedSeconds(nameof(CheckIfDirty), 0.5f, VRC.Udon.Common.Enums.EventTiming.Update);
    }

    bool WaitedEnoughSinceLastDirty()
    {
        return (Time.timeAsDouble - lastDirty) > 1.0;
    }

    public void CheckIfDirty()
    {
        bool isDirtyNow = 
            (syncedPosition != checkedTransform.position)
            | (syncedRotation != checkedTransform.eulerAngles)
            | (syncedScale != checkedTransform.localScale);

        if (isDirtyNow)
        {
            bool hasChangedSinceLastCheck =
                (lastDirtyPos != checkedTransform.position)
                | (lastDirtyRotation != checkedTransform.eulerAngles)
                | (lastDirtyScale != checkedTransform.localScale);
            if (hasChangedSinceLastCheck)
            {
                lastDirtyPos = checkedTransform.position;
                lastDirtyRotation = checkedTransform.eulerAngles;
                lastDirtyScale = checkedTransform.localScale;
                lastDirty = Time.timeAsDouble;
            }
        }

        isDirty |= isDirtyNow;

        if (isDirty && WaitedEnoughSinceLastDirty())
        {
            SyncPosition();
            isDirty = false;
        }
        SendCustomEventDelayedSeconds(nameof(CheckIfDirty), 0.25f, VRC.Udon.Common.Enums.EventTiming.Update);
    }

    void SyncPosition()
    {
        GameObject go = gameObject;
        if (Networking.IsOwner(go))
        {
            Debug.Log("<color=cyan>SYNCING WITH OWNERSHIP !</color>");
            WeGotOwnership();
        }
        else
        {
            Debug.Log("<color=cyan>REQUESTING OWNERSNIP !</color>");
            Networking.SetOwner(Networking.LocalPlayer, go);
        }
    }

    public void WeGotOwnership()
    {
        syncedPosition = transform.position;
        syncedRotation = transform.eulerAngles;
        syncedScale = transform.localScale;
        RequestSerialization();
        Notify("ObjectTransformUpdated");
    }

    public override void OnOwnershipTransferred(VRCPlayerApi player)
    {
        if (player == Networking.LocalPlayer)
        {
            WeGotOwnership();
        }
    }

    public override void OnDeserialization()
    {
        transform.position = syncedPosition;
        transform.eulerAngles = syncedRotation;
        transform.localScale = syncedScale;
        lastDirtyPos = syncedPosition;
        lastDirtyRotation = syncedRotation;
        lastDirtyScale = syncedScale;
        Debug.Log("DESERIALIZED !");
        Notify("ObjectTransformUpdated");
    }

    void Notify(string message)
    {
        int nReceivers = objectTransformUpdatedReceivers.Length;
        for (int i = 0; i < nReceivers; i++)
        {
            UdonSharpBehaviour receiver = objectTransformUpdatedReceivers[i];
            if (receiver == null) continue;
            receiver.SendCustomEvent(message);
        }
    }

}
