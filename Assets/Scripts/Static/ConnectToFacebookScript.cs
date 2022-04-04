using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;
using System;
public class ConnectToFacebookScript : MonoBehaviour
{
    public static ConnectToFacebookScript instance;

    public Image fbImage;

    public GameObject connectToFbObj;
    public GameObject fbAwatarObj;

    public GameObject connectToFbInSetting;
    public GameObject alreadyConnectToFbInSetting;

    internal Texture2D fbProfile;

    private string connectFbUrl;
    private string fbEmailCheckUrl;
    private string fbImgUrl;

    private bool isProfilePicCallback;
    private bool isSendImgToServerCallback;

    List<string> perms;
    Dictionary<string, object> FBUserDetails;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        connectFbUrl = ServerChanger.instance.domainURL + "api/v1/user/update-fb-login-details";
        fbEmailCheckUrl = ServerChanger.instance.domainURL + "api/v1/user/check-fb-email";

        perms = new List<string>() { "public_profile", "email" };
    }

    [Serializable]
    public class FBData
    {
        public string id;
        public string name;
        public string email;
        public string first_name;
        public string last_name;
    }

    [SerializeField] public FBData fBData;

    
    public void ClickConnectToFb()
    {
        ClubManagement.instance.loadingPanel.SetActive(true);
        FB.LogInWithReadPermissions(perms, AuthCallback);

    }

    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            var aToken = AccessToken.CurrentAccessToken;

            FetchFBProfile();
        }        
    }

    private void FetchFBProfile()
    {
        FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);
        FB.API("/me?fields=name,first_name,last_name,email", HttpMethod.GET, FetchProfileCallback, new Dictionary<string, string>() { });
    }

    
    void DisplayProfilePic(IGraphResult result)
    {
        isProfilePicCallback = true;

        if (result.Texture != null)
        {
            //Debug.Log("fb pic found..........");            
            fbProfile = result.Texture;            
        }
        
    }
    private void FetchProfileCallback(IGraphResult result)
    {
        FBLogin.instance.isFbLoginClick = true;

        Debug.Log("result.RawResult............." + result.RawResult);

        FBUserDetails = (Dictionary<string, object>)result.ResultDictionary;

        fBData.id = FBUserDetails["id"].ToString();

        try
        {
            fBData.email = FBUserDetails["email"].ToString();
            
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        ClubManagement.instance.loadingPanel.SetActive(true);

        StartCoroutine(CheckEmailId());        
    }

    #region Send image to server

    IEnumerator SendImageToServer()
    {
        //........wait for fb profile pic api......//
        while (true)
        {
            if (!isProfilePicCallback)
            {
                yield return new WaitForSeconds(1f);
            }
            else
            {
                break;
            }
        }

        if (fbProfile != null)
        {
            SendImage(fbProfile);                        //.........upload image on server.......//
        }

        //........wait for image upload......//

        while (true)
        {
            if (!isSendImgToServerCallback)
            {
                yield return new WaitForSeconds(1f);
            }
            else
            {
                break;
            }
        }

        //........Now connect to fb request......//
        ConnectToFbRequest();
    }

    [Serializable]
    public class UploadImageResponse
    {
        public bool error;
        public string url;
        public string image;
    }

    [SerializeField]
    public UploadImageResponse uploadImageResponse;

    [SerializeField]
    public ImageUpload imageUpload;

    public void SendImage(Texture2D tex)
    {
        imageUpload.file = Communication.instance.GetCurrentImageByte(tex);

        string body = JsonUtility.ToJson(imageUpload);
        Debug.Log(body);
        ClubManagement.instance.loadingPanel.SetActive(true);
        Communication.instance.PostData(Registration.instance.imageUploadUrl, body, PickImageProcess);
    }
    void PickImageProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        isSendImgToServerCallback = true;

        if (!string.IsNullOrEmpty(response))
        {
            Debug.Log(response);
            uploadImageResponse = JsonUtility.FromJson<UploadImageResponse>(response);

            if (!uploadImageResponse.error)
            {
                fbImgUrl = uploadImageResponse.url;
                Communication.instance.GetImage(fbImgUrl, GetProfilePitctureProcess);
            }
            else
            {
                Debug.Log("some error in upload image");
            }
        }
    }

    public void GetProfilePitctureProcess(Sprite profilePic)
    {
        if (profilePic != null)
        {
            fbImage.sprite = profilePic;
            connectToFbObj.SetActive(false);
            fbAwatarObj.SetActive(true);

            alreadyConnectToFbInSetting.SetActive(true);
            connectToFbInSetting.SetActive(false);
            SocialProfile._instance.SocialChips();
        }
    }

    #endregion

    #region Facebook Email check API

    [Serializable]
    public class EmailCheck
    {
        public string facebook_email;
    }

    [Serializable]
    public class EmailCheckResponse
    {
        public bool error;
        public Errors errors;

    }

    [Serializable]
    public class Errors
    {
        public string message;
    }

    [SerializeField] EmailCheck emailCheck;
    [SerializeField] EmailCheckResponse emailCheckResponse;
    public IEnumerator CheckEmailId()
    {
        emailCheck.facebook_email = fBData.email;
        string body = JsonUtility.ToJson(emailCheck);

        try
        {
            ClubManagement.instance.loadingPanel.SetActive(true);
            Communication.instance.PostData(fbEmailCheckUrl, body, EmailCheckProcess);
        }
        catch (Exception e)
        {
            Debug.Log("CheckEmailId : " + e.Message);
        }

        yield return null;
    }

    void EmailCheckProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("error");
        }
        else
        {
            print(response);
            emailCheckResponse = JsonUtility.FromJson<EmailCheckResponse>(response);

            if (!emailCheckResponse.error)
            {
                //email id is not exist//
                StartCoroutine(SendImageToServer());
            }
            else
            {
                //email id is already exist//
                Cashier.instance.toastMsgPanel.SetActive(true);
                Cashier.instance.toastMsg.text = emailCheckResponse.errors.message;
                
            }
        }
    }

    #endregion

    #region Connect Facebook API

    [Serializable]
    public class ConnectFbRequest
    {
        public string username;
        public bool is_facebook_login;
        public string facebook_email;
        public string fb_user_image;

    }

    [Serializable]
    public class ConnectFbResponse
    {
        public bool error;
    }


    [SerializeField] public ConnectFbRequest connectFbRequest;
    [SerializeField] public ConnectFbResponse connectFbResponse;

    void ConnectToFbRequest()
    {
        connectFbRequest.username = Profile._instance.usernameInputField.text;
        connectFbRequest.is_facebook_login = true;
        connectFbRequest.facebook_email = fBData.email;
        connectFbRequest.fb_user_image = fbImgUrl;
        
        string body = JsonUtility.ToJson(connectFbRequest);
        print(body);
        Communication.instance.PostData(connectFbUrl, body, ConnectToFbCallback);
    }

    void ConnectToFbCallback(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);

        if (!string.IsNullOrEmpty(response))
        {
            print("ConnectToFbCallback : " + response);
            connectFbResponse = JsonUtility.FromJson<ConnectFbResponse>(response);

            if (!connectFbResponse.error)
            {
                //print("updated successfully.......");

                ApiHitScript.instance.playerToken.data.is_facebook_login = true;                
                ApiHitScript.instance.playerToken.data.fb_user_image = fbImgUrl;
                
                ApiHitScript.SaveUserData(ApiHitScript.instance.playerToken.data);
                Registration.instance.connectFbPopup.SetActive(false);

            }
        }
    }
    #endregion
}
