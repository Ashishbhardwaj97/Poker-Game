using SmartLocalization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SocialTournamentScript : MonoBehaviour
{
    public static SocialTournamentScript instance;

    [Header("GameObject Reference")]
    public GameObject upcomingTournyContent;
    public GameObject upcomingTournyPanel;
    public GameObject registeredTournyContent;
    public GameObject registeredTournyPanel;
    public GameObject registeredSuccessfullyPanel;
    public GameObject tournamentGameDetailPanel;
    public GameObject bottomPanel;
    public GameObject detailPage;
    public GameObject entriesPage;
    public GameObject tablesPage;
    public GameObject rewardsPage;
    public GameObject blindStructurePage;
    public GameObject registeredPanel_Popup;
    


    public Sprite silverTrophy;
    public Sprite bronzeTrophy;

    public GameObject registeredScrollView;
    public GameObject upcomingScrollView;

    public List<GameObject> selectedBtnTourneyUI;

    [Header("List Properties")]
    public List<GameObject> upcomingTournyList;
    public List<GameObject> registeredTournyList;
    public List<GameObject> gameDetailTabNavigationList;
    public List<GameObject> gameDetailTabList;
    public List<GameObject> bottomPanelbtns;
    
    string upcomingTournyUrl;
    string registeredTournyUrl;
    string registerForTournamentUrl;
    string unregisterFromTournamentUrl;
    string tournamentEntriesUrl;
    string tournamentRewardsUrl;

    int upcomingCount;
    int registeredCount;

    GameObject scrollItemObj;

    internal string throwableDestination;
    internal string throwableSource;
    internal int throwableCharge;
    internal string animationName;
    internal string observingTableNumber;

    public bool isTournamentVideo;

    public string login_error;
    public string startTime;
    public Text PlayerName;
    



    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        upcomingTournyUrl = ServerChanger.instance.domainURL + "api/v1/socialtable/upcoming-tournament-list";
        registeredTournyUrl = ServerChanger.instance.domainURL + "api/v1/socialtable/user-tournament-list";
        registerForTournamentUrl = ServerChanger.instance.domainURL + "api/v1/socialtable/create-tournament-register";
        unregisterFromTournamentUrl = ServerChanger.instance.domainURL + "api/v1/socialtable/unregister-tournament";
        tournamentEntriesUrl = ServerChanger.instance.domainURL + "api/v1/socialtable/tournament-register-list";

        tournamentRewardsUrl = ServerChanger.instance.domainURL + "api/v1/game/wining-list";

        upcomingCount = 1;
        registeredCount = 1;
        memberConut = 1;
        rankCount = 1;
        rewardCount = 1;
        blindDetailsCount = 1;
        tableCount = 1;

        upcomingTournyList = new List<GameObject>();
        registeredTournyList = new List<GameObject>();
    }

    public void FindLocalPlayer()
    {
        for (int i = 0; i < GameManagerScript.instance.playersParent.transform.childCount; i++)
        {
            if(GameManagerScript.instance.playersParent.transform.GetChild(i).childCount == 2)
            {
                if(GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetComponent<PokerPlayerController>().isLocalPlayer)
                {
                    throwableSource = GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).gameObject.name;
                }
            }
        }
    }

    public void TurnOnOffBottomPanelBtns(int firstBtn, int secondBtn, bool isAllfalse)
    {
        print("TurnOnOffBottomPanelBtns()");
        if (isAllfalse)
        {
            for (int i = 0; i < bottomPanelbtns.Count; i++)
            {
                bottomPanelbtns[i].SetActive(false);
            }
        }
        else
        {
            if (secondBtn != -1)
            {
                print("if...TurnOnOffBottomPanelBtns");
                for (int i = 0; i < bottomPanelbtns.Count; i++)
                {
                    bottomPanelbtns[i].SetActive(false);
                }
                bottomPanelbtns[firstBtn].SetActive(true);
                bottomPanelbtns[secondBtn].SetActive(true);
            }
            else
            {
                print("else...TurnOnOffBottomPanelBtns");
                for (int i = 0; i < bottomPanelbtns.Count; i++)
                {
                    bottomPanelbtns[i].SetActive(false);
                }
                bottomPanelbtns[firstBtn].SetActive(true);
            }
        }
    }
    public void CloseAllBottomButtons()
    {
        for (int i = 0; i < bottomPanelbtns.Count; i++)
        {
            bottomPanelbtns[i].SetActive(false);
        }
    }


    #region Upcoming Tournament List

    [Serializable]
    public class TournamentResponse
    {
        public bool error;
        public UpcomingData[] upcomingData;
        public int statusCode;
    }

    [Serializable]
    public class UpcomingData
    {
        public string table_name;
        public string game_type;
        public string start_time;
        public string tour_id;
        public int buyin;
        public int table_size;
        public int count;
        public int tournament_status;
        public bool video;
        public bool observer_audio;
    }
    [Serializable]
    public class TournamentReq
    {
        public string timezone;
    }
    [SerializeField] TournamentReq tournamentReq;

    [SerializeField] TournamentResponse tournamentResponse;

    public void TournamentRequest()
    {
        if (string.IsNullOrEmpty(Registration.instance.timeZone))
        {
            StartCoroutine(Registration.instance.DetectCountry());
        }
        tournamentReq.timezone = Registration.instance.timeZone;
        string body = JsonUtility.ToJson(tournamentReq);
        print(body);

        if (!PokerSceneManagement.instance.isSceneRestart)
        {

            if (!string.IsNullOrEmpty(Registration.instance.timeZone))
            {
                ClubManagement.instance.loadingPanel.SetActive(true);
                Communication.instance.PostData(upcomingTournyUrl, body, TournamentCallback);
                //print(upcomingTournyUrl);
            }
            else
            {
                Cashier.instance.toastMsg.text = LanguageManager.Instance.GetTextValue("allow location");
                Cashier.instance.toastMsgPanel.SetActive(true);
            }
        }
    }

    void TournamentCallback(string response)
    {
        //SocialGame.instance.pokerUICanvas.GetComponent<Canvas>().enabled = true;
        ClubManagement.instance.loadingPanel.SetActive(false);
        print(response);
        if (!string.IsNullOrEmpty(response))
        {
            tournamentResponse = JsonUtility.FromJson<TournamentResponse>(response);

            if (!tournamentResponse.error)
            {
                SocialPokerGameManager.instance.EnableTournamentPage();
                if (tournamentResponse.upcomingData.Length != upcomingCount)
                {
                    for (int i = upcomingCount; i < tournamentResponse.upcomingData.Length; i++)
                    {
                        upcomingCount++;
                        GenerateUpcomingTournyList();
                    }
                }

                for (int i = 0; i < upcomingTournyContent.transform.childCount; i++)
                {
                    upcomingTournyContent.transform.GetChild(i).gameObject.SetActive(false);
                }

                for (int i = 0; i < tournamentResponse.upcomingData.Length; i++)
                {
                    upcomingTournyContent.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = tournamentResponse.upcomingData[i].table_name + "            ";
                    upcomingTournyContent.transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>().text = "(" + tournamentResponse.upcomingData[i].table_size.ToString() + " " + "Max" + ")";

                    string dateTime = tournamentResponse.upcomingData[i].start_time;
                    upcomingTournyContent.transform.GetChild(i).GetChild(11).GetComponent<Text>().text = dateTime;
                    string date = dateTime.Substring(8, 2) + "/" + dateTime.Substring(5, 2);
                    string time = dateTime.Substring(11, 5);
                    upcomingTournyContent.transform.GetChild(i).GetChild(2).GetChild(0).GetComponent<Text>().text = date;
                    upcomingTournyContent.transform.GetChild(i).GetChild(3).GetChild(0).GetComponent<Text>().text = time;

                    upcomingTournyContent.transform.GetChild(i).GetChild(4).GetChild(0).GetComponent<Text>().text = tournamentResponse.upcomingData[i].count.ToString();
                    upcomingTournyContent.transform.GetChild(i).GetChild(5).GetChild(0).GetComponent<Text>().text = tournamentResponse.upcomingData[i].buyin.ToString();
                    upcomingTournyContent.transform.GetChild(i).GetChild(8).GetComponent<Text>().text = tournamentResponse.upcomingData[i].tour_id;

                    upcomingTournyContent.transform.GetChild(i).GetChild(13).GetComponent<Text>().text = tournamentResponse.upcomingData[i].observer_audio.ToString();


                    if (tournamentResponse.upcomingData[i].game_type == "NLH")
                    {
                        upcomingTournyContent.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(true);
                        upcomingTournyContent.transform.GetChild(i).GetChild(0).GetChild(2).gameObject.SetActive(false);
                    }

                    else
                    {
                        upcomingTournyContent.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(false);
                        upcomingTournyContent.transform.GetChild(i).GetChild(0).GetChild(2).gameObject.SetActive(true);
                    }

                    if (tournamentResponse.upcomingData[i].video)
                    {
                        upcomingTournyContent.transform.GetChild(i).GetChild(0).GetChild(1).gameObject.SetActive(true);
                        upcomingTournyContent.transform.GetChild(i).GetChild(0).GetChild(3).gameObject.SetActive(false);
                    }

                    else
                    {
                        upcomingTournyContent.transform.GetChild(i).GetChild(0).GetChild(1).gameObject.SetActive(false);
                        upcomingTournyContent.transform.GetChild(i).GetChild(0).GetChild(3).gameObject.SetActive(true);
                    }
                    if (tournamentResponse.upcomingData[i].tournament_status == 0)
                    {
                        print("Tournament Not Started");
                        upcomingTournyContent.transform.GetChild(i).GetChild(7).gameObject.SetActive(true);
                        upcomingTournyContent.transform.GetChild(i).GetChild(12).gameObject.SetActive(false);
                    }

                    else if (tournamentResponse.upcomingData[i].tournament_status == 1)
                    {
                        print("Tournament Started");
                        upcomingTournyContent.transform.GetChild(i).GetChild(7).gameObject.SetActive(false);
                        upcomingTournyContent.transform.GetChild(i).GetChild(12).gameObject.SetActive(true);
                    }

                    else if (tournamentResponse.upcomingData[i].tournament_status == 9)
                    {
                        print("Restricted for tourny register");
                        upcomingTournyContent.transform.GetChild(i).GetChild(7).gameObject.SetActive(false);
                        upcomingTournyContent.transform.GetChild(i).GetChild(12).gameObject.SetActive(false);
                    }

                    else
                    {
                        upcomingTournyContent.transform.GetChild(i).GetChild(7).gameObject.SetActive(false);
                        upcomingTournyContent.transform.GetChild(i).GetChild(12).gameObject.SetActive(false);
                    }
                    upcomingTournyContent.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
            else
            {
                if (tournamentResponse.statusCode == 403)
                {
                    Cashier.instance.toastMsg.text = LanguageManager.Instance.GetTextValue("session expired");// login_error;
                    Cashier.instance.toastMsgPanel.SetActive(true);
                    Uimanager.instance.SignOut();
                }
            }
        }

        else
        {
            Cashier.instance.toastMsg.text = LanguageManager.Instance.GetTextValue("internet check");//SocialProfile._instance.networkErrorMsg;
            Cashier.instance.toastMsgPanel.SetActive(true);
            SocialPokerGameManager.instance.ClickHome();
        }

    }


    void GenerateUpcomingTournyList()
    {
        scrollItemObj = Instantiate(upcomingTournyPanel);
        scrollItemObj.transform.SetParent(upcomingTournyContent.transform, false);
        upcomingTournyList.Add(scrollItemObj);
    }

    public void ResetUpcomingTournyList()
    {
        if (upcomingTournyList.Count > 0)
        {
            for (int i = 0; i < upcomingTournyList.Count; i++)
            {
                Destroy(upcomingTournyList[i]);
            }
            upcomingTournyList.Clear();
            upcomingCount = 1;
        }
    }

    #endregion

    #region Registered Tournament List

    [Serializable]
    public class RegisteredTournamentResponse
    {
        public bool error;
        public RegisteredData[] registeredData;
        public int statusCode;
        public int registeredUsers;
    }

    [Serializable]

    public class RegisteredData
    {
        public Tournament tournament;
        public string start_time;
    }

    [Serializable]
    public class Tournament
    {
        public string table_name;
        public string game_type;
        //public string start_time;
        public string tour_id;
        public int buyin;
        public int table_size;
        public int count;
        public int tournament_status;
        public bool video;
        public bool observer_audio;
    }

    [SerializeField]

     public RegisteredTournamentResponse registeredTournamentResponse;

    public void RegisteredTournamentRequest()
    {
        //.......for table ....//
        if (string.IsNullOrEmpty(Registration.instance.timeZone))
        {
            StartCoroutine(Registration.instance.DetectCountry());
        }
        tournamentReq.timezone = Registration.instance.timeZone;
        string body = JsonUtility.ToJson(tournamentReq);

        print(body);
        
        ClubManagement.instance.loadingPanel.SetActive(true);
        Communication.instance.PostData(registeredTournyUrl, body, RegisteredTournamentCallback);
       
    }

    void RegisteredTournamentCallback(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        print(response);
        if (!string.IsNullOrEmpty(response))
        {
            registeredTournamentResponse = JsonUtility.FromJson<RegisteredTournamentResponse>(response);
            if (!registeredTournamentResponse.error)
            {
                print("successfull.....");

                if (registeredTournamentResponse.registeredData.Length != registeredCount)
                {
                    for (int i = registeredCount; i < registeredTournamentResponse.registeredData.Length; i++)
                    {
                        registeredCount++;
                        GenerateRegisteredTournyList();
                    }
                }

                for (int i = 0; i < registeredTournyContent.transform.childCount; i++)
                {
                    registeredTournyContent.transform.GetChild(i).gameObject.SetActive(false);
                }

                for (int i = 0; i < registeredTournamentResponse.registeredData.Length; i++)
                {
                    registeredTournyContent.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = registeredTournamentResponse.registeredData[i].tournament.table_name + "            ";
                    registeredTournyContent.transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>().text = "(" + registeredTournamentResponse.registeredData[i].tournament.table_size.ToString() + " " + "Max" + ")";

                    string dateTime = registeredTournamentResponse.registeredData[i].start_time;
                    registeredTournyContent.transform.GetChild(i).GetChild(11).GetComponent<Text>().text = dateTime;
                    string date = dateTime.Substring(8, 2) + "/" + dateTime.Substring(5, 2);
                    //print(date);
                    string time = dateTime.Substring(11, 5);
                    //print(time);

                    registeredTournyContent.transform.GetChild(i).GetChild(2).GetChild(0).GetComponent<Text>().text = date;
                    registeredTournyContent.transform.GetChild(i).GetChild(3).GetChild(0).GetComponent<Text>().text = time;

                    registeredTournyContent.transform.GetChild(i).GetChild(4).GetChild(0).GetComponent<Text>().text = registeredTournamentResponse.registeredData[i].tournament.count.ToString();

                    registeredTournyContent.transform.GetChild(i).GetChild(5).GetChild(0).GetComponent<Text>().text = registeredTournamentResponse.registeredData[i].tournament.buyin.ToString();
                    registeredTournyContent.transform.GetChild(i).GetChild(8).GetComponent<Text>().text = registeredTournamentResponse.registeredData[i].tournament.tour_id;

                    registeredTournyContent.transform.GetChild(i).GetChild(12).GetComponent<Text>().text = registeredTournamentResponse.registeredData[i].tournament.observer_audio.ToString();

                    if (registeredTournamentResponse.registeredData[i].tournament.game_type == "NLH")
                    {
                        registeredTournyContent.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(true);
                        registeredTournyContent.transform.GetChild(i).GetChild(0).GetChild(2).gameObject.SetActive(false);
                    }
                    else
                    {
                        registeredTournyContent.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(false);
                        registeredTournyContent.transform.GetChild(i).GetChild(0).GetChild(2).gameObject.SetActive(true);
                    }

                    if (registeredTournamentResponse.registeredData[i].tournament.video)
                    {
                        registeredTournyContent.transform.GetChild(i).GetChild(0).GetChild(1).gameObject.SetActive(true);
                        registeredTournyContent.transform.GetChild(i).GetChild(0).GetChild(3).gameObject.SetActive(false);
                    }

                    else
                    {
                        registeredTournyContent.transform.GetChild(i).GetChild(0).GetChild(1).gameObject.SetActive(false);
                        registeredTournyContent.transform.GetChild(i).GetChild(0).GetChild(3).gameObject.SetActive(true);
                    }

                    if(registeredTournamentResponse.registeredData[i].tournament.tournament_status == 0)
                    {
                        print("Tournament Not Started");
                        registeredTournyContent.transform.GetChild(i).GetChild(7).gameObject.SetActive(true);
                        registeredTournyContent.transform.GetChild(i).GetChild(9).gameObject.SetActive(false);
                    }

                    else if(registeredTournamentResponse.registeredData[i].tournament.tournament_status == 1)
                    {
                        print("Tournament Started");
                        registeredTournyContent.transform.GetChild(i).GetChild(7).gameObject.SetActive(false);
                        registeredTournyContent.transform.GetChild(i).GetChild(9).gameObject.SetActive(true);
                    }

                    else
                    {
                        registeredTournyContent.transform.GetChild(i).GetChild(7).gameObject.SetActive(false);
                        registeredTournyContent.transform.GetChild(i).GetChild(9).gameObject.SetActive(false);
                    }

                    registeredTournyContent.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
            else
            {
                if (registeredTournamentResponse.statusCode == 403)
                {
                    Cashier.instance.toastMsg.text = LanguageManager.Instance.GetTextValue("session expired");// login_error;
                    Cashier.instance.toastMsgPanel.SetActive(true);
                    Uimanager.instance.SignOut();
                }
            }
        }

    }

    void GenerateRegisteredTournyList()
    {
        scrollItemObj = Instantiate(registeredTournyPanel);
        scrollItemObj.transform.SetParent(registeredTournyContent.transform, false);
        registeredTournyList.Add(scrollItemObj);
    }

    public void ResetRegisteredTournyList()
    {
        if (registeredTournyList.Count > 0)
        {
            for (int i = 0; i < registeredTournyList.Count; i++)
            {
                Destroy(registeredTournyList[i]);
            }
            registeredTournyList.Clear();
            registeredCount = 1;
        }
    }

    #endregion

    #region Click Registered Tab / Upcoming Tab

    public void ClickTournamentsTab(Transform btn)
    {
        for (int i = 0; i < selectedBtnTourneyUI.Count; i++)
        {
            selectedBtnTourneyUI[i].transform.GetChild(0).gameObject.SetActive(false);
            selectedBtnTourneyUI[i].transform.GetChild(1).gameObject.SetActive(false);
            selectedBtnTourneyUI[i].transform.GetChild(2).gameObject.SetActive(true);
            selectedBtnTourneyUI[i].transform.GetComponent<Button>().enabled = true;
        }

        btn.GetChild(0).gameObject.SetActive(true);
        btn.GetChild(1).gameObject.SetActive(true);
        btn.GetChild(2).gameObject.SetActive(false);
        btn.GetComponent<Button>().enabled = false;

        //bottomPanel.transform.GetChild(0).gameObject.SetActive(false);
        //bottomPanel.transform.GetChild(1).gameObject.SetActive(false);
        //bottomPanel.transform.GetChild(2).gameObject.SetActive(false);
        //bottomPanel.transform.GetChild(3).gameObject.SetActive(false);

        TurnOnOffBottomPanelBtns(0, 0, true);


        if (btn.CompareTag("Registered"))
        {
            RegisteredTournamentRequest();
            upcomingScrollView.SetActive(false);
            registeredScrollView.SetActive(true);
        }
        else if (btn.CompareTag("Upcoming"))
        {
            TournamentRequest();
            upcomingScrollView.SetActive(true);
            registeredScrollView.SetActive(false);
        }
    }

    public void ClickUpcomingTab()
    {
        ClickTournamentsTab(selectedBtnTourneyUI[1].transform);
    }

    #endregion

    #region Register for Tournament

    [Serializable]
    public class RegisterInfo
    {
        public string tournament_id;
        public string user_image;
    }

    [SerializeField] RegisterInfo registerInfo;

    public class RegisterResponseInfo
    {
        public bool error;
        public bool is_registerd;
        //public string errors;
        public Errors errors;
        public int statusCode;
    }

    public class Errors
    {
        public Error error;
    }

    public class Error
    {
        public Properties properties;
    }

    public class Properties
    {
        public string message;
    }


    [SerializeField] RegisterResponseInfo registerResponse;


    Transform registerBtn;
    public void Register(GameObject obj)
    {
        print(obj.transform.parent);
        registerBtn = obj.transform;
        isObserverAudio = bool.Parse(obj.transform.parent.GetChild(13).GetComponent<Text>().text);
        if (!tournamentGameDetailPanel.activeInHierarchy)
        {
            startTime = obj.transform.parent.GetChild(11).GetComponent<Text>().text;
            if (obj.transform.parent.GetChild(0).GetChild(1).gameObject.activeInHierarchy)
            {
                isTournamentVideo = true;
            }
            else
            {
                isTournamentVideo = false;
            }
        }

        ClubManagement.instance.loadingPanel.SetActive(true);
        registerInfo.tournament_id = obj.transform.parent.GetChild(8).GetComponent<Text>().text;
        registerInfo.user_image = ApiHitScript.instance.updatedUserImageUrl;
        print("ID: " + registerInfo.tournament_id);
        string body = JsonUtility.ToJson(registerInfo);
        print(body);
        Communication.instance.PostData(registerForTournamentUrl, body, RegisterCallback);
    }

    void RegisterCallback(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        print(response);
        if (!string.IsNullOrEmpty(response))
        {
            registerResponse = JsonUtility.FromJson<RegisterResponseInfo>(response);

            if (!registerResponse.error)
            {
                print("LateRegistrationEmitter_tournament_status" + GameSerializeClassesCollection.instance.tournament.tournament_status);
                if (GameSerializeClassesCollection.instance.tournament.tournament_status == 1 /*&& GameSerializeClassesCollection.instance.tournament.tournament_id == tournament_ID*/)
                {
                    TournamentManagerScript.instance.LateRegistrationEmitter();
                    CloseAllBottomButtons();
                }
                else
                {
                    print("Successfully Registered");
                    isRegistered = registerResponse.is_registerd;
                    if (!tournamentGameDetailPanel.activeInHierarchy)
                    {
                        //registeredSuccessfullyPanel.SetActive(true);
                        OpenGameDetailPageAfterRegistration(registerInfo.tournament_id, isRegistered);
                    }

                    else
                    {
                        SocialProfile._instance.SocialChips();
                        GameManagerScript.instance.isTournament = true;
                        SocialGame.instance.StartGame();
                        isRegistered = true;
                        tournament_ID = registerInfo.tournament_id;
                    }
                }
            }
            else
            {
                print("Some error in Registering");
                if (registerResponse.error)
                {
                    registeredPanel_Popup.SetActive(false);
                    TurnOnOffBottomPanelBtns(0, -1, false);

                    Cashier.instance.toastMsg.text = "Maximum players registered";
                    Cashier.instance.toastMsgPanel.SetActive(true);

                }
                else if (registerResponse.statusCode == 401)
                {
                    Cashier.instance.toastMsg.text = LanguageManager.Instance.GetTextValue("Insufficient Balance");// Insufficient Balance_error;
                    Cashier.instance.toastMsgPanel.SetActive(true);
                    print("error....401..");
                }
                else if (registerResponse.statusCode == 404)
                {
                    Cashier.instance.toastMsg.text = LanguageManager.Instance.GetTextValue("Tour reg error");// tournament_Registration_error;
                    Cashier.instance.toastMsgPanel.SetActive(true);
                    print("error......");
                }
                else if (registerResponse.statusCode == 402)
                {
                    Cashier.instance.toastMsg.text = LanguageManager.Instance.GetTextValue("please try");// Tournament table is preparing, please try again in sometime
                    Cashier.instance.toastMsgPanel.SetActive(true);
                    registerBtn.GetComponent<Button>().interactable = false;
                    StartCoroutine(EnableRegisterBtn(registerBtn));
                    //Uimanager.instance.SignOut();
                }
                else if (registerResponse.statusCode == 403)
                {
                    Cashier.instance.toastMsg.text = LanguageManager.Instance.GetTextValue("session expired");// login_error;
                    Cashier.instance.toastMsgPanel.SetActive(true);
                    Uimanager.instance.SignOut();
                }


            }
        }
        else
        {
            if (string.IsNullOrEmpty(response))
            {
                if (tournamentGameDetailPanel.activeInHierarchy)
                {
                    if (GameSerializeClassesCollection.instance.tournament.tournament_status == 1)
                    {
                        CloseAllBottomButtons();
                        TurnOnOffBottomPanelBtns(4, 5, false);
                    }
                    else
                    {
                        CloseAllBottomButtons();
                        TurnOnOffBottomPanelBtns(0, -1, false);
                    }
                }
            }
        }
    }

    IEnumerator EnableRegisterBtn(Transform btn)
    {
        yield return new WaitForSeconds(20f);

        btn.GetComponent<Button>().interactable = true;
    }

    public void OpenGameDetailPageAfterRegistration(string tourID, bool isRegister)
    {


        print("OpenGameDetailPageAfterRegistration");
        CloseAllBottomButtons();

        ClubManagement.instance.loadingPanel.SetActive(true);
        ClickOnDetails();

        GameManagerScript.instance.isTournament = true;
        SocialGame.instance.StartGame();
        isRegistered = isRegister;
        tournament_ID = tourID;

        StartCoroutine(CheckServerConnection());
    }

    IEnumerator CheckServerConnection()
    {
        print("CheckServerConnection");
        StartCoroutine(TimeoutChecker(10));
        while (true)
        {
            if (TournamentManagerScript.instance.socket.wsConnected)
            {
                print("tournamentGameDetailPanel true");
                //if (GameSerializeClassesCollection.instance.tournament.tournament_status == 1 && isRegistered)
                //{
                //    bottomPanel.transform.GetChild(2).gameObject.SetActive(false);
                //}
                tournamentGameDetailPanel.SetActive(true);
                if (isTournamentRunning)
                {
                    yield return new WaitForSeconds(2);

                    Enter();
                }
                break;
            }
            else if (timedOut)
            {
                print("timedOut true");
                SocialPokerGameManager.instance.ClickTournament();
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        print("pokerUICanvas" + SocialGame.instance.pokerUICanvas.GetComponent<Canvas>().enabled);
        SocialGame.instance.pokerUICanvas.GetComponent<Canvas>().enabled = true;
        PokerSceneManagement.instance.isSceneRestart = false;
    }

    bool timedOut;
    private IEnumerator TimeoutChecker(float timeout)
    {
        timedOut = false;
        while (timeout > 0)
        {
            timeout -= Time.deltaTime;
            yield return null;
        }
        timedOut = true;
    }

    public string tournament_ID;
    public bool isRegistered;
    public bool isObserverAudio;
    public void ClickOnViewMoreFromUpcoming(GameObject obj)
    {
        CloseAllBottomButtons();
        isObserverAudio = bool.Parse(obj.transform.parent.GetChild(13).GetComponent<Text>().text);
        ClickOnDetails();
        if (obj.transform.parent.GetChild(0).GetChild(1).gameObject.activeInHierarchy)
        {
            isTournamentVideo = true;
        }
        else
        {
            isTournamentVideo = false;
        }

        //registerObj = obj.transform.parent;
        GameManagerScript.instance.isTournament = true;
        SocialGame.instance.StartGame();
        isRegistered = false;
        tournament_ID = obj.transform.parent.GetChild(8).GetComponent<Text>().text;
        startTime = obj.transform.parent.GetChild(11).GetComponent<Text>().text;
    }

    public IEnumerator TournamentCheck()
    {
        yield return new WaitForSeconds(0.2f);
        ClubManagement.instance.loadingPanel.SetActive(false);
        //if (GameSerializeClassesCollection.instance.tournament.tournament_status == 1 || GameSerializeClassesCollection.instance.tournament.tournament_status == 3)
        //{
        //    bottomPanel.transform.GetChild(2).gameObject.SetActive(false);
        //}
        print("MTT2....");
        TournamentManagerScript.instance.MttEntyEmitter(false);
        yield return new WaitForSeconds(0.1f);
        TournamentManagerScript.instance.IsRegisteredEmitter();
        yield return new WaitForSeconds(0.1f);
        TournamentManagerScript.instance.CountDownDetailPageEmitter();
       
        yield return new WaitForSeconds(1f);
        TournamentManagerScript.instance.PlayerExistEmitter();
        tournamentGameDetailPanel.SetActive(true);

        yield return new WaitForSeconds(2f);
        TournamentManagerScript.instance.MttEntyEmitter(false);
        //

    }

    public void ClickOnViewMoreFromRegistered(GameObject obj)
    {
        CloseAllBottomButtons();
        isObserverAudio = bool.Parse(obj.transform.parent.GetChild(12).GetComponent<Text>().text);
        ClickOnDetails();
        if (obj.transform.parent.GetChild(0).GetChild(1).gameObject.activeInHierarchy)
        {
            isTournamentVideo = true;
        }
        else
        {
            isTournamentVideo = false;
        }

        GameManagerScript.instance.isTournament = true;
        SocialGame.instance.StartGame();

        isRegistered = true;
        tournament_ID = obj.transform.parent.GetChild(8).GetComponent<Text>().text;
        startTime = obj.transform.parent.GetChild(11).GetComponent<Text>().text;
    }

    public void RegisterOnGameDetailPage()
    {
        isRegistered = true;
        if (GameSerializeClassesCollection.instance.tournament.tournament_status == 0)
        {
            ClubManagement.instance.loadingPanel.SetActive(true);
            registerInfo.tournament_id = tournament_ID;
            registerInfo.user_image = ApiHitScript.instance.updatedUserImageUrl;
            print("ID: " + registerInfo.tournament_id);
            string body = JsonUtility.ToJson(registerInfo);
            print(body);
            Communication.instance.PostData(registerForTournamentUrl, body, RegisterCallback);
        }

        else if (GameSerializeClassesCollection.instance.tournament.tournament_status == 1)
        {
            registerInfo.tournament_id = tournament_ID;
            registerInfo.user_image = ApiHitScript.instance.updatedUserImageUrl;
            print("ID: " + registerInfo.tournament_id);
            string body = JsonUtility.ToJson(registerInfo);
            Communication.instance.PostData(registerForTournamentUrl, body, RegisterCallback);

            

        }
        else
        {
            TurnOnOffBottomPanelBtns(0, 0, true);
        }
    }

    public void AlreadyRegistered()
    {
        Cashier.instance.toastMsg.text = "You have already Registered this Game.";
        Cashier.instance.toastMsgPanel.SetActive(true);
    }
    #endregion

    #region Unregister from Tournament

    [Serializable]
    public class UnregisterInfo
    {
        public string tournament_id;
    }

    [SerializeField] UnregisterInfo unregisterInfo;

    public class UnregisterResponseInfo
    {
        public bool error;
        public bool is_registerd;
        public string errors;
        public int statusCode;
    }

    [SerializeField] UnregisterResponseInfo unregisterResponse;

    public void Unregister()
    {
        ClubManagement.instance.loadingPanel.SetActive(true);
        unregisterInfo.tournament_id = GameSerializeClassesCollection.instance.tournament.tournament_id;
        print("ID: " + unregisterInfo.tournament_id);
        string body = JsonUtility.ToJson(unregisterInfo);
        print(body);
        Communication.instance.PostData(unregisterFromTournamentUrl, body, UnregisterCallback);
    }

    public void UnregisterCallback(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        print(response);
        if (!string.IsNullOrEmpty(response))
        {
            unregisterResponse = JsonUtility.FromJson<UnregisterResponseInfo>(response);

            if (!unregisterResponse.error)
            {
                print("Successfully Unregistered");
                RegisteredTournamentRequest();
                isRegistered = unregisterResponse.is_registerd;
                isRegistered = false;
                TournamentManagerScript.instance.IsRegisteredEmitter();
                SocialProfile._instance.SocialChips();
                bottomPanel.transform.GetChild(0).gameObject.SetActive(true);
                bottomPanel.transform.GetChild(1).gameObject.SetActive(false);
                bottomPanel.transform.GetChild(2).gameObject.SetActive(false);
                bottomPanel.transform.GetChild(3).gameObject.SetActive(false);
            }
            else
            {
                print("Some error in Unregistering");
                if (unregisterResponse.statusCode == 403)
                {
                    Cashier.instance.toastMsg.text = LanguageManager.Instance.GetTextValue("session expired");// login_error;
                    Cashier.instance.toastMsgPanel.SetActive(true);
                    Uimanager.instance.SignOut();
                }
                else
                {
                    Cashier.instance.toastMsg.text = unregisterResponse.errors;
                    Cashier.instance.toastMsgPanel.SetActive(true);
                }
            }
        }
    }

    public void UnregisterOnGameDetailPage(GameObject obj)
    {
        ClubManagement.instance.loadingPanel.SetActive(true);
        unregisterInfo.tournament_id = obj.transform.parent.GetChild(8).GetComponent<Text>().text; ;
        print("ID: " + unregisterInfo.tournament_id);
        string body = JsonUtility.ToJson(unregisterInfo);
        print(body);
        Communication.instance.PostData(unregisterFromTournamentUrl, body, UnregisterCallback);
    }

    #endregion

    #region Game Details

    public void Enter()
    {
        GameManagerScript.instance.InitlializeOnStart();
        UIManagerScript.instance.loadingPanel.SetActive(true);
        tableCount = 1;
        GameManagerScript.instance.NonVideoTable.GetComponent<Canvas>().enabled = true;
        SocialGame.instance.pokerUICanvas.SetActive(false);
        if (isTournamentVideo)
        {
            Screen.orientation = ScreenOrientation.Landscape;
        }
        else
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
        if (TournamentManagerScript.instance.isLateRegistration)
        {
            print("LateRegistrationEnterEmitter");
            TournamentManagerScript.instance.LateRegistrationEnterEmitter();
        }
        else
        {
            print("MttRejoin");
            TournamentManagerScript.instance.isLateRegistration = false;
            StartCoroutine(TournamentManagerScript.instance.MttRejoin());
        }
    }

    public void ClickOnDetails()
    {
        GameDetailNavigationTabs(gameDetailTabNavigationList[0]);
        GameDetailTabs(detailPage);
    }

    public void ClickOnEntries()
    {
        GameDetailNavigationTabs(gameDetailTabNavigationList[1]);
        GameDetailTabs(entriesPage);
        if (GameSerializeClassesCollection.instance.tournament.tournament_status == 0 || GameSerializeClassesCollection.instance.tournament.tournament_status == 3)
        {
            entriesPage.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            entriesPage.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            ClickOnTournamentEntries();
        }
        else
        {
            entriesPage.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            entriesPage.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            TournamentManagerScript.instance.RankingListEmitter();
        }
    }

    public void ClickOnRanking()
    {
        GameDetailNavigationTabs(gameDetailTabNavigationList[2]);
        GameDetailTabs(tablesPage);
        TournamentManagerScript.instance.TableListEmitter();
    }

    [Serializable]
    public class TournamentReward
    {
        public string tournament_id;

    }
    [SerializeField]
    TournamentReward tournamentReward;

    [Serializable]
    public class TournamentRewardResponse
    {
        public bool error;
        public RewardData[] data;
    }
    [SerializeField] TournamentRewardResponse tournamentRewardResponse;

    [Serializable]
    public class RewardData
    {
        public Winner[] winner;
    }

    [Serializable]
    public class Winner
    {
        public string username;
        public string chips;
    }

    public void ClickOnRewards()
    {
        GameDetailNavigationTabs(gameDetailTabNavigationList[3]);
        GameDetailTabs(rewardsPage);

        print("tournament_status:" + GameSerializeClassesCollection.instance.tournament.tournament_status);

        if (GameSerializeClassesCollection.instance.tournament.tournament_status == 3) //..when tournament is over....//
        {
            tournamentReward.tournament_id = tournament_ID;
            string body = JsonUtility.ToJson(tournamentReward);
            print(body);

            ClubManagement.instance.loadingPanel.SetActive(true);
            Communication.instance.PostData(tournamentRewardsUrl, body, RewardCallback);
        }
        else
        {
            TournamentManagerScript.instance.RewardListEmitter();
        }
    }

    void RewardCallback(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        print(response);
        if (!string.IsNullOrEmpty(response))
        {
            tournamentRewardResponse = JsonUtility.FromJson<TournamentRewardResponse>(response);

            if (!tournamentRewardResponse.error)
            {
                RewardsListingAfterTournamentCompletion();
            }
        }
    }

    public void RewardsListingAfterTournamentCompletion()
    {
        print("Reward Listing");
        PlayerName.gameObject.SetActive(true);
        if (tournamentRewardResponse.data[0].winner.Length != rewardCount)
        {
            for (int i = rewardCount; i < tournamentRewardResponse.data[0].winner.Length; i++)
            {
                //if (int.Parse(tournamentRewardResponse.data[0].winner[i].chips) > 0)
                //{
                    rewardCount++;
                    GenerateRewardsMembersItem1();
                //}
            }
        }

        for (int i = 0; i < tournamentRewardResponse.data[0].winner.Length; i++)
        {
            //if (int.Parse(tournamentRewardResponse.data[0].winner[i].chips) > 0)
            //{
            //print(tournamentRewardResponse.data[0].winner[i].username);
            rewardListingContent.GetChild(i).GetChild(0).GetComponent<Text>().text = (i + 1) + "         ";
            rewardListingContent.GetChild(i).GetChild(1).GetComponent<Text>().text = tournamentRewardResponse.data[0].winner[i].chips;
            rewardListingContent.GetChild(i).GetChild(2).GetComponent<Text>().text = tournamentRewardResponse.data[0].winner[i].username;
            rewardListingContent.GetChild(i).GetChild(2).gameObject.SetActive(true);
            rewardListingContent.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
            if (i == 1)
            {
                rewardListingContent.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().sprite = silverTrophy;
                rewardListingContent.GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(true);
            }
            if (i == 2)
            {
                rewardListingContent.GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().sprite = bronzeTrophy;
                rewardListingContent.GetChild(2).GetChild(0).GetChild(0).gameObject.SetActive(true);
            }
            else if (i > 1)
            {
                rewardListingContent.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(false);
            }
                rewardListingContent.GetChild(i).gameObject.SetActive(true);
            //}
        }
    }

    public void GenerateRewardsMembersItem1()
    {
        print("Generate");
        scrollrewardItemObj = Instantiate(rewardListingPanel.gameObject);
        scrollrewardItemObj.transform.SetParent(rewardListingContent, false);
        rewardList.Add(scrollrewardItemObj);
    }

    public void ResetRewardsList1()
    {
        if (rewardList.Count > 0)
        {
            for (int i = 0; i < rewardList.Count; i++)
            {
                Destroy(rewardList[i]);
            }
            rewardList.Clear();
            rewardCount = 1;
        }
        for (int i = 0; i < rewardListingContent.transform.childCount; i++)
        {
            rewardListingContent.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void BackFromGameDetails()
    {
        TournamentManagerScript.instance.timesCalled = 0;
        SocialProfile._instance.SocialChips();
        TournamentRequest();
        RegisteredTournamentRequest();
        ResetAllValuesForImage();
        ResetRewardsList();
        ResetRewardsList1();

        ResetEntriesList();
        if (gameDetailTimerCorotine != null)
        {
            StopCoroutine(gameDetailTimerCorotine);
        }
        ClubManagement.instance.isBottomPanelClose = false;
        if (TournamentManagerScript.instance.timeSubscribeEvent == 1)
        {
            TournamentManagerScript.instance.UnSubscribeToServerEvents();
        }

        GameManagerScript.instance.SocketReset();
        GameManagerScript.instance.isTournament = false;
        GameManagerScript.instance.Tournamentsocket.gameObject.SetActive(false);
        GameManagerScript.instance.tournamentManager.SetActive(false);

        GameSerializeClassesCollection.instance.tournament.tournament_status = 0;
        GameSerializeClassesCollection.instance.tournament = null;
        //GameSerializeClassesCollection.Tournament newTour = new GameSerializeClassesCollection.Tournament();
        //GameSerializeClassesCollection.instance.tournament = newTour;

        GameSerializeClassesCollection.instance.tournament = new GameSerializeClassesCollection.Tournament();


    }

    public void GameDetailNavigationTabs(GameObject obj)
    {
        for (int i = 0; i < gameDetailTabNavigationList.Count; i++)
        {
            gameDetailTabNavigationList[i].transform.GetChild(0).gameObject.SetActive(false);
            gameDetailTabNavigationList[i].transform.GetChild(1).gameObject.SetActive(true);
            gameDetailTabNavigationList[i].transform.GetChild(2).gameObject.SetActive(false);
        }
        obj.transform.GetChild(0).gameObject.SetActive(true);
        obj.transform.GetChild(1).gameObject.SetActive(false);
        obj.transform.GetChild(2).gameObject.SetActive(true);
    }

    public void GameDetailTabs(GameObject obj)
    {
        for (int i = 0; i < gameDetailTabList.Count; i++)
        {
            gameDetailTabList[i].SetActive(false);
        }
        obj.SetActive(true);
    }

    #endregion

    #region Update Values of GameDetails

    public Text ticket;
    public void Observer()
    {
        print("TNO. :" + GameSerializeClassesCollection.instance.tournament.ticket);
        if (GameSerializeClassesCollection.instance.tournament.tournament_status == 1)
        {
            tableCount = 1;
            observingTableNumber = GameSerializeClassesCollection.instance.tournament.ticket;

            if (istableListReceived)
            {
                ticket.text = GameSerializeClassesCollection.instance.mttTableListingData.data[0].ticket;
            }
            else
            {
                ticket.text = GameSerializeClassesCollection.instance.tournament.ticket;
            }
            print("ticket....." + ticket.text);
            GameManagerScript.instance.isObserver = true;
            TournamentManagerScript.instance.MttObserver(ticket);

        }
        else
        {
            print("Error in observer........");
        }
    }

    //.........................//
    public void UpdateGameDetailPage()
    {
        detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.tournament.table_name;
        detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.tournament.tournament_id;

        if (GameSerializeClassesCollection.instance.tournament.blinds_up < 10)
        {
            detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(3).GetChild(1).GetComponent<Text>().text = "0" + GameSerializeClassesCollection.instance.tournament.blinds_up.ToString() + ":00";
        }
        else if (GameSerializeClassesCollection.instance.tournament.blinds_up >= 10)
        {
            detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(3).GetChild(1).GetComponent<Text>().text = GameSerializeClassesCollection.instance.tournament.blinds_up.ToString() + ":00";
        }

        detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(4).GetChild(1).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("Level") + " " + GameSerializeClassesCollection.instance.tournament.late_registration.ToString();

        if (GameSerializeClassesCollection.instance.tournament.tournament_status == 0)
        {
            detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(5).GetChild(1).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("Registering"); //"Registering";
        }
        else
        {
            detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(5).GetChild(1).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("Level") + " " + GameSerializeClassesCollection.instance.tournament.level.ToString();
        }

        //..........................Remaining Players.............................//
        if (GameSerializeClassesCollection.instance.tournament.tournament_status == 1)
        {
            print("Remaining 1");
            detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(6).GetChild(0).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("Remaining");//"Remaining";
            detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(6).GetChild(1).GetComponent<Text>().text = GameSerializeClassesCollection.instance.tournament.remaining_player + "/" + GameSerializeClassesCollection.instance.tournament.entries.ToString();
        }
        else if (GameSerializeClassesCollection.instance.tournament.tournament_status == 3)
        {
            print("Remaining 2");
            detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(6).GetChild(0).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("Remaining");//"Remaining";
            detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(6).GetChild(1).GetComponent<Text>().text = "1/" + GameSerializeClassesCollection.instance.tournament.entries.ToString();
        }
        else
        {
            print("Remaining 3");
            detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(6).GetChild(0).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("Registered"); //"Registered";
            detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(6).GetChild(1).GetComponent<Text>().text = GameSerializeClassesCollection.instance.tournament.remaining_player + "/" + GameSerializeClassesCollection.instance.tournament.max_player_number.ToString();
        }
        //...............................................................................//

        detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(7).GetChild(1).GetComponent<Text>().text = GameSerializeClassesCollection.instance.tournament.new_avg_satck;

        if (GameSerializeClassesCollection.instance.tournament.next_game_break < 10)
        {
            detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(8).GetChild(1).GetComponent<Text>().text = "0" + GameSerializeClassesCollection.instance.tournament.next_game_break.ToString() + ":00";
        }
        else if (GameSerializeClassesCollection.instance.tournament.next_game_break >= 10)
        {
            detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(8).GetChild(1).GetComponent<Text>().text = GameSerializeClassesCollection.instance.tournament.next_game_break.ToString() + ":00";
        }
        detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(9).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.tournament.game_type;
       
        //..........................//
        if(isTournamentVideo)
        {
            detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(9).GetChild(1).gameObject.SetActive(true);
            detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(9).GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(9).GetChild(1).gameObject.SetActive(false);
            detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(9).GetChild(2).gameObject.SetActive(true);
        }
        //...........................//

        detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(4).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.tournament.table_size.ToString();
        detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(5).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.tournament.buyIn + " " + "(" + (GameSerializeClassesCollection.instance.tournament.buyIn - ((GameSerializeClassesCollection.instance.tournament.buyIn * GameSerializeClassesCollection.instance.tournament.fee) / 100)) +"+" + ((GameSerializeClassesCollection.instance.tournament.buyIn * GameSerializeClassesCollection.instance.tournament.fee) / 100) +")";
        detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(6).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.tournament.new_prize;
        detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(7).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.tournament.bounty.ToString("F1");
        detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(8).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.tournament.entries.ToString();
        detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(9).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.tournament.min_player_number.ToString() + "-" + GameSerializeClassesCollection.instance.tournament.max_player_number.ToString();
        detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(10).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.tournament.rebuy.ToString();
        detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(11).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.tournament.addon.ToString("F2");
        detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(12).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.tournament.new_starting_chips;
        //detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(13).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.tournament.blind_structure.Substring(0, 1).ToUpper() + GameSerializeClassesCollection.instance.tournament.blind_structure.Substring(1);
        if (GameSerializeClassesCollection.instance.tournament.blind_structure == "standard")
        {
            detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(13).GetChild(0).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("Standard");
        }
        else if (GameSerializeClassesCollection.instance.tournament.blind_structure == "turbo")
        {
            detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(13).GetChild(0).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("Turbo");
        }

        detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.tournament.tournament_description;

        print("isRegistered: " + isRegistered);
        print("tournament_status: " + GameSerializeClassesCollection.instance.tournament.tournament_status);
        if (!isRegistered)
        {
            if (GameSerializeClassesCollection.instance.tournament.tournament_status == 0 && !isRegistered) //...when tournament is not started ....//
            {
                print("Tournament Not Started and User is not Registered");
                TurnOnOffBottomPanelBtns(0, -1, false);
            }

            if (GameSerializeClassesCollection.instance.tournament.tournament_status == 1 && !isRegistered)
            {
                timerText.text = "00:00:00";
                if (GameSerializeClassesCollection.instance.tournament.late_registration >= GameSerializeClassesCollection.instance.tournament.level)
                {
                    print("ObserveAndRegister");
                    TurnOnOffBottomPanelBtns(4, 5, false);
                }
                else
                {
                    print("Observe");
                    TurnOnOffBottomPanelBtns(7, -1, false);
                }
            }
        }
        else if ((GameSerializeClassesCollection.instance.tournament.tournament_status == 9 || GameSerializeClassesCollection.instance.tournament.tournament_status == 0) && isRegistered)
        {
            TurnOnOffBottomPanelBtns(2, -1, false);
        }

        if (GameSerializeClassesCollection.instance.tournament.tournament_status == 3) //...when tournament is over....//
        {
            timerText.text = "00:00:00";
            detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(7).GetChild(1).GetComponent<Text>().text = "0";
            TurnOnOffBottomPanelBtns(0, 0, true);
        }
        //if (GameSerializeClassesCollection.instance.tournament.tournament_status == 9)
        //{
        //    TurnOnOffBottomPanelBtns(0, 0, true);
        //}

        detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(1).GetComponent<Text>().text = startTime.Substring(0, 10);
        detailPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(2).GetComponent<Text>().text = startTime.Substring(11, 8);
    }
    //.......................//

    public Text timerText;
    Coroutine gameDetailTimerCorotine;
    public void Timer()
    {
        print("TIMER Started");
        print("Tour StartTime: " + GameSerializeClassesCollection.instance.tournament.start_time);
        if (!string.IsNullOrEmpty(GameSerializeClassesCollection.instance.tournament.start_time))
        {
            string date = GameSerializeClassesCollection.instance.tournament.start_time.Substring(3, 2) + "/" + GameSerializeClassesCollection.instance.tournament.start_time.Substring(0, 2) + "/" + GameSerializeClassesCollection.instance.tournament.start_time.Substring(6, 4) + " " + GameSerializeClassesCollection.instance.tournament.start_time.Substring(11);
            print("DATETIME: " + date);
            double remainingSec = ClubManagement.instance.TimeDifference(date);
            gameDetailTimerCorotine = StartCoroutine(ClubManagement.instance.RemainingTimerValue(remainingSec, timerText));
            //StartCoroutine(ClubManagement.instance.RemainingTimerValue(remainingSec, timerText));
        }
    }

    public void CheckRegisterStatus()
    {
        print("CheckRegisterStatus.........");
        if (!isRegistered)
        {
            TurnOnOffBottomPanelBtns(0, 0, true);
        }
        //StartCoroutine(WaitToShowRejister());
    }

    public void CheckRegisterStatusAfterStart()
    {
        print("CheckRegisterStatus.........");
        if (!isRegistered)
        {
            if (GameSerializeClassesCollection.instance.tournament.late_registration >= GameSerializeClassesCollection.instance.tournament.level)
            {
                print("ObserveAndRegister");
                TurnOnOffBottomPanelBtns(4, 5, false);
            }
            else
            {
                print("Observe");
                TurnOnOffBottomPanelBtns(7, -1, false);
            }
            TournamentManagerScript.instance.MttEntyEmitter(false);
        }
        //StartCoroutine(WaitToShowRejister());
    }

    #endregion

    #region Tournament Entry

    int memberConut;
    public Transform entriesPanel;
    public Transform entriesContent;
    public Text entriesCountText;

    GameObject entriesScrollItemObj;
    public List<GameObject> entryMemList;

    [Serializable]
    public class EntryResponse
    {
        public bool error;
        public Data[] data;
        public int statusCode;
    }

    [Serializable]
    public class Data
    {
        public string username;
        public string client_id;
        public string user_image;
    }

    [SerializeField] EntryResponse entryResponse;

    [Serializable]
    public class EntriesData
    {
        public string tournament_id;
    }

    [SerializeField] EntriesData entriesData;

    public void ClickOnTournamentEntries()
    {
        entriesData.tournament_id = tournament_ID;
        string body = JsonUtility.ToJson(entriesData);
        print("body : " + body);
        ClubManagement.instance.loadingPanel.SetActive(true);
        Communication.instance.PostData(tournamentEntriesUrl, body, TournamentEntryProcess);
    }

    void TournamentEntryProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);

        if (string.IsNullOrEmpty(response))
        {
            print("ASHISH BHARDWAJ" + "error");
        }
        else
        {
            print("MTT Entry Response" + response);
            entryResponse = JsonUtility.FromJson<EntryResponse>(response);

            if (!entryResponse.error)
            {
                if (entryResponse.data.Length > 0)
                {
                    entriesCountText.text = entryResponse.data.Length.ToString();
                    if (entryResponse.data.Length != memberConut)
                    {
                        for (int i = memberConut; i < entryResponse.data.Length; i++)
                        {
                            memberConut++;
                            GenerateEntryMembersItem();
                        }
                    }

                    userImage.Clear();
                    for (int i = 0; i < entriesContent.childCount; i++)
                    {
                        entriesContent.GetChild(i).gameObject.SetActive(false);
                    }

                    for (int i = 0; i < entryResponse.data.Length; i++)
                    {
                        entriesContent.GetChild(i).GetChild(1).GetComponent<Text>().text = entryResponse.data[i].username;
                        entriesContent.GetChild(i).GetChild(2).GetChild(0).GetComponent<Text>().text = entryResponse.data[i].client_id;

                        userImage.Add(entryResponse.data[i].user_image);
                        entriesContent.GetChild(i).gameObject.SetActive(true);
                    }

                    UpdatePlayerImage();
                }
                else
                {
                    for (int i = 0; i < entriesContent.childCount; i++)
                    {
                        entriesContent.GetChild(i).gameObject.SetActive(false);
                    }
                    entriesCountText.text = "0";
                }
            }
            else
            {
                if (entryResponse.statusCode == 403)
                {
                    Cashier.instance.toastMsg.text = LanguageManager.Instance.GetTextValue("session expired");
                                                                                                              
                    Cashier.instance.toastMsgPanel.SetActive(true);
                    Uimanager.instance.SignOut();
                }   
            }

        }

    }

    #region update Player Image

    public List<string> userImage;

    [SerializeField]
    private List<PlayerImageInSequence> playerImageInSequence;

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
                ClubManagement.instance.loadingPanel.SetActive(true);

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
            ClubManagement.instance.loadingPanel.SetActive(false);

            for (int i = 0; i < userImage.Count; i++)
            {
                entriesContent.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
            }

            for (int i = 0; i < userImage.Count; i++)
            {
                if (!string.IsNullOrEmpty(userImage[i]))
                {
                    //print("i = " + i);
                    entriesContent.GetChild(i).GetChild(0).GetComponent<Image>().sprite = playerImageInSequence[count].imgPic;
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
                instance.totalImageCount++;
                instance.ApplyImage();

            }
        }
    }

    public void ResetAllValuesForImage()
    {
        totalImageCount = 0;
        k = 0;
        count = 0;
        previousCountForMemberList = 0;
        userImage.Clear();
        playerImageInSequence.Clear();
    }

    #endregion


    void GenerateEntryMembersItem()
    {
        scrollItemObj = Instantiate(entriesPanel.gameObject);
        scrollItemObj.transform.SetParent(entriesContent, false);
        entryMemList.Add(scrollItemObj);
    }

    public void ResetEntriesList()
    {
        if (entryMemList.Count > 0)
        {
            for (int i = 0; i < entryMemList.Count; i++)
            {
                Destroy(entryMemList[i]);
            }
            entryMemList.Clear();
            memberConut = 1;
            ResetAllValuesForImage();
        }
    }
    #endregion

    #region Tournament Prizes Listing

    public int rewardCount;
    GameObject scrollrewardItemObj;
    public Transform rewardListingPanel;
    public Transform rewardListingContent;
    public List<GameObject> rewardList;
    Transform prizePanel;
    Transform prizeContent;

    public void RewardsListing()
    {
        if (tournamentGameDetailPanel.activeInHierarchy)
        {
            print("if..RewardsListing");
            prizePanel = rewardListingPanel;
            prizeContent = rewardListingContent;
        }
        else
        {
            print("else..RewardsListing");
            prizePanel = UIManagerScript.instance.prizePanel.transform;
            prizeContent = UIManagerScript.instance.prizeContent.transform;
        }
        print("rewardCount: " + rewardCount);
        print("RewardLength: " + GameSerializeClassesCollection.instance.mttRewardListingData.data.Length);
        PlayerName.gameObject.SetActive(false);
        if (GameSerializeClassesCollection.instance.mttRewardListingData.data.Length != rewardCount)
        {
            print("rewardCount: " + rewardCount);
            for (int i = rewardCount; i < GameSerializeClassesCollection.instance.mttRewardListingData.data.Length; i++)
            {
                rewardCount++;
                GenerateRewardsMembersItem();
            }
        }

        for (int i = 0; i < GameSerializeClassesCollection.instance.mttRewardListingData.data.Length; i++)
        {
            print("Length: " + GameSerializeClassesCollection.instance.mttRewardListingData.data.Length);
            print("payout: " + GameSerializeClassesCollection.instance.mttRewardListingData.data[i].payout);
            prizeContent.GetChild(i).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttRewardListingData.data[i].rank.ToString()/* + "         "*/;
            prizeContent.GetChild(i).GetChild(1).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttRewardListingData.data[i].payout;

            if (tournamentGameDetailPanel.activeInHierarchy)
            {
                if (i == 1)
                {
                    rewardListingContent.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().sprite = silverTrophy;
                    rewardListingContent.GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(true);
                }
                if (i == 2)
                {
                    rewardListingContent.GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().sprite = bronzeTrophy;
                    rewardListingContent.GetChild(2).GetChild(0).GetChild(0).gameObject.SetActive(true);
                }
            }

            prizeContent.GetChild(i).gameObject.SetActive(true);


        }
    }

    public void GenerateRewardsMembersItem()
    {
        print("GenerateRewardsMembersItem");
        scrollrewardItemObj = Instantiate(prizePanel.gameObject);
        scrollrewardItemObj.transform.SetParent(prizeContent, false);
        rewardList.Add(scrollrewardItemObj);
    }

    public void ResetRewardsList()
    {
        prizeContent = rewardListingContent;
        if (rewardList.Count > 0)
        {
            for (int i = 0; i < rewardList.Count; i++)
            {
                Destroy(rewardList[i]);
            }
            rewardList.Clear();
            rewardCount = 1;
        }
        for (int i = 0; i < prizeContent.transform.childCount; i++)
        {
            prizeContent.GetChild(i).gameObject.SetActive(false);
        }
    }
    #endregion

    #region Tournament Ranking Listing

    int rankCount;
    GameObject scrollRankItemObj;
    public Transform RankListingPanel;
    public Transform RankListingContent;
    public List<GameObject> RankList;
    Transform rankingPanel;
    Transform rankingContent;
    string localPlayerInList;
    int playerPosition;

    public void RankingListing()
    {
        if (tournamentGameDetailPanel.activeInHierarchy)
        {
            print("if..RankingListing");
            rankingPanel = RankListingPanel;
            rankingContent = RankListingContent;
        }
        else
        {
            print("else..RankingListing");
            for (int i = 0; i < UIManagerScript.instance.infoPanelTabList.Count; i++)
            {
                print("OFF");
                UIManagerScript.instance.infoPanelTabList[i].SetActive(false);
            }
            UIManagerScript.instance.infoPanelTabList[0].SetActive(true);

            for (int i = 0; i < UIManagerScript.instance.infoPanelNavigationTabList.Count; i++)
            {
                UIManagerScript.instance.infoPanelNavigationTabList[i].transform.GetChild(0).gameObject.SetActive(false);
                UIManagerScript.instance.infoPanelNavigationTabList[i].transform.GetChild(1).gameObject.SetActive(true);
                UIManagerScript.instance.infoPanelNavigationTabList[i].transform.GetChild(2).gameObject.SetActive(false);
            }
            UIManagerScript.instance.infoPanelNavigationTabList[0].transform.GetChild(0).gameObject.SetActive(true);
            UIManagerScript.instance.infoPanelNavigationTabList[0].transform.GetChild(1).gameObject.SetActive(false);
            UIManagerScript.instance.infoPanelNavigationTabList[0].transform.GetChild(2).gameObject.SetActive(true);

            rankingPanel = UIManagerScript.instance.rankingPanel.transform;
            rankingContent = UIManagerScript.instance.rankingContent.transform;
        }

        if (GameSerializeClassesCollection.instance.mttRankingListingData.data.Length != rankCount)
        {
            for (int i = rankCount; i < GameSerializeClassesCollection.instance.mttRankingListingData.data.Length; i++)
            {
                rankCount++;
                GenerateRankingMembersItem();
            }
        }

        FindLocalPlayer();
        ResetAllValuesForImage1();
        userImage1.Clear();
        for (int i = 0; i < GameSerializeClassesCollection.instance.mttRankingListingData.data.Length; i++)
        {
            rankingContent.GetChild(i).GetChild(0).GetComponent<Text>().text = (i + 1).ToString();

            //string username5= GameSerializeClassesCollection.instance.mttRankingListingData.data[i].username;

            rankingContent.GetChild(i).GetChild(2).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttRankingListingData.data[i].username;
            rankingContent.GetChild(i).GetChild(3).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttRankingListingData.data[i].new_chips;
            rankingContent.GetChild(i).GetChild(4).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttRankingListingData.data[i].rebuy + "R" + "+" + GameSerializeClassesCollection.instance.mttRankingListingData.data[i].addOn + "A";
            if (tournamentGameDetailPanel.activeInHierarchy)
            {
                userImage1.Add(GameSerializeClassesCollection.instance.mttRankingListingData.data[i].userimage);
            }
            rankingContent.GetChild(i).gameObject.SetActive(true);

            if (GameSerializeClassesCollection.instance.mttRankingListingData.data[i].username == throwableSource)
            {
                localPlayerInList = GameSerializeClassesCollection.instance.mttRankingListingData.data[i].username;
                playerPosition = (i + 1);
                UpdatePlayerPosition();
            }
        }

        if (tournamentGameDetailPanel.activeInHierarchy)
        {
            UpdatePlayerImage1();
        }
    }

    public void GenerateRankingMembersItem()
    {
        scrollRankItemObj = Instantiate(rankingPanel.gameObject);
        scrollRankItemObj.transform.SetParent(rankingContent, false);

        RankList.Add(scrollRankItemObj);
    }

    public void ResetRankList()
    {
        rankingContent = RankListingContent;
        if (RankList.Count > 0)
        {
            for (int i = 0; i < RankList.Count; i++)
            {
                Destroy(RankList[i]);
            }
            RankList.Clear();
            rankCount = 1;
        }
        for (int i = 0; i < rankingContent.transform.childCount; i++)
        {
            rankingContent.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void UpdatePlayerPosition()
    {
        print("UpdatePlayerPosition");
        for (int i = 0; i < GameManagerScript.instance.playersParent.transform.childCount; i++)
        {
            if (GameManagerScript.instance.playersParent.transform.GetChild(i).childCount == 2)
            {
                if (GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetComponent<PokerPlayerController>().isLocalPlayer)
                {
                    print(GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetComponent<PokerPlayerController>().player.playerName);
                    print("POSition: " + playerPosition);
                    if (GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetComponent<PokerPlayerController>().player.playerName == localPlayerInList)
                    {
                        UIManagerScript.instance.position.text = playerPosition.ToString();
                    }
                }
            }
        }
    }

    #region update Player Image

    public List<string> userImage1;

    [SerializeField]
    private List<PlayerImageInSequence1> playerImageInSequence1;

    private int k1 = 0;
    private int totalImageCount1;
    private int count1 = 0;
    private int previousCountForMemberList1;

    void UpdatePlayerImage1()
    {
        if (userImage1.Count == previousCountForMemberList1)
        {
            return;
        }

        k1 = 0;
        previousCountForMemberList1 = 0;
        totalImageCount1 = 0;
        playerImageInSequence1.Clear();

        playerImageInSequence1 = new List<PlayerImageInSequence1>();

        for (int i = 0; i < userImage1.Count; i++)
        {
            if (!string.IsNullOrEmpty(userImage1[i]))
            {
                playerImageInSequence1.Add(new PlayerImageInSequence1());
                ClubManagement.instance.loadingPanel.SetActive(true);

                playerImageInSequence1[k1].imgUrl = userImage1[i];
                playerImageInSequence1[k1].ImageProcess(userImage1[i]);

                k1 = k1 + 1;

            }
            previousCountForMemberList1++;
        }
    }

    public void ApplyImage1()
    {
        print("K Value: " + k1);
        print(totalImageCount1);
        if (k1 == totalImageCount1)
        {
            count1 = 0;
            ClubManagement.instance.loadingPanel.SetActive(false);

            for (int i = 0; i < userImage1.Count; i++)
            {
                RankListingContent.GetChild(i).GetChild(1).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
            }

            for (int i = 0; i < userImage1.Count; i++)
            {
                if (!string.IsNullOrEmpty(userImage1[i]))
                {
                    //print("i = " + i);
                    RankListingContent.GetChild(i).GetChild(1).GetComponent<Image>().sprite = playerImageInSequence1[count1].imgPic;
                    count1++;
                }
            }

        }
    }

    [Serializable]
    public class PlayerImageInSequence1
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
                instance.totalImageCount1++;
                instance.ApplyImage1();

            }
        }
    }

    public void ResetAllValuesForImage1()
    {
        totalImageCount1 = 0;
        k1 = 0;
        count1 = 0;
        previousCountForMemberList1 = 0;
        userImage1.Clear();
        playerImageInSequence1.Clear();
    }

    #endregion

    #endregion

    #region Tournament Blind Details Listing

    int blindDetailsCount;
    GameObject blindDetailsItemObj;
    public Transform blindDetailsPanel;
    public Transform blindDetailsContent;
    public List<GameObject> blindDetailsList;
    string sb;
    string bb;
    Transform blindPanel;
    Transform blindContent;

    public void BlindDetailsListing()
    {
        if (tournamentGameDetailPanel.activeInHierarchy)
        {
            print("if..BlindDetailsListing");
            blindPanel = blindDetailsPanel;
            blindContent = blindDetailsContent;
        }
        else
        {
            print("else..BlindDetailsListing");
            blindPanel = UIManagerScript.instance.blindsPanel.transform;
            blindContent = UIManagerScript.instance.blindsContent.transform;
        }

        blindStructurePage.SetActive(true);
        blindStructurePage.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.tournament.blind_structure.Substring(0, 1).ToUpper() + GameSerializeClassesCollection.instance.tournament.blind_structure.Substring(1);

        if (GameSerializeClassesCollection.instance.blindDetailListener.blind_structure.Length != blindDetailsCount)
        {
            for (int i = blindDetailsCount; i < GameSerializeClassesCollection.instance.blindDetailListener.blind_structure.Length; i++)
            {
                blindDetailsCount++;
                GenerateBlindDetailsItem();
            }
        }

        for (int i = 0; i < GameSerializeClassesCollection.instance.blindDetailListener.blind_structure.Length; i++)
        {
            blindContent.GetChild(i).GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
            sb = GameManagerScript.instance.KiloFormat(GameSerializeClassesCollection.instance.blindDetailListener.blind_structure[i].sb);
            bb = GameManagerScript.instance.KiloFormat(GameSerializeClassesCollection.instance.blindDetailListener.blind_structure[i].bb);
            blindContent.GetChild(i).GetChild(1).GetComponent<Text>().text = sb + "/" + bb;

            blindContent.GetChild(i).gameObject.SetActive(true);
        }
        SelectCurrentBlind();
    }

    public void GenerateBlindDetailsItem()
    {
        blindDetailsItemObj = Instantiate(blindPanel.gameObject);
        blindDetailsItemObj.transform.SetParent(blindContent, false);

        blindDetailsList.Add(blindDetailsItemObj);
    }

    public void ResetBlindDetailsList()
    {
        blindContent = blindDetailsContent;
        if (blindDetailsList.Count > 0)
        {
            for (int i = 0; i < blindDetailsList.Count; i++)
            {
                Destroy(blindDetailsList[i]);
            }
            blindDetailsList.Clear();
            blindDetailsCount = 1;
        }
        for (int i = 0; i < blindContent.transform.childCount; i++)
        {
            blindContent.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void BackFromBlindDetailsPage()
    {
        blindStructurePage.SetActive(false);
        ResetBlindDetailsList();
    }

    public void SelectCurrentBlind()
    {
        if (!tournamentGameDetailPanel.activeInHierarchy)
        {
            print("SelectCurrentBlind()");
            for (int i = 0; i < GameSerializeClassesCollection.instance.blindDetailListener.blind_structure.Length; i++)
            {
                blindContent.GetChild(i).GetChild(3).gameObject.SetActive(false);
                
            }

            for (int i = 0; i < GameSerializeClassesCollection.instance.blindDetailListener.blind_structure.Length; i++)
            {
                if (GameSerializeClassesCollection.instance.tournament.current_level.sb == GameSerializeClassesCollection.instance.blindDetailListener.blind_structure[i].sb)
                {
                    print("1111111111111111111111111111111111111111111111111" + GameSerializeClassesCollection.instance.tournament.current_level.sb);
                    //print("1111111111111111111111111111111111111111111111111" + GameSerializeClassesCollection.instance.tournament.current_level.sb);
                    blindContent.GetChild(i).GetChild(3).gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

    #endregion

    #region Tournament Table Listing

    public int tableCount;
    public GameObject scrollTableItemObj;
    public Transform tableListingPanel;
    public Transform tableListingContent;
    public List<GameObject> tableList;
    string minChips;
    string maxChips;
    public Transform tablePanel;
    public Transform tableContent;

    public void TableListing()
    {
        StartCoroutine(WaitTry());
    }

    IEnumerator WaitTry()
    {
        if (tournamentGameDetailPanel.activeInHierarchy)
        {
            print("if..TableListing");
            tablePanel = tableListingPanel;
            tableContent = tableListingContent;
        }
        else
        {
            print("else..TableListing");
            tablePanel = UIManagerScript.instance.tablePanel.transform;
            tableContent = UIManagerScript.instance.tableContent.transform;
            tablesPage = UIManagerScript.instance.tablePage;
        }
        print("TableList: Ashish");
        print("mttTableListingData.data.Length: " + GameSerializeClassesCollection.instance.mttTableListingData.data.Length);
        print("tableCount.." + tableCount);
        //yield return new WaitForSeconds(2f);

        ResetTableList();

        //yield return new WaitForSeconds(2f);
        tableContent.GetChild(0).gameObject.SetActive(false);
        print("tableCount2.." + tableCount);
        if (GameSerializeClassesCollection.instance.mttTableListingData.data.Length != tableCount)
        {
            for (int i = tableCount; i < GameSerializeClassesCollection.instance.mttTableListingData.data.Length; i++)
            {
                tableCount++;
                GenerateTableMembersItem();
            }
        }
        print("tableCount" + tableCount);

        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < GameSerializeClassesCollection.instance.mttTableListingData.data.Length; i++)
        {
            print("LOOP");
            tableContent.GetChild(i).GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
            tableContent.GetChild(i).GetChild(2).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttTableListingData.data[i].current_players.ToString();
            minChips = GameManagerScript.instance.KiloFormat(GameSerializeClassesCollection.instance.mttTableListingData.data[i].min_chips);
            maxChips = GameManagerScript.instance.KiloFormat(GameSerializeClassesCollection.instance.mttTableListingData.data[i].max_chips);
            tableContent.GetChild(i).GetChild(3).GetChild(0).GetComponent<Text>().text = minChips + "/" + maxChips;
            tableContent.GetChild(i).GetChild(4).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttTableListingData.data[i].ticket;

            if (tournamentGameDetailPanel.activeInHierarchy)
            {
                if (isTournamentVideo)
                {
                    tableContent.GetChild(i).GetChild(6).gameObject.SetActive(true);
                    tableContent.GetChild(i).GetChild(1).gameObject.SetActive(false);
                }
                else
                {
                    tableContent.GetChild(i).GetChild(1).gameObject.SetActive(true);
                    tableContent.GetChild(i).GetChild(6).gameObject.SetActive(false);
                }
            }

            tableContent.GetChild(i).gameObject.SetActive(true);
        }

        //yield return new WaitForSeconds(2f);

        if (tournamentGameDetailPanel.activeInHierarchy)
        {
            tablesPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = tableCount.ToString();     //Update Table Count
        }
        else
        {
            print("ELSE: " + tablesPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).name);
            tablesPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = tableCount.ToString();     //Update Table Count
        }
        print("observingTableNumber: " + observingTableNumber);
        print("isObserver" + GameManagerScript.instance.isObserver);
        if (GameManagerScript.instance.isObserver && UIManagerScript.instance.mttSideInfoPanel != null && UIManagerScript.instance.mttSideInfoPanel.activeInHierarchy)
        {
            HighlightSelectedObservingTable(observingTableNumber);
        }
    }

    public void GenerateTableMembersItem()
    {
        print("GENERATE");
        scrollTableItemObj = Instantiate(tablePanel.gameObject);
        scrollTableItemObj.transform.SetParent(tableContent, false);
        tableList.Add(scrollTableItemObj);
    }

    public void ResetTableList()
    {
        try
        {
            print("tableList..Count." + tableList.Count);
            for (int i = 0; i < tableList.Count; i++)
            {
                tableList[i].SetActive(true);
                Destroy(tableList[i]);
            }
            tableList.Clear();
            tableCount = 1;

            if (tableList.Count > 0)
            {

            }

            //tableContent = tableListingContent;
            //for (int i = 0; i < tableContent.transform.childCount; i++)
            //{
            //    tableContent.GetChild(i).gameObject.SetActive(false);
            //}
        }
        catch
        {
            print("Error in ResetTableList.......");
        }
    }

    public void HighlightSelectedObservingTable(string tableNum)
    {
        for (int i = 0; i < GameSerializeClassesCollection.instance.mttTableListingData.data.Length; i++)
        {
            tableContent.GetChild(i).GetChild(7).gameObject.SetActive(false);
        }

        for (int i = 0; i < GameSerializeClassesCollection.instance.mttTableListingData.data.Length; i++)
        {
            if (GameSerializeClassesCollection.instance.mttTableListingData.data[i].ticket == tableNum)
            {
                tableContent.GetChild(i).GetChild(7).gameObject.SetActive(true);
            }
        }
    }

    #endregion

    #region Update SideInfo Panel in Video and Non-Video Table

    public void UpdateSidePanelInfo()
    {
        print("UpdateSidePanelInfo.....");
        UIManagerScript.instance.tournamentName.text = GameSerializeClassesCollection.instance.tournament.table_name;
        UIManagerScript.instance.tournamentID.text ="ID: "+ GameSerializeClassesCollection.instance.tournament.tournament_id;
        //UIManagerScript.instance.position.text = GameSerializeClassesCollection.instance.tournament.position.ToString();
        UIManagerScript.instance.level.text = LanguageManager.Instance.GetTextValue("Level") + " " + GameSerializeClassesCollection.instance.tournament.level.ToString();
        UIManagerScript.instance.remaining.text = GameSerializeClassesCollection.instance.tournament.remaining_player + "/" + GameSerializeClassesCollection.instance.tournament.entries;
        UIManagerScript.instance.lateRegistration.text = GameSerializeClassesCollection.instance.tournament.late_registration.ToString();
        UIManagerScript.instance.prizePool.text = GameSerializeClassesCollection.instance.tournament.new_prize;
        UIManagerScript.instance.avgStack.text = GameSerializeClassesCollection.instance.tournament.new_avg_satck;
        UIManagerScript.instance.rebuys.text = GameSerializeClassesCollection.instance.tournament.reBuyCount.ToString();
        UIManagerScript.instance.addOns.text = GameSerializeClassesCollection.instance.tournament.addOnCount.ToString();
        UIManagerScript.instance.smallestStack.text = GameSerializeClassesCollection.instance.tournament.smallest_stack.chips.ToString();
        UIManagerScript.instance.largestStack.text = GameSerializeClassesCollection.instance.tournament.largest_stack.chips.ToString();
        UIManagerScript.instance.totalBuyIns.text = GameSerializeClassesCollection.instance.tournament.buyIn.ToString();
        UIManagerScript.instance.currentLevel.text = GameSerializeClassesCollection.instance.tournament.current_level.sb.ToString() + "/" + GameSerializeClassesCollection.instance.tournament.current_level.bb.ToString();
        UIManagerScript.instance.nextLevel.text = GameSerializeClassesCollection.instance.tournament.next_level.sb.ToString() + "/" + GameSerializeClassesCollection.instance.tournament.next_level.bb.ToString(); ;
    }

    public void TournamentInfoPanelNavigationTabs(GameObject obj)
    {
        for (int i = 0; i < UIManagerScript.instance.infoPanelNavigationTabList.Count; i++)
        {
            UIManagerScript.instance.infoPanelNavigationTabList[i].transform.GetChild(0).gameObject.SetActive(false);
            UIManagerScript.instance.infoPanelNavigationTabList[i].transform.GetChild(1).gameObject.SetActive(true);
            UIManagerScript.instance.infoPanelNavigationTabList[i].transform.GetChild(2).gameObject.SetActive(false);
        }
        obj.transform.GetChild(0).gameObject.SetActive(true);
        obj.transform.GetChild(1).gameObject.SetActive(false);
        obj.transform.GetChild(2).gameObject.SetActive(true);
    }

    public void TournamentInfoPanelTabs(GameObject obj)
    {
        for (int i = 0; i < UIManagerScript.instance.infoPanelTabList.Count; i++)
        {
            UIManagerScript.instance.infoPanelTabList[i].SetActive(false);
        }
        obj.SetActive(true);
    }

    public void CloseAllInfoPanels()
    {
        for (int i = 0; i < UIManagerScript.instance.infoPanelTabList.Count; i++)
        {
            UIManagerScript.instance.infoPanelTabList[i].SetActive(false);
        }
        for (int i = 0; i < UIManagerScript.instance.infoPanelNavigationTabList.Count; i++)
        {
            UIManagerScript.instance.infoPanelNavigationTabList[i].transform.GetChild(0).gameObject.SetActive(false);
            UIManagerScript.instance.infoPanelNavigationTabList[i].transform.GetChild(1).gameObject.SetActive(true);
            UIManagerScript.instance.infoPanelNavigationTabList[i].transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    public void ClickObserver()
    {
        StartCoroutine(ClickObserverCo());
    }
    public bool istableListReceived;
    IEnumerator ClickObserverCo()
    {
        float time = 3;
        istableListReceived = false;
        TournamentManagerScript.instance.TableListEmitter();
        UIManagerScript.instance.loadingPanel.SetActive(true);
        while (true)
        {
            if(istableListReceived || time < 0.5f)
            {
                break;
            }
            time -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        if (!istableListReceived)
        {
            UIManagerScript.instance.loadingPanel.SetActive(false);
            UIManagerScript.instance.TableToPokerUI(0);
        }
        else
        {
            yield return new WaitForSeconds(1f);
            RemoveLocalPlayer(TournamentManagerScript.instance.localPlayer);
        }
    }

    public void RemoveLocalPlayer(GameObject localPlayer)
    {
        GameManagerScript.instance.totalPlayersSitting--;
        GameManagerScript.instance.CheckTotalPlayersSitting();
        if (localPlayer != null)
        {
            localPlayer.GetComponent<PokerPlayerController>().PlayerDeactivate(false);
            SpawnManager.instance.Despawn("PlayerPool", localPlayer.transform);
        }
        for (int i = 0; i < UIManagerScript.instance.allOtherBottomPanelBtn.Count; i++)
        {
            UIManagerScript.instance.allOtherBottomPanelBtn[i].SetActive(false);
        }
        UIManagerScript.instance.mttSideInfoPanel.SetActive(false);
        UIManagerScript.instance.ReShuffleTable();
        GameSerializeClassesCollection.instance.tournament.tournament_status = 1;
        TournamentManagerScript.instance.PlayerExistEmitter();
        Observer();
        HighlightSelectedObservingTable(GameSerializeClassesCollection.instance.mttTableListingData.data[0].ticket);
    }

    #endregion

    #region Timer

    public double timerCounter;                             //Should be less than 60sec
    public bool isTopUpPanelOn;
    internal Coroutine runningCoroutine;
    public void StartCountDown()
    {
        //print("StartCountDown");
        //print("timerCounter"+ timerCounter);
        timerCounter = 20;
        if (UIManagerScript.instance.emptypChipsPanel.activeInHierarchy)
        {
            //print("emptypChipsPanel...");
            runningCoroutine = StartCoroutine(RemainingTimerValue(timerCounter, UIManagerScript.instance.timerCountdown));
        }
        else if (UIManagerScript.instance.emptypChipsLessBuyInPanel.activeInHierarchy)
        {
            //print("emptypChipsLessBuyInPanel...");
            runningCoroutine = StartCoroutine(RemainingTimerValue(timerCounter, UIManagerScript.instance.timerCountdown2));
        }
    }
    public static string oldtimer;

    double runningTimeCounter;
    private void OnApplicationPause(bool pause)
    {
        if (isTopUpPanelOn)
        {
            if (!pause)
            {
                //print("come back to app....");
                //......calculate time difference

                if (PlayerPrefs.HasKey(oldtimer))
                {
                    string oldTime = PlayerPrefs.GetString(oldtimer);

                    double timeDiff = TimeDifference(oldTime);
                    //print("timeDiff..." + timeDiff);

                    DateTime now = DateTime.Now;

                    string currentDate = now.ToString("dd/MM/yyyy HH:mm:ss");

                    if (timeDiff > 0)
                    {
                        //.....show timer count down
                        if (runningCoroutine != null)
                        {
                            StopCoroutine(runningCoroutine);
                        }
                        if (UIManagerScript.instance.emptypChipsPanel.activeInHierarchy)
                        {
                            runningCoroutine = StartCoroutine(RemainingTimerValue(timeDiff, UIManagerScript.instance.timerCountdown));
                        }
                        else if (UIManagerScript.instance.emptypChipsLessBuyInPanel.activeInHierarchy)
                        {
                            runningCoroutine = StartCoroutine(RemainingTimerValue(timeDiff, UIManagerScript.instance.timerCountdown2));
                        }
                        //runningCoroutine = StartCoroutine(RemainingTimerValue(timeDiff, timerCountdown));
                    }
                    else
                    {
                        //.....time has ended
                        if (runningCoroutine != null)
                        {
                            StopCoroutine(runningCoroutine);
                        }
                        if (UIManagerScript.instance.emptypChipsPanel.activeInHierarchy)
                        {
                            runningCoroutine = StartCoroutine(RemainingTimerValue(timeDiff, UIManagerScript.instance.timerCountdown));
                        }
                        else if (UIManagerScript.instance.emptypChipsLessBuyInPanel.activeInHierarchy)
                        {
                            runningCoroutine = StartCoroutine(RemainingTimerValue(timeDiff, UIManagerScript.instance.timerCountdown2));
                        }
                        //runningCoroutine = StartCoroutine(RemainingTimerValue(0, timerCountdown));
                    }
                }

            }
            else
            {
                //.........set timer in player prefs

                DateTime now = DateTime.Now;
                now = now.AddSeconds(runningTimeCounter);
                string currentDate = now.ToString("dd/MM/yyyy HH:mm:ss");

                PlayerPrefs.SetString(oldtimer, currentDate);
            }
        }
    }

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
                //print("isTopUpClick: " + PokerNetworkManager.instance.isTopUpClick);
                if (PokerNetworkManager.instance.isTopUpClick)
                {
                    PokerNetworkManager.instance.isTopUpClick = false;
                    break;
                }
                //print(totalTableRemainingTime);
                totalTableRemainingTime -= 1;
                RemainingDisplayTime(totalTableRemainingTime, _text);
                yield return new WaitForSeconds(1);
            }

            else
            {
                Debug.Log("Table has Ended!");
                _text.text = "00";
                if (GameManagerScript.instance.networkManager.activeInHierarchy)
                {
                    PokerNetworkManager.instance.StandUpChipsEmptyEmitter();
                }
                break;
            }
        }
    }

    void RemainingDisplayTime(double _timeToDisplay, Text _text)
    {
        int timeToDisplay = (int)_timeToDisplay;

        timeToDisplay += 1;

        float hours = Mathf.FloorToInt(timeToDisplay / 3600);
        float minutes = Mathf.FloorToInt(timeToDisplay / 60) % 60;
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        if (_text != null)
        {
            _text.text = string.Format("{0:00}", seconds);
            Image img = _text.gameObject.transform.parent.GetChild(0).GetComponent<Image>();
            img.fillAmount = (float)_timeToDisplay / (float)timerCounter;
        }
        runningTimeCounter = seconds;
    }



    #endregion

    public void StopTimer()
    {
        print("StopTimer.....");
        StartCoroutine(StopTimerCoroutine());
    }

    public IEnumerator StopTimerCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        PokerNetworkManager.instance.isTopUpClick = false;
        UIManagerScript.instance.isTopupBtnClicked = false;
        if (runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
        }
    }

    #endregion

    public void ClickNoRebuyAddOn()
    {
        TournamentManagerScript.instance.RevokeCurrPlayer();
    }

    bool isTournamentRunning;
    public void EnterTourFromPushNotify(string tourID, bool isVideo, int tourStatus)
    {
        if (tourStatus == 1)
        {
            isTournamentRunning = true;
        }
        else
        {
            isTournamentRunning = false;
        }
        print("OpenGameDetailPageAfterRegistration");
        CloseAllBottomButtons();

        ClubManagement.instance.loadingPanel.SetActive(true);
        ClickOnDetails();

        GameManagerScript.instance.isTournament = true;
        tournament_ID = tourID;
        isTournamentVideo = isVideo;
        isRegistered = true;
        GameSerializeClassesCollection.instance.tournament.tournament_status = tourStatus;
        GameSerializeClassesCollection.instance.tournament.tournament_id = tourID;
        SocialGame.instance.StartGame();

        StartCoroutine(CheckServerConnection());
    }
}