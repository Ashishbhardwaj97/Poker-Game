using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class CreateClubMain : MonoBehaviour
{
    public static CreateClubMain instance;
    [Header("GameObject Reference")]
    public GameObject homePage;   
    public GameObject countryPanel;
    public GameObject countryContent;

    [Header("Image Reference")]
    public Image uploadClubImage;
    [Header("InputField Reference")]
    public InputField GenerateMailInputField;
    //public InputField otp1InputField;
    //public InputField otp2InputField;
    //public InputField otp3InputField;
    //public InputField otp4InputField;      
    public InputField clubNameInputField;
    public InputField cityInputField;
    public TMP_InputField otpInputField;

    public Text clubNameErrorText;
    public Text otpError;
    internal string otp;
    internal string uploadImgURL;
    internal string uploadImgName;
    //private string clubNameCheckUrl;

    [Serializable]
    public class PlayerToken
    {
        public bool error;
        public string token;
        public Data data;
    }

    [Serializable]
    public class Data
    {
        public string username;
        public string client_id;
    }

    [Serializable]
    public class PlayerData
    {
        public string club_name;
        public string country;
        public string city;
        public string upload_logo;
        //...................//

        public string email;
        public string code;

        //...................//
    }

    [Header("Properties")]
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
        //clubNameCheckUrl = ServerChanger.instance.domainURL + "api/v1/club/club-name-check";

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (homePage.transform.GetChild(2).gameObject.activeInHierarchy)
            {
                homePage.transform.GetChild(2).gameObject.SetActive(false);
                homePage.transform.GetChild(1).gameObject.SetActive(true);
            }

            if (homePage.transform.GetChild(3).gameObject.activeInHierarchy)
            {
                homePage.transform.GetChild(3).gameObject.SetActive(false);
                homePage.transform.GetChild(2).gameObject.SetActive(true);
            }

            if (homePage.transform.GetChild(4).gameObject.activeInHierarchy)
            {
                homePage.transform.GetChild(4).gameObject.SetActive(false);
                homePage.transform.GetChild(3).gameObject.SetActive(true);
            }
        }
    }

    public void createClubButton()
    {

        player.club_name = clubNameInputField.text.ToString();
        player.country = homePage.transform.GetChild(2).GetChild(12).GetChild(0).GetComponent<Text>().text;
        player.city = cityInputField.text.ToString();
        if (!string.IsNullOrEmpty(uploadImgName))
        {
            player.upload_logo = uploadImgName;
        }
        else
        {
            player.upload_logo = "";
        }
        string body = JsonUtility.ToJson(player);

        print("test body--> " + body);

        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(JoinClub.instance.createClubUrl, body, CeateclubCallback);
    }

    void CeateclubCallback(string response)
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
                print("login correct..Club Created...");
                CreateClubSuccess();

            }
            else
            {
                print("login incorrect..Club not Created.");
            }
        }
    }

    //...............for Generate OTP...........................//

    public void CreateClubGenerateButton()
    {
        if (string.IsNullOrEmpty(GenerateMailInputField.text) || !GenerateMailInputField.GetComponent<ValidateInput>().isValidInput)
        {
            GenerateMailInputField.GetComponent<ValidateInput>().Validate(GenerateMailInputField.text);
            GenerateMailInputField.Select();
        }
        else
        {
            otpInputField.text = string.Empty;
            player.email = GenerateMailInputField.text.ToString();

            string body = JsonUtility.ToJson(player);

            ClubManagement.instance.loadingPanel.SetActive(true);

            //Communication.instance.PostData(JoinClub.instance.createClubGenrateUrl, body, CeateclubGenerateCallback);
        }
    }

    void CeateclubGenerateCallback(string response)
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
                print("login correct OTP Generated...");
                CreateClubVerify();
            }
            else
            {
                print("login incorrect OTP not Generated...");
                
            }
        }
    }
    //..............for verification............................//

    public void CreateClubVerifyButton()
    {
        //if (string.IsNullOrEmpty(otp1InputField.text) || !otp1InputField.GetComponent<ValidateInput>().isValidInput)
        //{
        //    otp1InputField.GetComponent<ValidateInput>().Validate(otp1InputField.text);
        //    otp1InputField.Select();
        //}
        //else if (string.IsNullOrEmpty(otp2InputField.text) || !otp2InputField.GetComponent<ValidateInput>().isValidInput)
        //{
        //    otp2InputField.GetComponent<ValidateInput>().Validate(otp2InputField.text);
        //    otp2InputField.Select();
        //}
        //else if (string.IsNullOrEmpty(otp3InputField.text) || !otp3InputField.GetComponent<ValidateInput>().isValidInput)
        //{
        //    otp3InputField.GetComponent<ValidateInput>().Validate(otp3InputField.text);
        //    otp3InputField.Select();
        //}
        //else if (string.IsNullOrEmpty(otp4InputField.text) || !otp4InputField.GetComponent<ValidateInput>().isValidInput)
        //{
        //    otp4InputField.GetComponent<ValidateInput>().Validate(otp4InputField.text);
        //    otp4InputField.Select();
        //}
        if (string.IsNullOrEmpty(otpInputField.text) || otpInputField.text.Length < 4)
        {
            otpError.text = "OTP cannot be blank.";
        }

        else
        {
            //otp = otp1InputField.text + otp2InputField.text + otp3InputField.text + otp4InputField.text;
            otp = otpInputField.text;

            player.email = GenerateMailInputField.text.ToString();
            player.code = otp;

            string body = JsonUtility.ToJson(player);
            print(body);

            ClubManagement.instance.loadingPanel.SetActive(true);

            //Communication.instance.PostData(JoinClub.instance.createClubMailVerifyUrl, body, CeateclubVerifyCallback);
        }
    }

    [SerializeField]
    public Cashier.ClaimSendResponse claimSendResponse;

    void CeateclubVerifyCallback(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("Some error in login! Please try after some time.");
        }
        else
        {
            print("response" + response);

            claimSendResponse = JsonUtility.FromJson<Cashier.ClaimSendResponse>(response);

            if (!claimSendResponse.error)
            {
                print("login correct OTP verified...");
                createClubButton();
            }
            else
            {
                Cashier.instance.toastMsg.text = claimSendResponse.errors.club.properties.message;
                Cashier.instance.toastMsgPanel.SetActive(true);
                print("login incorrect OTP not verified...");

            }
        }
    }
    //..........................................//

    [Serializable]
    class ClubNameCheck
    {
        public string club_name;
    }
    
    [Serializable]
    public class ClubNameCheckResponse
{
        public bool error;
        public Errors errors;
    }

    [Serializable]
    public class Errors
    {
        public Club club;
    }

    [Serializable]
    public class Club
    {
        public Properties properties;
    }

    [Serializable]
    public class Properties
    {
        public string message;
    }

    [SerializeField] ClubNameCheck clubNameCheck;
    [SerializeField] ClubNameCheckResponse clubNameCheckResponse;

    public void CreateClubNext()
    {
        if (string.IsNullOrEmpty(clubNameInputField.text) || !clubNameInputField.GetComponent<ValidateInput>().isValidInput)
        {
            clubNameInputField.GetComponent<ValidateInput>().Validate(clubNameInputField.text);
            clubNameInputField.Select();
        }
        else
        {
            clubNameCheck.club_name = clubNameInputField.text;

            string body = JsonUtility.ToJson(clubNameCheck);
            print(body);
            ClubManagement.instance.loadingPanel.SetActive(true);
            //Communication.instance.PostData(clubNameCheckUrl, body, ClubNameCheckProcess);
            
        }
    }

    void ClubNameCheckProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("Some error in login! Please try after some time.");
        }
        else
        {
            print("response" + response);
            clubNameCheckResponse = JsonUtility.FromJson<ClubNameCheckResponse>(response);
            if (!clubNameCheckResponse.error)
            {
                homePage.transform.GetChild(3).gameObject.SetActive(true);
                homePage.transform.GetChild(2).gameObject.SetActive(false);
            }
            else
            {
                clubNameErrorText.text = clubNameCheckResponse.errors.club.properties.message;
            }
        }
    }
    public void CreateClubVerify()
    {
        homePage.transform.GetChild(3).gameObject.SetActive(false);
        homePage.transform.GetChild(4).gameObject.SetActive(true);

    }
    public void CreateClubResend()
    {
        homePage.transform.GetChild(3).gameObject.SetActive(true);
        homePage.transform.GetChild(4).gameObject.SetActive(false);

    }
    public void CreateClubSuccess()
    {
        homePage.transform.GetChild(1).gameObject.SetActive(true);
        homePage.transform.GetChild(0).gameObject.SetActive(true);
        homePage.transform.GetChild(2).gameObject.SetActive(false);
        homePage.transform.GetChild(3).gameObject.SetActive(false);
        homePage.transform.GetChild(4).gameObject.SetActive(false);

        uploadClubImage.sprite = Registration.instance.defaultClubImage.sprite;
        clubNameInputField.text = string.Empty;
        cityInputField.text = string.Empty;
        GenerateMailInputField.text = string.Empty;
        homePage.transform.GetChild(2).GetChild(12).GetChild(0).GetComponent<Text>().text = "US";
    }


    //............Back Process..............//

    public void BacktoCreateClubPage()
    {
        homePage.transform.GetChild(1).gameObject.SetActive(true);
        homePage.transform.GetChild(2).gameObject.SetActive(false);

        clubNameInputField.text = string.Empty;
        cityInputField.text = string.Empty;
        homePage.transform.GetChild(2).GetChild(12).GetChild(0).GetComponent<Text>().text = "US";
        uploadClubImage.sprite = Registration.instance.defaultClubImage.sprite;
        Registration.instance.DestroyContryGeneratedList();
    }

    public void BackEmailVerify()
    {
        homePage.transform.GetChild(3).gameObject.SetActive(false);
        homePage.transform.GetChild(2).gameObject.SetActive(true);

        GenerateMailInputField.text = string.Empty;
    }

    public void BackVerification()
    {
        homePage.transform.GetChild(4).gameObject.SetActive(false);
        homePage.transform.GetChild(3).gameObject.SetActive(true);

        otpInputField.text = string.Empty;
    }
    //.........................................//
}
