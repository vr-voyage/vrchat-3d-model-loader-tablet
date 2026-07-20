
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
        bool firstSync = false;
        double urlSetTime = 0;

        [UdonSynced]
        [HideInInspector]
        public VRCUrl syncedUrl;

        [UdonSynced]
        [HideInInspector]
        public double syncedTime;

        public void Load(byte[] data)
        {
            mainUi.LoadData(data, lastUrl);
        }

        private void Start()
        {
            urlSetTime = Networking.GetServerTimeInSeconds();
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
            initiator = false;
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
            Debug.Log("<color=cyan>URL INTERACT !</color>");
            VRCUrl url = inputField.GetUrl();
            if (!ValidURL(url)) return;

            initiator = true;
            urlSetTime = Networking.GetServerTimeInSeconds();
            Download(url);
        }

        public override void OnDeserialization()
        {
            /* We ignore invalid URL */
            if (!ValidURL(syncedUrl)) return;
            Debug.Log($"<color=cyan>{urlSetTime} >= {syncedTime} ?</color>");
            /* We ignore synchro from the past. This breaks sync loops */
            if (urlSetTime >= syncedTime) return;
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
            syncedTime = urlSetTime;
            RequestSerialization();
        }


    }
}

