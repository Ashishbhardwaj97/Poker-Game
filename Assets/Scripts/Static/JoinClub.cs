using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;

public class JoinClub : MonoBehaviour
{
    public static JoinClub instance;

    //public string joinClubUrl, createClubUrl, createClubMailVerifyUrl, createClubGenrateUrl;

    public InputField clubIdInputField;
    public InputField agentIdInputField;

    public GameObject clubPage;
    public GameObject shopPopUp;

    public GameObject buttonPanel, joinClubPage;
    public GameObject filterPanel;
    public GameObject filterBlackPanel;

    public GameObject favFilterPanel;
    public GameObject favFilterBlackPanel;

    public Text userName, clientId;

    public bool isOwnerInFilter;
    public bool isManagerInFilter;
    public bool isAccountantInFilter;
    public bool isPartnerInFilter;
    public bool isAgentInFilter;
    public bool isPlayerInFilter;

    public GameObject tutorialPanel;
    internal int tutorialCount;

    public List<GameObject> afterFilterInClubList;
    public List<GameObject> tutorialScreensList;

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
        public string club_id;
        public string agent_id;
    }

    [SerializeField]
    public PlayerData player;

    [SerializeField]
    public Cashier.ClaimSendResponse claimSendResponse;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        //joinClubUrl = ServerChanger.instance.domainURL + "api/v1/club/join-club";

        //createClubUrl = ServerChanger.instance.domainURL + "api/v1/club/create-club";
        //createClubGenrateUrl = ServerChanger.instance.domainURL + "api/v1/club/generate-code-club";
        //createClubMailVerifyUrl = ServerChanger.instance.domainURL + "api/v1/club/verify-club";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (shopPopUp.activeInHierarchy)
            {
                shopPopUp.SetActive(false);
            }

            else if(filterPanel.activeInHierarchy)
            {
                filterPanel.SetActive(false);
                filterBlackPanel.SetActive(false);
                ResetFilterValues();
            }
        }
    }

    public void ClearJoinClubInputField()
    {
        clubIdInputField.text = string.Empty;
        agentIdInputField.text = string.Empty;
    }

    public void joinClubButton()
    {
        player.club_id = clubIdInputField.text.ToString();
        player.agent_id = agentIdInputField.text.ToString();

        string body = JsonUtility.ToJson(player);
        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(joinClubUrl, body, LoginCallback);
    }

    void LoginCallback(string response)
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
                print("join correct...");

                joinClubPage.SetActive(false);

                buttonPanel.SetActive(true);
                Uimanager.instance.homePage.SetActive(true);

                //.............MailBox...............//
                MailBoxScripts._instance.MailBoxCallCount();
                //.......................................//
                Cashier.instance.toastMsg.text = "Request sent to the club owner.\nWe will notify you when he approves.";
                Cashier.instance.toastMsgPanel.SetActive(true);
            }
            else
            {
                print("join incorrect...");
                Cashier.instance.toastMsg.text = claimSendResponse.errors.club.properties.message;
                Cashier.instance.toastMsgPanel.SetActive(true);
            }

        }
    }

    public void OpenShopPopUp()
    {
        shopPopUp.SetActive(true);
    }

    public void CloseShopPopUp()
    {
        shopPopUp.SetActive(false);
    }

    public void OpenClubListing()
    {
        joinClubPage.SetActive(false);
        clubPage.SetActive(true);
    }

    public GameObject maskPlayerImage;
    public void OpenClubFilter()
    {
        filterBlackPanel.SetActive(true);
        filterPanel.SetActive(true);
        maskPlayerImage.GetComponent<Image>().color = new Color32(21, 23, 26, 255);
    }

    public void CloseClubFilter()
    {
        filterBlackPanel.SetActive(false);
        filterPanel.SetActive(false);
        maskPlayerImage.GetComponent<Image>().color = new Color32(33, 37, 43, 255);
    }

    #region Update days on ClubListing
    int monCount = 0;
    int tueCount = 0;
    int wedCount = 0;
    int thurCount = 0;
    int friCount = 0;
    int satCount = 0;
    int sunCount = 0;
    public void Monday()
    {
        monCount++;
        if (monCount % 2 == 0)
        {
            ClubManagement.instance.myClubsListing.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(32).GetChild(0).gameObject.SetActive(true);
            ClubManagement.instance.myClubsListing.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(32).GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            ClubManagement.instance.myClubsListing.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(32).GetChild(1).gameObject.SetActive(true);
            ClubManagement.instance.myClubsListing.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(32).GetChild(0).gameObject.SetActive(false);
        }
    }

    public void Tuesday()
    {
        tueCount++;
        if (tueCount % 2 == 0)
        {
            ClubManagement.instance.myClubsListing.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(33).GetChild(0).gameObject.SetActive(true);
            ClubManagement.instance.myClubsListing.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(33).GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            ClubManagement.instance.myClubsListing.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(33).GetChild(1).gameObject.SetActive(true);
            ClubManagement.instance.myClubsListing.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(33).GetChild(0).gameObject.SetActive(false);
        }
    }

    public void Wednesday()
    {
        wedCount++;
        if (wedCount % 2 == 0)
        {
            ClubManagement.instance.myClubsListing.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(34).GetChild(0).gameObject.SetActive(true);
            ClubManagement.instance.myClubsListing.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(34).GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            ClubManagement.instance.myClubsListing.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(34).GetChild(1).gameObject.SetActive(true);
            ClubManagement.instance.myClubsListing.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(34).GetChild(0).gameObject.SetActive(false);
        }
    }

    public void Thursday()
    {
        thurCount++;
        if (thurCount % 2 == 0)
        {
            ClubManagement.instance.myClubsListing.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(35).GetChild(0).gameObject.SetActive(true);
            ClubManagement.instance.myClubsListing.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(35).GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            ClubManagement.instance.myClubsListing.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(35).GetChild(1).gameObject.SetActive(true);
            ClubManagement.instance.myClubsListing.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(35).GetChild(0).gameObject.SetActive(false);
        }
    }

    public void Friday()
    {
        friCount++;
        if (friCount % 2 == 0)
        {
            ClubManagement.instance.myClubsListing.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(36).GetChild(0).gameObject.SetActive(true);
            ClubManagement.instance.myClubsListing.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(36).GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            ClubManagement.instance.myClubsListing.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(36).GetChild(1).gameObject.SetActive(true);
            ClubManagement.instance.myClubsListing.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(36).GetChild(0).gameObject.SetActive(false);
        }
    }

    public void Saturday()
    {
        satCount++;
        if (satCount % 2 == 0)
        {
            ClubManagement.instance.myClubsListing.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(37).GetChild(0).gameObject.SetActive(true);
            ClubManagement.instance.myClubsListing.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(37).GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            ClubManagement.instance.myClubsListing.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(37).GetChild(1).gameObject.SetActive(true);
            ClubManagement.instance.myClubsListing.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(37).GetChild(0).gameObject.SetActive(false);
        }
    }

    public void Sunday()
    {
        sunCount++;
        if (sunCount % 2 == 0)
        {
            ClubManagement.instance.myClubsListing.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(38).GetChild(0).gameObject.SetActive(true);
            ClubManagement.instance.myClubsListing.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(38).GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            ClubManagement.instance.myClubsListing.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(38).GetChild(1).gameObject.SetActive(true);
            ClubManagement.instance.myClubsListing.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(38).GetChild(0).gameObject.SetActive(false);
        }
    }
    #endregion

    #region ClubListing Filter
    int ownerCount = 0;
    int managerCount = 0;
    int partnerCount = 0;
    int agentCount = 0;
    int playerCount = 0;
    int accountantCount = 0;

    public void Owner()
    {
        ownerCount++;
        if (clubPage.activeInHierarchy)
        {
            if (ownerCount % 2 == 0)
            {
                isOwnerInFilter = false;
                filterPanel.transform.GetChild(1).GetChild(0).GetComponent<Text>().color = Color.gray;
                filterPanel.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
                filterPanel.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
            }
            else
            {
                isOwnerInFilter = true;
                filterPanel.transform.GetChild(1).GetChild(0).GetComponent<Text>().color = Color.white;
                filterPanel.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
                filterPanel.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
            }
        }
        else
        {
            if (ownerCount % 2 == 0)
            {
                isOwnerInFilter = false;
                favFilterPanel.transform.GetChild(1).GetChild(0).GetComponent<Text>().color = Color.gray;
                favFilterPanel.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
                favFilterPanel.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
            }
            else
            {
                isOwnerInFilter = true;
                favFilterPanel.transform.GetChild(1).GetChild(0).GetComponent<Text>().color = Color.white;
                favFilterPanel.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
                favFilterPanel.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
            }
        }
    }

    public void Manager()
    {
        managerCount++;
        if (clubPage.activeInHierarchy)
        {
            if (managerCount % 2 == 0)
            {
                isManagerInFilter = false;
                filterPanel.transform.GetChild(2).GetChild(0).GetComponent<Text>().color = Color.gray;
                filterPanel.transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
                filterPanel.transform.GetChild(2).GetChild(2).gameObject.SetActive(false);
            }
            else
            {
                isManagerInFilter = true;
                filterPanel.transform.GetChild(2).GetChild(0).GetComponent<Text>().color = Color.white;
                filterPanel.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
                filterPanel.transform.GetChild(2).GetChild(2).gameObject.SetActive(true);
            }
        }
        else
        {
            if (managerCount % 2 == 0)
            {
                isManagerInFilter = false;
                favFilterPanel.transform.GetChild(2).GetChild(0).GetComponent<Text>().color = Color.gray;
                favFilterPanel.transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
                favFilterPanel.transform.GetChild(2).GetChild(2).gameObject.SetActive(false);
            }
            else
            {
                isManagerInFilter = true;
                favFilterPanel.transform.GetChild(2).GetChild(0).GetComponent<Text>().color = Color.white;
                favFilterPanel.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
                favFilterPanel.transform.GetChild(2).GetChild(2).gameObject.SetActive(true);
            }
        }
    }

    public void Partner()
    {
        partnerCount++;
        if (clubPage.activeInHierarchy)
        {
            if (partnerCount % 2 == 0)
            {
                isPartnerInFilter = false;
                filterPanel.transform.GetChild(3).GetChild(0).GetComponent<Text>().color = Color.gray;
                filterPanel.transform.GetChild(3).GetChild(1).gameObject.SetActive(true);
                filterPanel.transform.GetChild(3).GetChild(2).gameObject.SetActive(false);
            }
            else
            {
                isPartnerInFilter = true;
                filterPanel.transform.GetChild(3).GetChild(0).GetComponent<Text>().color = Color.white;
                filterPanel.transform.GetChild(3).GetChild(1).gameObject.SetActive(false);
                filterPanel.transform.GetChild(3).GetChild(2).gameObject.SetActive(true);
            }
        }
        else
        {
            if (partnerCount % 2 == 0)
            {
                isPartnerInFilter = false;
                favFilterPanel.transform.GetChild(3).GetChild(0).GetComponent<Text>().color = Color.gray;
                favFilterPanel.transform.GetChild(3).GetChild(1).gameObject.SetActive(true);
                favFilterPanel.transform.GetChild(3).GetChild(2).gameObject.SetActive(false);
            }
            else
            {
                isPartnerInFilter = true;
                favFilterPanel.transform.GetChild(3).GetChild(0).GetComponent<Text>().color = Color.white;
                favFilterPanel.transform.GetChild(3).GetChild(1).gameObject.SetActive(false);
                favFilterPanel.transform.GetChild(3).GetChild(2).gameObject.SetActive(true);
            }
        }
    }

    public void Agent()
    {
        agentCount++;
        if (clubPage.activeInHierarchy)
        {
            if (agentCount % 2 == 0)
            {
                isAgentInFilter = false;
                filterPanel.transform.GetChild(4).GetChild(0).GetComponent<Text>().color = Color.gray;
                filterPanel.transform.GetChild(4).GetChild(1).gameObject.SetActive(true);
                filterPanel.transform.GetChild(4).GetChild(2).gameObject.SetActive(false);
            }
            else
            {
                isAgentInFilter = true;
                filterPanel.transform.GetChild(4).GetChild(0).GetComponent<Text>().color = Color.white;
                filterPanel.transform.GetChild(4).GetChild(1).gameObject.SetActive(false);
                filterPanel.transform.GetChild(4).GetChild(2).gameObject.SetActive(true);
            }
        }
        else
        {
            if (agentCount % 2 == 0)
            {
                isAgentInFilter = false;
                favFilterPanel.transform.GetChild(4).GetChild(0).GetComponent<Text>().color = Color.gray;
                favFilterPanel.transform.GetChild(4).GetChild(1).gameObject.SetActive(true);
                favFilterPanel.transform.GetChild(4).GetChild(2).gameObject.SetActive(false);
            }
            else
            {
                isAgentInFilter = true;
                favFilterPanel.transform.GetChild(4).GetChild(0).GetComponent<Text>().color = Color.white;
                favFilterPanel.transform.GetChild(4).GetChild(1).gameObject.SetActive(false);
                favFilterPanel.transform.GetChild(4).GetChild(2).gameObject.SetActive(true);
            }
        }
    }

    public void Player()
    {
        playerCount++;
        if (clubPage.activeInHierarchy)
        {
            if (playerCount % 2 == 0)
            {
                isPlayerInFilter = false;
                filterPanel.transform.GetChild(5).GetChild(0).GetComponent<Text>().color = Color.gray;
                filterPanel.transform.GetChild(5).GetChild(1).gameObject.SetActive(true);
                filterPanel.transform.GetChild(5).GetChild(2).gameObject.SetActive(false);
            }
            else
            {
                isPlayerInFilter = true;
                filterPanel.transform.GetChild(5).GetChild(0).GetComponent<Text>().color = Color.white;
                filterPanel.transform.GetChild(5).GetChild(1).gameObject.SetActive(false);
                filterPanel.transform.GetChild(5).GetChild(2).gameObject.SetActive(true);
            }
        }
        else
        {
            if (playerCount % 2 == 0)
            {
                isPlayerInFilter = false;
                favFilterPanel.transform.GetChild(5).GetChild(0).GetComponent<Text>().color = Color.gray;
                favFilterPanel.transform.GetChild(5).GetChild(1).gameObject.SetActive(true);
                favFilterPanel.transform.GetChild(5).GetChild(2).gameObject.SetActive(false);
            }
            else
            {
                isPlayerInFilter = true;
                favFilterPanel.transform.GetChild(5).GetChild(0).GetComponent<Text>().color = Color.white;
                favFilterPanel.transform.GetChild(5).GetChild(1).gameObject.SetActive(false);
                favFilterPanel.transform.GetChild(5).GetChild(2).gameObject.SetActive(true);
            }
        }
    }

    public void Accountant()
    {
        accountantCount++;
        if (clubPage.activeInHierarchy)
        {
            if (accountantCount % 2 == 0)
            {
                isAccountantInFilter = false;
                filterPanel.transform.GetChild(6).GetChild(0).GetComponent<Text>().color = Color.gray;
                filterPanel.transform.GetChild(6).GetChild(1).gameObject.SetActive(true);
                filterPanel.transform.GetChild(6).GetChild(2).gameObject.SetActive(false);
            }
            else
            {
                isAccountantInFilter = true;
                filterPanel.transform.GetChild(6).GetChild(0).GetComponent<Text>().color = Color.white;
                filterPanel.transform.GetChild(6).GetChild(1).gameObject.SetActive(false);
                filterPanel.transform.GetChild(6).GetChild(2).gameObject.SetActive(true);
            }
        }
        else
        {
            if (accountantCount % 2 == 0)
            {
                isAccountantInFilter = false;
                favFilterPanel.transform.GetChild(6).GetChild(0).GetComponent<Text>().color = Color.gray;
                favFilterPanel.transform.GetChild(6).GetChild(1).gameObject.SetActive(true);
                favFilterPanel.transform.GetChild(6).GetChild(2).gameObject.SetActive(false);
            }
            else
            {
                isAccountantInFilter = true;
                favFilterPanel.transform.GetChild(6).GetChild(0).GetComponent<Text>().color = Color.white;
                favFilterPanel.transform.GetChild(6).GetChild(1).gameObject.SetActive(false);
                favFilterPanel.transform.GetChild(6).GetChild(2).gameObject.SetActive(true);
            }
        }
    }
    #endregion

    public void ResetFilterValues()
    {
            filterPanel.transform.GetChild(1).GetChild(0).GetComponent<Text>().color = Color.gray;
            filterPanel.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
            filterPanel.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);

            filterPanel.transform.GetChild(2).GetChild(0).GetComponent<Text>().color = Color.gray;
            filterPanel.transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
            filterPanel.transform.GetChild(2).GetChild(2).gameObject.SetActive(false);

            filterPanel.transform.GetChild(3).GetChild(0).GetComponent<Text>().color = Color.gray;
            filterPanel.transform.GetChild(3).GetChild(1).gameObject.SetActive(true);
            filterPanel.transform.GetChild(3).GetChild(2).gameObject.SetActive(false);

            filterPanel.transform.GetChild(4).GetChild(0).GetComponent<Text>().color = Color.gray;
            filterPanel.transform.GetChild(4).GetChild(1).gameObject.SetActive(true);
            filterPanel.transform.GetChild(4).GetChild(2).gameObject.SetActive(false);

            filterPanel.transform.GetChild(5).GetChild(0).GetComponent<Text>().color = Color.gray;
            filterPanel.transform.GetChild(5).GetChild(1).gameObject.SetActive(true);
            filterPanel.transform.GetChild(5).GetChild(2).gameObject.SetActive(false);

            filterPanel.transform.GetChild(6).GetChild(0).GetComponent<Text>().color = Color.gray;
            filterPanel.transform.GetChild(6).GetChild(1).gameObject.SetActive(true);
            filterPanel.transform.GetChild(6).GetChild(2).gameObject.SetActive(false);



            favFilterPanel.transform.GetChild(1).GetChild(0).GetComponent<Text>().color = Color.gray;
            favFilterPanel.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
            favFilterPanel.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);

            favFilterPanel.transform.GetChild(2).GetChild(0).GetComponent<Text>().color = Color.gray;
            favFilterPanel.transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
            favFilterPanel.transform.GetChild(2).GetChild(2).gameObject.SetActive(false);

            favFilterPanel.transform.GetChild(3).GetChild(0).GetComponent<Text>().color = Color.gray;
            favFilterPanel.transform.GetChild(3).GetChild(1).gameObject.SetActive(true);
            favFilterPanel.transform.GetChild(3).GetChild(2).gameObject.SetActive(false);

            favFilterPanel.transform.GetChild(4).GetChild(0).GetComponent<Text>().color = Color.gray;
            favFilterPanel.transform.GetChild(4).GetChild(1).gameObject.SetActive(true);
            favFilterPanel.transform.GetChild(4).GetChild(2).gameObject.SetActive(false);

            favFilterPanel.transform.GetChild(5).GetChild(0).GetComponent<Text>().color = Color.gray;
            favFilterPanel.transform.GetChild(5).GetChild(1).gameObject.SetActive(true);
            favFilterPanel.transform.GetChild(5).GetChild(2).gameObject.SetActive(false);

            favFilterPanel.transform.GetChild(6).GetChild(0).GetComponent<Text>().color = Color.gray;
            favFilterPanel.transform.GetChild(6).GetChild(1).gameObject.SetActive(true);
            favFilterPanel.transform.GetChild(6).GetChild(2).gameObject.SetActive(false);

        ownerCount = 0;
        managerCount = 0;
        partnerCount = 0;
        agentCount = 0;
        playerCount = 0;
        accountantCount = 0;

        isOwnerInFilter = false;
        isManagerInFilter = false;
        isAccountantInFilter = false;
        isPartnerInFilter = false;
        isAgentInFilter = false;
        isPlayerInFilter = false;
    }

    public void ClickOnFilterApplyButton()
    {
        if (afterFilterInClubList.Count > 0)
            afterFilterInClubList.Clear();

        afterFilterInClubList = new List<GameObject>();

        if (isOwnerInFilter)
        {
            if (clubPage.activeInHierarchy)
            {
                afterFilterInClubList = afterFilterInClubList.Concat(ClubManagement.instance.ownerList).ToList();
            }
            else if (FavoriteClubScript.instance.favClubListingObject.activeInHierarchy)
            {
                afterFilterInClubList = afterFilterInClubList.Concat(FavoriteClubScript.instance.ownerList).ToList();
            }
        }
        if (isManagerInFilter)
        {
            if (clubPage.activeInHierarchy)
            {
                afterFilterInClubList = afterFilterInClubList.Concat(ClubManagement.instance.managerList).ToList();
            }
            else if (FavoriteClubScript.instance.favClubListingObject.activeInHierarchy)
            {
                afterFilterInClubList = afterFilterInClubList.Concat(FavoriteClubScript.instance.managerList).ToList();
            }
        }
        if (isAccountantInFilter)
        {
            if (clubPage.activeInHierarchy)
            {
                afterFilterInClubList = afterFilterInClubList.Concat(ClubManagement.instance.accountantList).ToList();
            }
            else if (FavoriteClubScript.instance.favClubListingObject.activeInHierarchy)
            {
                afterFilterInClubList = afterFilterInClubList.Concat(FavoriteClubScript.instance.accountantList).ToList();
            }
        }
        if (isPartnerInFilter)
        {
            if (clubPage.activeInHierarchy)
            {
                afterFilterInClubList = afterFilterInClubList.Concat(ClubManagement.instance.partnerList).ToList();
            }
            else if (FavoriteClubScript.instance.favClubListingObject.activeInHierarchy)
            {
                afterFilterInClubList = afterFilterInClubList.Concat(FavoriteClubScript.instance.partnerList).ToList();
            }
        }
        if (isAgentInFilter)
        {
            if (clubPage.activeInHierarchy)
            {
                afterFilterInClubList = afterFilterInClubList.Concat(ClubManagement.instance.agentList).ToList();
            }
            else if (FavoriteClubScript.instance.favClubListingObject.activeInHierarchy)
            {
                afterFilterInClubList = afterFilterInClubList.Concat(FavoriteClubScript.instance.agentList).ToList();
            }
        }
        if (isPlayerInFilter)
        {
            if (clubPage.activeInHierarchy)
            {
                afterFilterInClubList = afterFilterInClubList.Concat(ClubManagement.instance.playerList).ToList();
            }
            else if (FavoriteClubScript.instance.favClubListingObject.activeInHierarchy)
            {
                afterFilterInClubList = afterFilterInClubList.Concat(FavoriteClubScript.instance.playerList).ToList();
            }
        }

        if (clubPage.activeInHierarchy)
        {
            for (int i = 0; i < ClubManagement.instance.myClubScrollContent.transform.childCount; i++)
            {
                ClubManagement.instance.myClubScrollContent.transform.GetChild(i).gameObject.SetActive(false);
            }

            for (int i = 0; i < afterFilterInClubList.Count; i++)
            {
                afterFilterInClubList[i].transform.SetParent(null);
                afterFilterInClubList[i].transform.SetParent(ClubManagement.instance.myClubScrollContent.transform);
                afterFilterInClubList[i].SetActive(true);
            }
        }

        else if(FavoriteClubScript.instance.favClubListingObject.activeInHierarchy)
        {
            for (int i = 0; i < FavoriteClubScript.instance.favClubScrollContent.transform.childCount; i++)
            {
                FavoriteClubScript.instance.favClubScrollContent.transform.GetChild(i).gameObject.SetActive(false);
            }

            for (int i = 0; i < afterFilterInClubList.Count; i++)
            {
                afterFilterInClubList[i].transform.SetParent(null);
                afterFilterInClubList[i].transform.SetParent(FavoriteClubScript.instance.favClubScrollContent.transform);
                afterFilterInClubList[i].SetActive(true);
            }
        }
    }

    public void TutorialScreens()
    {
        tutorialCount++;

        for (int i = 0; i < tutorialScreensList.Count; i++)
        {
            tutorialScreensList[i].SetActive(false);
        }
        if (tutorialCount == 5)
        {
            tutorialPanel.SetActive(false);
        }
        if (tutorialCount < 5)
        {
            tutorialScreensList[tutorialCount].SetActive(true);
        }
    }

    public void SkipScreens()
    {
        for (int i = 0; i < tutorialScreensList.Count; i++)
        {
            tutorialScreensList[i].SetActive(false);
        }
        tutorialPanel.SetActive(false);
    }
}