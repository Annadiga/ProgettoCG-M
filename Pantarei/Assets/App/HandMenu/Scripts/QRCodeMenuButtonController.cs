using System;
using UnityEngine;

using Texts;
using MRTKExtensions.QRCodes;
using Microsoft.MixedReality.Toolkit.UI;

public class QRCodeMenuButtonController : IHandMenuButtonController
{
    private static class TextStatus
    {
        public static readonly string QR_CODE_MANAGER_DOWN = "Start QR\nScanner";
        public static readonly string QR_CODE_MANAGER_UP = "Stop QR\nScanner";
    }

    private string GetStatusText()
    {
        return QRCodeManager.IsTrackingActive
            ? TextStatus.QR_CODE_MANAGER_UP
            : TextStatus.QR_CODE_MANAGER_DOWN;
    }

    [SerializeField, Tooltip("QR code manager GameObject")]
    private QRTrackerController QRCodeManager;

    [SerializeField, Tooltip("Base Dialog for Exceptions")]
    private DialogOneBtnController ExceptionDialog;

    private void Awake()
    {
        if (NeedsConfirmDialog) RightHandler.Add(ClickHandler);
        else GetComponent<Interactable>().OnClick.AddListener(ClickHandler);
    }

    public void UpdateText()
    {
        SetButtonText(GetStatusText());
    }

    private new void Start()
    {
        base.Start();
        UpdateText();
    }

    private void ClickHandler()
    {
        try
        {
            QRCodeManager.ToggleTracker();
        }
        catch (Exception ex)
        {
            DialogOneBtnController Dialog = Instantiate(ExceptionDialog);
            Dialog.AddDefaultButtonsCallbacks();
            Dialog.SetTitle(TitleTexts.DEFAULT_ERROR_TITLE);
            Dialog.SetContent("Error Message:\n" + ex.Message);
            Dialog.SetBtnText(ButtonTexts.OK);
            this.SetButtonText(ButtonTexts.QR_ERROR);
        }
        finally
        {
            Debug.Log($"Finally: {GetStatusText()}, {QRCodeManager.IsTrackingActive}");
            UpdateText();
        }
        //Debug.Log($"Finally: {GetStatusText()}, {QRCodeManager.IsTrackingActive}");
        //UpdateText();
    }
}
