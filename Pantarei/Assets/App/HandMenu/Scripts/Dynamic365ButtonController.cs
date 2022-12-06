using UnityEngine;

public class Dynamic365ButtonController : IHandMenuButtonController
{
    public static string PantareiAppUri = "pantarei-mr-app";
    //private string StringUriGuides = @"ms-guides://"; //Dynamics365guides URI
    private string StringUriDynamic = "ms-voip-call:?contactids=NoTvAlIdAzUrEiD&returnto=" + PantareiAppUri;

    private void Awake()
    {
        this.RightHandler.Add(this.CallDynamic);
    }

    private void CallDynamic()
    {
        System.Uri Uri = new System.Uri(StringUriDynamic);
#if !UNITY_EDITOR
        Application.OpenURL(StringUriDynamic);
#else
        Debug.Log("Opening Dynamic365: " + PantareiAppUri);
#endif

    }
}
