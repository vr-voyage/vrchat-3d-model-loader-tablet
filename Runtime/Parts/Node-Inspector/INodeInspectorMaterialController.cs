using System.Collections;
using System.Collections.Generic;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

public abstract class INodeInspectorMaterialController : UdonSharpBehaviour
{
    public TMPro.TMP_Text materialName;
    public TMPro.TMP_Text shaderName;

    public void SetText(TMPro.TMP_Text text, string content)
    {
        if (text != null) text.text = content;
    }

    public void SetColor(Image image, Color c)
    {
        if (image != null) image.color = c;
    }

    public void SetTexture(RawImage image, Texture t)
    {
        if (image != null) image.texture = t;
    }

    public const int PROPERTY_TEXTURE = 0;
    public const int PROPERTY_COLOR = 1;
    public const int PROPERTY_FLOAT = 2;
    public const int PROPERTY_INT = 3;
    public const int PROPERTY_TILINGOFFSET = 4;

    void SetProperty(Material m, object[] property)
    {
        if (property == null || property.Length != 3)
        {
            return;
        }

        string propertyName = (string)property[0];
        if (string.IsNullOrWhiteSpace(propertyName)) { return; }

        int propertyType = (int)property[1];
        object uiElement = property[2];

        if (uiElement == null)
        {
            return;
        }

        System.Type uiElementType = uiElement.GetType();

        switch(propertyType)
        {
            case PROPERTY_TEXTURE:
                if (uiElementType == typeof(RawImage))
                {
                    RawImage rawImage = (RawImage)uiElement;
                    rawImage.texture = m.GetTexture(propertyName);
                }
                break;
            case PROPERTY_COLOR:
                if (uiElementType == typeof(Image))
                {
                    Image image = (Image)uiElement;
                    image.color = m.GetColor(propertyName);
                }
                break;
            case PROPERTY_FLOAT:
                float value = m.GetFloat(propertyName);
                if (uiElementType == typeof(TMPro.TextMeshProUGUI))
                {
                    TMPro.TMP_Text textElement = (TMPro.TMP_Text)uiElement;
                    textElement.text = value.ToString("F3");
                }
                else if (uiElementType == typeof(Toggle))
                {
                    Toggle toggle = (Toggle)uiElement;
                    toggle.isOn = value != 0;
                }
                else
                {
                    Debug.Log(uiElementType.ToString());
                    Debug.Log(uiElementType == typeof(TMPro.TMP_Text));
                }
                break;
            case PROPERTY_INT:
                if (uiElementType == typeof(TMPro.TMP_Text))
                {
                    TMPro.TMP_Text textElement = (TMPro.TMP_Text)uiElement;
                    textElement.text = m.GetInt(propertyName).ToString();
                }
                break;
            case PROPERTY_TILINGOFFSET:
                if (uiElementType == typeof(TMPro.TMP_Text[]))
                {
                    TMPro.TMP_Text[] textElements = (TMPro.TMP_Text[])uiElement;
                    if (textElements.Length != 4) return;
                    Vector2 tiling = m.GetTextureScale(propertyName);
                    Vector2 offset = m.GetTextureOffset(propertyName);
                    TMPro.TMP_Text uiTilingX = textElements[0];
                    TMPro.TMP_Text uiTilingY = textElements[1];
                    TMPro.TMP_Text uiOffsetX = textElements[2];
                    TMPro.TMP_Text uiOffsetY = textElements[3];

                    if (uiTilingX != null) uiTilingX.text = tiling.x.ToString("F2");
                    if (uiTilingY != null) uiTilingY.text = tiling.y.ToString("F2");
                    if (uiOffsetX != null) uiOffsetX.text = offset.x.ToString("F2");
                    if (uiOffsetY != null) uiOffsetY.text = offset.y.ToString("F2");
                }
                break;
            default:
                Debug.Log($"<color=red>Unhandled type {uiElementType.ToString()}");
                break;
                
        }

    }
    protected void SetProperties(Material m, object[] properties)
    {
        if (m == null) return;
        int nProperties = properties.Length;
        for (int i = 0; i < nProperties; i++)
        {
            SetProperty(m, (object[])properties[i]);
        }
    }

    virtual public void DisplayMaterial(Material m)
    {
        if (m == null) { return; }
        
        SetText(materialName, m.name);
        SetText(shaderName, m.shader.name);
        
    }
}
