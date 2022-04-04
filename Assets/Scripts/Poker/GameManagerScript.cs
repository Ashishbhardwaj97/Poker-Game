using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SocketIO;
using UnityEngine.Networking;
using Random = UnityEngine.Random;
using SmartLocalization;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;
    public int localPlayerSeatid;
    public SocketIOComponent socket;
    public SocketIOComponent Tournamentsocket;
    public GameObject networkManager;
    public GameObject tournamentManager;

    public int totalPlayersSitting;
    public int totalPlayersOnNewRound;
    [Space]
    public bool isChanceActive;
    public bool isPlayerExcluded;
    public bool isStandupClicked;
    public bool isTopUp;
    [Space]
    public bool isVideoTable;
    public bool isHandEnd;
    public bool isShuffflingComplete;
    public bool isPlayerGenerating;
    public bool isCheckActive;
    public bool isTournament;
    public bool isObserver;
    [Space]
    public bool alarmsound;
    public bool isSwitching;
    public bool isStandUp;

    [Header("Chair Amination")]
    [Space]
    public bool chairAnimForLocalPlayer;
    public bool chairAnimForBeforeLocalPlayer;
    public bool chairAnimForAfterPlayer;
    public bool isNewTimer;
    [Space]
    public bool isObserverAudio;

    [HideInInspector]
    public bool isStaging;
    [HideInInspector]
    public bool isDev;
    [HideInInspector]
    public bool isSocialPokerDev;
    [HideInInspector]
    public bool isSocialPokerStage;

    [Space]
    [HideInInspector]
    //public string regularTableUrl;
    public string tournamentTableDevUrl = "ws://poker-dev.kelltontech.net:8585/socket.io/?EIO=3&transport=websocket";
    [HideInInspector]
    public string tournamentTableStagingUrl = "ws://poker-stage.kelltontech.net:9595/socket.io/?EIO=3&transport=websocket";
    [Space]
    [HideInInspector]
    public string regularTableDevUrl = "ws://poker-dev.kelltontech.net:8080/socket.io/?EIO=3&transport=websocket";
    [HideInInspector]
    public string regularTableStagingUrl = "ws://poker-stage.kelltontech.net:9090/socket.io/?EIO=3&transport=websocket";
    [Space]
    [HideInInspector]
    //public string socialPokerDevUrl = "ws://poker-dev.kelltontech.net:6060/socket.io/?EIO=3&transport=websocket"; //...For Dev server...//
    public string socialPokerUrl = "ws://23.23.68.112:6060/socket.io/?EIO=3&transport=websocket"; //  It is a common variable...For AWS server... AND For Live Server.....//
    //public string socialPokerDevUrl = "wss://poker-face.kelltontech.net:7070/socket.io/?EIO=3&transport=websocket"; //...For AWS...live server...//
    //public string socialPokerDevUrl = "ws://54.224.48.195:7070/socket.io/?EIO=3&transport=websocket";
    [HideInInspector]
    public string socialPokerStageUrl = "ws://poker-stage.kelltontech.net:7070/socket.io/?EIO=3&transport=websocket";
    [Space]
    [HideInInspector]
    //public string socialPokerTournamentDevUrl = "ws://poker-dev.kelltontech.net:7575/socket.io/?EIO=3&transport=websocket"; //...For Dev server...//
    public string socialPokerTournamentUrl = "ws://23.23.68.112:7575/socket.io/?EIO=3&transport=websocket"; //..........AWS.......//
    //public string socialPokerTournamentDevUrl = "wss://poker-face.kelltontech.net:6565/socket.io/?EIO=3&transport=websocket"; //..........AWS Live.......// 
    //public string socialPokerTournamentDevUrl = "ws://54.224.48.195:6565/socket.io/?EIO=3&transport=websocket";
    [HideInInspector]
    public string socialPokerTournamentStageUrl = "ws://poker-stage.kelltontech.net:6565/socket.io/?EIO=3&transport=websocket";

    public bool isDirectLogIn;
    public GameObject videoTable;
    public GameObject SocialVideoTable;
    public GameObject NonVideoTable;
    public GameObject activeTable;
    public GameObject playersParent;
    public GameObject HandRankPanel;

    [Header("Player Video")]
    [Space]
    [Space]
    public GameObject agoraController;
    public GameObject hostPlayerVideoPanel;
    [Header("Observer Values")]
    [Space]
    [Space]
    public GameObject observerTextPrefab;
    public GameObject observerContent;
    [Header("Command Timer Objects")]
    [Space]
    public int commandTimer;
    public int commandTimer2;
    public Coroutine TimerCouroutine;
    public Transform timerImage;
    [Space]
    public List<GameObject> addPlayers;
    public string playerdata;
    public string playerdata2;
    public int localSeatIDDummy;
    public int localSeatID;
    public int bigBlindAmount;
    public int activePlayerSeatId;

    public int timesAnimLocalPlayer;
    public int timesAnimBeforeLocalPlayer;
    public int timesAnimAfterLocalPlayer;
    public GameObject testingTakeASeatButton;
    [Space]
    public int NumberOfSeats;
    public List<Text> observerNumber;
    [Space]
    public int timesRaise;
    public int minBet;
    public int reRaise;
    public int currentPlayerTableBet;

    public int dealerSeatid;
    public string dealerName;
    public Transform dealer;
    [Space]
    public bool slowInternet;
    public bool noInternet;
    public bool fastInternet;
    public int slowInternetTimes = 0;
    public int noInternetTimes = 0;
    [Space]
    public List<GameObject> start;
    public bool isGameResumed;
    string response = "";
    string checkInternetUrl = "";
    int timerVal = 0;
    bool isResponseDone;
    bool networkStatus;

    private void Awake()
    {
        instance = this;
        isShuffflingComplete = true;
    }

    private void Start()
    {
        alarmsound = false;

        if (ServerChanger.instance.isDev && !ServerChanger.instance.isAWS && !isStaging) //...... dev server......//
        {
            tournamentTableDevUrl = "ws://poker-dev.kelltontech.net:8585/socket.io/?EIO=3&transport=websocket";
            regularTableDevUrl = "ws://poker-dev.kelltontech.net:8080/socket.io/?EIO=3&transport=websocket";
        }

        else if (ServerChanger.instance.isStaging && !ServerChanger.instance.isDev && !ServerChanger.instance.isAWS) //...... stage server......//
        {
            tournamentTableStagingUrl = "ws://poker-stage.kelltontech.net:9595/socket.io/?EIO=3&transport=websocket";
            regularTableStagingUrl = "ws://poker-stage.kelltontech.net:9090/socket.io/?EIO=3&transport=websocket";
        }

        else if (ServerChanger.instance.isAWS && !ServerChanger.instance.isDev && !ServerChanger.instance.isStaging) //...... aws dev server......//
        {
            socialPokerTournamentUrl = "ws://23.23.68.112:7575/socket.io/?EIO=3&transport=websocket";
            socialPokerUrl = "ws://23.23.68.112:6060/socket.io/?EIO=3&transport=websocket";
        }

        else if (ServerChanger.instance.isAWS_Live)
        {
            socialPokerUrl = "wss://poker-face.kelltontech.net:7070/socket.io/?EIO=3&transport=websocket"; //...For AWS...live server...//
            socialPokerTournamentUrl = "wss://poker-face.kelltontech.net:6565/socket.io/?EIO=3&transport=websocket"; //..........AWS Live.......//
        }
        else if (ServerChanger.instance.isAWS_QA)
        {
            //socialPokerUrl = "ws://poker-qa.kelltontech.net:6060/socket.io/?EIO=3&transport=websocket";
            //socialPokerTournamentUrl = "ws://poker-qa.kelltontech.net:7575/socket.io/?EIO=3&transport=websocket";

            socialPokerUrl = "wss://poker-qa.kelltontech.net:6100/socket.io/?EIO=3&transport=websocket";
            socialPokerTournamentUrl = "wss://poker-qa.kelltontech.net:6200/socket.io/?EIO=3&transport=websocket";
        }

        totalPlayersSitting = 0;
        CommonFunctions.instance.NeverSleep();
        localSeatID = 0;
        checkInternetUrl = ServerChanger.instance.domainURL + "api/v1/user/check-internet";
        if (isSocialPokerDev || isSocialPokerStage)
        {
            SocialGame.instance.SocialPokerButtons.gameObject.SetActive(true);
        }
        InternetCheck();
    }
    void OnApplicationPause(bool pauseStatus)
    {
        print("Pause Status...........start.. " + pauseStatus);

        if (isSwitching)
        {
            if (pauseStatus)
            {
                if (isTournament)
                {
                    TournamentManagerScript.instance.socket.Emit("__minimise");
                    if (TournamentManagerScript.instance.timeSubscribeEvent == 1)
                    {
                        TournamentManagerScript.instance.UnSubscribeToServerEvents();
                    }
                }
                else
                {
                    GameSerializeClassesCollection.instance.switchTableEmitter.token = GameSerializeClassesCollection.instance.observeTable.token;
                    string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.switchTableEmitter);
                    print("__minimise emmitter..." + data);
                    PokerNetworkManager.instance.socket.Emit("__minimise", new JSONObject(data));
                    PokerNetworkManager.instance.UnSubscribeToServerEvents();
                }
            }
            else
            {
                print("Pause Status 4" + pauseStatus);
                UIManagerScript.instance.loadingPanel.SetActive(false);
                PokerNetworkManager.instance.SubscribeToServerEvents();

                StopCoroutine(nameof(SwitchTrueResume));
                StartCoroutine(SwitchTrueResume());

            }
            return;
        }

        else if (activeTable != null && activeTable.activeInHierarchy && Table.instance.table.status != 0)
        {
            if (pauseStatus)
            {
                PlayerActionManagement.instance.ResetGameOnNewRound(false);
            }
            else
            {
                print("Pause Status 3" + pauseStatus);
                if (!isResetCompleted)
                {
                    print("Pause Status 4" + pauseStatus);
                    StopCoroutine("ReSubscribe");
                    UIManagerScript.instance.loadingPanel.SetActive(false);           //Update
                    StartCoroutine(ReSubscribe());                                    //Update
                    if (isVideoTable)
                    {
                        AgoraInit.instance.MuteUnmuteLocalPlayer(false);
                        AgoraInit.instance.MuteUnmuteAllRemotePlayer(false);

                    }
                }

            }
        }

        else
        {
            if (totalPlayersSitting == 1)
            {
                if (pauseStatus)
                {
                    if (isTournament)
                    {
                        TournamentManagerScript.instance.socket.Emit("__minimise");
                        if (TournamentManagerScript.instance.timeSubscribeEvent == 1)
                        {
                            TournamentManagerScript.instance.UnSubscribeToServerEvents();
                        }
                    }
                    else
                    {
                        GameSerializeClassesCollection.instance.switchTableEmitter.token = GameSerializeClassesCollection.instance.observeTable.token;
                        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.switchTableEmitter);
                        print("__minimise emmitter..." + data);
                        PokerNetworkManager.instance.socket.Emit("__minimise", new JSONObject(data));
                        PokerNetworkManager.instance.UnSubscribeToServerEvents();
                    }
                }
                else
                {
                    print("Pause Status 3" + pauseStatus);
                    if (!isResetCompleted)
                    {
                        print("Pause Status 4" + pauseStatus);
                        StopCoroutine("ReSubscribe");
                        UIManagerScript.instance.loadingPanel.SetActive(false);
                        StartCoroutine(ReSubscribe());
                        if (isVideoTable)
                        {
                            AgoraInit.instance.MuteUnmuteLocalPlayer(false);
                            AgoraInit.instance.MuteUnmuteAllRemotePlayer(false);

                        }
                    }
                }
            }
        }
    }

    bool isResetCompleted = false;

    IEnumerator SwitchTrueResume()
    {
        yield return new WaitForSeconds(3f);
        if (!isSwitching)
        {
            StopCoroutine(nameof(ReSubscribe));
            StartCoroutine(ReSubscribe());
        }
    }

    // This function is used to reset all table UIs after resuming from minimize or resuming after internet disconnect
  
    public IEnumerator ReSubscribe()
    {
        isResetCompleted = true;
        UIManagerScript.instance.loadingPanel.SetActive(true);
        UIManagerScript.instance.loadingPanel.transform.GetChild(1).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("Resuming Game");//"Resuming Game...";
        if (isVideoTable)
        {
            UIManagerScript.instance.ResetOnReFocus(true);
        }
        else
        {
            UIManagerScript.instance.ResetOnReFocus(false);
        }

        print("ReSubscribe...........................");
        if (isTournament)
        {
            TournamentManagerScript.instance.SubscribeOnReJoin();
            TournamentManagerScript.instance.socket.eventQueue.Clear();
            GameSerializeClassesCollection.instance.resumeInternet.tournament_id = SocialTournamentScript.instance.tournament_ID;
            GameSerializeClassesCollection.instance.resumeInternet.playerName = AccessGallery.instance.profileName[0].text;

            string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.resumeInternet);
            TournamentManagerScript.instance.socket.Emit("__resume_minimise", new JSONObject(data));
            yield return new WaitForSeconds(2f);
            if (TournamentManagerScript.instance.timeSubscribeEvent == 0)
            {
                TournamentManagerScript.instance.SubscribeToServerEvents();
            }
        }

        else
        {
            PokerNetworkManager.instance.SubscribeOnReJoin();
            PokerNetworkManager.instance.socket.eventQueue.Clear();
            GameSerializeClassesCollection.instance.switchTableEmitter.token = GameSerializeClassesCollection.instance.observeTable.token;
            GameSerializeClassesCollection.instance.switchTableEmitter.tableNumber = GameSerializeClassesCollection.instance.observeTable.ticket;
            string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.switchTableEmitter);
            print("__resume_minimise emiter..." + data);
            PokerNetworkManager.instance.socket.Emit("__resume_minimise", new JSONObject(data));
            yield return new WaitForSeconds(2f);
            PokerNetworkManager.instance.SubscribeToServerEvents();
        }
        UIManagerScript.instance.loadingPanel.SetActive(false);
        UIManagerScript.instance.loadingPanel.transform.GetChild(1).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("Reconnecting");//"Reconnecting...";

        isResetCompleted = false;
    }

    // This function is used to resume all videos after resuming from minimize or resuming after internet disconnect......//
    public IEnumerator GenerateVideoOnResume()
    {
        yield return new WaitForSeconds(2f);
        if (!isObserver)
        {
            PlayersGenerator.instance.AssignVideoToLocalPlayer();
        }
        PlayersGenerator.instance.AssignVideoPanelsToPlayers();
    }

    //...................................................................................................//
    private bool timedOut;
    private IEnumerator TimeOutCoroutine;

    // This function is used to check timed out during no internet or slow internet.....//
    public void StartTimeoutChecker(float timeout)
    {
        TimeOutCoroutine = TimeoutChecker(timeout);
        try
        {
            StopCoroutineTimeOutChecker();
        }
        catch
        {
            print("Already stopped");
        }
        StartCoroutine(TimeOutCoroutine);
    }
    public void StopCoroutineTimeOutChecker()
    {
        StopCoroutine(TimeOutCoroutine);
    }

    private IEnumerator TimeoutChecker(float timeout)
    {
        timedOut = false;
        while (timeout > 0)
        {
            timeout -= Time.deltaTime;
            //print("timeout........" + timeout);
            yield return null;
        }
        UIManagerScript.instance.lowChipPanel.gameObject.SetActive(true);
        UIManagerScript.instance.lowChipPanelText.text = "Internet Problem!!";
        timedOut = true;
    }
    //...................................................................................................//

    // This function is used to check internet status.
    public IEnumerator Internet()
    {
        yield return new WaitForSeconds(1);
        print("INTERNET CHECK START........");
        while (true)
        {
            if (isTournament)
            {
                if (noInternet || Application.internetReachability == NetworkReachability.NotReachable || !Tournamentsocket.wsConnected)
                {
                    noInternetTimes++;

                    NoInternet();
                    noInternet = true;
                    slowInternet = false;
                    fastInternet = false;
                    break;
                }

                if (slowInternet)
                {
                    slowInternetTimes++;
                    try
                    {
                        StartTimeoutChecker(60);
                        //AgoraInit.instance.MuteVideo(true);
                        //AgoraInit.instance.MuteAllRemoteVideo(true);
                        //hostPlayerVideoPanel.transform.GetChild(0).gameObject.SetActive(true);

                        //Off local player 
                        Debug.Log("SLOW INTERNET Check internet connection!");

                        StartCoroutine(InternetImproveCoroutine());
                        break;
                    }
                    catch
                    {
                        StartCoroutine(InternetImproveCoroutine());
                        break;
                    }
                }

                yield return new WaitForSeconds(1);
            }
            else
            {
                if (noInternet || Application.internetReachability == NetworkReachability.NotReachable || !socket.wsConnected)
                {
                    noInternetTimes++;

                    NoInternet();
                    noInternet = true;
                    slowInternet = false;
                    fastInternet = false;
                    break;
                }

                if (slowInternet)
                {
                    slowInternetTimes++;
                    try
                    {
                        StartTimeoutChecker(60);
                        //AgoraInit.instance.MuteVideo(true);
                        //AgoraInit.instance.MuteAllRemoteVideo(true);
                        //hostPlayerVideoPanel.transform.GetChild(0).gameObject.SetActive(true);

                        //Off local player 
                        Debug.Log("SLOW INTERNET Check internet connection!");

                        StartCoroutine(InternetImproveCoroutine());
                        break;
                    }
                    catch
                    {
                        StartCoroutine(InternetImproveCoroutine());
                        break;
                    }
                }

                yield return new WaitForSeconds(1);
            }
        }

        yield return new WaitForSeconds(2);

    }

    // This function is used during slow internet
    IEnumerator InternetImproveCoroutine()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            if (timedOut)
            {
                UIManagerScript.instance.TableToPokerUI(2);
                break;
            }
            if (isTournament)
            {
                if (!Tournamentsocket.wsConnected || noInternet || Application.internetReachability == NetworkReachability.NotReachable)
                {
                    NoInternet();
                    noInternet = true;
                    slowInternet = false;
                    fastInternet = false;
                    break;
                }

                if (fastInternet && Tournamentsocket.wsConnected && (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork))
                {
                    try
                    {
                        Debug.Log("Internet connection reachable");
                        StopCoroutineTimeOutChecker();
                        //AgoraInit.instance.MuteVideo(false);
                        //AgoraInit.instance.MuteAllRemoteVideo(false);
                        //hostPlayerVideoPanel.transform.GetChild(0).gameObject.SetActive(false);

                        StartCoroutine(Internet());
                        break;
                    }
                    catch
                    {
                        StartCoroutine(Internet());
                        break;
                    }
                }
            }
            else
            {
                if (!socket.wsConnected || noInternet || Application.internetReachability == NetworkReachability.NotReachable)
                {
                    NoInternet();
                    noInternet = true;
                    slowInternet = false;
                    fastInternet = false;
                    break;
                }

                if (fastInternet && socket.wsConnected && (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork))
                {
                    try
                    {
                        Debug.Log("Internet connection reachable");
                        StopCoroutineTimeOutChecker();
                        //AgoraInit.instance.MuteVideo(false);
                        //AgoraInit.instance.MuteAllRemoteVideo(false);
                        //hostPlayerVideoPanel.transform.GetChild(0).gameObject.SetActive(false);

                        StartCoroutine(Internet());
                        break;
                    }
                    catch
                    {
                        StartCoroutine(Internet());
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    // This function is checks if internet is fast again.
    IEnumerator InternetResumeAgainCoroutine()
    {
        yield return new WaitForSeconds(2f);
        while (true)
        {
            if (timedOut)
            {
                UIManagerScript.instance.TableToPokerUI(1);
                break;
            }
            if (isTournament)
            {
                if (/*fastInternet &&*/ Tournamentsocket.wsConnected && (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork))
                {
                    Debug.Log("Internet connection reachable");
                    StopCoroutineTimeOutChecker();

                    if (activeTable != null)
                    {
                        if (isTournament)
                        {
                            UIManagerScript.instance.loadingPanel.transform.GetChild(1).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("Resuming Game");
                            TournamentManagerScript.instance.socket.Connect();
                            TournamentManagerScript.instance.SubscribeOnReJoin();
                            yield return new WaitForSeconds(1f);
                            GameSerializeClassesCollection.instance.resumeInternet.tournament_id = SocialTournamentScript.instance.tournament_ID;
                            GameSerializeClassesCollection.instance.resumeInternet.ticket = TournamentManagerScript.instance.tableNo;
                            GameSerializeClassesCollection.instance.resumeInternet.playerName = AccessGallery.instance.profileName[0].text;
                            GameSerializeClassesCollection.instance.resumeInternet.token = Communication.instance.playerToken;
                            string body = JsonUtility.ToJson(GameSerializeClassesCollection.instance.resumeInternet);
                            Debug.Log("__internet_handler ...." + body);
                            TournamentManagerScript.instance.socket.Emit("__internet_handler", new JSONObject(body));
                            TournamentManagerScript.instance.socket.eventQueue.Clear();
                            yield return new WaitForSeconds(2f);
                            if (TournamentManagerScript.instance.timeSubscribeEvent == 0)
                            {
                                TournamentManagerScript.instance.SubscribeToServerEvents();
                            }
                        }
                    }
                    yield return new WaitForSeconds(1f);
                    if (UIManagerScript.instance.loadingPanel != null)
                    {
                        UIManagerScript.instance.loadingPanel.SetActive(false);
                    }
                    StartCoroutine(Internet());
                    break;
                }
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                if (fastInternet && socket.wsConnected && (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork))
                {
                    Debug.Log("Internet connection reachable");
                    StopCoroutineTimeOutChecker();

                    if (activeTable != null)
                    {
                        UIManagerScript.instance.loadingPanel.transform.GetChild(1).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("Resuming Game");
                        PokerNetworkManager.instance.socket.Connect();
                        PokerNetworkManager.instance.SubscribeOnReJoin();
                        yield return new WaitForSeconds(1f);
                        GameSerializeClassesCollection.instance.resumeInternet.ticket = GameSerializeClassesCollection.instance.observeTable.ticket;
                        GameSerializeClassesCollection.instance.resumeInternet.playerName = AccessGallery.instance.profileName[0].text;
                        GameSerializeClassesCollection.instance.resumeInternet.token = Communication.instance.playerToken;
                        string body = JsonUtility.ToJson(GameSerializeClassesCollection.instance.resumeInternet);
                        PokerNetworkManager.instance.socket.Emit("__internet_handler", new JSONObject(body));
                        PokerNetworkManager.instance.socket.eventQueue.Clear();
                        Debug.Log("__internet_handler ...." + body);
                        yield return new WaitForSeconds(2f);
                        PokerNetworkManager.instance.SubscribeToServerEvents();
                    }
                    yield return new WaitForSeconds(1f);
                    if (UIManagerScript.instance.loadingPanel != null)
                    {
                        UIManagerScript.instance.loadingPanel.SetActive(false);
                    }
                    StartCoroutine(Internet());
                    break;
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
        yield return new WaitForSeconds(0.1f);
    }

    // This function is used in no internet situation
    void NoInternet()
    {
        Debug.Log("Error. Check internet connection!");
        isGameResumed = false;
        StartTimeoutChecker(20);
        if (UIManagerScript.instance.loadingPanel != null)
        {
            UIManagerScript.instance.loadingPanel.SetActive(true);
            UIManagerScript.instance.loadingPanel.transform.GetChild(1).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("Reconnecting");
        }
        if (isTournament)
        {
            if (TournamentManagerScript.instance.timeSubscribeEvent == 1)
            {
                TournamentManagerScript.instance.UnSubscribeToServerEvents();
            }
        }
        else
        {
            if (networkManager.activeInHierarchy)
            {
                PokerNetworkManager.instance.UnSubscribeToServerEvents();
            }
        }

        if (isVideoTable)
        {
            UIManagerScript.instance.ResetOnReFocus(true);
        }
        else
        {
            UIManagerScript.instance.ResetOnReFocus(false);
        }

        StartCoroutine(InternetResumeAgainCoroutine());
    }

    // This function is used to initilize game table when a player enters in the game.
    public void InitlializeOnStart()
    {
        if (isVideoTable)
        {
            NonVideoTable.SetActive(false);
            NumberOfSeats = 6;
            //NumberOfSeats = 8;
            if (!isTournament)
            {
                Screen.orientation = ScreenOrientation.Landscape;
            }
            activeTable = videoTable;
            if (isSocialPokerDev || isSocialPokerStage)
            {
                activeTable = videoTable;
            }

            activeTable.gameObject.SetActive(true);

        }
        else
        {
            videoTable.SetActive(false);
            SocialVideoTable.SetActive(false);

            NumberOfSeats = 6;
            Screen.orientation = ScreenOrientation.Portrait;
            activeTable = NonVideoTable;
            activeTable.gameObject.SetActive(true);
        }
    }

    // This function is used to initlize video on video table
    public void OnVideoEngine(string tableId, int clientId)
    {
        AgoraInit gameController = agoraController.GetComponent<AgoraInit>();
        gameController.LeaveChannel();
        gameController.onJoinButtonClicked(tableId, clientId);
    }

    float totalTime = 0;
    float totalTimerecieve = 0;
    // This function is used to run command timer to execute each commands for every player
    public IEnumerator Timer(float time, bool islocal)
    {
        totalTime = time;
        timerImage.GetComponent<Image>().fillAmount = 1f;
        if (isNewTimer)
        {
            float a = 1 / totalTime;
            float b = a * Math.Abs(totalTimerecieve);
            timerImage.GetComponent<Image>().fillAmount = b;
            isNewTimer = false;
        }

        timerImage.gameObject.SetActive(true);

        while (time != 0)
        {
            time -= 0.1f;
            timerImage.GetComponent<Image>().fillAmount -= 0.1f / totalTime;

            if (timerImage.GetComponent<Image>().fillAmount > 0 & timerImage.GetComponent<Image>().fillAmount <= 8f * Time.deltaTime & alarmsound == false)
            {
                print("timerImage.GetComponent<Image>().fillAmount------------11 ->" + timerImage.GetComponent<Image>().fillAmount);

                if (islocal)
                {
                    alarmsound = true;

                    UIManagerScript.instance.alarmClock.SetActive(true);

                    if (PlayerPrefs.GetInt("SoundOffOn") == 0)
                    {
                        SoundManager.instance.alarmClocksound.volume = 1;
                        SoundManager.instance.alarmClocksound.Play();

                    }
                }
            }
            yield return new WaitForSeconds(0.078f);
        }

        totalTime = 0;
        yield return new WaitForSeconds(0.5f);
        UIManagerScript.instance.HideCommands();
    }

    private IEnumerator TimerEnum;

    // This function is used to inilize command timer..
    public void StartCoroutineTimer(int time, bool islocal)
    {
        TimerEnum = Timer(time, islocal);
        try
        {
            StopCoroutineTimer();
        }
        catch
        {
            print("Already stopped");
        }
        StartCoroutine(TimerEnum);
    }

    // This function is used to stop command timer
    public void StopCoroutineTimer()
    {
        if (TimerEnum != null)
        {
            StopCoroutine(TimerEnum);
        }
    }

    #region Club Poker

    // This function is used to update observer list(no use in social poker)
    public void UpdateObserver(string name)
    {
        GameObject observer = Instantiate(observerTextPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        observer.transform.SetParent(observerContent.transform);
        observer.transform.localScale = Vector3.one;
        observer.transform.GetComponent<Text>().text = name;
    }


    // This function is used to show start button on the table (no use in social poker)
    public void PostDataForTableStartButton(string tableId)
    {
        print("error..... ");
        GameSerializeClassesCollection.instance.tableID.table_id = tableId;
        print("error.....111111 ");

        string body = JsonUtility.ToJson(GameSerializeClassesCollection.instance.tableID);
    }

    // This function is used to shuffle players on the table (no use in social poker)
    public IEnumerator ShuffleStart()
    {
        print("ShuffleStart");
        //if (isVideoTable)
        //{
        //    //yield return new WaitForSeconds(1f);
        //}
        //localSeatID = localSeatIDDummy;
        isShuffflingComplete = false;
        yield return new WaitForSeconds(0.1f);
        while (true)
        {
            if (ChairAnimation.instance.isChairAnimComplete && CardShuffleAnimation.instance.isAnimationComplete)
            {
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(0.1f);
        //print("Shuffling Start 3 sec");
        //for (int i = 0; i < playersParent.transform.childCount; i++)
        //{
        //    if (playersParent.transform.GetChild(i).childCount > 1)
        //    {
        //        addPlayers.Add(playersParent.transform.GetChild(i).GetChild(0).gameObject);
        //    }
        //}
        //if (addPlayers.Count == 1)
        //{
        //    UIManagerScript.instance.waitOtherPlayerPanel.gameObject.SetActive(true);
        //}
        ////StartCoroutine(ShuffleNew());
        //ShuffleNew();
        print("Shuffling end 2");


        //yield return new WaitForSeconds(1f);

        //if (dealerSeatid != 0)
        //{
        //    int dealerseatid = GetActualSeatID(dealerSeatid);
        //    playersParent.transform.GetChild(dealerseatid).GetChild(0).transform.GetComponent<PokerPlayerController>().TurnOnDealer(1);
        //}
        //dealerSeatid = 0;
        //dealerName = null;

        isShuffflingComplete = true;
    }

    // This function is used to shuffle players on the table (no use in social poker)
    void ShuffleNew()
    {
        //TurnOffVideoTableUI();
        for (int i = 0; i < addPlayers.Count; i++)
        {
            int id = int.Parse(addPlayers[i].transform.parent.name);
            int getSeatId = GetActualSeatID(id);

            addPlayers[i].transform.SetParent(playersParent.transform.GetChild(getSeatId));
            addPlayers[i].transform.SetAsFirstSibling();

            playersParent.transform.GetChild(getSeatId).GetChild(0).localPosition = Vector3.zero;
            playersParent.transform.GetChild(getSeatId).GetChild(0).GetComponent<PokerPlayerController>().ResetValues(1);
            playersParent.transform.GetChild(getSeatId).GetChild(0).GetComponent<PokerPlayerController>().StartCoroutine("Initlize");


        }
        print("Shuffling end 1 ");
        //  yield return null;
        //yield return new WaitForSeconds(2f);
    }
    public void ShowTableStartButton(string response)
    {
        if (string.IsNullOrEmpty(response))
        {
            print("error");
        }
        else
        {
            print("MailScriptASHISH..." + response);
            GameSerializeClassesCollection.instance.tableCreated = JsonUtility.FromJson<GameSerializeClassesCollection.TableCreated>(response);
            if (GameSerializeClassesCollection.instance.tableCreated.isCreatorBoard)
            {
                if (Table.instance.table.status == 0 && !ClubManagement.instance.currentSelectedTableIsAutoStart)
                {
                    // UIManagerScript.instance.tableStartButton.gameObject.SetActive(true);
                    StartCoroutine(ActivateTableStartButton());
                }

                if (ClubManagement.instance.currentSelectedTableIsBuyInAuth)
                {
                    print("Ashish2");
                    //if (!MailBoxScripts._instance.buyinAuthKeyButtonVideo.activeInHierarchy || !MailBoxScripts._instance.buyinAuthKeyButtonNonVideo.activeInHierarchy)
                    //{
                    //    //MailBoxScripts._instance.BuyInAuthKeyButton(true);

                    //}
                }
            }
            else
            {
                UIManagerScript.instance.tableStartButton.gameObject.SetActive(false);
            }

        }
    }
    IEnumerator ActivateTableStartButton()
    {
        print("Couroutine start");
        ClubManagement.instance.currentSelectedTableMinAutoStart = 2;
        yield return new WaitForSeconds(0.1f);
        while (true)
        {
            if (ClubManagement.instance.currentSelectedTableMinAutoStart <= totalPlayersInGame)
            {
                UIManagerScript.instance.tableStartButton.gameObject.SetActive(true);
                break;
            }
            totalPlayersInGame = 0;
            yield return new WaitForSeconds(0.5f);
            CheckTotalPlayers();
        }
    }

    public int GetActualSeatIDClubPoker(int seatID)
    {

        return (seatID - 1);
        //if (isObserver)
        //{
        //    return (seatID - 1);
        //}
        //else if (localSeatID == 0)
        //{
        //    return (seatID - 1);
        //}
        //else
        //{
        //    int id = seatID;
        //    int diff = GameManagerScript.instance.localSeatID - id;

        //    if (diff >= 1)
        //    {
        //        int actuallId = NumberOfSeats - diff;
        //        return actuallId;
        //    }
        //    else if (diff <= -1)
        //    {
        //        int actuallId = id - GameManagerScript.instance.localSeatID;
        //        return actuallId;
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}
    }

    #endregion

    // This function is used to check total player on the table when table is started (no use in social poker)
    public int totalPlayersInGame = 0;
    public void CheckTotalPlayers()
    {
        try
        {
            for (int i = 0; i < playersParent.transform.childCount; i++)
            {
                if (playersParent.transform.GetChild(i).childCount > 1)
                {
                    totalPlayersInGame++;
                }
            }
        }
        catch
        {
            print("Some Error");
        }
    }

    public int GetActualSeatID(int seatID)
    {
        return (seatID - 1);
    }

    // This function is used to activate commands for local player
    public void CheckActiveCommands(GameSerializeClassesCollection.PlayerOnRoundStart playerInfo)
    {
        if (playerInfo.action_buttons.Length <= 3)
        {
            for (int i = 0; i < playerInfo.action_buttons.Length; i++)
            {
                try
                {
                    if (playerInfo.action_buttons[i] == "call")
                    {
                        UIManagerScript.instance.footer.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(true);
                        string value = GameManagerScript.instance.KiloFormat(playerInfo.minBet);
                        UIManagerScript.instance.footer.GetChild(0).GetChild(0).GetChild(1).GetChild(2).GetComponent<Text>().text = "CALL " + value;
                    }

                    if (playerInfo.action_buttons[i] == "raise")
                    {
                        UIManagerScript.instance.footer.GetChild(0).GetChild(0).GetChild(2).gameObject.SetActive(true);
                        GameManagerScript.instance.reRaise = playerInfo.reRaise;
                        int actuallId = GetActualSeatID(playerInfo.seatId);
                        print("ActualID " + actuallId + "Child " + playersParent.transform.GetChild(actuallId).gameObject.name + "Player " + playersParent.transform.GetChild(actuallId).GetChild(0).name);
                        int value = playersParent.transform.GetChild(actuallId).GetChild(0).transform.GetComponent<PokerPlayerController>().player.bet;
                        UIManagerScript.instance.raiseSlider.GetComponent<Slider>().minValue = playerInfo.minBet;
                        if (playerInfo.minBet == 0)
                        {
                            UIManagerScript.instance.raiseSlider.GetComponent<Slider>().minValue = value;
                        }
                        currentPlayerTableBet = value;

                        UIManagerScript.instance.raiseSlider.GetComponent<Slider>().maxValue = playerInfo.chips;
                        if (playerInfo.chips > playerInfo.minBet && playerInfo.chips > value)
                        {
                            if (playerInfo.minBet == 0)
                            {
                                UIManagerScript.instance.raiseSlider.GetComponent<Slider>().value = value;
                            }
                            else
                            {
                                UIManagerScript.instance.raiseSlider.GetComponent<Slider>().value = playerInfo.minBet;
                            }
                        }
                        else
                        {
                            UIManagerScript.instance.raiseSlider.GetComponent<Slider>().value = playerInfo.chips;
                            UIManagerScript.instance.raiseSlider.GetComponent<Slider>().minValue = playerInfo.chips;
                        }
                        UIManagerScript.instance.raiseSlider.parent.GetChild(1).GetChild(0).GetComponent<Text>().text = "" + UIManagerScript.instance.raiseSlider.GetComponent<Slider>().value;
                        PlayerActionManagement.instance.actionName = "raise";
                    }

                    if (playerInfo.action_buttons[i] == "fold")
                    {
                        UIManagerScript.instance.footer.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
                    }

                    if (playerInfo.action_buttons[i] == "bet")
                    {
                        UIManagerScript.instance.footer.GetChild(0).GetChild(0).GetChild(3).gameObject.SetActive(true);
                        UIManagerScript.instance.betSlider.GetComponent<Slider>().minValue = playerInfo.minBet;
                        UIManagerScript.instance.betSlider.GetComponent<Slider>().maxValue = playerInfo.chips;
                        UIManagerScript.instance.betSlider.GetComponent<Slider>().value = playerInfo.minBet;
                        if (playerInfo.chips < playerInfo.minBet)
                        {
                            UIManagerScript.instance.betSlider.GetComponent<Slider>().value = playerInfo.chips;
                            UIManagerScript.instance.betSlider.GetComponent<Slider>().minValue = playerInfo.chips;
                        }
                        UIManagerScript.instance.betSlider.parent.GetChild(1).GetChild(0).GetComponent<Text>().text = "" + UIManagerScript.instance.betSlider.GetComponent<Slider>().value;
                        PlayerActionManagement.instance.actionName = "bet";
                    }

                    if (playerInfo.action_buttons[i] == "check")
                    {
                        isCheckActive = true;
                        UIManagerScript.instance.footer.GetChild(0).GetChild(0).GetChild(4).gameObject.SetActive(true);
                        minBet = playerInfo.bet;
                    }

                    if (playerInfo.action_buttons[i] == "allin")
                    {
                        UIManagerScript.instance.footer.GetChild(0).GetChild(0).GetChild(5).gameObject.SetActive(true);           //Update
                    }
                }
                catch (Exception e)
                {
                    print("Some error in CheckActiveCommands in GamemanagerScript.cs" + e);
                }

            }
            print("CheckActiveCommands");
        }
    }

    public void NewRoundStart(GameSerializeClassesCollection.RoundStartInfo roundStartInfoObj)
    {
        for (int i = 0; i < playersParent.transform.childCount; i++)
        {
            try
            {
                if (playersParent.transform.childCount > 1)
                {
                    if (playersParent.transform.GetChild(i).GetChild(0).transform.GetComponent<PokerPlayerController>().player.seatId == roundStartInfoObj.player.seatId)
                    {
                        print("ID Match " + roundStartInfoObj.player.seatId);
                        playersParent.transform.GetChild(i).GetChild(0).transform.GetComponent<PokerPlayerController>().player.bet = roundStartInfoObj.player.bet;
                        playersParent.transform.GetChild(i).GetChild(0).transform.GetComponent<PokerPlayerController>().player.chips = roundStartInfoObj.player.chips;
                        playersParent.transform.GetChild(i).GetChild(0).transform.GetComponent<PokerPlayerController>().player.cards = roundStartInfoObj.player.cards;
                        playersParent.transform.GetChild(i).GetChild(0).transform.GetComponent<PokerPlayerController>().player.folded = roundStartInfoObj.player.folded;
                        playersParent.transform.GetChild(i).GetChild(0).transform.GetComponent<PokerPlayerController>().player.roundBet = roundStartInfoObj.player.roundBet;
                        playersParent.transform.GetChild(i).GetChild(0).transform.GetComponent<PokerPlayerController>().player.isSurvive = roundStartInfoObj.player.isSurvive;
                        playersParent.transform.GetChild(i).GetChild(0).transform.GetComponent<PokerPlayerController>().player.allIn = roundStartInfoObj.player.allIn;

                        playersParent.transform.GetChild(i).GetChild(0).transform.GetComponent<PokerPlayerController>().UpdateValues();

                        UIManagerScript.instance.standUpButton.SetActive(true);
                        GameManagerScript.instance.isStandUp = false;
                        GameManagerScript.instance.isStandupClicked = false;
                        UIManagerScript.instance.observingUI.SetActive(false);
                        GameManagerScript.instance.isObserver = false;
                        for (int j = 0; j < UIManagerScript.instance.allOtherBottomPanelBtn.Count; j++)
                        {
                            UIManagerScript.instance.allOtherBottomPanelBtn[j].SetActive(true);
                        }
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                print("Some Error in NewRoundStart GameManagerScript.cs" + e);
            }
        }
    }

    // This function is used to update table information
    public void UpdateTable(GameSerializeClassesCollection.TableInfo tableInfo)
    {
        try
        {
            Table.instance.table.tableNumber = tableInfo.tableNumber;
            Table.instance.table.status = tableInfo.status;
            Table.instance.table.roundName = tableInfo.roundName;
            Table.instance.table.board = tableInfo.board;
            Table.instance.table.roundCount = tableInfo.roundCount;
            Table.instance.table.raiseCount = tableInfo.raiseCount;
            Table.instance.table.betCount = tableInfo.betCount;
            Table.instance.table.totalBet = tableInfo.totalBet;
            Table.instance.table.initChips = tableInfo.initChips;
            Table.instance.table.currentPlayer = tableInfo.currentPlayer;
            Table.instance.table.currentPlayerName = tableInfo.currentPlayerName;
            Table.instance.table.maxReloadCount = tableInfo.maxReloadCount;

            Table.instance.table.smallBlind.playerName = tableInfo.smallBlind.playerName;
            Table.instance.table.smallBlind.seatId = tableInfo.smallBlind.seatId;
            Table.instance.table.smallBlind.amount = tableInfo.smallBlind.amount;

            Table.instance.table.bigBlind.playerName = tableInfo.bigBlind.playerName;
            Table.instance.table.bigBlind.seatId = tableInfo.bigBlind.seatId;
            Table.instance.table.bigBlind.amount = tableInfo.bigBlind.amount;

            if (tableInfo.sidePots.Length != 0)
            {
                Table.instance.table.sidePots = new int[tableInfo.sidePots.Length];
                for (int i = 0; i < tableInfo.sidePots.Length; i++)
                {
                    Table.instance.table.sidePots[i] = tableInfo.sidePots[i];
                }
                Table.instance.table.realSidePots = new int[tableInfo.realSidePots.Length];
                for (int i = 0; i < tableInfo.realSidePots.Length; i++)
                {
                    Table.instance.table.realSidePots[i] = tableInfo.realSidePots[i];
                }
            }
            if (tableInfo.sidePots.Length == 0)
            {
                Table.instance.table.sidePots = new int[0];
                Table.instance.table.realSidePots = new int[0];
            }

            Table.instance.UpdateTableValues();
            Table.instance.TableSidePots();
        }
        catch (Exception e)
        {
            print("Cannot Update Table" + e);
        }
        print(" Update Table ends");
    }

    // This function is used to update all players as well as table information if player entered in game table when game is running.
    public void AllBasicDataUpdate(GameSerializeClassesCollection.Basicdata basicdata)
    {
        print("GameManagerScript.instance.isGameResumed = true");
        isGameResumed = true;
        StartCoroutine(AllBasicDataUpdateCoroutine(basicdata));
    }

    IEnumerator AllBasicDataUpdateCoroutine(GameSerializeClassesCollection.Basicdata basicdata)
    {
        SoundManager.instance.attachedAudioSource.GetComponent<AudioSource>().mute = true;
        isNewTimer = true;
        commandTimer2 = basicdata.table.commandTimeout;
        totalTimerecieve = basicdata.table.timer;
        print("TIMER: " + basicdata.table.commandTimeout);

        //.........................................................................................................................//
        if (isHandEnd)
        {
            print("Called in gamemanager..........");
            WinningLogic.instance.StopWinningAnim();
            StartCoroutine(WinningLogic.instance.GameReset());
        }

        while (true)
        {
            if (ChairAnimation.instance.isChairAnimComplete && GameManagerScript.instance.isShuffflingComplete && !isPlayerGenerating)
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        //if (isHandEnd)
        //{
        //    print("Called in gamemanager..........");
        //    WinningLogic.instance.StopWinningAnim();
        //    StartCoroutine(WinningLogic.instance.GameReset());
        //}
        if (isTournament)
        {
            yield return new WaitForSeconds(1f);
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }

        try
        {
            print(" All Basic Data Update Coroutine starts");
            for (int i = 0; i < basicdata.players.Length; i++)
            {
                try
                {
                    int playerSeatId = GetActualSeatID(basicdata.players[i].seatId);

                    playersParent.transform.GetChild(playerSeatId).GetChild(0).transform.GetComponent<PokerPlayerController>().player.bet = basicdata.players[i].bet;
                    playersParent.transform.GetChild(playerSeatId).GetChild(0).transform.GetComponent<PokerPlayerController>().player.chips = basicdata.players[i].chips;
                    playersParent.transform.GetChild(playerSeatId).GetChild(0).transform.GetComponent<PokerPlayerController>().player.cards = basicdata.players[i].cards;
                    playersParent.transform.GetChild(playerSeatId).GetChild(0).transform.GetComponent<PokerPlayerController>().player.folded = basicdata.players[i].folded;
                    playersParent.transform.GetChild(playerSeatId).GetChild(0).transform.GetComponent<PokerPlayerController>().player.roundBet = basicdata.players[i].roundBet;
                    playersParent.transform.GetChild(playerSeatId).GetChild(0).transform.GetComponent<PokerPlayerController>().player.isSurvive = basicdata.players[i].isSurvive;
                    playersParent.transform.GetChild(playerSeatId).GetChild(0).transform.GetComponent<PokerPlayerController>().player.allIn = basicdata.players[i].allIn;
                    playersParent.transform.GetChild(playerSeatId).GetChild(0).transform.GetComponent<PokerPlayerController>().CardOn();
                    playersParent.transform.GetChild(playerSeatId).GetChild(0).transform.GetComponent<PokerPlayerController>().UpdateValues();
                }
                catch (Exception e)
                {
                    print("Basic Data UpdateValues.." + e);
                }
            }
            if (!isTournament)
            {
                Table.instance.blind.transform.parent.gameObject.SetActive(true);
                string sBlind = KiloFormat(basicdata.table.smallBlind.amount);
                string bBlind = KiloFormat(basicdata.table.bigBlind.amount);
                Table.instance.blind.text = sBlind + "/" + bBlind;
            }

            dealerSeatid = basicdata.table.dealer.seatId;
            dealerName = basicdata.table.dealer.playerName;

            int dealerseatid = GetActualSeatID(basicdata.table.dealer.seatId);
            playersParent.transform.GetChild(dealerseatid).GetChild(0).transform.GetComponent<PokerPlayerController>().TurnOnDealer(1);
            if (isVideoTable)
            {
                if (basicdata.table.dealer.isStraddle)
                {
                    playersParent.transform.GetChild(dealerseatid).GetChild(0).transform.GetComponent<PokerPlayerController>().dealerButtonFirstChild.GetChild(0).gameObject.SetActive(true);
                    playersParent.transform.GetChild(dealerseatid).GetChild(0).transform.GetComponent<PokerPlayerController>().dealerButtonFirstChild.GetChild(0).GetChild(0).GetComponent<Text>().text = "Straddle " + basicdata.table.dealer.amount.ToString();
                }
                else
                {
                    playersParent.transform.GetChild(dealerseatid).GetChild(0).transform.GetComponent<PokerPlayerController>().dealerButtonFirstChild.GetChild(0).gameObject.SetActive(false);
                }
            }
            else
            {
                if (basicdata.table.dealer.isStraddle)
                {
                    playersParent.transform.GetChild(dealerseatid).GetChild(0).transform.GetComponent<PokerPlayerController>().dealerButton.GetChild(0).gameObject.SetActive(true);
                    playersParent.transform.GetChild(dealerseatid).GetChild(0).transform.GetComponent<PokerPlayerController>().dealerButton.GetChild(0).GetChild(0).GetComponent<Text>().text = "Straddle " + basicdata.table.dealer.amount.ToString();
                }
                else
                {
                    playersParent.transform.GetChild(dealerseatid).GetChild(0).transform.GetComponent<PokerPlayerController>().dealerButton.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
        catch (Exception e)
        {
            print("Catch in All Basic Data Update Coroutine.." + e);
        }
        isGameResumed = false;

        yield return new WaitForSeconds(1f);
        SoundManager.instance.attachedAudioSource.GetComponent<AudioSource>().mute = false;

        print("All Basic Data Update Coroutine ends");
    }

    // This function is used to update all players as well as table information with next deal event().
    public IEnumerator UpdatePlayerOnNextDeal(GameSerializeClassesCollection.NextDealInfo nextDealInfo)
    {
        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < playersParent.transform.childCount; i++)
        {
            try
            {
                if (playersParent.transform.GetChild(i).childCount > 1)
                {
                    playersParent.transform.GetChild(i).GetChild(0).transform.GetComponent<PokerPlayerController>().UpdateValues();
                    playersParent.transform.GetChild(i).GetChild(0).transform.GetComponent<PokerPlayerController>().TurnOffActionUI(true);

                }
            }
            catch (Exception e)
            {
                print("Some Error in UpdatePlayerOnNextDeal GameManagerScript, PokerPlayerController Not Found..." + e);
                GameSerializeClassesCollection.instance.onError.token = Communication.instance.playerToken;
                GameSerializeClassesCollection.instance.onError.eventname = " __next_deal";
                GameSerializeClassesCollection.instance.onError.message = "ERROR IN NEXT DEAL DATA";
                string data2 = JsonUtility.ToJson(GameSerializeClassesCollection.instance.onError);
                print(data2);
                socket.Emit("custom_error", new JSONObject(data2));
            }
        }
        StartCoroutine(FindLocalPlayerAndUpdateHand(nextDealInfo.players));
    }

    // This function is used to show local player's hand rank during the game.
    public IEnumerator FindLocalPlayerAndUpdateHand(GameSerializeClassesCollection.PlayerOnRoundStart[] players)
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < playersParent.transform.childCount; i++)
        {
            try
            {
                if (playersParent.transform.GetChild(i).childCount == 2)
                {
                    if (playersParent.transform.GetChild(i).GetChild(0).GetComponent<PokerPlayerController>().isLocalPlayer)
                    {
                        if (isVideoTable)
                        {
                            HandRankPanel = playersParent.transform.GetChild(i).GetChild(1).GetChild(12).GetChild(3).gameObject;
                        }
                        else
                        {
                            HandRankPanel = playersParent.transform.GetChild(i).GetChild(1).GetChild(14).GetChild(2).gameObject;
                        }
                        //HandRankPanel = playersParent.transform.GetChild(i).GetChild(1).GetChild(12).GetChild(3).gameObject;
                        if (!isObserver && !isPlayerExcluded)
                        {
                            for (int j = 0; j < players.Length; j++)
                            {
                                if (players[j].message != "" && !players[j].folded)
                                {
                                    HandRankPanel.transform.GetChild(0).GetComponent<Text>().text = players[j].message;
                                    HandRankPanel.SetActive(true);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (HandRankPanel != null)
                {
                    HandRankPanel.SetActive(false);
                }
                print("NO PLayer found........" + e);
            }
        }
    }
    public void ResetTableOnExit(bool isResume)
    {
        for (int i = 0; i < playersParent.transform.childCount; i++)
        {
            try
            {
                if (playersParent.transform.GetChild(i).childCount > 1)
                {
                    playersParent.transform.GetChild(i).GetChild(0).transform.GetComponent<PokerPlayerController>().ResetPlayerOnTableExit(isResume);
                }
            }
            catch (Exception e)
            {
                print("Problem in Reset Player On TableExit...playersParent.transform.childCount.." + i);
                print("Problem in Reset Player On TableExit GameManagerScript.cs.." + e);
            }
        }
        try
        {
            if (isVideoTable)
            {

                TurnOffVideoTableUI(isResume);
                for (int i = 0; i < playersParent.transform.childCount; i++)
                {
                    playersParent.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(true);
                    playersParent.transform.GetChild(i).GetChild(0).GetChild(20).gameObject.SetActive(true);
                }
            }
            else
            {
                TurnOffNonVideoTable(isResume);
            }
        }
        catch (Exception e)
        {
            print("Problem in players Parent On TableExit.." + e);
        }
    }

    void TurnOffNonVideoTable(bool isResume)
    {
        for (int i = 0; i < playersParent.transform.childCount; i++)
        {
            if (!isStandUp)
            {
                playersParent.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                playersParent.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(true);
            }
            playersParent.transform.GetChild(i).GetChild(0).GetChild(1).gameObject.SetActive(false);
            playersParent.transform.GetChild(i).GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(false);
            playersParent.transform.GetChild(i).GetChild(0).GetChild(2).gameObject.SetActive(false);
            playersParent.transform.GetChild(i).GetChild(0).GetChild(3).gameObject.SetActive(false);
            playersParent.transform.GetChild(i).GetChild(0).GetChild(4).gameObject.SetActive(false);
            playersParent.transform.GetChild(i).GetChild(0).GetChild(5).gameObject.SetActive(false);
            playersParent.transform.GetChild(i).GetChild(0).GetChild(6).gameObject.SetActive(false);
            playersParent.transform.GetChild(i).GetChild(0).GetChild(7).gameObject.SetActive(false);
            playersParent.transform.GetChild(i).GetChild(0).GetChild(8).gameObject.SetActive(false);
            playersParent.transform.GetChild(i).GetChild(0).GetChild(9).gameObject.SetActive(false);
            playersParent.transform.GetChild(i).GetChild(0).GetChild(10).gameObject.SetActive(false);
            playersParent.transform.GetChild(i).GetChild(0).GetChild(11).gameObject.SetActive(false);
            playersParent.transform.GetChild(i).GetChild(0).GetChild(12).gameObject.SetActive(false);
            playersParent.transform.GetChild(i).GetChild(0).GetChild(13).gameObject.SetActive(false);
            playersParent.transform.GetChild(i).GetChild(0).GetChild(16).gameObject.SetActive(false);
        }
    }
    void TurnOffVideoTableUI(bool isResume)
    {
        if (isVideoTable)
        {
            for (int i = 0; i < playersParent.transform.childCount; i++)
            {
                try
                {
                    if (playersParent.transform.GetChild(i).childCount > 1)
                    {
                        playersParent.transform.GetChild(i).GetChild(1).GetChild(0).gameObject.SetActive(true);
                        playersParent.transform.GetChild(i).GetChild(1).GetChild(1).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(1).GetChild(2).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(1).GetChild(3).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(1).GetChild(4).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(1).GetChild(5).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(1).GetChild(6).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(1).GetChild(7).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(1).GetChild(8).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(1).GetChild(9).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(1).GetChild(10).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(1).GetChild(11).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(1).GetChild(12).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(1).GetChild(13).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(1).GetChild(14).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(1).GetChild(15).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(1).GetChild(16).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(1).GetChild(17).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(1).GetChild(18).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(1).GetChild(20).gameObject.SetActive(false);
                    }
                    else
                    {
                        playersParent.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(true);
                        playersParent.transform.GetChild(i).GetChild(0).GetChild(1).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(0).GetChild(2).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(0).GetChild(3).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(0).GetChild(4).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(0).GetChild(5).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(0).GetChild(6).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(0).GetChild(7).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(0).GetChild(8).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(0).GetChild(9).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(0).GetChild(10).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(0).GetChild(11).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(0).GetChild(12).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(0).GetChild(13).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(0).GetChild(14).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(0).GetChild(15).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(0).GetChild(16).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(0).GetChild(17).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(0).GetChild(18).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(0).GetChild(19).gameObject.SetActive(false);
                        playersParent.transform.GetChild(i).GetChild(0).GetChild(20).gameObject.SetActive(false);
                    }

                }
                catch
                {
                    print("Some error in TurnOffVideoTableUI GameManagerScript.cs");
                }
                if (i == dealerSeatid - 1)
                {
                    print("dealerSeatid in Gamemanager......." + dealerSeatid);
                    dealer.SetParent(playersParent.transform.GetChild(i).GetChild(0).GetChild(18));
                }
            }
        }
    }
    public void ResetTable(bool isSocketReset)
    {
        Table.instance.table.status = 0;
        Table.instance.table.roundName = null;
        Table.instance.table.board = null;
        Table.instance.table.roundCount = 0;
        Table.instance.table.raiseCount = 0;
        Table.instance.table.betCount = 0;
        Table.instance.table.totalBet = 0;
        Table.instance.table.initChips = 0;
        Table.instance.table.currentPlayer = 0;
        Table.instance.table.currentPlayerName = null;
        Table.instance.table.maxReloadCount = 0;

        Table.instance.table.smallBlind.playerName = null;
        Table.instance.table.smallBlind.seatId = 0;
        Table.instance.table.smallBlind.amount = 0;

        Table.instance.table.bigBlind.playerName = null;
        Table.instance.table.bigBlind.seatId = 0;
        Table.instance.table.bigBlind.amount = 0;

        Table.instance.table.sidePots = new int[0];
        UIManagerScript.instance.straddlePanelSymbol.SetActive(false);
        WinningLogic.instance.onePlayerShowdown = false;
        Table.instance.roundNameFromGameClass = "";

        totalPlayersInGame = 0;
        CardShuffleAnimation.instance.shuffleCardCount = 0;
        if (UIManagerScript.instance.tableStartButton.gameObject.activeInHierarchy)
        {
            UIManagerScript.instance.tableStartButton.gameObject.SetActive(false);
        }

        TableTimer.instance.activeTournamentTimer = false;
        if (isSocketReset)
        {
            SocketReset();
        }
        tournamentManager.SetActive(false);
        networkManager.SetActive(false);
        UIManagerScript.instance.loadingPanel.SetActive(false);
        if (isTournament)
        {
            SocialTournamentScript.instance.ResetEntriesList();
            SocialTournamentScript.instance.ResetRewardsList();
            TournamentGameDetail.instance.ResetTableList();
            TournamentGameDetail.instance.ResetRankList();
            TournamentGameDetail.instance.ResetWinnerList();
        }
        ClubManagement.instance.isBottomPanelClose = false;
        SocialPokerGameManager.instance.ResetThrowableAnimation();
    }
    public void SocketReset()
    {
        if (!isTournament)
        {
            socket.thPinging = false;
            socket.thPong = false;
            socket.connected = false;
            socket.wsConnected = false;

            socket.url = "";
            socket.gameObject.SetActive(false);
        }
        else
        {
            Tournamentsocket.thPinging = false;
            Tournamentsocket.thPong = false;
            Tournamentsocket.connected = false;
            Tournamentsocket.wsConnected = false;

            Tournamentsocket.url = "";
            Tournamentsocket.gameObject.SetActive(false);
        }
    }

    public void TableSetEmptytrue()
    {
        for (int i = 0; i < playersParent.transform.childCount; i++)
        {
            if (isVideoTable)
            {
                playersParent.transform.GetChild(i).GetChild(0).GetChild(20).gameObject.SetActive(false);
            }
            else
            {
                playersParent.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(false);
                playersParent.transform.GetChild(i).GetChild(0).GetChild(16).gameObject.SetActive(true);
            }
        }
    }

    public void InternetCheck()
    {
        StartCoroutine(ImageDownloadScript.instance.RepeatInternetAPIHit());
    }

    public string KiloFormat(int num)
    {
        if (num >= 1000000)
        {
            return (num / 1000000D).ToString("0.#") + "M";
        }
        if (num >= 1000)
        {
            return (num / 1000D).ToString("0.#") + "K";
        }
        return num.ToString("#,0");
    }

    public int totalplayerfound;
    public void CheckTotalPlayersSitting()
    {
        StartCoroutine(CheckTotalPlayersSittingCoroutine());
    }
    public IEnumerator CheckTotalPlayersSittingCoroutine()
    {

        print("totalPlayersSitting..........." + totalPlayersSitting);

        if (totalPlayersSitting <= 1)
        {
            totalplayerfound = 0;
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < playersParent.transform.childCount; i++)
            {
                try
                {
                    if (playersParent.transform.GetChild(i).childCount > 1)
                    {
                        totalplayerfound++;
                    }
                }
                catch (Exception e)
                {
                    print("Some Error in CheckTotalPlayersSittingCoroutine GameManagerScript.cs.." + e);
                }
            }

            if (totalPlayersSitting != totalplayerfound)
            {
                totalPlayersSitting = totalplayerfound;
            }
        }
        yield return null;
    }

    public string newTableID;
    public void SwitchTables(string tableID)
    {
        GameSerializeClassesCollection.instance.disbandTableMessage.errorStatus = true;
        print("SwitchTable1...");
        newTableID = tableID;

        if (isVideoTable)
        {
            UIManagerScript.instance.ResetOnReFocus(false);

            PokerNetworkManager.instance.socket.Emit("__exit_handle");
            for (int i = 0; i < PlayersGenerator.instance.videoPanelPlayers.Count; i++)
            {
                PlayersGenerator.instance.videoPanelPlayers.Remove(PlayersGenerator.instance.videoPanelPlayers[i]);
            }
            for (int i = 0; i < PlayersGenerator.instance.videoPanelsForAllClient.Count; i++)
            {
                GameObject objTrans = PlayersGenerator.instance.videoPanelsForAllClient[i].gameObject;
                PlayersGenerator.instance.videoPanelsForAllClient.Remove(PlayersGenerator.instance.videoPanelsForAllClient[i]);
                Destroy(objTrans);
            }

            Destroy(hostPlayerVideoPanel);
            GameObject go = new GameObject();
            go.name = "HostPlayerVideoPanel";
            go.AddComponent<RawImage>();
            hostPlayerVideoPanel = go;
            hostPlayerVideoPanel.transform.SetParent(UIManagerScript.instance.tableParent.parent);
            hostPlayerVideoPanel.transform.localPosition = Vector3.zero;
            hostPlayerVideoPanel.transform.localEulerAngles = new Vector3(0, 0, 0);
            hostPlayerVideoPanel.transform.localScale = new Vector3(1, 1, 1);
            hostPlayerVideoPanel.transform.GetComponent<RawImage>().color = new Color(1, 1, 1, 0);
            hostPlayerVideoPanel.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(300f, 180f);

            AgoraInit.instance.LeaveChannel();
        }
        else
        {
            UIManagerScript.instance.ResetOnReFocus(false);
            PokerNetworkManager.instance.socket.Emit("__exit_handle");
        }

        socket.Close();
        networkManager.SetActive(false);
        SocketReset();

        SocialGame.instance.StartGame();
    }

    public bool isSwitchForJoinFriends;
    private string newFriendTable;
    private bool newFriendVideo;
    public bool isReserveSeat;

    public void JoinFriendTable(string tableID, bool isVideo, bool joinStatus, bool reserve)
    {
        if (tableID == PokerNetworkManager.instance.tableID)
        {
            FriendandSocialScript.instance.isJoining = false;
            UIManagerScript.instance.lowChipPanel.gameObject.SetActive(true);
            UIManagerScript.instance.lowChipPanelText.text = LanguageManager.Instance.GetTextValue("Sitting on");  //"Sitting on same table!!";
            return;
        }

        isReserveSeat = false;
        if (reserve)
        {
            isReserveSeat = true;
        }

        if (joinStatus)
        {
            isSwitchForJoinFriends = false;
            StartCoroutine(JoinImmediate(tableID, isVideo));
        }
        else
        {
            if (totalPlayersSitting == 1)
            {
                isSwitchForJoinFriends = false;
                StartCoroutine(JoinImmediate(tableID, isVideo));
            }
            else
            {
                isSwitchForJoinFriends = true;
                newFriendTable = tableID;
                newFriendVideo = isVideo;
            }
        }
    }

    IEnumerator JoinImmediate(string tableID, bool isVideo)
    {
        isSwitching = true;
        FriendandSocialScript.instance.isJoining = false;

        PokerNetworkManager.instance.CheckPlayerExist(tableID);
        yield return new WaitForSeconds(1f);

        if (GameSerializeClassesCollection.instance.checkPlayerExistListner != null)
        {
            if (GameSerializeClassesCollection.instance.checkPlayerExistListner.success)
            {
                FriendandSocialScript.instance.check_username = GameSerializeClassesCollection.instance.checkPlayerExistListner.username;
                if (isVideoTable)
                {
                    AgoraInit.instance.LeaveChannel();
                }

                if (isVideo)
                {
                    SocialGame.instance.currentIsVideo = true;
                }
                else
                {
                    SocialGame.instance.currentIsVideo = false;
                }

                if (tableID == PokerNetworkManager.instance.tableID)
                {
                    UIManagerScript.instance.lowChipPanel.gameObject.SetActive(true);
                    UIManagerScript.instance.lowChipPanelText.text = LanguageManager.Instance.GetTextValue("Sitting on");  // "Sitting on same table!!";

                    isSwitching = false;
                }
                else
                {
                    print("SwitchTable1...");
                    newTableID = tableID;
                    UIManagerScript.instance.ResetOnReFocus(false);
                    PokerNetworkManager.instance.socket.Emit("__exit_handle");

                    socket.Close();
                    networkManager.SetActive(false);
                    SocketReset();
                    GameSerializeClassesCollection.instance.disbandTableMessage.errorStatus = true;

                    try
                    {
                        for (int i = 0; i < PlayersGenerator.instance.videoPanelPlayers.Count; i++)
                        {
                            PlayersGenerator.instance.videoPanelPlayers.Remove(PlayersGenerator.instance.videoPanelPlayers[i]);
                        }

                        if (isVideoTable)
                        {
                            for (int i = 0; i < PlayersGenerator.instance.videoPanelsForAllClient.Count; i++)
                            {
                                GameObject objTrans = PlayersGenerator.instance.videoPanelsForAllClient[i].gameObject;
                                PlayersGenerator.instance.videoPanelsForAllClient.Remove(PlayersGenerator.instance.videoPanelsForAllClient[i]);
                                Destroy(objTrans);
                            }
                            PlayersGenerator.instance.videoPanelPlayers.Clear();
                            PlayersGenerator.instance.videoPanelsForAllClient.Clear();
                            Destroy(hostPlayerVideoPanel);
                            GameObject go = new GameObject();
                            go.name = "HostPlayerVideoPanel";
                            go.AddComponent<RawImage>();
                            hostPlayerVideoPanel = go;
                            hostPlayerVideoPanel.transform.SetParent(UIManagerScript.instance.tableParent.parent);
                            hostPlayerVideoPanel.transform.localPosition = Vector3.zero;
                            hostPlayerVideoPanel.transform.localEulerAngles = new Vector3(0, 0, 0);
                            hostPlayerVideoPanel.transform.localScale = new Vector3(1, 1, 1);
                            hostPlayerVideoPanel.transform.GetComponent<RawImage>().color = new Color(1, 1, 1, 0);
                            hostPlayerVideoPanel.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(300f, 180f);
                        }
                    }
                    catch
                    {
                        print("No Host Player Video Panel");
                    }

                    SocialGame.instance.StartGame();
                }
                GameSerializeClassesCollection.instance.checkPlayerExistListner = null;
            }
            else
            {
                FriendandSocialScript.instance.CheckUserPopUp(true);
            }
        }
        else
        {
            FriendandSocialScript.instance.CheckUserPopUp(true);
        }
    }
    public void WaitForSwitch(int wait)
    {
        StartCoroutine(WaitForSwitchCo(wait));
    }

    IEnumerator WaitForSwitchCo(int wait)
    {
        yield return new WaitForSeconds(wait);
        isSwitching = true;
        FriendandSocialScript.instance.isJoining = false;
        StartCoroutine(JoinImmediate(newFriendTable, newFriendVideo));
        GameManagerScript.instance.isSwitchForJoinFriends = false;
    }

    public void InviteResponse(string response)
    {
        UIManagerScript.instance.lowChipPanel.gameObject.SetActive(true);
        UIManagerScript.instance.lowChipPanelText.text = response;
    }

    bool isTableJoin;
    string tableID;
    eTableType tableType;
    bool isVideo;
    string check_username;
    public void FillInviteFriendData(string tableid, eTableType tabletype, bool isvideo, string name)
    {
        tableID = tableid;
        tableType = tabletype;
        isVideo = isvideo;
        check_username = name;
    }

    public void FriendsServerConnection(bool status)
    {
        isTableJoin = status;
        if (ServerChanger.instance.isAWS)
        {
            socket.url = socialPokerUrl;
        }
        if (ServerChanger.instance.isAWS_Live)
        {
            socket.url = socialPokerUrl;
        }
        if (ServerChanger.instance.isAWS_QA)
        {
            socket.url = socialPokerUrl;
        }
        socket.gameObject.SetActive(true);
        networkManager.SetActive(true);
        PokerNetworkManager.instance.FriendsConnectServer(CallbackInviteFriend);
    }

    public void CallbackInviteFriend(bool status)
    {
        Debug.Log("Connected.........Friends server.......");
        if (status)
        {
            PokerNetworkManager.instance.SubscribeInviteFriendsOnLobby();
            if (isTableJoin) // join btn clicked on invite panel comes in lobby , socket need...
            {
                if (PokerNetworkManager.instance != null)
                    PokerNetworkManager.instance.Chk_userName = check_username;
                JoinFriendFromLobby(tableID, isVideo, false);
            }
            else
            {
                FriendandSocialScript.instance.CheckSocketConnection(1);
                FriendandSocialScript.instance.CheckSocketConnection(0);
            }
        }
        else
        {
            Debug.Log("Error .........socket connection.......");
        }
    }

    public void DisconnectFromFriendsServer()
    {
        StartCoroutine(DisconnectFromServerCo());
    }

    IEnumerator DisconnectFromServerCo()
    {
        // PokerNetworkManager.instance.UnSubscribeOnLogin();
        PokerNetworkManager.instance.UnsubscribeInviteFriendsOnLobby();

        yield return new WaitForSeconds(0.5f);
        socket.Close();
        networkManager.SetActive(false);
        SocketReset();

        Debug.Log("DisConnected.........Friends server.......");
    }
    public void JoinFriendFromLobby(string tableID, bool isVideo, bool reserve)
    {
        ClubManagement.instance.loadingPanel.SetActive(true);

        if (SocialGame.instance.isTickImage)
        {
            PlayerPrefs.SetString("disclaimerpanel", "2");
        }

        isReserveSeat = false;
        if (reserve)
        {
            isReserveSeat = true;
        }

        PokerNetworkManager.instance.CheckPlayerExist(tableID);
        StartCoroutine(SendWatchTable(tableID));
    }
    IEnumerator SendWatchTable(string tableID)
    {
        yield return new WaitForSeconds(1f);
        if (GameSerializeClassesCollection.instance.checkPlayerExistListner != null)
        {
            if (GameSerializeClassesCollection.instance.checkPlayerExistListner.success)
            {
                FriendandSocialScript.instance.check_username = GameSerializeClassesCollection.instance.checkPlayerExistListner.username;
                newTableID = tableID;
                if (isVideo)
                {
                    isVideoTable = true;
                }
                else
                {
                    isVideoTable = false;
                }

                Tournamentsocket.gameObject.SetActive(false);
                tournamentManager.SetActive(false);
                PokerNetworkManager.instance.SubscribeToServerEvents();
                isPlayerGenerating = false;
                PokerNetworkManager.instance.startPanel.gameObject.SetActive(false);
                ClubManagement.instance.loadingPanel.SetActive(true);

                PokerNetworkManager.instance.SwitchTableWatchEmitter();
                SocialGame.instance.pokerUICanvas.SetActive(false);

                FriendandSocialScript.instance.isJoining = false;
                GameSerializeClassesCollection.instance.checkPlayerExistListner = null;
            }
            else// in case of lobby...
            {
                FriendandSocialScript.instance.CheckUserPopUp(false);
                DisconnectFromFriendsServer();
            }
        }
        else// in case of lobby...
        {
            FriendandSocialScript.instance.CheckUserPopUp(false);
            DisconnectFromFriendsServer();
        }
    }
 
    public void DeActiveLocalPlayer(GameObject localPlayer)
    {
        totalPlayersSitting--;
        CheckTotalPlayersSitting();

        if (isVideoTable)
        {
            AgoraInit.instance.ResetUserJoinCount(1);
            AgoraInit.instance.MuteUnmuteLocalPlayer(true);
            AgoraInit.instance.MuteUnmuteAllRemotePlayer(true);
            //AgoraInit.instance.MuteVideo(true);
        }

        localPlayer.GetComponent<PokerPlayerController>().PlayerDeactivate(false);
        SpawnManager.instance.Despawn("PlayerPool", localPlayer.transform);
        for (int i = 0; i < UIManagerScript.instance.allOtherBottomPanelBtn.Count; i++)
        {
            UIManagerScript.instance.allOtherBottomPanelBtn[i].SetActive(false);
        }
        UIManagerScript.instance.ActivateSitIn();
    }

    public bool isSwitchForTableSwitch;
    public void WaitForSwitchTable(int wait, int chips)
    {
        StartCoroutine(WaitForSwitchTableCo(wait, chips));
    }

    IEnumerator WaitForSwitchTableCo(int wait, int chips)
    {
        yield return new WaitForSeconds(wait);
        isSwitching = true;
        GameSerializeClassesCollection.instance.switchTableEmitter.switch_player_chips = chips;
        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.switchTableEmitter);
        PokerNetworkManager.instance.socket.Emit("__switch_table", new JSONObject(data));         //Update
        //isSwitchForTableSwitch = false;
    }

    public void WaitForTopUp(int wait)
    {
        StartCoroutine(WaitForTopUpCo(wait));
    }

    IEnumerator WaitForTopUpCo(int wait)
    {
        CalculateChips();

        yield return new WaitForSeconds(wait);
        isTopUp = false;
        string data1 = JsonUtility.ToJson(GameSerializeClassesCollection.instance.topUp);
        socket.Emit("__topup_chip_balance", new JSONObject(data1));           //Update
        print("__topup_chip_balance " + data1);
    }

    public void CalculateChips()
    {
        for (int i = 0; i < playersParent.transform.childCount; i++)
        {
            if (playersParent.transform.GetChild(i).childCount == 2)
            {
                if (playersParent.transform.GetChild(i).GetChild(0).GetComponent<PokerPlayerController>().isLocalPlayer)
                {
                    print(playersParent.transform.GetChild(i).GetChild(0).GetComponent<PokerPlayerController>().player.chips);
                    GameSerializeClassesCollection.instance.topUp.chips = playersParent.transform.GetChild(i).GetChild(0).GetComponent<PokerPlayerController>().player.chips;
                    print(GameSerializeClassesCollection.instance.topUp.chips);

                }
            }
        }
    }

}