
using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class NodeInspectorTransformController : INodeInspectorComponent
{
    public TMPro.TMP_InputField[] position;
    public TMPro.TMP_InputField[] rotation;
    public TMPro.TMP_InputField[] scale;

    Transform displayedTransform = null;

    public TMPro.TextMeshProUGUI rotationXLetter;
    public TMPro.TextMeshProUGUI rotationYLetter;
    public TMPro.TextMeshProUGUI rotationZLetter;

    public Color defaultTextColor;
    public Color selectedButtonTextColor;

    int selectedRotationAxis = 0;
    public Slider rotationSlider;
    Vector3 currentAngles;

    void Clear(TMPro.TMP_InputField[] texts)
    {
        int n = texts.Length;
        for (int i = 0; i < n; i++)
        {
            TMPro.TMP_InputField text = texts[i];
            if (text == null) continue;
            text.text = "";
            text.interactable = false;
        }
    }

    void MakeInteractable(TMPro.TMP_InputField[] texts)
    {
        int n = texts.Length;
        for (int i = 0; i < n; i++)
        {
            TMPro.TMP_InputField text = texts[i];
            if (text == null) continue;
            text.interactable = true;
        }
    }


    public override void Clear()
    {
        displayedTransform = null;
        Clear(position);
        Clear(rotation);
        Clear(scale);
        rotationSlider.interactable = false;
    }

    void SetValue(TMPro.TMP_InputField ui, float value)
    {
        if (ui == null) return;
        ui.text = value.ToString("F3");
    }

    void DisplayVector3(Vector3 v, TMPro.TMP_InputField[] output)
    {
        SetValue(output[0], v[0]);
        SetValue(output[1], v[1]);
        SetValue(output[2], v[2]);
    }

    void MakeInteractable()
    {
        MakeInteractable(position);
        MakeInteractable(rotation);
        MakeInteractable(scale);
        rotationSlider.interactable = true;
    }

    void Display(Transform t)
    {
        if (t == null) return;
        DisplayVector3(t.localPosition, position);
        currentAngles = t.localEulerAngles;
        DisplayVector3(currentAngles, rotation);
        DisplayVector3(t.localScale, scale);
        displayedTransform = t;
        MakeInteractable();
        if (selectedRotationAxis < 0 || selectedRotationAxis > 2) return;
        rotationSlider.value = currentAngles[selectedRotationAxis];
        
    }

    void RefreshDisplay()
    {
        if (displayedTransform == null) return;
        Display(displayedTransform);
    }

    public override void Display(GameObject go)
    {
        Clear();
        if (go == null) return;
        Display(go.transform);
        
    }


    bool TryGetValue(TMPro.TMP_InputField text, out float value)
    {
        value = 0f;
        if (text == null) return false;
        string textValue = text.text;
        if (textValue == null || textValue == "") return false;
        bool gotValue = Single.TryParse(textValue.Trim(), out value);
        return gotValue;
    }

    public void SetPosX()
    {
        Debug.Log("<color=cyan>Set Pos X !</color>");
        if (displayedTransform == null || position == null || position.Length < 3)
        {
            Debug.Log($"<color=cyan>{displayedTransform == null} - {position == null} - {position.Length}</color>");
            return;
        }
        if (TryGetValue(position[0], out float value))
        {
            Vector3 localPosition = displayedTransform.localPosition;
            localPosition.x = value;
            displayedTransform.localPosition = localPosition;
        }
        Display(displayedTransform.gameObject);
    }

    public void SetPosY()
    {
        if (displayedTransform == null || position == null || position.Length < 3) return;
        if (TryGetValue(position[1], out float value))
        {
            Vector3 localPosition = displayedTransform.localPosition;
            localPosition.y = value;
            displayedTransform.localPosition = localPosition;
        }
        RefreshDisplay();
    }

    public void SetPosZ()
    {
        if (displayedTransform == null || position == null || position.Length < 3) return;
        if (TryGetValue(position[2], out float value))
        {
            Vector3 localPosition = displayedTransform.localPosition;
            localPosition.z = value;
            displayedTransform.localPosition = localPosition;
        }
        RefreshDisplay();
    }

    public void SetRotX()
    {
        if (displayedTransform == null || rotation == null || rotation.Length < 3) return;
        if (TryGetValue(rotation[0], out float value))
        {
            Vector3 localRotation = displayedTransform.localEulerAngles;
            localRotation.x = value;
            displayedTransform.localEulerAngles = localRotation;
        }
        RefreshDisplay();
    }

    public void SetRotY()
    {
        if (displayedTransform == null || rotation == null || position.Length < 3) return;
        if (TryGetValue(rotation[1], out float value))
        {
            Vector3 localRotation = displayedTransform.localEulerAngles;
            localRotation.y = value;
            displayedTransform.localEulerAngles = localRotation;
        }
        RefreshDisplay();
    }

    public void SetRotZ()
    {
        if (displayedTransform == null || position == null || rotation.Length < 3) return;
        if (TryGetValue(rotation[2], out float value))
        {
            Vector3 localRotation = displayedTransform.localEulerAngles;
            localRotation.z = value;
            displayedTransform.localEulerAngles = localRotation;
        }
        RefreshDisplay();
    }

    public void SetScaleX()
    {
        if (displayedTransform == null || scale == null || scale.Length < 3) return;
        if (TryGetValue(scale[0], out float value))
        {
            Vector3 localScale = displayedTransform.localScale;
            localScale.x = value;
            displayedTransform.localScale = localScale;
        }
        RefreshDisplay();
    }

    public void SetScaleY()
    {
        if (displayedTransform == null || scale == null || scale.Length < 3) return;
        if (TryGetValue(scale[1], out float value))
        {
            Vector3 localScale = displayedTransform.localScale;
            localScale.y = value;
            displayedTransform.localScale = localScale;
        }
        RefreshDisplay();
    }

    public void SetScaleZ()
    {
        if (displayedTransform == null || scale == null || scale.Length < 3) return;
        if (TryGetValue(scale[2], out float value))
        {
            Vector3 localScale = displayedTransform.localScale;
            localScale.z = value;
            displayedTransform.localScale = localScale;
        }
        RefreshDisplay();
    }

    void SetRotationLettersColorsBack()
    {
        rotationXLetter.color = defaultTextColor;
        rotationYLetter.color = defaultTextColor;
        rotationZLetter.color = defaultTextColor;
    }

    public void SetRotationAxisX()
    {
        selectedRotationAxis = 0;
        SetRotationLettersColorsBack();
        rotationXLetter.color = selectedButtonTextColor;
        rotationSlider.value = currentAngles[0];
    }

    public void SetRotationAxisY()
    {
        selectedRotationAxis = 1;
        SetRotationLettersColorsBack();
        rotationYLetter.color = selectedButtonTextColor;
        rotationSlider.value = currentAngles[1];
    }

    public void SetRotationAxisZ()
    {
        selectedRotationAxis = 2;
        SetRotationLettersColorsBack();
        rotationZLetter.color = selectedButtonTextColor;
        rotationSlider.value = currentAngles[2];
    }

    public void RotationSliderChanged()
    {
        if (displayedTransform == null) return;
        float rotationValue = rotationSlider.value;
        currentAngles[selectedRotationAxis] = rotationValue;
        displayedTransform.localEulerAngles = currentAngles;
        rotation[selectedRotationAxis].text = rotationValue.ToString("F3");
    }

}
