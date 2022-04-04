using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyAccount : MonoBehaviour
{
    public static MyAccount instance;
    public GameObject profile;
    public GameObject personalInfo;
    public GameObject changePassword;
    public GameObject settings;
    public GameObject aboutUs;
    public GameObject sound;

    public GameObject profileBackButton;

    int soundCount = 1;
    private bool notificationStatus;

    public string ChangePasswordUrl;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        ChangePasswordUrl = ServerChanger.instance.domainURL + "api/v1/user/change-password";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (personalInfo.activeInHierarchy)
            {
                profile.SetActive(true);
                personalInfo.SetActive(false);
            }
            else if (changePassword.activeInHierarchy)
            {
                changePassword.SetActive(false);
                profile.SetActive(true);
            }
            else if (settings.activeInHierarchy)
            {
                settings.SetActive(false);
                profile.SetActive(true);
            }
            else if (aboutUs.activeInHierarchy)
            {
                aboutUs.SetActive(false);
                profile.SetActive(true);
            }
        }

        if (profile.activeInHierarchy)
        {
            profileBackButton.SetActive(false);
        }
        //}
    }


    public void PersonalInfo()
    {
        personalInfo.SetActive(true);
        profile.SetActive(false);
        profileBackButton.SetActive(true);
    }

    public void ChangePassword()
    {
        changePassword.SetActive(true);
        profile.SetActive(false);
        profileBackButton.SetActive(true);
    }

    public void Settings()
    {
        settings.SetActive(true);
        profile.SetActive(false);
        profileBackButton.SetActive(true);
    }

    public void AboutUs()
    {
        aboutUs.SetActive(true);
        profile.SetActive(false);
        profileBackButton.SetActive(true);
    }

    public Image profilePicInPersonelInfo;

    public void Back()
    {
        profile.SetActive(true);
        personalInfo.SetActive(false);
        changePassword.SetActive(false);
        settings.SetActive(false);
        aboutUs.SetActive(false);
        profileBackButton.SetActive(false);

        Profile._instance.editProfilePic.sprite = profilePicInPersonelInfo.sprite;
        Registration.instance.DestroyContryGeneratedList();
    }

    public void SoundImage(GameObject gameObject)
    {
        soundCount++;
        if (soundCount % 2 == 0)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
            GameObject.Find("SoundsEffectsAudioSource").GetComponent<AudioSource>().volume = 0;
        }
        else
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
            GameObject.Find("SoundsEffectsAudioSource").GetComponent<AudioSource>().volume = 1;
        }
    }

    public void NotificationImage(Transform obj)
    {
        if (obj.GetChild(0).gameObject.activeInHierarchy)
        {
            obj.GetChild(1).gameObject.SetActive(true);
            obj.GetChild(0).gameObject.SetActive(false);
            notificationStatus = true;
        }
        else
        {
            obj.GetChild(1).gameObject.SetActive(false);
            obj.GetChild(0).gameObject.SetActive(true);
            notificationStatus = false;
        }

        Profile._instance.ClickNotification(notificationStatus);
    }

    #region Change Password

    public InputField setNewPassword1InputField;
    public InputField setNewPassword2InputField;

    [Serializable]
    public class SetNewPassword
    {
        public string new_password;
        public string confirm_password;
        public string current_password;
    }

    

    [SerializeField] SetNewPassword setNewPassword;
    [SerializeField] ChangePasswordResponse changePasswordResponse;
    public void ChangePasswordSubmitButton()
    {
        if (string.IsNullOrEmpty(setNewPassword1InputField.text) || !setNewPassword1InputField.GetComponent<ValidateInput>().isValidInput)
        {
            setNewPassword1InputField.GetComponent<ValidateInput>().Validate(setNewPassword1InputField.text);
            setNewPassword1InputField.Select();
        }
        else if (string.IsNullOrEmpty(setNewPassword2InputField.text) || !setNewPassword2InputField.GetComponent<ValidateInput>().isValidInput)
        {
            setNewPassword2InputField.GetComponent<ValidateInput>().Validate(setNewPassword2InputField.text);
            setNewPassword2InputField.Select();
        }
        else
        {

            setNewPassword.new_password = setNewPassword1InputField.textComponent.text;
            setNewPassword.confirm_password = setNewPassword2InputField.textComponent.text;
            setNewPassword.current_password = ClubManagement.instance.userPassword;

            string body = JsonUtility.ToJson(setNewPassword);

            ClubManagement.instance.loadingPanel.SetActive(true);
            Communication.instance.PostData(ChangePasswordUrl, body, SetNewPasswordValuesProcess);
            print(body);
        }

    }

    void SetNewPasswordValuesProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("error");
        }
        else
        {
            changePasswordResponse = JsonUtility.FromJson<ChangePasswordResponse>(response);
            if (!changePasswordResponse.error)
            {
                print("success..." + response);

                profile.SetActive(true);
                changePassword.SetActive(false);
            }
            
        }
    }

    public bool MatchPassword()
    {
        if (setNewPassword1InputField.textComponent.text == setNewPassword2InputField.textComponent.text)
        {
            return true;
        }
        return false;
    }

    #endregion
}

[Serializable]
public class ChangePasswordResponse
{
    public bool error;

}
