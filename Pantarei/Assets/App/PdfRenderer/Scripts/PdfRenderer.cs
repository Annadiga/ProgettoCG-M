using System;
using System.IO;
using System.Threading.Tasks;
using Texts;
using TMPro;
using UnityEngine;

public class PdfRenderer : MonoBehaviour
{
    [SerializeField, Tooltip("Pdf Plane")]
    private GameObject PDFPlane;
    [SerializeField, Tooltip("ErrorDialog")]
    private DialogOneBtnController ErrorDialog;
    [SerializeField, Tooltip("Text where visualize Filename and page")]
    private TextMeshPro Title;
    private string LastTitle = string.Empty;

    private MemoryStream[] files;
    private int index = 0;

    private string LastRenderedPath;

    public string GetLastRendered() { return LastRenderedPath; }

    public async void RenderPath(string path, string title)
    {
        string totPath = String.Format("{0}/{1}", path, title);
        LastTitle = title;
        Stream[] t = await getFromUrl(totPath);
        if (t == null)
        {
            //DialogOneBtnController ErrDialog = Instantiate(ErrorDialog, new Vector3(transform.position.x, transform.position.y, transform.position.z - 42), transform.rotation);
            GameObject parent = transform.parent.gameObject;

            GameObject slate = parent.transform.GetChild(0).gameObject;
            DialogOneBtnController ErrDialog = Instantiate(ErrorDialog, parent.transform);
            ErrDialog.transform.parent = parent.transform;

            ErrDialog.SetTitle(TitleTexts.DEFAULT_ERROR_TITLE);
            ErrDialog.SetContent(ContentTexts.ERROR_FETCHING_PDF);
            ErrDialog.SetBtnText(ButtonTexts.OK);
            ErrDialog.AddDefaultButtonsCallbacks();
            ErrDialog.AddCallback(() =>
            {
                slate.gameObject.SetActive(true);
            });
            slate.gameObject.SetActive(false);
            gameObject.SetActive(false);
            throw new System.Exception("no stream content. please double check the uri");
        }
        files = new MemoryStream[t.Length];

        for (int i = 0; i < t.Length; i++)
        {
            files[i] = new MemoryStream();
            t[i].CopyTo(files[i]);
        }
        index = 0;
        RenderStream(files[index]);
        SetTitle();
        LastRenderedPath = totPath;
    }

    private void SetTitle()
    {
        Title.text = String.Format("{0} Page: {1}", LastTitle, index);
    }

    private void RenderStream(MemoryStream toRender)
    {
        byte[] imgBytes = new byte[16 * 1024];
        imgBytes = toRender.ToArray();

        Texture2D imgTex = new Texture2D(2048, 2048, TextureFormat.BC7, false);
        //Texture2D imgTex = new Texture2D(2, 2);
        //Texture2D imgTex = new Texture2D(2048, 2048, TextureFormat.RGBA32, false);

        imgTex.LoadImage(imgBytes);
        //imgTex.filterMode = FilterMode.Bilinear;
        //imgTex.wrapMode = TextureWrapMode.Clamp;

        MeshRenderer meshRenderer = PDFPlane.GetComponent<MeshRenderer>();
        Material mat = meshRenderer.material;
        mat.SetTexture("_MainTex", imgTex);
        mat.mainTextureScale = new Vector2(1, 1);
    }

    public void GoOnRendering()
    {
        if (index >= files.Length - 1) return;
        index += 1;
        RenderStream(files[index]);
        SetTitle();
    }

    public void GoBackRendering()
    {
        if (index <= 0) return;
        index -= 1;
        RenderStream(files[index]);
        SetTitle();
    }

    private async ValueTask<Stream[]> getFromUrl(string path)
    {
        // exmple string url = "http://localhost:3000/GIBEN/Schema/SCHEMA.zip";

        ZipParser parser = new ZipParser();
        //parser.SetHost("52.140.61.46");
        
        return await parser.GetFiles(path);
    }

}
