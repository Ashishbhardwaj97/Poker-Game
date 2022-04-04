using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class Uimanager : MonoBehaviour
{
    public static Uimanager instance;

    [Header("GameObject Reference")]
    public GameObject clubHomePage;
    public GameObject createTablePage;
    public GameObject DemoClaimPage;
    public GameObject DemoSendOutPage;
    public GameObject bottomPanel;
    public GameObject joinClub;
    public GameObject clubListing;
    public GameObject changePasswordPage;
    public GameObject settingPage;
    public GameObject profilePage;
    public GameObject homePage;
    public GameObject myAccount;
    public GameObject shop;
    public GameObject favoriteClubListing;
    public GameObject clubChipsPanelInClaimBack;
    public GameObject agentCreditPanelInClaimBack;
    public GameObject clubChipsPanelInSendout;
    public GameObject agentCreditPanelInSendout;
    public GameObject splash;
    public GameObject login;
    public GameObject ClaimContent;
    public GameObject SendOutContent;
    public GameObject MenuCanvas;



    public List<GameObject> PlayerC;
    public List<GameObject> PlayerC1;
    internal GameObject currentTable;

    [Header("InputField Reference")]
    public InputField claimAmountValuedemo;
    public InputField SendoutAmountValuedemo;

    [Header("String Reference")]
    public string amountvalue;
    //private string playerExitUrl;

    [Header("Text Reference")]
    public Text agentCreditBalanceInClaimback;
    public Text agentCreditBalanceInSendout;
    public Text claimTotalChips, sendoutTotalChips;
    public Text claimClubChipsBalance, sendoutClubChipsBalance;

    [Header("Bool Reference")]
    public bool isHomePage;
    public bool isClubListingPage;

    public string logoutUrl;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        
        //playerExitUrl = ServerChanger.instance.domainURL + "api/v1/user/update-exit-status";
        logoutUrl = ServerChanger.instance.domainURL + "api/v1/user/logout";

        PlayerC = new List<GameObject>();
        PlayerC1 = new List<GameObject>();
        if (PlayerPrefs.GetInt("SceneChange") == 1)
        {
            splash.SetActive(false);
            login.SetActive(false);
            clubHomePage.SetActive(true);
            PlayerPrefs.SetInt("SceneChange", 0);
        }

        amountvalue = "0";
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Autoupdatevalue()
    {

        int playerCount = Convert.ToInt32(Cashier.instance.noPlayerCount.text);

        if (!string.IsNullOrEmpty(claimAmountValuedemo.GetComponent<InputField>().text))
        {
            claimTotalChips.text = (Convert.ToInt32(claimAmountValuedemo.GetComponent<InputField>().text) * playerCount).ToString();
        }
        if (!string.IsNullOrEmpty(SendoutAmountValuedemo.GetComponent<InputField>().text))
        {
            sendoutTotalChips.text = (Convert.ToInt32(SendoutAmountValuedemo.GetComponent<InputField>().text) * playerCount).ToString();
        }
    }

    //.........ClaimBack..........//

    public void DemoClaimBackButton()
    {
        claimAmountValuedemo.text = string.Empty;
        SendoutAmountValuedemo.text = string.Empty;
        claimTotalChips.text = "0";
        sendoutTotalChips.text = "0";

        PlayerC.Clear();
        for (int i = 0; i < PlayerC1.Count; i++)
        {
            Destroy(PlayerC1[i]);
        }
        for (int i = 0; i < Cashier.instance.TradeContent.transform.childCount; i++)
        {
            if (Cashier.instance.TradeContent.transform.GetChild(i).GetChild(6).GetChild(2).GetChild(0).gameObject.activeInHierarchy)
            {
                PlayerC.Add(Cashier.instance.TradeContent.transform.GetChild(i).gameObject);
            }
        }
        InstanceData();

        DemoClaimPage.SetActive(true);
        clubHomePage.SetActive(false);

        if (ClubManagement.instance.currentRoleInSelectedClub == 4)
        {
            agentCreditBalanceInClaimback.text = Cashier.instance.agentCreditBalance.text;
            agentCreditPanelInClaimBack.SetActive(true);
            clubChipsPanelInClaimBack.SetActive(false);
        }
        else
        {
            claimClubChipsBalance.text = Cashier.instance.clubChip.text;
            clubChipsPanelInClaimBack.SetActive(true);
            agentCreditPanelInClaimBack.SetActive(false);
        }
    }

    public void InstanceData()
    {
        for (int i = 0; i < PlayerC.Count; i++)
        {
            if (Cashier.instance.TradeContent.transform.GetChild(i).GetChild(6).GetChild(2).GetChild(1).gameObject.activeInHierarchy)
            {
                GameObject instobj = Instantiate(PlayerC[i]);
                instobj.transform.SetParent(ClaimContent.transform);
                instobj.transform.localScale = new Vector3(1, 1, 1);
                PlayerC1.Add(instobj);
            }
        }
    }

    public void UIClaimBackButton()
    {
        PlayerPrefs.SetInt("TickedItem", 0);
        amountvalue = claimAmountValuedemo.text.ToString();
        claimAmountValuedemo.text = "";

        Cashier.instance.currentClubId.text = ClubManagement.instance._clubID;
        ClubManagement.instance.ClickOnClubDetails(Cashier.instance.currentClubId);
        PlayerPrefs.SetInt("CurrentChips", 1);

        ClubManagement.instance.ClickMyTrade();
    }


    //.......Send Out........//

    public void DemoSendOutButton()
    {
        claimAmountValuedemo.text = string.Empty;
        SendoutAmountValuedemo.text = string.Empty;
        claimTotalChips.text = "0";
        sendoutTotalChips.text = "0";

        PlayerC.Clear();
        for (int i = 0; i < PlayerC1.Count; i++)
        {
            Destroy(PlayerC1[i]);
        }
        for (int i = 0; i < Cashier.instance.TradeContent.transform.childCount; i++)
        {
            if (Cashier.instance.TradeContent.transform.GetChild(i).GetChild(6).GetChild(2).GetChild(0).gameObject.activeInHierarchy)
            {
                PlayerC.Add(Cashier.instance.TradeContent.transform.GetChild(i).gameObject);
            }
        }
        InstanceData1();

        DemoSendOutPage.SetActive(true);
        clubHomePage.SetActive(false);

        if (ClubManagement.instance.currentRoleInSelectedClub == 4)
        {
            agentCreditBalanceInSendout.text = Cashier.instance.agentCreditBalance.text;
            agentCreditPanelInSendout.SetActive(true);
            clubChipsPanelInSendout.SetActive(false);
        }
        else
        {
            sendoutClubChipsBalance.text = Cashier.instance.clubChip.text;
            clubChipsPanelInSendout.SetActive(true);
            agentCreditPanelInSendout.SetActive(false);
        }
    }

    public void InstanceData1()
    {
        for (int i = 0; i < PlayerC.Count; i++)
        {
            if (Cashier.instance.TradeContent.transform.GetChild(i).GetChild(6).GetChild(2).GetChild(1).gameObject.activeInHierarchy)
            {
                GameObject instobj = Instantiate(PlayerC[i]);
                instobj.transform.SetParent(SendOutContent.transform);
                instobj.transform.localScale = new Vector3(1, 1, 1);
                PlayerC1.Add(instobj);
            }
        }
    }

    public void UISendOutButton()
    {
        PlayerPrefs.SetInt("TickedItem", 0);
        amountvalue = SendoutAmountValuedemo.text.ToString();
        SendoutAmountValuedemo.text = "";

        Cashier.instance.currentClubId.text = ClubManagement.instance._clubID;
        ClubManagement.instance.ClickOnClubDetails(Cashier.instance.currentClubId);
        PlayerPrefs.SetInt("CurrentChips", 1);

        ClubManagement.instance.ClickMyTrade();
    }

    //.......................//
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (joinClub.activeInHierarchy)
            {
                homePage.SetActive(true);
                joinClub.SetActive(false);
            }

            if (DemoClaimPage.activeInHierarchy)
            {
                DemoClaimPage.SetActive(false);
                clubHomePage.SetActive(true);
                Cashier.instance.RemoveAllTick();
            }

            else if (DemoSendOutPage.activeInHierarchy)
            {
                DemoSendOutPage.SetActive(false);
                clubHomePage.SetActive(true);
                Cashier.instance.RemoveAllTick();
            }
        }

        if (homePage.activeInHierarchy || joinClub.activeInHierarchy || clubListing.activeInHierarchy || changePasswordPage.activeInHierarchy || settingPage.activeInHierarchy || profilePage.activeInHierarchy || shop.activeInHierarchy)
        {
            if (homePage.transform.GetChild(2).gameObject.activeSelf == true || homePage.transform.GetChild(3).gameObject.activeSelf == true || homePage.transform.GetChild(4).gameObject.activeSelf == true)
            {
                bottomPanel.SetActive(false);
            }
            else
            {
                bottomPanel.SetActive(true);
            }
        }

        else
        {
            bottomPanel.SetActive(false);
        }

        Autoupdatevalue();

        ////...................................................//

        //if (PlayerPrefs.GetInt("TableMatchOffOn") == 0)
        //{
        //TableMatchOnOff.transform.GetChild(1).gameObject.SetActive(false);
        //    TableMatchOnOff.transform.GetChild(2).gameObject.SetActive(true);
        //}
        //if (PlayerPrefs.GetInt("TableMatchOffOn") == 1)
        //{
        //    TableMatchOnOff.transform.GetChild(1).gameObject.SetActive(true);
        //    TableMatchOnOff.transform.GetChild(2).gameObject.SetActive(false);
        //}

        ////...................................................//

    }

    //...................................................//
    //public void TMclickOn()
    //{       
    //    PlayerPrefs.SetInt("TableMatchOffOn", 0);         
    //}
    //public void TMclickOff()
    //{
    //    PlayerPrefs.SetInt("TableMatchOffOn", 1);       
    //}
    //...................................................//

    public void SeeAllFavoriteClub()
    {
        favoriteClubListing.SetActive(true);
        homePage.transform.GetChild(1).gameObject.SetActive(false);
        for (int i = 0; i < FavoriteClubScript.instance.favClubScrollContent.transform.childCount; i++)
        {
            FavoriteClubScript.instance.favClubScrollContent.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void BackFavoriteClub()
    {
        favoriteClubListing.SetActive(false);
        homePage.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void ClickOnPlay()
    {
        ClubManagement.instance.isClubDetailPage = false;
        myAccount.SetActive(false);
        shop.SetActive(false);
        changePasswordPage.SetActive(false);
        joinClub.SetActive(false);
        clubListing.SetActive(false);
        homePage.SetActive(true);
        //homePage.transform.GetChild(1).gameObject.SetActive(true);
        JoinClub.instance.ResetFilterValues();

        bottomPanel.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        bottomPanel.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);

        bottomPanel.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
        bottomPanel.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
        bottomPanel.transform.GetChild(3).GetChild(1).gameObject.SetActive(false);

        bottomPanel.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
        bottomPanel.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
        bottomPanel.transform.GetChild(3).GetChild(0).gameObject.SetActive(true);

        FavoriteClubScript.instance.ClickOnPlayBtton();
        isClubListingPage = false;
        isHomePage = true;
    }

    public void ClickOnClub()
    {
        ClubManagement.instance.isClubDetailPage = false;
        shop.SetActive(false);
        myAccount.SetActive(false);
        changePasswordPage.SetActive(false);
        clubListing.SetActive(true);
        joinClub.SetActive(false);
        homePage.SetActive(false);
        FavoriteClubScript.instance.favClubListingObject.SetActive(false);

        bottomPanel.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        bottomPanel.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);

        bottomPanel.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        bottomPanel.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
        bottomPanel.transform.GetChild(3).GetChild(1).gameObject.SetActive(false);

        bottomPanel.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        bottomPanel.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
        bottomPanel.transform.GetChild(3).GetChild(0).gameObject.SetActive(true);

        isClubListingPage = true;
        isHomePage = false;
    }

    public void ClickOnShop()
    {
        ClubManagement.instance.isClubDetailPage = false;
        shop.SetActive(true);
        changePasswordPage.SetActive(false);
        joinClub.SetActive(false);
        clubListing.SetActive(false);
        homePage.SetActive(false);
        myAccount.SetActive(false);
        JoinClub.instance.ResetFilterValues();
        FavoriteClubScript.instance.favClubListingObject.SetActive(false);

        bottomPanel.transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
        bottomPanel.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
        bottomPanel.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        bottomPanel.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        bottomPanel.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
        bottomPanel.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
        bottomPanel.transform.GetChild(3).GetChild(0).gameObject.SetActive(true);
        bottomPanel.transform.GetChild(3).GetChild(1).gameObject.SetActive(false);
    }

    public void ClickOnProfile()
    {
        ClubManagement.instance.isClubDetailPage = false;
        shop.SetActive(false);
        changePasswordPage.SetActive(false);
        joinClub.SetActive(false);
        clubListing.SetActive(false);
        homePage.SetActive(false);
        myAccount.SetActive(true);
        JoinClub.instance.ResetFilterValues();
        FavoriteClubScript.instance.favClubListingObject.SetActive(false);

        bottomPanel.transform.GetChild(3).GetChild(0).gameObject.SetActive(false);
        bottomPanel.transform.GetChild(3).GetChild(1).gameObject.SetActive(true);

        bottomPanel.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
        bottomPanel.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
        bottomPanel.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);

        bottomPanel.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
        bottomPanel.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
        bottomPanel.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
    }

    public void ClickOnJoinClub()
    {
        JoinClub.instance.ClearJoinClubInputField();

        joinClub.SetActive(true);
        homePage.SetActive(false);

    }

    public void BackJoinClub()
    {
        joinClub.SetActive(false);
        homePage.SetActive(true);
    }

    public void ClicktoCreateClubPage()
    {
        homePage.transform.GetChild(2).gameObject.SetActive(true);
        homePage.transform.GetChild(1).gameObject.SetActive(false);
        bottomPanel.SetActive(false);
        homePage.transform.GetChild(2).GetChild(12).GetChild(0).GetComponent<Text>().text = Profile._instance.country.text;
        CreateClubMain.instance.cityInputField.text = Profile._instance.city.text;
    }

    //..........For Eye Ball..........//
    int EyeBallCount = 0;
    public void EyeBall()
    {
        EyeBallCount++;
        if (EyeBallCount % 2 == 0)
        {

            Login.instance.eyeBall_On.SetActive(false);
            Login.instance.eyeBall_Off.SetActive(true);

            print("onnnn");

            if (ApiHitScript.instance.passwordInputField != null)
            {

                if (ApiHitScript.instance.passwordInputField.contentType == TMP_InputField.ContentType.Standard)
                {
                    ApiHitScript.instance.passwordInputField.contentType = TMP_InputField.ContentType.Password;
                    ApiHitScript.instance.passwordInputField.textComponent.enableAutoSizing = true;
                    ApiHitScript.instance.passwordInputField.textComponent.fontSize = 72f;
                }

                ApiHitScript.instance.passwordInputField.ForceLabelUpdate();
            }

        }
        else
        {
            Login.instance.eyeBall_On.SetActive(true);
            Login.instance.eyeBall_Off.SetActive(false);

            print("offffffff");

            if (ApiHitScript.instance.passwordInputField != null)
            {
                if (ApiHitScript.instance.passwordInputField.contentType == TMP_InputField.ContentType.Password)
                {
                    ApiHitScript.instance.passwordInputField.contentType = TMP_InputField.ContentType.Standard;
                    ApiHitScript.instance.passwordInputField.textComponent.enableAutoSizing = false;
                    ApiHitScript.instance.passwordInputField.textComponent.fontSize = 50f;
                }
                ApiHitScript.instance.passwordInputField.ForceLabelUpdate();
            }
        }
    }

    //.............................//

    public void SignOut()
    {
        
        FriendandSocialScript.instance.EraseFriendsData();

        ClubManagement.instance.StopCountDown();
        PlayerPrefs.SetInt(ApiHitScript.PLAYERKEY_LOGINSTATUS, ApiHitScript.LOGGED_OUT);
        myAccount.SetActive(false);
        homePage.SetActive(false);
        SocialPokerGameManager.instance.profilePage.SetActive(false);
        SocialPokerGameManager.instance.profilePage.transform.GetChild(1).gameObject.SetActive(false);
        SocialPokerGameManager.instance.profilePage.transform.GetChild(2).gameObject.SetActive(false);
        Registration.instance.ClearInputFields();

        if (!Login.instance.isRememberMe)
        {
            ApiHitScript.instance.ClearLoginInputFields();
        }

        login.SetActive(true);
        Registration.instance.secondPanel.SetActive(false);
        Registration.instance.firstPanel.SetActive(true);
        Registration.instance.registrationPage.SetActive(false);

        ApiHitScript.instance.getUserImageUrl = "";
        Registration.uploadedImage = "";
        Registration.getImageUrl = "";

        if (ClubManagement.instance.myClubObjList.Count > 0)
        {
            for (int i = 0; i < ClubManagement.instance.myClubObjList.Count; i++)
            {
                Destroy(ClubManagement.instance.myClubObjList[i]);
            }

        }
        if (ClubManagement.instance.memList.Count > 0)
        {
            for (int i = 0; i < ClubManagement.instance.memList.Count; i++)
            {
                Destroy(ClubManagement.instance.memList[i]);
            }

        }
        if (ClubManagement.instance.memReqList.Count > 0)
        {
            for (int i = 0; i < ClubManagement.instance.memReqList.Count; i++)
            {
                Destroy(ClubManagement.instance.memReqList[i]);
            }

        }
        if (ClubManagement.instance.tradeMemList.Count > 0)
        {
            for (int i = 0; i < ClubManagement.instance.tradeMemList.Count; i++)
            {
                Destroy(ClubManagement.instance.tradeMemList[i]);
            }

        }

        if (ClubManagement.instance.tradeHistoryList.Count > 0)
        {
            for (int i = 0; i < ClubManagement.instance.tradeHistoryList.Count; i++)
            {
                Destroy(ClubManagement.instance.tradeHistoryList[i]);
            }

        }

        if (ClubManagement.instance.chipReqList.Count > 0)
        {
            for (int i = 0; i < ClubManagement.instance.chipReqList.Count; i++)
            {
                Destroy(ClubManagement.instance.chipReqList[i]);
            }
        }
        ClubManagement.instance.chipReqList.Clear();

        ClubManagement.instance.tradeHistoryList.Clear();

        ClubManagement.instance.tradeMemList.Clear();
        ClubManagement.instance.myClubObjList.Clear();
        ClubManagement.instance.memList.Clear();
        ClubManagement.instance.memReqList.Clear();

        ClubManagement.instance.memberScrollPanel.SetActive(false);

        ClubManagement.instance.MyclubConut = 1;
        ClubManagement.instance.memberConut = 1;
        ClubManagement.instance.memberReqCount = 1;
        ClubManagement.instance.memberChipConut = 1;
        ClubManagement.instance.tradeaMemberConut = 1;
        ClubManagement.instance.tradeaHistoryConut = 1;

        ClubManagement.instance.ResetAllValuesForImage();
        TournamentGameDetail.instance.ResetAllValuesForImage();

        ClubManagement.instance.ResetPlayerImageInTradeList();
        ClubManagement.instance.ResetPlayerImageInTradeHistory();
        ClubManagement.instance.ResetPlayerImageInChipReq();

        homePage.transform.GetChild(1).GetChild(3).GetChild(0).GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
        Registration.instance.DestroyContryGeneratedList();

        FavoriteClubScript.instance.ResetGeneratedFavClubItem();
        isClubListingPage = false;
        isHomePage = false;
        ClubManagement.instance.ResetClubImages();
        FavoriteClubScript.instance.ResetFavoriteClubImages();
        ClubManagement.instance.memberProfile.GetComponent<MemberProfile>().ResetAgentListItems();
        ClubManagement.instance.memberProfileForAgent.GetComponent<MemberProfile>().ResetAgentListItems();

        ClubManagement.instance.DestroyGeneratedTableItem();
        ClubManagement.instance.ResetRegularTableItem();
        ClubManagement.instance.ResetTournamentTableItem();
        ClubManagement.instance.ResetAllValuesForImageInCreateTable();
        ClubManagement.instance.ResetGenerateMembersInCreateTable();
        //PlayerExitThroughLogout(false);
        if (GoogleLoginScript.instance.isLoginWithGoogle)
        {
            GoogleLoginScript.instance.isLoginWithGoogle = false;
            GoogleLoginScript.instance.SignOutFromGoogle();
        }

        if (FBLogin.instance.isFbLoginClick)
        {
            FBLogin.instance.isFbLoginClick = false;
            FBLogin.instance.ClickFacebookLogout();
            FriendandSocialScript.instance.facebookInviteBtn.interactable = false;
            FriendandSocialScript.instance.isLoginThroughFacebook = false;
        }
        MailBoxScripts._instance.DestroyOutBoxItem();
        Admin.instance.DestroyExportRecordItem();
        SocialTournamentScript.instance.ResetUpcomingTournyList();
        SocialTournamentScript.instance.ResetRegisteredTournyList();
        LeadershipScript.instance.ResetAllValuesForImage4();
        PokerSceneManagement.instance.isSceneRestart = false;

        PlayerPrefs.SetInt(ApiHitScript.GalleryAwatar_Pic, ApiHitScript.GalleryAwatar_False);

        Registration.instance.isFacebookLogin = false;
        Registration.instance.DeleteKeyCurrentFbPopupCount();
    }

    #region Player Exit process

    [Serializable]
    class ExitStatusReq
    {
        public bool exit_status;
    }

    [SerializeField] ExitStatusReq exitStatusReq;
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            //exitStatusReq.exit_status = !pause;

            //string body = JsonUtility.ToJson(exitStatusReq);
            //print("body........." + body);

            //if (!string.IsNullOrEmpty(playerExitUrl))
            //{
            //    Communication.instance.PostData(playerExitUrl, body, PlayerExitProcess);
            //}
        }
        else
        {
            print("come back to game ........... ");
            //exitStatusReq.exit_status = !pause;
            //string body = JsonUtility.ToJson(exitStatusReq);
            //if (!string.IsNullOrEmpty(playerExitUrl))
            //{
            //    Communication.instance.PostData(playerExitUrl, body, PlayerExitProcess);
            //}
            //.............MailBox...............//
            MailBoxScripts._instance.MailBoxCallCount();
            //.......................................//
        }
    }

    public void PlayerExitThroughLogout(bool _status)
    {
        //exitStatusReq.exit_status = _status;

        //string body = JsonUtility.ToJson(exitStatusReq);
        //print("body........." + body);

        //Communication.instance.PostData(playerExitUrl, body, PlayerExitProcess);
    }

    void PlayerExitProcess(string response)
    {
        if (!string.IsNullOrEmpty(response))
        {
            print("ExitProcess : " + response);
        }
    }

    #endregion

    //..........................//

    public void GamePagePanel(GameObject panel)
    {
        ClubManagement.instance.currentSelectedLimitWinPlayer = bool.Parse(panel.transform.GetChild(35).GetComponent<Text>().text);
        ClubManagement.instance.resetStartTime = false;
        currentTable = panel;
        ClubManagement.instance.currentSelectedStartDateTime = panel.transform.GetChild(30).GetComponent<Text>().text;
        ClubManagement.instance.currentSelectedTableActionTime = int.Parse(panel.transform.GetChild(20).GetComponent<Text>().text);
        ClubManagement.instance.currentSelectedTableIsAutoStart = bool.Parse(panel.transform.GetChild(21).GetComponent<Text>().text);
        ClubManagement.instance.currentSelectedTableIsVideoMode = bool.Parse(panel.transform.GetChild(22).GetComponent<Text>().text);
        ClubManagement.instance.currentSelectedTableIsMississippiStraddle = bool.Parse(panel.transform.GetChild(23).GetComponent<Text>().text);
        ClubManagement.instance.currentSelectedTableIsBuyInAuth = bool.Parse(panel.transform.GetChild(24).GetComponent<Text>().text);
        ClubManagement.instance.currentSelectedTableMinAutoStart = int.Parse(panel.transform.GetChild(25).GetComponent<Text>().text);
        ClubManagement.instance.currentSelectedTableSize = int.Parse(panel.transform.GetChild(4).GetComponent<Text>().text);
        ClubManagement.instance.currentSelectedTableId = panel.transform.GetChild(19).GetComponent<Text>().text;
        ClubManagement.instance.currentSelectedTableStartTime = panel.transform.GetChild(5).GetComponent<Text>().text;

        ClubManagement.instance.currentSelectedTableEndTime = panel.transform.GetChild(6).GetComponent<Text>().text;
        ClubManagement.instance.currentSelectedTableName = panel.transform.GetChild(0).GetComponent<Text>().text;
        ClubManagement.instance.currentSelectedGameType = panel.transform.GetChild(8).GetChild(0).GetComponent<Text>().text;
        ClubManagement.instance.currentSelectedTableBlinds = panel.transform.GetChild(1).GetComponent<Text>().text + "/" + panel.transform.GetChild(2).GetComponent<Text>().text;
        ClubManagement.instance.currentSelectedTableType = panel.transform.GetChild(29).GetComponent<Text>().text;
        ClubManagement.instance.currentSelectedRuleId = panel.transform.GetChild(27).GetComponent<Text>().text;
        ClubManagement.instance.currentSelectedTableBotEnabled = bool.Parse(panel.transform.GetChild(33).GetComponent<Text>().text);
        ClubManagement.instance.currentSelectedTableBots = int.Parse(panel.transform.GetChild(34).GetComponent<Text>().text);
        ClubManagement.instance.isBottomPanelClose = false;
        if (ClubManagement.instance.currentSelectedTableType == "regular")
        {
            if (GameManagerScript.instance.isDev)
            {
                GameManagerScript.instance.socket.url = GameManagerScript.instance.regularTableDevUrl;
            }
            if (GameManagerScript.instance.isStaging)
            {
                GameManagerScript.instance.socket.url = GameManagerScript.instance.regularTableStagingUrl;
            }
            GameManagerScript.instance.isTournament = false;
            GameManagerScript.instance.socket.gameObject.SetActive(true);
            GameManagerScript.instance.networkManager.SetActive(true);
            PokerNetworkManager.instance.ObserveTable();
        }
        else
        {
            if (GameManagerScript.instance.isDev)
            {
                GameManagerScript.instance.Tournamentsocket.url = GameManagerScript.instance.tournamentTableDevUrl;
            }
            if (GameManagerScript.instance.isStaging)
            {
                GameManagerScript.instance.Tournamentsocket.url = GameManagerScript.instance.tournamentTableStagingUrl;
            }
            TournamentGameDetail.instance.tableCancelledPanel.SetActive(false);
            GameManagerScript.instance.isTournament = true;
            GameManagerScript.instance.Tournamentsocket.gameObject.SetActive(true);
            GameManagerScript.instance.tournamentManager.SetActive(true);

            TournamentManagerScript.instance.ConnectToServer();
        }
        PlayerPrefs.SetInt("SceneChange", 0);
        CancelInvoke();
    }

    public void GamePagePanelTracePlayer(GameObject panel)
    {
        TableInfoScript tableInfoScript = panel.GetComponent<TableInfoScript>();

        ClubManagement.instance.resetStartTime = false;
        currentTable = panel;

        //ClubManagement.instance.currentSelectedStartDateTime = panel.transform.GetChild(30).GetComponent<Text>().text;
        ClubManagement.instance.currentSelectedTableActionTime = tableInfoScript.tableData.action_time;
        ClubManagement.instance.currentSelectedTableIsAutoStart = tableInfoScript.tableData.auto_start;
        ClubManagement.instance.currentSelectedTableIsVideoMode = tableInfoScript.tableData.video_mode;
        ClubManagement.instance.currentSelectedTableIsMississippiStraddle = tableInfoScript.tableData.mississippi_straddle;
        ClubManagement.instance.currentSelectedTableIsBuyInAuth = tableInfoScript.tableData.buy_in_authorization;
        ClubManagement.instance.currentSelectedTableMinAutoStart = tableInfoScript.tableData.min_auto_start;
        ClubManagement.instance.currentSelectedTableSize = tableInfoScript.tableData.table_size;
        ClubManagement.instance.currentSelectedTableId = tableInfoScript.tableData.table_id;
        ClubManagement.instance.currentSelectedTableStartTime = tableInfoScript.tableData.start_time;

        ClubManagement.instance.currentSelectedTableEndTime = tableInfoScript.tableData.end_time;
        ClubManagement.instance.currentSelectedTableName = tableInfoScript.tableData.table_name;
        ClubManagement.instance.currentSelectedGameType = tableInfoScript.tableData.game_type;
        ClubManagement.instance.currentSelectedTableBlinds = tableInfoScript.tableData.small_blind.ToString("0.##") + "/" + tableInfoScript.tableData.big_blind.ToString("0.##");

        ClubManagement.instance.currentSelectedTableType = tableInfoScript.tableData.table_type;
        ClubManagement.instance.currentSelectedRuleId = tableInfoScript.tableData.rule_id;
        //ClubManagement.instance.currentSelectedTableBotEnabled = bool.Parse(panel.transform.GetChild(33).GetComponent<Text>().text);
        //ClubManagement.instance.currentSelectedTableBots = int.Parse(panel.transform.GetChild(34).GetComponent<Text>().text);
        ClubManagement.instance.isBottomPanelClose = false;


        if (ClubManagement.instance.currentSelectedTableType == "regular-table")
        {
            if (GameManagerScript.instance.isDev)
            {
                GameManagerScript.instance.socket.url = GameManagerScript.instance.regularTableDevUrl;
            }
            if (GameManagerScript.instance.isStaging)
            {
                GameManagerScript.instance.socket.url = GameManagerScript.instance.regularTableStagingUrl;
            }
            GameManagerScript.instance.isTournament = false;
            GameManagerScript.instance.socket.gameObject.SetActive(true);
            GameManagerScript.instance.networkManager.SetActive(true);
            PokerNetworkManager.instance.ObserveTable();
        }
        else
        {
            if (GameManagerScript.instance.isDev)
            {
                GameManagerScript.instance.Tournamentsocket.url = GameManagerScript.instance.tournamentTableDevUrl;
            }
            if (GameManagerScript.instance.isStaging)
            {
                GameManagerScript.instance.Tournamentsocket.url = GameManagerScript.instance.tournamentTableStagingUrl;
            }
            TournamentGameDetail.instance.tableCancelledPanel.SetActive(false);
            GameManagerScript.instance.isTournament = true;
            GameManagerScript.instance.Tournamentsocket.gameObject.SetActive(true);
            GameManagerScript.instance.tournamentManager.SetActive(true);
            TournamentManagerScript.instance.ConnectToServer();

        }
        PlayerPrefs.SetInt("SceneChange", 0);
        CancelInvoke();
    }

    #region Logout 

    [Serializable]
    public class LogoutRequest
    {
        public string username;
    }
    [Serializable]
    public class LogoutResponse
    { 
        public bool error;
        public bool logout;
    }
    [SerializeField] LogoutRequest logoutRequest;
    [SerializeField] LogoutResponse logoutResponse;


    public void LogoutRequestAPI()
    {
        logoutRequest.username = Profile._instance.usernameInputField.text;
        string body = JsonUtility.ToJson(logoutRequest);
        print(body);

        Communication.instance.PostData(logoutUrl, body, LogoutCallback);
    }

    void LogoutCallback(string response)
    {
        print(response);

        if(!string.IsNullOrEmpty(response))
        {
           logoutResponse = JsonUtility.FromJson<LogoutResponse>(response);
            if (!logoutResponse.error)
            {
                if (logoutResponse.logout)
                {
                    // log out successfull..

                }
                
            }
        }
    }

    #endregion

}