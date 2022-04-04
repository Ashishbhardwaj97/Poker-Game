using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class Profile : MonoBehaviour
{
    public static Profile _instance;

    private string notificationUrl;
    public string profileUrl;
    public Text userFullName;
    public InputField usernameInputField;
    //public InputField firstnameInputField;
    //public InputField lastnameInputField;
    //public InputField cityInputField;
    public TMP_InputField firstnameInputField;
    public TMP_InputField lastnameInputField;
    public TMP_InputField cityInputField;
    public InputField emailInputField;
    


    public GameObject profileEditPanel;
    
    public GameObject countryPanel;
    public GameObject countryContent;
    public GameObject pushNotificationBtn;
    public Image editProfilePic;
    public Image editProfilePic1;
    public Image editProfilePic2;
    public Text userName, email, country, city, countryCityInProfilePage;
    public Text countryTextInPersonalInfo;
    public string user_id;

    public GameObject dropDownCountryPanel;
    //public GameObject profileEditPanel;

    public bool isAwaterFromGallery;
    public bool isUploadAwatarFromGallery;
    internal Sprite galleryDefaultPic;

    [Serializable]
    public class PlayerToken
    {
        public bool error;
        public string token;
        public Item item;

    }

    [Serializable]
    public class Item
    {
        public string email;
        public string country;
        public string city;
        public string first_name;
        public string last_name;
    }

    [Serializable]
    public class PlayerData
    {
        public string username;
        public string first_name;
        public string last_name;
        public string email;
        public string country;
        public string city;

        //public string password;
        //public string user_type;
        public string user_image;
    }

    [SerializeField]
    public PlayerData player;

    [SerializeField]
    public PlayerToken playerToken;

    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        profileUrl = ServerChanger.instance.domainURL + "api/v1/user/";
        notificationUrl = ServerChanger.instance.domainURL + "api/v1/user/update-token-status";

        galleryDefaultPic = profileAvatar[6].GetComponent<Image>().sprite;
    }

    public void ClickOnEditProfile()
    {
        editProfilePic1.sprite = editProfilePic2.sprite;
        profileEditPanel.SetActive(true);
    }

    public void ClickUpdateImage()
    {
        SendImage(editProfilePic.sprite.texture);
    }

    public void SendImage(Texture2D tex)
    {
        Registration.instance.imageUpload.file = Communication.instance.GetCurrentImageByte(tex);

        string body = JsonUtility.ToJson(Registration.instance.imageUpload);
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
            Registration.instance.uploadImageResponse = JsonUtility.FromJson<Registration.UploadImageResponse>(response);

            if (!Registration.instance.uploadImageResponse.error)
            {
                Registration.getImageUrl = Registration.instance.uploadImageResponse.url;
                Registration.uploadedImage = Registration.instance.uploadImageResponse.image;
                editProfilePic1.sprite = editProfilePic.sprite;
            }
        }
    }

    public void ProfileUpdateButton()
    {
        if (string.IsNullOrEmpty(firstnameInputField.text))
        {
            firstnameInputField.GetComponent<ValidateInput>().Validate(firstnameInputField.text);
            firstnameInputField.Select();
        }
        else if (string.IsNullOrEmpty(cityInputField.text))
        {
            cityInputField.GetComponent<ValidateInput>().Validate(cityInputField.text);
            cityInputField.Select();
        }

        else
        {
            player.username = usernameInputField.text;
            player.first_name = firstnameInputField.text.ToString();
            player.last_name = lastnameInputField.text.ToString();
            player.email = emailInputField.text.ToString();

            player.country = country.text;
            player.city = cityInputField.text.ToString();

            if (string.IsNullOrEmpty(Registration.uploadedImage))
            {
                string str = ApiHitScript.instance.getUserImageUrl;
                int startIndex = str.LastIndexOf('/') + 1;

                player.user_image = str.Substring(startIndex);
            }
            else
            {
                player.user_image = Registration.uploadedImage;
            }

            string body = JsonUtility.ToJson(player);
            Debug.Log(body);
            string newUrl = "" + profileUrl + user_id;
            ClubManagement.instance.loadingPanel.SetActive(true);
            Communication.instance.PostData(newUrl, body, ProfileCallback);

            if (!string.IsNullOrEmpty(Registration.getImageUrl))
            {
                ClubManagement.instance.loadingPanel.SetActive(true);
                Communication.instance.GetImage(Registration.getImageUrl, GetEditProfilePitctureProcess);
            }
            //Registration.instance.DestroyContryGeneratedList();
        }
    }

    void GetEditProfilePitctureProcess(Sprite editProfilePic)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (editProfilePic != null)
        {
            print("Profile pic updated.....");
            Registration.instance.profileImage.sprite = editProfilePic;
            for (int i = 0; i < AccessGallery.instance.profileTex.Length; i++)
            {
                AccessGallery.instance.profileTex[i].sprite = editProfilePic;
            }
        }
    }

    void ProfileCallback(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("Some error in login! Please try after some time.");
        }
        else
        {
            print("response" + response);

            playerToken = JsonUtility.FromJson<PlayerToken>(response);


            if (!playerToken.error)
            {
                print("login correct...");
                //MyAccount.instance.profile.SetActive(true);
                //MyAccount.instance.personalInfo.SetActive(false);
                profileEditPanel.SetActive(false);
                email.text = player.email;
                country.text = player.country;
                countryTextInPersonalInfo.text = player.country;
                city.text = player.city;

                firstnameInputField.text = player.first_name;
                lastnameInputField.text = player.last_name;
                emailInputField.text = player.email;

                cityInputField.text = player.city;
                userFullName.text = player.first_name + " " + player.last_name;
                countryCityInProfilePage.text = player.city + ", " + player.country;
                //MyAccount.instance.profileBackButton.SetActive(false);

                //.......................................//
                ApiHitScript.instance.playerToken.data.first_name = player.first_name;
                ApiHitScript.instance.playerToken.data.last_name = player.last_name;
                ApiHitScript.instance.playerToken.data.city = player.city;
                ApiHitScript.instance.playerToken.data.country = player.country;
                ApiHitScript.instance.updatedUserImageUrl = Registration.getImageUrl;
                if (string.IsNullOrEmpty(Registration.getImageUrl))
                {
                    ApiHitScript.instance.playerToken.data.user_image = ApiHitScript.instance.getUserImageUrl;
                }
                else
                {
                    ApiHitScript.instance.playerToken.data.user_image = Registration.getImageUrl;
                }
                //.......................................//

                ApiHitScript.SaveUserData(ApiHitScript.instance.playerToken.data);
                print("Save user details : \n" + JsonUtility.ToJson(ApiHitScript.instance.playerToken.data));

                if (isAwaterFromGallery && isUploadAwatarFromGallery)
                {
                    PlayerPrefs.SetInt(ApiHitScript.GalleryAwatar_Pic, ApiHitScript.GalleryAwatar_True);
                }
                else
                {
                    PlayerPrefs.SetInt(ApiHitScript.GalleryAwatar_Pic, ApiHitScript.GalleryAwatar_False);
                }

            }
            else
            {
                print("login incorrect...");
                //MyAccount.instance.profileBackButton.SetActive(true);
            }
        }
    }

    public List<GameObject> profileRing;
    public List<GameObject> profileAvatar;

    public GameObject uploadAndDoneBtn;
    public GameObject uploadBtn;
    public GameObject doneBtn;

    public void SelectAvatar(GameObject obj)
    {
        for (int i = 0; i < profileRing.Count; i++)
        {
            profileRing[i].GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);

            if (profileRing[i].transform.parent.childCount == 2)
            {
                profileRing[i].transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(187, 188);
                profileRing[i].transform.parent.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(60, 60);
                profileRing[i].transform.parent.GetChild(1).localPosition = new Vector3(0, -90, 0);
            }
        }

        for (int i = 0; i < profileAvatar.Count; i++)
        {
            profileAvatar[i].GetComponent<RectTransform>().sizeDelta = new Vector2(187, 188);

        }
        obj.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 250);
        obj.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(237, 238);

        if (obj.transform.parent.childCount == 2)
        {
            obj.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(237, 238);
            obj.transform.parent.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);
            obj.transform.parent.GetChild(1).localPosition = new Vector3(0, -140, 0);

        }
        editProfilePic.sprite = obj.transform.GetChild(0).GetComponent<Image>().sprite;
        //editProfilePic1.sprite = obj.transform.GetChild(0).GetComponent<Image>().sprite;

        if (obj.CompareTag("AccessGallery"))
        {
            if (profileAvatar[6].GetComponent<Image>().sprite.name == "Group 156")
            {
                uploadAndDoneBtn.SetActive(false);
                uploadBtn.SetActive(true);
            }
            else
            {
                uploadAndDoneBtn.SetActive(true);
                uploadBtn.SetActive(false);
                doneBtn.SetActive(false);
            }

            doneBtn.SetActive(false);
            isAwaterFromGallery = true;
        }
        else
        {
            uploadBtn.SetActive(false);
            uploadAndDoneBtn.SetActive(false);
            doneBtn.SetActive(true);
            isAwaterFromGallery = false;
        }

    }

    #region Notification in Profile

    [Serializable]
    public class NotificationRequest
    {
        public string device_token;
        public bool device_token_status;
    }

    [Serializable]
    public class NotificationResponse
    {
        public bool error;
    }

    [SerializeField] NotificationRequest notificationRequest;
    [SerializeField] NotificationResponse notificationResponse;

    public void ClickNotification(bool status)
    {
        if (status)
        {
            notificationRequest.device_token = PushNotification._push_instance.mytoken;
        }
        else
        {
            notificationRequest.device_token = string.Empty;
        }
        notificationRequest.device_token_status = status;
        string body = JsonUtility.ToJson(notificationRequest);
        print(body);

        ClubManagement.instance.loadingPanel.SetActive(true);
        Communication.instance.PostData(notificationUrl, body, NotificationProcess);
    }

    void NotificationProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);

        if (!string.IsNullOrEmpty(response))
        {
            print(response);

            notificationResponse = JsonUtility.FromJson<NotificationResponse>(response);

            if (!notificationResponse.error)
            {
                Debug.Log("update successfull.........");
            }
        }
    }

    #endregion


    Transform scrollContent;
    public void MoveScrollInProfileStatsInfo(ScrollRect scrollRect)
    {

        if ((scrollRect.velocity.x > -50f && scrollRect.velocity.x < 0f) || (scrollRect.velocity.x < 50f && scrollRect.velocity.x > 0f))
        {
            scrollContent = scrollRect.transform.GetChild(0).GetChild(0);

            if (scrollContent.localPosition.x > -600)                                                    //......... 1st scroll position
            {
                scrollContent.localPosition = new Vector3(0, scrollContent.localPosition.y, scrollContent.localPosition.z);
            }
            else if (scrollContent.localPosition.x < -600 && scrollContent.localPosition.x > -1800)      //......... 2nd scroll position
            {
                scrollContent.localPosition = new Vector3(-1170, scrollContent.localPosition.y, scrollContent.localPosition.z);
            }
            else if (scrollContent.localPosition.x < -1800)                                              //......... 3rd scroll position
            {
                scrollContent.localPosition = new Vector3(-2320, scrollContent.localPosition.y, scrollContent.localPosition.z);
            }
        }
    }
}