using UnityEngine;

public class QRSatusController : MonoBehaviour
{
    private static class TextStatus
    {
        public static readonly string QR_CODE_MANAGER_DOWN = "Start QR\nScanner";
        public static readonly string QR_CODE_MANAGER_UP = "Stop QR\nScanner";
    }

    [SerializeField]
    private QRTracking.QRCodesManager qrCodeManager;
    private string statusText;

    // Start is called before the first frame update
    void Start()
    {
        SetStatusText();
    }

    private void SetStatusText()
    {
        this.statusText = this.qrCodeManager.IsTrackerRunning
            ? TextStatus.QR_CODE_MANAGER_UP
            : TextStatus.QR_CODE_MANAGER_DOWN;
    }

    private void StartScan()
    {
        this.qrCodeManager.StartQRTracking();
        SetStatusText();
    }

    private void StopScan()
    {
        this.qrCodeManager.StopQRTracking();
        SetStatusText();
    }

    public void ToggleScan()
    {
        if (this.qrCodeManager.IsTrackerRunning) StopScan();
        else StartScan();
    }

    public bool GetTrackerStatus()
    {
        return this.qrCodeManager.IsTrackerRunning;
    }

    public string GetStatusText()
    {
        return statusText;
    }
}
