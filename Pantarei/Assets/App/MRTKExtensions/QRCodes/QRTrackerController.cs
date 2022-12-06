using System;
using Microsoft.MixedReality.Toolkit;
using UnityEngine;

namespace MRTKExtensions.QRCodes
{
  
    public class QRTrackerController : MonoBehaviour
    {
        [SerializeField]
        private SpatialGraphCoordinateSystemSetter spatialGraphCoordinateSystemSetter;


        private Transform markerHolder;
        private GameObject markerDisplay;
        private QRInfo lastMessage;
        private int trackingCounter;

        private bool _IsTrackingActive = false;
        public bool IsTrackingActive { 
            get
            {
                return QRCodeTrackingService.IsInitialized ? _IsTrackingActive : false;
            }
            private set
            {
                _IsTrackingActive = value;
            } 
        }

        private IQRCodeTrackingService qrCodeTrackingService;

        private IQRCodeTrackingService QRCodeTrackingService
        {
            get
            {
                while (!MixedRealityToolkit.IsInitialized && Time.time < 5) ;
                return qrCodeTrackingService ??= MixedRealityToolkit.Instance.GetService<IQRCodeTrackingService>();
            }
        }

        private void Start()
        {
            if (!QRCodeTrackingService.IsSupported)
            {
                Debug.Log("Unaviable");
                return;
            }

            markerHolder = spatialGraphCoordinateSystemSetter.gameObject.transform;
            markerDisplay = markerHolder.GetChild(0).gameObject;
            markerDisplay.SetActive(false);

            QRCodeTrackingService.QRCodeFound += ProcessTrackingFound;
            spatialGraphCoordinateSystemSetter.PositionAcquired += SetPosition;
            spatialGraphCoordinateSystemSetter.PositionAcquisitionFailed +=
                (s, e) => ResetTracking();


            if (QRCodeTrackingService.IsInitialized)
            {
                StartTracking();
            }
            else
            {
                QRCodeTrackingService.Initialized += QRCodeTrackingService_Initialized;
            }
        }

        private void QRCodeTrackingService_Initialized(object sender, EventArgs e)
        {
            StartTracking();
        }

        public void StartTracking()
        {
            Debug.Log("Enabling");
            QRCodeTrackingService.Enable();
            IsTrackingActive = true;
        }
        public void StopTracking()
        {
            Debug.Log("Disabling");
            QRCodeTrackingService.Disable();
            IsTrackingActive = false;
        }

        public void ToggleTracker()
        {
            if (IsTrackingActive) StopTracking();
            else StartTracking();
            ClearTracking();
        }

        public void ClearTracking()
        {
            if (QRCodeTrackingService.IsInitialized)
                markerDisplay.SetActive(false);
        }

        public void ResetTracking()
        {
            if (QRCodeTrackingService.IsInitialized)
            {
                markerDisplay.SetActive(false);
                IsTrackingActive = true;
            }
        }

        private void ProcessTrackingFound(object sender, QRInfo msg)
        {
            if (msg == null || !IsTrackingActive )
            {
                return;
            }

            lastMessage = msg;

            if (Math.Abs((DateTimeOffset.UtcNow - msg.LastDetectedTime.UtcDateTime).TotalMilliseconds) < 200)
            {
                spatialGraphCoordinateSystemSetter.SetLocationIdSize(msg.SpatialGraphNodeId,
                    msg.PhysicalSideLength);
            }
        }

        public QRInfo GetLastMessage()
        {
            return lastMessage;
        }

        private void SetPosition(object sender, Pose pose)
        {
            IsTrackingActive = false;
            markerHolder.localScale = Vector3.one * lastMessage.PhysicalSideLength;
            markerDisplay.SetActive(true);
            PositionSet?.Invoke(this, pose);
        }

        public EventHandler<Pose> PositionSet;
    }
}