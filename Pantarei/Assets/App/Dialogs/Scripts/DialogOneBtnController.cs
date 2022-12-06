using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogOneBtnController : DialogBaseController
{
    private GameObject Btn;
    [SerializeField, Tooltip("Button text")]
    private string BtnText;

    public void Awake()
    {
        SetTitle(TitleText);
        SetContent(ContentText);
        GameObject ButtonContent = transform.GetChild(2).gameObject;

        this.Btn = ButtonContent.transform.GetChild(0).gameObject;
    }

    public void SetBtnText(string msg)
    {
        this.BtnText = msg;
        this.Btn.GetComponent<ButtonConfigHelper>().MainLabelText = string.IsNullOrEmpty(BtnText) ? "Ok" : BtnText;
    }
    public void AddDefaultButtonsCallbacks()
    {
        if (!this.IsInitialized)
        {
            this.IsInitialized = true;
            AddCallback(() => Destroy(this.gameObject));
        }
    }

    public void AddCallback(UnityAction Clbk)
    {
        if (Clbk != null) Btn.GetComponent<Interactable>().OnClick.AddListener(Clbk);
    }

    internal void DisableButton()
    {
        this.Btn.SetActive(false);
    }
    internal void EnableButton()
    {
        this.Btn.SetActive(true);
    }
}
