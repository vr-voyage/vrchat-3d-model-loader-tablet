
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SetLocationByRaycast : UdonSharpBehaviour
{
    public GameObject gameObjectToMove;

    public Transform hitLocation;
    public void HandleHitLocation()
    {
        if (gameObjectToMove != null)
        {
            //Debug.Log($"<color=cyan>Moving Object to {hitLocation.position}</color>");

            SetLocationSynced locationSynced = gameObjectToMove.GetComponent<SetLocationSynced>();
            if (locationSynced != null)  { locationSynced.SyncPosition(); }
        }
    }
}
