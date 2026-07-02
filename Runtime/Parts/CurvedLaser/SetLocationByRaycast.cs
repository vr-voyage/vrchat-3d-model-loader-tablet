
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SetLocationByRaycast : UdonSharpBehaviour
{
    public GameObject gameObjectToMove;

    public void HandleHitLocation()
    {
        if (gameObjectToMove != null)
        {
            SetLocationSynced locationSynced = gameObjectToMove.GetComponent<SetLocationSynced>();
            if (locationSynced != null)  { locationSynced.SyncPosition(); }
        }
    }
}
