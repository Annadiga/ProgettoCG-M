using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class IHandMenuButtonController : MonoBehaviour
{
    private Interactable OnClickEventsHandler;
    private ButtonConfigHelper ButtonConfigHelper;
    public bool NeedsConfirmDialog = true;
    private bool IsConfirmDialogOpen = false;

    [SerializeField, Tooltip("Prefab ti instanciate to get action confirm or deny")]
    private GameObject Prefab = null;

    public List<UnityAction> LeftHandler = new List<UnityAction>(); // Da implementare
    public List<UnityAction> RightHandler = new List<UnityAction>(); // Da implementare


    public void Start()
    {
        this.OnClickEventsHandler = this.GetComponent<Interactable>();
        this.ButtonConfigHelper = this.GetComponent<ButtonConfigHelper>();

        this.OnClickEventsHandler.OnClick.AddListener(this.OnClick);
    }

    public void SetButtonText(string NewText)
    {
        this.ButtonConfigHelper.MainLabelText = NewText;
    }

    private void OnClick()
    {
        if (this.Prefab == null || !NeedsConfirmDialog || IsConfirmDialogOpen) return;
        InstantiateConfirmDialog();
        ToggleDialogOpen();
        //SetStatus(false); // From ButtonStatusController
    }

    public void ToggleDialogOpen()
    {
        IsConfirmDialogOpen = !IsConfirmDialogOpen;
    }

    private void InstantiateConfirmDialog()
    { 
        GameObject ConfirmDialog = Instantiate(Prefab);
        ConfirmDialog.GetComponent<DialogTwoBtnController>().AddButtonsCallbacks(this);
    }

}
