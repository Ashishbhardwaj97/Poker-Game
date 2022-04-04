using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using SmartLocalization;

public class Registration : MonoBehaviour
{
    public static Registration instance;

    private string countryUrl;
    public string signUpUrl;
    public string imageUploadUrl;
    public string clubImageUploadUrl;
    public static string getImageUrl;
    public string emailCheckUrl;
    public string userNameCheckUrl;

    public string latLongUrl;

    public string updateFbPopupCountUrl;

    //public string textError;
    public static string uploadedImage;
    public Text countryCodeText;

    public Image profileImage;
   // public Image profileImageTest;
    public GameObject registrationPage;
    public GameObject firstPanel;
    public GameObject secondPanel;
    public GameObject signIn;
    public GameObject registration;    
    public GameObject dropDownCountryPanel;    
    public GameObject countryBtn;    

    public TMP_InputField firstNameInputField, lastNameInputField, cityInputField;
    public string timeZone;
    public string userCity;
    public string userContryCode;

    public TMP_InputField emailInputField;
    public TMP_InputField userNameInputField;
    public TMP_InputField passwordInputField;
    public TMP_InputField repeatPasswordInputField;

    //public InputField emailInputField;
    //public InputField userNameInputField;
    //public InputField passwordInputField;
    //public InputField repeatPasswordInputField;



    public InputField userNameInputFieldDuringFbAndGoogle;
    public Text userNameError;
    public Text userNameSuggestion;
    public GameObject selectUsernameScreen;
    public List<GameObject> contryGeneratedList;

    public int minLengthInPassword = 5;


    [SerializeField]
    public RegistrationInfo registrationInfo;


    public TouchscreenKeyboard mobileKeys;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //SendImage(profileImageTest.sprite.texture);

        signUpUrl = ServerChanger.instance.domainURL + "api/v1/user";
        imageUploadUrl = ServerChanger.instance.domainURL + "api/v1/user/image-upload";
        countryUrl = ServerChanger.instance.domainURL + "api/v1/user/country";
        clubImageUploadUrl = ServerChanger.instance.domainURL + "api/v1/club/club-image-upload";
        emailCheckUrl = ServerChanger.instance.domainURL + "api/v1/user/email-check";
        userNameCheckUrl = ServerChanger.instance.domainURL + "api/v1/user/username-check";
        updateFbPopupCountUrl = ServerChanger.instance.domainURL + "api/v1/user/update_fb_popup_count";


        latLongUrl = ServerChanger.instance.domainURL + "api/v1/user/lat-long-details";

        countryCount = 1;
        contryGeneratedList = new List<GameObject>();
        StartCoroutine(DetectCountry());
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (secondPanel.activeInHierarchy)
            {
                firstPanel.SetActive(true);
                secondPanel.SetActive(false);
            }
            else if (firstPanel.activeInHierarchy)
            {
                registration.SetActive(false);
                signIn.SetActive(true);
            }
        }


        if (emailInputField.isFocused == true)
        {
            registrationPage.transform.GetChild(0).GetChild(1).localPosition = new Vector3(registrationPage.transform.GetChild(0).GetChild(1).localPosition.x, 320, 0);
        }
        if (emailInputField.isFocused == false)
        {
            registrationPage.transform.GetChild(0).GetChild(1).localPosition = new Vector3(registrationPage.transform.GetChild(0).GetChild(1).localPosition.x, 0, 0);
        }
    }

    //public void OnInputEvent()
    //{
    //    mobileKeys = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.default, false);
    //}


    #region  Late Long api

    [Serializable]
    public class LatLong
    {
        public string latitude;
        public string longitude;

    }

    [Serializable]
    public class LatLongResponse
    {
        public bool error;
        public string[] timezone;
        public string countryCode;

    }

    [SerializeField] LatLong latLong;
    [SerializeField] LatLongResponse latLongResponse;
    public void DetectTimezoneAPI()
    {
        latLong.latitude = LatLongScript.instance.latitude.ToString();
        latLong.longitude = LatLongScript.instance.longitude.ToString();
        string body = JsonUtility.ToJson(latLong);
        Debug.Log(body);

        Communication.instance.PostData(latLongUrl, body, DetectTimezoneAPICallback);
    }

    //public string timezone;
    void DetectTimezoneAPICallback(string response)
    {
        print(response);

        if (!string.IsNullOrEmpty(response))
        {
            latLongResponse = JsonUtility.FromJson<LatLongResponse>(response);

            if (!latLongResponse.error)
            {
                Debug.Log("success timezone api");
                
                timeZone = latLongResponse.timezone[0];
                userContryCode = latLongResponse.countryCode;

                Debug.Log("timezone........." + timeZone);
                Debug.Log("country code........." + userContryCode);
            }
        }
    }

    #endregion

    #region Auto fill country code and city

    [Serializable]
    public class Country
    {
        public string city;
        public string countryCode;
        public string timezone;
    }

    public IEnumerator DetectCountry()
    {
        //UnityWebRequest request = UnityWebRequest.Get("https://extreme-ip-lookup.com/json");

#if UNITY_EDITOR || UNITY_ANDROID

        UnityWebRequest request = UnityWebRequest.Get("http://ip-api.com/json");
        request.chunkedTransfer = false;
        yield return request.SendWebRequest();

        if (request.isDone)
        {
            if (!string.IsNullOrEmpty(request.downloadHandler.text))
            {
                Debug.Log("DetectCountry done............." + request.downloadHandler.text);
                Country res = JsonUtility.FromJson<Country>(request.downloadHandler.text);

                countryCodeText.text = res.countryCode;
                cityInputField.text = res.city;
                timeZone = res.timezone;
                userCity = res.city;
                userContryCode = res.countryCode;
            }
        }
        else
        {
            Debug.Log(": Error: " + request.error);
            yield break;
        }

#elif UNITY_IOS 

        while (true)
        {
            if (!LatLongScript.instance.islatlong)
            {
                yield return new WaitForSeconds(1f);
            }
            else
            {
                break;
            }
        }
       
        DetectTimezoneAPI();
        yield return new WaitForSeconds(0f);
        Debug.Log("call lat long api in mobile device.....");

#endif
    }

    #endregion

    [Serializable]
    public class UserNameCheck
    {
        public string username;
    }

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
        public string[] suggesions; 
    }

    [Serializable]
    public class Errors
    {
        public Username username;
    }

    [Serializable]
    public class Username
    {
        public Properties properties;
    }

    [Serializable]
    public class Properties
    {
        public string message;
    }

    [SerializeField] UserNameCheck userNameCheck;
    [SerializeField] EmailCheck emailCheck;
    [SerializeField] EmailCheckResponse emailCheckResponse;

    public void Next()
    {

        if (string.IsNullOrEmpty(firstNameInputField.text) || !firstNameInputField.GetComponent<ValidateInput>().isValidInput)
        {
            firstNameInputField.GetComponent<ValidateInput>().Validate(firstNameInputField.text);
            firstNameInputField.Select();
        }
        //if (string.IsNullOrEmpty(cityInputField.text) || !cityInputField.GetComponent<ValidateInput>().isValidInput)
        //{
        //    cityInputField.GetComponent<ValidateInput>().Validate(cityInputField.text);
        //    cityInputField.Select();
        //}
        if ((string.IsNullOrEmpty(emailInputField.text) || !emailInputField.GetComponent<ValidateInput>().isValidInput))
        {
            emailInputField.GetComponent<ValidateInput>().Validate(emailInputField.text);
            emailInputField.Select();
        }

        else
        {
            if (string.IsNullOrEmpty(firstNameInputField.text))//|| string.IsNullOrEmpty(cityInputField.text))
            {
                print("First name  not be blank");
            }
            else
            {
                emailCheck.email = emailInputField.text.Trim((char)8203);
                string body = JsonUtility.ToJson(emailCheck);
                ClubManagement.instance.loadingPanel.SetActive(true);
                Communication.instance.PostData(emailCheckUrl, body, EmailCheckProcess);
            }
        }
    }

    public Text emailCheckErrorTextFromServer;
    public Text userNameCheckErrorTextFromServer;
    public Text suggestedUserName;

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
                print("Next Validate true");
                firstPanel.SetActive(false);
                secondPanel.SetActive(true);
                if (toggleTick.transform.GetChild(0).gameObject.activeInHierarchy)
                {
                    toggleTick.transform.GetChild(0).gameObject.SetActive(false);
                    isTermCondition = false;
                }
                if (toggleTick1.transform.GetChild(0).gameObject.activeInHierarchy)
                {
                    toggleTick1.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
            else
            {
                emailCheckErrorTextFromServer.text = LanguageManager.Instance.GetTextValue("email already exist");
            }

        }
    }

    public void Back()
    {
        firstPanel.SetActive(true);
        secondPanel.SetActive(false);
        userNameInputField.text = string.Empty;
        passwordInputField.text = string.Empty;
        repeatPasswordInputField.text = string.Empty;
    }

    public void SignIn()
    {
        signIn.SetActive(true);
        registration.SetActive(false);
        firstPanel.SetActive(true);
        secondPanel.SetActive(false);
        ClearInputFields();
    }

    public Texture2D profileDefaultPic;
    public RawImage uploadButton;

    public Image defaultPlayerImage;
    public Image defaultClubImage;
    public Image defaultClubImageWithHeart;

    public void ClearInputFields()
    {
        userNameInputField.text = string.Empty;
        passwordInputField.text = string.Empty;
        repeatPasswordInputField.text = string.Empty;

        if (toggleTick.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            toggleTick.transform.GetChild(0).gameObject.SetActive(false);
            isTermCondition = false;
        }
        if (toggleTick1.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            toggleTick1.transform.GetChild(0).gameObject.SetActive(false);
        }

        uploadButton.texture = profileDefaultPic;

        firstNameInputField.text = string.Empty;
        emailInputField.text = string.Empty;
        lastNameInputField.text = string.Empty;
        cityInputField.text = string.Empty;
        countryCodeText.text = string.Empty;
    }

    public void SignUp()
    {
        if (string.IsNullOrEmpty(userNameInputField.text) || !userNameInputField.GetComponent<ValidateInput>().isValidInput)
        {
            userNameInputField.GetComponent<ValidateInput>().Validate(userNameInputField.text);
            userNameInputField.Select();
        }
        if (string.IsNullOrEmpty(passwordInputField.text) || !passwordInputField.GetComponent<ValidateInput>().isValidInput)
        {
            passwordInputField.GetComponent<ValidateInput>().Validate(passwordInputField.text);
            passwordInputField.Select();
        }
        if (string.IsNullOrEmpty(repeatPasswordInputField.text) || !repeatPasswordInputField.GetComponent<ValidateInput>().isValidInput)
        {
            repeatPasswordInputField.GetComponent<ValidateInput>().Validate(repeatPasswordInputField.text);
            repeatPasswordInputField.Select();
            print("Repeat password...");
        }
        else if (!isTermCondition)
        {
            toggleTick.transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {

            if (string.IsNullOrEmpty(userNameInputField.text) || string.IsNullOrEmpty(passwordInputField.text))
            {
                print("Password varification..Test");
            }
            else
            {
                userNameCheck.username = userNameInputField.text.Trim((char)8203); ;
                string body = JsonUtility.ToJson(userNameCheck);
                print(body);
                ClubManagement.instance.loadingPanel.SetActive(true);
                Communication.instance.PostData(userNameCheckUrl, body, UserNameCheckProcess);
            }
        }
    }

    public void UserNameCheckProcess(string response)
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
                print("Signup Validate true");

                registrationInfo.username = userNameInputField.textComponent.text.Trim((char)8203); 
                registrationInfo.password = passwordInputField.textComponent.text.Trim((char)8203); 
                registrationInfo.first_name = firstNameInputField.textComponent.text.Trim((char)8203); 
                registrationInfo.last_name = lastNameInputField.textComponent.text.Trim((char)8203); 
                registrationInfo.email = emailInputField.textComponent.text.Trim((char)8203); 
                registrationInfo.city = cityInputField.textComponent.text.Trim((char)8203); 
                registrationInfo.country = countryBtn.transform.GetChild(0).GetComponent<Text>().text;
                registrationInfo.user_type = "player";
                registrationInfo.is_social = 0;
                registrationInfo.social_id = "";
                registrationInfo.device_token = PushNotification._push_instance.mytoken;
                registrationInfo.facebook_email = string.Empty;
                registrationInfo.is_facebook_login = false;

                if (string.IsNullOrEmpty(uploadedImage))///0//== "")
                {
                    registrationInfo.user_image = "";
                }
                else
                {
                    registrationInfo.user_image = uploadedImage;
                }
                string body = JsonUtility.ToJson(registrationInfo);
                print(body);
                ClubManagement.instance.loadingPanel.SetActive(true);
                Communication.instance.PostData(signUpUrl, body, SignUpProcess);
            }
            else
            {
                userNameCheckErrorTextFromServer.text = LanguageManager.Instance.GetTextValue("username already exist");
                suggestedUserName.text = emailCheckResponse.suggesions[0] + ",\n" + emailCheckResponse.suggesions[1];
            }
        }
    }

#region Registration with Google or Facebook

    public void ClickBackBtn()
    {
        selectUsernameScreen.SetActive(false);

        userNameInputFieldDuringFbAndGoogle.text = string.Empty;
        userNameError.text = string.Empty;
        userNameSuggestion.text = string.Empty;

        if (GoogleLoginScript.instance.isLoginWithGoogle)
        {
            GoogleLoginScript.instance.isLoginWithGoogle = false;
            GoogleLoginScript.instance.SignOutFromGoogle();
        }
    }
    public void ClickRegisterSubmitBtn()
    {
        if (string.IsNullOrEmpty(userNameInputFieldDuringFbAndGoogle.text)) //|| !userNameInputFieldDuringFbAndGoogle.GetComponent<ValidateInputField>().isValidInput)
        {
            userNameInputFieldDuringFbAndGoogle.GetComponent<ValidateInputField>().Validate(userNameInputFieldDuringFbAndGoogle.text);
            userNameInputFieldDuringFbAndGoogle.Select();
        }
        else
        {
            userNameCheck.username = userNameInputFieldDuringFbAndGoogle.text;
            string body = JsonUtility.ToJson(userNameCheck);
            print(body);
            ClubManagement.instance.loadingPanel.SetActive(true);
            Communication.instance.PostData(userNameCheckUrl, body, UserNameCheckProcess1);

        }
    }

    public void UserNameCheckProcess1(string response)
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
                print("Signup Validate true");

                string username = userNameInputFieldDuringFbAndGoogle.text;

                if (FBLogin.instance.isFbLoginClick)
                {
                    FBLogin.instance.RegisterWithFacebook(username);
                }
                else if (GoogleLoginScript.instance.isLoginWithGoogle)
                {
                    GoogleLoginScript.instance.RegisterWithGoogle(username);
                }
            }
            else
            {
                userNameError.text = LanguageManager.Instance.GetTextValue("username already exist");
                userNameSuggestion.text = emailCheckResponse.suggesions[0] + ",\n" + emailCheckResponse.suggesions[1];
            }
        }
    }

#endregion

    [SerializeField]
    PlayerToken playerToken;

   public void SignUpProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        selectUsernameScreen.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("error");
        }
        else
        {
            playerToken = JsonUtility.FromJson<PlayerToken>(response);
            if (playerToken.error)
            {
                print("sign up error.....");
                
                //print("errors.message...." + playerToken.errors.message);
                if (FBLogin.instance.isFbLoginClick)
                {
                    Cashier.instance.toastMsg.text = "Add Email-ID in your facbook account to login successfully.";
                    Cashier.instance.toastMsgPanel.SetActive(true);
                }
            }
            else
            {
                print("sign up sucessfull....."+response);

                PlayerPrefs.SetString("disclaimerpanel", "1");
                PlayerPrefs.SetInt(SocialGame.instance.VideoCount_SignUp, 0);

                Communication.instance.playerToken = playerToken.token;
                FriendandSocialScript.instance.LoadFriendandPendingList(1);
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

                ApiHitScript.instance.userName.text = playerToken.data.username;
                ApiHitScript.instance.clientId.text = playerToken.data.client_id;

                if (!string.IsNullOrEmpty(getImageUrl))
                {
                    Communication.instance.GetImage(getImageUrl, GetProfilePitctureProcess);
                }
                else
                {
                    ApiHitScript.instance.loginPage.SetActive(false);
                    registrationPage.SetActive(false);

                    Uimanager.instance.homePage.SetActive(true);
                    Uimanager.instance.ClickOnPlay();
                    SocialPokerGameManager.instance.ClickHome();

                    for (int i = 0; i < AccessGallery.instance.profileName.Length; i++)
                    {
                        AccessGallery.instance.profileName[i].text = playerToken.data.username;
                        AccessGallery.instance.profileId[i].text = playerToken.data.client_id;
                    }
                    ApiHitScript.instance.userNameAndId.text = playerToken.data.username + " (" + "ID" + " " + playerToken.data.client_id + ")";
                    for (int i = 0; i < AccessGallery.instance.profileTex.Length; i++)
                    {
                        Texture2D tex = profileDefaultPic;
                        Sprite defautImg = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
                        AccessGallery.instance.profileTex[i].sprite = defautImg;
                    }

                }

                //.........Copy date to player profile......//
                Profile._instance.usernameInputField.text = playerToken.data.username;
                Profile._instance.firstnameInputField.text = playerToken.data.first_name;
                Profile._instance.lastnameInputField.text = playerToken.data.last_name;
                Profile._instance.emailInputField.text = playerToken.data.email;
                Profile._instance.cityInputField.text = playerToken.data.city;

                Profile._instance.user_id = playerToken.data.id;
                Profile._instance.email.text = playerToken.data.email;
                Profile._instance.country.text = playerToken.data.country;
                Profile._instance.countryTextInPersonalInfo.text = playerToken.data.country;
                Profile._instance.city.text = playerToken.data.city;
                Profile._instance.userFullName.text = playerToken.data.first_name + " " + playerToken.data.last_name;
                //.......................................//

                //..............Save User Details.........................//

                ApiHitScript.instance.playerToken.data.username = playerToken.data.username;
                ApiHitScript.instance.playerToken.data.email = playerToken.data.email;
                ApiHitScript.instance.playerToken.data.country = playerToken.data.country;
                ApiHitScript.instance.playerToken.data.city = playerToken.data.city;
                ApiHitScript.instance.playerToken.data.first_name = playerToken.data.first_name;
                ApiHitScript.instance.playerToken.data.last_name = playerToken.data.last_name;
                ApiHitScript.instance.playerToken.data.user_image = playerToken.data.user_image;
                ApiHitScript.instance.playerToken.data.social_chips = playerToken.data.social_chips;
                ApiHitScript.instance.playerToken.data.client_id = playerToken.data.client_id;
                ApiHitScript.instance.playerToken.data.id = playerToken.data.id;
                ApiHitScript.instance.playerToken.data.token = playerToken.data.token;
                ApiHitScript.instance.playerToken.data.device_token_status = playerToken.data.device_token_status;
                ApiHitScript.instance.playerToken.data.is_facebook_login = playerToken.data.is_facebook_login;
                ApiHitScript.instance.playerToken.data.is_google_login = playerToken.data.is_google_login;
                ApiHitScript.instance.playerToken.data.fb_user_image = playerToken.data.fb_user_image;
                ApiHitScript.instance.playerToken.data.google_user_image = playerToken.data.google_user_image;

                ApiHitScript.instance.playerToken.data.token = playerToken.token;
                ApiHitScript.SaveUserData(ApiHitScript.instance.playerToken.data);

                //.........................................//
                ApiHitScript.instance.getUserImageUrl = playerToken.data.user_image;
                ApiHitScript.instance.updatedUserImageUrl = playerToken.data.user_image;
                print("ApiHitScript.instance.updatedUserImageUrl........"+ ApiHitScript.instance.updatedUserImageUrl);
                PlayerPrefs.SetString("SOCIAL_CHIPS", playerToken.data.social_chips);

                for (int i = 0; i < ApiHitScript.instance.socialChips.Count; i++)                                     //Updating Social Chips while Login
                {
                    //ApiHitScript.instance.socialChips[i].text = "$" + playerToken.data.social_chips;
                    ApiHitScript.instance.socialChips[i].text = playerToken.data.social_chips;
                }

                DestroyContryGeneratedList();

                //.............MailBox...............//
                MailBoxScripts._instance.MailBoxCallCount();
                //.......................................//

                if (PlayerPrefs.GetInt(ApiHitScript.User_FirstTime_Key, 0) == 0)
                {
                    PlayerPrefs.SetInt(ApiHitScript.User_FirstTime_Key, ApiHitScript.User_First_Time);
                    //ApiHitScript.instance.tutorialScreen.SetActive(true);
                }
                else
                {
                    ApiHitScript.instance.tutorialScreen.SetActive(false);
                }
                ApiHitScript.instance.CheckFbGoogleConnection(playerToken.data);


                maxFbPopupCount = playerToken.data.max_fb_popup_count;
                currentFbPopupCount = playerToken.data.current_fb_popup_count;
                isFacebookLogin = playerToken.data.is_facebook_login;
                CheckConnectFbPopupUI();
            }
        }
    }

   public void GetProfilePitctureProcess(Sprite profilePic)
    {
        if (profilePic != null)
        {
            profileImage.sprite = profilePic;

            for (int i = 0; i < AccessGallery.instance.profileName.Length; i++)
            {
                AccessGallery.instance.profileName[i].text = playerToken.data.username;
                AccessGallery.instance.profileId[i].text = playerToken.data.client_id;
            }
            ApiHitScript.instance.userNameAndId.text = playerToken.data.username + " (" + "ID" + " " + playerToken.data.client_id + ")";
            for (int i = 0; i < AccessGallery.instance.profileTex.Length; i++)
            {
                AccessGallery.instance.profileTex[i].sprite = profilePic;
            }
            ApiHitScript.instance.loginPage.SetActive(false);
            registrationPage.SetActive(false);
            Uimanager.instance.homePage.SetActive(true);
            SocialPokerGameManager.instance.ClickHome();
        }
    }

    public bool MatchPassword()
    {
        if (passwordInputField.textComponent.text == repeatPasswordInputField.textComponent.text)
        {
            return true;
        }
        else if (!string.IsNullOrEmpty(Login.instance.setNewPassword1InputField.text) && !string.IsNullOrEmpty(Login.instance.setNewPassword2InputField.text))
        {
            if (Login.instance.setNewPassword1InputField.textComponent.text == Login.instance.setNewPassword2InputField.textComponent.text)
            {
                return true;
            }
        }
        return false;
    }
    public void ChangeDropDownListHeight()
    {
        firstPanel.transform.GetChild(10).GetChild(3).GetComponent<RectTransform>().localScale = new Vector3(3f, 3f, 0);
        firstPanel.transform.GetChild(10).GetChild(3).GetComponent<RectTransform>().localPosition = new Vector3(150,-150,0);
    }

    public GameObject toggleTick;
    public GameObject toggleTick1;
    public bool isTermCondition;
    public GameObject signUpButton;
    public void ToggleTick()
    {
        if (toggleTick.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            toggleTick.transform.GetChild(0).gameObject.SetActive(false);
            isTermCondition = false;
        }
        else
        {
            toggleTick.transform.GetChild(0).gameObject.SetActive(true);
            isTermCondition = true;
            toggleTick.transform.GetChild(2).gameObject.SetActive(false);


        }
    }

    public void ToggleTick1()
    {
        if (toggleTick1.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            toggleTick1.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            toggleTick1.transform.GetChild(0).gameObject.SetActive(true);
        }
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
        Communication.instance.PostData(imageUploadUrl, body, PickImageProcess);
    }

    public void SendImageForClub(Texture2D tex)
    {
        imageUpload.file = Communication.instance.GetCurrentImageByte(tex);

        string body = JsonUtility.ToJson(imageUpload);
        Debug.Log(body);
        ClubManagement.instance.loadingPanel.SetActive(true);
        Communication.instance.PostData(clubImageUploadUrl, body, PickImageProcess);
    }

    void PickImageProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (!string.IsNullOrEmpty(response))
        {
            Debug.Log(response);
            uploadImageResponse = JsonUtility.FromJson<UploadImageResponse>(response);

            if (!uploadImageResponse.error)
            {
                getImageUrl = uploadImageResponse.url;
                uploadedImage = uploadImageResponse.image;

                Debug.Log("successful....upload.......image.......");

                if (FBLogin.instance.isFbLoginClick)
                {
                    FBLogin.instance.RegistrationUsingFB();
                }
            }
            else
            {
                Debug.Log("data nhi copy hua.... = ");
            }
        }
    }

#region Country Drop down list

    public GameObject countryPanel;
    public GameObject countryContent;

    private GameObject scrollItemObj;
    public int countryCount;
    [Serializable]
    public class CountryResponseList
    {

        public bool error;
        public Data[] data;

    }
    [Serializable]
    public class Data
    {
        public string country_name;
        public string code;
    }

    [SerializeField] public CountryResponseList countryResponseList;

    string selectedText;
    public void OpenDropdown(Text _text)
    {
        selectedText = _text.text;
        if (countryCount == 1)
        {
            ClubManagement.instance.loadingPanel.SetActive(true);
            Communication.instance.PostData(countryUrl, OpenDropdownProcess);
        }

        if (firstPanel.activeInHierarchy)
        {
            dropDownCountryPanel.SetActive(true);
            dropDownCountryPanel.transform.GetChild(1).GetComponent<InputField>().text = string.Empty;

            if (dropDownCountryPanel.transform.GetChild(2).GetChild(0).GetChild(0).childCount > 2)
            {
                for (int i = 0; i < dropDownCountryPanel.transform.GetChild(2).GetChild(0).GetChild(0).childCount; i++)
                {
                    dropDownCountryPanel.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(i).GetChild(2).GetChild(0).gameObject.SetActive(false);
                    if (_text.text == dropDownCountryPanel.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(i).GetChild(1).GetComponent<Text>().text)
                    {
                        dropDownCountryPanel.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(i).GetChild(2).GetChild(0).gameObject.SetActive(true);
                    }
                }
            }
        }
        else if (Profile._instance.profileEditPanel.activeInHierarchy)
        {
            Profile._instance.dropDownCountryPanel.SetActive(true);
            Profile._instance.dropDownCountryPanel.transform.GetChild(1).GetComponent<InputField>().text = string.Empty;

            if (Profile._instance.dropDownCountryPanel.transform.GetChild(2).GetChild(0).GetChild(0).childCount > 2)
            {
                for (int i = 0; i < Profile._instance.dropDownCountryPanel.transform.GetChild(2).GetChild(0).GetChild(0).childCount; i++)
                {
                    Profile._instance.dropDownCountryPanel.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(i).GetChild(2).GetChild(0).gameObject.SetActive(false);
                    if (_text.text == Profile._instance.dropDownCountryPanel.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(i).GetChild(1).GetComponent<Text>().text)
                    {
                        Profile._instance.dropDownCountryPanel.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(i).GetChild(2).GetChild(0).gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    void OpenDropdownProcess(string response)
    {
        print(response);
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (!string.IsNullOrEmpty(response))
        {
            print(response);
            countryResponseList = JsonUtility.FromJson<CountryResponseList>(response);

            if (!countryResponseList.error)
            {
                if (countryResponseList.data.Length != countryCount)
                {
                    for (int i = countryCount; i < countryResponseList.data.Length; i++)
                    {
                        countryCount++;

                        GenerateCountryItem();
                    }

                    if (SocialProfile._instance.countryScrollView.activeInHierarchy)
                    {
                        for (int i = 0; i < SocialProfile._instance.countryContent.transform.childCount; i++)
                        {
                            SearchingScript.instance.countryListName.Add(SocialProfile._instance.countryContent.transform.GetChild(i));
                        }
                    }
                }
                SearchingScript.instance.listName.Clear();
                if (firstPanel.activeInHierarchy)
                {
                    for (int i = 0; i < countryResponseList.data.Length; i++)
                    {
                        countryContent.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = countryResponseList.data[i].code;
                        countryContent.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = countryResponseList.data[i].country_name + "(" + countryResponseList.data[i].code + ")";
                        SearchingScript.instance.listName.Add(countryContent.transform.GetChild(i));
                    }
                    for (int i = 0; i < dropDownCountryPanel.transform.GetChild(2).GetChild(0).GetChild(0).childCount; i++)
                    {
                        dropDownCountryPanel.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(i).GetChild(2).GetChild(0).gameObject.SetActive(false);
                        if (selectedText == dropDownCountryPanel.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(i).GetChild(1).GetComponent<Text>().text)
                        {
                            dropDownCountryPanel.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(i).GetChild(2).GetChild(0).gameObject.SetActive(true);
                        }
                    }
                }
                else if (Profile._instance.profileEditPanel.activeInHierarchy)
                {
                    for (int i = 0; i < countryResponseList.data.Length; i++)
                    {
                        Profile._instance.countryContent.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = countryResponseList.data[i].code;
                        Profile._instance.countryContent.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = countryResponseList.data[i].country_name + "(" + countryResponseList.data[i].code + ")";
                        SearchingScript.instance.listName.Add(Profile._instance.countryContent.transform.GetChild(i));
                    }
                    for (int i = 0; i < Profile._instance.dropDownCountryPanel.transform.GetChild(2).GetChild(0).GetChild(0).childCount; i++)
                    {
                        Profile._instance.dropDownCountryPanel.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(i).GetChild(2).GetChild(0).gameObject.SetActive(false);
                        if (selectedText == Profile._instance.dropDownCountryPanel.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(i).GetChild(1).GetComponent<Text>().text)
                        {
                            Profile._instance.dropDownCountryPanel.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(i).GetChild(2).GetChild(0).gameObject.SetActive(true);
                        }
                    }
                }

                else if (SocialProfile._instance.selectPreferredCountries.activeInHierarchy)
                {
                    for (int i = 0; i < countryResponseList.data.Length; i++)
                    {
                        //SocialProfile._instance.countryContent.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = countryResponseList.data[i].code;
                        SocialProfile._instance.countryContent.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = countryResponseList.data[i].country_name /*+ "(" + countryResponseList.data[i].code + ")"*/;
                    }
                }
            }
        }
    }

    void GenerateCountryItem()
    {
        if (firstPanel.activeInHierarchy)
        {
            scrollItemObj = Instantiate(countryPanel);
            scrollItemObj.transform.SetParent(countryContent.transform, false);
        }
        else if (Profile._instance.profileEditPanel.activeInHierarchy)
        {
            scrollItemObj = Instantiate(Profile._instance.countryPanel);
            scrollItemObj.transform.SetParent(Profile._instance.countryContent.transform, false);
        }
        else if (CreateClubMain.instance.homePage.transform.GetChild(2).gameObject.activeInHierarchy)
        {
            scrollItemObj = Instantiate(CreateClubMain.instance.countryPanel);
            scrollItemObj.transform.SetParent(CreateClubMain.instance.countryContent.transform, false);
        }
        else if (Admin.instance.adminPanel.transform.GetChild(2).gameObject.activeInHierarchy)
        {
            scrollItemObj = Instantiate(Admin.instance.countryPanel.gameObject);
            scrollItemObj.transform.SetParent(Admin.instance.countryContent, false);
        }
        else if (SocialProfile._instance.countryScrollView.activeInHierarchy)
        {
            scrollItemObj = Instantiate(SocialProfile._instance.countryPanel);
            scrollItemObj.transform.SetParent(SocialProfile._instance.countryContent.transform, false);
        }
        contryGeneratedList.Add(scrollItemObj);
    }

    public void DestroyContryGeneratedList()
    {
        if (contryGeneratedList.Count > 0)
        {
            for (int i = 0; i < contryGeneratedList.Count; i++)
            {
                Destroy(contryGeneratedList[i]);
            }
            contryGeneratedList.Clear();
            countryCount = 1;
        }
    }

    public void CloseDropdown()
    {
        if (firstPanel.activeInHierarchy)
        {
            //firstPanel.transform.GetChild(23).gameObject.SetActive(false);
            //firstPanel.transform.GetChild(24).gameObject.SetActive(false);
            dropDownCountryPanel.SetActive(false);
        }
        else if (Profile._instance.profileEditPanel.activeInHierarchy)
        {
            Profile._instance.dropDownCountryPanel.SetActive(false);
            //MyAccount.instance.personalInfo.transform.GetChild(1).gameObject.SetActive(false);
            //MyAccount.instance.personalInfo.transform.GetChild(2).gameObject.SetActive(false);
        }
        else if (CreateClubMain.instance.homePage.transform.GetChild(2).gameObject.activeInHierarchy)
        {
            CreateClubMain.instance.homePage.transform.GetChild(2).GetChild(13).gameObject.SetActive(false);
            CreateClubMain.instance.homePage.transform.GetChild(2).GetChild(14).gameObject.SetActive(false);
        }
        else if (Admin.instance.adminPanel.transform.GetChild(2).gameObject.activeInHierarchy)
        {
            Admin.instance.contryList.SetActive(false);
            Admin.instance.contryFadeUI.SetActive(false);
        }
    }

    public void SetCountry(Text Object)
    {
        if (firstPanel.activeInHierarchy)
        {
            countryBtn.transform.GetChild(0).GetComponent<Text>().text = Object.text;
            dropDownCountryPanel.SetActive(false);
            //firstPanel.transform.GetChild(23).gameObject.SetActive(false);
            //firstPanel.transform.GetChild(24).gameObject.SetActive(false);
        }
        else if (Profile._instance.profileEditPanel.activeInHierarchy)
        {
            Profile._instance.country.text = Object.text;
            Profile._instance.dropDownCountryPanel.SetActive(false);
        }
        else if (CreateClubMain.instance.homePage.transform.GetChild(2).gameObject.activeInHierarchy)
        {
            CreateClubMain.instance.homePage.transform.GetChild(2).GetChild(12).GetChild(0).GetComponent<Text>().text = Object.text;
            CreateClubMain.instance.homePage.transform.GetChild(2).GetChild(13).gameObject.SetActive(false);
            CreateClubMain.instance.homePage.transform.GetChild(2).GetChild(14).gameObject.SetActive(false);
        }
        else if (Admin.instance.adminPanel.transform.GetChild(2).gameObject.activeInHierarchy)
        {
            Admin.instance.country.text = Object.text;
            Admin.instance.contryList.SetActive(false);
            Admin.instance.contryFadeUI.SetActive(false);
        }
    }

    public string selectCountryText;
    public void SelectCountryBtn()
    {

        if (firstPanel.activeInHierarchy)
        {
            countryBtn.transform.GetChild(0).GetComponent<Text>().text = selectCountryText;
            dropDownCountryPanel.SetActive(false);
        }
        else if (Profile._instance.profileEditPanel.activeInHierarchy)
        {
            Profile._instance.country.text = selectCountryText;
            Profile._instance.dropDownCountryPanel.SetActive(false);
        }
    }
#endregion


#region Check for Connect to facebook popup 

    internal bool isFacebookLogin;

    public int currentFbPopupCount;
    public int maxFbPopupCount;

    public GameObject connectFbPopup;
    public GameObject tickImage;
    public bool isTickImage;
    public void CheckConnectFbPopupUI()
    {
        if (isFacebookLogin)
        {
            connectFbPopup.SetActive(false);
        }
        else
        {
            //......check count for pop up.....//
            if (!PlayerPrefs.HasKey("currentFbPopupCount"))
            {
                PlayerPrefs.SetInt("currentFbPopupCount", currentFbPopupCount);
            }

            //print("count val ........ " + PlayerPrefs.GetInt("currentFbPopupCount", 0));
            //print("maxFbPopupCount val ........ " + maxFbPopupCount);
            if (PlayerPrefs.GetInt("currentFbPopupCount", 0) < maxFbPopupCount)
            {
                connectFbPopup.SetActive(true);
            }
            else
            {
                connectFbPopup.SetActive(false);
            }
        }
    }

    public void ClickOnLaterButton()
    {
        if (isTickImage)
        {
            UpdateCountForConnectFbPopup(maxFbPopupCount);
            PlayerPrefs.SetInt("currentFbPopupCount", maxFbPopupCount);
        }
        else
        {
            currentFbPopupCount = PlayerPrefs.GetInt("currentFbPopupCount", 0) + 1;
            PlayerPrefs.SetInt("currentFbPopupCount", currentFbPopupCount);
            UpdateCountForConnectFbPopup(currentFbPopupCount);
        }
    }

    public void DeleteKeyCurrentFbPopupCount()
    {
        PlayerPrefs.DeleteKey("currentFbPopupCount");
    }

    public void DontShowConnectFbUI(GameObject obj)
    {
        if (!obj.activeInHierarchy)
        {
            isTickImage = true;
            tickImage.SetActive(true);
        }
        else
        {
            isTickImage = false;
            tickImage.SetActive(false);
        }
    }
    [Serializable]
    public class UpdateCountForConnectFbPopupRequest
    {
        public string username;
        public int current_fb_popup_count;
    }
    [Serializable]
    public class UpdateCountForConnectFbPopupResponse
    {
        public bool error;
    }

    [SerializeField] UpdateCountForConnectFbPopupRequest updateCount;
    [SerializeField] UpdateCountForConnectFbPopupResponse updateCountResponse;
    public void UpdateCountForConnectFbPopup(int count)
    {
        updateCount.username = AccessGallery.instance.profileName[0].text;
        updateCount.current_fb_popup_count = count;
        string body = JsonUtility.ToJson(updateCount);
        print(body);
        ClubManagement.instance.loadingPanel.SetActive(true);
        Communication.instance.PostData(updateFbPopupCountUrl, body, UpdateCountCallback);
    }

    void UpdateCountCallback(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            //.....check network connection.....//
        }
        else
        {
            updateCountResponse = JsonUtility.FromJson<UpdateCountForConnectFbPopupResponse>(response);
            if (!updateCountResponse.error)
            {
                connectFbPopup.SetActive(false);
            }
        }
    }

#endregion


    public bool CheckMinCharcterInPassword()
    {
        if (Convert.ToInt32(passwordInputField.text.Length) < minLengthInPassword)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

}


[Serializable]
public class RegistrationInfo
{
    public string first_name;
    public string last_name;
    public string username;
    public string password;
    public string email;
    public string country;
    public string city;
    public string user_type;
    public string user_image;
    public int is_social;
    public string social_id;
    public string device_token;
    public string facebook_email;
    public bool is_facebook_login;
    public bool is_google_login;
    public string fb_user_image;
    public string google_user_image;
}

[Serializable]
public class ImageUpload
{
    public string file;
}





