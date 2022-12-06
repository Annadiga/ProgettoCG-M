using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using System.IO.Compression;
using System.IO;
using System.Threading.Tasks;
using System;


public class ZipParser
{
    //[SerializeField] private string serverHost = "localhost";
    //[SerializeField] private int serverPort = 3000;

    //private HttpClient client;

    //public void SetHost(string host)
    //{
    //    serverHost = host;
    //}
    //public void SetPort(int port)
    //{
    //    serverPort = port;
    //}

    //private string BuildBaseUri()
    //{
    //    return string.Format("http://{0}:{1}/", serverHost, serverPort);
    //}
    //private string buildUri(string path)
    //{
    //    return string.Format(path.StartsWith("/") ? "{0}{1}" : "{0}/{1}", BuildBaseUri(), path);
    //}

    private async ValueTask<ZipArchive> getZipAsync(string path)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                string url = ApiUtils.BuildUri(path);
                HttpResponseMessage httpResponseMessage = await client.GetAsync(url);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    Stream content = await httpResponseMessage.Content.ReadAsStreamAsync();

                    ZipArchive archive = new ZipArchive(content, ZipArchiveMode.Read);

                    return archive;
                }
                return null;
            }
        } catch (Exception e)
        {
            Debug.LogError(e);
            return null;
        }
    }

    public async ValueTask<Stream[]> GetFiles(string path)
    {
        ZipArchive archive = await getZipAsync(path);
        if (archive == null) return null;

        int len = archive.Entries.Count;
        Stream[] files = new Stream[len];
        int counter = 0;

        foreach(ZipArchiveEntry entry in archive.Entries) files[counter++] = entry.Open();
        return files;
    }

}
