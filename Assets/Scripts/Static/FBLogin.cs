using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;
using System;

public class FBLogin : MonoBehaviour
{
    public static FBLogin instance;

    public string userExistURL;
    public string loginURL;
    public string registerURL;

    List<string> perms;

    Dictionary<string, object> FBUserDetails;

    public bool isFbLoginClick;

    void Awake()
    {
        instance = this;
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }

    void Start()
    {
        loginURL = ServerChanger.instance.domainURL + "api/v1/user/token";
        registerURL = ServerChanger.instance.domainURL + "api/v1/user";
        userExistURL = ServerChanger.instance.domainURL + "api/v1/user/social-check";

        perms = new List<string>() { "public_profile", "email" };

        //Used afterwards for facebook review ...
       // perms = new List<string>() { "public_profile", "email", "user_friends" };
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

    [Serializable]
    public class CheckUserID
    {
        public string social_id;
        
    }
   
    [Serializable]
    public class UserExistData
    {
        public bool error;
    }


    [SerializeField] public FBData fBData;
    [SerializeField] public CheckUserID checkUserID;

    [SerializeField] UserExistData userExistData;

    [SerializeField] ApiHitScript.PlayerData loginPlayerData;
    [SerializeField] RegistrationInfo registrationData;

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
        
    }

    public void ClickOnFBLogin()
    {
        FB.LogInWithReadPermissions(perms, AuthCallback);
        //StartCoroutine(Registration.instance.DetectCountry());            //........Detect city or country.....//
    }

    

    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            
            var aToken = AccessToken.CurrentAccessToken;
           
            Debug.Log("token ......... id "+aToken.UserId);
            Debug.Log("Token.........."+aToken);
            
            FetchFBProfile();

            foreach (string perm in aToken.Permissions)
            {
                Debug.Log("perm........"+perm);
               
            }


            // used for facebook review ...
            // FriendandSocialScript.instance.isLoginThroughFacebook = true;
            //  FriendandSocialScript.instance.facebookInviteBtn.interactable = true;


        }
        else
        {
            Debug.Log("User cancelled login");

            // used for facebook review ...
            //FriendandSocialScript.instance.facebookInviteBtn.interactable = false;
           // FriendandSocialScript.instance.isLoginThroughFacebook = false;
        }
        FB.Android.RetrieveLoginStatus(LoginStatusCallback);

    }


    private void LoginStatusCallback(ILoginStatusResult result)
    {
        if (!string.IsNullOrEmpty(result.Error))
        {
            Debug.Log("Error: " + result.Error);
        }
        else if (result.Failed)
        {
            Debug.Log("Failure: Access Token could not be retrieved");
        }
        else
        {
            // Successfully logged user in
            // A popup notification will appear that says "Logged in as <User Name>"
            Debug.Log("Success: " + result.AccessToken.UserId);
        }
    }

    private void FetchFBProfile()
    {
        FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);
     
        FB.API("/me?fields=name,first_name,last_name,email", HttpMethod.GET, FetchProfileCallback, new Dictionary<string, string>() { });
        
        
        // used for facebook review ...
       // FriendandSocialScript.instance.GettingFacebookFriendList();

    }

    
    private void FetchProfileCallback(IGraphResult result)
    {
        isFbLoginClick = true;
        Debug.Log("result.RawResult............." + result.RawResult);
        
        FBUserDetails = (Dictionary<string, object>)result.ResultDictionary;

        checkUserID.social_id = FBUserDetails["id"].ToString();
        fBData.id = FBUserDetails["id"].ToString();

        fBData.name = FBUserDetails["name"].ToString();
        fBData.first_name = FBUserDetails["first_name"].ToString();
        

        Debug.Log("Profile: id: " + FBUserDetails["id"].ToString());
        Debug.Log("Profile: name: ............" + FBUserDetails["name"].ToString());
        Debug.Log("Profile: first name: ............" + FBUserDetails["first_name"].ToString());

        try
        {
            fBData.last_name = FBUserDetails["last_name"].ToString();
            Debug.Log("Profile: last name: " + FBUserDetails["last_name"].ToString());
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        try
        {
            fBData.email = FBUserDetails["email"].ToString();

            Debug.Log("Profile: email: " + FBUserDetails["email"].ToString());
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        ClubManagement.instance.loadingPanel.SetActive(true);
        if (!string.IsNullOrEmpty(fBData.email))
        {
            CheckEmailId();
        }
        else
        {
            CheckFacebookUserID();
        }

    }

    public void CheckFacebookUserID()
    {
        string body = JsonUtility.ToJson(checkUserID);
        print("CheckFacebookUserID body : " + body);
        
        Communication.instance.PostData(userExistURL, body, UserExist);
    }

    #region email check api

    [Serializable]
    public class EmailCheck
    {
        public string email;
    }

    [Serializable]
    public class EmailCheckResponse
    {
        public bool error;
        public Errors errors;
    }

    [SerializeField] EmailCheck emailCheck;
    [SerializeField] EmailCheckResponse emailCheckResponse;
    
    public void CheckEmailId()
    {
        emailCheck.email = fBData.email;
        string body = JsonUtility.ToJson(emailCheck);        
        Communication.instance.PostData(Registration.instance.emailCheckUrl, body, EmailCheckProcess);
    }

    void EmailCheckProcess(string response)
    {        
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
                //......please register..... email id is not exist//
                print("please check user fb id");
                CheckFacebookUserID();
            }
            else
            {
                //.....please login.....email id is already exist//
                
                loginPlayerData.username = fBData.email;
                loginPlayerData.password = fBData.id;
                loginPlayerData.device_token = PushNotification._push_instance.mytoken;
                loginPlayerData.is_social = 1;
                loginPlayerData.social_id = fBData.id;
                loginPlayerData.facebook_email = fBData.email;
                loginPlayerData.is_facebook_login = true;

                string body = JsonUtility.ToJson(loginPlayerData);
                Debug.Log("loginPlayerData body......." + body);

                Communication.instance.PostData(loginURL, body, ApiHitScript.instance.LoginCallback);
            }

        }
    }

    #endregion

    public void UserExist(string response)
    {
        userExistData = JsonUtility.FromJson<UserExistData>(response);
        if (userExistData.error)
        {
            print("Response" + response);

            if (!string.IsNullOrEmpty(fBData.email))
            {
                loginPlayerData.username = fBData.email;
            }
            else
            {
                loginPlayerData.username = fBData.first_name + fBData.id.Substring(0, 5);
            }
            loginPlayerData.password = fBData.id;
            loginPlayerData.device_token = PushNotification._push_instance.mytoken;
            loginPlayerData.is_social = 1;
            loginPlayerData.social_id = fBData.id;
            loginPlayerData.facebook_email = fBData.email;
            loginPlayerData.is_facebook_login = true;

            string body = JsonUtility.ToJson(loginPlayerData);
            Debug.Log("loginPlayerData body......." + body);
            ClubManagement.instance.loadingPanel.SetActive(true);
            Communication.instance.PostData(loginURL, body, ApiHitScript.instance.LoginCallback);
        }
        else
        {
            print("Response" + response);

            registrationData.first_name = fBData.first_name;
            if (fBData.last_name != null)
            {
                registrationData.last_name = fBData.last_name;
            }
            else
            {
                registrationData.last_name = "";
            }
            registrationData.password = fBData.id;
            if (!string.IsNullOrEmpty(fBData.email))
            {
                registrationData.email = fBData.email;
                string ext = fBData.email.Substring(0, fBData.email.LastIndexOf("@"));
                registrationData.username = ext;
            }
            else
            {
                registrationData.email = "";
                registrationData.username = fBData.first_name + fBData.id.Substring(0, 5);
            }
            registrationData.user_type = "player";
            registrationData.device_token = PushNotification._push_instance.mytoken;
            registrationData.is_social = 1;
            registrationData.social_id = fBData.id;
            registrationData.facebook_email = registrationData.email;
            registrationData.is_facebook_login = true;

            StartCoroutine(SendImageToServer());
            
        }
    }


    IEnumerator SendImageToServer()
    {
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

            Registration.instance.SendImage(fbProfile);                        //.........upload image on server.......//
        }
        else
        {
            RegistrationUsingFB();
        }
    }

    public void RegistrationUsingFB()
    {

        registrationData.country = Registration.instance.userContryCode;
        registrationData.city = Registration.instance.userCity;

        if (!string.IsNullOrEmpty(Registration.uploadedImage))
        {
            registrationData.user_image = Registration.uploadedImage;
            registrationData.fb_user_image = Registration.getImageUrl;
        }
        else
        {
            registrationData.user_image = "";
        }

        Registration.instance.userNameInputFieldDuringFbAndGoogle.text = registrationData.username;
        Registration.instance.selectUsernameScreen.SetActive(true);
    }

    public void RegisterWithFacebook(string username)
    {
        registrationData.username = username;

        string body = JsonUtility.ToJson(registrationData);
        Debug.Log("registrationData body......." + body);

        ClubManagement.instance.loadingPanel.SetActive(true);
        Communication.instance.PostData(registerURL, body, Registration.instance.SignUpProcess);
    }

    public Texture2D fbProfile;
    bool isProfilePicCallback;
    void DisplayProfilePic(IGraphResult result)
    {
        isProfilePicCallback = true;

        if (result.Texture != null)
        {
            Debug.Log("fb pic found..........");

            fbProfile = result.Texture;

            //ProfilePic.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 512, 512), new Vector2());
            
        }
        else
        {
            Debug.Log("fb pic not found..........");
            
        }

    }
    public void ClickFacebookLogout()
    {
        FB.LogOut();
    }
}
