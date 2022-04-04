using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Firebase;
using TMPro;
using SmartLocalization;

public class ApiHitScript : MonoBehaviour
{
    public static ApiHitScript instance;

    string tokenUrl; 
    string downloadAppUrl;
    public GameObject bottomPanel;
    public GameObject updatePanel;
    public TMP_InputField userNameInputField;
    public TMP_InputField passwordInputField;

    public GameObject incorrectResponse;
    public GameObject tutorialScreen;
    public GameObject loginPage, clubPage, joinPage;
    public Text userName, clientId;
    public List<Text> socialChips;
    public GameObject mainButtonPanel;
    public Text userNameAndId;
    public Text tokenText;
    private FirebaseApp app;
    

    public List<GameObject> mailBoxSymbol;

    [Serializable]
    public class PlayerData
    {
        public string username;
        public string password;
        public string device_token;
        public int is_social;
        public string social_id;
        public string facebook_email;
        public bool is_facebook_login;
    }

    [SerializeField]
    public PlayerData player;

    [SerializeField]
    public PlayerToken playerToken;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        tokenUrl = ServerChanger.instance.domainURL + "api/v1/user/token";
        downloadAppUrl = ServerChanger.instance.domainURL + "api/v1/user/download-app";
        //....................................................//

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
        updatedUserImageUrl = getUserImageUrl;
        //....................................................//
    }

    public void ClearLoginInputFields()
    {
        userNameInputField.text = string.Empty;
        passwordInputField.text = string.Empty;
    }

    public void SignIn()
    {

        if (string.IsNullOrEmpty(userNameInputField.text)) //|| !userNameInputField.GetComponent<ValidateInput>().isValidInput)
        {
            userNameInputField.GetComponent<ValidateInput>().Validate(userNameInputField.text);
            userNameInputField.Select();
        }
        if (string.IsNullOrEmpty(passwordInputField.text)) //|| !passwordInputField.GetComponent<ValidateInput>().isValidInput)
        {
            passwordInputField.GetComponent<ValidateInput>().Validate(passwordInputField.text);
            passwordInputField.Select();
        }
        if (!string.IsNullOrEmpty(userNameInputField.text) && !string.IsNullOrEmpty(passwordInputField.text))
        {
            player.username = userNameInputField.text.ToString();
            player.password = passwordInputField.text.ToString();
            ClubManagement.instance.userPassword = player.password;
            if (PushNotification._push_instance.mytoken != null)
            {
                player.device_token = PushNotification._push_instance.mytoken;
                tokenText.text = "Token : " + PushNotification._push_instance.mytoken;
            }
            player.is_social = 0;
            player.social_id = "";
            player.facebook_email = string.Empty;
            player.is_facebook_login = false;

            string body = JsonUtility.ToJson(player);
            Debug.Log(body);
            ClubManagement.instance.loadingPanel.SetActive(true);
            Communication.instance.PostData(tokenUrl, body, LoginCallback);
            SocialPokerGameManager.instance.exitPanel.SetActive(false);
        }
    }

    public bool isLogin;
    public void ConnectFromServer()
    {
        print("ConnectFromServer");
        isLogin = true;
        GameManagerScript.instance.socket.url = GameManagerScript.instance.socialPokerUrl;
        GameManagerScript.instance.socket.gameObject.SetActive(true);
        GameManagerScript.instance.networkManager.SetActive(true);
        PokerNetworkManager.instance.SubscribeOnLogin();
        GameSerializeClassesCollection.instance.multipleLogin.username = playerToken.data.username;
        GameSerializeClassesCollection.instance.multipleLogin.token = Communication.instance.playerToken;
        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.multipleLogin);

        StartCoroutine(PokerNetworkManager.instance.ConnectSocialPoker(data));
    }

    public IEnumerator DisconnectFromServer()
    {
        //yield return new WaitForSeconds(1f);
        isLogin = false;
        PokerNetworkManager.instance.UnSubscribeOnLogin();
        //GameManagerScript.instance.socket.Emit("__exit_handle");

        yield return new WaitForSeconds(0.5f);
        GameManagerScript.instance.socket.Close();
        GameManagerScript.instance.networkManager.SetActive(false);
        GameManagerScript.instance.SocketReset();
    }
    int count;
    IEnumerator Delay()
    {
        count = 25;
        while (count >= 0) 
        {
            ClubManagement.instance.loadingPanel.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            count--;
            if(count == 0)
            {
                break;
            }
        }
        ClubManagement.instance.loadingPanel.SetActive(false);
    }

    public string getUserImageUrl = "";
    public string updatedUserImageUrl = "";

    public void LoginCallback(string response)
    {
        if (string.IsNullOrEmpty(response))
        {
            print("Some error in login! Please try after some time.");
            Cashier.instance.toastMsgPanel.SetActive(true);
            Cashier.instance.toastMsg.text = LanguageManager.Instance.GetTextValue("internet check");//SocialProfile._instance.networkErrorMsg;
            //ClubManagement.instance.loadingPanel.SetActive(false);
        }
        else
        {
            print("response" + response);

            playerToken = JsonUtility.FromJson<PlayerToken>(response);


            if (!playerToken.error)
            {
                print("login correct...");
                StartCoroutine(Delay());

                if (playerToken.data.device_token_status)
                {
                    Profile._instance.pushNotificationBtn.transform.GetChild(1).gameObject.SetActive(true);
                    Profile._instance.pushNotificationBtn.transform.GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    Profile._instance.pushNotificationBtn.transform.GetChild(1).gameObject.SetActive(false);
                    Profile._instance.pushNotificationBtn.transform.GetChild(0).gameObject.SetActive(true);
                }
                incorrectResponse.SetActive(false);

                userName.text = playerToken.data.username;
                clientId.text = playerToken.data.client_id;
                PlayerPrefs.SetString("SOCIAL_CHIPS", playerToken.data.social_chips);

                for (int i = 0; i < socialChips.Count; i++)                                     //Updating Social Chips while Login
                {
                    print("Login Time Chips Value: " + PlayerPrefs.GetString("SOCIAL_CHIPS"));
                    //socialChips[i].text = "$" + playerToken.data.social_chips;
                    socialChips[i].text = playerToken.data.social_chips;
                }
                getUserImageUrl = playerToken.data.user_image;
                updatedUserImageUrl = getUserImageUrl;

                Communication.instance.playerToken = playerToken.token;
                FriendandSocialScript.instance.LoadFriendandPendingList(1);
                for (int i = 0; i < ClubManagement.instance.diamondText.Length; i++)
                {
                    ClubManagement.instance.diamondText[i].text = playerToken.data.diamond.ToString();
                }

                for (int i = 0; i < AccessGallery.instance.profileName.Length; i++)
                {
                    AccessGallery.instance.profileName[i].text = playerToken.data.username;
                    AccessGallery.instance.profileId[i].text = playerToken.data.client_id;
                }
                userNameAndId.text = playerToken.data.username + " (" + "ID" + " " + playerToken.data.client_id + ")";

                if (string.IsNullOrEmpty(getUserImageUrl))
                {
                    Uimanager.instance.homePage.SetActive(true);
                    loginPage.SetActive(false);
                    //ClubManagement.instance.loadingPanel.SetActive(false);
                    Uimanager.instance.ClickOnPlay();
                    SocialPokerGameManager.instance.ClickHome();
                    for (int i = 0; i < AccessGallery.instance.profileTex.Length; i++)
                    {
                        Texture2D tex = Registration.instance.profileDefaultPic;
                        Sprite defautImg = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
                        AccessGallery.instance.profileTex[i].sprite = defautImg;
                    }
                }
                else
                {
                    Communication.instance.GetImage(getUserImageUrl, GetUserImageProcess);

                }

                //.........Copy date to player profile......//
                Profile._instance.usernameInputField.text = playerToken.data.username;
                Profile._instance.firstnameInputField.text = playerToken.data.first_name;
                Profile._instance.lastnameInputField.text = playerToken.data.last_name;
                Profile._instance.emailInputField.text = playerToken.data.email;
                Profile._instance.user_id = playerToken.data.id;

                Profile._instance.email.text = playerToken.data.email;
                Profile._instance.country.text = playerToken.data.country;
                Profile._instance.countryTextInPersonalInfo.text = playerToken.data.country;
                Profile._instance.city.text = playerToken.data.city;
                Profile._instance.cityInputField.text = playerToken.data.city;
                Profile._instance.userFullName.text = playerToken.data.first_name + " " + playerToken.data.last_name;
                //........Save user data.....//
                playerToken.data.token = playerToken.token;
                
                if (playerToken.data.user_table_match_preference)
                {
                    PlayerPrefs.SetInt(TableMatch, TableMatch_True);
                }
                else
                {
                    PlayerPrefs.SetInt(TableMatch, TableMatch_False);
                }
                SaveUserData(playerToken.data);
                //...........................//
                if (PlayerPrefs.GetInt(User_FirstTime_Key, 0) == 0)
                {
                    print("LOGINPREFS");
                    PlayerPrefs.SetInt(User_FirstTime_Key, User_First_Time);
                    //tutorialScreen.SetActive(true);
                }
                else
                {
                    print("LOGINPREFS False");
                    //tutorialScreen.SetActive(false);
                }

                //......get tap to spin timer from server............//
                SpinRotate.instance.GetSpinTime();
                CheckFbGoogleConnection(playerToken.data);

                ConnectFromServer();
                Registration.instance.DestroyContryGeneratedList();

                //........Remember me............//
                if (Login.instance.isRememberMe)
                {
                    //print("RememberMe_Credential is save");
                    PlayerPrefs.SetInt(RememberMe_Credential, RememberMe_True);
                    PlayerPrefs.SetString(RememberMe_username, userNameInputField.text.ToString());
                    PlayerPrefs.SetString(RememberMe_password, passwordInputField.text.ToString());
                }
                else
                {
                    //print("RememberMe_Credential is not save");
                    PlayerPrefs.SetInt(RememberMe_Credential, RememberMe_False);
                }

                //....................................//

                Registration.instance.maxFbPopupCount = playerToken.data.max_fb_popup_count;
                Registration.instance.currentFbPopupCount = playerToken.data.current_fb_popup_count;
                Registration.instance.isFacebookLogin = playerToken.data.is_facebook_login;
                Registration.instance.CheckConnectFbPopupUI();
            }
            else
            {
                print("login incorrect...");
                string backendResponse = playerToken.errors.password.properties.message;
                if (backendResponse == "Invalid Username")
                {
                    incorrectResponse.transform.GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("invalid username");
                }
                else if (backendResponse == "Invalid Password")
                {
                    incorrectResponse.transform.GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("invalid password");
                }
                else
                {
                    incorrectResponse.transform.GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("invalid credential");
                }

                incorrectResponse.SetActive(true);
                ClubManagement.instance.loadingPanel.SetActive(false);
            }
            //.............MailBox...............//
            MailBoxScripts._instance.MailBoxCallCount();
            //.......................................//
        }
    }

    public void CheckFbGoogleConnection(Data data)
    {
        if (data.is_facebook_login)
        {
            ConnectToFacebookScript.instance.connectToFbObj.SetActive(false);
            ConnectToFacebookScript.instance.fbAwatarObj.SetActive(true);
            ConnectToFacebookScript.instance.connectToFbInSetting.SetActive(false);
            ConnectToFacebookScript.instance.alreadyConnectToFbInSetting.SetActive(true);

            Communication.instance.GetImage(data.fb_user_image, FbProfilePicCallback);
        }
        else
        {
            ConnectToFacebookScript.instance.connectToFbObj.SetActive(true);
            ConnectToFacebookScript.instance.fbAwatarObj.SetActive(false);
            ConnectToFacebookScript.instance.connectToFbInSetting.SetActive(true);
            ConnectToFacebookScript.instance.alreadyConnectToFbInSetting.SetActive(false);
        }
        if (data.is_google_login)
        {
            ConnectToGoogleScript.instance.connectToGoogleObj.SetActive(false);
            ConnectToGoogleScript.instance.googleAwatarObj.SetActive(true);
            Communication.instance.GetImage(data.google_user_image, GoogleProfilePicCallback);

        }
        else
        {
            ConnectToGoogleScript.instance.connectToGoogleObj.SetActive(true);
            ConnectToGoogleScript.instance.googleAwatarObj.SetActive(false);
        }
    }

    void FbProfilePicCallback(Sprite response)
    {
        if (response != null)
        {
            ConnectToFacebookScript.instance.fbImage.sprite = response;
        }
    }

    void GoogleProfilePicCallback(Sprite response)
    {
        if (response != null)
        {
            ConnectToGoogleScript.instance.googleImage.sprite = response;
        }
    }

    public Image profileImage;
    void GetUserImageProcess(Sprite image)
    {
        //ClubManagement.instance.loadingPanel.SetActive(false);
        if (image != null)
        {
            profileImage.sprite = image;

            for (int i = 0; i < AccessGallery.instance.profileTex.Length; i++)
            {
                AccessGallery.instance.profileTex[i].sprite = image;
            }
            loginPage.SetActive(false);
            Uimanager.instance.homePage.SetActive(true);
            SplashScreenCanvas.SetActive(false);
            Uimanager.instance.ClickOnPlay();
            //SocialPokerGameManager.instance.ClickHome();
            if (PokerSceneManagement.instance.isSceneRestart)
            {
                if (PokerSceneManagement.instance.istour)
                {
                    SocialPokerGameManager.instance.ClickTournament();
                }
                else
                {
                    SocialPokerGameManager.instance.ClickGames();
                }
            }
            else
            {
                SocialPokerGameManager.instance.ClickHome();
            }

            if (PlayerPrefs.HasKey(GalleryAwatar_Pic))
            {
                if (PlayerPrefs.GetInt(GalleryAwatar_Pic) == 1)
                {
                    Profile._instance.profileAvatar[6].GetComponent<Image>().sprite = AccessGallery.instance.profileTex[0].sprite;
                }
                else
                {
                    Profile._instance.profileAvatar[6].GetComponent<Image>().sprite = Profile._instance.galleryDefaultPic;
                }
            }

        }
    }

    #region Save Player Data

    public const string PLAYERKEY_LOGINSTATUS = "isloggedin";
    public const string PLAYERKEY_USERDATA = "userdata";
    public const string User_FirstTime_Key = "userfirsttimekey";
    public const int LOGGED_IN = 1;
    public const int LOGGED_OUT = 2;
    public const int User_First_Time = 1;
    public const int User_Again = 2;

    public const string RememberMe_Credential = "rememberme";
    public const string RememberMe_username = "rememberusername";
    public const string RememberMe_password = "rememberpassword";
    public const int RememberMe_True = 1;
    public const int RememberMe_False = 0;

    public const string GalleryAwatar_Pic = "galleryawatar";
    public const int GalleryAwatar_True = 1;
    public const int GalleryAwatar_False = 0;

    public const string TableMatch = "tablematch";
    public const int TableMatch_True = 1;
    public const int TableMatch_False = 0;

    public Data playerData = new Data();
    public static void SaveUserData(Data userdata)
    {
        print("Save user data");
        PlayerPrefs.SetInt(PLAYERKEY_LOGINSTATUS, LOGGED_IN);
        print("Save user details : \n" + JsonUtility.ToJson(userdata));
        PlayerPrefs.SetString(PLAYERKEY_USERDATA, JsonUtility.ToJson(userdata));
    }
    public void LoadUserData()
    {
        print("Loaduserdata \n" + PlayerPrefs.GetString(PLAYERKEY_USERDATA));
        playerData = JsonUtility.FromJson<Data>(PlayerPrefs.GetString(PLAYERKEY_USERDATA));
        playerToken.data = playerData;

        userName.text = playerData.username;
        clientId.text = playerData.client_id;
        for (int i = 0; i < socialChips.Count; i++)                                     //Updating Social Chips while Login
        {
            //socialChips[i].text = playerToken.data.social_chips.ToString();
            print("Loading Time Chips Value: "+PlayerPrefs.GetString("SOCIAL_CHIPS"));
            //socialChips[i].text = "$" + PlayerPrefs.GetString("SOCIAL_CHIPS");
            socialChips[i].text = PlayerPrefs.GetString("SOCIAL_CHIPS");
        }
        getUserImageUrl = playerData.user_image;
        Communication.instance.playerToken = playerData.token;
        FriendandSocialScript.instance.LoadFriendandPendingList(1);
        for (int i = 0; i < ClubManagement.instance.diamondText.Length; i++)
        {
            ClubManagement.instance.diamondText[i].text = playerData.diamond.ToString();
        }

        for (int i = 0; i < AccessGallery.instance.profileName.Length; i++)
        {
            AccessGallery.instance.profileName[i].text = playerData.username;
            AccessGallery.instance.profileId[i].text = playerData.client_id;
        }
        userNameAndId.text = playerData.username + " (" + "ID" + " " + playerData.client_id + ")";

        if (string.IsNullOrEmpty(getUserImageUrl))// == "")
        {
            Uimanager.instance.homePage.SetActive(true);
            loginPage.SetActive(false);
            ClubManagement.instance.loadingPanel.SetActive(false);
            SplashScreenCanvas.SetActive(false);

            if (PokerSceneManagement.instance.isSceneRestart)
            {
                if (PokerSceneManagement.instance.istour)
                {
                    SocialPokerGameManager.instance.ClickTournament();
                }
                else
                {
                    SocialPokerGameManager.instance.ClickGames();
                }
            }
            else
            {
                SocialPokerGameManager.instance.ClickHome();
            }
        }
        else
        {
            Communication.instance.GetImage(getUserImageUrl, GetUserImageProcess);

        }

        //.........Copy date to player profile......//
        Profile._instance.usernameInputField.text = playerData.username;
        Profile._instance.firstnameInputField.text = playerData.first_name;
        Profile._instance.lastnameInputField.text = playerData.last_name;
        Profile._instance.emailInputField.text = playerData.email;
        Profile._instance.user_id = playerData.id;

        Profile._instance.email.text = playerData.email;
        Profile._instance.country.text = playerData.country;
        Profile._instance.countryTextInPersonalInfo.text = playerData.country;
        Profile._instance.city.text = playerData.city;
        Profile._instance.cityInputField.text = playerData.city;
        Profile._instance.userFullName.text = playerData.first_name + " " + playerData.last_name;

        FavoriteClubScript.instance.ClickOnPlayBtton();
        //Uimanager.instance.PlayerExitThroughLogout(true);           //Auto Login player exit status true.
        //........Mail Box.........//
        MailBoxScripts._instance.MailBoxCallCount();
        //.............................//
        if (PlayerPrefs.GetInt(User_FirstTime_Key, 0) == 0)
        {
            print("PLAYERPREFS");
            PlayerPrefs.SetInt(User_FirstTime_Key, User_First_Time);
            //tutorialScreen.SetActive(true);
        }
        else
        {
            print("PLAYERPREFS FALSE");
            tutorialScreen.SetActive(false);
        }
        //......get tap to spin timer from server............//
        SpinRotate.instance.GetSpinTime();

        CheckFbGoogleConnection(playerData);
        updatedUserImageUrl = getUserImageUrl;

        Registration.instance.maxFbPopupCount = playerData.max_fb_popup_count;
        Registration.instance.currentFbPopupCount = playerData.current_fb_popup_count;
        Registration.instance.isFacebookLogin = playerData.is_facebook_login;
        Registration.instance.CheckConnectFbPopupUI();
       
    }

    public GameObject SplashScreenCanvas;
    public void LoadPlayerData()
    {
        if (PlayerPrefs.GetInt(PLAYERKEY_LOGINSTATUS) == LOGGED_IN)
        {
            ClubManagement.instance.loadingPanel.SetActive(true);
            
            LoadUserData();
        }
        else
        {
            loginPage.SetActive(true);
            SplashScreenCanvas.SetActive(false);
            //Uimanager.instance.PlayerExitThroughLogout(false);

            if (PlayerPrefs.HasKey(RememberMe_Credential))
            {
                if (PlayerPrefs.GetInt(RememberMe_Credential, 0) == 1)
                {
                    userNameInputField.text = PlayerPrefs.GetString(RememberMe_username);
                    passwordInputField.text = PlayerPrefs.GetString(RememberMe_password);
                    Login.instance.ToggleTick();
                }
                else
                {
                    userNameInputField.text = string.Empty;
                    passwordInputField.text = string.Empty;
                }
            }
        }
    }

    void DownloadUserProfilePic()
    {
        string url = playerData.user_image;
        print("Profile pic url " + url);
        if (!string.IsNullOrEmpty(url))
        {
            print("profile data h.....");
            Communication.instance.GetImage(url, GetUserImageProcess);
        }
        else
        {
            print("profile data nhi  h.....");
            ClubManagement.instance.loadingPanel.SetActive(false);
            loginPage.SetActive(false);
            Uimanager.instance.homePage.SetActive(true);
        }
    }


    #endregion

    #region Check App Version Update

    internal bool isCheckAppVersionCallback;
    public bool isAppUpdate;
    public string appDownloadUrl;
    [Serializable]
    public class CheckAppVersionResponse
    {
        public bool error;
        public AppVersionData data;

    }

    [Serializable]
    public class AppVersionData
    {
        public bool error;
        public string app_url;
        public string app_version;
    }

    [SerializeField] CheckAppVersionResponse checkAppVersionResponse;
  

    public void CheckAppVersion()
    {
        print("url..." + downloadAppUrl);
        Communication.instance.PostData(downloadAppUrl, CheckAppVersionCallback);
    }

    void CheckAppVersionCallback(string response)
    {
        isCheckAppVersionCallback = true;

        if (string.IsNullOrEmpty(response))
        {
            //..........check internet connection.........//
        }
        else
        {
            print("CheckAppVersionCallback : " + response);

            checkAppVersionResponse = JsonUtility.FromJson<CheckAppVersionResponse>(response);
            print(checkAppVersionResponse.data.app_version);
            if (!checkAppVersionResponse.error)
            {
                if (!Application.version.Equals(checkAppVersionResponse.data.app_version))
                {
                    isAppUpdate = true;
                    appDownloadUrl = checkAppVersionResponse.data.app_url;

                }
                else
                {
                    isAppUpdate = false;
                }
            }
        }
    }

    public void DownloadNewVersionApp()
    {
        if (!string.IsNullOrEmpty(appDownloadUrl))
        {
            Application.OpenURL(appDownloadUrl);
        }
    }

    #endregion

}



[Serializable]
public class PlayerToken
{
    public bool error;
    public Errors errors;
    public string token;
    public Data data;
    public string statusCode;
}

[Serializable]
public class Data
{
    public string username;
    public string user_type;
    public string email;
    public string country;
    public string city;
    public string first_name;
    public string last_name;
    public string client_id;
    public string user_image;
    public string id;
    public string token;
    public int diamond;
    public string social_chips;
    public bool device_token_status;
    public bool is_facebook_login;
    public bool is_google_login;
    public string fb_user_image;
    public string google_user_image;
    public int current_fb_popup_count;
    public int max_fb_popup_count;
    public bool user_table_match_preference;
}

[Serializable]
public class Errors
{
    public Password password;
}
[Serializable]
public class Password
{
    public Properties properties;
}

[Serializable]
public class Properties
{
    public string message;
}
