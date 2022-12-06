using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogTwoBtnController : DialogBaseController
{

    private GameObject BtnLeft;
    public string LeftButtonText;

    private GameObject BtnRight;
    public string RightButtonText;


    public void Awake()
    {
        SetTitle(TitleText);
        SetContent(ContentText);
        GameObject ButtonContent = transform.GetChild(2).gameObject;

        this.BtnLeft = ButtonContent.transform.GetChild(0).gameObject;
        this.BtnRight = ButtonContent.transform.GetChild(1).gameObject;

        SetLeftText(this.LeftButtonText);
        SetRightText(this.RightButtonText);

        AddButtonsCallbacks(null);
    }

    internal void SetRightText(string msg)
    {
        this.RightButtonText = msg;
        this.BtnRight.GetComponent<ButtonConfigHelper>().MainLabelText = String.IsNullOrEmpty(RightButtonText) ? "Ok" : RightButtonText;
    }

    internal void SetLeftText(string msg)
    {
        this.LeftButtonText = msg;
        this.BtnLeft.GetComponent<ButtonConfigHelper>().MainLabelText = String.IsNullOrEmpty(LeftButtonText) ? "Ok" : LeftButtonText;
    }

    public void AddButtonsCallbacks(IHandMenuButtonController MenuButtonCaller)
    {
        if (MenuButtonCaller != null)
        {
            //AddButtonsCallbacks(MenuButtonCaller.LeftHandler, MenuButtonCaller.RightHandler);
            foreach (UnityAction clbk in MenuButtonCaller.LeftHandler) AddLeftCallback(clbk);
            AddLeftCallback(MenuButtonCaller.ToggleDialogOpen);
            //AddLeftCallback(() => MenuButtonCaller.SetStatus(true)); // from ButtonStatusController

            foreach (UnityAction clbk in MenuButtonCaller.RightHandler) AddRightCallback(clbk);
            AddRightCallback(MenuButtonCaller.ToggleDialogOpen);
            //AddRightCallback(() => MenuButtonCaller.SetStatus(true)); // from ButtonStatusController
        }

        if (!this.IsInitialized)
        {
            this.IsInitialized = true;
            AddLeftCallback(() => Destroy(this.gameObject));

            AddRightCallback(() => Destroy(this.gameObject));
        }

    }

    public void AddButtonsCallbacks(UnityAction LeftHandler, UnityAction RightHandler)
    {
        AddRightCallback(LeftHandler);

        AddLeftCallback(RightHandler);
    }

    internal void AddRightCallback(UnityAction Clbk)
    {
        if (Clbk != null) BtnRight.GetComponent<Interactable>().OnClick.AddListener(Clbk);
    }

    internal void AddLeftCallback(UnityAction Clbk)
    {
        if (Clbk != null) BtnLeft.GetComponent<Interactable>().OnClick.AddListener(Clbk);
    }
}
