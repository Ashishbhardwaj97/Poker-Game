using SmartLocalization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SocialProfile : MonoBehaviour
{
    public static SocialProfile _instance;

    private string userStatsUrl;
    private string languageUrl;
    private string buyChipsUrl;
    private string updateCountriesOnServerUrl;
    private string ChangePasswordUrl;
    private string updatedSocialChipsUrl;

    [Header("GameObject Reference")]
    public GameObject selectPreferredCountries;
    public GameObject selectPreferredLanguages;
    public GameObject changeLanguageScreen;
    public GameObject countryScrollView;
    public GameObject countryContent;
    public GameObject countryPanel;
    public GameObject languageScrollView;
    public GameObject languageContent;
    public GameObject languagePanel;
    public GameObject changeLanguageContent;
    public GameObject changePassword;
    public GameObject changeLanguage;
    public GameObject buyChips;

    public List<GameObject> languageGeneratedList;

    [Header("Text Reference")]
    public Text totalGamePlayed;
    public Text totalTourney;
    public Text totalWinnings;
    public Text vpip;
    public Text prf;
    public Text cBet;
    public Text threeBet;
    public Text tourvpip;
    public Text tourprf;
    public Text tourcBet;
    public Text tourthreeBet;
    public Text totalvpip;
    public Text totalprf;
    public Text totalcBet;
    public Text totalthreeBet;
    public Text username;
    public Text userID;
    public Text stateAndCountry;
    public Text chipBalance;
    public Text preferredCountryText;
    public Text preferredLanguageText;
    public Text changeLanguageText;

    [Header("InputField Reference")]
    public InputField setNewPassword1InputField;
    public InputField setNewPassword2InputField;
    public InputField currentPasswordInputField;
    public InputField countriesInputField;
    public InputField languageInputField;
    public InputField changeLanguageInputField;

    public int languageCount;
    public string networkErrorMsg;

    [Serializable]
    public class UpdateProfileInfo
    {
        public string username;
    }

    [SerializeField]
    public UpdateProfileInfo profileInfo;

    [Serializable]
    public class PlayerStats
    {
        public string totalGame;
        public string totalTour;
        public string totalWin;
        public float vpip;
        public float prf;
        public float cbet;
        public float threeBet;
        public float vpip_tournament;
        public float pfr_tournament;
        public float cbet_tournament;
        public float threeBet_tournament;
        public float vpipTotal;
        public float pfrTotal;
        public float cbetTotal;
        public float threeBetTotal;
        public List<string> preferred_country;
        public List<string> preferred_language;
        public int statusCode;
        public bool error;
    }

    [SerializeField]
    public PlayerStats player;

    [Serializable]
    public class SelectedCountriesInfo
    {
        public List<string> preferred_country;
    }

    [SerializeField]
    public SelectedCountriesInfo countriesInfo;

    [Serializable]
    public class SelectedLanguagesInfo
    {
        public List<string> preferred_language;
    }

    [SerializeField]
    public SelectedLanguagesInfo languagesInfo;

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        userStatsUrl = ServerChanger.instance.domainURL + "api/v1/user/user-Statistics";
        languageUrl = ServerChanger.instance.domainURL + "api/v1/user/language-list";
        buyChipsUrl = ServerChanger.instance.domainURL + "api/v1/user/add-social-chips";
        updateCountriesOnServerUrl = ServerChanger.instance.domainURL + "api/v1/user/update-social-user";
        ChangePasswordUrl = ServerChanger.instance.domainURL + "api/v1/user/change-password";
        updatedSocialChipsUrl = ServerChanger.instance.domainURL + "api/v1/user/social-chips";

        languageCount = 1;
        //networkErrorMsg = LanguageManager.Instance.GetTextValue("internet check");
    }


    #region Profile Information

    public void ProfileButton()
    {
        UpdateUserInfo();

        profileInfo.username = username.text;
        string body = JsonUtility.ToJson(profileInfo);
        ClubManagement.instance.loadingPanel.SetActive(true);
        Communication.instance.PostData(userStatsUrl, body, ProfileCallback);
    }

    void UpdateUserInfo()
    {
        stateAndCountry.text = ApiHitScript.instance.playerToken.data.city + "," + " " + ApiHitScript.instance.playerToken.data.country;
    }

    void ProfileCallback(string response)
    {
        print("ProfileCallback: " + response);
        ClubManagement.instance.loadingPanel.SetActive(false);
        player = JsonUtility.FromJson<PlayerStats>(response);
        if (string.IsNullOrEmpty(response))
        {
            Cashier.instance.toastMsg.text = LanguageManager.Instance.GetTextValue("internet check");//networkErrorMsg;
            Cashier.instance.toastMsgPanel.SetActive(true);
            SocialPokerGameManager.instance.ClickHome();
            print("Some error occurred.");
        }
        else
        {
            print("response" + response);
            if(!player.error)
            {
                SocialPokerGameManager.instance.ClickProfile();

                totalGamePlayed.text = player.totalGame;
                totalTourney.text = player.totalTour;
                totalWinnings.text = player.totalWin;
                vpip.text = player.vpip.ToString() + "%";
                prf.text = player.prf.ToString() + "%";
                cBet.text = player.cbet.ToString() + "%";
                threeBet.text = player.threeBet.ToString() + "%";

                tourvpip.text = player.vpip_tournament.ToString() + "%";
                tourprf.text = player.pfr_tournament.ToString() + "%";
                tourcBet.text = player.cbet_tournament.ToString() + "%";
                tourthreeBet.text = player.threeBet_tournament.ToString() + "%";
                totalvpip.text = player.vpipTotal.ToString() + "%";
                totalprf.text = player.pfrTotal.ToString() + "%";
                totalcBet.text = player.cbetTotal.ToString() + "%";
                totalthreeBet.text = player.threeBetTotal.ToString() + "%";

                if (player.preferred_country.Count == 1)
                {
                    preferredCountryText.text = player.preferred_country[0];
                }
                else if (player.preferred_country.Count > 1)
                {
                    preferredCountryText.text = player.preferred_country[0] + "," + " " + player.preferred_country[1];
                }
                else
                {
                    print("No Data From Server.");
                }

                if (player.preferred_language.Count == 1)
                {
                    preferredLanguageText.text = player.preferred_language[0];
                    changeLanguageText.text = player.preferred_language[0];
                }
                else if (player.preferred_language.Count > 1)
                {
                    preferredLanguageText.text = player.preferred_language[0] + "," + " " + player.preferred_language[1];
                    changeLanguageText.text = player.preferred_language[0] + "," + " " + player.preferred_language[1];
                }
                else
                {
                    print("No Data From Server.");
                }
            }
            else
            {
                if (player.statusCode == 403)
                {
                    Cashier.instance.toastMsg.text = LanguageManager.Instance.GetTextValue("session expired");// SocialTournamentScript.instance.login_error;
                    Cashier.instance.toastMsgPanel.SetActive(true);
                    Uimanager.instance.SignOut();
                }
            }


            
        }
    }
    #endregion

    #region Preferred Countries

    public void OpenPreferredCountries()
    {
        selectPreferredCountries.SetActive(true);
        countriesInfo.preferred_country.Clear();
    }

    public void BackFromCountries()
    {
        for (int i = 0; i < countryContent.transform.childCount; i++)
        {
            if (countryContent.transform.GetChild(i).GetChild(1).GetChild(0).gameObject.activeInHierarchy)
            {
                countryContent.transform.GetChild(i).GetChild(1).GetChild(0).gameObject.SetActive(false);
            }
        }
        countriesInputField.text = string.Empty;
        selectPreferredCountries.SetActive(false);
    }
    public List<GameObject> allLanguage;
    public void PreferredCountriesCheckBox(GameObject obj)
    {
        if (!obj.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            for (int i = 0; i < allLanguage.Count; i++)
            {
                allLanguage[i].SetActive(false);
            }
            if (obj.CompareTag("English"))
            {
                LanguageHandler.instance.isEnglish = true;
                LanguageHandler.instance.isSpanish = false;
            }
            else if (obj.CompareTag("Spanish"))
            {
                LanguageHandler.instance.isEnglish = false;
                LanguageHandler.instance.isSpanish = true;
            }
            else
            {
                LanguageHandler.instance.isEnglish = true;
                LanguageHandler.instance.isSpanish = false;
            }

            obj.transform.GetChild(0).gameObject.SetActive(true);

        }
        else
        {
            obj.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    public void DefaultLanguageBtnSelectionUI()
    {
        if (LanguageHandler.instance.isSpanish)
        {
            allLanguage[0].SetActive(false);
            allLanguage[1].SetActive(true);
        }
        else if (LanguageHandler.instance.isEnglish)
        {
            allLanguage[0].SetActive(true);
            allLanguage[1].SetActive(false);
        }

    }

    public void PreferredCountriesDone()
    {
        if (countryContent.activeInHierarchy)
        {
            for (int i = 0; i < countryContent.transform.childCount; i++)
            {
                if (countryContent.transform.GetChild(i).GetChild(1).GetChild(0).gameObject.activeInHierarchy)
                {
                    countriesInfo.preferred_country.Add(countryContent.transform.GetChild(i).GetChild(0).GetComponent<Text>().text);
                }
            }
            string body = JsonUtility.ToJson(countriesInfo);
            print("body : " + body);
            Communication.instance.PostData(updateCountriesOnServerUrl, body, PreferredCountriesUpdatedOnServer);

            for (int i = 0; i < countryContent.transform.childCount; i++)
            {
                if (countryContent.transform.GetChild(i).GetChild(1).GetChild(0).gameObject.activeInHierarchy)
                {
                    countryContent.transform.GetChild(i).GetChild(1).GetChild(0).gameObject.SetActive(false);
                }
            }
            countriesInfo.preferred_country.Clear();
            selectPreferredCountries.SetActive(false);

            ClubManagement.instance.loadingPanel.SetActive(true);
            //Communication.instance.PostData(userStatsUrl, CountryCallBack);
        }

        else if (languageContent.activeInHierarchy || changeLanguage.activeInHierarchy)
        {
            for (int i = 0; i < languageContent.transform.childCount; i++)
            {
                if (languageContent.transform.GetChild(i).GetChild(1).GetChild(0).gameObject.activeInHierarchy)
                {
                    languagesInfo.preferred_language.Add(languageContent.transform.GetChild(i).GetChild(0).GetComponent<Text>().text);
                }
            }
            string body = JsonUtility.ToJson(languagesInfo);
            print("body : " + body);
            Communication.instance.PostData(updateCountriesOnServerUrl, body, PreferredCountriesUpdatedOnServer);

            for (int i = 0; i < languageContent.transform.childCount; i++)
            {
                if (languageContent.transform.GetChild(i).GetChild(1).GetChild(0).gameObject.activeInHierarchy)
                {
                    languageContent.transform.GetChild(i).GetChild(1).GetChild(0).gameObject.SetActive(false);
                }
            }

            languagesInfo.preferred_language.Clear();
            selectPreferredLanguages.SetActive(false);

            ClubManagement.instance.loadingPanel.SetActive(true);
            //Communication.instance.PostData(userStatsUrl, LanguageCallBack);
        }
        ClubManagement.instance.loadingPanel.SetActive(true);
    }

    //public void CountryCallBack(string response)
    //{
    //    ClubManagement.instance.loadingPanel.SetActive(false);
    //    countriesInfo = JsonUtility.FromJson<SelectedCountriesInfo>(response);
    //    if (string.IsNullOrEmpty(response))
    //    {
    //        print("Some error occurred.");
    //    }
    //    else
    //    {
    //        print("response" + response);
    //        if (countriesInfo.preferred_country.Count > 1)
    //        {
    //            preferredCountryText.text = countriesInfo.preferred_country[0] + "," + " " + countriesInfo.preferred_country[1];
    //        }
    //        else if(countriesInfo.preferred_country.Count == 1)
    //        {
    //            preferredCountryText.text = countriesInfo.preferred_country[0];
    //        }
    //    }
    //}

    //public void LanguageCallBack(string response)
    //{
    //    ClubManagement.instance.loadingPanel.SetActive(false);
    //    languagesInfo = JsonUtility.FromJson<SelectedLanguagesInfo>(response);
    //    if (string.IsNullOrEmpty(response))
    //    {
    //        print("Some error occurred.");
    //    }
    //    else
    //    {
    //        print("response" + response);
    //        if (languagesInfo.preferred_language.Count > 1)
    //        {
    //            preferredLanguageText.text = languagesInfo.preferred_language[0] + "," + " " + languagesInfo.preferred_language[1];
    //        }
    //        else if(languagesInfo.preferred_language.Count == 1)
    //        {
    //            preferredLanguageText.text = languagesInfo.preferred_language[0];
    //        }
    //    }
    //}

    void PreferredCountriesUpdatedOnServer(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("error");
        }
        else
        {
            print("" + response);
            ProfileButton();
            print("Updated Successfully...");
        }
    }
    #endregion

    #region Preferred Languages

    [Serializable]
    public class LanguageResponseList
    {
        public bool error;
        public chips[] chips;
        public int statusCode;
    }
    [Serializable]
    public class chips
    {
        public string name;
        public string code;
    }

    [SerializeField] public LanguageResponseList languageResponseList;

    private GameObject scrollItemObj;

    public void BackFromLanguages()
    {
        for (int i = 0; i < languageContent.transform.childCount; i++)
        {
            if (languageContent.transform.GetChild(i).GetChild(1).GetChild(0).gameObject.activeInHierarchy)
            {
                languageContent.transform.GetChild(i).GetChild(1).GetChild(0).gameObject.SetActive(false);
            }
        }
        languageInputField.text = string.Empty;
        languagesInfo.preferred_language.Clear();
        selectPreferredLanguages.SetActive(false);
    }

    public void OpenLanguageList()
    {
        selectPreferredLanguages.SetActive(true);
        languagesInfo.preferred_language.Clear();
        ClubManagement.instance.loadingPanel.SetActive(true);
        Communication.instance.PostData(languageUrl, OpenDropdownProcess);
    }

    void OpenDropdownProcess(string response)
    {
        print("Enter2");
        print(response);
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (!string.IsNullOrEmpty(response))
        {
            print("Enter2.2");
            print(response);
            languageResponseList = JsonUtility.FromJson<LanguageResponseList>(response);

            if (!languageResponseList.error)
            {
                if (languageResponseList.chips.Length != languageCount)
                {
                    for (int i = languageCount; i < languageResponseList.chips.Length; i++)
                    {
                        languageCount++;

                        GenerateLanguageItem();
                    }
                }

                for (int i = 0; i < languageContent.transform.childCount; i++)
                {
                    SearchingScript.instance.languageListName.Add(languageContent.transform.GetChild(i));
                }
            }
            else
            {
                if (languageResponseList.statusCode == 403)
                {
                    Cashier.instance.toastMsg.text = LanguageManager.Instance.GetTextValue("session expired");// SocialTournamentScript.instance.login_error;
                    Cashier.instance.toastMsgPanel.SetActive(true);
                    Uimanager.instance.SignOut();
                }
            }

            if (selectPreferredLanguages.activeInHierarchy)
            {
                for (int i = 0; i < languageResponseList.chips.Length; i++)
                {
                    languageContent.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = languageResponseList.chips[i].name;
                }
            }
        }
    }

    void GenerateLanguageItem()
    {
        if (languageScrollView.activeInHierarchy)
        {
            scrollItemObj = Instantiate(languagePanel);
            scrollItemObj.transform.SetParent(languageContent.transform, false);
        }
        languageGeneratedList.Add(scrollItemObj);
    }

    #endregion

    #region Buy Chips(Shop)

    [Serializable]
    public class ShopInfo
    {
        public int social_chips;
    }

    [SerializeField]
    public ShopInfo shopInfo;

    [Serializable]
    public class DiamondCheckResponse
    {
        public bool error;
        public string diamond;
        public int statusCode;
        public Errors errors;
    }

    [Serializable]
    public class Errors
    {
        public string message;
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

    [SerializeField] DiamondCheckResponse diamondCheckResponse;

    public void BuyChips(int chips)
    {
        shopInfo.social_chips = chips;
        string body = JsonUtility.ToJson(shopInfo);
        ClubManagement.instance.loadingPanel.SetActive(true);
        Communication.instance.PostData(buyChipsUrl, body, BuyChipProcess);
    }

    void BuyChipProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            Cashier.instance.toastMsg.text = LanguageManager.Instance.GetTextValue("internet check");//networkErrorMsg;
            Cashier.instance.toastMsgPanel.SetActive(true);
            print("error");
        }
        else
        {
            print("response " + response);
            
            diamondCheckResponse = JsonUtility.FromJson<DiamondCheckResponse>(response);

            //chipBalance.text = diamondCheckResponse.diamond.ToString();
            if(!diamondCheckResponse.error)
            {
                print("Chips Updated Successfully");

                for (int i = 0; i < ApiHitScript.instance.socialChips.Count; i++)                                     //Updating Social Chips while Login
                {
                    //ApiHitScript.instance.socialChips[i].text = "$" + diamondCheckResponse.diamond;
                    ApiHitScript.instance.socialChips[i].text = diamondCheckResponse.diamond;
                }
                print("CHIPS: " + chipBalance.text);
                string playerChips = diamondCheckResponse.diamond;
                PlayerPrefs.SetString("SOCIAL_CHIPS", playerChips);
            }
            else
            {
                if (diamondCheckResponse.statusCode == 403)
                {
                    Cashier.instance.toastMsg.text = LanguageManager.Instance.GetTextValue("session expired");// SocialTournamentScript.instance.login_error;
                    Cashier.instance.toastMsgPanel.SetActive(true);
                    Uimanager.instance.SignOut();
                }
                else
                {
                    Cashier.instance.toastMsg.text = diamondCheckResponse.errors.message;
                    Cashier.instance.toastMsgPanel.SetActive(true);
                }
            }
        }
    }

    #endregion

    #region Change Password

    [Serializable]
    public class SetNewPassword
    {
        public string new_password;
        public string confirm_password;
        public string current_password;
    }

    [Serializable]
    public class ChangePasswordResponse
    {
        public bool error;
        public Errors errors;
    }

    [SerializeField] SetNewPassword setNewPassword;
    [SerializeField] ChangePasswordResponse changePasswordResponse;
    public void ChangePasswordSubmitButton()
    {
        if (string.IsNullOrEmpty(currentPasswordInputField.text) || !currentPasswordInputField.GetComponent<ValidateInputField>().isValidInput)
        {
            currentPasswordInputField.GetComponent<ValidateInputField>().Validate(currentPasswordInputField.text);
            currentPasswordInputField.Select();
        }
        else if (string.IsNullOrEmpty(setNewPassword1InputField.text) || !setNewPassword1InputField.GetComponent<ValidateInputField>().isValidInput)
        {
            setNewPassword1InputField.GetComponent<ValidateInputField>().Validate(setNewPassword1InputField.text);
            setNewPassword1InputField.Select();
        }
        else if (string.IsNullOrEmpty(setNewPassword2InputField.text) || !setNewPassword2InputField.GetComponent<ValidateInputField>().isValidInput)
        {
            setNewPassword2InputField.GetComponent<ValidateInputField>().Validate(setNewPassword2InputField.text);
            setNewPassword2InputField.Select();
        }
        else
        {

            setNewPassword.new_password = setNewPassword1InputField.textComponent.text;
            setNewPassword.confirm_password = setNewPassword2InputField.textComponent.text;
            setNewPassword.current_password = currentPasswordInputField.textComponent.text;

            string body = JsonUtility.ToJson(setNewPassword);

            ClubManagement.instance.loadingPanel.SetActive(true);
            Communication.instance.PostData(ChangePasswordUrl, body, SetNewPasswordValuesProcess);
            print(body);
        }

    }

    public Text serverErrorChangePasswordText;
    void SetNewPasswordValuesProcess(string response)
    {
        print(response);
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

                ResetInputFields();

                Cashier.instance.toastMsgPanel.SetActive(true);
                Cashier.instance.toastMsg.text = LanguageManager.Instance.GetTextValue("password changed success");
            }
            else
            {
                serverErrorChangePasswordText.text = LanguageManager.Instance.GetTextValue("Server Error Change Password");
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

    public void ResetInputFields()
    {
        changePassword.SetActive(false);
        setNewPassword1InputField.text = string.Empty; 
        setNewPassword2InputField.text = string.Empty;
        currentPasswordInputField.text = string.Empty; 
    }

    #endregion

    #region Change Language

    public void ResetChangeLanguage()
    {
        print(changeLanguageContent.transform.childCount);
        for (int i = 0; i < changeLanguageContent.transform.childCount; i++)
        {
            if (changeLanguageContent.transform.GetChild(i).GetChild(1).GetChild(0).gameObject.activeInHierarchy)
            {
                changeLanguageContent.transform.GetChild(i).GetChild(1).GetChild(0).gameObject.SetActive(false);
            }
        }
        changeLanguageInputField.text = string.Empty;
        languagesInfo.preferred_language.Clear();
        changeLanguageScreen.SetActive(false);
    }

    public void DoneChangeLanguage()
    {
        for (int i = 0; i < changeLanguageContent.transform.childCount; i++)
        {
            if (changeLanguageContent.transform.GetChild(i).GetChild(1).GetChild(0).gameObject.activeInHierarchy)
            {
                languagesInfo.preferred_language.Add(changeLanguageContent.transform.GetChild(i).GetChild(0).GetComponent<Text>().text);
            }
        }
        string body = JsonUtility.ToJson(languagesInfo);
        print("body : " + body);
        Communication.instance.PostData(updateCountriesOnServerUrl, body, PreferredCountriesUpdatedOnServer);

        ResetChangeLanguage();
        LanguageHandler.instance.SetLanguage();
    }

    #endregion

    #region Buy Chips

    public void UpdateBuyChipsInfo()
    {
        CurrencyConversion.instance.BuyChipRequest();
        buyChips.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = ApiHitScript.instance.playerToken.data.username;
        buyChips.transform.GetChild(1).GetChild(2).GetComponent<Text>().text = ApiHitScript.instance.playerToken.data.client_id;
        buyChips.SetActive(true);
    }
    #endregion

    #region Update Social Chips

    [Serializable]
    public class UpdateSocialChips
    {
        public bool error;
        public string social_chips;
        public int statusCode;
    }

    [SerializeField]
    public UpdateSocialChips updateChips;

    public void SocialChips()
    {
        ClubManagement.instance.loadingPanel.SetActive(true);
        Communication.instance.PostData(updatedSocialChipsUrl, SocialChipsProcess);
    }

    void SocialChipsProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        print("SOCIALCHIPS" + response);
        updateChips = JsonUtility.FromJson<UpdateSocialChips>(response);
        if (string.IsNullOrEmpty(response))
        {
            Cashier.instance.toastMsg.text = LanguageManager.Instance.GetTextValue("internet check");//networkErrorMsg;
            Cashier.instance.toastMsgPanel.SetActive(true);
            print("Some error occurred.");
        }
        else
        {
            print("response" + response);

            if(!updateChips.error)
            {
                for (int i = 0; i < ApiHitScript.instance.socialChips.Count; i++)                                     //Updating Social Chips while Login
                {
                    //ApiHitScript.instance.socialChips[i].text = "$" + updateChips.social_chips;
                    ApiHitScript.instance.socialChips[i].text = updateChips.social_chips;
                }
                print("Updated CHIPS: " + updateChips.social_chips);
                string playerChips = updateChips.social_chips;
                PlayerPrefs.SetString("SOCIAL_CHIPS", playerChips);
            }
            else
            {
                if (updateChips.statusCode == 403)
                {
                    Cashier.instance.toastMsg.text = LanguageManager.Instance.GetTextValue("session expired");// SocialTournamentScript.instance.login_error;
                    Cashier.instance.toastMsgPanel.SetActive(true);
                    Uimanager.instance.SignOut();
                }
            }


        }
    }

    #endregion

    public bool CheckMinCharcterInPassword()
    {
        if (Convert.ToInt32(setNewPassword1InputField.text.Length) < Registration.instance.minLengthInPassword)
        {
            return false;
        }
        else
        {
            return true;
        }

    }
}