using System.Collections;
using System.Collections.Generic;
using Texts;
using UnityEngine;

public class ItemCardModel 
{
    private string title;
    private string extension;

    public ItemCardModel(string title, string extention)
    {
        this.title = title;
        this.extension = extention;
    }

    public ItemCardModel()
    {
        title = TitleTexts.DEFAULT_TITLE;
        extension = "FOLDER";
    }
    public ItemCardModel(string Title)
    {
        title = Title;
        extension = "FOLDER";
    }

    public string Title { get { return title; } }
    public void SetTitle(string t) { title = t; }
    public string Extention { get { return extension; } }
    public void SetExtention(string e) { extension = e; }

    public string GetName()
    {
        return string.IsNullOrEmpty(extension) || IsFolder() ? title : string.Format("{0}.{1}", title, extension);
    }

    public bool IsFolder()
    {
        return string.Equals(Extention.ToUpper(), "FOLDER");
    }
}
