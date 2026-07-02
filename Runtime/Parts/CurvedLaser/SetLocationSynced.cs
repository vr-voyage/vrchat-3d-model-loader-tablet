
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class SetLocationSynced : UdonSharpBehaviour
{
    [SerializeField]
    [UdonSynced]
    public Vector3 syncedPosition;
    
    public Transform referenceTransform;
    public void SyncPosition()
    {
        GameObject go = gameObject;
        if (Networking.IsOwner(go))
        {
            WeGotOwnership();
        }
        else
        {
            Networking.GetOwner(go);
        }
    }

    public void WeGotOwnership()
    {
        if (referenceTransform == null) { return; }
        syncedPosition = referenceTransform.position;
        RequestSerialization();
        TeleportToReference();
    }

    public override void OnDeserialization()
    {
        if (referenceTransform == null) { return; }
        referenceTransform.position = syncedPosition;
        TeleportToReference();
    }

    void TeleportToReference()
    {
        transform.position = referenceTransform.position;

    }

}
