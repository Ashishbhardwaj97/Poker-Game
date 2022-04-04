using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;

public class Cashier : MonoBehaviour
{
    public static Cashier instance;

    [Header("GameObject References")]
    public GameObject trade;
    public GameObject tradeImage;
    public GameObject tradeHistory;
    public GameObject tradeHistoryImage;
    public GameObject chipsRequested;
    public GameObject chipsRequestedImage;
    public GameObject cashierCanvas;
    public GameObject clubHomePage;
    public GameObject joinClubCanvas;
    public GameObject menuPanel;
    public GameObject groupByButton;
    public GameObject selectAllButton;
    public GameObject inputField;
    public GameObject requestChipsPanel;
    public GameObject tradeBottomPanel;
    public GameObject DemoClaimPage;
    public GameObject tickButton;
    public GameObject allButtonPanel;
    public GameObject TradeContent;
    public GameObject clubChipsPanel;
    public GameObject agentCreditPanel;
    public GameObject toastMsgPanel;

    [Header("Text References")]
    public Text clubChip;
    public Text playerChip;
    public Text individualChip;
    public Text currentClubId;
    public Text noPlayerCount;
    public Text noSendPlayerCount;
    public Text toastMsg;
    public Text availableChipBalanceForPlayer;
    public Text agentCreditBalance;

    [Header("Properties")]
    public List<GameObject> alphabeticallySortedList;
    public List<GameObject> chipSortedList;
    public List<GameObject> tradeMemberList;
    public Dictionary<GameObject, int> chipsList = new Dictionary<GameObject, int>();

    //private string sendClaimBackUrl;
    //private string sendClaimForAgentUrl;

    private int groupByCounter = 0;
    private int selectAllCounter = 0;
    private int groupByCount = 0;
    private int selectAllCount = 0;
    private int selectedPlayerCount;  

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        PlayerPrefs.SetInt("TickedItem", 0);
        //sendClaimBackUrl = ServerChanger.instance.domainURL + "api/v1/club/send-claim-chip";
        //sendClaimForAgentUrl = ServerChanger.instance.domainURL + "api/v1/club/agent-cashier-chips";
    }

    void Update()
    {
        if (trade.activeInHierarchy)
        {
            tradeBottomPanel.SetActive(true);
        }
        else
        {
            tradeBottomPanel.SetActive(false);
        }
        
        noPlayerCount.text = PlayerPrefs.GetInt("TickedItem").ToString();
        noSendPlayerCount.text = PlayerPrefs.GetInt("TickedItem").ToString();

    }

    public void RemoveAllTick()
    {
        for (int i = 0; i < trade.transform.GetChild(6).GetChild(0).GetChild(0).GetChild(0).childCount; i++)
        {
            trade.transform.GetChild(6).GetChild(0).GetChild(0).GetChild(0).GetChild(i).GetChild(6).GetChild(2).GetChild(1).gameObject.SetActive(false);
        }
        PlayerPrefs.SetInt("TickedItem", 0);
    }

    public void PanelTick(GameObject image)
    {
        
        if (image.activeInHierarchy)
        {
            image.SetActive(false);
            if (PlayerPrefs.GetInt("TickedItem") > 1)
            {
                PlayerPrefs.SetInt("TickedItem", PlayerPrefs.GetInt("TickedItem") - 1);
            }
        }
        else
        {
            image.SetActive(true);
            PlayerPrefs.SetInt("TickedItem", PlayerPrefs.GetInt("TickedItem") + 1);
        }
    }

    public void GroupByRolesTick()
    {
        groupByCount++;
        if (groupByCount % 2 == 0)
        {
            trade.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            trade.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
        }
    }

    public void SelectAllTick()
    {
        selectAllCount++;
        if (selectAllCount % 2 == 0)
        {
            trade.transform.GetChild(4).GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            trade.transform.GetChild(4).GetChild(0).gameObject.SetActive(true);
        }
    }
   
    public void Trade()
    {
        tradeHistory.SetActive(false);
        chipsRequested.SetActive(false);
        tradeImage.SetActive(true);
        tradeHistoryImage.SetActive(false);
        chipsRequestedImage.SetActive(false);
        inputField.SetActive(false);
        trade.transform.GetChild(1).transform.GetComponent<InputField>().text = string.Empty;
        cashierCanvas.transform.GetChild(12).gameObject.SetActive(false);
    }

    public void TradeHistory()
    {
        trade.SetActive(false);
        tradeHistory.SetActive(true);
        chipsRequested.SetActive(false);
        tradeImage.SetActive(false);
        tradeHistoryImage.SetActive(true);
        chipsRequestedImage.SetActive(false);
        if (ClubManagement.instance.currentRoleInSelectedClub == 3)   //.....For Player
        {
            inputField.SetActive(false);
        }
        else
        {
            inputField.SetActive(true);
            inputField.transform.GetComponent<InputField>().text = string.Empty;
        }
        cashierCanvas.transform.GetChild(12).gameObject.SetActive(false);
    }

    public void ChipsRequested()
    {
        trade.SetActive(false);
        tradeHistory.SetActive(false);
        tradeImage.SetActive(false);
        tradeHistoryImage.SetActive(false);
        chipsRequestedImage.SetActive(true);
        inputField.SetActive(false);
        cashierCanvas.transform.GetChild(12).gameObject.SetActive(true);
        cashierCanvas.transform.GetChild(12).GetComponent<InputField>().text = string.Empty;
    }

    public void BackCashierCanvas()
    {
        cashierCanvas.SetActive(false);
        menuPanel.SetActive(false);
        clubHomePage.SetActive(true);
        cashierCanvas.transform.GetChild(5).GetChild(2).GetChild(0).gameObject.SetActive(false);
        cashierCanvas.transform.GetChild(5).GetChild(4).GetChild(0).gameObject.SetActive(false);

        currentClubId.text = ClubManagement.instance._clubID;

        ClubManagement.instance.ClickMyClubs();
        ApiHitScript.instance.clubPage.SetActive(false);
        PlayerPrefs.SetInt("TickedItem", 0);

        cashierCanvas.transform.GetChild(9).GetChild(0).GetChild(2).gameObject.SetActive(false);
        cashierCanvas.transform.GetChild(9).GetChild(1).GetChild(2).gameObject.SetActive(false);
    }

    public void OpenJoinClub()
    {
        cashierCanvas.SetActive(false);
        joinClubCanvas.SetActive(true);
    }

    public void OpenMenu()
    {
        menuPanel.SetActive(true);
    }

    public void RequestChipsOn()
    {
        requestChipsPanel.SetActive(true);
    }

    public void RequestChipsOff()
    {
        requestChipsPanel.SetActive(false);
    }

    public void GroupBy()
    {
        groupByCounter++;
        if (groupByCounter % 2 == 0)
        {
            groupByButton.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            groupByButton.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void SelectAll()
    {
        selectAllCounter++;
        if (selectAllCounter % 2 == 0)
        {
            selectAllButton.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            selectAllButton.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    #region Click on Send/Claim chips to players

    [Serializable]
    public class UserData
    {
        public int txn_type;
        public List<Chips> chips;
    }

    [Serializable]
    public class Chips
    {
        public string club_id;
        public int chips_request;
        public int txn_type;
        public int chips;
        public string receiver_club_role;
        public string sender_club_role;
        public string agent_id;
        public string receiver_client_id;
        public string receiver_username;
        public string receiver_image;
        public string receiver_user_id;
    }

    [Serializable]
    public class ClaimSendResponse
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

    [SerializeField]
    public List<Chips> chips;

    [SerializeField]
    public ClaimSendResponse claimSendResponse;

    [SerializeField]
    public UserData userData;

    

    public void ClaimBackButton()
    {
        Uimanager.instance.DemoClaimPage.SetActive(false);
        Uimanager.instance.DemoSendOutPage.SetActive(false);
        Uimanager.instance.clubHomePage.SetActive(true);
        Uimanager.instance.amountvalue = Uimanager.instance.claimAmountValuedemo.text.ToString();
        print(Uimanager.instance.amountvalue);

        userData.chips.Clear();

        selectedPlayerCount = 0;
        for (int i = 0; i < ClubManagement.instance.getMemberList.data.Length; i++)
        {

            if (ClubManagement.instance.tradeScrollPanel.transform.GetChild(i).GetChild(6).GetChild(2).GetChild(1).gameObject.activeInHierarchy)
            {
                userData.chips.Add(new Chips());

                userData.chips[selectedPlayerCount].club_id = ClubManagement.instance._clubID;
                userData.chips[selectedPlayerCount].chips_request = 1;
                userData.chips[selectedPlayerCount].txn_type = 1;
                userData.chips[selectedPlayerCount].chips = Convert.ToInt32(Uimanager.instance.amountvalue);

                userData.chips[selectedPlayerCount].receiver_club_role = ClubManagement.instance.tradeScrollPanel.transform.GetChild(i).GetChild(2).GetChild(0).GetComponent<Text>().text;
                userData.chips[selectedPlayerCount].sender_club_role = ClubManagement.instance.currentSelectedClubRole;
                userData.chips[selectedPlayerCount].agent_id = ClubManagement.instance.tradeScrollPanel.transform.GetChild(i).GetChild(7).GetComponent<Text>().text;
                userData.chips[selectedPlayerCount].receiver_client_id = ClubManagement.instance.tradeScrollPanel.transform.GetChild(i).GetChild(1).GetComponent<Text>().text;
                userData.chips[selectedPlayerCount].receiver_username = ClubManagement.instance.tradeScrollPanel.transform.GetChild(i).GetChild(0).GetComponent<Text>().text;
                userData.chips[selectedPlayerCount].receiver_image = ClubManagement.instance.tradeScrollPanel.transform.GetChild(i).GetChild(8).GetComponent<Text>().text;
                userData.chips[selectedPlayerCount].receiver_user_id = ClubManagement.instance.tradeScrollPanel.transform.GetChild(i).GetChild(3).GetComponent<Text>().text;

                selectedPlayerCount = selectedPlayerCount + 1;
            }

        }
        userData.txn_type = 1;
        string body = JsonUtility.ToJson(userData);

        print(body);
        ClubManagement.instance.loadingPanel.SetActive(true);

        if (ClubManagement.instance.currentSelectedClubRole == "Owner" || ClubManagement.instance.currentSelectedClubRole == "Manager")
        {
            //Communication.instance.PostData(sendClaimBackUrl, body, ClaimBackCallback);
        }
        else if (ClubManagement.instance.currentSelectedClubRole == "Agent")
        {
            //Communication.instance.PostData(sendClaimForAgentUrl, body, ClaimBackCallback);
        }

    }

    void ClaimBackCallback(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("Some error in claim ...!");
        }
        else
        {
            print("response" + response);

            claimSendResponse = JsonUtility.FromJson<ClaimSendResponse>(response);


            if (!claimSendResponse.error)
            {
                print("correct...");
                Uimanager.instance.UIClaimBackButton();
                
            }
            else
            {
                print("incorrect...");
                toastMsg.text = claimSendResponse.errors.club.properties.message;
                toastMsgPanel.SetActive(true);

                Uimanager.instance.DemoClaimPage.SetActive(false);
                Uimanager.instance.clubHomePage.SetActive(true);
            }
            RemoveAllTick();

            //.............MailBox...............//
            MailBoxScripts._instance.MailBoxCallCount();
            //.......................................//
        }
    }

    public void SendOutButton()
    {

        Uimanager.instance.DemoClaimPage.SetActive(false);
        Uimanager.instance.DemoSendOutPage.SetActive(false);
        Uimanager.instance.clubHomePage.SetActive(true);
        Uimanager.instance.amountvalue = Uimanager.instance.SendoutAmountValuedemo.text.ToString();

        userData.chips.Clear();
        selectedPlayerCount = 0;

        for (int i = 0; i < ClubManagement.instance.getMemberList.data.Length; i++)
        {
            if (ClubManagement.instance.tradeScrollPanel.transform.GetChild(i).GetChild(6).GetChild(2).GetChild(1).gameObject.activeInHierarchy)
            {
                userData.chips.Add(new Chips());

                userData.chips[selectedPlayerCount].club_id = ClubManagement.instance._clubID;
                userData.chips[selectedPlayerCount].receiver_user_id = ClubManagement.instance.tradeScrollPanel.transform.GetChild(i).GetChild(3).GetComponent<Text>().text;
                userData.chips[selectedPlayerCount].receiver_client_id = ClubManagement.instance.tradeScrollPanel.transform.GetChild(i).GetChild(1).GetComponent<Text>().text;
                userData.chips[selectedPlayerCount].receiver_username = ClubManagement.instance.tradeScrollPanel.transform.GetChild(i).GetChild(0).GetComponent<Text>().text;
                userData.chips[selectedPlayerCount].receiver_image = ClubManagement.instance.tradeScrollPanel.transform.GetChild(i).GetChild(8).GetComponent<Text>().text;

                userData.chips[selectedPlayerCount].chips_request = 1;
                userData.chips[selectedPlayerCount].chips = Convert.ToInt32(Uimanager.instance.amountvalue);
                userData.chips[selectedPlayerCount].txn_type = 2;
                userData.chips[selectedPlayerCount].receiver_club_role = ClubManagement.instance.tradeScrollPanel.transform.GetChild(i).GetChild(2).GetChild(0).GetComponent<Text>().text;
                userData.chips[selectedPlayerCount].sender_club_role = ClubManagement.instance.currentSelectedClubRole;
                userData.chips[selectedPlayerCount].agent_id = ClubManagement.instance.tradeScrollPanel.transform.GetChild(i).GetChild(7).GetComponent<Text>().text;

                selectedPlayerCount = selectedPlayerCount + 1;
            }
        }
        userData.txn_type = 2;

        string body = JsonUtility.ToJson(userData);
        print(body);
        ClubManagement.instance.loadingPanel.SetActive(true);

        if (ClubManagement.instance.currentSelectedClubRole == "Owner" || ClubManagement.instance.currentSelectedClubRole == "Manager")
        {
            //Communication.instance.PostData(sendClaimBackUrl, body, SendoutCallback);
        }
        else if (ClubManagement.instance.currentSelectedClubRole == "Agent")
        {
            //Communication.instance.PostData(sendClaimForAgentUrl, body, SendoutCallback);
        }
        

    }
    void SendoutCallback(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("Some error in Send out ...!");
        }
        else
        {
            print("response" + response);

            claimSendResponse = JsonUtility.FromJson<ClaimSendResponse>(response);


            if (!claimSendResponse.error)
            {
                print("correct...");
                Uimanager.instance.UISendOutButton();
                
            }
            else
            {
                print("incorrect...");
                toastMsg.text = claimSendResponse.errors.club.properties.message;
                toastMsgPanel.SetActive(true);

                Uimanager.instance.DemoSendOutPage.SetActive(false);
                Uimanager.instance.clubHomePage.SetActive(true);
            }
            RemoveAllTick();

            //.............MailBox...............//
            MailBoxScripts._instance.MailBoxCallCount();
            //.......................................//
        }
    }

    #endregion

    #region Filters in Trade in Cashier

    public void AlphabetTick()
    {
        cashierCanvas.transform.GetChild(9).GetChild(0).GetChild(2).gameObject.SetActive(true);
        cashierCanvas.transform.GetChild(8).gameObject.SetActive(false);
        cashierCanvas.transform.GetChild(9).gameObject.SetActive(false);
        cashierCanvas.transform.GetChild(9).GetChild(1).GetChild(2).gameObject.SetActive(false);

        tradeMemberList = new List<GameObject>();

        for (int i = 0; i < ClubManagement.instance.tradeScrollPanel.transform.childCount; i++)
        {
            tradeMemberList.Add(ClubManagement.instance.tradeScrollPanel.transform.GetChild(i).gameObject);
        }

        alphabeticallySortedList = tradeMemberList.OrderBy(z => z.transform.GetChild(0).GetComponent<Text>().text).ToList();

        for (int i = 0; i < alphabeticallySortedList.Count; i++)
        {
            alphabeticallySortedList[i].transform.SetParent(null);
            alphabeticallySortedList[i].transform.SetParent(ClubManagement.instance.tradeScrollPanel.transform);
        }
    }


    public void ChipBalanceTick()
    {
        cashierCanvas.transform.GetChild(9).GetChild(1).GetChild(2).gameObject.SetActive(true);
        cashierCanvas.transform.GetChild(8).gameObject.SetActive(false);
        cashierCanvas.transform.GetChild(9).gameObject.SetActive(false);
        cashierCanvas.transform.GetChild(9).GetChild(0).GetChild(2).gameObject.SetActive(false);

        tradeMemberList = new List<GameObject>();
        chipsList.Clear();
        for (int i = 0; i < ClubManagement.instance.tradeScrollPanel.transform.childCount; i++)
        {
            tradeMemberList.Add(ClubManagement.instance.tradeScrollPanel.transform.GetChild(i).gameObject);
            chipsList.Add(ClubManagement.instance.tradeScrollPanel.transform.GetChild(i).gameObject, Convert.ToInt32(ClubManagement.instance.tradeScrollPanel.transform.GetChild(i).GetChild(6).GetChild(1).GetComponent<Text>().text));
        }

        foreach (KeyValuePair<GameObject, int> trade in chipsList.OrderBy(key => key.Value))
        {
            chipSortedList.Add(trade.Key.gameObject);
        }
        for (int i = 0; i < chipSortedList.Count; i++)
        {
            chipSortedList[i].transform.SetParent(null);
            chipSortedList[i].transform.SetParent(ClubManagement.instance.tradeScrollPanel.transform);
        }
    }

    public void OpenCashierFilter()
    {
        cashierCanvas.transform.GetChild(8).gameObject.SetActive(true);
        cashierCanvas.transform.GetChild(9).gameObject.SetActive(true);
    }

    public void CloseCashierFilter()
    {
        cashierCanvas.transform.GetChild(8).gameObject.SetActive(false);
        cashierCanvas.transform.GetChild(9).gameObject.SetActive(false);
    }

    #endregion

}

[Serializable]
public class PlayerData
{
    public string club_id;
    public int chips_request;
    public int chips;
    public string receiver_user_id;
    public string club_member_role;
    public string agent_id;
    public string sender_club_role;
}

[Serializable]
public class HistoryDetailsResponse
{
    public bool error;
    public HistoryDetails[] historyDetails;
}
[Serializable]
public class HistoryDetails
{
    public string chips;
    public string sender_username;
    public string username;
    public string sender_client_id;
    public string client_id;
    public string receiver_club_role;
    public string sender_club_role;
    public string receiver_username;
    public string receiver_client_id;
    public string createdAt;
    public int txn_type;
    public string receiver_image;
    public string user_image;
}