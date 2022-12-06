using System.Collections;
using System.Collections.Generic;
using Texts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ItemCardController : MonoBehaviour
{
    [SerializeField, Tooltip("Model to show in card prefab")]
    private GameObject ItemCardModelPrefab;

    [SerializeField] private GameObject Model3D;

    //[SerializeField] private Transform pos;


    ItemCardModel CardInfo;

    //[SerializeField, Tooltip("3d model for folders")]
    //private GameObject FolderPrefab;
    //[SerializeField, Tooltip("3d model for pdfs")]
    //private GameObject PdfPrefab;

    public void SetCardInfo(ItemCardModel card)
    {
        CardInfo = card;
    }
    public ItemCardModel GetCardInfo()
    {
        return CardInfo;
    }

    public void SetTitle()
    {
        transform.GetChild(1).gameObject.GetComponent<TextMeshPro>().text =
            string.IsNullOrEmpty(CardInfo.GetName()) ? TitleTexts.DEFAULT_TITLE : CardInfo.GetName();
    }

    public void SetPrefab()
    {
        if (string.IsNullOrEmpty(CardInfo.Extention)) return;
        switch (CardInfo.Extention.ToUpper())
        {
            case "FOLDER":
                break;
            case "PDF":
                break;
            default:
                break;
        }
    }

    public void UpdateInfo()
    {
        SetTitle();
        SetPrefab();
    }

}
