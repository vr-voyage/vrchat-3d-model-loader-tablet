
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class MainDispatcher : UdonSharpBehaviour
{
    public GameObject placementLaser;
    public Transform objectRoot;

    public void ActivateLaser()
    {
        if (placementLaser != null) { placementLaser.SetActive(true); }
    }
}
