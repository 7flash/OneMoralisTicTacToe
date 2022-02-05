using System;
using System.Net;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;

[Serializable]
public class Token
{
    public string token_address;
    public string token_id;
    public string amount;
    public string contract_type;
    public string name;
    public string symbol;
    public string token_uri;
    public string metadata;
    // public DateTime synced_at;
}

[Serializable]
public class Response
{
    public string total;
    public int page;
    public int page_size;
    public string cursor;
    public List<Token> result;
}

[Serializable]
public class Metadata
{
    public string image { get; set; }
    public string name { get; set; }
    public string[] attributes { get; set; }
    public string description { get; set; }
}

public class MoralisTTT : MonoBehaviour
{
    [SerializeField]
    private RawImage img;

    [SerializeField]
    private Material mat1;

    [SerializeField]
    private Material mat2;

    [SerializeField]
    private Material mat3;

    void Start()
    {
        fetchNFTs();   
    }

    private void fetchNFTs() {
        // define random offset
        int offset = UnityEngine.Random.Range(0, 1000);

        string baseUrl = "https://deep-index.moralis.io/api/v2/nft/0xb47e3cd837ddf8e4c57f05d70ab865de6e193bbb?chain=eth&format=decimal&&offset="+offset+"&limit=3";
    
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl);

        WebHeaderCollection headers = new WebHeaderCollection();
        headers.Add("X-API-KEY", "EN02OYLf1mF5hSSlyQd9oo5lMCfixnHfJVvztbxfH8LbZ8fSPOWNVedtl6dCjnL1");

        request.Method = "GET";
        request.Headers = headers;

        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        StreamReader reader = new StreamReader(response.GetResponseStream());

        string jsonResponse = reader.ReadToEnd();

        Response responseObject = JsonUtility.FromJson<Response>(jsonResponse);

        for (int i = 0; i < responseObject.result.Count; i++) {
            print(responseObject.result[i].metadata);

            string responseMetadata = responseObject.result[i].metadata;

            string metadataImage = responseMetadata.Substring(responseMetadata.IndexOf("https://"), responseMetadata.IndexOf(".png") + 4 - responseMetadata.IndexOf("https://"));

            StartCoroutine(DownloadImage(metadataImage, i));
        }
    }

    IEnumerator DownloadImage(string url, int i)
    {
        UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url);
        yield return uwr.SendWebRequest();

        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error While Sending Request: " + uwr.error);
        }
        else
        {
            Texture myTexture = DownloadHandlerTexture.GetContent(uwr);
            // img.texture = myTexture;
            if (i == 0) {
             mat1.mainTexture = myTexture;

            } else if (i == 1) {
                mat2.mainTexture = myTexture;

            } else if (i == 2) {
                mat3.mainTexture = myTexture;

            }
        }
    }
}
