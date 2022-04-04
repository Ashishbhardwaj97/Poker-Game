using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectToSocialMediaScript : MonoBehaviour
{
    public static ConnectToSocialMediaScript instance;

    public string shareLinkString;
    public string shareLinkMessage;

    public string getLinkUrl;

    [Serializable]
    public class GetLinkResponse
    {
        public bool error;
        public ShareLinkData shareLinkData;
    }

    [Serializable]
    public class ShareLinkData
    {
        public string link;
        public string message;
    }

    [SerializeField] public GetLinkResponse getLinkResponse;

    void Start()
    {
        instance = this;
        getLinkUrl = ServerChanger.instance.domainURL + "api/v1/user/get-link";

        //GetLinkRequest();

    }

    public void GetLinkRequest()
    {
        if (string.IsNullOrEmpty(shareLinkString))
        {
            ClubManagement.instance.loadingPanel.SetActive(true);
            //print(getLinkUrl);
            Communication.instance.GetData(getLinkUrl, GetLinkCallback);
        }
    }

    void GetLinkCallback(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);

        if (!string.IsNullOrEmpty(response))
        {
            print("GetLinkCallback : " + response);
            getLinkResponse = JsonUtility.FromJson<GetLinkResponse>(response);
            
            if (!getLinkResponse.error)
            {
                print("updated successfully.......");
                shareLinkString = getLinkResponse.shareLinkData.link;
                shareLinkMessage = getLinkResponse.shareLinkData.message;
            }
            else
            {
                print("Somthing went wrong.......!!");
            }
        }
    }

}
