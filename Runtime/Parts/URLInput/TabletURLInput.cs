
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VoyageVoyage;
using VRC.SDK3.Components;
using VRC.SDK3.StringLoading;
using VRC.Udon.Common.Interfaces;
using ThreeDModelLoader;

namespace VoyageVoyage
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class TabletURLInput : UdonSharpBehaviour
    {
        public VRCUrlInputField inputField;
        public MainUIController mainUi;

        VRCUrl lastUrl;

        bool initiator = false;

        [UdonSynced]
        [HideInInspector]
        public VRCUrl syncedUrl;

        public void Load(byte[] data)
        {
            mainUi.LoadData(data, lastUrl);
        }

        bool ValidURL(VRCUrl url)
        {
            return ((url != lastUrl) & (url != null)) && url.ToString() != "";
        }

        public void Download(VRCUrl url)
        {
            lastUrl = url;
            inputField.interactable = false;
            VRCStringDownloader.LoadUrl(url, (IUdonEventReceiver)this);
        }

        public override void OnStringLoadSuccess(IVRCStringDownload result)
        {
            mainUi.DownloadSuccess(lastUrl);
            Load(result.ResultBytes);

            inputField.interactable = true;
            if (!initiator) return;
            if (WeAreTheOwner())
            {
                WeGotOwnership();
            }
            else
            {
                GetOwnership();
            }
        }

        public override void OnStringLoadError(IVRCStringDownload result)
        {
            mainUi.DownloadFailure($"Download error {result.ErrorCode} : {result.Error}");
            inputField.interactable = true;
        }

        public override void Interact()
        {
            VRCUrl url = inputField.GetUrl();
            if (!ValidURL(url)) return;

            initiator = true;
            Download(url);
        }

        public override void OnDeserialization()
        {
            if (!ValidURL(syncedUrl)) return;
            inputField.SetUrl(syncedUrl);
            Download(syncedUrl);
        }

        public bool WeAreTheOwner()
        {
            return Networking.LocalPlayer == Networking.GetOwner(gameObject);
        }

        public void GetOwnership()
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
        }

        public override void OnOwnershipTransferred(VRCPlayerApi player)
        {
            if (player != Networking.LocalPlayer) return;
            WeGotOwnership();
        }

        void WeGotOwnership()
        {
            syncedUrl = lastUrl;
            RequestSerialization();
        }


    }
}

