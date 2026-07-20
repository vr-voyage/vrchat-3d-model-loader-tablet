
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class CameraAxisSetter : UdonSharpBehaviour
{
    public Transform cameraAxis;
    float currentValue = 0f;

    public Slider slider;

    public override void Interact()
    {
        if (slider == null || cameraAxis == null) return;

        float newValue = slider.value;
        float relative = newValue - currentValue;
        currentValue = newValue;
        cameraAxis.Rotate(cameraAxis.up, relative);
    }
}
