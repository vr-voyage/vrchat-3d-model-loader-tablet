
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class CameraDistanceSlider : UdonSharpBehaviour
{
    public Transform cameraTransform;
    public Slider sliderDistance;

    public Vector3 wantedPosition;
    public Vector3 wantedRotation;

    public override void Interact()
    {
        Vector3 cameraPosition = wantedPosition;
        cameraPosition.z = -sliderDistance.value;
        cameraTransform.localPosition = cameraPosition;
        cameraTransform.localEulerAngles = wantedRotation;
        
    }
}
