using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class FileExplorerController : MonoBehaviour
{

    [SerializeField, Tooltip("Item card prefab")]
    private GameObject ItemCardPrefab;

    [SerializeField, Tooltip("GameObjects collection grid")]
    private GameObject GridCollection;

    [SerializeField, Tooltip("Text where visualize Path")]
    private TextMeshPro PathTitle;

    [SerializeField, Tooltip("Renderer prefab")]
    private PdfRenderer PdfRenderer;


    private List<ItemCardController> Items;

    private string ActualPath = "/";

    private bool updated = false;

    public void Awake()
    {
        UpdateGrid(ActualPath);
    }

    public void Update()
    {
        if (updated) return;
        GridCollection.GetComponent<GridObjectCollection>().UpdateCollection();

    }

    public async void UpdateGrid(string path)
    {
        List<ItemCardModel> itemss = await GetFilesInPath(path);
        UpdateCollection(itemss);
        ActualPath = path;
        PathTitle.text = ActualPath;
    }

    public void GoBack()
    {
        if (ActualPath.Equals("/")) return;
        string[] SplitPath = ActualPath.Split("/");
        ActualPath = string.Join("/", SplitPath[0..(SplitPath.Length - 1)]);
        if (string.IsNullOrEmpty(ActualPath)) ActualPath = "/";
        UpdateGrid(ActualPath);
    }

    private void UpdateCollection(List<ItemCardModel> FoldersAndFiles)
    {
        if (FoldersAndFiles == null) return;
        if (Items == null) Items = new List<ItemCardController>();
        else
        {
            for (int i = 0; i < GridCollection.transform.childCount; i++)
            {
                Destroy(GridCollection.transform.GetChild(i).gameObject);
                Items.Clear();
            }
        }

        foreach (ItemCardModel FolderOrFile in FoldersAndFiles)
        {
            GameObject newObj = Instantiate(ItemCardPrefab, GridCollection.transform);
            ItemCardController ItemController = newObj.GetComponent<ItemCardController>();
            ItemController.SetCardInfo(FolderOrFile);
            ItemController.UpdateInfo();

            ItemController.GetComponent<Interactable>().OnClick.AddListener(() =>
            {
                string Path = String.Format("{0}/{1}", ActualPath.Equals("/") ? "" : ActualPath, FolderOrFile.GetName());
                if (FolderOrFile.IsFolder())
                    UpdateGrid(Path);
                else
                {
                    if (PdfRenderer != null
                        && !PdfRenderer.isActiveAndEnabled)
                        RenderFile(ItemController);
                }
            });

            //string Path = String.Format("{0}/{1}", ActualPath.Equals("/") ? "" : ActualPath, FolderOrFile.GetName());
            //if (FolderOrFile.IsFolder())
            //    ItemController.GetComponent<Interactable>().OnClick.AddListener(() => UpdateGrid(Path));
            //else
            //    ItemController.GetComponent<Interactable>().OnClick.AddListener(() => CheckAndRenderPath(ItemController, Path));

            Items.Add(ItemController);
        }
        updated = false;
    }

    private void CheckAndRenderPath(ItemCardController item, string Path)
    {
        if (PdfRenderer != null
            && !PdfRenderer.isActiveAndEnabled
            && !string.Equals(Path, PdfRenderer.GetLastRendered()))
            RenderFile(item);
    }

    private void RenderFile(ItemCardController folderOrFile)
    {
        if (PdfRenderer == null) PdfRenderer = Instantiate(PdfRenderer, transform.position, Quaternion.identity);

        string Path = String.Format("{0}/{1}", ActualPath.Equals("/") ? "" : ActualPath, folderOrFile.GetCardInfo().GetName());
        if (!string.Equals(Path, PdfRenderer.GetLastRendered()))
            PdfRenderer.RenderPath(String.Format("{0}/{1}", ActualPath.Equals("/") ? "" : ActualPath, folderOrFile.GetCardInfo().GetName()));
        
        PdfRenderer.gameObject.SetActive(true);
        PdfRenderer.GetComponent<FollowMeToggle>().SetFollowMeBehavior(true);
    }

    private async ValueTask<List<ItemCardModel>> GetFilesInPath(string path)
    {
        //return MockServer.GetList(path);
        List<ItemCardModel> listToReturn = new List<ItemCardModel>();
        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage httpResponseMessage = await client.GetAsync(ApiUtils.BuildPathUri(path));

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    //string jsonstring = await httpResponseMessage.Content.ReadAsStringAsync();

                    //listToReturn = JsonSerializer.Deserialize<List<ItemCardModel>>(JsonReader.);

                    System.IO.Stream jsonString = await httpResponseMessage.Content.ReadAsStreamAsync();
                    System.IO.StreamReader SR = new System.IO.StreamReader(jsonString);
                    JsonReader r = new JsonTextReader(SR);

                    List<ApiReturnType> ret =  new JsonSerializer().Deserialize<List<ApiReturnType>>(r);
                    foreach (ApiReturnType item in ret)
                    {
                        //Debug.Log(string.Format("{0}, {1}", item.name, item.extension));
                        listToReturn.Add(new ItemCardModel(item.name, item.extension));
                    }
                    return listToReturn;
                }
                return null;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return null;
        }
    }

    private class MockServer
    {
        internal static List<ItemCardModel> GetList(string path)
        {
            if (path == null) return null;

            List<ItemCardModel> list = new List<ItemCardModel>();
            switch (path.Trim())
            {
                case "/":
                    list.Add(new ItemCardModel("GIBEN"));
                    break;
                case "/GIBEN":
                    list.Add(new ItemCardModel("Schema"));
                    list.Add(new ItemCardModel("Ricambi"));
                    break;
                case "/GIBEN/Schema":
                    list.Add(new ItemCardModel("SCHEMA", "zip"));
                    break;
                default:
                    list.Add(new ItemCardModel("Schema"));
                    list.Add(new ItemCardModel("Ricambi"));
                    break;
            }
            return list;

        }
    }
}
