
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VoyageVoyage;
using VRC.SDKBase;
using VRC.Udon;

public class LoadStatusController : UdonSharpBehaviour
{
    public Image loadedIcon;
    public Image couldNotLoadIcon;
    public TMPro.TMP_InputField errorText;

    public Image voyageExtensionIcon;
    public Image mtoonExtensionIcon;
    public Image shaderMotionExtensionIcon;
    
    public void Clear()
    {

        loadedIcon.gameObject.SetActive(false);
        couldNotLoadIcon.gameObject.SetActive(false);
        errorText.gameObject.SetActive(false);

        voyageExtensionIcon.gameObject.SetActive(false);
        mtoonExtensionIcon.gameObject.SetActive(false);
        shaderMotionExtensionIcon.gameObject.SetActive(false);
    }

    public void ShowError(string message)
    {
        Clear();
        errorText.text = message;
        errorText.gameObject.SetActive(true);
        couldNotLoadIcon.gameObject.SetActive(true);
    }

    public void ShowExtensions(string[] extensions)
    {
        object[] extensionsAndIcons = new object[]
        {
            "EXT_voyage_exporter", voyageExtensionIcon,
            "VRMC_materials_mtoon", mtoonExtensionIcon,
            "EXT_voyage_shadermotion", shaderMotionExtensionIcon
        };

        for (int i = 0; i < extensions.Length; i++)
        {
            Debug.Log($"<color=cyan>{extensions[i]}</color>");
        }

        int nExtensions = extensionsAndIcons.Length;
        for (int i = 0; i < nExtensions; i+=2)
        {
            string extensionName = (string)extensionsAndIcons[i];
            Image extensionIcon = (Image)extensionsAndIcons[i + 1];

            bool extensionIsUsed = extensions.Contains(extensionName);
            extensionIcon.gameObject.SetActive(extensionIsUsed);
        } 
    }

    public void ShowSuccess()
    {
        Clear();
        loadedIcon.gameObject.SetActive(true);
    }

}
