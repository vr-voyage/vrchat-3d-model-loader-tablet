
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
    
    public void Clear()
    {

        loadedIcon.gameObject.SetActive(false);
        couldNotLoadIcon.gameObject.SetActive(false);
        errorText.gameObject.SetActive(false);
    }

    public void ShowError(string message)
    {
        Clear();
        errorText.text = message;
        errorText.gameObject.SetActive(true);
        couldNotLoadIcon.gameObject.SetActive(true);
    }

    public void ShowSuccess()
    {
        Clear();
        loadedIcon.gameObject.SetActive(true);
    }

}
