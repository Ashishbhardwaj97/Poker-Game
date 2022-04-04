using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Google;
using UnityEngine;
using UnityEngine.UI;

public class ConnectToGoogleScript : MonoBehaviour
{
    public static ConnectToGoogleScript instance;

    public Image googleImage;
    public GameObject connectToGoogleObj;
    public GameObject googleAwatarObj;


    private string connectgoogleUrl;
    private string googleEmailCheckUrl;

    private string idToken;
    private string email;
    private string googleImgUrl;

    Uri profilepic;

    

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        connectgoogleUrl = ServerChanger.instance.domainURL + "api/v1/user/connect-to-google";
        googleEmailCheckUrl = ServerChanger.instance.domainURL + "api/v1/user/check-google-email";
    }

    public void ConnectWithGoogle()
    {
        ClubManagement.instance.loadingPanel.SetActive(true);
        GoogleLoginScript.instance.isLoginWithGoogle = true;
        OnSignIn();
        
    }
    private void OnSignIn()
    {
        if (GoogleSignIn.Configuration == null)
        {
            GoogleSignIn.Configuration = GoogleLoginScript.instance.configuration;
            GoogleSignIn.Configuration.UseGameSignIn = false;
            GoogleSignIn.Configuration.RequestIdToken = true;
            AddToInformation("Calling SignIn");
        }
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);

    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                    AddToInformation("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    AddToInformation("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            AddToInformation("Canceled");
        }
        else
        {
            try
            {
                idToken = task.Result.IdToken;
                AddToInformation("Google ID Token = " + task.Result.IdToken);
            }
            catch (Exception e)
            {
                Debug.Log("UN-success 3");
                Debug.Log(e.Message);
            }

            try
            {
                email = task.Result.Email;
                AddToInformation("Email = " + task.Result.Email);
            }
            catch (Exception e)
            {
                Debug.Log("UN-success 2");
                Debug.Log(e.Message);
            }

            try
            {
                profilepic = task.Result.ImageUrl;
                AddToInformation("profile pic found...........");

            }
            catch (Exception e)
            {
                Debug.Log("UN-success Profile Pic");
                Debug.Log(e.Message);
            }

            try
            {
                SignInWithGoogleOnFirebase(task.Result.IdToken);
            }
            catch (Exception e)
            {
                Debug.Log("UN-success 4");
                Debug.Log(e.Message);
            }

        }
    }
    private void SignInWithGoogleOnFirebase(string idToken)
    {

        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

        GoogleLoginScript.instance.auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {

            AggregateException ex = task.Exception;
            if (ex != null)
            {
                if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
                    AddToInformation("\nError code = " + inner.ErrorCode + " Message = " + inner.Message);
            }
            else
            {
                AddToInformation("Sign In Successful.");
                UnityMainThreadDispatcher.Instance().Enqueue(CheckEmailId());

            }
        });
    }

    #region Google email check API

    [Serializable]
    public class EmailCheck
    {
        public string google_email;
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

        emailCheck.google_email = email;
        string body = JsonUtility.ToJson(emailCheck);

        try
        {
            ClubManagement.instance.loadingPanel.SetActive(true);
            Communication.instance.PostData(googleEmailCheckUrl, body, EmailCheckProcess);
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
                StartCoroutine(ProfilePictureCorotine());
            }
            else
            {
                //email id is already exist//
                Cashier.instance.toastMsgPanel.SetActive(true);
                Cashier.instance.toastMsg.text = emailCheckResponse.errors.message;
                GoogleLoginScript.instance.SignOutFromGoogle();
            }

        }

    }

    #endregion

    public IEnumerator ProfilePictureCorotine()
    {
        GetProfilePicture(profilepic);
        ClubManagement.instance.loadingPanel.SetActive(true);
        yield return null;
    }

    void GetProfilePicture(Uri profilePic)
    {
        Communication.instance.GetImage(profilePic, GetProfilePictureCallback);

    }

    void GetProfilePictureCallback(Sprite response)
    {
        if (response != null)
        {
            SendImage(response.texture);                        //.........upload image on server.......//
        }
    }

    #region Send image to server

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

        if (!string.IsNullOrEmpty(response))
        {
            Debug.Log(response);
            uploadImageResponse = JsonUtility.FromJson<UploadImageResponse>(response);

            if (!uploadImageResponse.error)
            {
                googleImgUrl = uploadImageResponse.url;
                Communication.instance.GetImage(googleImgUrl, GetProfilePitctureProcess);

                //.........Request to Google connect......//
                ConnectToGoogleRequest();
            }
            else
            {
                Debug.Log("some error in upload image");
            }
        }
    }

    public void GetProfilePitctureProcess(Sprite profilePic)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (profilePic != null)
        {
            googleImage.sprite = profilePic;

            connectToGoogleObj.SetActive(false);
            googleAwatarObj.SetActive(true);
        }
    }

    #endregion

    #region Connect With Google API

    [Serializable]
    public class ConnectGoogleRequest
    {
        public string username;
        public bool is_google_login;
        public string google_email;
        public string google_user_image;
    }

    [Serializable]
    public class ConnectGoogleResponse
    {
        public bool error;
    }

    [SerializeField] public ConnectGoogleRequest connectGoogleRequest;
    [SerializeField] public ConnectGoogleResponse connectGoogleResponse;

    void ConnectToGoogleRequest()
    {
        connectGoogleRequest.username = Profile._instance.usernameInputField.text;
        connectGoogleRequest.is_google_login = true;
        connectGoogleRequest.google_email = email;
        connectGoogleRequest.google_user_image = googleImgUrl;
        
        string body = JsonUtility.ToJson(connectGoogleRequest);
        print(body);
        Communication.instance.PostData(connectgoogleUrl, body, ConnectToGoogleCallback);
    }

    void ConnectToGoogleCallback(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);

        if (!string.IsNullOrEmpty(response))
        {
            print("Connect To Google Callback : " + response);
            connectGoogleResponse = JsonUtility.FromJson<ConnectGoogleResponse>(response);

            if (!connectGoogleResponse.error)
            {
                //print("updated successfully.......");

                ApiHitScript.instance.playerToken.data.is_google_login = true;
                ApiHitScript.instance.playerToken.data.google_user_image = googleImgUrl;

                ApiHitScript.SaveUserData(ApiHitScript.instance.playerToken.data);

            }
        }
    }

    #endregion

    private void AddToInformation(string str)
    {
        if (str != null)
        {
            Debug.Log("str.......... = " + str);
        }
    }
}
