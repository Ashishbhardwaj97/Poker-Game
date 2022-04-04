using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;

public class ClubManagement : MonoBehaviour
{
    public static ClubManagement instance;

    [Header("GameObject Reference")]
    public GameObject myClubsListing;
    public GameObject joinedClubsListing;
    public GameObject myClubPanel;
    public GameObject joinedClubPanel;
    public GameObject myClubImage;
    public GameObject joinedClubImage;
    public GameObject myTablePanel;
    public GameObject myTableScrollContent;
    public GameObject memberScrollContent;
    public GameObject memberReqScrollContent;
    public GameObject addTableButton;
    public GameObject clubChipsPanel;
    public GameObject myClubScrollContent;
    public GameObject joinClubScrollContent;
    public GameObject clubPage;
    public GameObject clubHomePage;
    public GameObject loadingPanel;
    public GameObject clubHomeScreenScrollList;
    public GameObject menuFadePanel;
    public GameObject chipReqApplicationPanel;
    public GameObject memberPanel;
    public GameObject memberScrollPanel;
    public GameObject memberReqPanel;
    public GameObject memberReqScrollPanel;
    public GameObject createTablePage;
    public GameObject tradepanel;
    public GameObject tradeScrollPanel;
    public GameObject memberChipReqScrollContent;
    public GameObject chipReqPanel;
    public GameObject tradeHistoryPanel;
    public GameObject tradeHistoryScrollPanel;
    public GameObject clubImageLogo;
    public GameObject agentChipMsgButton;
    public GameObject agentChipMsgPanel;
    public GameObject agentCreditBal;
    public GameObject disbandClubUI;
    public GameObject memberProfile;
    public GameObject memberProfileForAgent;
    public GameObject reassignDownlinePanel;
    public GameObject quitClubPanel;
    public GameObject quitClubFadePanel;
    public GameObject activeStatusObj;
    public GameObject onlinePlayingListContent;
    public GameObject onlinePlayingListPanel;
    public GameObject onlineObservingListContent;
    public GameObject onlineObservingListPanel;
    public GameObject groupByRoleObj;
    public GameObject groupByRoleObjInTrade;
    public GameObject selectAllObjInTrade;
    public GameObject membersInCreateTableContent;
    public GameObject membersInCreateTablePanel;
    public GameObject tournamentTableContent;
    public GameObject tournamentTablePanel;
    public GameObject regularTableListingContent;
    public GameObject regularTableListingPanel;
    public GameObject regularTableListingScreen;
    public GameObject tournamentTableListingScreen;

    public List<GameObject> onlineMemberObj;    

    internal GameObject currentSelectedClubObject;
    internal GameObject currentSelectedMemberProfile;
    private GameObject scrollItemObj;
    private GameObject currentSelectedInMemberReqList;
    private GameObject currentSelectedMemberInChipReq;

    [Header("Text References")]
    public Text clubId;
    public Text clubMemberRole;
    public Text clubName;
    public Text members;
    public Text clubChipBalance;
    public Text individualClipBalance;

    public Text clubCity;
    public Text clubCountry;
    public Text playerUserName;
    public Text playerUserId;
    public Text CurrentChipsBalance;
    public Text chipValApplicationPanel;
    public Text convertedChips;
    public Text chipPanelText;
    public Text playingPlayerCountText;
    public Text observingPlayerCountText;
    public Text[] diamondText;

    [Header("InputField References")]
    public InputField amountInputField;
    public InputField clubMsg1;
    public InputField clubMsg2;
    public InputField agentChipMsgInputField;
    public InputField disbandClubIdInputField;
    public InputField diamondInputField;
    public InputField searchInCreateTableInputField;

    [Header("Button References")]
    public Button individualChipButton;

    [Header("Diamond Conversion Properties")]
    public int chipFactor;

    [Header("Table Listing Filter Properties")]
    public float microValMTTBuyIn;                // 20   
    public float smallMaxValMTTBuyIn;             // 80     
    public float midMaxValMTTBuyIn;               // 200
    [Space]
    public float microValRegularSmallBlind;       //.5
    public float smallMaxValRegularSmallBlind;    //2
    public float midMaxValRegularSmallBlind;      //5

    [Header("Table Listing Filters Images")]
    public List<GameObject> allImgInTableListingFilter;

    [Header("List Properties")]
    public List<GameObject> ownerList;
    public List<GameObject> partnerList;
    public List<GameObject> accountantList;
    public List<GameObject> managerList;
    public List<GameObject> agentList;
    public List<GameObject> playerList;
    [Space]

    public List<GameObject> ownerListInMembers;
    public List<GameObject> managerListInMembers;
    public List<GameObject> agentListInMembers;
    public List<GameObject> playerListInMembers;
    public List<GameObject> accountantListInMembers;
    public List<GameObject> partnerListInMembers;

    [Space]
    public List<GameObject> ownerListInTrade;
    public List<GameObject> managerListInTrade;
    public List<GameObject> agentListInTrade;
    public List<GameObject> playerListInTrade;
    public List<GameObject> accountantListInTrade;
    public List<GameObject> partnerListInTrade;

    [Space]

    public List<GameObject> myClubObjList;
    public List<GameObject> memList;
    public List<GameObject> memReqList;
    public List<GameObject> tradeMemList;
    public List<GameObject> tradeHistoryList;
    public List<GameObject> chipReqList;
    public List<GameObject> allTableItemObjList;

    //private string myClubUrl;
    //private string clubDetailsUrl;
    //private string memberListUrl;
    //private string memberReqListUrl;
    //private string allMemberReqListUrl;
    //private string updateMemberUrl;
    //private string chipReqDetailsUrl;
    //private string updatePlayerChipsUrl;
    //private string updateAllPlayerChipsUrl;
    //private string tradeHistoryUrl;
    //private string sendChipRequestUrl;
    //private string updateWelcomeMsgUrl;
    //private string disbandClubUrl;
    //private string diamondExchangeUrl;
    //private string agentConfirmRejectAllChipsUrl;
    //private string currentPlayerUrl;
    //private string deleteClubMemberUrl;
    //private string downlineMemberListUrl;

    private string tableListUrl;

    internal string _clubID;
    internal string _clubName;
    internal string _clubMember;
    internal string _clubOwnerUserId;
    internal string selectedClubUserId;
    internal string currentSelectedAgentId;
    internal string userPassword;
    internal string currentSelectedTableId;
    internal string currentSelectedTableName;
    internal string currentSelectedGameType;
    internal string currentSelectedTableBlinds;
    internal string currentSelectedTableStartTime;
    internal string currentSelectedTableEndTime;
    internal string currentSelectedRuleId;
    public string currentSelectedTableType;
    public string currentSelectedStartDateTime;

    internal int currentSelectedTableMinAutoStart;
    internal int currentSelectedTableSize;
    internal int currentSelectedTableActionTime;
    internal int currentSelectedTableBots;

    internal bool currentSelectedTableIsAutoStart;
    internal bool currentSelectedTableIsBuyInAuth;
    internal bool currentSelectedTableIsMississippiStraddle;
    internal bool currentSelectedTableIsVideoMode;
    internal bool currentSelectedTableBotEnabled;
    internal bool isClubDetailPage;
    internal bool currentSelectedLimitWinPlayer;

    internal int MyclubConut;
    internal int memberConut;
    internal int memberReqCount;
    internal int memberChipConut;
    internal int tradeaMemberConut;
    internal int playerChipCount;

    internal bool isPlayer;

    bool isPlayingTime;
    bool isPlayingTime1;

    [Serializable]
    public class MyClubList
    {
        public bool error;
        public MyClubData[] myClubData;
    }

    [Serializable]
    public class MyClubData
    {
        public string user_id;
        public string club_name;
        public string club_id;
        public int joining;
        public int agent_joining;
        public int no_of_member;
        public string club_member_role;
        public string agent_id;
        public string country;
        public string city;
        public int totalChipRequest;
        public int agent_totalChipRequest;
        public int is_favorite;
        public string upload_logo;
    }

    [Serializable]
    public class GetTableList
    {
        public bool error;
        public ClubDetailsData clubDetailsData;
        
    }


    [Serializable]
    public class ClubDetailsData
    {
        public string club_id;
        public string club_name;
        public int chips;
        public string club_role;
        public string country;
        public string agent_credit_balance;
        public string city;
        public string upload_logo;
        public string welcome_msg1;
        public string welcome_msg2;
        public string start_time;
        public string end_time;
        public float individual_chip_balance;
        public string user_id;
        public string club_member_role;
    }

    [Serializable]
    public class RegularTableData
    {
        public string club_id;
        public string game_type;
        public bool video_mode;
        public string table_name;
        public int table_size;
        public float small_blind;
        public float big_blind;
        public int min_buy_in;
        public int max_buy_in;
        public string start_time;
        public string start_date;
        public string end_time;
        public string active_player_count;
        public string table_id;
        public int action_time;
        public string individual_chip_balance;
        public string createdAt;
        public bool auto_start;
        public bool mississippi_straddle;
        public bool buy_in_authorization;
        public int min_auto_start;
        public string rule_id;
        public string table_type;
        public int status;
        public bool bot_enabled;
        public int bots;
        public bool limit_win_player;
        public CurrentPlayers[] current_players;

    }

    [Serializable]
    public class CurrentPlayers
    {

    }

    [Serializable]
    public class GetMemberList
    {
        public bool error;
        public Data[] data;
    }

    [Serializable]
    public class GetChipReqDetailsList
    {
        public bool error;
        public Data[] data;
    }

    [Serializable]
    public class Data
    {
        public string first_name;
        public string last_name;
        public string username;
        public string receiver_username;
        public string client_id;
        public string receiver_client_id;
        public string individual_chip;
        public string user_id;
        public string receiver_user_id;
        public float chips;
        public string club_member_role;
        public string user_image;
        public string sender_client_id;
        public string sender_username;
        public string createdAt;
        public string agent_id;
        public string country;
        public string city;
        public string country_flag;
        public string receiver_club_role;
        public string agent_credit_balance;
        public string updatedAt;
        public bool exit_status;
    }

    [Serializable]
    public class ClubDetail
    {
        public string club_id;
    }

    [Serializable]
    public class MemberList
    {
        public string club_id;
        public int club_role;
    }

    [Serializable]
    public class UpdateMember
    {
        public string club_id;
        public string joining_request;
        public string user_id;

    }

    [Serializable]
    public class UpdateAllMember
    {
        public int status;
        public string club_id;
        public int joining_request;
        public List<Joining> joining;

    }

    [Serializable]
    public class Joining
    {
        public string user_id;
    }

    [Serializable]
    public class UpdateAllMemberChips
    {
        public string club_id;
        public string sender_club_role;
        public int chips_request;
        public List<Chips> chips;
    }

    [Serializable]
    public class Chips
    {
        public string user_id;
        public int chips;
        public string club_member_role;
        public string agent_id;
    }

    [Serializable]
    class DiamondExchange
    {
        public string club_id;
        public int chips;
        public int diamond;
    }

    [Serializable]
    class DiamondExchangeResponse
    {
        public bool error;
        public int diamond;
    }

    [Serializable]
    class CurrentPlayerResponse
    {
        public bool error;
        public int count;
        public int player;
        public int observer;
        public OnlinePlayerList[] onlinePlayerList;
        public ObserverList[] observerList;
        public int activeTables;
        public int activeTour;
    }

    [Serializable]
    class OnlinePlayerList
    {
        public CurrentPalyers[] current_players;
        public string user_image;
        public string country_flag;
        public string client_id;
        public string username;

        public string table_type;
        public string end_time;
        public string game_type;
        public string table_name;
        public string table_id;
        public string start_time;
        public string rule_id;

        public int small_blind;
        public int big_blind;
        public int min_buy_in;
        public int min_auto_start;
        public int blinds_up;
        public int table_size;
        public int action_time;

        public bool mississippi_straddle;
        public bool auto_start;
        public bool buy_in_authorization;
        public bool video_mode;
    }
    [Serializable]
    class CurrentPalyers
    {
        public string playerName;
        public double joinedAt;
    }

    [Serializable]
    class CurrentObservers
    {
        public string playerName;
        public double joinedAt;
    }

    [Serializable]
    class ObserverList
    {
        public CurrentObservers[] current_observers;
        public string user_image;
        public string country_flag;
        public string client_id;
        public string username;

        public string table_type;
        public string end_time;
        public string game_type;
        public string table_name;
        public string table_id;
        public string start_time;
        public string rule_id;

        public int small_blind;
        public int big_blind;
        public int min_buy_in;
        public int min_auto_start;
        public int blinds_up;
        public int table_size;
        public int action_time;

        public bool mississippi_straddle;
        public bool auto_start;
        public bool buy_in_authorization;
        public bool video_mode;
    }

    [Serializable]
   public class TableListReq
    {
        public string club_id;
        public List<string> table_type;
    }

    [Serializable]
    public class TableListResponse
    {
        public bool error;
        public RegularTableData[] data;
    }

    [Header("Serialize Properties")]
    [SerializeField] MyClubList myClubList;

    [SerializeField] GetTableList getTableList;

    [SerializeField] ClubDetail clubDetail;

    [SerializeField] MemberList memberList;

    [SerializeField]
    public GetMemberList getMemberList;

    [SerializeField] UpdateMember updateMember;

    [SerializeField] UpdateAllMember updateAllMember;

    [SerializeField] UpdateAllMemberChips updateAllMemberChips;

    [SerializeField] PlayerData player;

    [SerializeField] GetChipReqDetailsList getChipReqDetailsList;

    [SerializeField] CurrentPlayerResponse currentPlayerResponse;
    [SerializeField] TableListReq tableListReq;
    [SerializeField] TableListResponse tableListResponse;

    public List<int> oldHour;
    public List<int> oldMin;

    public List<int> oldHour1;
    public List<int> oldMin1;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //myClubUrl = ServerChanger.instance.domainURL + "api/v1/club/my-club";
        //clubDetailsUrl = ServerChanger.instance.domainURL + "api/v1/club/club-details";
        //memberListUrl = ServerChanger.instance.domainURL + "api/v1/club/member-list";
        //memberReqListUrl = ServerChanger.instance.domainURL + "api/v1/club/member-request-list";
        //updateMemberUrl = ServerChanger.instance.domainURL + "api/v1/club/update-member";
        //chipReqDetailsUrl = ServerChanger.instance.domainURL + "api/v1/club/chips-request-details";
        //updatePlayerChipsUrl = ServerChanger.instance.domainURL + "api/v1/club/update-player-chips";
        //tradeHistoryUrl = ServerChanger.instance.domainURL + "api/v1/club/history-details";
        //sendChipRequestUrl = ServerChanger.instance.domainURL + "api/v1/club/sent-chips-request";
        //allMemberReqListUrl = ServerChanger.instance.domainURL + "api/v1/club/confirm-reject-joining-request";
        //updateAllPlayerChipsUrl = ServerChanger.instance.domainURL + "api/v1/club/confirm-reject-all-chips-request";
        //updateWelcomeMsgUrl = ServerChanger.instance.domainURL + "api/v1/club/update-welcome-message";
        //disbandClubUrl = ServerChanger.instance.domainURL + "api/v1/club/disband-club";
        //diamondExchangeUrl = ServerChanger.instance.domainURL + "api/v1/club/diamond-chip-exchange";
        //agentConfirmRejectAllChipsUrl = ServerChanger.instance.domainURL + "api/v1/club/agent-confirm-reject-all-chips";
        //currentPlayerUrl = ServerChanger.instance.domainURL + "api/v1/pokertable/current-player";
        //deleteClubMemberUrl = ServerChanger.instance.domainURL + "api/v1/club/delete-club-member";
        //downlineMemberListUrl = ServerChanger.instance.domainURL + "api/v1/club/downline-members-list";

        tableListUrl = ServerChanger.instance.domainURL + "api/v1/pokertable/table-list";

        MyclubConut = 1;
        memberConut = 1;
        memberReqCount = 1;
        memberChipConut = 1;
        tradeaMemberConut = 1;
        tradeaHistoryConut = 1;
        tableItemCount = 1;
        onlinePlayingListCount = 1;
        onlineObservingListCount = 1;
        memberConutInCreateTable = 1;
        regularTableItemCount = 1;
        tournamentTableItemCount = 1;

        allTableItemObjList = new List<GameObject>();
        myClubObjList = new List<GameObject>();
        memList = new List<GameObject>();
        memReqList = new List<GameObject>();
        tradeMemList = new List<GameObject>();
        tradeHistoryList = new List<GameObject>();
        userImage = new List<string>();
        chipReqList = new List<GameObject>();
        ownerList = new List<GameObject>();
        playerList = new List<GameObject>();
        managerList = new List<GameObject>();
        agentList = new List<GameObject>();
        accountantList = new List<GameObject>();
        partnerList = new List<GameObject>();
        onlinePlayingListObj = new List<GameObject>();
        onlineObservingListObj = new List<GameObject>();

        oldHour = new List<int>();
        oldMin = new List<int>();
        oldHour1 = new List<int>();
        oldMin1 = new List<int>();

        ownerListInMembers = new List<GameObject>();
        managerListInMembers = new List<GameObject>();
        agentListInMembers = new List<GameObject>();
        playerListInMembers = new List<GameObject>();
        accountantListInMembers = new List<GameObject>();
        partnerListInMembers = new List<GameObject>();

        ownerListInTrade = new List<GameObject>();
        managerListInTrade = new List<GameObject>();
        agentListInTrade = new List<GameObject>();
        playerListInTrade = new List<GameObject>();
        accountantListInTrade = new List<GameObject>();
        partnerListInTrade = new List<GameObject>();

        regularTableListObj = new List<GameObject>();
        tournamentTableListObj = new List<GameObject>();
        corotineList = new List<Coroutine>();
        mttCorotineList = new List<Coroutine>();
        sateliteTableList = new List<string>();
        sateliteTableListRuleId = new List<string>();
        sateliteTableListTime = new List<string>();
    }

    #region Click On My Clubs
    public void ClickMyClubs()
    {
    //    if (!clubPage.activeInHierarchy)
    //    {
            loadingPanel.SetActive(true);
            //Communication.instance.PostData(myClubUrl, ClickMyClubsProcess);
        //}

        Uimanager.instance.ClickOnClub();
    }

    internal string memberReqTotalCount;
    internal string chipReqTotalCount;
    internal string memberReqTotalCountForAgent;
    internal string chipReqTotalCountForAgent;

    void ClickMyClubsProcess(string response)
    {
        loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("error");
        }
        else
        {
            print("" + response);

            myClubList = JsonUtility.FromJson<MyClubList>(response);
            print("Token : " + Communication.instance.playerToken);
            if (myClubList.error)
            {
                myClubsListing.SetActive(false);
            }
            else
            {
                if (myClubList.myClubData.Length != MyclubConut)
                {
                    for (int i = MyclubConut; i < myClubList.myClubData.Length; i++)
                    {
                        MyclubConut++;
                        GenerateMyClubItem();
                    }
                }
                clubImage.Clear();
                ownerList.Clear();
                partnerList.Clear();
                accountantList.Clear();
                managerList.Clear();
                agentList.Clear();
                playerList.Clear();
                for (int i = 0; i < myClubList.myClubData.Length; i++)
                {
                    myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = myClubList.myClubData[i].club_name + "      ";

                    myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(1).GetComponent<Text>().text = myClubList.myClubData[i].club_id;

                    myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(3).GetComponent<Text>().text = myClubList.myClubData[i].club_member_role;
                    string str = myClubList.myClubData[i].club_member_role;
                    if (!string.IsNullOrEmpty(str))
                    {
                        if (str == "Accountant")
                        {
                            myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = str.Substring(0, 2);
                        }
                        else if (str == "Partner")
                        {
                            myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = str.Substring(0, 2);
                        }
                        else
                        {
                            myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = str.Substring(0, 1);
                        }
                        
                    }
                    myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(10).GetChild(0).GetComponent<Text>().text = myClubList.myClubData[i].club_member_role;
                    myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(4).GetComponent<Text>().text = myClubList.myClubData[i].no_of_member.ToString();

                    myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(5).GetComponent<Text>().text = myClubList.myClubData[i].agent_id;

                    myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(6).GetComponent<Text>().text = myClubList.myClubData[i].country;
                    myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(7).GetComponent<Text>().text = myClubList.myClubData[i].city;
                    myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(8).GetComponent<Text>().text = myClubList.myClubData[i].joining.ToString();
                    myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(9).GetComponent<Text>().text = myClubList.myClubData[i].totalChipRequest.ToString();
                    myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(12).GetComponent<Text>().text = myClubList.myClubData[i].user_id;

                    myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(13).GetComponent<Text>().text = myClubList.myClubData[i].agent_joining.ToString();
                    myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(14).GetComponent<Text>().text = myClubList.myClubData[i].agent_totalChipRequest.ToString();

                    myClubScrollContent.transform.GetChild(i).gameObject.SetActive(true);

                    string memberRole = myClubList.myClubData[i].club_member_role;

                    if (memberRole == "Player")
                    {
                        myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(2).gameObject.SetActive(false);
                        playerList.Add(myClubScrollContent.transform.GetChild(i).gameObject);
                    }

                    else if (memberRole == "Owner")
                    {
                        myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(2).gameObject.SetActive(true);
                        ownerList.Add(myClubScrollContent.transform.GetChild(i).gameObject);
                        myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = (myClubList.myClubData[i].joining + myClubList.myClubData[i].totalChipRequest).ToString();
                    }
                    else if (memberRole == "Manager")
                    {
                        myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(2).gameObject.SetActive(true);
                        managerList.Add(myClubScrollContent.transform.GetChild(i).gameObject);
                        myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = (myClubList.myClubData[i].joining + myClubList.myClubData[i].totalChipRequest).ToString();
                    }
                    else if (memberRole == "Agent")
                    {
                        myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(2).gameObject.SetActive(true);
                        agentList.Add(myClubScrollContent.transform.GetChild(i).gameObject);
                        myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = (myClubList.myClubData[i].agent_joining + myClubList.myClubData[i].agent_totalChipRequest).ToString();
                    }
                    else if (memberRole == "Partner")
                    {
                        myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(2).gameObject.SetActive(true);
                        partnerList.Add(myClubScrollContent.transform.GetChild(i).gameObject);
                        myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = (myClubList.myClubData[i].joining + myClubList.myClubData[i].totalChipRequest).ToString();
                    }
                    else if (memberRole == "Accountant")
                    {
                        myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(2).gameObject.SetActive(true);
                        accountantList.Add(myClubScrollContent.transform.GetChild(i).gameObject);
                        myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = (myClubList.myClubData[i].joining + myClubList.myClubData[i].totalChipRequest).ToString();
                    }

                    int reqCount = myClubList.myClubData[i].joining + myClubList.myClubData[i].totalChipRequest;

                    if (reqCount == 0)
                    {
                        myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(2).gameObject.SetActive(false);
                    }

                    //......................Categorize Fav club list.................//
                    if (myClubList.myClubData[i].is_favorite == 1)
                    {
                        myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(11).GetChild(0).gameObject.SetActive(false);
                        myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(11).GetChild(1).gameObject.SetActive(true);
                    }
                    else
                    {
                        myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(11).GetChild(0).gameObject.SetActive(true);
                        myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(11).GetChild(1).gameObject.SetActive(false);
                    }
                    //..............................................................//

                    //....................Download club image...........//

                    clubImage.Add(myClubList.myClubData[i].upload_logo);
                    //.................................................//
                }

                myClubsListing.SetActive(true);
                if (currentSelectedClubObject != null)  //........ For reset ...........//
                {
                    memberReqTotalCount = currentSelectedClubObject.transform.GetChild(0).GetChild(8).GetComponent<Text>().text;
                    chipReqTotalCount = currentSelectedClubObject.transform.GetChild(0).GetChild(9).GetComponent<Text>().text;

                    memberReqTotalCountForAgent = currentSelectedClubObject.transform.GetChild(0).GetChild(13).GetComponent<Text>().text;
                    chipReqTotalCountForAgent = currentSelectedClubObject.transform.GetChild(0).GetChild(14).GetComponent<Text>().text;
                }

                UpdateClubImage();
            }

        }
    }

    #region update Club Image

    private int p;
    private int totalImageCountForClub;
    private int countForClub;
    private int previousCountForClub;

    public List<string> clubImage;
    [SerializeField]
    public List<ClubImageInSequence> clubImageInSequence;

    public void ResetClubImages()
    {
        p = 0;
        totalImageCountForClub = 0;
        countForClub = 0;
        previousCountForClub = 0;
        clubImage.Clear();
        clubImageInSequence.Clear();
    }

    void UpdateClubImage()
    {
        if (clubImage.Count == previousCountForClub)
        {
            return;
        }

        clubImageInSequence.Clear();
        p = 0;
        totalImageCountForClub = 0;
        previousCountForClub = 0;

        clubImageInSequence = new List<ClubImageInSequence>();

        for (int i = 0; i < clubImage.Count; i++)
        {
            if (!string.IsNullOrEmpty(clubImage[i]))
            {
                print("UpdatePlayerImageFor Club ......");
                clubImageInSequence.Add(new ClubImageInSequence());
                loadingPanel.SetActive(true);
                clubImageInSequence[p].imgUrl = clubImage[i];
                clubImageInSequence[p].ClubImageProcess(clubImage[i]);

                p = p + 1;

            }

            previousCountForClub++;
        }
    }

    public void ApplyImageInClub()
    {
        print(p);
        print(totalImageCountForClub);
        if (p == totalImageCountForClub)
        {
            countForClub = 0;
            loadingPanel.SetActive(false);

            for (int i = 0; i < clubImage.Count; i++)
            {
                myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(11).GetComponent<Image>().sprite = Registration.instance.defaultClubImage.sprite;
            }

            for (int i = 0; i < clubImage.Count; i++)
            {
                if (!string.IsNullOrEmpty(clubImage[i]))
                {
                    myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(11).GetComponent<Image>().sprite = clubImageInSequence[countForClub].imgPic;
                    countForClub++;
                }
            }

        }
    }

    [Serializable]
    public class ClubImageInSequence
    {
        public string imgUrl;
        public Sprite imgPic;

        public void ClubImageProcess(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                Communication.instance.GetImage(url, ClubImageResponse);
            }
        }

        void ClubImageResponse(Sprite response)
        {
            if (response != null)
            {
                imgPic = response;

                if (instance.myClubsListing.activeInHierarchy)
                {
                    instance.totalImageCountForClub++;
                    instance.ApplyImageInClub();
                }

            }
        }
    }

    #endregion

    public void GenerateMyClubItem()
    {
        GameObject scrollItemObj = Instantiate(myClubPanel);
        scrollItemObj.transform.SetParent(myClubScrollContent.transform, false);

        myClubObjList.Add(scrollItemObj);

    }
    #endregion

    #region Click on Club Details

    #region Club Detail part-1
    public void ClickOnClubDetailForClubRole(GameObject panel)
    {
        //..........For Reset..............//
        if (tradeMemList.Count > 0)
        {
            for (int i = 0; i < tradeMemList.Count; i++)
            {
                Destroy(tradeMemList[i]);
            }

        }
        tradeaMemberConut = 1;
        tradeMemList.Clear();
        Cashier.instance.trade.SetActive(false);

        if (tradeHistoryList.Count > 0)
        {
            for (int i = 0; i < tradeHistoryList.Count; i++)
            {
                Destroy(tradeHistoryList[i]);
            }
        }
        tradeaHistoryConut = 1;
        tradeHistoryList.Clear();
        Cashier.instance.tradeHistory.SetActive(false);

        if (chipReqList.Count > 0)
        {
            for (int i = 0; i < chipReqList.Count; i++)
            {
                Destroy(chipReqList[i]);
            }
        }
        memberChipConut = 1;
        chipReqList.Clear();

        //......................................//

        clubImageLogo.transform.GetComponent<Image>().sprite = panel.transform.GetChild(0).GetChild(11).GetComponent<Image>().sprite;
        currentSelectedClubObject = panel;
        Cashier.instance.chipsRequested.SetActive(false);

        _clubName = panel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text;
        _clubMember = panel.transform.GetChild(0).GetChild(4).GetComponent<Text>().text;
        currentSelectedClubRole = panel.transform.GetChild(0).GetChild(3).GetComponent<Text>().text;

        receiverClubRole = panel.transform.GetChild(0).GetChild(3).GetComponent<Text>().text;
        currentSelectedAgentId = panel.transform.GetChild(0).GetChild(5).GetComponent<Text>().text;
        selectedClubUserId = panel.transform.GetChild(0).GetChild(12).GetComponent<Text>().text;
        if (panel.transform.GetChild(0).GetChild(3).GetComponent<Text>().text == "Player")
        {
            currentRoleInSelectedClub = 3;
            clubChipsPanel.SetActive(false);
            addTableButton.SetActive(false);
            individualClipBalance.gameObject.transform.parent.localPosition = new Vector3(370.5f, 85.5f, 0);

            Cashier.instance.allButtonPanel.SetActive(false);
            Cashier.instance.cashierCanvas.transform.GetChild(10).gameObject.SetActive(true);
            Cashier.instance.cashierCanvas.transform.GetChild(11).gameObject.SetActive(true);
            //Cashier.instance.cashierCanvas.transform.GetChild(6).GetChild(1).gameObject.SetActive(false);
            isPlayer = true;

            clubMsg1.enabled = false;
            clubMsg2.enabled = false;

            Cashier.instance.menuPanel.transform.GetChild(4).GetChild(2).gameObject.SetActive(false);
            Cashier.instance.menuPanel.transform.GetChild(5).GetChild(2).gameObject.SetActive(false);

            for (int i = 0; i < onlineMemberObj.Count; i++)
            {
                onlineMemberObj[i].SetActive(false);
            }
            agentChipMsgButton.SetActive(false);
            agentCreditBal.SetActive(false);
        }
        else if (panel.transform.GetChild(0).GetChild(3).GetComponent<Text>().text == "Owner" ||
                 panel.transform.GetChild(0).GetChild(3).GetComponent<Text>().text == "Manager")
        {
            if (panel.transform.GetChild(0).GetChild(3).GetComponent<Text>().text == "Manager")
            {
                currentRoleInSelectedClub = 2; //.......For Manager.....//
            }
            else
            {
                currentRoleInSelectedClub = 1; //.......For Owner......//
            }

            clubChipsPanel.SetActive(true);
            addTableButton.SetActive(true);
            addTableButton.transform.GetComponent<Button>().interactable = true;
            isPlayer = false;
            individualClipBalance.gameObject.transform.parent.localPosition = new Vector3(371, 5, 0);

            Cashier.instance.allButtonPanel.SetActive(true);
            Cashier.instance.cashierCanvas.transform.GetChild(10).gameObject.SetActive(false);
            Cashier.instance.cashierCanvas.transform.GetChild(11).gameObject.SetActive(false);            
            isPlayer = false;
            Cashier.instance.menuPanel.transform.GetChild(4).GetChild(2).gameObject.SetActive(true);
            Cashier.instance.menuPanel.transform.GetChild(5).GetChild(2).gameObject.SetActive(true);

            clubMsg1.enabled = true;
            clubMsg2.enabled = true;

            

            for (int i = 0; i < onlineMemberObj.Count; i++)
            {
                onlineMemberObj[i].SetActive(true);
            }
            agentChipMsgButton.SetActive(false);
            agentCreditBal.SetActive(false);
        }
        else if (panel.transform.GetChild(0).GetChild(3).GetComponent<Text>().text == "Agent")
        {
            currentRoleInSelectedClub = 4; //.......For Agent......//

            addTableButton.SetActive(false);
            agentChipMsgButton.SetActive(true);
            clubMsg1.enabled = false;
            clubMsg2.enabled = false;

            isPlayer = false;
            clubChipsPanel.SetActive(false);
            agentCreditBal.SetActive(true);
            individualClipBalance.gameObject.transform.parent.localPosition = new Vector3(371, 5, 0);
        }
        else if (panel.transform.GetChild(0).GetChild(3).GetComponent<Text>().text == "Accountant" || panel.transform.GetChild(0).GetChild(3).GetComponent<Text>().text == "Partner")
        {
            if(panel.transform.GetChild(0).GetChild(3).GetComponent<Text>().text == "Accountant")
            {
                currentRoleInSelectedClub = 5; //.......For Manager.....//
            }
            else
            {
                currentRoleInSelectedClub = 6; //.......For Manager.....//
            }
            clubMsg1.enabled = false;
            clubMsg2.enabled = false;            

            clubChipsPanel.SetActive(true);
            agentCreditBal.SetActive(false);

            addTableButton.SetActive(true);
            addTableButton.transform.GetComponent<Button>().interactable = false;

            isPlayer = false;
            individualClipBalance.gameObject.transform.parent.localPosition = new Vector3(371, 5, 0);

            Cashier.instance.allButtonPanel.SetActive(true);
            Cashier.instance.cashierCanvas.transform.GetChild(10).gameObject.SetActive(false);
            Cashier.instance.cashierCanvas.transform.GetChild(11).gameObject.SetActive(false);
            isPlayer = false;
            Cashier.instance.menuPanel.transform.GetChild(4).GetChild(2).gameObject.SetActive(true);
            Cashier.instance.menuPanel.transform.GetChild(5).GetChild(2).gameObject.SetActive(true);

        }
        
        memberReqTotalCount = panel.transform.GetChild(0).GetChild(8).GetComponent<Text>().text;
        chipReqTotalCount = panel.transform.GetChild(0).GetChild(9).GetComponent<Text>().text;

        memberReqTotalCountForAgent = panel.transform.GetChild(0).GetChild(13).GetComponent<Text>().text;
        chipReqTotalCountForAgent = panel.transform.GetChild(0).GetChild(14).GetComponent<Text>().text;

        Uimanager.instance.homePage.SetActive(false);
    }

    #endregion

    public void ClickOnClubDetails(Text _clubId)
    {
        isClubDetailPage = true;
        clubDetail.club_id = _clubId.text;

        _clubID = _clubId.text;

        string body = JsonUtility.ToJson(clubDetail);
        loadingPanel.SetActive(true);
        //Communication.instance.PostData(clubDetailsUrl, body, ClickOnClubDetailsProcess);
        //Communication.instance.PostData(currentPlayerUrl, body, CurrentPlayerProcess);

        //...........Table listing All........//
        ClickOnAllTableListing();
        
        //....................................//

    }

    public void ClickOnClubDetailsOnRefresh(Text _clubId)
    {
        clubDetail.club_id = _clubId.text;

        _clubID = _clubId.text;

        string body = JsonUtility.ToJson(clubDetail);
        loadingPanel.SetActive(true);
        //Communication.instance.PostData(clubDetailsUrl, body, ClickOnClubDetailsProcess);

        //Communication.instance.PostData(currentPlayerUrl, body, CurrentPlayerProcess2);
    }

    void CurrentPlayerProcess2(string response)
    {
        loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("error");
        }
        else
        {
            print("" + response);
            currentPlayerResponse = JsonUtility.FromJson<CurrentPlayerResponse>(response);
            if (!currentPlayerResponse.error)
            {
                members.text = currentPlayerResponse.count.ToString();

            }
        }
    }

    #region Click On All Table Listing

    public List<string> sateliteTableList;
    public List<string> sateliteTableListRuleId;
    public List<string> sateliteTableListTime;
    int tableItemCount;
    public void ClickOnAllTableListing()
    {
        tableListReq.club_id = _clubID;

        tableListReq.table_type = new List<string>();
        tableListReq.table_type.Add("regular-table");
        tableListReq.table_type.Add("tournament-table");

        string body1 = JsonUtility.ToJson(tableListReq);
        print("body1 : " + body1);
        print("url : " + tableListUrl);

        //Communication.instance.PostData(tableListUrl, body1, AllTableListingProcess);

        clubHomePage.transform.GetComponent<ClubHome>().All();
        DisableAllBtnImgInTableListingFilter();
    }
    void AllTableListingProcess(string response)
    {
        if (string.IsNullOrEmpty(response))
        {
            print("error");
        }
        else
        {
            print("response : " + response);

            tableListResponse = JsonUtility.FromJson<TableListResponse>(response);
            
            if (!tableListResponse.error)
            {
                if (tableListResponse.data.Length > 0)
                {
                    if (tableListResponse.data.Length != tableItemCount)
                    {
                        for (int i = tableItemCount; i < tableListResponse.data.Length; i++)
                        {
                            tableItemCount++;
                            GenerateTableItem();
                        }
                    }
                    StopCountDown();
                    sateliteTableList.Clear();
                    sateliteTableListRuleId.Clear();
                    sateliteTableListTime.Clear();
                    TournamentScript.instance.DestroySateliteTableItem();
                    for (int i = 0; i < tableListResponse.data.Length; i++)
                    {
                        myTableScrollContent.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = tableListResponse.data[i].table_name;
                        myTableScrollContent.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = tableListResponse.data[i].small_blind.ToString();
                        myTableScrollContent.transform.GetChild(i).GetChild(2).GetComponent<Text>().text = tableListResponse.data[i].big_blind.ToString();
                        myTableScrollContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text = tableListResponse.data[i].min_buy_in.ToString();
                        myTableScrollContent.transform.GetChild(i).GetChild(4).GetComponent<Text>().text = tableListResponse.data[i].table_size.ToString();
                        
                        myTableScrollContent.transform.GetChild(i).GetChild(7).GetComponent<Text>().text = tableListResponse.data[i].current_players.Length.ToString();
                        myTableScrollContent.transform.GetChild(i).GetChild(19).GetComponent<Text>().text = tableListResponse.data[i].table_id;
                        myTableScrollContent.transform.GetChild(i).GetChild(20).GetComponent<Text>().text = tableListResponse.data[i].action_time.ToString();
                        myTableScrollContent.transform.GetChild(i).GetChild(21).GetComponent<Text>().text = tableListResponse.data[i].auto_start.ToString();
                        myTableScrollContent.transform.GetChild(i).GetChild(22).GetComponent<Text>().text = tableListResponse.data[i].video_mode.ToString();
                        myTableScrollContent.transform.GetChild(i).GetChild(23).GetComponent<Text>().text = tableListResponse.data[i].mississippi_straddle.ToString();
                        myTableScrollContent.transform.GetChild(i).GetChild(24).GetComponent<Text>().text = tableListResponse.data[i].buy_in_authorization.ToString();
                        myTableScrollContent.transform.GetChild(i).GetChild(25).GetComponent<Text>().text = tableListResponse.data[i].min_auto_start.ToString();
                        myTableScrollContent.transform.GetChild(i).GetChild(27).GetComponent<Text>().text = tableListResponse.data[i].rule_id;
                        myTableScrollContent.transform.GetChild(i).GetChild(30).GetComponent<Text>().text = tableListResponse.data[i].start_date +"   "+ tableListResponse.data[i].start_time;
                        myTableScrollContent.transform.GetChild(i).GetChild(32).GetComponent<Text>().text = tableListResponse.data[i].status.ToString();
                        myTableScrollContent.transform.GetChild(i).GetChild(33).GetComponent<Text>().text = tableListResponse.data[i].bot_enabled.ToString();
                        myTableScrollContent.transform.GetChild(i).GetChild(34).GetComponent<Text>().text = tableListResponse.data[i].bots.ToString();
                        myTableScrollContent.transform.GetChild(i).GetChild(35).GetComponent<Text>().text = tableListResponse.data[i].limit_win_player.ToString();
                        Text text = myTableScrollContent.transform.GetChild(i).GetChild(31).GetComponent<Text>();
                        string dateTime = tableListResponse.data[i].start_date + " " + tableListResponse.data[i].start_time + ":00";
                        print("dateTime : " + dateTime);
                        double remainingSec = TimeDifference(dateTime);
                        corotineList.Add(StartCoroutine(RemainingTimerValue(remainingSec, text)));
                        if (remainingSec > 3600)
                        {
                            sateliteTableList.Add(tableListResponse.data[i].table_name);
                            sateliteTableListRuleId.Add(tableListResponse.data[i].rule_id);
                            sateliteTableListTime.Add(tableListResponse.data[i].start_date + " " +tableListResponse.data[i].start_time);
                        }
                        bool isVideo = tableListResponse.data[i].video_mode;

                        if (tableListResponse.data[i].buy_in_authorization)
                        {
                            myTableScrollContent.transform.GetChild(i).GetChild(28).gameObject.SetActive(true);
                        }
                        else
                        {
                            myTableScrollContent.transform.GetChild(i).GetChild(28).gameObject.SetActive(false);
                        }

                        if (tableListResponse.data[i].table_type == "tournament-table") 
                        {
                            //currentSelectedTableType = "tournament";
                            myTableScrollContent.transform.GetChild(i).GetChild(29).GetComponent<Text>().text = "tournament";
                            if (!isVideo)
                            {
                                myTableScrollContent.transform.GetChild(i).GetChild(13).GetComponent<Text>().text = "NLH(9 Max)";
                            }
                            else
                            {
                                myTableScrollContent.transform.GetChild(i).GetChild(13).GetComponent<Text>().text = "NLH(8 Max)";
                            }

                            myTableScrollContent.transform.GetChild(i).GetChild(8).GetComponent<Image>().color = new Color32(255, 197, 89, 255);
                            myTableScrollContent.transform.GetChild(i).GetChild(8).GetChild(0).GetComponent<Text>().text = "MTT";

                            myTableScrollContent.transform.GetChild(i).GetChild(5).GetComponent<Text>().text = tableListResponse.data[i].start_date.Substring(0, 5) + " [" + tableListResponse.data[i].start_time + "]";
                            myTableScrollContent.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
                            myTableScrollContent.transform.GetChild(i).GetChild(2).gameObject.SetActive(false);
                            myTableScrollContent.transform.GetChild(i).GetChild(11).gameObject.SetActive(false);
                            myTableScrollContent.transform.GetChild(i).GetChild(31).gameObject.SetActive(true);
                        }
                        else
                        {
                            //currentSelectedTableType = "regular";
                            myTableScrollContent.transform.GetChild(i).GetChild(29).GetComponent<Text>().text = "regular";
                            myTableScrollContent.transform.GetChild(i).GetChild(13).GetComponent<Text>().text = "Regular";
                            myTableScrollContent.transform.GetChild(i).GetChild(8).GetComponent<Image>().color = new Color32(3, 168, 124, 255);
                            myTableScrollContent.transform.GetChild(i).GetChild(8).GetChild(0).GetComponent<Text>().text = "NLH";

                            myTableScrollContent.transform.GetChild(i).GetChild(5).GetComponent<Text>().text = tableListResponse.data[i].start_time + " - " + tableListResponse.data[i].end_time;
                            myTableScrollContent.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
                            myTableScrollContent.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);
                            myTableScrollContent.transform.GetChild(i).GetChild(11).gameObject.SetActive(true);
                            myTableScrollContent.transform.GetChild(i).GetChild(31).gameObject.SetActive(false);
                        }

                       
                        if (isVideo)
                        {
                            myTableScrollContent.transform.GetChild(i).GetChild(9).gameObject.SetActive(true);
                            myTableScrollContent.transform.GetChild(i).GetChild(26).gameObject.SetActive(false);
                        }
                        else
                        {
                            myTableScrollContent.transform.GetChild(i).GetChild(9).gameObject.SetActive(false);
                            myTableScrollContent.transform.GetChild(i).GetChild(26).gameObject.SetActive(true);
                        }

                        myTableScrollContent.transform.GetChild(i).gameObject.SetActive(true);
                    }

                    clubHomeScreenScrollList.SetActive(true);
                    tournamentTableListingScreen.SetActive(false);
                    regularTableListingScreen.SetActive(false);
                    
                }
                else
                {
                    for (int i = 0; i < myTableScrollContent.transform.childCount; i++)
                    {
                        myTableScrollContent.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
            
        }

    }

    

    public void GenerateTableItem()
    {
        scrollItemObj = Instantiate(myTablePanel);
        scrollItemObj.transform.SetParent(myTableScrollContent.transform, false);
        allTableItemObjList.Add(scrollItemObj);
    }

    public void DestroyGeneratedTableItem()
    {
        for (int i = 0; i < allTableItemObjList.Count; i++)
        {
            Destroy(allTableItemObjList[i]);
        }
        allTableItemObjList.Clear();
        tableItemCount = 1;
    }

    #endregion

    #region Click on Regular Table Listing

    int regularTableItemCount;
    public List<GameObject> regularTableListObj;

    public void ClickOnRegular()
    {
        tableListReq.club_id = _clubID;

        tableListReq.table_type = new List<string>();
        tableListReq.table_type.Add("regular-table");
        
        string body1 = JsonUtility.ToJson(tableListReq);
        print("body1 : " + body1);
        
        //Communication.instance.PostData(tableListUrl, body1, RegularTableListingProcess);

        clubHomePage.transform.GetComponent<ClubHome>().Regular();
        DisableAllBtnImgInTableListingFilter();
    }

    void RegularTableListingProcess(string response)
    {
        if (string.IsNullOrEmpty(response))
        {
            print("error");
        }
        else
        {
            print("response : " + response);

            tableListResponse = JsonUtility.FromJson<TableListResponse>(response);
            //print("" + tableListResponse.data[0].table_name);
            //print("" + tableListResponse.data.Length);

            if (!tableListResponse.error)
            {
                if (tableListResponse.data.Length > 0)
                {
                    if (tableListResponse.data.Length != regularTableItemCount)
                    {
                        for (int i = regularTableItemCount; i < tableListResponse.data.Length; i++)
                        {
                            regularTableItemCount++;
                            GenerateRegularTableItem();
                        }
                    }
                    for (int i = 0; i < tableListResponse.data.Length; i++)
                    {
                        regularTableListingContent.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = tableListResponse.data[i].table_name;
                        regularTableListingContent.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = tableListResponse.data[i].small_blind.ToString();
                        regularTableListingContent.transform.GetChild(i).GetChild(2).GetComponent<Text>().text = tableListResponse.data[i].big_blind.ToString();
                        regularTableListingContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text = tableListResponse.data[i].min_buy_in.ToString();
                        regularTableListingContent.transform.GetChild(i).GetChild(4).GetComponent<Text>().text = tableListResponse.data[i].table_size.ToString();
                        regularTableListingContent.transform.GetChild(i).GetChild(5).GetComponent<Text>().text = tableListResponse.data[i].start_time;
                        regularTableListingContent.transform.GetChild(i).GetChild(6).GetComponent<Text>().text = tableListResponse.data[i].end_time;
                        regularTableListingContent.transform.GetChild(i).GetChild(7).GetComponent<Text>().text = tableListResponse.data[i].current_players.Length.ToString();
                        regularTableListingContent.transform.GetChild(i).GetChild(19).GetComponent<Text>().text = tableListResponse.data[i].table_id;
                        regularTableListingContent.transform.GetChild(i).GetChild(20).GetComponent<Text>().text = tableListResponse.data[i].action_time.ToString();
                        regularTableListingContent.transform.GetChild(i).GetChild(21).GetComponent<Text>().text = tableListResponse.data[i].auto_start.ToString();
                        regularTableListingContent.transform.GetChild(i).GetChild(22).GetComponent<Text>().text = tableListResponse.data[i].video_mode.ToString();
                        regularTableListingContent.transform.GetChild(i).GetChild(23).GetComponent<Text>().text = tableListResponse.data[i].mississippi_straddle.ToString();
                        regularTableListingContent.transform.GetChild(i).GetChild(24).GetComponent<Text>().text = tableListResponse.data[i].buy_in_authorization.ToString();
                        regularTableListingContent.transform.GetChild(i).GetChild(25).GetComponent<Text>().text = tableListResponse.data[i].min_auto_start.ToString();
                        regularTableListingContent.transform.GetChild(i).GetChild(27).GetComponent<Text>().text = tableListResponse.data[i].rule_id;
                        regularTableListingContent.transform.GetChild(i).GetChild(30).GetComponent<Text>().text = tableListResponse.data[i].start_date + "   " + tableListResponse.data[i].start_time;
                        regularTableListingContent.transform.GetChild(i).GetChild(31).GetComponent<Text>().text = tableListResponse.data[i].status.ToString();
                        regularTableListingContent.transform.GetChild(i).GetChild(29).GetComponent<Text>().text = "regular";
                        regularTableListingContent.transform.GetChild(i).GetChild(33).GetComponent<Text>().text = tableListResponse.data[i].bot_enabled.ToString();
                        regularTableListingContent.transform.GetChild(i).GetChild(34).GetComponent<Text>().text = tableListResponse.data[i].bots.ToString();
                        regularTableListingContent.transform.GetChild(i).GetChild(35).GetComponent<Text>().text = tableListResponse.data[i].limit_win_player.ToString();

                        if (tableListResponse.data[i].buy_in_authorization)
                        {
                            regularTableListingContent.transform.GetChild(i).GetChild(28).gameObject.SetActive(true);
                        }
                        else
                        {
                            regularTableListingContent.transform.GetChild(i).GetChild(28).gameObject.SetActive(false);
                        }

                        bool isVideo = tableListResponse.data[i].video_mode;
                        if (isVideo)
                        {
                            regularTableListingContent.transform.GetChild(i).GetChild(9).gameObject.SetActive(true);
                            regularTableListingContent.transform.GetChild(i).GetChild(26).gameObject.SetActive(false);
                        }
                        else
                        {
                            regularTableListingContent.transform.GetChild(i).GetChild(9).gameObject.SetActive(false);
                            regularTableListingContent.transform.GetChild(i).GetChild(26).gameObject.SetActive(true);
                        }

                        regularTableListingContent.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    
                }
                else
                {
                    for (int i = 0; i < regularTableListingContent.transform.childCount; i++)
                    {
                        regularTableListingContent.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                tournamentTableListingScreen.SetActive(false);
                regularTableListingScreen.SetActive(true);
                clubHomeScreenScrollList.SetActive(false);
            }

        }

    }

    public void GenerateRegularTableItem()
    {
        scrollItemObj = Instantiate(regularTableListingPanel);
        scrollItemObj.transform.SetParent(regularTableListingContent.transform, false);
        regularTableListObj.Add(scrollItemObj);
    }

    public void ResetRegularTableItem()
    {
        if (regularTableListObj.Count > 0)
        {
            for (int i = 0; i < regularTableListObj.Count; i++)
            {
                Destroy(regularTableListObj[i]);
            }

            regularTableListObj.Clear();
            regularTableItemCount = 1;
        }
    }

    #endregion

    #region Click on Tournament

    int tournamentTableItemCount;
    public List<GameObject> tournamentTableListObj;

    public void ClickOnTournament()
    {
        tableListReq.club_id = _clubID;

        tableListReq.table_type = new List<string>();
        
        tableListReq.table_type.Add("tournament-table");

        string body1 = JsonUtility.ToJson(tableListReq);
        print("body1 : " + body1);

        //Communication.instance.PostData(tableListUrl, body1, TournamentTableListingProcess);

        clubHomePage.transform.GetComponent<ClubHome>().MTT();
        DisableAllBtnImgInTableListingFilter();
    }

    void TournamentTableListingProcess(string response)
    {
        if (string.IsNullOrEmpty(response))
        {
            print("error");
        }
        else
        {
            print("response : " + response);

            tableListResponse = JsonUtility.FromJson<TableListResponse>(response);
            
            if (!tableListResponse.error)
            {
                if (tableListResponse.data.Length > 0)
                {
                    if (tableListResponse.data.Length != tournamentTableItemCount)
                    {
                        for (int i = tournamentTableItemCount; i < tableListResponse.data.Length; i++)
                        {
                            tournamentTableItemCount++;
                            GenerateTournamentTableItem();
                        }
                    }
                    StopCountDown();
                    for (int i = 0; i < tableListResponse.data.Length; i++)
                    {
                        tournamentTableContent.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = tableListResponse.data[i].table_name;
                        tournamentTableContent.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = tableListResponse.data[i].small_blind.ToString();
                        tournamentTableContent.transform.GetChild(i).GetChild(2).GetComponent<Text>().text = tableListResponse.data[i].big_blind.ToString();
                        tournamentTableContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text = tableListResponse.data[i].min_buy_in.ToString();
                        tournamentTableContent.transform.GetChild(i).GetChild(4).GetComponent<Text>().text = tableListResponse.data[i].table_size.ToString();
                        
                        tournamentTableContent.transform.GetChild(i).GetChild(7).GetComponent<Text>().text = tableListResponse.data[i].current_players.Length.ToString();
                        tournamentTableContent.transform.GetChild(i).GetChild(19).GetComponent<Text>().text = tableListResponse.data[i].table_id;
                        tournamentTableContent.transform.GetChild(i).GetChild(20).GetComponent<Text>().text = tableListResponse.data[i].action_time.ToString();
                        tournamentTableContent.transform.GetChild(i).GetChild(21).GetComponent<Text>().text = tableListResponse.data[i].auto_start.ToString();
                        tournamentTableContent.transform.GetChild(i).GetChild(22).GetComponent<Text>().text = tableListResponse.data[i].video_mode.ToString();
                        tournamentTableContent.transform.GetChild(i).GetChild(23).GetComponent<Text>().text = tableListResponse.data[i].mississippi_straddle.ToString();
                        tournamentTableContent.transform.GetChild(i).GetChild(24).GetComponent<Text>().text = tableListResponse.data[i].buy_in_authorization.ToString();
                        tournamentTableContent.transform.GetChild(i).GetChild(25).GetComponent<Text>().text = tableListResponse.data[i].min_auto_start.ToString();
                        tournamentTableContent.transform.GetChild(i).GetChild(27).GetComponent<Text>().text = tableListResponse.data[i].rule_id;
                        tournamentTableContent.transform.GetChild(i).GetChild(30).GetComponent<Text>().text = tableListResponse.data[i].start_date + "   " + tableListResponse.data[i].start_time;
                        tournamentTableContent.transform.GetChild(i).GetChild(32).GetComponent<Text>().text = tableListResponse.data[i].status.ToString();
                        tournamentTableContent.transform.GetChild(i).GetChild(5).GetComponent<Text>().text = tableListResponse.data[i].start_date.Substring(0, 5) + " [" + tableListResponse.data[i].start_time + "]";
                        tournamentTableContent.transform.GetChild(i).GetChild(29).GetComponent<Text>().text = "tournament";
                        tournamentTableContent.transform.GetChild(i).GetChild(33).GetComponent<Text>().text = tableListResponse.data[i].bot_enabled.ToString();
                        tournamentTableContent.transform.GetChild(i).GetChild(34).GetComponent<Text>().text = tableListResponse.data[i].bots.ToString();
                        tournamentTableContent.transform.GetChild(i).GetChild(35).GetComponent<Text>().text = tableListResponse.data[i].limit_win_player.ToString();

                        Text text = tournamentTableContent.transform.GetChild(i).GetChild(31).GetComponent<Text>();
                        string dateTime = tableListResponse.data[i].start_date + " " + tableListResponse.data[i].start_time + ":00";
                        print("dateTime : " + dateTime);
                        double remainingSec = TimeDifference(dateTime);
                        mttCorotineList.Add(StartCoroutine(RemainingTimerValue(remainingSec, text)));

                        if (tableListResponse.data[i].buy_in_authorization)
                        {
                            tournamentTableContent.transform.GetChild(i).GetChild(28).gameObject.SetActive(true);
                        }
                        else
                        {
                            tournamentTableContent.transform.GetChild(i).GetChild(28).gameObject.SetActive(false);
                        }

                        bool isVideo = tableListResponse.data[i].video_mode;
                        if (isVideo)
                        {
                            tournamentTableContent.transform.GetChild(i).GetChild(9).gameObject.SetActive(true);
                            tournamentTableContent.transform.GetChild(i).GetChild(26).gameObject.SetActive(false);

                            tournamentTableContent.transform.GetChild(i).GetChild(13).GetComponent<Text>().text = "NLH(8 Max)";
                        }
                        else
                        {
                            tournamentTableContent.transform.GetChild(i).GetChild(9).gameObject.SetActive(false);
                            tournamentTableContent.transform.GetChild(i).GetChild(26).gameObject.SetActive(true);

                            tournamentTableContent.transform.GetChild(i).GetChild(13).GetComponent<Text>().text = "NLH(9 Max)";
                        }

                        tournamentTableContent.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    
                }
                else
                {
                    for (int i = 0; i < tournamentTableContent.transform.childCount; i++)
                    {
                        tournamentTableContent.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                tournamentTableListingScreen.SetActive(true);
                regularTableListingScreen.SetActive(false);
                clubHomeScreenScrollList.SetActive(false);
            }

        }

    }

    public void GenerateTournamentTableItem()
    {
        scrollItemObj = Instantiate(tournamentTablePanel);
        scrollItemObj.transform.SetParent(tournamentTableContent.transform, false);
        tournamentTableListObj.Add(scrollItemObj);
    }

    public void ResetTournamentTableItem()
    {
        if (tournamentTableListObj.Count > 0)
        {
            for (int i = 0; i < tournamentTableListObj.Count; i++)
            {
                Destroy(tournamentTableListObj[i]);
            }

            tournamentTableListObj.Clear();
            tournamentTableItemCount = 1;
        }
    }

    #endregion

    #region Filters inside Table Listing

    public void DisableAllBtnImgInTableListingFilter()
    {
        for (int i = 0; i < allImgInTableListingFilter.Count; i++)
        {
            allImgInTableListingFilter[i].gameObject.SetActive(false);
        }
    }
    public void ClickFiltersInTableListing(GameObject img)
    {
        DisableAllBtnImgInTableListingFilter();
        img.SetActive(true);
    }

    #region NLH/PLO Filter
    public void ClickNLHPLOFilter(string tableType)
    {
        if (tableListResponse.data.Length > 0)
        {
            if (clubHomeScreenScrollList.activeInHierarchy)
            {
                for (int i = 0; i < myTableScrollContent.transform.childCount; i++)
                {
                    if (myTableScrollContent.transform.GetChild(i).GetChild(8).GetChild(0).GetComponent<Text>().text.Contains(tableType) ||
                        myTableScrollContent.transform.GetChild(i).GetChild(13).GetComponent<Text>().text.Contains(tableType))
                    {
                        myTableScrollContent.transform.GetChild(i).gameObject.SetActive(true);

                    }
                    else
                    {
                        myTableScrollContent.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }

            }
            else if (regularTableListingScreen.activeInHierarchy)
            {
                for (int i = 0; i < regularTableListingContent.transform.childCount; i++)
                {
                    if (regularTableListingContent.transform.GetChild(i).GetChild(8).GetChild(0).GetComponent<Text>().text.Contains(tableType))
                    {
                        regularTableListingContent.transform.GetChild(i).gameObject.SetActive(true);

                    }
                    else
                    {
                        regularTableListingContent.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
            else if (tournamentTableListingScreen.activeInHierarchy)
            {
                for (int i = 0; i < tournamentTableContent.transform.childCount; i++)
                {
                    if (tournamentTableContent.transform.GetChild(i).GetChild(13).GetComponent<Text>().text.Contains(tableType))
                    {
                        tournamentTableContent.transform.GetChild(i).gameObject.SetActive(true);

                    }
                    else
                    {
                        tournamentTableContent.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    #endregion

    #region Video/Non-video Filter
    public void ClickVideoFilter(bool isVideo)
    {
        if (tableListResponse.data.Length > 0)
        {
            if (clubHomeScreenScrollList.activeInHierarchy)
            {
                for (int i = 0; i < myTableScrollContent.transform.childCount; i++)
                {
                    if (isVideo)
                    {
                        if (!bool.Parse(myTableScrollContent.transform.GetChild(i).GetChild(22).GetComponent<Text>().text))
                        {
                            myTableScrollContent.transform.GetChild(i).gameObject.SetActive(false);
                        }
                        else
                        {
                            myTableScrollContent.transform.GetChild(i).gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        if (bool.Parse(myTableScrollContent.transform.GetChild(i).GetChild(22).GetComponent<Text>().text))
                        {
                            myTableScrollContent.transform.GetChild(i).gameObject.SetActive(false);
                        }
                        else
                        {
                            myTableScrollContent.transform.GetChild(i).gameObject.SetActive(true);
                        }
                    }

                }
            }

            else if (regularTableListingScreen.activeInHierarchy)
            {
                for (int i = 0; i < regularTableListingContent.transform.childCount; i++)
                {
                    if (isVideo)
                    {
                        if (!bool.Parse(regularTableListingContent.transform.GetChild(i).GetChild(22).GetComponent<Text>().text))
                        {
                            regularTableListingContent.transform.GetChild(i).gameObject.SetActive(false);
                        }
                        else
                        {
                            regularTableListingContent.transform.GetChild(i).gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        if (bool.Parse(regularTableListingContent.transform.GetChild(i).GetChild(22).GetComponent<Text>().text))
                        {
                            regularTableListingContent.transform.GetChild(i).gameObject.SetActive(false);
                        }
                        else
                        {
                            regularTableListingContent.transform.GetChild(i).gameObject.SetActive(true);
                        }
                    }
                }
            }
            else if (tournamentTableListingScreen.activeInHierarchy)
            {
                for (int i = 0; i < tournamentTableContent.transform.childCount; i++)
                {
                    if (isVideo)
                    {
                        if (!bool.Parse(tournamentTableContent.transform.GetChild(i).GetChild(22).GetComponent<Text>().text))
                        {
                            tournamentTableContent.transform.GetChild(i).gameObject.SetActive(false);
                        }
                        else
                        {
                            tournamentTableContent.transform.GetChild(i).gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        if (bool.Parse(tournamentTableContent.transform.GetChild(i).GetChild(22).GetComponent<Text>().text))
                        {
                            tournamentTableContent.transform.GetChild(i).gameObject.SetActive(false);
                        }
                        else
                        {
                            tournamentTableContent.transform.GetChild(i).gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
    }

    #endregion

    #region Micro/Small/Mid/High Filter
    public void ClickBlindsFilter(string type)
    {
        if (tableListResponse.data.Length > 0)
        {
            if (clubHomeScreenScrollList.activeInHierarchy)
            {
                for (int i = 0; i < myTableScrollContent.transform.childCount; i++)
                {
                    if (type == "Micro")
                    {
                        if (myTableScrollContent.transform.GetChild(i).GetChild(8).GetChild(0).GetComponent<Text>().text.Contains("MTT"))
                        {
                            if (float.Parse(myTableScrollContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text) < microValMTTBuyIn)
                            {
                                myTableScrollContent.transform.GetChild(i).gameObject.SetActive(true);
                            }
                            else
                            {
                                myTableScrollContent.transform.GetChild(i).gameObject.SetActive(false);
                            }
                        }

                        else
                        {
                            if (float.Parse(myTableScrollContent.transform.GetChild(i).GetChild(1).GetComponent<Text>().text) < microValRegularSmallBlind)
                            {
                                myTableScrollContent.transform.GetChild(i).gameObject.SetActive(true);
                            }
                            else
                            {
                                myTableScrollContent.transform.GetChild(i).gameObject.SetActive(false);
                            }
                        }
                    }
                    else if (type == "Small")
                    {
                        if (myTableScrollContent.transform.GetChild(i).GetChild(8).GetChild(0).GetComponent<Text>().text.Contains("MTT"))
                        {
                            if (float.Parse(myTableScrollContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text) >= microValMTTBuyIn &&
                                float.Parse(myTableScrollContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text) < smallMaxValMTTBuyIn
                                )
                            {
                                myTableScrollContent.transform.GetChild(i).gameObject.SetActive(true);
                            }
                            else
                            {
                                myTableScrollContent.transform.GetChild(i).gameObject.SetActive(false);
                            }
                        }

                        else
                        {
                            if (float.Parse(myTableScrollContent.transform.GetChild(i).GetChild(1).GetComponent<Text>().text) >= microValRegularSmallBlind &&
                                float.Parse(myTableScrollContent.transform.GetChild(i).GetChild(1).GetComponent<Text>().text) < smallMaxValRegularSmallBlind
                                )
                            {
                                myTableScrollContent.transform.GetChild(i).gameObject.SetActive(true);
                            }
                            else
                            {
                                myTableScrollContent.transform.GetChild(i).gameObject.SetActive(false);
                            }
                        }
                    }
                    else if (type == "Medium")
                    {
                        if (myTableScrollContent.transform.GetChild(i).GetChild(8).GetChild(0).GetComponent<Text>().text.Contains("MTT"))
                        {
                            if (float.Parse(myTableScrollContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text) >= smallMaxValMTTBuyIn &&
                                float.Parse(myTableScrollContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text) < midMaxValMTTBuyIn
                                )
                            {
                                myTableScrollContent.transform.GetChild(i).gameObject.SetActive(true);
                            }
                            else
                            {
                                myTableScrollContent.transform.GetChild(i).gameObject.SetActive(false);
                            }
                        }

                        else
                        {
                            if (float.Parse(myTableScrollContent.transform.GetChild(i).GetChild(1).GetComponent<Text>().text) >= smallMaxValRegularSmallBlind &&
                                float.Parse(myTableScrollContent.transform.GetChild(i).GetChild(1).GetComponent<Text>().text) < midMaxValRegularSmallBlind
                                )
                            {
                                myTableScrollContent.transform.GetChild(i).gameObject.SetActive(true);
                            }
                            else
                            {
                                myTableScrollContent.transform.GetChild(i).gameObject.SetActive(false);
                            }
                        }
                    }

                    else if (type == "High")
                    {
                        if (myTableScrollContent.transform.GetChild(i).GetChild(8).GetChild(0).GetComponent<Text>().text.Contains("MTT"))
                        {
                            if (float.Parse(myTableScrollContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text) >= midMaxValMTTBuyIn)
                            {
                                myTableScrollContent.transform.GetChild(i).gameObject.SetActive(true);
                            }
                            else
                            {
                                myTableScrollContent.transform.GetChild(i).gameObject.SetActive(false);
                            }
                        }

                        else
                        {
                            if (float.Parse(myTableScrollContent.transform.GetChild(i).GetChild(1).GetComponent<Text>().text) >= midMaxValRegularSmallBlind)
                            {
                                myTableScrollContent.transform.GetChild(i).gameObject.SetActive(true);
                            }
                            else
                            {
                                myTableScrollContent.transform.GetChild(i).gameObject.SetActive(false);
                            }
                        }
                    }

                }

            }
            else if (regularTableListingScreen.activeInHierarchy)
            {
                for (int i = 0; i < regularTableListingContent.transform.childCount; i++)
                {
                    if (type == "Micro")
                    {
                        if (float.Parse(regularTableListingContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text) < microValRegularSmallBlind)
                        {
                            regularTableListingContent.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else
                        {
                            regularTableListingContent.transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }
                    else if (type == "Small")
                    {
                        if (float.Parse(regularTableListingContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text) >= microValRegularSmallBlind &&
                                float.Parse(regularTableListingContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text) < smallMaxValRegularSmallBlind
                                )
                        {
                            regularTableListingContent.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else
                        {
                            regularTableListingContent.transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }
                    else if (type == "Medium")
                    {
                        if (float.Parse(regularTableListingContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text) >= smallMaxValRegularSmallBlind &&
                               float.Parse(regularTableListingContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text) < midMaxValRegularSmallBlind
                               )
                        {
                            regularTableListingContent.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else
                        {
                            regularTableListingContent.transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }

                    else if (type == "High")
                    {
                        if (float.Parse(regularTableListingContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text) >= midMaxValRegularSmallBlind)
                        {
                            regularTableListingContent.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else
                        {
                            regularTableListingContent.transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }
                }
            }
            else if (tournamentTableListingScreen.activeInHierarchy)
            {
                for (int i = 0; i < tournamentTableContent.transform.childCount; i++)
                {
                    if (type == "Micro")
                    {
                        if (float.Parse(tournamentTableContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text) < microValMTTBuyIn)
                        {
                            tournamentTableContent.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else
                        {
                            tournamentTableContent.transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }
                    else if (type == "Small")
                    {
                        if (float.Parse(tournamentTableContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text) >= microValMTTBuyIn &&
                                 float.Parse(tournamentTableContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text) < smallMaxValMTTBuyIn
                                 )
                        {
                            tournamentTableContent.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else
                        {
                            tournamentTableContent.transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }
                    else if (type == "Medium")
                    {
                        if (float.Parse(tournamentTableContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text) >= smallMaxValMTTBuyIn &&
                                 float.Parse(tournamentTableContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text) < midMaxValMTTBuyIn
                                 )
                        {
                            tournamentTableContent.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else
                        {
                            tournamentTableContent.transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }

                    else if (type == "High")
                    {
                        if (float.Parse(tournamentTableContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text) >= midMaxValMTTBuyIn)
                        {
                            tournamentTableContent.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else
                        {
                            tournamentTableContent.transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
    }

    #endregion

    #region Running Table Filter
    public void ClickRunningTableFilter()
    {
        if (tableListResponse.data.Length > 0)
        {
            if (clubHomeScreenScrollList.activeInHierarchy)
            {
                for (int i = 0; i < myTableScrollContent.transform.childCount; i++)
                {
                    if (myTableScrollContent.transform.GetChild(i).GetChild(32).GetComponent<Text>().text == "1")
                    {
                        myTableScrollContent.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    else
                    {
                        myTableScrollContent.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }

            }
            else if (regularTableListingScreen.activeInHierarchy)
            {
                for (int i = 0; i < regularTableListingContent.transform.childCount; i++)
                {
                    if (regularTableListingContent.transform.GetChild(i).GetChild(31).GetComponent<Text>().text == "1")
                    {
                        regularTableListingContent.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    else
                    {
                        regularTableListingContent.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
            else if (tournamentTableListingScreen.activeInHierarchy)
            {
                for (int i = 0; i < tournamentTableContent.transform.childCount; i++)
                {
                    if (tournamentTableContent.transform.GetChild(i).GetChild(32).GetComponent<Text>().text == "1")
                    {
                        tournamentTableContent.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    else
                    {
                        tournamentTableContent.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    #endregion

    #region Empty Table Filter
    public void ClickEmptyTableFilter()
    {
        if (tableListResponse.data.Length > 0)
        {
            if (clubHomeScreenScrollList.activeInHierarchy)
            {
                for (int i = 0; i < myTableScrollContent.transform.childCount; i++)
                {
                    if (myTableScrollContent.transform.GetChild(i).GetChild(8).GetChild(0).GetComponent<Text>().text.Contains("MTT"))
                    {
                        if (myTableScrollContent.transform.GetChild(i).GetChild(31).GetComponent<Text>().text == "00:00:00")
                        {
                            myTableScrollContent.transform.GetChild(i).gameObject.SetActive(false);
                        }
                        else
                        {
                            myTableScrollContent.transform.GetChild(i).gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        if (myTableScrollContent.transform.GetChild(i).GetChild(7).GetComponent<Text>().text == myTableScrollContent.transform.GetChild(i).GetChild(4).GetComponent<Text>().text)
                        {
                            myTableScrollContent.transform.GetChild(i).gameObject.SetActive(false);
                        }
                        else
                        {
                            myTableScrollContent.transform.GetChild(i).gameObject.SetActive(true);
                        }
                    }
                }
            }

            else if (regularTableListingScreen.activeInHierarchy)
            {
                for (int i = 0; i < regularTableListingContent.transform.childCount; i++)
                {
                    if (regularTableListingContent.transform.GetChild(i).GetChild(7).GetComponent<Text>().text == myTableScrollContent.transform.GetChild(i).GetChild(4).GetComponent<Text>().text)
                    {
                        regularTableListingContent.transform.GetChild(i).gameObject.SetActive(false);
                    }
                    else
                    {
                        regularTableListingContent.transform.GetChild(i).gameObject.SetActive(true);
                    }
                }
            }
            else if (tournamentTableListingScreen.activeInHierarchy)
            {
                for (int i = 0; i < tournamentTableContent.transform.childCount; i++)
                {
                    if (tournamentTableContent.transform.GetChild(i).GetChild(31).GetComponent<Text>().text != "00:00:00")
                    {
                        tournamentTableContent.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    else
                    {
                        tournamentTableContent.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    #endregion

    #endregion

    #region Time Count Down

    double defaultVal;

    public double TimeDifference(string _dateTime)
    {
        if (_dateTime.Length == 19)
        {
            int day = Convert.ToInt32(_dateTime.Substring(0, 2));

            int month = Convert.ToInt32(_dateTime.Substring(3, 2));

            int year = Convert.ToInt32(_dateTime.Substring(6, 4));

            int hours = Convert.ToInt32(_dateTime.Substring(11, 2));

            int minutes = Convert.ToInt32(_dateTime.Substring(14, 2));

            int seconds = Convert.ToInt32(_dateTime.Substring(17, 2));

            DateTime now = DateTime.Now;

            string currentDate = now.ToString("dd/MM/yyyy HH:mm:ss");

            int day1 = Convert.ToInt32(currentDate.Substring(0, 2));
            int month1 = Convert.ToInt32(currentDate.Substring(3, 2));
            int year1 = Convert.ToInt32(currentDate.Substring(6, 4));
            int hours1 = Convert.ToInt32(currentDate.Substring(11, 2));
            int minutes1 = Convert.ToInt32(currentDate.Substring(14, 2));
            int seconds1 = Convert.ToInt32(currentDate.Substring(17, 2));

            DateTime target = new DateTime(year, month, day, hours, minutes, seconds);

            DateTime current = new DateTime(year1, month1, day1, hours1, minutes1, seconds1);

            TimeSpan travelTime = target - current;

            return travelTime.TotalSeconds;
        }
        return defaultVal;
    }

    public IEnumerator RemainingTimerValue(double _totalTableRemainingTime, Text _text)
    {
        int totalTableRemainingTime = (int)_totalTableRemainingTime;
        while (true)
        {
            if (totalTableRemainingTime > 0)
            {

                totalTableRemainingTime -= 1;
                RemainingDisplayTime(totalTableRemainingTime, _text);
                yield return new WaitForSeconds(1);
            }

            else
            {
                Debug.Log("Table has Ended!");
                _text.text = "00:00:00";
                break;
            }
        }
    }

    int totalTableRemainingTime = 0;
    public IEnumerator TimerIncreaseValue(Text _text, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        while (true)
        {
             totalTableRemainingTime += 1;
             RemainingDisplayTime(totalTableRemainingTime, _text);
             yield return new WaitForSeconds(1);
        }
    }

    internal bool isBottomPanelClose;
    internal bool resetStartTime;
    void RemainingDisplayTime(double _timeToDisplay, Text _text)
    {
        int timeToDisplay = (int)_timeToDisplay;

        timeToDisplay += 1;

        float hours = Mathf.FloorToInt(timeToDisplay / 3600);
        float minutes = Mathf.FloorToInt(timeToDisplay / 60) % 60;
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        if (_text != null)
        {
            _text.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        }

        int hourVal = int.Parse(SocialTournamentScript.instance.timerText.text.Substring(0, 2));
        int minVal = int.Parse(SocialTournamentScript.instance.timerText.text.Substring(3, 2));
        int secVal = int.Parse(SocialTournamentScript.instance.timerText.text.Substring(6));
        //print("seconds" + secVal);

        if (hourVal == 00 && minVal == 00 && secVal <= 15 /*&& secVal % 2 == 0*/ && !isBottomPanelClose && GameSerializeClassesCollection.instance.tournament.tournament_status == 0) 
        {
            TournamentManagerScript.instance.TimerEmitter();

            isBottomPanelClose = true;
            totalTableRemainingTime = 0;
            //TournamentGameDetail.instance.tournamentCountDownCorotine = TimerIncreaseValue(UIManagerScript.instance.tournamentIncreaseTimer, 15);
            //TournamentGameDetail.instance.StartTournamentCountDownCorotine();
        }
        //if (Uimanager.instance.currentTable != null)
        //{
        //if (!resetStartTime)
        //{
        //    TournamentGameDetail.instance.timerCountDownText.text = Uimanager.instance.currentTable.transform.GetChild(31).GetComponent<Text>().text;
        //}



        //if (TournamentGameDetail.instance.timerCountDownText.text == "00:00:59")
        //{
        //    TournamentManagerScript.instance.TimerEmitter();
        //}
        //}
    }

    List<Coroutine> corotineList;
    List<Coroutine> mttCorotineList;
    public void StopCountDown()
    {
        print("corotineList.Count : " + corotineList.Count);
        if (corotineList.Count > 0)
        {
            for (int i = 0; i < corotineList.Count; i++)
            {
                if (corotineList[i] != null)
                    StopCoroutine(corotineList[i]);
            }
            corotineList.Clear();
        }

        if (mttCorotineList.Count > 0)
        {
            for (int i = 0; i < mttCorotineList.Count; i++)
            {
                if (mttCorotineList[i] != null)
                    StopCoroutine(mttCorotineList[i]);
            }
            mttCorotineList.Clear();
        }
    }

    #endregion

    void ClickOnClubDetailsProcess(string response)
    {
        loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("error");
        }
        else
        {
            print("" + response);
            getTableList = JsonUtility.FromJson<GetTableList>(response);

            if (!getTableList.error)
            {
                string str = getTableList.clubDetailsData.club_member_role;
                if (str == "Accountant")
                {
                    clubName.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = str.Substring(0, 2);
                }
                else if (str == "Partner")
                {
                    clubName.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = str.Substring(0, 2);
                }
                else
                {
                    clubName.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = str.Substring(0, 1);
                }
                clubMemberRole.text = str.Substring(0, 1);
                clubId.text = getTableList.clubDetailsData.club_id;
                clubName.text = getTableList.clubDetailsData.club_name + "      ";
                
                clubChipBalance.text = getTableList.clubDetailsData.chips.ToString();

                if (PlayerPrefs.GetInt("CurrentChips") == 1)
                {
                    CurrentChipsBalance.text = clubChipBalance.text;
                    
                    PlayerPrefs.SetInt("CurrentChips", 0);
                }
                
                _clubOwnerUserId = getTableList.clubDetailsData.user_id;
                clubSenderRole = getTableList.clubDetailsData.club_role;
                clubCity.text = getTableList.clubDetailsData.city;

                Cashier.instance.clubChip.text = clubChipBalance.text;

                clubCountry.text = getTableList.clubDetailsData.country;
                clubMsg1.text = getTableList.clubDetailsData.welcome_msg1;
                clubMsg2.text = getTableList.clubDetailsData.welcome_msg2;
                agentCreditBal.transform.GetChild(2).GetComponent<Text>().text = getTableList.clubDetailsData.agent_credit_balance;

                Cashier.instance.agentCreditBalance.text = getTableList.clubDetailsData.agent_credit_balance;

                int chipBal = Convert.ToInt32(getTableList.clubDetailsData.individual_chip_balance);
                if (chipBal <= 0)
                {
                    individualClipBalance.text = "0";
                    Cashier.instance.individualChip.text = "0";
                }
                else
                {
                    individualClipBalance.text = getTableList.clubDetailsData.individual_chip_balance.ToString();
                    Cashier.instance.individualChip.text = individualClipBalance.text;
                }

                //clubHomeScreenScrollList.SetActive(false);

                playerUserName.text = ApiHitScript.instance.userName.text;
                playerUserId.text = ApiHitScript.instance.clientId.text;

                //myTableScrollContent.SetActive(true);
                //myTablePanel.SetActive(true);
                clubHomePage.SetActive(true);
                createTablePage.SetActive(false);
            }
            else
            {
                clubId.text = _clubID;
                clubName.text = _clubName;
                myTablePanel.SetActive(false);
            }
            ClickClubDetails();
        }
    }
    public void ClickClubDetails()
    {
        clubPage.SetActive(false);

        clubHomePage.SetActive(true);
        createTablePage.SetActive(false);

    }

    public void ClickOnBackButton()
    {
        isClubDetailPage = false;
        StopCountDown();
        if (Uimanager.instance.isHomePage)
        {
            clubPage.SetActive(false);

            clubHomePage.SetActive(false);
            Uimanager.instance.homePage.SetActive(true);
        }
        else if (Uimanager.instance.isClubListingPage)
        {
            clubPage.SetActive(true);

            clubHomePage.SetActive(false);
        }

        JoinClub.instance.ResetFilterValues();

        DestroyGeneratedTableItem();
        ResetRegularTableItem();
        ResetTournamentTableItem();
        ResetAllValuesForImage();
        ResetAllValuesForImageInCreateTable();
        ResetGenerateMembersInCreateTable();

        ResetPlayerImageInTradeList();
        ResetPlayerImageInTradeHistory();
        ResetPlayerImageInChipReq();
        if (memList.Count > 0)
        {
            for (int i = 0; i < memList.Count; i++)
            {
                Destroy(memList[i]);
            }
            memList.Clear();
        }
        memberConut = 1;

        ResetOnlineActivePlayers();
        MailBoxScripts._instance.DestroyMemberListObject();
        Admin.instance.DestroyGeneratedTableItem();
        Admin.instance.DestroyGeneratedPlayerWinItem();
        Admin.instance.DestroyGeneratedFeeItem();
    }


    public void ClickOnAddTable()
    {
        clubHomePage.SetActive(false);
        createTablePage.SetActive(true);
        createTablePage.transform.GetComponent<CreateTable>().ResetCreateTableUI();
        createTablePage.transform.GetComponent<CreateTable>().DefaultDateTimeReq();
    }

    public void ClickOnBackButtonCreateTableHomePage()
    {
        createTablePage.SetActive(false);
        clubHomePage.SetActive(true);
        createTablePage.transform.GetChild(1).GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(false);
        createTablePage.transform.GetChild(1).GetChild(1).GetChild(1).GetChild(0).gameObject.SetActive(false);
        createTablePage.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);

    }

    #endregion

    //..........Click on Member section ........//

    #region Click On Members in Menu Section

    #region Click On Members List
    public void ClickOnMembersList()
    {
        memberList.club_id = _clubID;
        memberList.club_role = AssignClubRoleInNumberFormat(currentSelectedClubObject.transform.GetChild(0).GetChild(3).GetComponent<Text>().text);
        string body = JsonUtility.ToJson(memberList);
        print(body);
        loadingPanel.SetActive(true);
        //Communication.instance.PostData(memberListUrl, body, ClickOnMembersListProcess);

        Members.instance.OpenMembersList();
    }


    void ClickOnMembersListProcess(string response)
    {
        loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("error");
        }
        else
        {
            print("" + response);
            getMemberList = JsonUtility.FromJson<GetMemberList>(response);

            if (!getMemberList.error)
            {
                if (getMemberList.data.Length != memberConut)
                {
                    for (int i = memberConut; i < getMemberList.data.Length; i++)
                    {
                        memberConut++;
                        GenerateMembersItem();
                    }
                }
                userImage.Clear();
                prevGroupByList.Clear();
                ownerListInMembers.Clear();
                managerListInMembers.Clear();
                playerListInMembers.Clear();
                agentListInMembers.Clear();
                partnerListInMembers.Clear();
                accountantListInMembers.Clear();
                SearchingScript.instance.memberListName.Clear();
                clubRoleInMemberList.Clear();

                for (int i = 0; i < getMemberList.data.Length; i++)
                {
                    memberScrollContent.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = getMemberList.data[i].first_name + " " + getMemberList.data[i].last_name;
                    memberScrollContent.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = getMemberList.data[i].username + "      ";
                    memberScrollContent.transform.GetChild(i).GetChild(2).GetComponent<Text>().text = getMemberList.data[i].client_id;
                    memberScrollContent.transform.GetChild(i).GetChild(3).GetChild(1).GetComponent<Text>().text = getMemberList.data[i].chips.ToString();
                    
                    string _role = getMemberList.data[i].club_member_role;
                    if (!string.IsNullOrEmpty(_role))
                    {
                        if (_role == "Accountant")
                        {
                            memberScrollContent.transform.GetChild(i).GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = _role.Substring(0, 2);
                        }
                        else if (_role == "Partner")
                        {
                            memberScrollContent.transform.GetChild(i).GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = _role.Substring(0, 2);
                        }
                        else
                        {
                            memberScrollContent.transform.GetChild(i).GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = _role.Substring(0, 1);
                        }

                        memberScrollContent.transform.GetChild(i).GetChild(4).GetChild(0).GetComponent<Text>().text = getMemberList.data[i].club_member_role;
                        memberScrollContent.transform.GetChild(i).GetChild(12).GetComponent<Text>().text = getMemberList.data[i].club_member_role;
                    }
                    memberScrollContent.transform.GetChild(i).GetChild(8).GetComponent<Text>().text = getMemberList.data[i].country;
                    memberScrollContent.transform.GetChild(i).GetChild(9).GetComponent<Text>().text = getMemberList.data[i].city;
                    memberScrollContent.transform.GetChild(i).GetChild(10).GetComponent<Text>().text = getMemberList.data[i].user_id;
                    memberScrollContent.transform.GetChild(i).GetChild(11).GetComponent<Text>().text = getMemberList.data[i].country_flag;
                    
                    memberScrollContent.transform.GetChild(i).GetChild(13).GetComponent<Text>().text = getMemberList.data[i].agent_id;
                    memberScrollContent.transform.GetChild(i).GetChild(14).GetComponent<Text>().text = getMemberList.data[i].agent_credit_balance;
                    userImage.Add(getMemberList.data[i].user_image);
                    clubRoleInMemberList.Add(_role);

                    if (getMemberList.data[i].exit_status)
                    {
                        memberScrollContent.transform.GetChild(i).GetChild(5).gameObject.SetActive(true);
                        memberScrollContent.transform.GetChild(i).GetChild(16).gameObject.SetActive(false);

                    }
                    else
                    {
                        memberScrollContent.transform.GetChild(i).GetChild(5).gameObject.SetActive(false);
                        memberScrollContent.transform.GetChild(i).GetChild(16).gameObject.SetActive(true);
                        memberScrollContent.transform.GetChild(i).GetChild(16).GetChild(1).GetComponent<Text>().text = ConvertDateTimeIn24Hours(getMemberList.data[i].updatedAt);
                    }

                    if (_role == "Owner")
                    {
                        ownerListInMembers.Add(memberScrollContent.transform.GetChild(i).gameObject);
                    }
                    else if (_role == "Manager")
                    {
                        managerListInMembers.Add(memberScrollContent.transform.GetChild(i).gameObject);
                    }
                    else if (_role == "Agent")
                    {
                        agentListInMembers.Add(memberScrollContent.transform.GetChild(i).gameObject);
                    }
                    else if (_role == "Player")
                    {
                        playerListInMembers.Add(memberScrollContent.transform.GetChild(i).gameObject);
                    }
                    else if (_role == "Accountant")
                    {
                        accountantListInMembers.Add(memberScrollContent.transform.GetChild(i).gameObject);
                    }
                    else if (_role == "Partner")
                    {
                        partnerListInMembers.Add(memberScrollContent.transform.GetChild(i).gameObject);
                    }
                    prevGroupByList.Add(memberScrollContent.transform.GetChild(i).gameObject);
                    SearchingScript.instance.memberListName.Add(memberScrollContent.transform.GetChild(i));
                }
                UpdatePlayerImage();
                memberScrollPanel.SetActive(true);
                Members.instance.memberList.SetActive(true);
                FindDownlineMemberCount();
            }
        }
    }

    #region Downline Member count

    public List<string> clubRoleInMemberList;
    int downlineCount;
    int downlineCount1;
    int totalDownlineCount;
    [Serializable]
    class DownlineMemberRequest
    {
        public string club_id;
        public string agent_id;
    }

    [Serializable]
    class DownlineMemberResponse
    {
        public bool error;
        public int count;
    }

    [SerializeField] List<DownlineMemberCountInSequence> downlineMemberCountInSequence;
    public void FindDownlineMemberCount()
    {
        downlineCount = 0;
        downlineCount1 = 0;
        totalDownlineCount = 0;

        downlineMemberCountInSequence = new List<DownlineMemberCountInSequence>();

        for (int i = 0; i < clubRoleInMemberList.Count; i++)
        {
            memberScrollContent.transform.GetChild(i).GetChild(15).gameObject.SetActive(false);
        }
        for (int i = 0; i < clubRoleInMemberList.Count; i++)
        {
            if (clubRoleInMemberList[i] == "Agent")
            {
                downlineMemberCountInSequence.Add(new DownlineMemberCountInSequence());
                downlineMemberCountInSequence[downlineCount].DownlineMemberReq(memberScrollContent.transform.GetChild(i).GetChild(13).GetComponent<Text>().text);

                downlineCount = downlineCount + 1;
            }
        }
        
    }

    public void ApplyDownlineCount()
    {
        if (totalDownlineCount == downlineCount)
        {
            for (int i = 0; i < clubRoleInMemberList.Count; i++)
            {
                if (clubRoleInMemberList[i] == "Agent")
                {
                    memberScrollContent.transform.GetChild(i).GetChild(15).GetChild(0).GetComponent<Text>().text = downlineMemberCountInSequence[downlineCount1].count.ToString();
                    downlineCount1 = downlineCount1 + 1;
                    memberScrollContent.transform.GetChild(i).GetChild(15).gameObject.SetActive(true);
                }
            }
        }
    }

    [SerializeField] DownlineMemberRequest downlineMemberRequest;
    [SerializeField] DownlineMemberResponse downlineMemberResponse;

    [Serializable]
    public class DownlineMemberCountInSequence
    {
        public int count;

        public void DownlineMemberReq(string agentID)
        {
            instance.downlineMemberRequest.club_id = instance._clubID;
            instance.downlineMemberRequest.agent_id = agentID;
            string body = JsonUtility.ToJson(instance.downlineMemberRequest);
            print(body);
            //Communication.instance.PostData(instance.downlineMemberListUrl, body, DownlineMemberProcess);
        }

        void DownlineMemberProcess(string response)
        {
            print("DownlineMemberProcess" + response);
            if (!string.IsNullOrEmpty(response))
            {
                instance. downlineMemberResponse = JsonUtility.FromJson<DownlineMemberResponse>(response);
                if (!instance.downlineMemberResponse.error)
                {
                    count = instance.downlineMemberResponse.count;
                }
                instance.totalDownlineCount++;
                instance.ApplyDownlineCount();
            }
        }
    }

    #endregion

    public List<GameObject> afterGroupByList;
    public List<GameObject> prevGroupByList;
    
    public void ClickOnGroupBy()
    {
        if (groupByRoleObj.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            afterGroupByList.Clear();
            afterGroupByList = new List<GameObject>();
            afterGroupByList = afterGroupByList.Concat(ownerListInMembers).ToList();
            afterGroupByList = afterGroupByList.Concat(managerListInMembers).ToList();
            afterGroupByList = afterGroupByList.Concat(partnerListInMembers).ToList();
            afterGroupByList = afterGroupByList.Concat(accountantListInMembers).ToList();
            afterGroupByList = afterGroupByList.Concat(agentListInMembers).ToList();
            afterGroupByList = afterGroupByList.Concat(playerListInMembers).ToList();

            for (int i = 0; i < afterGroupByList.Count; i++)
            {
                afterGroupByList[i].transform.SetParent(null);
                afterGroupByList[i].transform.SetParent(memberScrollContent.transform);
            }
        }
        else
        {
            for (int i = 0; i < prevGroupByList.Count; i++)
            {
                prevGroupByList[i].transform.SetParent(null);
                prevGroupByList[i].transform.SetParent(memberScrollContent.transform);
            }
        }
    }

    void GenerateMembersItem()
    {
        scrollItemObj = Instantiate(memberPanel);
        scrollItemObj.transform.SetParent(memberScrollContent.transform, false);
        memList.Add(scrollItemObj);
    }

    #region update Player Image

    public List<string> userImage;
    public List<string> userImageForMemberReqList;

    [SerializeField]
    private List<PlayerImageInSequence> playerImageInSequence;

    [SerializeField]
    private List<PlayerImageInSequence> playerImageInSequenceForMemberReqList;

    private int k = 0;
    private int totalImageCount;
    private int count = 0;

    private int previousCountForMemberList;
    void UpdatePlayerImage()
    {
        if (userImage.Count == previousCountForMemberList)
        {
            return;
        }

        k = 0;
        previousCountForMemberList = 0;
        totalImageCount = 0;
        playerImageInSequence.Clear();

        playerImageInSequence = new List<PlayerImageInSequence>();

        for (int i = 0; i < userImage.Count; i++)
        {
            if (!string.IsNullOrEmpty(userImage[i]))
            {
                playerImageInSequence.Add(new PlayerImageInSequence());
                loadingPanel.SetActive(true);
                playerImageInSequence[k].imgUrl = userImage[i];
                playerImageInSequence[k].ImageProcess(userImage[i]);

                k = k + 1;

            }
            previousCountForMemberList++;
        }
    }

    public void ApplyImage()
    {
        print(k);
        print(totalImageCount);
        if (k == totalImageCount)
        {
            count = 0;
            loadingPanel.SetActive(false);

            for (int i = 0; i < userImage.Count; i++)
            {
                memberScrollContent.transform.GetChild(i).GetChild(7).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
            }

            for (int i = 0; i < userImage.Count; i++)
            {
                if (!string.IsNullOrEmpty(userImage[i]))
                {
                    print("i = " + i);
                    memberScrollContent.transform.GetChild(i).GetChild(7).GetComponent<Image>().sprite = playerImageInSequence[count].imgPic;
                    count++;
                }
            }

        }
    }

    [Serializable]
    public class PlayerImageInSequence
    {
        public string imgUrl;
        public Sprite imgPic;

        public void ImageProcess(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                Communication.instance.GetImage(url, ImageResponse);
            }
        }

        void ImageResponse(Sprite response)
        {
            if (response != null)
            {
                imgPic = response;

                if (instance.memberScrollPanel.activeInHierarchy)
                {
                    instance.totalImageCount++;
                    instance.ApplyImage();
                }
                else if (instance.memberReqScrollPanel.activeInHierarchy)
                {
                    instance.totalImageCountForMemberReqList++;
                    instance.ApplyImageInReqMemberList();
                }
                else if (Cashier.instance.trade.activeInHierarchy)
                {
                    instance.totalImageCountForTradeList++;
                    instance.ApplyImageInTradeList();
                }
                else if (Cashier.instance.tradeHistory.activeInHierarchy)
                {
                    instance.totalImageCountForTradeHistory++;
                    instance.ApplyImageInTradeHistory();
                }
                else if (Cashier.instance.chipsRequested.activeInHierarchy)
                {
                    instance.totalImageCountForChipReq++;
                    instance.ApplyImageInChipReq();
                }
            }
        }
    }

    public void ResetAllValuesForImage()
    {
        totalImageCount = 0;
        k = 0;
        j = 0;
        count = 0;
        previousCountForMemberReqList = 0;
        previousCountForMemberList = 0;
        userImage.Clear();
        playerImageInSequence.Clear();
        userImageForMemberReqList.Clear();
        playerImageInSequenceForMemberReqList.Clear();
    }

    #endregion

    #endregion

    #region Click On Members Request List

    [Serializable]
    class MemberReq
    {
        public string club_id;
        public int club_role;
        public string agent_id;
    }

    [SerializeField] MemberReq memberReq;

    public void ClickOnMembersReqList()
    {
        if (!memberReqScrollPanel.activeInHierarchy)
        {
            memberReq.club_id = _clubID;
            memberReq.club_role = currentRoleInSelectedClub;
            memberReq.agent_id = currentSelectedAgentId;
            string body = JsonUtility.ToJson(memberReq);
            print(body);
            loadingPanel.SetActive(true);
            //Communication.instance.PostData(memberReqListUrl, body, ClickOnMembersReqListProcess);
        }
        Members.instance.OpenRequestList();

        if(currentRoleInSelectedClub==5 || currentRoleInSelectedClub == 6)
        {
            Members.instance.requestList.transform.GetChild(1).GetComponent<Button>().interactable = false;
            Members.instance.requestList.transform.GetChild(2).GetComponent<Button>().interactable = false;
            for (int i = 0; i < memberReqScrollContent.transform.childCount; i++)
            {
                memberReqScrollContent.transform.GetChild(i).GetChild(13).GetComponent<Button>().interactable = false;
                memberReqScrollContent.transform.GetChild(i).GetChild(14).GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            Members.instance.requestList.transform.GetChild(1).GetComponent<Button>().interactable = true;
            Members.instance.requestList.transform.GetChild(2).GetComponent<Button>().interactable = true;
            for (int i = 0; i < memberReqScrollContent.transform.childCount; i++)
            {
                memberReqScrollContent.transform.GetChild(i).GetChild(13).GetComponent<Button>().interactable = true;
                memberReqScrollContent.transform.GetChild(i).GetChild(14).GetComponent<Button>().interactable = true;
            }
        }

    }


    void ClickOnMembersReqListProcess(string response)
    {
        loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("error");
        }
        else
        {
            print("" + response);
            getMemberList = JsonUtility.FromJson<GetMemberList>(response);

            if (!getMemberList.error)
            {
                if (getMemberList.data.Length != memberReqCount)
                {
                    for (int i = memberReqCount; i < getMemberList.data.Length; i++)
                    {
                        memberReqCount++;
                        GenerateMembersReqItem();
                    }
                }
                userImageForMemberReqList.Clear();
                for (int i = 0; i < getMemberList.data.Length; i++)
                {
                    memberReqScrollContent.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = getMemberList.data[i].first_name + " " + getMemberList.data[i].last_name;
                    memberReqScrollContent.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = getMemberList.data[i].username;
                    memberReqScrollContent.transform.GetChild(i).GetChild(2).GetComponent<Text>().text = getMemberList.data[i].client_id;
                    memberReqScrollContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text = getMemberList.data[i].user_id;
                    //memberReqScrollContent.transform.GetChild(i).GetChild(4).GetChild(1).GetComponent<Text>().text = getMemberList.data[i].chips;
                    string dateTime = ConvertDateTime(getMemberList.data[i].createdAt);
                    memberReqScrollContent.transform.GetChild(i).GetChild(4).GetComponent<Text>().text = dateTime;

                    memberReqScrollContent.transform.GetChild(i).gameObject.SetActive(true);
                    userImageForMemberReqList.Add(getMemberList.data[i].user_image);
                }
                UpdatePlayerImageForMemberReq();
                memberReqScrollPanel.SetActive(true);
                Members.instance.requestList.SetActive(true);
            }
        }
    }
    void GenerateMembersReqItem()
    {
        scrollItemObj = Instantiate(memberReqPanel);
        scrollItemObj.transform.SetParent(memberReqScrollContent.transform, false);
        memReqList.Add(scrollItemObj);
    }

    #region Update player image in member Request list

    private int j;
    private int totalImageCountForMemberReqList;
    private int countForMemberReqList;
    private int previousCountForMemberReqList;

    void UpdatePlayerImageForMemberReq()
    {
        if (userImageForMemberReqList.Count == previousCountForMemberReqList)
        {
            return;
        }

        playerImageInSequenceForMemberReqList.Clear();
        j = 0;
        totalImageCountForMemberReqList = 0;
        previousCountForMemberReqList = 0;

        playerImageInSequenceForMemberReqList = new List<PlayerImageInSequence>();

        for (int i = 0; i < userImageForMemberReqList.Count; i++)
        {
            if (!string.IsNullOrEmpty(userImageForMemberReqList[i]))
            {
                print("UpdatePlayerImageForMemberReq......");
                playerImageInSequenceForMemberReqList.Add(new PlayerImageInSequence());
                loadingPanel.SetActive(true);
                playerImageInSequenceForMemberReqList[j].imgUrl = userImageForMemberReqList[i];
                playerImageInSequenceForMemberReqList[j].ImageProcess(userImageForMemberReqList[i]);

                j = j + 1;

            }

            previousCountForMemberReqList++;
        }
    }

    public void ApplyImageInReqMemberList()
    {
        print(j);
        print(totalImageCountForMemberReqList);
        if (j == totalImageCountForMemberReqList)
        {
            countForMemberReqList = 0;
            loadingPanel.SetActive(false);

            for (int i = 0; i < userImageForMemberReqList.Count; i++)
            {
                memberReqScrollContent.transform.GetChild(i).GetChild(7).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
            }

            for (int i = 0; i < userImageForMemberReqList.Count; i++)
            {
                if (!string.IsNullOrEmpty(userImageForMemberReqList[i]))
                {
                    memberReqScrollContent.transform.GetChild(i).GetChild(7).GetComponent<Image>().sprite = playerImageInSequenceForMemberReqList[countForMemberReqList].imgPic;
                    countForMemberReqList++;
                }
            }

        }
    }

    #endregion

    #endregion

    #region Click on Member Management
    public void ClickOnMemberManagement(Transform panel)
    {
        currentSelectedUserName = panel.GetChild(1).GetComponent<Text>().text.Trim();
        string role = panel.GetChild(12).GetComponent<Text>().text;
        clubHomePage.transform.GetChild(1).gameObject.SetActive(false);
        if (role != "Agent")
        {
            memberProfile.SetActive(true);
            memberProfile.transform.GetComponent<MemberProfile>().ClickOnMemberProfile(panel);
        }
        else
        {
            memberProfileForAgent.SetActive(true);
            memberProfileForAgent.transform.GetComponent<MemberProfile>().ClickOnMemberProfile(panel);
        }

    }

    #region Player Stats

    [SerializeField] Admin.ClubId clubId1;
    private string currentSelectedUserName;
    public void ClickPlayerStats()
    {
        clubId1.club_id = _clubID;
        clubId1.username = currentSelectedUserName;

        string body = JsonUtility.ToJson(clubId1);
        print(body);

        loadingPanel.SetActive(true);
        //Communication.instance.PostData(Admin.instance.statsUrl, body, Admin.instance.PlayerStatsProcess);

    }

    #endregion

    #endregion

    #endregion

    #region Update Member Request List
    public void ClickOnRejectAll(GameObject content)
    {
        updateAllMember.status = 2;
        updateAllMember.club_id = _clubID;
        updateAllMember.joining_request = 2;


        for (int i = 0; i < content.transform.childCount; i++)
        {
            updateAllMember.joining.Add(new Joining());
            updateAllMember.joining[i].user_id = content.transform.GetChild(i).GetChild(3).GetComponent<Text>().text;

        }
        string body = JsonUtility.ToJson(updateAllMember);
        print(body);
        loadingPanel.SetActive(true);
        //Communication.instance.PostData(allMemberReqListUrl, body, UpdateAllMemberReqProcess);
    }

    public void ClickOnAccepttAll(GameObject content)
    {
        updateAllMember.status = 1;
        updateAllMember.club_id = _clubID;
        updateAllMember.joining_request = 1;

        for (int i = 0; i < content.transform.childCount; i++)
        {
            updateAllMember.joining.Add(new Joining());
            updateAllMember.joining[i].user_id = content.transform.GetChild(i).GetChild(3).GetComponent<Text>().text;

        }
        string body = JsonUtility.ToJson(updateAllMember);
        print(body);
        loadingPanel.SetActive(true);
        //Communication.instance.PostData(allMemberReqListUrl, body, UpdateAllMemberReqProcess);
    }

    void UpdateAllMemberReqProcess(string response)
    {
        loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("error");
        }
        else
        {
            print("" + response);
            for (int i = 0; i < memberReqScrollContent.transform.childCount; i++)
            {
                memberReqScrollContent.transform.GetChild(i).gameObject.SetActive(false);

            }
            updateAllMember.joining.Clear();

            //.............MailBox...............//
            MailBoxScripts._instance.MailBoxCallCount();
            //.......................................//
        }
    }

    public void ClickOnRejectButton(Text userId)
    {
        updateMember.club_id = _clubID;
        updateMember.joining_request = "-1";
        updateMember.user_id = userId.text;

        currentSelectedInMemberReqList = userId.gameObject.transform.parent.gameObject;

        string body = JsonUtility.ToJson(updateMember);
        print(body);
        UpdateMemberFunction(body);
    }

    public void ClickOnAcceptButton(Text userId)
    {
        updateMember.club_id = _clubID;
        updateMember.joining_request = "1";
        updateMember.user_id = userId.text;
        currentSelectedInMemberReqList = userId.gameObject.transform.parent.gameObject;
        string body = JsonUtility.ToJson(updateMember);
        print(body);
        UpdateMemberFunction(body);
    }

    void UpdateMemberFunction(string body)
    {
        loadingPanel.SetActive(true);
        //Communication.instance.PostData(updateMemberUrl, body, UpdateMemberProcess);
    }

    void UpdateMemberProcess(string response)
    {
        loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("error");
        }
        else
        {
            print("" + response);
            currentSelectedInMemberReqList.SetActive(false);
            print("request updated.......");

            currentSelectedInMemberReqList.transform.SetParent(transform);
            currentSelectedInMemberReqList.transform.SetParent(memberReqScrollContent.transform);

            //.............MailBox...............//
            MailBoxScripts._instance.MailBoxCallCount();
            //.......................................//
        }
    }

    #endregion

    public void ClickOnMembersButton()
    {
        menuFadePanel.SetActive(false);
    }
    //........................................//

    //............Click on Chashier Section....................//

    #region Click On Cashier button in Menu Section

    public void ClickOnCashierButton()
    {
        clubHomePage.transform.GetChild(7).gameObject.SetActive(false);
        menuFadePanel.SetActive(false);
        if (currentRoleInSelectedClub == 3)
        {
            ClickOnTradeHistory();
            Cashier.instance.availableChipBalanceForPlayer.text = individualClipBalance.text;
        }
        else //if (currentRoleInSelectedClub == 1 || currentRoleInSelectedClub == 2 || currentRoleInSelectedClub == 4)
        {
            if (currentRoleInSelectedClub == 4)
            {
                Cashier.instance.agentCreditPanel.SetActive(true);
                Cashier.instance.clubChipsPanel.SetActive(false);
                Cashier.instance.cashierCanvas.transform.GetChild(10).gameObject.SetActive(false);
                Cashier.instance.cashierCanvas.transform.GetChild(11).gameObject.SetActive(false);
                Cashier.instance.cashierCanvas.transform.GetChild(3).gameObject.SetActive(true);
            }
            else
            {
                Cashier.instance.agentCreditPanel.SetActive(false);
                Cashier.instance.clubChipsPanel.SetActive(true);


            }
            ClickMyTrade();
            if (currentRoleInSelectedClub == 5 || currentRoleInSelectedClub == 6)
            {
                Cashier.instance.tradeBottomPanel.transform.GetChild(0).GetComponent<Button>().interactable = false;
                Cashier.instance.tradeBottomPanel.transform.GetChild(1).GetComponent<Button>().interactable = false;
            }
            else
            {
                Cashier.instance.tradeBottomPanel.transform.GetChild(0).GetComponent<Button>().interactable = true;
                Cashier.instance.tradeBottomPanel.transform.GetChild(1).GetComponent<Button>().interactable = true;
            }

        }

    }

    #region Click On Trade in Cashier
    public void ClickMyTrade()
    {
        memberList.club_id = _clubID;
        memberList.club_role = AssignClubRoleInNumberFormat(currentSelectedClubObject.transform.GetChild(0).GetChild(3).GetComponent<Text>().text);
        string body = JsonUtility.ToJson(memberList);
        loadingPanel.SetActive(true);
        //Communication.instance.PostData(memberListUrl, body, ClickTradeProcess);

        Cashier.instance.Trade();
    }

    int AssignClubRoleInNumberFormat(string role)
    {
        if (role == "Owner" || role == "Partner")
        {
            return 1;
        }
        else if (role == "Manager" || role == "Accountant")
        {
            return 2;
        }
        else if (role == "Player")
        {
            return 3;
        }
        else if (role == "Agent")
        {
            return 4;
        }
        else
        {
            return 0;
        }
    }

    void ClickTradeProcess(string response)
    {
        loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("error");
        }
        else
        {
            print("" + response);
            getMemberList = JsonUtility.FromJson<GetMemberList>(response);

            if (!getMemberList.error)
            {
                if (getMemberList.data.Length != tradeaMemberConut)
                {
                    for (int i = tradeaMemberConut; i < getMemberList.data.Length; i++)
                    {
                        tradeaMemberConut++;
                        GenerateTradeMembersItem();
                    }
                }
                playerChipCount = 0;
                userImageForTradeList.Clear();

                prevGroupByListInTrade.Clear();
                ownerListInTrade.Clear();
                managerListInTrade.Clear();
                playerListInTrade.Clear();
                agentListInTrade.Clear();
                partnerListInTrade.Clear();
                accountantListInTrade.Clear();

                SearchingScript.instance.memberListNameInTrade.Clear();
                for (int i = 0; i < getMemberList.data.Length; i++)
                {
                    //tradeScrollPanel.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = getMemberList.data[i].first_name + " " + getMemberList.data[i].last_name;
                    tradeScrollPanel.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = getMemberList.data[i].username + "      ";
                    tradeScrollPanel.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = getMemberList.data[i].client_id;
                    string _role = getMemberList.data[i].club_member_role;
                    if (!string.IsNullOrEmpty(_role))
                    {
                        if (_role == "Accountant")
                        {
                            tradeScrollPanel.transform.GetChild(i).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = _role.Substring(0, 2);

                        }
                        else if (_role == "Partner")
                        {
                            tradeScrollPanel.transform.GetChild(i).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = _role.Substring(0, 2);
                        }
                        else
                        {
                            tradeScrollPanel.transform.GetChild(i).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = _role.Substring(0, 1);
                        }
                    }
                    tradeScrollPanel.transform.GetChild(i).GetChild(2).GetChild(0).GetComponent<Text>().text = getMemberList.data[i].club_member_role;
                    tradeScrollPanel.transform.GetChild(i).GetChild(3).GetComponent<Text>().text = getMemberList.data[i].user_id;
                    tradeScrollPanel.transform.GetChild(i).GetChild(6).GetChild(1).GetComponent<Text>().text = getMemberList.data[i].chips.ToString();
                    tradeScrollPanel.transform.GetChild(i).GetChild(7).GetComponent<Text>().text = getMemberList.data[i].agent_id;
                    tradeScrollPanel.transform.GetChild(i).GetChild(8).GetComponent<Text>().text = getMemberList.data[i].user_image;
                    playerChipCount = playerChipCount + Mathf.RoundToInt(getMemberList.data[i].chips);
                    userImageForTradeList.Add(getMemberList.data[i].user_image);
                    SearchingScript.instance.memberListNameInTrade.Add(tradeScrollPanel.transform.GetChild(i));

                    if (_role == "Owner")
                    {
                        ownerListInTrade.Add(tradeScrollPanel.transform.GetChild(i).gameObject);
                    }
                    else if (_role == "Manager")
                    {
                        managerListInTrade.Add(tradeScrollPanel.transform.GetChild(i).gameObject);
                    }
                    else if (_role == "Agent")
                    {
                        agentListInTrade.Add(tradeScrollPanel.transform.GetChild(i).gameObject);
                    }
                    else if (_role == "Player")
                    {
                        playerListInTrade.Add(tradeScrollPanel.transform.GetChild(i).gameObject);
                    }
                    else if (_role == "Accountant")
                    {
                        accountantListInTrade.Add(tradeScrollPanel.transform.GetChild(i).gameObject);
                    }
                    else if (_role == "Partner")
                    {
                        partnerListInTrade.Add(tradeScrollPanel.transform.GetChild(i).gameObject);
                    }
                    prevGroupByListInTrade.Add(tradeScrollPanel.transform.GetChild(i).gameObject);
                }
                

                Cashier.instance.playerChip.text = playerChipCount.ToString();
                Cashier.instance.trade.SetActive(true);
                UpdatePlayerImageForTrade();
            }
        }

    }

    #region Update player image in Trade list

    private int l;
    private int totalImageCountForTradeList;
    private int countForTradeList;
    private int previousCountForTradeList;

    public List<string> userImageForTradeList;
    [SerializeField]
    public List<PlayerImageInSequence> playerImageInSequenceForTradeList;

    public void ResetPlayerImageInTradeList()
    {
        l = 0;
        totalImageCountForTradeList = 0;
        countForTradeList = 0;
        previousCountForTradeList = 0;
        userImageForTradeList.Clear();
        playerImageInSequenceForTradeList.Clear();
    }

    void UpdatePlayerImageForTrade()
    {
        if (userImageForTradeList.Count == previousCountForTradeList)
        {
            return;
        }

        playerImageInSequenceForTradeList.Clear();
        l = 0;
        totalImageCountForTradeList = 0;
        previousCountForTradeList = 0;

        playerImageInSequenceForTradeList = new List<PlayerImageInSequence>();

        for (int i = 0; i < userImageForTradeList.Count; i++)
        {
            if (!string.IsNullOrEmpty(userImageForTradeList[i]))
            {
                print("UpdatePlayerImageFor Trade ......");
                playerImageInSequenceForTradeList.Add(new PlayerImageInSequence());
                loadingPanel.SetActive(true);
                playerImageInSequenceForTradeList[l].imgUrl = userImageForTradeList[i];
                playerImageInSequenceForTradeList[l].ImageProcess(userImageForTradeList[i]);

                l = l + 1;

            }

            previousCountForTradeList++;
        }
    }

    public void ApplyImageInTradeList()
    {
        print(l);
        print(totalImageCountForTradeList);
        if (l == totalImageCountForTradeList)
        {
            countForTradeList = 0;
            loadingPanel.SetActive(false);

            for (int i = 0; i < userImageForTradeList.Count; i++)
            {
                tradeScrollPanel.transform.GetChild(i).GetChild(4).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
            }

            for (int i = 0; i < userImageForTradeList.Count; i++)
            {
                if (!string.IsNullOrEmpty(userImageForTradeList[i]))
                {
                    tradeScrollPanel.transform.GetChild(i).GetChild(4).GetComponent<Image>().sprite = playerImageInSequenceForTradeList[countForTradeList].imgPic;
                    countForTradeList++;
                }
            }

        }
    }

    #endregion

    void GenerateTradeMembersItem()
    {
        scrollItemObj = Instantiate(tradepanel);
        scrollItemObj.transform.SetParent(tradeScrollPanel.transform, false);
        tradeMemList.Add(scrollItemObj);
    }

    public List<GameObject> afterGroupByListInTrade;
    public List<GameObject> prevGroupByListInTrade;

    public void ClickOnGroupByInTrade()
    {
        if (groupByRoleObjInTrade.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            afterGroupByListInTrade.Clear();
            afterGroupByListInTrade = new List<GameObject>();
            afterGroupByListInTrade = afterGroupByListInTrade.Concat(ownerListInTrade).ToList();
            afterGroupByListInTrade = afterGroupByListInTrade.Concat(managerListInTrade).ToList();
            afterGroupByListInTrade = afterGroupByListInTrade.Concat(partnerListInTrade).ToList();
            afterGroupByListInTrade = afterGroupByListInTrade.Concat(accountantListInTrade).ToList();
            afterGroupByListInTrade = afterGroupByListInTrade.Concat(agentListInTrade).ToList();
            afterGroupByListInTrade = afterGroupByListInTrade.Concat(playerListInTrade).ToList();

            for (int i = 0; i < afterGroupByListInTrade.Count; i++)
            {
                afterGroupByListInTrade[i].transform.SetParent(null);
                afterGroupByListInTrade[i].transform.SetParent(tradeScrollPanel.transform);
            }
        }
        else
        {
            for (int i = 0; i < prevGroupByListInTrade.Count; i++)
            {
                prevGroupByListInTrade[i].transform.SetParent(null);
                prevGroupByListInTrade[i].transform.SetParent(tradeScrollPanel.transform);
            }
        }
    }

    
    public void ClickOnSelectAllInTrade()
    {
        if (selectAllObjInTrade.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            for (int i = 0; i < tradeScrollPanel.transform.childCount; i++)
            {
                tradeScrollPanel.transform.GetChild(i).GetChild(6).GetChild(2).GetChild(1).gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < tradeScrollPanel.transform.childCount; i++)
            {
                tradeScrollPanel.transform.GetChild(i).GetChild(6).GetChild(2).GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    #endregion

    #region Click on Trade History Details

    internal int currentRoleInSelectedClub;
    internal int tradeaHistoryConut;



    [Serializable]
    class HistoryDetailsReq
    {
        public string club_id;
        public int club_role;
        public string agent_id;
    }

    [SerializeField]
    HistoryDetailsReq historyDetailsReq;

    [SerializeField] HistoryDetailsResponse historyDetailsResponse;

    public void ClickOnTradeHistory()
    {
        if (!tradeHistoryScrollPanel.activeInHierarchy)
        {
            historyDetailsReq.club_id = _clubID;

            if (currentRoleInSelectedClub == 5 || currentRoleInSelectedClub == 6)
            {
                historyDetailsReq.club_role = 2;
            }
            else
            {
                historyDetailsReq.club_role = currentRoleInSelectedClub;
            }
            historyDetailsReq.agent_id = currentSelectedClubObject.transform.GetChild(0).GetChild(5).GetComponent<Text>().text;
            string body = JsonUtility.ToJson(historyDetailsReq);
            print(body);

            loadingPanel.SetActive(true);
            //Communication.instance.PostData(tradeHistoryUrl, body, ClickOnTradeHistoryProcess);
        }
        Cashier.instance.TradeHistory();
    }

    void ClickOnTradeHistoryProcess(string response)
    {
        loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("error");
            tradeHistoryScrollPanel.SetActive(false);
        }
        else
        {
            print(response);

            historyDetailsResponse = JsonUtility.FromJson<HistoryDetailsResponse>(response);

            if (!historyDetailsResponse.error)
            {
                print("successful....respose.....");
                if (historyDetailsResponse.historyDetails != null)
                {
                    if (historyDetailsResponse.historyDetails.Length != tradeaHistoryConut)
                    {
                        for (int i = tradeaHistoryConut; i < historyDetailsResponse.historyDetails.Length; i++)
                        {
                            tradeaHistoryConut++;
                            GenerateTradeHistoryItem();
                        }
                    }
                    userImageForTradeHistory.Clear();
                    SearchingScript.instance.memberListNameInTradeHistory.Clear();
                    for (int i = 0; i < historyDetailsResponse.historyDetails.Length; i++)
                    {
                        string username = historyDetailsResponse.historyDetails[i].username;
                        if (username.Contains("@"))
                        {
                            username = username.Substring(0, username.LastIndexOf("@"));
                        }

                        tradeHistoryScrollPanel.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = username + "      ";
                        tradeHistoryScrollPanel.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = historyDetailsResponse.historyDetails[i].client_id;
                        string senderRole = historyDetailsResponse.historyDetails[i].sender_club_role;
                        tradeHistoryScrollPanel.transform.GetChild(i).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = senderRole.Substring(0, 1);
                        tradeHistoryScrollPanel.transform.GetChild(i).GetChild(2).GetChild(0).GetComponent<Text>().text = senderRole.Substring(0, 1);

                        string receiverUsername = historyDetailsResponse.historyDetails[i].receiver_username;
                        receiverUsername = receiverUsername.Trim();
                        if (receiverUsername.Contains("@"))
                        {
                            receiverUsername = receiverUsername.Substring(0, receiverUsername.LastIndexOf("@"));
                        }
                        
                        tradeHistoryScrollPanel.transform.GetChild(i).GetChild(3).GetComponent<Text>().text = receiverUsername + "      ";
                        tradeHistoryScrollPanel.transform.GetChild(i).GetChild(4).GetComponent<Text>().text = historyDetailsResponse.historyDetails[i].receiver_client_id;

                        string receiverRole = historyDetailsResponse.historyDetails[i].receiver_club_role;

                        if (receiverRole == "Accountant")
                        {
                            tradeHistoryScrollPanel.transform.GetChild(i).GetChild(3).GetChild(0).GetChild(0).GetComponent<Text>().text = receiverRole.Substring(0, 2);
                        }
                        else if (receiverRole == "Partner")
                        {
                            tradeHistoryScrollPanel.transform.GetChild(i).GetChild(3).GetChild(0).GetChild(0).GetComponent<Text>().text = receiverRole.Substring(0, 2);
                        }
                        else
                        {
                            tradeHistoryScrollPanel.transform.GetChild(i).GetChild(3).GetChild(0).GetChild(0).GetComponent<Text>().text = receiverRole.Substring(0, 1);
                        }

                        tradeHistoryScrollPanel.transform.GetChild(i).GetChild(5).GetChild(0).GetComponent<Text>().text = receiverRole.Substring(0, 1);
                        tradeHistoryScrollPanel.transform.GetChild(i).GetChild(6).GetChild(1).GetComponent<Text>().text = historyDetailsResponse.historyDetails[i].chips;

                        string date = ConvertDateTime(historyDetailsResponse.historyDetails[i].createdAt);
                        tradeHistoryScrollPanel.transform.GetChild(i).GetChild(7).GetComponent<Text>().text = date;

                        if (historyDetailsResponse.historyDetails[i].txn_type == 1)
                        {
                            //....... claim chips arrow .....//
                            tradeHistoryScrollPanel.transform.GetChild(i).GetChild(8).localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
                        }
                        else if (historyDetailsResponse.historyDetails[i].txn_type == 2)
                        {
                            //....... send chips arrow .....//
                            tradeHistoryScrollPanel.transform.GetChild(i).GetChild(8).localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                        }
                        else
                        {
                            //....... send chips arrow .....//
                            tradeHistoryScrollPanel.transform.GetChild(i).GetChild(8).localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                        }

                        userImageForTradeHistory.Add(historyDetailsResponse.historyDetails[i].receiver_image);
                        userImageForTradeHistory.Add(historyDetailsResponse.historyDetails[i].user_image);

                        if (historyDetailsResponse.historyDetails[i].txn_type == 3 && receiverRole == "Agent")
                        {
                            tradeHistoryScrollPanel.transform.GetChild(i).GetChild(6).GetChild(0).gameObject.SetActive(false);
                            tradeHistoryScrollPanel.transform.GetChild(i).GetChild(6).GetChild(2).gameObject.SetActive(true);
                        }
                        else
                        {
                            tradeHistoryScrollPanel.transform.GetChild(i).GetChild(6).GetChild(0).gameObject.SetActive(true);
                            tradeHistoryScrollPanel.transform.GetChild(i).GetChild(6).GetChild(2).gameObject.SetActive(false);
                        }
                        SearchingScript.instance.memberListNameInTradeHistory.Add(tradeHistoryScrollPanel.transform.GetChild(i));
                    }

                    tradeHistoryScrollPanel.SetActive(true);
                    UpdatePlayerImageForTradeHistory();
                }
            }
            else
            {
                print("UN-successful....respose.....");
                tradeHistoryScrollPanel.SetActive(false);
            }
        }
    }

    #region Update player image in Trade History

    private int m;
    private int totalImageCountForTradeHistory;
    private int countForTradeHistory;

    private int previousCountForTradeHistory;

    public List<string> userImageForTradeHistory;
    [SerializeField]
    public List<PlayerImageInSequence> playerImageInSequenceForTradeHistory;

    public void ResetPlayerImageInTradeHistory()
    {
        m = 0;
        totalImageCountForTradeHistory = 0;
        countForTradeHistory = 0;
        previousCountForTradeHistory = 0;
        userImageForTradeHistory.Clear();
        playerImageInSequenceForTradeHistory.Clear();
    }

    void UpdatePlayerImageForTradeHistory()
    {
        if (userImageForTradeHistory.Count == previousCountForTradeHistory)
        {
            return;
        }

        for (int i = 0; i < userImageForTradeHistory.Count / 2; i++)
        {
            print(i);
            tradeHistoryScrollPanel.transform.GetChild(i).GetChild(9).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
            tradeHistoryScrollPanel.transform.GetChild(i).GetChild(10).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
        }

        playerImageInSequenceForTradeHistory.Clear();
        m = 0;
        totalImageCountForTradeHistory = 0;
        previousCountForTradeHistory = 0;

        playerImageInSequenceForTradeHistory = new List<PlayerImageInSequence>();

        for (int i = 0; i < userImageForTradeHistory.Count; i++)
        {
            if (!string.IsNullOrEmpty(userImageForTradeHistory[i]))
            {
                print("UpdatePlayerImageFor Trade ......");
                playerImageInSequenceForTradeHistory.Add(new PlayerImageInSequence());
                loadingPanel.SetActive(true);
                playerImageInSequenceForTradeHistory[m].imgUrl = userImageForTradeHistory[i];
                playerImageInSequenceForTradeHistory[m].ImageProcess(userImageForTradeHistory[i]);

                m = m + 1;

            }

            previousCountForTradeHistory++;
        }
    }

    public void ApplyImageInTradeHistory()
    {
        print(m);
        print(totalImageCountForTradeHistory);
        if (m == totalImageCountForTradeHistory)
        {
            countForTradeHistory = 0;

            loadingPanel.SetActive(false);

            for (int i = 0; i < userImageForTradeHistory.Count / 2; i++)
            {
                print(i);
                tradeHistoryScrollPanel.transform.GetChild(i).GetChild(9).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
                tradeHistoryScrollPanel.transform.GetChild(i).GetChild(10).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
            }

            for (int i = 0; i < userImageForTradeHistory.Count; i++)
            {
                if (!string.IsNullOrEmpty(userImageForTradeHistory[i]))
                {
                    if (countForTradeHistory < playerImageInSequenceForTradeHistory.Count)
                    {
                        if (i % 2 == 0)
                        {
                            tradeHistoryScrollPanel.transform.GetChild(i / 2).GetChild(9).GetComponent<Image>().sprite = playerImageInSequenceForTradeHistory[countForTradeHistory].imgPic;

                        }
                        else
                        {
                            tradeHistoryScrollPanel.transform.GetChild((int)(i / 2)).GetChild(10).GetComponent<Image>().sprite = playerImageInSequenceForTradeHistory[countForTradeHistory].imgPic;
                        }
                        countForTradeHistory = countForTradeHistory + 1;
                    }
                }
            }

        }
    }

    #endregion

    void GenerateTradeHistoryItem()
    {
        scrollItemObj = Instantiate(tradeHistoryPanel);
        scrollItemObj.transform.SetParent(tradeHistoryScrollPanel.transform, false);
        tradeHistoryList.Add(scrollItemObj);
    }


    #endregion

    #region Click On Chip Request in Cashier
    public void ClickOnChipReqButton()
    {
        if (!Cashier.instance.chipsRequested.activeInHierarchy)
        {
            memberReq.club_id = _clubID;
            memberReq.club_role = currentRoleInSelectedClub;
            memberReq.agent_id = currentSelectedAgentId;

            string body = JsonUtility.ToJson(memberReq);
            print(body);
            loadingPanel.SetActive(true);
            //Communication.instance.PostData(chipReqDetailsUrl, body, ClickOnChipReqButtonProcess);
        }
        Cashier.instance.ChipsRequested();

        if (currentRoleInSelectedClub == 5 || currentRoleInSelectedClub == 6)
        {
            Cashier.instance.chipsRequested.transform.GetChild(0).GetComponent<Button>().interactable = false;
            Cashier.instance.chipsRequested.transform.GetChild(1).GetComponent<Button>().interactable = false;
            
            for (int i = 0; i < memberChipReqScrollContent.transform.childCount; i++)
            {
                memberChipReqScrollContent.transform.GetChild(i).GetChild(19).GetComponent<Button>().interactable = false;
                memberChipReqScrollContent.transform.GetChild(i).GetChild(20).GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            Cashier.instance.chipsRequested.transform.GetChild(0).GetComponent<Button>().interactable = true;
            Cashier.instance.chipsRequested.transform.GetChild(1).GetComponent<Button>().interactable = true;

            for (int i = 0; i < memberChipReqScrollContent.transform.childCount; i++)
            {
                memberChipReqScrollContent.transform.GetChild(i).GetChild(19).GetComponent<Button>().interactable = true;
                memberChipReqScrollContent.transform.GetChild(i).GetChild(20).GetComponent<Button>().interactable = true;
            }
        }
    }
    void ClickOnChipReqButtonProcess(string response)
    {
        loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("error");
        }
        else
        {
            print("" + response);
            getChipReqDetailsList = JsonUtility.FromJson<GetChipReqDetailsList>(response);
            if (!getChipReqDetailsList.error)
            {
                print(getChipReqDetailsList.data.Length);
                print(getChipReqDetailsList);

                if (!getChipReqDetailsList.error)
                {
                    if (getChipReqDetailsList.data.Length != memberChipConut)
                    {
                        for (int i = memberChipConut; i < getChipReqDetailsList.data.Length; i++)
                        {
                            memberChipConut++;
                            GenerateMembersChipReqItem();
                        }
                    }

                    userImageForChipReq.Clear();
                    SearchingScript.instance.memberListNameInChipReqDetail.Clear();
                    for (int i = 0; i < getChipReqDetailsList.data.Length; i++)
                    {
                        memberChipReqScrollContent.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = getChipReqDetailsList.data[i].first_name + " " + getChipReqDetailsList.data[i].last_name;
                        memberChipReqScrollContent.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = getChipReqDetailsList.data[i].receiver_username;
                        memberChipReqScrollContent.transform.GetChild(i).GetChild(2).GetChild(1).GetComponent<Text>().text = getChipReqDetailsList.data[i].chips.ToString();
                        memberChipReqScrollContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text = getChipReqDetailsList.data[i].receiver_client_id;//....user id
                        memberChipReqScrollContent.transform.GetChild(i).GetChild(4).GetComponent<Text>().text = getChipReqDetailsList.data[i].receiver_user_id;
                        memberChipReqScrollContent.transform.GetChild(i).GetChild(5).GetComponent<Text>().text = getChipReqDetailsList.data[i].agent_id;
                        memberChipReqScrollContent.transform.GetChild(i).GetChild(8).GetComponent<Text>().text = getChipReqDetailsList.data[i].receiver_club_role;

                        string dateStr = ConvertDateTime(getChipReqDetailsList.data[i].createdAt);
                        memberChipReqScrollContent.transform.GetChild(i).GetChild(6).GetComponent<Text>().text = dateStr;
                        //print(dateStr);
                        userImageForChipReq.Add(getChipReqDetailsList.data[i].user_image);
                        memberChipReqScrollContent.transform.GetChild(i).gameObject.SetActive(true);
                        SearchingScript.instance.memberListNameInChipReqDetail.Add(memberChipReqScrollContent.transform.GetChild(i));
                    }

                    Cashier.instance.chipsRequested.SetActive(true);
                    //chipReqPanel.SetActive(true);
                    UpdatePlayerImageForChipReq();
                    
                }
            }
            //.............MailBox...............//
            MailBoxScripts._instance.MailBoxCallCount();
            //.......................................//
        }

    }

    #region Update player image in Chip Request

    private int n;
    private int totalImageCountForChipReq;
    private int countForChipReq;
    private int previousCountForChipReq;

    public List<string> userImageForChipReq;
    [SerializeField]
    public List<PlayerImageInSequence> playerImageInSequenceForChipReq;

    public void ResetPlayerImageInChipReq()
    {
        n = 0;
        totalImageCountForChipReq = 0;
        countForChipReq = 0;
        previousCountForChipReq = 0;
        userImageForChipReq.Clear();
        playerImageInSequenceForChipReq.Clear();
    }

    void UpdatePlayerImageForChipReq()
    {
        for (int i = 0; i < userImageForChipReq.Count; i++)
        {
            memberChipReqScrollContent.transform.GetChild(i).GetChild(7).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
        }

        if (userImageForChipReq.Count == previousCountForChipReq)
        {
            return;
        }

        playerImageInSequenceForChipReq.Clear();
        n = 0;
        totalImageCountForChipReq = 0;
        previousCountForChipReq = 0;

        playerImageInSequenceForChipReq = new List<PlayerImageInSequence>();

        for (int i = 0; i < userImageForChipReq.Count; i++)
        {
            if (!string.IsNullOrEmpty(userImageForChipReq[i]))
            {
                print("UpdatePlayerImageFor chip req ......");
                playerImageInSequenceForChipReq.Add(new PlayerImageInSequence());
                loadingPanel.SetActive(true);
                playerImageInSequenceForChipReq[n].imgUrl = userImageForChipReq[i];
                playerImageInSequenceForChipReq[n].ImageProcess(userImageForChipReq[i]);

                n = n + 1;

            }

            previousCountForChipReq++;
        }
    }

    public void ApplyImageInChipReq()
    {
        print(n);
        print(totalImageCountForChipReq);
        if (n == totalImageCountForChipReq)
        {
            countForChipReq = 0;
            loadingPanel.SetActive(false);

            for (int i = 0; i < userImageForChipReq.Count; i++)
            {
                memberChipReqScrollContent.transform.GetChild(i).GetChild(7).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
            }

            for (int i = 0; i < userImageForChipReq.Count; i++)
            {
                if (!string.IsNullOrEmpty(userImageForChipReq[i]))
                {
                    memberChipReqScrollContent.transform.GetChild(i).GetChild(7).GetComponent<Image>().sprite = playerImageInSequenceForChipReq[countForChipReq].imgPic;
                    countForChipReq++;
                }
            }

        }
    }

    #endregion

    void GenerateMembersChipReqItem()
    {
        scrollItemObj = Instantiate(chipReqPanel);
        scrollItemObj.transform.SetParent(memberChipReqScrollContent.transform, false);
        chipReqList.Add(scrollItemObj);
    }

    #endregion

    #endregion

    #region Update Chip Request List

    public void ClickOnRejectAllChipsRequest()
    {
        updateAllMemberChips.chips.Clear();
        updateAllMemberChips.club_id = _clubID;
        updateAllMemberChips.chips_request = 2;
        updateAllMemberChips.sender_club_role = currentSelectedClubRole;

        for (int i = 0; i < memberChipReqScrollContent.transform.childCount; i++)
        {
            if (memberChipReqScrollContent.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                updateAllMemberChips.chips.Add(new Chips());
                updateAllMemberChips.chips[i].chips = Convert.ToInt32(memberChipReqScrollContent.transform.GetChild(i).GetChild(2).GetChild(1).GetComponent<Text>().text);
                updateAllMemberChips.chips[i].user_id = memberChipReqScrollContent.transform.GetChild(i).GetChild(4).GetComponent<Text>().text;
                updateAllMemberChips.chips[i].agent_id = memberChipReqScrollContent.transform.GetChild(i).GetChild(5).GetComponent<Text>().text;
                updateAllMemberChips.chips[i].club_member_role = memberChipReqScrollContent.transform.GetChild(i).GetChild(8).GetComponent<Text>().text;
            }
        }

        string body = JsonUtility.ToJson(updateAllMemberChips);
        print(body);
        loadingPanel.SetActive(true);

        if (currentSelectedClubRole == "Owner" || currentSelectedClubRole == "Manager")
        {
            //Communication.instance.PostData(updateAllPlayerChipsUrl, body, UpdateAllRequestedChipsProcess);
        }
        else if (currentSelectedClubRole == "Agent")
        {
            //Communication.instance.PostData(agentConfirmRejectAllChipsUrl, body, UpdateAllRequestedChipsProcess);
        }

    }

    public void ClickOnAcceptAllChipsRequest()
    {
        updateAllMemberChips.chips.Clear();
        updateAllMemberChips.club_id = _clubID;
        updateAllMemberChips.chips_request = 1;
        updateAllMemberChips.sender_club_role = currentSelectedClubRole;

        for (int i = 0; i < memberChipReqScrollContent.transform.childCount; i++)
        {
            if (memberChipReqScrollContent.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                updateAllMemberChips.chips.Add(new Chips());
                updateAllMemberChips.chips[i].chips = Convert.ToInt32(memberChipReqScrollContent.transform.GetChild(i).GetChild(2).GetChild(1).GetComponent<Text>().text);
                updateAllMemberChips.chips[i].user_id = memberChipReqScrollContent.transform.GetChild(i).GetChild(4).GetComponent<Text>().text;
                updateAllMemberChips.chips[i].agent_id = memberChipReqScrollContent.transform.GetChild(i).GetChild(5).GetComponent<Text>().text;
                updateAllMemberChips.chips[i].club_member_role = memberChipReqScrollContent.transform.GetChild(i).GetChild(8).GetComponent<Text>().text;
            }
        }

        string body = JsonUtility.ToJson(updateAllMemberChips);
        print(body);
        loadingPanel.SetActive(true);
        if (currentSelectedClubRole == "Owner" || currentSelectedClubRole == "Manager")
        {
            //Communication.instance.PostData(updateAllPlayerChipsUrl, body, UpdateAllRequestedChipsProcess);
        }
        else if (currentSelectedClubRole == "Agent")
        {
            //Communication.instance.PostData(agentConfirmRejectAllChipsUrl, body, UpdateAllRequestedChipsProcess);
        }

    }

    void UpdateAllRequestedChipsProcess(string response)
    {
        loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("error");
        }
        else
        {
            claimSendResponse = JsonUtility.FromJson<Cashier.ClaimSendResponse>(response);
            if (!claimSendResponse.error)
            {
                for (int i = 0; i < memberChipReqScrollContent.transform.childCount; i++)
                {
                    memberChipReqScrollContent.transform.GetChild(i).gameObject.SetActive(false);
                }
                Cashier.instance.currentClubId.text = _clubID;
                ClickOnClubDetails(Cashier.instance.currentClubId);
                print("request updated.......");
                updateAllMemberChips.chips.Clear();
            }
            else
            {
                print("request not updated.......");
                Cashier.instance.toastMsg.text = claimSendResponse.errors.club.properties.message;
                Cashier.instance.toastMsgPanel.SetActive(true);
            }
        }
    }

    public void ClickOnRejectChipsButton(GameObject panel)
    {
        panel.SetActive(false);
        player.club_id = _clubID;
        player.chips_request = -1;
        player.chips = Convert.ToInt32(panel.transform.GetChild(2).GetChild(1).GetComponent<Text>().text);
        player.receiver_user_id = panel.transform.GetChild(4).GetComponent<Text>().text;
        player.club_member_role = panel.transform.GetChild(8).GetComponent<Text>().text;
        player.agent_id = panel.transform.GetChild(5).GetComponent<Text>().text;
        player.sender_club_role = currentSelectedClubRole;
        currentSelectedMemberInChipReq = panel;

        string body = JsonUtility.ToJson(player);
        print(body);

        UpdateMemberChipsFunction(body);
    }


    public void ClickOnAcceptChipsButton(GameObject panel)
    {
        player.club_id = _clubID;
        player.chips_request = 1;
        player.chips = Convert.ToInt32(panel.transform.GetChild(2).GetChild(1).GetComponent<Text>().text);
        player.receiver_user_id = panel.transform.GetChild(4).GetComponent<Text>().text;
        player.club_member_role = panel.transform.GetChild(8).GetComponent<Text>().text;
        player.agent_id = panel.transform.GetChild(5).GetComponent<Text>().text;
        player.sender_club_role = currentSelectedClubRole;
        currentSelectedMemberInChipReq = panel;

        string body = JsonUtility.ToJson(player);

        print(body);
        loadingPanel.SetActive(true);
        UpdateMemberChipsFunction(body);
    }
    void UpdateMemberChipsFunction(string body)
    {
       // Communication.instance.PostData(updatePlayerChipsUrl, body, UpdateMemberChipsProcess);
    }

    void UpdateMemberChipsProcess(string response)
    {
        loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("error");
        }
        else
        {
            claimSendResponse = JsonUtility.FromJson<Cashier.ClaimSendResponse>(response);
            if (!claimSendResponse.error)
            {
                print("" + response);
                currentSelectedMemberInChipReq.SetActive(false);
                print("request updated.......");
                currentSelectedMemberInChipReq.transform.SetParent(transform);
                currentSelectedMemberInChipReq.transform.SetParent(memberChipReqScrollContent.transform);
                Cashier.instance.currentClubId.text = _clubID;
                ClickOnClubDetails(Cashier.instance.currentClubId);
            }
            else
            {
                Cashier.instance.toastMsg.text = claimSendResponse.errors.club.properties.message;
                Cashier.instance.toastMsgPanel.SetActive(true);
            }

        }
    }

    #endregion

    //........................................................//

    #region Send Chip request

    private string receiverClubRole;
    private string clubSenderRole;



    [Serializable]
    public class SendChipRequest
    {
        public string club_id;
        public string user_id;
        public string chips;
        public string receiver_club_role;
        public string sender_club_role;
        public string agent_id;
    }

    [SerializeField] SendChipRequest sendChipRequest;

    [SerializeField] ChangePasswordResponse changePasswordResponse;

    [SerializeField]
    public Cashier.ClaimSendResponse claimSendResponse;

    public void SendChipRequestButton()
    {
        if (string.IsNullOrEmpty(amountInputField.text) || !amountInputField.GetComponent<ValidateInput>().isValidInput)
        {
            amountInputField.GetComponent<ValidateInput>().Validate(amountInputField.text);
            amountInputField.Select();
        }
        else
        {
            sendChipRequest.club_id = _clubID;
            sendChipRequest.user_id = _clubOwnerUserId;
            sendChipRequest.chips = amountInputField.textComponent.text;
            sendChipRequest.sender_club_role = clubSenderRole;
            sendChipRequest.receiver_club_role = receiverClubRole;
            sendChipRequest.agent_id = currentSelectedAgentId;

            string body = JsonUtility.ToJson(sendChipRequest);
            print(body);

            loadingPanel.SetActive(true);
            //Communication.instance.PostData(sendChipRequestUrl, body, ClickOnChipRequestProcess);
        }
    }

    void ClickOnChipRequestProcess(string response)
    {
        loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("error");
        }
        else
        {
            print(response);
            claimSendResponse = JsonUtility.FromJson<Cashier.ClaimSendResponse>(response);
            if (!claimSendResponse.error)
            {
                print("sucess....");
                chipReqApplicationPanel.SetActive(false);
                amountInputField.text = string.Empty;
            }
            else
            {
                Cashier.instance.toastMsg.text = claimSendResponse.errors.club.properties.message;
                Cashier.instance.toastMsgPanel.SetActive(true);
            }

        }
    }


    #endregion

    public string ConvertDateTime(string jsonDate)
    {
        var dateStr = DateTime.Parse(jsonDate);
        string str = dateStr.ToString("MM/dd/yyyy hh:mm tt");
        return str;
    }

    public string ConvertDateTimeIn24Hours(string jsonDate)
    {
        var dateStr = DateTime.Parse(jsonDate);
        string str = dateStr.ToString("MM/dd/yyyy HH:mm");
        return str;
    }

    #region Update Welcome Message

    internal string currentSelectedClubRole;

    [Serializable]
    class WelcomeMsg
    {
        public string welcome_msg1;
        public string welcome_msg2;
        public string club_id;
        public string agent_id;
        public string club_member_role;
    }

    [SerializeField] WelcomeMsg welcomeMsg;
    [SerializeField] UpdateRoleResponse updateRoleResponse;
    public void EndChangeEditOnMsg1()
    {
        welcomeMsg.welcome_msg1 = clubMsg1.textComponent.text;
        welcomeMsg.welcome_msg2 = clubMsg2.textComponent.text;
        welcomeMsg.club_id = _clubID;
        welcomeMsg.club_member_role = currentSelectedClubRole;
        welcomeMsg.agent_id = currentSelectedAgentId;

        string body = JsonUtility.ToJson(welcomeMsg);

        print(body);
        loadingPanel.SetActive(true);
        //Communication.instance.PostData(updateWelcomeMsgUrl, body, UpdateWelcomeMsgProcess);
    }

    public void ClickOnAgentChipMsg()
    {
        welcomeMsg.welcome_msg1 = clubMsg1.textComponent.text;
        welcomeMsg.welcome_msg2 = agentChipMsgInputField.textComponent.text;
        welcomeMsg.club_id = _clubID;
        welcomeMsg.club_member_role = currentSelectedClubRole;
        welcomeMsg.agent_id = currentSelectedAgentId;

        string body = JsonUtility.ToJson(welcomeMsg);

        print(body);
        loadingPanel.SetActive(true);
        //Communication.instance.PostData(updateWelcomeMsgUrl, body, UpdateWelcomeMsgProcess);
    }

    void UpdateWelcomeMsgProcess(string response)
    {
        loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("error");
        }
        else
        {
            print(response);
            updateRoleResponse = JsonUtility.FromJson<UpdateRoleResponse>(response);

            if (!updateRoleResponse.error)
            {
                agentChipMsgPanel.SetActive(false);
                agentChipMsgInputField.text = string.Empty;
            }

        }
    }
    #endregion

    #region Disband Club 

    [SerializeField] ClubDetail disbandClub;

    [SerializeField] UpdateRoleResponse disbandClubResposne;

    public void ConfirmDisbandClub()
    {
        if (string.IsNullOrEmpty(disbandClubIdInputField.text) || !disbandClubIdInputField.GetComponent<ValidateInput>().isValidInput)
        {
            disbandClubIdInputField.GetComponent<ValidateInput>().Validate(disbandClubIdInputField.text);
            disbandClubIdInputField.Select();
        }
        else
        {
            disbandClub.club_id = _clubID;

            string body = JsonUtility.ToJson(disbandClub);
            print(body);
            loadingPanel.SetActive(true);
            //Communication.instance.PostData(disbandClubUrl, body, DisbandClubProcess);
        }
    }

    void DisbandClubProcess(string response)
    {
        loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("error");
        }
        else
        {
            print(response);

            disbandClubResposne = JsonUtility.FromJson<UpdateRoleResponse>(response);
            if (!disbandClubResposne.error)
            {
                print("Disband club success....");
                disbandClubUI.SetActive(false);
                quitClubFadePanel.SetActive(false);

                clubHomePage.SetActive(false);
                clubHomePage.transform.GetChild(0).gameObject.SetActive(true);
                clubHomePage.transform.GetChild(4).gameObject.SetActive(false);

                currentSelectedClubObject.SetActive(false);
                currentSelectedClubObject.transform.SetParent(transform);
                currentSelectedClubObject.transform.SetParent(myClubScrollContent.transform);

                ClickMyClubs();
            }
        }
    }

    public bool MatchClubID()
    {
        if (disbandClubIdInputField.text == _clubID)
        {
            return true;
        }
        return false;
    }

    #endregion

    #region Quit the club

    [Serializable]
    class RemovePlayer
    {
        public string user_id;
        public string club_id;
        public string club_member_role;
        public string agent_id;
        public int type;
    }

    [Serializable]
    public class UpdateRoleResponse
    {
        public bool error;
    }

    [SerializeField] RemovePlayer quitClubRequest;
    [SerializeField] UpdateRoleResponse quitClubResponse;
    public void ConfirmQuitClub()
    {
        quitClubRequest.club_id = _clubID;
        quitClubRequest.user_id = selectedClubUserId;
        quitClubRequest.club_member_role = currentSelectedClubRole;
        quitClubRequest.agent_id = currentSelectedAgentId;
        quitClubRequest.type = 1;
        string body = JsonUtility.ToJson(quitClubRequest);
        print(body);

        loadingPanel.SetActive(true);
        //Communication.instance.PostData(deleteClubMemberUrl, body, QuitClubProcess);
    }

    void QuitClubProcess(string response)
    {
        loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("Some error ...!");
        }
        else
        {
            print("response" + response);
            updateRoleResponse = JsonUtility.FromJson<UpdateRoleResponse>(response);

            if (!updateRoleResponse.error)
            {
                quitClubFadePanel.SetActive(false);
                quitClubPanel.SetActive(false);

                clubHomePage.SetActive(false);
                clubHomePage.transform.GetChild(0).gameObject.SetActive(true);
                clubHomePage.transform.GetChild(4).gameObject.SetActive(false);

                currentSelectedClubObject.SetActive(false);
                currentSelectedClubObject.transform.SetParent(transform);
                currentSelectedClubObject.transform.SetParent(myClubScrollContent.transform);

                ClickMyClubs();
            }
            else
            {
                print("Some error ...!");
            }
            //.............MailBox...............//
            MailBoxScripts._instance.MailBoxCallCount();
            //.......................................//
        }
    }

    #endregion

    #region Diamond Exchange

    [SerializeField] DiamondExchange diamondExchange;
    [SerializeField] DiamondExchangeResponse diamondExchangeResponse;
    public void DimondToChipsConversion()
    {
        if (!string.IsNullOrEmpty(diamondInputField.text))
        {
            convertedChips.text = (Convert.ToInt32(diamondInputField.text) * chipFactor).ToString();
        }

    }

    public void ConfirmDiamondExchange()
    {
        if (string.IsNullOrEmpty(diamondInputField.text) || !diamondInputField.GetComponent<ValidateInput>().isValidInput)
        {
            diamondInputField.GetComponent<ValidateInput>().Validate(diamondInputField.text);
            diamondInputField.Select();
        }
        else
        {
            diamondExchange.club_id = _clubID;
            diamondExchange.chips = Convert.ToInt32(convertedChips.text);
            diamondExchange.diamond = Convert.ToInt32(diamondInputField.text);

            string body = JsonUtility.ToJson(diamondExchange);
            print(body);

            loadingPanel.SetActive(true);
            //Communication.instance.PostData(diamondExchangeUrl, body, DiamondExchangeProcess);
        }
    }

    public bool CheckDiamondVal()
    {
        if (Convert.ToInt32(diamondInputField.text) <= Convert.ToInt32(diamondText[0].text))
        {
            print("true.....");
            return true;
        }
        else
        {
            print("false.....");
            return false;
        }

    }

    void DiamondExchangeProcess(string response)
    {
        loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("error");
        }
        else
        {
            print(response);

            diamondExchangeResponse = JsonUtility.FromJson<DiamondExchangeResponse>(response);
            if (!diamondExchangeResponse.error)
            {
                print("diamond Exchange success....");
                diamondInputField.text = "";
                chipPanelText.text = "0";

                for (int i = 0; i < diamondText.Length; i++)
                {
                    diamondText[i].text = diamondExchangeResponse.diamond.ToString();
                }
                ClickOnClubDetails(clubId);
                clubHomePage.transform.GetChild(8).gameObject.SetActive(false);

            }
        }
    }
    #endregion

    #region Active Status

    public void ClickBackButtonActiveStatus()
    {
        clubHomePage.transform.GetChild(0).gameObject.SetActive(true);
        activeStatusObj.SetActive(false);
    }
    
    public void ClickOnActiveMembers()
    {
        clubHomePage.transform.GetChild(0).gameObject.SetActive(false);
        activeStatusObj.SetActive(true);
        ClickPlayingMembers();
    }

    public void ClickPlayingMembers()
    {
        activeStatusObj.transform.GetChild(8).gameObject.SetActive(true);
        activeStatusObj.transform.GetChild(9).gameObject.SetActive(false);
        activeStatusObj.transform.GetChild(6).GetChild(0).GetChild(0).gameObject.SetActive(true);
        activeStatusObj.transform.GetChild(6).GetChild(1).GetChild(0).gameObject.SetActive(false);

        for (int i = 0; i < onlinePlayingListContent.transform.childCount; i++)
        {
            onlinePlayingListContent.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
            onlinePlayingListContent.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
            onlinePlayingListContent.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta = new Vector2(0f, -200f);
        }

        for (int i = 0; i < onlineObservingListContent.transform.childCount; i++)
        {
            onlineObservingListContent.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
            onlineObservingListContent.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
            onlineObservingListContent.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta = new Vector2(0f, -200f);
        }

    }
    public void ClickObservingMembers()
    {
        activeStatusObj.transform.GetChild(8).gameObject.SetActive(false);
        activeStatusObj.transform.GetChild(9).gameObject.SetActive(true);
        activeStatusObj.transform.GetChild(6).GetChild(0).GetChild(0).gameObject.SetActive(false);
        activeStatusObj.transform.GetChild(6).GetChild(1).GetChild(0).gameObject.SetActive(true);
    }

    int onlinePlayingListCount;
    int onlineObservingListCount;
    public List<GameObject> onlinePlayingListObj;
    public List<GameObject> onlineObservingListObj;
    void CurrentPlayerProcess(string response)
    {
        loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("error");
        }
        else
        {
            print("" + response);
            currentPlayerResponse = JsonUtility.FromJson<CurrentPlayerResponse>(response);
            if (!currentPlayerResponse.error)
            {
                members.text = currentPlayerResponse.count.ToString();
                observingPlayerCountText.text = "Observing Players(" + currentPlayerResponse.observer.ToString() + ")";
                playingPlayerCountText.text = "Playing Players(" + currentPlayerResponse.player.ToString() + ")";

                activeStatusObj.transform.GetChild(3).GetChild(1).GetComponent<Text>().text = members.text;
                activeStatusObj.transform.GetChild(4).GetChild(1).GetComponent<Text>().text = currentPlayerResponse.activeTables.ToString();
                activeStatusObj.transform.GetChild(5).GetChild(1).GetComponent<Text>().text = currentPlayerResponse.activeTour.ToString();

                if (currentPlayerResponse.onlinePlayerList.Length > 0)
                {
                    if (currentPlayerResponse.onlinePlayerList.Length != onlinePlayingListCount)
                    {
                        for (int i = onlinePlayingListCount; i < currentPlayerResponse.onlinePlayerList.Length; i++)
                        {
                            onlinePlayingListCount++;
                            GeneratePlayingObj();
                        }
                    }

                    userImageForPlaying.Clear();
                    userImageForPlayingCountry.Clear();
                    for (int i = 0; i < currentPlayerResponse.onlinePlayerList.Length; i++)
                    {
                        onlinePlayingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.table_type = currentPlayerResponse.onlinePlayerList[i].table_type;
                        onlinePlayingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.end_time = currentPlayerResponse.onlinePlayerList[i].end_time;
                        onlinePlayingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.game_type = currentPlayerResponse.onlinePlayerList[i].game_type;
                        onlinePlayingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.table_name = currentPlayerResponse.onlinePlayerList[i].table_name;
                        onlinePlayingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.small_blind = currentPlayerResponse.onlinePlayerList[i].small_blind;
                        onlinePlayingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.big_blind = currentPlayerResponse.onlinePlayerList[i].big_blind;
                        onlinePlayingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.min_buy_in = currentPlayerResponse.onlinePlayerList[i].min_buy_in;
                        onlinePlayingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.min_auto_start = currentPlayerResponse.onlinePlayerList[i].min_auto_start;
                        onlinePlayingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.blinds_up = currentPlayerResponse.onlinePlayerList[i].blinds_up;
                        onlinePlayingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.table_size = currentPlayerResponse.onlinePlayerList[i].table_size;
                        onlinePlayingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.action_time = currentPlayerResponse.onlinePlayerList[i].action_time;
                        onlinePlayingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.start_time = currentPlayerResponse.onlinePlayerList[i].start_time;
                        onlinePlayingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.rule_id = currentPlayerResponse.onlinePlayerList[i].rule_id;
                        onlinePlayingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.table_id = currentPlayerResponse.onlinePlayerList[i].table_id;
                        onlinePlayingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.mississippi_straddle = currentPlayerResponse.onlinePlayerList[i].mississippi_straddle;
                        onlinePlayingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.auto_start = currentPlayerResponse.onlinePlayerList[i].auto_start;
                        onlinePlayingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.buy_in_authorization = currentPlayerResponse.onlinePlayerList[i].buy_in_authorization;
                        onlinePlayingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.video_mode = currentPlayerResponse.onlinePlayerList[i].video_mode;
                    }

                    for (int i = 0; i < currentPlayerResponse.onlinePlayerList.Length; i++)
                    {
                        onlinePlayingListContent.transform.GetChild(i).GetChild(0).GetChild(1).GetComponent<Text>().text = currentPlayerResponse.onlinePlayerList[i].table_name + "      ";
                        onlinePlayingListContent.transform.GetChild(i).GetChild(0).GetChild(2).GetComponent<Text>().text = currentPlayerResponse.onlinePlayerList[i].username + "        ";
                        onlinePlayingListContent.transform.GetChild(i).GetChild(0).GetChild(3).GetComponent<Text>().text = currentPlayerResponse.onlinePlayerList[i].client_id;
                        onlinePlayingListContent.transform.GetChild(i).GetChild(0).GetChild(4).GetChild(0).GetComponent<Text>().text = currentPlayerResponse.onlinePlayerList[i].game_type;
                        onlinePlayingListContent.transform.GetChild(i).GetChild(0).GetChild(5).GetComponent<Text>().text = currentPlayerResponse.onlinePlayerList[i].small_blind.ToString("0.##");
                        onlinePlayingListContent.transform.GetChild(i).GetChild(0).GetChild(6).GetComponent<Text>().text = currentPlayerResponse.onlinePlayerList[i].big_blind.ToString("0.##");

                        onlinePlayingListContent.transform.GetChild(i).GetChild(1).GetChild(1).GetComponent<Text>().text = currentPlayerResponse.onlinePlayerList[i].table_name + "      ";
                        onlinePlayingListContent.transform.GetChild(i).GetChild(1).GetChild(2).GetComponent<Text>().text = currentPlayerResponse.onlinePlayerList[i].username + "        ";
                        onlinePlayingListContent.transform.GetChild(i).GetChild(1).GetChild(3).GetComponent<Text>().text = currentPlayerResponse.onlinePlayerList[i].client_id;
                        onlinePlayingListContent.transform.GetChild(i).GetChild(1).GetChild(4).GetChild(0).GetComponent<Text>().text = currentPlayerResponse.onlinePlayerList[i].game_type;
                        onlinePlayingListContent.transform.GetChild(i).GetChild(1).GetChild(5).GetComponent<Text>().text = currentPlayerResponse.onlinePlayerList[i].small_blind.ToString("0.##");
                        onlinePlayingListContent.transform.GetChild(i).GetChild(1).GetChild(6).GetComponent<Text>().text = currentPlayerResponse.onlinePlayerList[i].big_blind.ToString("0.##");

                        if (currentPlayerResponse.onlinePlayerList[i].video_mode)
                        {
                            onlinePlayingListContent.transform.GetChild(i).GetChild(0).GetChild(8).gameObject.SetActive(true);
                            onlinePlayingListContent.transform.GetChild(i).GetChild(0).GetChild(9).gameObject.SetActive(false);

                            onlinePlayingListContent.transform.GetChild(i).GetChild(1).GetChild(8).gameObject.SetActive(true);
                            onlinePlayingListContent.transform.GetChild(i).GetChild(1).GetChild(9).gameObject.SetActive(false);
                        }
                        else
                        {
                            onlinePlayingListContent.transform.GetChild(i).GetChild(0).GetChild(8).gameObject.SetActive(false);
                            onlinePlayingListContent.transform.GetChild(i).GetChild(0).GetChild(9).gameObject.SetActive(true);

                            onlinePlayingListContent.transform.GetChild(i).GetChild(1).GetChild(8).gameObject.SetActive(false);
                            onlinePlayingListContent.transform.GetChild(i).GetChild(1).GetChild(9).gameObject.SetActive(true);
                        }

                        string _userName = currentPlayerResponse.onlinePlayerList[i].username;
                        for (int j = 0; j < currentPlayerResponse.onlinePlayerList[i].current_players.Length; j++)
                        {
                            if(_userName == currentPlayerResponse.onlinePlayerList[i].current_players[j].playerName)
                            {
                                if (!string.IsNullOrEmpty(currentPlayerResponse.onlinePlayerList[i].current_players[j].joinedAt.ToString()))
                                {
                                    DateTime date = new DateTime(Convert.ToInt64(currentPlayerResponse.onlinePlayerList[i].current_players[j].joinedAt.ToString()));
                                    string _joiningTime = date.ToString("hh:mm");

                                    onlinePlayingListContent.transform.GetChild(i).GetChild(1).GetChild(7).GetChild(1).GetComponent<Text>().text = _joiningTime;
                                }
                                else
                                {
                                    onlinePlayingListContent.transform.GetChild(i).GetChild(1).GetChild(7).GetChild(1).GetComponent<Text>().text = string.Empty;
                                }
                            }
                        }

                        userImageForPlaying.Add(currentPlayerResponse.onlinePlayerList[i].user_image);
                        userImageForPlayingCountry.Add(currentPlayerResponse.onlinePlayerList[i].country_flag);
                        onlinePlayingListContent.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    UpdatePlayerImageForPlaying();                   
                    UpdatePlayerImageForPlayingCountry();

                }

                else
                {
                    ResetOnlineActivePlayers();
                    onlinePlayingListContent.transform.GetChild(0).gameObject.SetActive(false);
                }

                if (currentPlayerResponse.observerList.Length > 0)
                {
                    if (currentPlayerResponse.observerList.Length != onlineObservingListCount)
                    {
                        for (int i = onlineObservingListCount; i < currentPlayerResponse.observerList.Length; i++)
                        {
                            onlineObservingListCount++;
                            GenerateObservingObj();
                        }
                    }

                    userImageForObserving.Clear();
                    userImageForObservingCountry.Clear();
                    for (int i = 0; i < currentPlayerResponse.observerList.Length; i++)
                    {
                        onlineObservingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.table_type = currentPlayerResponse.observerList[i].table_type;
                        onlineObservingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.end_time = currentPlayerResponse.observerList[i].end_time;
                        onlineObservingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.game_type = currentPlayerResponse.observerList[i].game_type;
                        onlineObservingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.table_name = currentPlayerResponse.observerList[i].table_name;
                        onlineObservingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.small_blind = currentPlayerResponse.observerList[i].small_blind;
                        onlineObservingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.big_blind = currentPlayerResponse.observerList[i].big_blind;
                        onlineObservingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.min_buy_in = currentPlayerResponse.observerList[i].min_buy_in;
                        onlineObservingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.min_auto_start = currentPlayerResponse.observerList[i].min_auto_start;
                        onlineObservingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.blinds_up = currentPlayerResponse.observerList[i].blinds_up;
                        onlineObservingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.table_size = currentPlayerResponse.observerList[i].table_size;
                        onlineObservingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.action_time = currentPlayerResponse.observerList[i].action_time;
                        onlineObservingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.start_time = currentPlayerResponse.observerList[i].start_time;
                        onlineObservingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.rule_id = currentPlayerResponse.observerList[i].rule_id;
                        onlineObservingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.table_id = currentPlayerResponse.observerList[i].table_id;
                        onlineObservingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.mississippi_straddle = currentPlayerResponse.observerList[i].mississippi_straddle;
                        onlineObservingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.auto_start = currentPlayerResponse.observerList[i].auto_start;
                        onlineObservingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.buy_in_authorization = currentPlayerResponse.observerList[i].buy_in_authorization;
                        onlineObservingListContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.video_mode = currentPlayerResponse.observerList[i].video_mode;
                    }

                    for (int i = 0; i < currentPlayerResponse.observerList.Length; i++)
                    {
                        onlineObservingListContent.transform.GetChild(i).GetChild(0).GetChild(1).GetComponent<Text>().text = currentPlayerResponse.observerList[i].table_name + "      ";
                        onlineObservingListContent.transform.GetChild(i).GetChild(0).GetChild(2).GetComponent<Text>().text = currentPlayerResponse.observerList[i].username + "        ";
                        onlineObservingListContent.transform.GetChild(i).GetChild(0).GetChild(3).GetComponent<Text>().text = currentPlayerResponse.observerList[i].client_id;
                        onlineObservingListContent.transform.GetChild(i).GetChild(0).GetChild(4).GetChild(0).GetComponent<Text>().text = currentPlayerResponse.observerList[i].game_type;
                        onlineObservingListContent.transform.GetChild(i).GetChild(0).GetChild(5).GetComponent<Text>().text = currentPlayerResponse.observerList[i].small_blind.ToString("0.##");
                        onlineObservingListContent.transform.GetChild(i).GetChild(0).GetChild(6).GetComponent<Text>().text = currentPlayerResponse.observerList[i].big_blind.ToString("0.##");

                        onlineObservingListContent.transform.GetChild(i).GetChild(1).GetChild(1).GetComponent<Text>().text = currentPlayerResponse.observerList[i].table_name + "      ";
                        onlineObservingListContent.transform.GetChild(i).GetChild(1).GetChild(2).GetComponent<Text>().text = currentPlayerResponse.observerList[i].username + "        ";
                        onlineObservingListContent.transform.GetChild(i).GetChild(1).GetChild(3).GetComponent<Text>().text = currentPlayerResponse.observerList[i].client_id;
                        onlineObservingListContent.transform.GetChild(i).GetChild(1).GetChild(4).GetChild(0).GetComponent<Text>().text = currentPlayerResponse.observerList[i].game_type;
                        onlineObservingListContent.transform.GetChild(i).GetChild(1).GetChild(5).GetComponent<Text>().text = currentPlayerResponse.observerList[i].small_blind.ToString("0.##");
                        onlineObservingListContent.transform.GetChild(i).GetChild(1).GetChild(6).GetComponent<Text>().text = currentPlayerResponse.observerList[i].big_blind.ToString("0.##");

                        if (currentPlayerResponse.observerList[i].video_mode)
                        {
                            onlineObservingListContent.transform.GetChild(i).GetChild(0).GetChild(8).gameObject.SetActive(true);
                            onlineObservingListContent.transform.GetChild(i).GetChild(0).GetChild(9).gameObject.SetActive(false);

                            onlineObservingListContent.transform.GetChild(i).GetChild(1).GetChild(8).gameObject.SetActive(true);
                            onlineObservingListContent.transform.GetChild(i).GetChild(1).GetChild(9).gameObject.SetActive(false);
                        }
                        else
                        {
                            onlineObservingListContent.transform.GetChild(i).GetChild(0).GetChild(8).gameObject.SetActive(false);
                            onlineObservingListContent.transform.GetChild(i).GetChild(0).GetChild(9).gameObject.SetActive(true);

                            onlineObservingListContent.transform.GetChild(i).GetChild(1).GetChild(8).gameObject.SetActive(false);
                            onlineObservingListContent.transform.GetChild(i).GetChild(1).GetChild(9).gameObject.SetActive(true);
                        }

                        string _userName = currentPlayerResponse.observerList[i].username;
                        for (int j = 0; j < currentPlayerResponse.observerList[i].current_observers.Length; j++)
                        {
                            if (_userName == currentPlayerResponse.observerList[i].current_observers[j].playerName)
                            {
                                if (!string.IsNullOrEmpty(currentPlayerResponse.observerList[i].current_observers[j].joinedAt.ToString()))
                                {
                                    DateTime date = new DateTime(Convert.ToInt64(currentPlayerResponse.observerList[i].current_observers[j].joinedAt.ToString()));
                                    string _joiningTime = date.ToString("hh:mm");

                                    onlineObservingListContent.transform.GetChild(i).GetChild(1).GetChild(7).GetChild(1).GetComponent<Text>().text = _joiningTime;
                                }
                                else
                                {
                                    onlineObservingListContent.transform.GetChild(i).GetChild(1).GetChild(7).GetChild(1).GetComponent<Text>().text = string.Empty;
                                }
                            }
                        }
                        
                        userImageForObserving.Add(currentPlayerResponse.observerList[i].user_image);
                        userImageForObservingCountry.Add(currentPlayerResponse.observerList[i].country_flag);

                        onlineObservingListContent.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    UpdatePlayerImageForObserving();
                    UpdatePlayerImageForObservingCountry();

                }

                else
                {
                    ResetOnlineActivePlayers();
                    onlineObservingListContent.transform.GetChild(0).gameObject.SetActive(false);
                }
                CancelInvoke("CalculateTime1");
                CancelInvoke("CalculateTime");
                isPlayingTime = false;
                isPlayingTime1 = false;

                oldHour.Clear();
                oldMin.Clear();
                oldHour1.Clear();
                oldMin1.Clear();
            }
        }
    }

    #region Update player image in Playing

    private int l1;
    private int totalImageCountForPlaying;
    private int countForPlaying;
    private int previousCountForPlaying;

    public List<string> userImageForPlaying;
    [SerializeField]
    public List<PlayerImageInPlaying> playerImageInPlaying;

    public void ResetPlayerImageInPlaying()
    {
        l1 = 0;
        totalImageCountForPlaying = 0;
        countForPlaying = 0;
        previousCountForPlaying = 0;
        userImageForPlaying.Clear();
        playerImageInPlaying.Clear();
    }

    void UpdatePlayerImageForPlaying()
    {
        if (userImageForPlaying.Count == previousCountForPlaying)
        {
            return;
        }

        playerImageInPlaying.Clear();
        l1 = 0;
        totalImageCountForPlaying = 0;
        previousCountForPlaying = 0;

        playerImageInPlaying = new List<PlayerImageInPlaying>();

        for (int i = 0; i < userImageForPlaying.Count; i++)
        {
            if (!string.IsNullOrEmpty(userImageForPlaying[i]))
            {
                print("UpdatePlayerImageFor Playing ......");
                playerImageInPlaying.Add(new PlayerImageInPlaying());
                //loadingPanel.SetActive(true);
                playerImageInPlaying[l1].imgUrl = userImageForPlaying[i];
                playerImageInPlaying[l1].ImageProcess(userImageForPlaying[i]);

                l1 = l1 + 1;

            }

            previousCountForPlaying++;
        }
    }

    [Serializable]
    public class PlayerImageInPlaying
    {
        public string imgUrl;
        public Sprite imgPic;

        public void ImageProcess(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                Communication.instance.GetImage(url, ImageResponse);
            }
        }

        void ImageResponse(Sprite response)
        {
            if (response != null)
            {
                imgPic = response;

                instance.totalImageCountForPlaying++;
                instance.ApplyImageInPlaying();
            }
        }
    }

    public void ApplyImageInPlaying()
    {
        print(l1);
        print(totalImageCountForPlaying);
        if (l1 == totalImageCountForPlaying)
        {
            countForPlaying = 0;
            loadingPanel.SetActive(false);

            for (int i = 0; i < userImageForPlaying.Count; i++)
            {
                onlinePlayingListContent.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
                onlinePlayingListContent.transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
            }

            for (int i = 0; i < userImageForPlaying.Count; i++)
            {
                if (!string.IsNullOrEmpty(userImageForPlaying[i]))
                {
                    onlinePlayingListContent.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().sprite = playerImageInPlaying[countForPlaying].imgPic;
                    onlinePlayingListContent.transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<Image>().sprite = playerImageInPlaying[countForPlaying].imgPic;
                    countForPlaying++;
                }
            }

        }
    }

    #endregion

    #region Update Country image in Playing

    private int l3;
    private int totalImageCountForPlayingCountry;
    private int countForPlayingCountry;
    private int previousCountForPlayingCountry;

    public List<string> userImageForPlayingCountry;
    [SerializeField]
    public List<PlayerImageInPlayingCountry> playerImageInPlayingCountry;

    public void ResetPlayerImageInPlayingCountry()
    {
        l3 = 0;
        totalImageCountForPlayingCountry = 0;
        countForPlayingCountry = 0;
        previousCountForPlayingCountry = 0;
        userImageForPlayingCountry.Clear();
        playerImageInPlayingCountry.Clear();
    }

    void UpdatePlayerImageForPlayingCountry()
    {
        if (userImageForPlayingCountry.Count == previousCountForPlayingCountry)
        {
            return;
        }

        playerImageInPlayingCountry.Clear();
        l3 = 0;
        totalImageCountForPlayingCountry = 0;
        previousCountForPlayingCountry = 0;

        playerImageInPlayingCountry = new List<PlayerImageInPlayingCountry>();

        for (int i = 0; i < userImageForPlayingCountry.Count; i++)
        {
            if (!string.IsNullOrEmpty(userImageForPlayingCountry[i]))
            {
                print("UpdatePlayerImageFor Playing Country ......");
                playerImageInPlayingCountry.Add(new PlayerImageInPlayingCountry());
                //loadingPanel.SetActive(true);
                playerImageInPlayingCountry[l3].imgUrl = userImageForPlayingCountry[i];
                playerImageInPlayingCountry[l3].ImageProcess(userImageForPlayingCountry[i]);

                l3 = l3 + 1;

            }

            previousCountForPlayingCountry++;
        }
    }

    [Serializable]
    public class PlayerImageInPlayingCountry
    {
        public string imgUrl;
        public Sprite imgPic;

        public void ImageProcess(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                Communication.instance.GetImage(url, ImageResponse);
            }
        }

        void ImageResponse(Sprite response)
        {
            if (response != null)
            {
                imgPic = response;

                instance.totalImageCountForPlayingCountry++;
                instance.ApplyImageInPlayingCountry();
            }
        }
    }

    public void ApplyImageInPlayingCountry()
    {
        print(l3);
        print(totalImageCountForPlayingCountry);
        if (l3 == totalImageCountForPlayingCountry)
        {
            countForPlayingCountry = 0;
            loadingPanel.SetActive(false);

            for (int i = 0; i < userImageForPlayingCountry.Count; i++)
            {
                onlinePlayingListContent.transform.GetChild(i).GetChild(0).GetChild(2).GetChild(0).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
                onlinePlayingListContent.transform.GetChild(i).GetChild(1).GetChild(2).GetChild(0).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
            }

            for (int i = 0; i < userImageForPlayingCountry.Count; i++)
            {
                if (!string.IsNullOrEmpty(userImageForPlayingCountry[i]))
                {
                    onlinePlayingListContent.transform.GetChild(i).GetChild(0).GetChild(2).GetChild(0).GetComponent<Image>().sprite = playerImageInPlayingCountry[countForPlayingCountry].imgPic;
                    onlinePlayingListContent.transform.GetChild(i).GetChild(1).GetChild(2).GetChild(0).GetComponent<Image>().sprite = playerImageInPlayingCountry[countForPlayingCountry].imgPic;
                    countForPlayingCountry++;
                }
            }

        }
    }

    #endregion

    #region Update player image in Observing

    private int l2;
    private int totalImageCountForObserving;
    private int countForObserving;
    private int previousCountForObserving;

    public List<string> userImageForObserving;
    [SerializeField]
    public List<PlayerImageInObserving> playerImageInObserving;

    public void ResetPlayerImageInObserving()
    {
        l2 = 0;
        totalImageCountForObserving = 0;
        countForObserving = 0;
        previousCountForObserving = 0;
        userImageForObserving.Clear();
        playerImageInObserving.Clear();
    }

    void UpdatePlayerImageForObserving()
    {
        if (userImageForObserving.Count == previousCountForObserving)
        {
            return;
        }

        playerImageInObserving.Clear();
        l2 = 0;
        totalImageCountForObserving = 0;
        previousCountForObserving = 0;

        playerImageInObserving = new List<PlayerImageInObserving>();

        for (int i = 0; i < userImageForObserving.Count; i++)
        {
            if (!string.IsNullOrEmpty(userImageForObserving[i]))
            {
                print("UpdatePlayerImageFor Observing ......");
                playerImageInObserving.Add(new PlayerImageInObserving());
                //loadingPanel.SetActive(true);
                playerImageInObserving[l2].imgUrl = userImageForObserving[i];
                playerImageInObserving[l2].ImageProcess(userImageForObserving[i]);

                l2 = l2 + 1;

            }

            previousCountForObserving++;
        }
    }

    [Serializable]
    public class PlayerImageInObserving
    {
        public string imgUrl;
        public Sprite imgPic;

        public void ImageProcess(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                Communication.instance.GetImage(url, ImageResponse);
            }
        }

        void ImageResponse(Sprite response)
        {
            if (response != null)
            {
                imgPic = response;

                instance.totalImageCountForObserving++;
                instance.ApplyImageInObserving();
            }
        }
    }

    public void ApplyImageInObserving()
    {
        print(l2);
        print(totalImageCountForObserving);
        if (l2 == totalImageCountForObserving)
        {
            countForObserving = 0;
            loadingPanel.SetActive(false);

            for (int i = 0; i < userImageForObserving.Count; i++)
            {
                onlineObservingListContent.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
                onlineObservingListContent.transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
            }

            for (int i = 0; i < userImageForObserving.Count; i++)
            {
                if (!string.IsNullOrEmpty(userImageForObserving[i]))
                {
                    onlineObservingListContent.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().sprite = playerImageInObserving[countForObserving].imgPic;
                    onlineObservingListContent.transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<Image>().sprite = playerImageInObserving[countForObserving].imgPic;
                    countForObserving++;
                }
            }

        }
    }

    #endregion

    #region Update Country image in Observing

    private int l4;
    private int totalImageCountForObservingCountry;
    private int countForObservingCountry;
    private int previousCountForObservingCountry;

    public List<string> userImageForObservingCountry;
    [SerializeField]
    public List<PlayerImageInObservingCountry> playerImageInObservingCountry;

    public void ResetPlayerImageInObservingCountry()
    {
        l4 = 0;
        totalImageCountForObservingCountry = 0;
        countForObservingCountry = 0;
        previousCountForObservingCountry = 0;
        userImageForObservingCountry.Clear();
        playerImageInObservingCountry.Clear();
    }

    void UpdatePlayerImageForObservingCountry()
    {
        if (userImageForObservingCountry.Count == previousCountForObservingCountry)
        {
            return;
        }

        playerImageInObservingCountry.Clear();
        l4 = 0;
        totalImageCountForObservingCountry = 0;
        previousCountForObservingCountry = 0;

        playerImageInObservingCountry = new List<PlayerImageInObservingCountry>();

        for (int i = 0; i < userImageForObservingCountry.Count; i++)
        {
            if (!string.IsNullOrEmpty(userImageForObservingCountry[i]))
            {
                print("UpdatePlayerImageFor Observing Country......");
                playerImageInObservingCountry.Add(new PlayerImageInObservingCountry());
                //loadingPanel.SetActive(true);
                playerImageInObservingCountry[l4].imgUrl = userImageForObservingCountry[i];
                playerImageInObservingCountry[l4].ImageProcess(userImageForObservingCountry[i]);

                l4 = l4 + 1;

            }

            previousCountForObservingCountry++;
        }
    }

    [Serializable]
    public class PlayerImageInObservingCountry
    {
        public string imgUrl;
        public Sprite imgPic;

        public void ImageProcess(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                Communication.instance.GetImage(url, ImageResponse);
            }
        }

        void ImageResponse(Sprite response)
        {
            if (response != null)
            {
                imgPic = response;

                instance.totalImageCountForObservingCountry++;
                instance.ApplyImageInObservingCountry();
            }
        }
    }

    public void ApplyImageInObservingCountry()
    {
        print(l4);
        print(totalImageCountForObservingCountry);
        if (l4 == totalImageCountForObservingCountry)
        {
            countForObservingCountry = 0;
            loadingPanel.SetActive(false);

            for (int i = 0; i < userImageForObservingCountry.Count; i++)
            {
                onlineObservingListContent.transform.GetChild(i).GetChild(0).GetChild(2).GetChild(0).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
                onlineObservingListContent.transform.GetChild(i).GetChild(1).GetChild(2).GetChild(0).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
            }

            for (int i = 0; i < userImageForObservingCountry.Count; i++)
            {
                if (!string.IsNullOrEmpty(userImageForObservingCountry[i]))
                {
                    onlineObservingListContent.transform.GetChild(i).GetChild(0).GetChild(2).GetChild(0).GetComponent<Image>().sprite = playerImageInObservingCountry[countForObservingCountry].imgPic;
                    onlineObservingListContent.transform.GetChild(i).GetChild(1).GetChild(2).GetChild(0).GetComponent<Image>().sprite = playerImageInObservingCountry[countForObservingCountry].imgPic;
                    countForObservingCountry++;
                }
            }

        }
    }

    #endregion


    public void CalculateTime()
    {
       
        for (int i = 0; i < onlinePlayingListContent.transform.childCount; i++)
        {
            if (!isPlayingTime)
            {

                oldHour.Add(Convert.ToInt32(onlinePlayingListContent.transform.GetChild(i).GetChild(1).GetChild(7).GetChild(1).GetComponent<Text>().text.Substring(0, 2)));
                oldMin.Add(Convert.ToInt32(onlinePlayingListContent.transform.GetChild(i).GetChild(1).GetChild(7).GetChild(1).GetComponent<Text>().text.Substring(3, 2)));

                onlinePlayingListContent.transform.GetChild(i).GetChild(1).GetChild(7).GetChild(1).GetComponent<Text>().text = Substract(DateTime.Now, oldHour[i], oldMin[i], 0);
                print("CalculateTime...first...");
            }
            else
            {
                print("CalculateTime...second...");
                onlinePlayingListContent.transform.GetChild(i).GetChild(1).GetChild(7).GetChild(1).GetComponent<Text>().text = Substract(DateTime.Now, oldHour[i], oldMin[i], 0);
            }
        }
        isPlayingTime = true;

        if (activeStatusObj.activeInHierarchy)
        {
            InvokeRepeating("CalculateTime", 60f, 60f);
        }
        else
        {
            CancelInvoke();
        }
    }


   public void CalculateTime1()
    {
        
        for (int i = 0; i < onlineObservingListContent.transform.childCount; i++)
        {
            if (!isPlayingTime1)
            {
                oldHour1.Add(Convert.ToInt32(onlineObservingListContent.transform.GetChild(i).GetChild(1).GetChild(7).GetChild(1).GetComponent<Text>().text.Substring(0, 2)));
                oldMin1.Add(Convert.ToInt32(onlineObservingListContent.transform.GetChild(i).GetChild(1).GetChild(7).GetChild(1).GetComponent<Text>().text.Substring(3, 2)));

                onlineObservingListContent.transform.GetChild(i).GetChild(1).GetChild(7).GetChild(1).GetComponent<Text>().text = Substract(DateTime.Now, oldHour1[i], oldMin1[i], 0);
                print("CalculateTime1...first...");
            }
            else
            {
                print("CalculateTime1...second...");
                onlineObservingListContent.transform.GetChild(i).GetChild(1).GetChild(7).GetChild(1).GetComponent<Text>().text = Substract(DateTime.Now, oldHour1[i], oldMin1[i], 0);
            }
        }
        isPlayingTime1 = true;
        if (activeStatusObj.activeInHierarchy)
        {
            InvokeRepeating("CalculateTime1", 60f, 60f);
        }
        else
        {
            CancelInvoke();
        }
        
    }

    public static string Substract(DateTime now, int hours, int minutes, int seconds)
    {
        TimeSpan T1 = new TimeSpan(hours, minutes, seconds);

        DateTime dateTime = now.Subtract(T1);
        string str = dateTime.ToString("HH:mm");
        return str;
    }

    void GeneratePlayingObj()
    {
        GameObject obj = Instantiate(onlinePlayingListPanel);
        obj.transform.SetParent(onlinePlayingListContent.transform, false);
        onlinePlayingListObj.Add(obj);
    }

    void GenerateObservingObj()
    {
        GameObject obj = Instantiate(onlineObservingListPanel);
        obj.transform.SetParent(onlineObservingListContent.transform, false);
        onlineObservingListObj.Add(obj);
    }

    void ResetOnlineActivePlayers()
    {
        if (onlinePlayingListObj.Count > 0)
        {
            for (int i = 0; i < onlinePlayingListObj.Count; i++)
            {
                Destroy(onlinePlayingListObj[i]);
            }
            onlinePlayingListObj.Clear();
            onlinePlayingListCount = 1;

            ResetPlayerImageInPlaying();
            ResetPlayerImageInPlayingCountry();
        }
        if (onlineObservingListObj.Count > 0)
        {
            for (int i = 0; i < onlineObservingListObj.Count; i++)
            {
                Destroy(onlineObservingListObj[i]);
            }
            onlineObservingListObj.Clear();
            onlineObservingListCount = 1;

            ResetPlayerImageInObserving();
            ResetPlayerImageInObservingCountry();
        }

    }

    #endregion

    #region Click On Members List In Create table
    public void ClickMembersListInCreateTable()
    {
        memberList.club_id = _clubID;
        memberList.club_role = AssignClubRoleInNumberFormat(currentSelectedClubObject.transform.GetChild(0).GetChild(3).GetComponent<Text>().text);
        string body = JsonUtility.ToJson(memberList);
        print(body);
        loadingPanel.SetActive(true);
        //Communication.instance.PostData(memberListUrl, body, MembersListInCreateTableProcess);

        searchInCreateTableInputField.text = string.Empty;
    }
    int memberConutInCreateTable;
   
    public List<GameObject> membersObjInCreateTable;
    void MembersListInCreateTableProcess(string response)
    {
        loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("error");
        }
        else
        {
            print("" + response);
            getMemberList = JsonUtility.FromJson<GetMemberList>(response);

            if (!getMemberList.error)
            {
                if (getMemberList.data.Length != memberConutInCreateTable)
                {
                    for (int i = memberConutInCreateTable; i < getMemberList.data.Length; i++)
                    {
                        memberConutInCreateTable++;
                        GenerateMembersItemInCreateTable();
                    }
                }
                userImageInCreateTable.Clear();
                
                SearchingScript.instance.memberListNameInCreateTable.Clear();

                for (int i = 0; i < getMemberList.data.Length; i++)
                {
                    membersInCreateTableContent.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = getMemberList.data[i].username;
                    membersInCreateTableContent.transform.GetChild(i).GetChild(2).GetComponent<Text>().text = getMemberList.data[i].client_id;
                    membersInCreateTableContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text = getMemberList.data[i].user_id;

                    userImageInCreateTable.Add(getMemberList.data[i].user_image);
                    membersInCreateTableContent.transform.GetChild(i).gameObject.SetActive(true);
                    SearchingScript.instance.memberListNameInCreateTable.Add(membersInCreateTableContent.transform.GetChild(i));
                }
                UpdatePlayerImageInCreateTable();

            }
        }
    }

    void GenerateMembersItemInCreateTable()
    {
        scrollItemObj = Instantiate(membersInCreateTablePanel);
        scrollItemObj.transform.SetParent(membersInCreateTableContent.transform, false);
        membersObjInCreateTable.Add(scrollItemObj);
    }

   public void ResetGenerateMembersInCreateTable()
    {
        if (membersObjInCreateTable.Count > 0)
        {
            for (int i = 0; i < membersObjInCreateTable.Count; i++)
            {
                Destroy(membersObjInCreateTable[i]);
            }
            membersObjInCreateTable.Clear();
            memberConutInCreateTable = 1;
        }
    }

    #region update Player Image In Create Table

    public List<string> userImageInCreateTable;
    
    [SerializeField]
    private List<PlayerImageInSequenceInCreateTable> playerImageInSequenceInCreateTable;

    private int k2 = 0;
    private int totalImageCountInCreateTable;
    private int countInCreateTable = 0;

    private int previousCountForMemberListInCreateTable;
    void UpdatePlayerImageInCreateTable()
    {
        if (userImageInCreateTable.Count == previousCountForMemberListInCreateTable)
        {
            return;
        }

        k2 = 0;
        previousCountForMemberListInCreateTable = 0;
        totalImageCountInCreateTable = 0;
        playerImageInSequenceInCreateTable.Clear();

        playerImageInSequenceInCreateTable = new List<PlayerImageInSequenceInCreateTable>();

        for (int i = 0; i < userImageInCreateTable.Count; i++)
        {
            if (!string.IsNullOrEmpty(userImageInCreateTable[i]))
            {
                playerImageInSequenceInCreateTable.Add(new PlayerImageInSequenceInCreateTable());
                loadingPanel.SetActive(true);
                playerImageInSequenceInCreateTable[k2].imgUrl = userImageInCreateTable[i];
                playerImageInSequenceInCreateTable[k2].ImageProcess(userImageInCreateTable[i]);

                k2 = k2 + 1;

            }
            previousCountForMemberListInCreateTable++;
        }
    }

    public void ApplyImageInCreateTable()
    {
        print(k2);
        print(totalImageCountInCreateTable);
        if (k2 == totalImageCountInCreateTable)
        {
            countInCreateTable = 0;
            loadingPanel.SetActive(false);

            for (int i = 0; i < userImageInCreateTable.Count; i++)
            {
                membersInCreateTableContent.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
            }

            for (int i = 0; i < userImageInCreateTable.Count; i++)
            {
                if (!string.IsNullOrEmpty(userImageInCreateTable[i]))
                {
                    print("i = " + i);
                    membersInCreateTableContent.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = playerImageInSequenceInCreateTable[countInCreateTable].imgPic;
                    countInCreateTable++;
                }
            }

        }
    }

    [Serializable]
    public class PlayerImageInSequenceInCreateTable
    {
        public string imgUrl;
        public Sprite imgPic;

        public void ImageProcess(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                Communication.instance.GetImage(url, ImageResponse);
            }
        }

        void ImageResponse(Sprite response)
        {
            if (response != null)
            {
                imgPic = response;
                instance.totalImageCountInCreateTable++;
                instance.ApplyImageInCreateTable();
            }
        }
    }

    public void ResetAllValuesForImageInCreateTable()
    {
        totalImageCountInCreateTable = 0;
        k2 = 0;
        countInCreateTable = 0;
        previousCountForMemberListInCreateTable = 0;
        userImageInCreateTable.Clear();
        playerImageInSequenceInCreateTable.Clear();       
    }

    #endregion

    #endregion

}





