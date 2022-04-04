using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Communication : MonoBehaviour
{
    public static Communication instance;

    public string playerToken;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        
    }
    Texture2D GetTextureCopy(Texture2D source)
    {
        //Create a RenderTexture
        RenderTexture rt = RenderTexture.GetTemporary(
                               source.width,
                               source.height,
                               0,
                               RenderTextureFormat.Default,
                               RenderTextureReadWrite.Linear
                           );

        //Copy source texture to the new render (RenderTexture) 
        Graphics.Blit(source, rt);

        //Store the active RenderTexture & activate new created one (rt)
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;

        //Create new Texture2D and fill its pixels from rt and apply changes.
        Texture2D readableTexture = new Texture2D(source.width, source.height);
        readableTexture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        readableTexture.Apply();

        //activate the (previous) RenderTexture and release texture created with (GetTemporary( ) ..)
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);

        return readableTexture;
    }
    byte[] textureBytes = null;

    public string GetCurrentImageByte(Texture2D img)
    {
        Texture2D imageTexture_copy = GetTextureCopy(img);
        
        textureBytes = imageTexture_copy.EncodeToJPG();

        string myTexturestring = Convert.ToBase64String(textureBytes);
        
        return myTexturestring;

    }


    string response = "";
    public void PostData(string url, string body, System.Action<string> _callback)
    {
        StartCoroutine(PostDataCoroutine(url, body, _callback));

    }

    public IEnumerator PostDataCoroutine(string url, string body, System.Action<string> _callback)
    {
        if (string.IsNullOrEmpty(url))
        {
            Debug.Log(": Player URL NULL.......");
            yield return null;
        }
        else
        {
            UnityWebRequest wwwWebRequest = UnityWebRequest.Post(url, body);

            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(body);
            wwwWebRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            wwwWebRequest.SetRequestHeader("Content-Type", "application/json");
            if (playerToken != "")
            {
                wwwWebRequest.SetRequestHeader("Authorization", "JWT " + playerToken);
            }

            yield return wwwWebRequest.SendWebRequest();

            Debug.Log(": Execute :");

            if (wwwWebRequest.isDone)
            {
                Debug.Log("success");
                response = wwwWebRequest.downloadHandler.text;

                //print("response..........." + response);
                _callback(response);
            }
            else
            {
                Debug.Log(": Error: " + wwwWebRequest.error);
                yield break;
            }
        }

    }

    public void PostData(string url, System.Action<string> _callback)
    {
        StartCoroutine(PostDataCoroutine(url, _callback));

    }

    public IEnumerator PostDataCoroutine(string url, System.Action<string> _callback)
    {
        if (string.IsNullOrEmpty(url))
        {
            Debug.Log(": Player URL NULL.......");
            yield return null;
        }
        else
        {
            UnityWebRequest wwwWebRequest = UnityWebRequest.Post(url, "");

            wwwWebRequest.SetRequestHeader("Content-Type", "application/json");
            if (playerToken != "")
            {
                wwwWebRequest.SetRequestHeader("Authorization", "JWT " + playerToken);
            }

            yield return wwwWebRequest.SendWebRequest();

            Debug.Log(": Execute :");

            if (wwwWebRequest.isDone)
            {
                Debug.Log("success");
                response = wwwWebRequest.downloadHandler.text;

                _callback(response);
            }
            else
            {
                Debug.Log(": Error: " + wwwWebRequest.error);
                yield break;
            }
        }

    }

    internal Sprite imageSprite;

    public void GetImage(string url, System.Action<Sprite> _callback)
    {
        StartCoroutine(GetImageCoroutine(url, _callback));
    }

    Texture2D tex;
    public IEnumerator GetImageCoroutine(string url, System.Action<Sprite> _callback)
    {
        if (string.IsNullOrEmpty(url))
        {
            Debug.Log(": Player URL NULL.......");
            yield return null;
        }
        else
        {
            UnityWebRequest wwwWebRequest = UnityWebRequestTexture.GetTexture(url);

            yield return wwwWebRequest.SendWebRequest();

            Debug.Log(": Execute :");

            if (wwwWebRequest.isDone)
            {
                Debug.Log("success");

                print("" + url);

                try
                {
                    tex = ((DownloadHandlerTexture)(wwwWebRequest.downloadHandler)).texture;
                    imageSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);

                    _callback(imageSprite);
                }
                catch (Exception e)
                {
                    Debug.Log("UN-success");
                    Debug.Log(e.Message);
                    _callback(Registration.instance.defaultPlayerImage.sprite);
                }

                //Texture2D tex = ((DownloadHandlerTexture)(wwwWebRequest.downloadHandler)).texture;

                //imageSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);

                //_callback(imageSprite);
            }
            else
            {
                Debug.Log(": Error: " + wwwWebRequest.error);
                yield break;
            }
        }
    }



    public void GetData(string url, System.Action<string> _callback) {

        StartCoroutine(GetDataCoroutine(url,_callback));

    }



    public IEnumerator GetDataCoroutine(string url,System.Action<string> _callback)
    {
        if (string.IsNullOrEmpty(url))
        {
            Debug.Log(": Player URL NULL.......");
            yield return null;
        }
        else
        {
            using (UnityWebRequest wwwWebRequest = UnityWebRequest.Get(url))
            {
                wwwWebRequest.SetRequestHeader("Content-Type", "application/json");
                if (playerToken != "")
                {
                    wwwWebRequest.SetRequestHeader("Authorization", "JWT " + playerToken);
                }

                yield return wwwWebRequest.SendWebRequest();

                Debug.Log(": Execute :");

                if (wwwWebRequest.isDone)
                {
                    Debug.Log("success");
                    response = wwwWebRequest.downloadHandler.text;

                    _callback(response);
                }
                else
                {
                    Debug.Log(": Error: " + wwwWebRequest.error);
                    yield break;
                }
            }
        }
    }

    public void GetImage(Uri url, Action<Sprite> _callback)
    {
        StartCoroutine(GetImageCoroutine(url, _callback));
    }

    //Texture2D tex;
    public IEnumerator GetImageCoroutine(Uri url, Action<Sprite> _callback)
    {
        if (url == null)
        {
            Debug.Log(": Player URL NULL.......");
            yield return null;
        }
        else
        {
            UnityWebRequest wwwWebRequest = UnityWebRequestTexture.GetTexture(url);

            yield return wwwWebRequest.SendWebRequest();

            Debug.Log(": Execute :");

            if (wwwWebRequest.isDone)
            {
                Debug.Log("success");

                print("" + url);

                try
                {
                    tex = ((DownloadHandlerTexture)(wwwWebRequest.downloadHandler)).texture;
                    imageSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);

                    _callback(imageSprite);
                }
                catch (Exception e)
                {
                    Debug.Log("UN-success");
                    Debug.Log(e.Message);
                    _callback(Registration.instance.defaultPlayerImage.sprite);
                }

            }
            else
            {
                Debug.Log(": Error: " + wwwWebRequest.error);
                yield break;
            }
        }
    }
}
