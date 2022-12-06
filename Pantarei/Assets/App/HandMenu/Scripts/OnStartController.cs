using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Texts;
using System;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using Microsoft.MixedReality.Toolkit.UI;

public class OnStartController : MonoBehaviour
{
    private enum TutorialIndex
    {
        LOOK_HAND,
        HAND_LOOKED,
        BTN1,
        BTN2,
        BTN2_FOLLOW,
        BTN3,
        BTN4,
        END
    }

    public bool IsTutorialEnabled = false;
    private List<GameObject> MenuButtons = new List<GameObject>();

    [SerializeField, Tooltip("Welcome Dialog")]
    private DialogTwoBtnController WelcomeDialogController;

    [SerializeField, Tooltip("Tutorial Dialog")]
    private DialogOneBtnController TutorialDialog;

    [SerializeField, Tooltip("Tutorial highlights materials list")]
    private List<Material> HighlightMaterials;
    private Material[] LastOldMaterials;

    private TutorialIndex tutorialIndex = 0;

    private void Awake()
    {
        this.WelcomeDialogController = Instantiate(WelcomeDialogController);
        this.WelcomeDialogController.SetTitle(TitleTexts.WELCOME);
        this.WelcomeDialogController.SetContent(ContentTexts.WELCOME);

        this.WelcomeDialogController.SetLeftText(ButtonTexts.CONTINUE);
        this.WelcomeDialogController.SetRightText(ButtonTexts.START_TUTORIAL);

        this.WelcomeDialogController.AddRightCallback(this.StartTutorial);
    }

    private void StartTutorial()
    {
        TutorialDialog = Instantiate(TutorialDialog);
        SetTutorialDialogTexts(TitleTexts.TUTORIAL0, ContentTexts.TUTORIAL0, ButtonTexts.GO_ON);

        TutorialDialog.DisableButton();
        GameObject ButtonsParent = transform.GetChild(0).GetChild(1).gameObject;
        foreach (Transform child in ButtonsParent.transform)
        {
            child.GetComponent<Interactable>().enabled = false;
        }

        this.GoOnTutorial();
    }

    private void SetTutorialDialogTexts(string title, string content, string button = "AVANTI")
    {
        TutorialDialog.SetTitle(title);
        TutorialDialog.SetContent(content);
        TutorialDialog.SetBtnText(button);
    }

    //private void SetHighlightsButton(int BtnIndex)
    //{
    //    GameObject ButtonsParent = GetButtonsParent();
    //    GameObject Button = ButtonsParent.transform.GetChild(BtnIndex).gameObject;
    //    EnableHighligth(Button);
    //}

    //private GameObject GetButtonsParent()
    //{
    //    GameObject ButtonsParent = transform.GetChild(0).GetChild(1).gameObject;
    //    if (ButtonsParent == null) return null;
    //    return ButtonsParent;
    //}

    //private void EnableHighligth(GameObject Button)
    //{
    //    MeshRenderer renderer =
    //        Button.transform.GetChild(3).GetChild(1).transform.GetComponent<MeshRenderer>();
    //    Material[] OldMaterials = renderer.materials;
    //    LastOldMaterials = new Material[OldMaterials.Length];
    //    OldMaterials.CopyTo(LastOldMaterials, 0);
    //    List<Material> NewMaterials = new List<Material>();
    //    foreach (Material m in OldMaterials) NewMaterials.Add(m);
    //    foreach (Material m in HighlightMaterials) NewMaterials.Add(m);
    //    OldMaterials = NewMaterials.ToArray();
    //}

    //private void ResetHighlightsButton(int BtnIndex)
    //{
    //    GameObject ButtonsParent = GetButtonsParent();
    //    GameObject Button = ButtonsParent.transform.GetChild(BtnIndex).gameObject;
    //    DisableHighlight(Button);
    //}

    //private void DisableHighlight(GameObject Button)
    //{
    //    MeshRenderer renderer =
    //        Button.transform.GetChild(3).GetChild(1).transform.GetComponent<MeshRenderer>();
    //    renderer.materials = LastOldMaterials;
    //    LastOldMaterials = null;
    //}


    private void GoOnTutorial()
    {
        if (this.tutorialIndex == TutorialIndex.LOOK_HAND)
        {
            this.tutorialIndex += 1;

            TutorialDialog.AddCallback(this.GoOnTutorial);
            // comment in debug
            GetComponent<HandConstraintPalmUp>().OnFirstHandDetected.AddListener(TutorialDialog.EnableButton);
            GetComponent<HandConstraintPalmUp>().OnFirstHandDetected.AddListener(GoOnTutorial);
            // end comment in debug

            //TutorialDialog.EnableButton(); // Debug
        }
        else if (this.tutorialIndex == TutorialIndex.HAND_LOOKED)
        {
            // Enable Highlight for button 1
            //SetHighlightsButton((int) TutorialIndex.HAND_LOOKED);
            tutorialIndex += 1;
            // comment in debug
            GetComponent<HandConstraintPalmUp>().OnFirstHandDetected.RemoveListener(GoOnTutorial);
            GetComponent<HandConstraintPalmUp>().OnFirstHandDetected.RemoveListener(TutorialDialog.EnableButton);
            // end comment in debug
            SetTutorialDialogTexts(TitleTexts.TUTORIAL01, ContentTexts.TUTORIAL01, ButtonTexts.GO_ON);
        }
        else if (this.tutorialIndex == TutorialIndex.BTN1)
        {
            // Disable Hightlight for button 1
            //ResetHighlightsButton((int)TutorialIndex.HAND_LOOKED);
            // Enable Highlight for button 2
            //SetHighlightsButton((int)TutorialIndex.BTN1);
            tutorialIndex += 1;
            SetTutorialDialogTexts(TitleTexts.TUTORIAL1, ContentTexts.TUTORIAL1, ButtonTexts.GO_ON);
        }
        else if (this.tutorialIndex == TutorialIndex.BTN2)
        {
            // Disable Hightlight for button 2
            //ResetHighlightsButton((int)TutorialIndex.BTN1);
            // Enable Highlight for button 3
            //SetHighlightsButton((int)TutorialIndex.BTN2);
            tutorialIndex += 1;
            SetTutorialDialogTexts(TitleTexts.TUTORIAL2, ContentTexts.TUTORIAL2, ButtonTexts.GO_ON);
        }
        else if (this.tutorialIndex == TutorialIndex.BTN2_FOLLOW)
        {
            // Disable Hightlight for button 2
            //ResetHighlightsButton((int)TutorialIndex.BTN1);
            // Enable Highlight for button 3
            //SetHighlightsButton((int)TutorialIndex.BTN2);
            tutorialIndex += 1;
            SetTutorialDialogTexts(TitleTexts.TUTORIAL2_FOLLOW, ContentTexts.TUTORIAL2_FOLLOW, ButtonTexts.GO_ON);
        }
        else if (this.tutorialIndex == TutorialIndex.BTN3)
        {
            // Disable Hightlight for button 3
            //ResetHighlightsButton((int)TutorialIndex.BTN2);
            // Enable Highlight for button 4
            //SetHighlightsButton((int)TutorialIndex.BTN3);
            tutorialIndex += 1;
            SetTutorialDialogTexts(TitleTexts.TUTORIAL3, ContentTexts.TUTORIAL3, ButtonTexts.GO_ON);
        }
        else if (this.tutorialIndex == TutorialIndex.BTN4)
        {
            this.tutorialIndex += 1;
            // Disable Hightlight for button 4
            //ResetHighlightsButton((int)TutorialIndex.BTN4);
            SetTutorialDialogTexts(TitleTexts.TUTORIAL4, ContentTexts.TUTORIAL4, ButtonTexts.GO_ON);
        }
        else if (this.tutorialIndex == TutorialIndex.END)
        {
            SetTutorialDialogTexts(TitleTexts.TUTORIALEND, ContentTexts.TUTORIALEND, ButtonTexts.CONTINUE);
            this.TutorialDialog.AddDefaultButtonsCallbacks();
            GameObject ButtonsParent = transform.GetChild(0).GetChild(1).gameObject;
            foreach (Transform child in ButtonsParent.transform)
            {
                child.GetComponent<Interactable>().enabled = true;
            }
        }
        else return;
    }
}
