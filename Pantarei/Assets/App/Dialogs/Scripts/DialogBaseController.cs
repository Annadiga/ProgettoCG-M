using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogBaseController : MonoBehaviour
{
    public string TitleText;
    [Multiline] public string ContentText;
    protected GameObject ButtonContent;

    protected bool IsInitialized = false;

    internal void SetContent(string message)
    {
        transform.GetChild(1).GetComponent<TextMeshPro>().text = string.IsNullOrEmpty(message) ? "Default" : message;
    }

    internal void SetTitle(string v)
    {
        transform.GetChild(0).GetComponent<TextMeshPro>().text = string.IsNullOrEmpty(v) ? "Default" : v;
    }

}
