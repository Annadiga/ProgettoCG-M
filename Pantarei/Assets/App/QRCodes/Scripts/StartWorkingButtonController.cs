using Microsoft.MixedReality.Toolkit.UI;
using MRTKExtensions.QRCodes;
using System.Net;
using System.Net.Http;
using Texts;
using UnityEngine;

public class StartWorkingButtonController : MonoBehaviour
{
    [SerializeField]
    private QRTrackerController trackerController;

    [SerializeField, Tooltip("Dialog for server responses")]
    private DialogOneBtnController DialogController;

    [SerializeField, Tooltip("QR code menu button controller to update text")]
    private QRCodeMenuButtonController QrButtonController;

    private string GetQRMessage() { return trackerController.GetLastMessage().Data; }

    private void Start()
    {
        gameObject.SetActive(false);
        this.GetComponent<Interactable>().OnClick.AddListener(FetchQr);
        trackerController.PositionSet += (object sender, Pose pose) => { gameObject.SetActive(true); };
    }

    private async void FetchQr()
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                string url = ApiUtils.BuildQrUri(GetQRMessage());
                HttpResponseMessage httpResponseMessage = await client.GetAsync(url);

                InstanciateDialog(httpResponseMessage.StatusCode);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            return;
        }
    }

    private bool IsDialogOpen = false;

    private void InstanciateDialog(HttpStatusCode code)
    {
        if (IsDialogOpen) return;
        DialogOneBtnController Dialog = Instantiate(DialogController);
        switch (code)
        {
            case HttpStatusCode.OK:
                trackerController.StopTracking();

                Dialog.AddDefaultButtonsCallbacks();
                Dialog.AddCallback(() =>
                {
                    try
                    {
                        trackerController.ClearTracking();
                        IsDialogOpen = false;
                        QrButtonController.UpdateText();
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError(ex);
                    }
                });
                Dialog.SetTitle(TitleTexts.QR_OK);
                Dialog.SetContent(ContentTexts.QR_OK);
                Dialog.SetBtnText(ButtonTexts.OK);
                break;

            default:
                Dialog.AddDefaultButtonsCallbacks();
                Dialog.SetTitle(TitleTexts.DEFAULT_ERROR_TITLE);
                Dialog.SetContent($"{ContentTexts.STATUS_CODE_ERROR} {code}");
                Dialog.SetBtnText(ButtonTexts.OK);
                break;
        }
        
        IsDialogOpen = true;
    }

    private void InstanciateErrorDialog(System.Exception e)
    {
        DialogOneBtnController Dialog = Instantiate(DialogController);
        Dialog.AddDefaultButtonsCallbacks();
        Dialog.SetTitle(TitleTexts.DEFAULT_ERROR_TITLE);
        Dialog.SetContent("Error Message:\n" + e.Message);
        Dialog.SetBtnText(ButtonTexts.OK);
    }
}
