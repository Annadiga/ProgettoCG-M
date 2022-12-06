using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class ButtonStatusController : MonoBehaviour
{
    private bool isInitialized = false;
    private Interactable BtnInteractable;

    private bool IsActive = true;
    private void Awake()
    {
        if (!isInitialized)
        {
            isInitialized = true;

            BtnInteractable = GetComponent<Interactable>();
        }

        Debug.Log(BtnInteractable);
    }
    public void SetStatus(bool active)
    {
        //iconRenderer.material.color = active ? iconOriginalColor : Color.gray;
        BtnInteractable.IsEnabled = active;
    }

    public void ToggleStatus()
    {
        if (!IsActive) return;

        IsActive = !IsActive;
        SetStatus(IsActive);
    }

}
