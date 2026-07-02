
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class TitleBarController : UdonSharpBehaviour
{
    public TMPro.TextMeshProUGUI uiFilename;

    void ResetFilename()
    {
        if (uiFilename != null) uiFilename.text = "";
    }

    public void SetFilenameFromUrl(VRCUrl url)
    {
        if (uiFilename == null) return;

        ResetFilename();

        if (url == null) return;
        string urlString = url.ToString().Trim();
        if (urlString.Length == 0) return;

        int lastSlashIndex = urlString.LastIndexOf('/');
        lastSlashIndex = lastSlashIndex >= 0 ? lastSlashIndex + 1 : 0;

        int lastQuestionMark = urlString.IndexOf('?', lastSlashIndex);
        lastQuestionMark = lastQuestionMark >= 0 ? lastQuestionMark : urlString.Length;

        uiFilename.text = urlString.Substring(lastSlashIndex, lastQuestionMark - lastSlashIndex);
    }
}
