using SmartLocalization;
using SocketIO;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PokerNetworkManager : MonoBehaviour
{
    public static PokerNetworkManager instance;
    public SocketIOComponent socket;
    public Transform poolManager;
    public Canvas startPanel;
    public InputField localPlayerTokenInput;
    public Text tableId;
    [Space]
    public string tableID;
    public object entry;

    private bool timedOut;
    public bool canEnter;

    public int subscribeTimes;
    //public int totalPlayersSitting;
    public int localPlayerSeatId;
    public int subscribeTimesOnRejoin;
    public GameObject localPlayer;

    private void Awake()
    {
        instance = this;
        entry = new object();
    }

    private void Start()
    {
        //isSeatReserved = false;
        //SubscribeToServerEvents();
        //Screen.orientation = ScreenOrientation.Landscape;
    }

    #region Subscribe Server Events

    public void SubscribeOnLogin()
    {
        socket.On("__revoke_token", ConnectOnLogin);
    }

    public void UnSubscribeOnLogin()
    {
        socket.Off("__revoke_token", ConnectOnLogin);
    }

    public void SubscribeOnReJoin()
    {
        if (subscribeTimesOnRejoin == 0)
        {
            subscribeTimesOnRejoin++;
            socket.On("__internet_handler", OnResumeAllClients);
            socket.On("__resume_minimise_clients", OnResumeAllClients);
            socket.On("__remove_minimise_player", RemoveMinimizePlayer);
            socket.On("__all_clients_observer", OnAllClientObserver);
            socket.On("__send_observer", OnSendObserver);
        }
    }

    public void SubscribeInviteFriendsOnLobby()
    {
        socket.On("__friends_list", OnGettingFriendList);
        socket.On("__pending_friends_list", OnGettingPendingList);
        socket.On("__invite_friends", GetInviteFromUser);

        socket.On("__join_friends", OnGettingJoinResponse);
        socket.On("__waiting_list", OnGetWaitResponse);
        socket.On("__reserve_seat", OnGettingReserveSeat);
        socket.On("__notify_waiting_list", OnNotifyUserWait);
        socket.On("__enable_details", OnCheckEnableDetail);
        socket.On("__check_player_exist", CheckPlayerExistListner);
    }

    public void UnsubscribeInviteFriendsOnLobby()
    {
        socket.Off("__friends_list", OnGettingFriendList);
        socket.Off("__pending_friends_list", OnGettingPendingList);
        socket.Off("__invite_friends", GetInviteFromUser);

        socket.Off("__join_friends", OnGettingJoinResponse);
        socket.Off("__waiting_list", OnGetWaitResponse);
        socket.Off("__reserve_seat", OnGettingReserveSeat);
        socket.Off("__notify_waiting_list", OnNotifyUserWait);
        socket.Off("__enable_details", OnCheckEnableDetail);
        socket.Off("__check_player_exist", CheckPlayerExistListner);
    }

    public void SubscribeToServerEvents()
    {
        if (subscribeTimes == 0)
        {
            subscribeTimes++;
            socket.On("connected", Connect);
            socket.On("__new_client", OnNewPeer);
            socket.On("__new_client_2", OnNewPeer2);
            socket.On("__all_clients", OnAllPeer);
            socket.On("__low_chip_balance", OnLowChipBalance);
            socket.On("__chip_balance", OnChipBalance);
            socket.On("__game_prepare", OnGamePrepare);
            socket.On("__game_start", OnGameStart);
            socket.On("__new_round_2", OnNewRound);
            socket.On("__new_round_broadcast", OnNewRoundBroadcast);
            socket.On("__next_chance", OnNextChance);
            socket.On("__action_performed", OnActionPerformed);
            socket.On("__next_deal", OnNextDeal);
            socket.On("__next_bet", OnNextBet);
            socket.On("__round_end", OnHandEnd);
            socket.On("__topup_chip_balance", OnTopUpChipBalance);
            socket.On("top_up_times_up", TopUpTimesUp);
            socket.On("__left_2", OnPlayerLeft2);
            socket.On("__left", OnPlayerLeft);
            socket.On("__current_left_player", OnCurrentPlayerLeft);
            socket.On("__me_left", MeLeft);
            socket.On("__start_reload", OnStartReload);
            socket.On("__seat_occupied", SeatOccupied);
            socket.On("__game_over", GameOver);
            socket.On("__disband_table", OnDisbandTableMessage);
            socket.On("__new_round", NewRound);
            socket.On("__buyin_auth_request", OnBuyInAuthRequest);
            socket.On("__buyin_auth_pending", OnBuyInAuthPending);
            socket.On("__error_table_size", OnErrorTableSize);
            socket.On("limit_win_player", LimitWinPlayer);
            socket.On("__social_game_start", OnSocialGame);
            socket.On("__social_table_entry", OnSocialTableEntry);
            socket.On("__st_throwables", Throwable);
            socket.On("__user_chip_balance", AddChips);
            socket.On("__chat", ChatListener);
            socket.On("__chat_message", ChatHistoryListener);
            socket.On("__latest_state", OnLatestState);
            socket.On("__add_manual_topup", OnManualTopUp);
            socket.On("__send_friend_request", OnSendingFriendRequest);
            socket.On("__friends_list", OnGettingFriendList);
            socket.On("__pending_friends_list", OnGettingPendingList);
            socket.On("__invite_friends", GetInviteFromUser);
            socket.On("__join_friends", OnGettingJoinResponse);
            socket.On("__waiting_list", OnGetWaitResponse);
            socket.On("__reserve_seat", OnGettingReserveSeat);
            socket.On("__notify_waiting_list", OnNotifyUserWait);
            socket.On("__st_invite_friends", OnGettingInviteResponse);

            socket.On("__stand_up_player", StandUpPlayerListner);
            socket.On("__switch_table", SwitchTables);
            socket.On("__check_st_invite_friends", OnVerifySeatReserved);

            socket.On("__last_player_standing", LastPlayerLeft);

            socket.On("__st_revoke_token", TokenRevoked);


            socket.On("__check_player_exist", CheckPlayerExistListner);
            socket.On("__enable_details", OnCheckEnableDetail);
        }
    }

    public void UnSubscribeToServerEvents()
    {
        subscribeTimes--;
        if (subscribeTimes < 0)
        {
            subscribeTimes = 0;
        }
        print("UNSuB......");
        socket.Off("connected", Connect);
        socket.Off("__new_client", OnNewPeer);
        socket.Off("__new_client_2", OnNewPeer2);
        socket.Off("__all_clients", OnAllPeer);
        socket.Off("__low_chip_balance", OnLowChipBalance);
        socket.Off("__chip_balance", OnChipBalance);
        socket.Off("__game_prepare", OnGamePrepare);
        socket.Off("__game_start", OnGameStart);
        socket.Off("__new_round_2", OnNewRound);
        socket.Off("__new_round_broadcast", OnNewRoundBroadcast);
        socket.Off("__next_chance", OnNextChance);
        socket.Off("__action_performed", OnActionPerformed);
        socket.Off("__next_deal", OnNextDeal);
        socket.Off("__next_bet", OnNextBet);
        socket.Off("__round_end", OnHandEnd);
        socket.Off("__topup_chip_balance", OnTopUpChipBalance);
        socket.Off("top_up_times_up", TopUpTimesUp);
        socket.Off("__left_2", OnPlayerLeft2);
        socket.Off("__left", OnPlayerLeft);
        socket.Off("__current_left_player", OnCurrentPlayerLeft);
        socket.Off("__me_left", MeLeft);
        socket.Off("__start_reload", OnStartReload);
        socket.Off("__seat_occupied", SeatOccupied);
        socket.Off("__game_over", GameOver);
        socket.Off("__disband_table", OnDisbandTableMessage);
        socket.Off("__new_round", NewRound);
        socket.Off("__buyin_auth_request", OnBuyInAuthRequest);
        socket.Off("__buyin_auth_pending", OnBuyInAuthPending);
        socket.Off("__error_table_size", OnErrorTableSize);
        //socket.Off("__resume_minimise_clients", OnResumeAllClients);
        //socket.Off("__internet_handler", OnResumeAllClients);
        socket.Off("limit_win_player", LimitWinPlayer);
        socket.Off("__social_game_start", OnSocialGame);
        socket.Off("__social_table_entry", OnSocialTableEntry);
        socket.Off("__st_throwables", Throwable);
        //socket.Off("__remove_minimise_player", RemoveMinimizePlayer);
        socket.Off("__user_chip_balance", AddChips);
        socket.Off("__chat", ChatListener);
        socket.Off("__chat_message", ChatHistoryListener);
        socket.Off("__latest_state", OnLatestState);
        socket.Off("__add_manual_topup", OnManualTopUp);
        socket.Off("__send_friend_request", OnSendingFriendRequest);
        socket.Off("__friends_list", OnGettingFriendList);
        socket.Off("__pending_friends_list", OnGettingPendingList);
        socket.Off("__invite_friends", GetInviteFromUser);
        socket.Off("__join_friends", OnGettingJoinResponse);
        socket.Off("__waiting_list", OnGetWaitResponse);
        socket.Off("__reserve_seat", OnGettingReserveSeat);
        socket.Off("__notify_waiting_list", OnNotifyUserWait);
        socket.Off("__st_invite_friends", OnGettingInviteResponse);

        socket.Off("__stand_up_player", StandUpPlayerListner);
        socket.Off("__switch_table", SwitchTables);
        socket.Off("__check_st_invite_friends", OnVerifySeatReserved);

        socket.Off("__last_player_standing", LastPlayerLeft);

        socket.Off("__st_revoke_token", TokenRevoked);

        socket.Off("__check_player_exist", CheckPlayerExistListner);
        socket.Off("__enable_details", OnCheckEnableDetail);


        if (subscribeTimesOnRejoin >= 1)
        {
            subscribeTimesOnRejoin--;
            socket.Off("__resume_minimise_clients", OnResumeAllClients);
            socket.Off("__internet_handler", OnResumeAllClients);
            socket.Off("__remove_minimise_player", RemoveMinimizePlayer);
            socket.Off("__internet_handler", OnResumeAllClients);
            socket.Off("__resume_minimise_clients", OnResumeAllClients);
            socket.Off("__remove_minimise_player", RemoveMinimizePlayer);
            socket.Off("__all_clients_observer", OnAllClientObserver);
            socket.Off("__send_observer", OnSendObserver);
        }

    }
    #endregion


    #region SOCIAL POKER SERVER CONNECTION  

    //.....................................................SOCIAL POKER SERVER CONNECTION...........................................................................................................//

    public void EnterInPokerSocialTable(string data)
    {
        SubscribeToServerEvents();
        GameManagerScript.instance.isPlayerGenerating = false;
        startPanel.gameObject.SetActive(false);
        ClubManagement.instance.loadingPanel.SetActive(true);

        if (!GameSerializeClassesCollection.instance.disbandTableMessage.errorStatus)
        {
            StartCoroutine(ConnectSocialPoker(data));
        }
        else
        {
            print("SwitchTable...");
            StartCoroutine(ConnectToServerOnSwitchTable());
        }
    }

    public IEnumerator ConnectSocialPoker(string data)
    {
        StartCoroutine(TimeoutChecker(4));              //Update
        while (true)
        {
            socket.Connect();
            if (socket.wsConnected)
            {
                break;
            }
            else if (timedOut)
            {
                UnSubscribeToServerEvents();
                StopConnectToServer();
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        //yield return new WaitForSeconds(2f);
        yield return new WaitForSeconds(0.1f);

        if (socket.wsConnected && !ApiHitScript.instance.isLogin)
        {
            print("Socket 1");
            print("__social_game_start Emitter...." + data);
            socket.Emit("__social_game_start", new JSONObject(data));
        }

        if (socket.wsConnected && ApiHitScript.instance.isLogin)
        {
            ClubManagement.instance.loadingPanel.SetActive(true);
            print("Socket 2");
            print("__revoke_token Emitter...." + data);
            socket.Emit("__revoke_token", new JSONObject(data));
            StartCoroutine(ApiHitScript.instance.DisconnectFromServer());
        }
    }

    public IEnumerator ConnectToServerOnSwitchTable()
    {
        UIManagerScript.instance.loadingPanel.SetActive(true);
        UIManagerScript.instance.loadingPanel.transform.GetChild(1).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("Redirecting");//"Redirecting...";
        yield return new WaitForSeconds(1f);
        StartCoroutine(TimeoutChecker(6));
        while (true)
        {
            socket.Connect();
            if (socket.wsConnected)
            {
                break;
            }
            else if (timedOut)
            {
                UnSubscribeToServerEvents();
                StopConnectToServer();
                UIManagerScript.instance.TableToPokerUI(0);
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);
        if (socket.wsConnected)
        {
            SwitchTableWatchEmitter();
        }
    }

    public void SwitchTableWatchEmitter()
    {
        if (GameManagerScript.instance.isSwitchForTableSwitch)
        {
            GameManagerScript.instance.isSwitchForTableSwitch = false;
            SendChipsOnSwitchTable();
        }

        //GameSerializeClassesCollection.instance.observeTable = JsonUtility.FromJson<GameSerializeClassesCollection.ObserveTableValues>(data);
        GameSerializeClassesCollection.instance.observeTable.ticket = GameManagerScript.instance.newTableID;
        tableID = GameManagerScript.instance.newTableID;
        GameSerializeClassesCollection.instance.playerData.ticket = GameSerializeClassesCollection.instance.observeTable.ticket;

        GameSerializeClassesCollection.instance.observeTable.token = Communication.instance.playerToken;
        GameSerializeClassesCollection.instance.observeTable.isHuman = true;
        GameSerializeClassesCollection.instance.observeTable.reserve = false;
        if (GameManagerScript.instance.isReserveSeat)
        {
            GameSerializeClassesCollection.instance.observeTable.reserve = true;
        }
        string data2 = JsonUtility.ToJson(GameSerializeClassesCollection.instance.observeTable);
        print("__switch_watch_table Emitter: " + data2);
        socket.Emit("__watch_table", new JSONObject(data2));

        canEnter = true;
        StartCoroutine(DenyEntry());
    }

    void SendChipsOnSwitchTable()
    {
        GameSerializeClassesCollection.instance.switchTableEmitter.tableNumber = GameManagerScript.instance.newTableID;
        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.switchTableEmitter);
        print("__assign_chips" + data);
        socket.Emit("__assign_chips", new JSONObject(data));
    }

    void OnSocialTableEntry(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("OnSocialTableEntry Listner: " + data);
        GameSerializeClassesCollection.instance.onEnterInSocialGame = JsonUtility.FromJson<GameSerializeClassesCollection.OnEnterInSocialGame>(data);

        if (GameSerializeClassesCollection.instance.onEnterInSocialGame.error && GameSerializeClassesCollection.instance.onEnterInSocialGame.tokenMissing)
        {
            UnSubscribeToServerEvents();
            socket.Emit("__exit_handle");
            StartCoroutine(SignOut());
        }
        else if (GameSerializeClassesCollection.instance.onEnterInSocialGame.error && GameSerializeClassesCollection.instance.onEnterInSocialGame.paramMissing)
        {
            print("Enter123........");
            GameSerializeClassesCollection.instance.enterInSocialGame.stake_type = "low";
            GameSerializeClassesCollection.instance.enterInSocialGame.buyin = 10000;
            GameSerializeClassesCollection.instance.enterInSocialGame.small_blind = 500;
            GameSerializeClassesCollection.instance.enterInSocialGame.big_blind = 1000;
            GameSerializeClassesCollection.instance.enterInSocialGame.video = true;
            GameSerializeClassesCollection.instance.enterInSocialGame.token = Communication.instance.playerToken;
            string data1 = JsonUtility.ToJson(GameSerializeClassesCollection.instance.enterInSocialGame);
            print(data1);
            socket.Emit("__social_game_start", new JSONObject(data1));
        }
        else if (GameSerializeClassesCollection.instance.onEnterInSocialGame.error && !GameSerializeClassesCollection.instance.onEnterInSocialGame.paramMissing)
        {
            canEnter = false;
        }
        if (GameSerializeClassesCollection.instance.onEnterInSocialGame.error && GameSerializeClassesCollection.instance.onEnterInSocialGame.paramMissing || GameSerializeClassesCollection.instance.onEnterInSocialGame.error && !GameSerializeClassesCollection.instance.onEnterInSocialGame.paramMissing)
        {
            StartCoroutine(DenyEntry());
        }
    }

    IEnumerator SignOut()
    {
        yield return new WaitForSeconds(0.5f);
        socket.Close();
        GameManagerScript.instance.networkManager.SetActive(false);
        GameManagerScript.instance.SocketReset();
        Uimanager.instance.SignOut();
        if (GameManagerScript.instance.activeTable != null)
        {
            GameManagerScript.instance.activeTable.SetActive(false);
        }
    }

    void OnSocialGame(SocketIOEvent socketIOEvent)
    {
        SocialGame.instance.pokerUICanvas.SetActive(false);

        string data = socketIOEvent.data.ToString();
        GameSerializeClassesCollection.instance.observeTable = JsonUtility.FromJson<GameSerializeClassesCollection.ObserveTableValues>(data);
        tableID = GameSerializeClassesCollection.instance.observeTable.ticket;
        GameSerializeClassesCollection.instance.playerData.ticket = GameSerializeClassesCollection.instance.observeTable.ticket;
        print("__social_game_start Listner: " + data);
        GameSerializeClassesCollection.instance.observeTable.token = Communication.instance.playerToken;
        GameSerializeClassesCollection.instance.observeTable.isHuman = true;
        GameSerializeClassesCollection.instance.observeTable.user_image = ApiHitScript.instance.updatedUserImageUrl;
        GameSerializeClassesCollection.instance.observeTable.reserve = false;
        if (GameManagerScript.instance.isReserveSeat)
        {
            GameSerializeClassesCollection.instance.observeTable.reserve = true;
        }

        string data2 = JsonUtility.ToJson(GameSerializeClassesCollection.instance.observeTable);
        print("__watch_table Emitter: " + data2);
        socket.Emit("__watch_table", new JSONObject(data2));

        canEnter = true;
        StartCoroutine(DenyEntry());
    }


    IEnumerator DenyEntry()
    {
        ClubManagement.instance.loadingPanel.SetActive(true);
        yield return new WaitForSeconds(0.1f);

        if (canEnter)
        {
            GameManagerScript.instance.InitlializeOnStart();

            Table.instance.tableType.transform.parent.gameObject.SetActive(true);
            Table.instance.blind.transform.parent.gameObject.SetActive(true);
            string sBlind = GameManagerScript.instance.KiloFormat((int)GameSerializeClassesCollection.instance.enterInSocialGame.small_blind);
            string bBlind = GameManagerScript.instance.KiloFormat((int)GameSerializeClassesCollection.instance.enterInSocialGame.big_blind);
            Table.instance.blind.text = sBlind + "/" + bBlind;

            yield return new WaitForSeconds(0.5f);
            UIManagerScript.instance.loadingPanel.SetActive(true);
            UIManagerScript.instance.loadingPanel.transform.GetChild(1).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("Setting Up");//"Setting Up...";

            UIManagerScript.instance.tournamentInfoButton.SetActive(false);


            yield return new WaitForSeconds(5f);
            StartCoroutine(GameManagerScript.instance.Internet());
        }
        else
        {
            Cashier.instance.toastMsgPanel.SetActive(true);
            Cashier.instance.toastMsg.text = "Sorry, you have not enough chips please add more chips";
            UnSubscribeToServerEvents();
            socket.Emit("__exit_handle");

            yield return new WaitForSeconds(0.5f);
            socket.Close();
            GameManagerScript.instance.networkManager.SetActive(false);
            GameManagerScript.instance.SocketReset();
        }

        GameManagerScript.instance.isSwitching = false;
        ClubManagement.instance.loadingPanel.SetActive(false);

    }

    //................................................................................................................................................................//
    #endregion


    #region Regular Table Server Connection Methods  

    void Connect(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("connected  : " + data);
    }

    // This function emit __watch_table 
    public void ObserveTable()
    {
        SubscribeToServerEvents();
        GameManagerScript.instance.isPlayerGenerating = false;
        startPanel.gameObject.SetActive(false);
        //.............................................FOR DIRECT ENTRY WITH TOKEN...........................................................................// 

        if (GameManagerScript.instance.isDirectLogIn)
        {
            GameSerializeClassesCollection.instance.observeTable.token = localPlayerTokenInput.text;
            GameSerializeClassesCollection.instance.observeTable.ticket = tableID.ToString();              //"192255";//"787871"; //511223  //220781 656048
            TableWebAPICommunication.instance.playerToken = localPlayerTokenInput.text;
            GameSerializeClassesCollection.instance.playerData.token = localPlayerTokenInput.text;
            GameSerializeClassesCollection.instance.playerData.ticket = tableID.ToString();
        }

        //..................................................................................................................................................// 


        GameSerializeClassesCollection.instance.observeTable.port = "8080";
        GameSerializeClassesCollection.instance.observeTable.isHuman = true;
        GameSerializeClassesCollection.instance.observeTable.table_type = "regulartable";

        //.............................................FOR ENTRY FROM THE CLUB TABLE........................................................................// 
        if (!GameManagerScript.instance.isDirectLogIn)
        {
            TableWebAPICommunication.instance.playerToken = Communication.instance.playerToken;
            GameSerializeClassesCollection.instance.observeTable.token = Communication.instance.playerToken;
            GameSerializeClassesCollection.instance.observeTable.ticket = ClubManagement.instance.currentSelectedTableId;

            GameSerializeClassesCollection.instance.playerData.token = Communication.instance.playerToken;
            GameSerializeClassesCollection.instance.playerData.ticket = ClubManagement.instance.currentSelectedTableId;

            print("Startttttt Time " + ClubManagement.instance.currentSelectedTableStartTime);
            print("ENNNNDDDDDD Time " + ClubManagement.instance.currentSelectedTableEndTime);
        }
        //..................................................................................................................................................// 
        print("ClubManagement.instance.currentSelectedTableBotEnabled.........." + ClubManagement.instance.currentSelectedTableBotEnabled);
        print("ClubManagement.instance.currentSelectedTableBots.........." + ClubManagement.instance.currentSelectedTableBots);
        if (ClubManagement.instance.currentSelectedTableBotEnabled)
        {
            StartCoroutine("BotConnect");
        }

        StartCoroutine("ConnectToServer");
    }

    public IEnumerator BotConnect()
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < ClubManagement.instance.currentSelectedTableBots; i++)
        {
            socket.Connect();

            GameSerializeClassesCollection.instance.ticket.ticket = ClubManagement.instance.currentSelectedTableId;
            string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.ticket);
            print("__bots_socket " + data);
            yield return new WaitForSeconds(1f);

            socket.Emit("__bots_socket", new JSONObject(data));

            yield return new WaitForSeconds(1f);
            socket.Emit("disconnect");
            socket.Close();
            yield return new WaitForSeconds(1f);
        }
    }

    void StopConnectToServer()
    {
        StopCoroutine("ConnectSocialPoker");
        if (!GameManagerScript.instance.isDirectLogIn)
        {
            ClubManagement.instance.loadingPanel.SetActive(false);
            Cashier.instance.toastMsgPanel.SetActive(true);
            Cashier.instance.toastMsg.text = "Oops something went wrong. Please try again.";
        }
    }


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

    // This function emit __watch_table 
    IEnumerator ConnectToServer()
    {

        if (!GameManagerScript.instance.isDirectLogIn)
        {
            ClubManagement.instance.loadingPanel.SetActive(true);
        }
        yield return new WaitForSeconds(ClubManagement.instance.currentSelectedTableBots * 3.2f);
        StartCoroutine(TimeoutChecker(10));
        while (true)
        {
            socket.Connect();
            if (socket.wsConnected/* && GameManagerScript.instance.fastInternet*/)
            {
                break;
            }
            else if (timedOut)
            {
                StopConnectToServer();
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        GameManagerScript.instance.isObserver = true;
        yield return new WaitForSeconds(1f);
        //.............................................FOR ENTRY FROM THE CLUB TABLE........................................................................// 
        if (!GameManagerScript.instance.isDirectLogIn)
        {
            Uimanager.instance.MenuCanvas.SetActive(false);
            Table.instance.tableNumber.text = ClubManagement.instance.currentSelectedTableId;
            Table.instance.tableType.text = ClubManagement.instance.currentSelectedTableType;

            //Table.instance.blind.text = ClubManagement.instance.currentSelectedTableBlinds;

            GameManagerScript.instance.commandTimer = ClubManagement.instance.currentSelectedTableActionTime;
            //.......................................... Uncomment this for table Variation ................................................................//
            GameManagerScript.instance.isVideoTable = ClubManagement.instance.currentSelectedTableIsVideoMode;


            //.......................................... For Buy In Auth Panel................................................................//
            //if (GameManagerScript.instance.isVideoTable)
            //{
            //    MailBoxScripts._instance.BuyInMain = MailBoxScripts._instance.video_Nonvideo_Object[3];
            //    MailBoxScripts._instance.buyinContent = MailBoxScripts._instance.video_Nonvideo_Object[4];
            //    MailBoxScripts._instance.buyinPanel = MailBoxScripts._instance.video_Nonvideo_Object[5];
            //}
            //else
            //{
            //    MailBoxScripts._instance.BuyInMain = MailBoxScripts._instance.video_Nonvideo_Object[0];
            //    MailBoxScripts._instance.buyinContent = MailBoxScripts._instance.video_Nonvideo_Object[1];
            //    MailBoxScripts._instance.buyinPanel = MailBoxScripts._instance.video_Nonvideo_Object[2];
            //}
        }
        //..................................................................................................................................................// 


        yield return new WaitForSeconds(0.5f);
        GameManagerScript.instance.InitlializeOnStart();
        yield return new WaitForSeconds(1f);
        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.observeTable);
        socket.Emit("__watch_table", new JSONObject(data));
        print("__watch_table " + data);

        UIManagerScript.instance.observingPanel.gameObject.SetActive(true);
        //if (GameManagerScript.instance.isVideoTable)
        //{
        //    UIManagerScript.instance.observingPanel.gameObject.SetActive(true);
        //}
        //else
        //{
        //    UIManagerScript.instance.observingPanelNonVideo.gameObject.SetActive(true);
        //}
        //.............................................FOR ENTRY FROM THE CLUB TABLE........................................................................// 
        if (!GameManagerScript.instance.isDirectLogIn)
        {
            ClubManagement.instance.loadingPanel.SetActive(false);
        }

        //..................................................................................................................................................// 
        //GameManagerScript.instance.checkInternet = true;
        //StartCoroutine(GameManagerScript.instance.InternetCheckCoroutine());
        //GameManagerScript.instance.InternetChecking();
    }

    public void JoinTable(int seatId)
    {
        GameManagerScript.instance.localSeatIDDummy = seatId;
        GameSerializeClassesCollection.instance.chipBalanceBuyInAuth.token = GameSerializeClassesCollection.instance.playerData.token;
        GameSerializeClassesCollection.instance.chipBalanceBuyInAuth.ticket = GameSerializeClassesCollection.instance.playerData.ticket;
        GameSerializeClassesCollection.instance.chipBalanceBuyInAuth.isHuman = true;
        GameSerializeClassesCollection.instance.chipBalanceBuyInAuth.table_type = "regulartable";
        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.chipBalanceBuyInAuth);
        print("__chip_balance " + data);

        if (ClubManagement.instance.currentSelectedTableIsBuyInAuth)
        {

            socket.Emit("__buyin_auth", new JSONObject(data));
        }
        else
        {

            socket.Emit("__chip_balance", new JSONObject(data));
        }
    }

    public void SitOnTable(float initial_chips, bool autoBuy, float percent)
    {
        UIManagerScript.instance.observingPanel.gameObject.SetActive(false);
        GameSerializeClassesCollection.instance.playerData.seatId = GameManagerScript.instance.localSeatIDDummy;
        GameSerializeClassesCollection.instance.playerData.port = "8080";
        GameSerializeClassesCollection.instance.playerData.isHuman = true;
        GameSerializeClassesCollection.instance.playerData.table_type = "regulartable";
        GameSerializeClassesCollection.instance.playerData.initial_chips = (int)initial_chips;
        //GameSerializeClassesCollection.instance.playerData.auto_rebuy = testAutobuy;
        //GameSerializeClassesCollection.instance.playerData.auto_rebuy_percentage = testBuyPercentage;

        GameSerializeClassesCollection.instance.playerData.auto_rebuy = autoBuy;
        GameSerializeClassesCollection.instance.playerData.auto_rebuy_percentage = (int)percent;

        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.playerData);
        print("Send " + data);
        JoinTableEmitter(data);
    }
    #endregion


    #region Listener Methods 

    public void OnVerifySeatReserved(SocketIOEvent socketIOEvent)
    {
        Debug.Log(">>>>" + "__check_st_invite_friends" + socketIOEvent.data.ToString());

        GameSerializeClassesCollection.instance.chkreserveInstance = JsonUtility.FromJson<GameSerializeClassesCollection.CheckForReserveSeatInviteClass>(socketIOEvent.data.ToString());

        if (GameSerializeClassesCollection.instance.chkreserveInstance.success)
        {
            string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance._st_inviteInstance);
            socket.Emit("__st_invite_friends", new JSONObject(data));
        }
        else
        {
            GameManagerScript.instance.InviteResponse("Seat already reserved!!");
        }
    }

    // Response After click on friend Invite btn.. 
    public void OnGettingInviteResponse(SocketIOEvent socketIOEvent)
    {
        Debug.Log(">>>>" + "__st_invite_friends" + socketIOEvent.data.ToString());

        //    Debug.Log(">>>current user  : " + currentInvitee.name);
        //  currentInvitee.GetComponent<MyFriendListPrefabHandler>().ResponseAfterInviteBtn(socketIOEvent.data.ToString());

        // Debug.Log(">>>>" + "__st_invite_friends" + "current user is null");
        FriendandSocialScript.instance.OnGettingResponseInvitee(socketIOEvent.data.ToString());

    }

    //getting notification for wait  user...
    public void OnNotifyUserWait(SocketIOEvent socketIOEvent)
    {
        Debug.Log(">>>>" + "__notify_waiting_list" + socketIOEvent.data.ToString());

        FriendandSocialScript.instance.ShowWaitInvitePopUp(socketIOEvent.data.ToString(), false);
    }


    //Response after seat reserved...
    public void OnGettingReserveSeat(SocketIOEvent socketIOEvent)
    {
        Debug.Log(">>>>" + "__reserve_seat" + socketIOEvent.data.ToString());
        GameSerializeClassesCollection.instance.reserveResponseInsance = JsonUtility.FromJson<GameSerializeClassesCollection.ReserveSeatResponseClass>(socketIOEvent.data.ToString());
        if (GameSerializeClassesCollection.instance.reserveResponseInsance.success)
        {
            FriendandSocialScript.instance.isSeatReserved = true;
            GameManagerScript.instance.JoinFriendTable(currTicket, currIsVideo, false, true);
        }
        else
        {
            FriendandSocialScript.instance.isSeatReserved = false;

            Debug.Log(">>Seat is not reserve success :  false");
            if (isJoinorInvite)
            {
                FriendandSocialScript.instance.isJoining = false;
                GameManagerScript.instance.InviteResponse("Seat not Reserved.Please try again");
                UIManagerScript.instance.menu_panelUIButton.SetActive(true);
            }
            else
            {
                GameManagerScript.instance.InviteResponse("Error!! Seat not Reserved");
                UIManagerScript.instance.menu_panelUIButton.SetActive(true);
            }
        }
    }

    //OnGetting waitlist response...
    public void OnGetWaitResponse(SocketIOEvent socketIOEvent)
    {
        Debug.Log(">>>>" + "__waiting_list" + socketIOEvent.data.ToString());
        
        GameSerializeClassesCollection.instance.waitListResInstance = JsonUtility.FromJson<GameSerializeClassesCollection.WaitingListResponse>(socketIOEvent.data.ToString());

        if (GameManagerScript.instance.activeTable != null)
        {
            if (GameManagerScript.instance.NonVideoTable.activeInHierarchy)
            {
                FriendandSocialScript.instance.tableType = eTableType.nonvideo;
            }
            else if (GameManagerScript.instance.videoTable.activeInHierarchy)
            {
                FriendandSocialScript.instance.tableType = eTableType.video;
            }
            else
            {
                FriendandSocialScript.instance.tableType = eTableType.lobby;
            }
        }
        else
        {
            FriendandSocialScript.instance.tableType = eTableType.lobby;
            Debug.Log(">> Table type not defined");
        }

        if (!GameSerializeClassesCollection.instance.waitListResInstance.errorStatus)
        {
            //success

            if (FriendandSocialScript.instance.tableType == eTableType.nonvideo)
            {
                GameManagerScript.instance.InviteResponse("Waiting List Success!!");

            }
            else if (FriendandSocialScript.instance.tableType == eTableType.video)
            {
                GameManagerScript.instance.InviteResponse("Waiting List Success!!");
            }
            else if (FriendandSocialScript.instance.tableType == eTableType.lobby)
            {
                Cashier.instance.toastMsgPanel.SetActive(true);
                Cashier.instance.toastMsg.text = "Waiting List Success!!";
            }

            if (currentWaitListUser != null)
                PokerSceneManagement.instance.CurrentWaitListUser.Add(currentWaitListUser.GetComponent<MyFriendListPrefabHandler>().clientID);
            else
            {
                Debug.Log("currentwaitlistuser is empty...");

            }
        }
        else
        {
            //fail
            if (FriendandSocialScript.instance.tableType == eTableType.nonvideo)
            {
                GameManagerScript.instance.InviteResponse("Error!! Waiting List");

            }
            else if (FriendandSocialScript.instance.tableType == eTableType.video)
            {
                GameManagerScript.instance.InviteResponse("Error!! Waiting List");
            }
            else if (FriendandSocialScript.instance.tableType == eTableType.lobby)
            {
                Cashier.instance.toastMsgPanel.SetActive(true);
                Cashier.instance.toastMsg.text = "Error!! Waiting List";
            }
            if (currentWaitListUser != null)
                currentWaitListUser.GetComponent<MyFriendListPrefabHandler>().joinBtn.SetActive(true);
            else
            {
                Debug.Log("currentwaitlistuser is empty...");
            }
        }
    }

    //After click on online friend join btn..
    public void OnGettingJoinResponse(SocketIOEvent socketIOEvent)
    {
        Debug.Log(">>>>" + "__join_friends" + socketIOEvent.data.ToString());
        if (currentJoiner != null)
        {
            currentJoiner.GetComponent<MyFriendListPrefabHandler>().ResponseAfterJoinBtn(socketIOEvent.data.ToString());
            Debug.Log(">>>>" + "__join_friends" + "current user is null : inviter case..");
        }
        else
        {

            Debug.Log(">>>>" + "__join_friends" + "current user is null : invitee case..");
        }
    }

    // sendingFriendRequest..

    void OnSendingFriendRequest(SocketIOEvent socketIOEvent)
    {

        print(">>>>>>__send_friend_request" + socketIOEvent.data.ToString());

      //  GameSerializeClassesCollection.instance.addfrindRequest = JsonUtility.FromJson<GameSerializeClassesCollection.AddingFriendParameters>(socketIOEvent.data.ToString());

        FriendandSocialScript.instance.ShowAddFriendPopUp(socketIOEvent.data.ToString(), null);

    }


    // __all_clients listener
    void OnAllPeer(SocketIOEvent socketIOEvent)
    {
        UIManagerScript.instance.waitOtherPlayerPanel.gameObject.SetActive(false);
        UIManagerScript.instance.mttSideInfoPanelSymbol.SetActive(false);

        string data = socketIOEvent.data.ToString();
        print("__all _ clients : " + data);
        GameSerializeClassesCollection.AllPlayers allPlayers = JsonUtility.FromJson<GameSerializeClassesCollection.AllPlayers>(data);

        //if (ClubManagement.instance.currentSelectedTableSize == allPlayers.players.Length && !GameManagerScript.instance.isSocialPokerDev && !GameManagerScript.instance.isSocialPokerStage)
        //{
        //    GameManagerScript.instance.TableSetEmptytrue();
        //}
        try
        {
            GameManagerScript.instance.TableSetEmptytrue();
        }
        catch
        {
            print("Error in TableSetEmptytrue");
        }
        Table.instance.table.status = allPlayers.tableStatus;
        try
        {
            print("Length:" + allPlayers.players.Length);
            for (int i = 0; i < allPlayers.players.Length; i++)
            {
                if (AccessGallery.instance.profileName[0].text == allPlayers.players[i].playerName)                     // LOCAL PLAYER..........
                {
                    //GameSerializeClassesCollection.instance.newLocalPlayer.player.playerName = allPlayers.players[i].playerName;
                    //GameSerializeClassesCollection.instance.newLocalPlayer.player.seatId = allPlayers.players[i].seatId;
                    //GameSerializeClassesCollection.instance.newLocalPlayer.player.clientId = allPlayers.players[i].clientId;
                    //GameSerializeClassesCollection.instance.newLocalPlayer.player.initialChips = allPlayers.players[i].initialChips;
                    //GameSerializeClassesCollection.instance.newLocalPlayer.player.initialChips = allPlayers.players[i].initialChips;
                    //GameManagerScript.instance.localSeatIDDummy = GameSerializeClassesCollection.instance.newLocalPlayer.player.seatId;
                    ////#if !UNITY_EDITOR
                    //if (GameManagerScript.instance.isVideoTable)
                    //{
                    //    GameManagerScript.instance.OnVideoEngine(GameSerializeClassesCollection.instance.playerData.ticket, GameSerializeClassesCollection.instance.newLocalPlayer.player.clientId);
                    //}
                    ////#endif
                    //GameManagerScript.instance.chairAnimForLocalPlayer = false;
                    //PlayersGenerator.instance.InstantiateLocalPlayer(GameSerializeClassesCollection.instance.newLocalPlayer);

                }
                else
                {
                    PlayersGenerator.instance.InstantiateAllOtherPlayer(allPlayers.players[i]);
                }
            }
        }
        catch
        {
            print("no players");
        }

        try
        {
            if (allPlayers.basicData.players.Length != 0)
            {
                CardShuffleAnimation.instance.isAnimationComplete = true;
                GameManagerScript.instance.AllBasicDataUpdate(allPlayers.basicData);

                GameManagerScript.instance.UpdateTable(allPlayers.basicData.table);
                Table.instance.UpdateTotalPot();
            }
        }
        catch
        {
            CardShuffleAnimation.instance.isAnimationComplete = true;
            print("no basic data");
        }
        try
        {
            if (allPlayers.checkActionPlayer)
            {
                GameManagerScript.instance.minBet = allPlayers.currentActionPlayerDetails.player.minBet;
                Table.instance.roundNameFromGameClass = allPlayers.currentActionPlayerDetails.game.roundName;
                PlayerActionManagement.instance.ShowPlayerCommands(allPlayers.currentActionPlayerDetails.player, 0.5f);
            }
        }
        catch
        {
            print("Error in current Action PlayerDetails");
        }

    }

    // __new_client listener
    void OnNewPeer(SocketIOEvent socketIOEvent)
    {
        GameManagerScript.instance.isSwitching = false;
        UIManagerScript.instance.observingUI.SetActive(false);
        GameManagerScript.instance.isObserver = false;

        string data = socketIOEvent.data.ToString();
        print("__new _ client: " + data);
        GameSerializeClassesCollection.Players localPlayer = JsonUtility.FromJson<GameSerializeClassesCollection.Players>(data);
        localPlayerSeatId = localPlayer.player.seatId;

        if (tableID == localPlayer.player.tableNumber)
        {
            lock (entry)
            {
                PlayersGenerator.instance.InstantiateLocalPlayer(localPlayer, false);
            }
        }
        else
        {
            GameSerializeClassesCollection.instance.onError.token = Communication.instance.playerToken;
            GameSerializeClassesCollection.instance.onError.eventname = " __new_client";
            GameSerializeClassesCollection.instance.onError.message = "Wrong table Number ";
            string data2 = JsonUtility.ToJson(GameSerializeClassesCollection.instance.onError);
            print(data2);
            socket.Emit("custom_error", new JSONObject(data2));
        }
        UIManagerScript.instance.loadingPanel.SetActive(false);
        UIManagerScript.instance.loadingPanel.transform.GetChild(1).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("Reconnecting"); //"Reconnecting...";

        StartCoroutine(SendInternetSpeed());
    }

    // __new_client_2 listener
    void OnNewPeer2(SocketIOEvent socketIOEvent)
    {
        UIManagerScript.instance.waitOtherPlayerPanel.gameObject.SetActive(false);
        string data = socketIOEvent.data.ToString();
        print("__new _ client _2: " + data);
        lock (entry)
        {
            GameSerializeClassesCollection.Players localPlayer = JsonUtility.FromJson<GameSerializeClassesCollection.Players>(data);
            if (tableID == localPlayer.player.tableNumber)
            {
                PlayersGenerator.instance.InstantiateNewRemotePlayer(localPlayer);
            }
            else
            {
                GameSerializeClassesCollection.instance.onError.token = Communication.instance.playerToken;
                GameSerializeClassesCollection.instance.onError.eventname = " __new_client_2";
                GameSerializeClassesCollection.instance.onError.message = "Wrong table Number ";
                string data2 = JsonUtility.ToJson(GameSerializeClassesCollection.instance.onError);
                print(data2);
                PokerNetworkManager.instance.socket.Emit("custom_error", new JSONObject(data2));
            }
        }
    }

    // __low_chip_balance listener
    void OnLowChipBalance(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("Low Balance Chip: " + data);
        GameSerializeClassesCollection.instance.onLowChipBalance = JsonUtility.FromJson<GameSerializeClassesCollection.OnLowChipBalance>(data);
        UIManagerScript.instance.lowChipPanel.gameObject.SetActive(true);
        UIManagerScript.instance.lowChipPanelText.text = GameSerializeClassesCollection.instance.onLowChipBalance.message;
    }

    // __chip_balance listener
    void OnChipBalance(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        GameSerializeClassesCollection.instance.chipBalance = JsonUtility.FromJson<GameSerializeClassesCollection.ChipBalance>(data);
        print("__on _chipBalance: " + data);
        //UIManagerScript.instance.BuyInFunctionality();

        // JoinTableBuyIn(chipBalance);
    }

    // __game_prepare listener
    void OnGamePrepare(SocketIOEvent socketIOEvent)
    {
        UIManagerScript.instance.tableStartButton.gameObject.SetActive(false);
        string data = socketIOEvent.data.ToString();
        print("Game Counter: " + data);
        GameSerializeClassesCollection.GamePrepare gamePrepare = JsonUtility.FromJson<GameSerializeClassesCollection.GamePrepare>(data);
        if (gamePrepare.countDown > 0)
        {
            Table.instance.gameStartCounter.enabled = true;
            Table.instance.gameStartCounter.text = gamePrepare.countDown.ToString();
            Table.instance.tableNumber.text = gamePrepare.tableNumber.ToString();
        }
        StartCoroutine(Table.instance.TurnOffCounter());
    }

    // __game_start listener
    void OnGameStart(SocketIOEvent socketIOEvent)
    {
        UIManagerScript.instance.waitOtherPlayerPanel.gameObject.SetActive(false);
        string data = socketIOEvent.data.ToString();
        print("Game Sart: " + data);
        GameSerializeClassesCollection.GameStart gameStart = JsonUtility.FromJson<GameSerializeClassesCollection.GameStart>(data);
    }

    // __new_round_2 listener
    void OnNewRound(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("On New Round 2: " + data);
        GameSerializeClassesCollection.RoundStartInfo roundStartInfoObj = JsonUtility.FromJson<GameSerializeClassesCollection.RoundStartInfo>(data);
        PlayerActionManagement.instance.OnNewRound2(roundStartInfoObj);
        UIManagerScript.instance.postBlind.SetActive(false);
    }

    // __new_round_broadcast listener
    void OnNewRoundBroadcast(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("broadcast New Round Broadcast: " + data);
        GameSerializeClassesCollection.RoundStartInfo roundStartInfoObj = JsonUtility.FromJson<GameSerializeClassesCollection.RoundStartInfo>(data);

        if (GameObject.Find(roundStartInfoObj.player.playerName) != null /*&& tableID == roundStartInfoObj.table.tableNumber*/)
        {
            int actuallId = GameManagerScript.instance.GetActualSeatID(roundStartInfoObj.player.seatId);
            try
            {
                GameManagerScript.instance.playersParent.transform.GetChild(actuallId).GetChild(0).transform.GetComponent<PokerPlayerController>().player.bet = roundStartInfoObj.player.bet;
                GameManagerScript.instance.playersParent.transform.GetChild(actuallId).GetChild(0).transform.GetComponent<PokerPlayerController>().player.chips = roundStartInfoObj.player.chips;
                GameManagerScript.instance.playersParent.transform.GetChild(actuallId).GetChild(0).transform.GetComponent<PokerPlayerController>().player.cards = roundStartInfoObj.player.cards;
                GameManagerScript.instance.playersParent.transform.GetChild(actuallId).GetChild(0).transform.GetComponent<PokerPlayerController>().player.folded = roundStartInfoObj.player.folded;
                GameManagerScript.instance.playersParent.transform.GetChild(actuallId).GetChild(0).transform.GetComponent<PokerPlayerController>().player.roundBet = roundStartInfoObj.player.roundBet;
                GameManagerScript.instance.playersParent.transform.GetChild(actuallId).GetChild(0).transform.GetComponent<PokerPlayerController>().player.allIn = roundStartInfoObj.player.allIn;
                GameManagerScript.instance.playersParent.transform.GetChild(actuallId).GetChild(0).transform.GetComponent<PokerPlayerController>().UpdateValues();
            }
            catch
            {
                print("Some error in OnNewRoundBroadcast PokerNetworkManager.cs");
            }
        }
        else
        {
            GameSerializeClassesCollection.instance.onError.token = Communication.instance.playerToken;
            GameSerializeClassesCollection.instance.onError.eventname = "__new_round_broadcast";
            GameSerializeClassesCollection.instance.onError.message = "NO PLAYER ON THE TABLE";
            string data2 = JsonUtility.ToJson(GameSerializeClassesCollection.instance.onError);
            print(data2);
            socket.Emit("custom_error", new JSONObject(data2));
            //UIManagerScript.instance.TableToPokerUI(5);
        }
    }

    // __next_chance listener
    void OnNextChance(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("Next chance: " + data);
        GameSerializeClassesCollection.NextChance nextChance = JsonUtility.FromJson<GameSerializeClassesCollection.NextChance>(data);
        if (GameObject.Find(nextChance.player.playerName) != null)
        {
            if (nextChance.tableNumber == tableID)
            {
                GameManagerScript.instance.minBet = nextChance.player.minBet;
                Table.instance.roundNameFromGameClass = nextChance.game.roundName;
                PlayerActionManagement.instance.ShowPlayerCommands(nextChance.player, 0f);
            }
        }
        else
        {
            GameSerializeClassesCollection.instance.onError.token = Communication.instance.playerToken;
            GameSerializeClassesCollection.instance.onError.eventname = " __next_chance";
            GameSerializeClassesCollection.instance.onError.message = "NO PLAYER ON THE TABLE";
            string data2 = JsonUtility.ToJson(GameSerializeClassesCollection.instance.onError);
            print(data2);
            socket.Emit("custom_error", new JSONObject(data2));
            //UIManagerScript.instance.TableToPokerUI(5);
        }
    }

    // __action_performed listener
    void OnActionPerformed(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("On  Action Performed: " + data);
        GameSerializeClassesCollection.ActionPerformed actionPerformed = JsonUtility.FromJson<GameSerializeClassesCollection.ActionPerformed>(data);
        if (GameObject.Find(actionPerformed.action.playerName) != null)
        {
            if (tableID == actionPerformed.table.tableNumber)
            {
                PlayerActionManagement.instance.ActionPerformedResult(actionPerformed);
            }
            else
            {
                GameSerializeClassesCollection.instance.onError.token = Communication.instance.playerToken;
                GameSerializeClassesCollection.instance.onError.eventname = " __action_performed";
                GameSerializeClassesCollection.instance.onError.message = "TABLE ID MISMATCH";
                string data2 = JsonUtility.ToJson(GameSerializeClassesCollection.instance.onError);
                print(data2);
                socket.Emit("custom_error", new JSONObject(data2));
            }
        }
        else
        {
            GameSerializeClassesCollection.instance.onError.token = Communication.instance.playerToken;
            GameSerializeClassesCollection.instance.onError.eventname = " __action_performed";
            GameSerializeClassesCollection.instance.onError.message = "NO PLAYER ON THE TABLE";
            string data2 = JsonUtility.ToJson(GameSerializeClassesCollection.instance.onError);
            print(data2);
            socket.Emit("custom_error", new JSONObject(data2));
            //UIManagerScript.instance.TableToPokerUI(5);
        }

    }

    // __next_deal listener
    void OnNextDeal(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("Next Deal: " + data);
        GameSerializeClassesCollection.NextDealInfo nextDealInfo = JsonUtility.FromJson<GameSerializeClassesCollection.NextDealInfo>(data);

        if (tableID == nextDealInfo.table.tableNumber)
        {
            GameManagerScript.instance.UpdateTable(nextDealInfo.table);
            StartCoroutine(GameManagerScript.instance.UpdatePlayerOnNextDeal(nextDealInfo));
        }
        else
        {
            GameSerializeClassesCollection.instance.onError.token = Communication.instance.playerToken;
            GameSerializeClassesCollection.instance.onError.eventname = " __next_deal";
            GameSerializeClassesCollection.instance.onError.message = "TABLE ID MISMATCH";
            string data2 = JsonUtility.ToJson(GameSerializeClassesCollection.instance.onError);
            print(data2);
            socket.Emit("custom_error", new JSONObject(data2));
            //UIManagerScript.instance.TableToPokerUI(5);
        }

    }

    // __next_bet listener
    void OnNextBet(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("Next Bet: " + data);
        GameSerializeClassesCollection.NextChance nextChance = JsonUtility.FromJson<GameSerializeClassesCollection.NextChance>(data);
        if (GameObject.Find(nextChance.player.playerName) != null)
        {
            if (nextChance.tableNumber == tableID)
            {
                GameManagerScript.instance.minBet = nextChance.player.minBet;
                Table.instance.roundNameFromGameClass = nextChance.game.roundName;
                PlayerActionManagement.instance.ShowPlayerCommands(nextChance.player, 0f);
            }
        }
        else
        {
            GameSerializeClassesCollection.instance.onError.token = Communication.instance.playerToken;
            GameSerializeClassesCollection.instance.onError.eventname = " __next_bet";
            GameSerializeClassesCollection.instance.onError.message = "NO PLAYER ON THE TABLE";
            string data2 = JsonUtility.ToJson(GameSerializeClassesCollection.instance.onError);
            print(data2);
            socket.Emit("custom_error", new JSONObject(data2));
            //UIManagerScript.instance.TableToPokerUI(5);
        }
    }

    // __round_end listener
    void OnHandEnd(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("Hand end (__round_end): " + data);
        GameSerializeClassesCollection.instance.winningHand = JsonUtility.FromJson<GameSerializeClassesCollection.WinningHand>(data);

        if (tableID == GameSerializeClassesCollection.instance.winningHand.table.tableNumber)
        {
            WinningLogic.instance.OnHandComplete(GameSerializeClassesCollection.instance.winningHand);
            if (GameManagerScript.instance.isSwitchForJoinFriends)
            {
                GameManagerScript.instance.WaitForSwitch(GameSerializeClassesCollection.instance.winningHand.roundInterval);
            }
            if (GameManagerScript.instance.isSwitchForTableSwitch)
            {
                GameManagerScript.instance.WaitForSwitchTable(GameSerializeClassesCollection.instance.winningHand.roundInterval, localPlayer.transform.GetComponent<PokerPlayerController>().player.chips);
            }
            if (GameManagerScript.instance.isTopUp)
            {
                GameManagerScript.instance.WaitForTopUp(GameSerializeClassesCollection.instance.winningHand.roundInterval);
            }
        }
        else
        {
            GameSerializeClassesCollection.instance.onError.token = Communication.instance.playerToken;
            GameSerializeClassesCollection.instance.onError.eventname = " __round_end";
            GameSerializeClassesCollection.instance.onError.message = "TABLE ID MISMATCH";
            string data2 = JsonUtility.ToJson(GameSerializeClassesCollection.instance.onError);
            print(data2);
            socket.Emit("custom_error", new JSONObject(data2));
            UIManagerScript.instance.TableToPokerUI(5);
        }
    }

    // __topup_chip_balance listener
    void OnTopUpChipBalance(SocketIOEvent socketIOEvent)
    {
        isZeroChips = false;
        string data = socketIOEvent.data.ToString();
        print("__topup_chip_balance: " + data);
        GameSerializeClassesCollection.instance.topUpListener = JsonUtility.FromJson<GameSerializeClassesCollection.TopUpListener>(data);

        if (!GameSerializeClassesCollection.instance.topUpListener.errorStatus)
        {
            for (int i = 0; i < GameManagerScript.instance.playersParent.transform.childCount; i++)
            {
                if (GameManagerScript.instance.playersParent.transform.GetChild(i).childCount == 2)
                {
                    if (GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetComponent<PokerPlayerController>().isLocalPlayer)
                    {
                        GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetComponent<PokerPlayerController>().chipsText.GetComponent<Text>().text = GameSerializeClassesCollection.instance.topUpListener.chips.ToString();
                    }
                }
            }
        }
        else
        {
            UIManagerScript.instance.lowChipPanel.gameObject.SetActive(true);
            UIManagerScript.instance.lowChipPanelText.text = "Top-Up not successful please try again.";
        }
    }

    void TopUpTimesUp(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("top_up_times_up: " + data);
        UIManagerScript.instance.topUpPanelSocialPoker.gameObject.SetActive(false);
    }

    void OnCurrentPlayerLeft(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__current_left_player : " + data);
        GameSerializeClassesCollection.instance.onCurrentPlayerLeft = JsonUtility.FromJson<GameSerializeClassesCollection.OnPlayerLeft>(data);
        print("__current_left_player isGuest : " + GameSerializeClassesCollection.instance.onCurrentPlayerLeft.isGuest);
        PlayersGenerator.instance.CurrentPlayerLeft();
    }

    #region NOT USING
    void OnPlayerLeft(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("On Player Left : __left : " + data);
    }

    void OnPlayerLeft2(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print(" __left_2 : " + data);
    }
    void MeLeft(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("me left : __me_left : " + data);
    }

    void OnStartReload(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__start_reload : " + data);
        GameSerializeClassesCollection.instance.onNewRound = JsonUtility.FromJson<GameSerializeClassesCollection.OnNewRound>(data);
    }
    void OnBuyInAuthRequest(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("Buy In Auth Request : " + data);
        MailBoxScripts._instance.OnBuyInNotification(true);
        MailBoxScripts._instance.BuyInResponseProcess(data, false);

    }

    void OnBuyInAuthPending(SocketIOEvent socketIOEvent)
    {

        string data = socketIOEvent.data.ToString();
        print("Buy In Auth Pending : " + data);
        GameSerializeClassesCollection.instance.buyInAuthPending = JsonUtility.FromJson<GameSerializeClassesCollection.BuyInAuthPending>(data);
        UIManagerScript.instance.lowChipPanel.gameObject.SetActive(true);
        UIManagerScript.instance.lowChipPanelText.text = GameSerializeClassesCollection.instance.buyInAuthPending.message;
    }

    #endregion

    void NewRound(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("New Round : " + data);
        GameSerializeClassesCollection.instance.onNewRound = JsonUtility.FromJson<GameSerializeClassesCollection.OnNewRound>(data);

        Table.instance.tableType.transform.parent.gameObject.SetActive(true);
        Table.instance.blind.transform.parent.gameObject.SetActive(true);
        string sBlind = GameManagerScript.instance.KiloFormat(GameSerializeClassesCollection.instance.onNewRound.table.smallBlind.amount);
        string bBlind = GameManagerScript.instance.KiloFormat(GameSerializeClassesCollection.instance.onNewRound.table.bigBlind.amount);
        Table.instance.blind.text = sBlind + "/" + bBlind;


        GameManagerScript.instance.dealerSeatid = GameSerializeClassesCollection.instance.onNewRound.table.dealer.seatId;
        GameManagerScript.instance.dealerName = GameSerializeClassesCollection.instance.onNewRound.table.dealer.playerName;
        PlayerActionManagement.instance.NewRound(GameSerializeClassesCollection.instance.onNewRound);
    }

    void SeatOccupied(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("Seat Occupied : " + data);
        GameSerializeClassesCollection.instance.seatOccupied = JsonUtility.FromJson<GameSerializeClassesCollection.OnLowChipBalance>(data);
        //UIManagerScript.instance.lowChipPanel.gameObject.SetActive(true);
        UIManagerScript.instance.lowChipPanelText.text = LanguageManager.Instance.GetTextValue("Seat is already Taken");//"Seat is already Taken. Click on any other seat";

        UIManagerScript.instance.lowChipPanelText.text = GameSerializeClassesCollection.instance.seatOccupied.message;
        UIManagerScript.instance.TableToPokerUI(4);
    }

    void OnDisbandTableMessage(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("Disband Table : " + data);
        GameSerializeClassesCollection.instance.disbandTableMessage = JsonUtility.FromJson<GameSerializeClassesCollection.DisbandTableMessage>(data);
        UIManagerScript.instance.lowChipPanel.gameObject.SetActive(true);
        UIManagerScript.instance.lowChipPanelText.text = GameSerializeClassesCollection.instance.disbandTableMessage.message;
    }


    void OnErrorTableSize(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("On Error Table Size : " + data);
        GameSerializeClassesCollection.instance.disbandTableMessage = JsonUtility.FromJson<GameSerializeClassesCollection.DisbandTableMessage>(data);

        if (GameSerializeClassesCollection.instance.disbandTableMessage.errorStatus)
        {
            if (string.IsNullOrEmpty(GameSerializeClassesCollection.instance.disbandTableMessage.ticket))
            {
                UIManagerScript.instance.lowChipPanel.gameObject.SetActive(true);
                UIManagerScript.instance.lowChipPanelText.text = GameSerializeClassesCollection.instance.disbandTableMessage.message;   /* "No Seat Available";*/
                StartCoroutine(ExitTable());
            }
            else
            {
                UIManagerScript.instance.lowChipPanel.gameObject.SetActive(true);
                UIManagerScript.instance.lowChipPanelText.text = LanguageManager.Instance.GetTextValue("All seats are reserved");//"All seats are reserved!! Redirecting you to another table";   /* "No Seat Available";*/
                GameManagerScript.instance.SwitchTables(GameSerializeClassesCollection.instance.disbandTableMessage.ticket);

            }
        }
    }


    void OnResumeAllClients(SocketIOEvent socketIOEvent)
    {
        GameManagerScript.instance.isGameResumed = true;
        string data = socketIOEvent.data.ToString();

        print("On Resume All Clients : " + data);
        GameSerializeClassesCollection.AllPlayers allPlayers = JsonUtility.FromJson<GameSerializeClassesCollection.AllPlayers>(data);
        try
        {
            for (int i = 0; i < allPlayers.players.Length; i++)
            {
                if (AccessGallery.instance.profileName[0].text == allPlayers.players[i].playerName)
                {
                    GameSerializeClassesCollection.instance.newLocalPlayer.player.playerName = allPlayers.players[i].playerName;
                    GameSerializeClassesCollection.instance.newLocalPlayer.player.seatId = allPlayers.players[i].seatId;
                    GameSerializeClassesCollection.instance.newLocalPlayer.player.clientId = allPlayers.players[i].clientId;
                    GameSerializeClassesCollection.instance.newLocalPlayer.player.initialChips = allPlayers.players[i].initialChips;
                    GameSerializeClassesCollection.instance.newLocalPlayer.player.user_image = allPlayers.players[i].user_image;
                    GameManagerScript.instance.localSeatIDDummy = GameSerializeClassesCollection.instance.newLocalPlayer.player.seatId;

                    GameManagerScript.instance.chairAnimForLocalPlayer = false;
                    PlayersGenerator.instance.InstantiateLocalPlayer(GameSerializeClassesCollection.instance.newLocalPlayer, true);

                }
                else
                {
                    PlayersGenerator.instance.InstantiateAllOtherPlayer(allPlayers.players[i]);
                }
            }
        }
        catch
        {
            print("no players");
        }

        if (!UIManagerScript.instance.winPanel.activeInHierarchy)
        {
            try
            {
                if (/*allPlayers.basicData != null*/ allPlayers.basicData.players.Length != 0)
                {
                    CardShuffleAnimation.instance.isAnimationComplete = true;
                    GameManagerScript.instance.AllBasicDataUpdate(allPlayers.basicData);

                    GameManagerScript.instance.UpdateTable(allPlayers.basicData.table);
                    Table.instance.UpdateTotalPot();
                    //Table.instance.UpdateRoundPot(allPlayers.basicData.table.totalBet);

                    StartCoroutine(GameManagerScript.instance.FindLocalPlayerAndUpdateHand(allPlayers.basicData.players));
                }
            }
            catch (Exception e)
            {
                CardShuffleAnimation.instance.isAnimationComplete = true;
                print("no basic data.." + e);
            }

            try
            {
                if (allPlayers.checkActionPlayer)
                {
                    GameManagerScript.instance.minBet = allPlayers.currentActionPlayerDetails.player.minBet;
                    Table.instance.roundNameFromGameClass = allPlayers.currentActionPlayerDetails.game.roundName;
                    PlayerActionManagement.instance.ShowPlayerCommands(allPlayers.currentActionPlayerDetails.player, 0.5f);
                }
            }
            catch
            {
                print("Error in current Action PlayerDetails");
            }
        }
        else
        {
            print("WIN Panel is acive");
        }
        UIManagerScript.instance.loadingPanel.SetActive(false);
        if (GameManagerScript.instance.isVideoTable)
        {
            StartCoroutine(GameManagerScript.instance.GenerateVideoOnResume());
        }
    }

    void OnSendObserver(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("On __send_observer : " + data);
        STObserverEmitter(true);
    }

    void OnAllClientObserver(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("On __all_clients_observer : " + data);
        GameManagerScript.instance.isGameResumed = true;

        GameSerializeClassesCollection.AllPlayers allPlayers = JsonUtility.FromJson<GameSerializeClassesCollection.AllPlayers>(data);
        try
        {
            for (int i = 0; i < allPlayers.players.Length; i++)
            {
                if (AccessGallery.instance.profileName[0].text == allPlayers.players[i].playerName)
                {
                    GameSerializeClassesCollection.instance.newLocalPlayer.player.playerName = allPlayers.players[i].playerName;
                    GameSerializeClassesCollection.instance.newLocalPlayer.player.seatId = allPlayers.players[i].seatId;
                    GameSerializeClassesCollection.instance.newLocalPlayer.player.clientId = allPlayers.players[i].clientId;
                    GameSerializeClassesCollection.instance.newLocalPlayer.player.initialChips = allPlayers.players[i].initialChips;
                    GameSerializeClassesCollection.instance.newLocalPlayer.player.user_image = allPlayers.players[i].user_image;
                    GameManagerScript.instance.localSeatIDDummy = GameSerializeClassesCollection.instance.newLocalPlayer.player.seatId;

                    GameManagerScript.instance.chairAnimForLocalPlayer = false;
                    PlayersGenerator.instance.InstantiateLocalPlayer(GameSerializeClassesCollection.instance.newLocalPlayer, true);

                }
                else
                {
                    PlayersGenerator.instance.InstantiateAllOtherPlayer(allPlayers.players[i]);
                }
            }
        }
        catch
        {
            print("no players");
        }

        if (!UIManagerScript.instance.winPanel.activeInHierarchy)
        {
            try
            {
                if (/*allPlayers.basicData != null*/ allPlayers.basicData.players.Length != 0)
                {
                    CardShuffleAnimation.instance.isAnimationComplete = true;
                    GameManagerScript.instance.AllBasicDataUpdate(allPlayers.basicData);

                    GameManagerScript.instance.UpdateTable(allPlayers.basicData.table);
                    Table.instance.UpdateTotalPot();
                    //Table.instance.UpdateRoundPot(allPlayers.basicData.table.totalBet);

                    StartCoroutine(GameManagerScript.instance.FindLocalPlayerAndUpdateHand(allPlayers.basicData.players));
                }
            }
            catch (Exception e)
            {
                CardShuffleAnimation.instance.isAnimationComplete = true;
                print("no basic data.." + e);
            }

            try
            {
                if (allPlayers.checkActionPlayer)
                {
                    GameManagerScript.instance.minBet = allPlayers.currentActionPlayerDetails.player.minBet;
                    Table.instance.roundNameFromGameClass = allPlayers.currentActionPlayerDetails.game.roundName;
                    PlayerActionManagement.instance.ShowPlayerCommands(allPlayers.currentActionPlayerDetails.player, 0.5f);
                }
            }
            catch
            {
                print("Error in current Action PlayerDetails");
            }
        }
        else
        {
            print("WIN Panel is acive");
        }
        UIManagerScript.instance.loadingPanel.SetActive(false);
        if (GameManagerScript.instance.isVideoTable)
        {
            StartCoroutine(GameManagerScript.instance.GenerateVideoOnResume());
            AgoraInit.instance.ResetUserJoinCount(1);
            AgoraInit.instance.MuteUnmuteLocalPlayer(true);
            AgoraInit.instance.MuteUnmuteAllRemotePlayer(true);
            //AgoraInit.instance.MuteVideo(true);
        }
        UIManagerScript.instance.ActivateSitIn();
    }

    void GameOver(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("Game Over : " + data);
        UIManagerScript.instance.TableToPokerUI(3);
    }


    void LimitWinPlayer(SocketIOEvent socketIOEvent)
    {
        print("limit ....");
        //UIManagerScript.instance.lowChipPanel.gameObject.SetActive(true);
        UIManagerScript.instance.lowChipPanelText.text = LanguageManager.Instance.GetTextValue("You Are Not");//"You Are Not a Win Player";
    }

    void Throwable(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__st_throwables: " + data);
        GameSerializeClassesCollection.instance.throwablesListener = JsonUtility.FromJson<GameSerializeClassesCollection.ThrowablesListener>(data);
        SocialPokerGameManager.instance.PlayThrowableAnimation(GameSerializeClassesCollection.instance.throwablesListener.animation, GameSerializeClassesCollection.instance.throwablesListener.destinationPlayerName, GameSerializeClassesCollection.instance.throwablesListener.sourcePlayerName);
    }

    void RemoveMinimizePlayer(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("RemoveMinimizePlayer: " + data);
        UIManagerScript.instance.TableToPokerUI(4);
    }

    public bool isZeroChips;
    void AddChips(SocketIOEvent socketIOEvent)
    {
        isZeroChips = true;
        SocialTournamentScript.instance.isTopUpPanelOn = true;
        string data = socketIOEvent.data.ToString();
        print("__user_chip_balance: " + data);
        GameSerializeClassesCollection.instance.userChipBalance = JsonUtility.FromJson<GameSerializeClassesCollection.UserChipBalance>(data);

        if (!GameSerializeClassesCollection.instance.userChipBalance.errorStatus)
        {
            if (GameSerializeClassesCollection.instance.userChipBalance.balance <= GameSerializeClassesCollection.instance.userChipBalance.buyIn)
            {
                print("userChipBalance.balance: " + GameSerializeClassesCollection.instance.userChipBalance.balance);
                print("emptypChipsLessBuyInPanel");
                string balance = GameManagerScript.instance.KiloFormat(GameSerializeClassesCollection.instance.userChipBalance.balance);
                print("balance: " + balance);
                UIManagerScript.instance.emptypChipsLessBuyInPanel.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text = "Top up amount: " + balance + "\n" + "Total chip Balance: " + balance + "\n" + "Top up chips to continue playing";
                UIManagerScript.instance.emptypChipsLessBuyInPanel.SetActive(true);
            }
            else
            {
                UIManagerScript.instance.emptypChipsPanel.SetActive(true);
                UIManagerScript.instance.topUpPanelSocialPoker.GetChild(0).GetChild(5).gameObject.SetActive(false);
                UIManagerScript.instance.topUpPanelSocialPoker.GetChild(0).GetChild(8).gameObject.SetActive(true);
            }
            SocialTournamentScript.instance.StartCountDown();
            topupCorotine = TopupBtnClickedCorotine();
            StartCoroutine(topupCorotine);
        }
        else
        {
            print("accountBalanceZeroPanel");
            UIManagerScript.instance.accountBalanceZeroPanel.SetActive(true);
            StopTopupCorotineFun();
        }
    }

    void ChatListener(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__chat: " + data);
        GameSerializeClassesCollection.instance.chatListener = JsonUtility.FromJson<GameSerializeClassesCollection.ChatListener>(data);
        Chat.instance.CreateChatForRemoteUser();
    }

    void ChatHistoryListener(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__chat_message: " + data);
        GameSerializeClassesCollection.instance.chatHistoryListener = JsonUtility.FromJson<GameSerializeClassesCollection.ChatHistoryListener>(data);
        Chat.instance.CreateChatHistory();
    }

    void ConnectOnLogin(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("ConnectOnLogin  : " + data);
    }

    void OnLatestState(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();

        print("OnLatestState: " + data);
        GameSerializeClassesCollection.AllPlayers allPlayers = JsonUtility.FromJson<GameSerializeClassesCollection.AllPlayers>(data);
        try
        {
            for (int i = 0; i < allPlayers.players.Length; i++)
            {
                if (AccessGallery.instance.profileName[0].text == allPlayers.players[i].playerName)
                {
                    GameSerializeClassesCollection.instance.newLocalPlayer.player.playerName = allPlayers.players[i].playerName;
                    GameSerializeClassesCollection.instance.newLocalPlayer.player.seatId = allPlayers.players[i].seatId;
                    GameSerializeClassesCollection.instance.newLocalPlayer.player.clientId = allPlayers.players[i].clientId;
                    GameSerializeClassesCollection.instance.newLocalPlayer.player.initialChips = allPlayers.players[i].initialChips;
                    GameSerializeClassesCollection.instance.newLocalPlayer.player.user_image = allPlayers.players[i].user_image;
                    GameManagerScript.instance.localSeatIDDummy = GameSerializeClassesCollection.instance.newLocalPlayer.player.seatId;
                    GameManagerScript.instance.chairAnimForLocalPlayer = false;
                    PlayersGenerator.instance.InstantiateLocalPlayer(GameSerializeClassesCollection.instance.newLocalPlayer, false);

                }
                else
                {
                    PlayersGenerator.instance.InstantiateAllOtherPlayer(allPlayers.players[i]);
                }
            }
        }
        catch
        {
            print("no players");
        }

        if (!UIManagerScript.instance.winPanel.activeInHierarchy)
        {
            try
            {
                if (allPlayers.basicData.players.Length != 0)
                {
                    CardShuffleAnimation.instance.isAnimationComplete = true;
                    GameManagerScript.instance.AllBasicDataUpdate(allPlayers.basicData);

                    Table.instance.UpdateTotalPot();
                    GameManagerScript.instance.UpdateTable(allPlayers.basicData.table);
                    //Table.instance.UpdateRoundPot(allPlayers.basicData.table.totalBet);
                }

            }
            catch
            {
                CardShuffleAnimation.instance.isAnimationComplete = true;
                print("no basic data");
            }

            try
            {
                if (allPlayers.checkActionPlayer)
                {
                    GameManagerScript.instance.minBet = allPlayers.currentActionPlayerDetails.player.minBet;
                    Table.instance.roundNameFromGameClass = allPlayers.currentActionPlayerDetails.game.roundName;
                    PlayerActionManagement.instance.ShowPlayerCommands(allPlayers.currentActionPlayerDetails.player, 0.5f);
                }
            }
            catch
            {
                print("Error in current Action PlayerDetails");
            }
        }
        else
        {
            print("WIN Panel is acive");
        }
        UIManagerScript.instance.loadingPanel.SetActive(false);
        StartCoroutine(GameManagerScript.instance.GenerateVideoOnResume());
    }

    int steps;
    void OnManualTopUp(SocketIOEvent socketIOEvent)
    {
        isInitialSliderState = false;

        string data = socketIOEvent.data.ToString();
        print("__add_manual_topup: " + data);
        GameSerializeClassesCollection.instance.userChipBalance = JsonUtility.FromJson<GameSerializeClassesCollection.UserChipBalance>(data);
        print(GameSerializeClassesCollection.instance.userChipBalance.errorStatus);
        if (GameSerializeClassesCollection.instance.userChipBalance.errorStatus)
        {
            UIManagerScript.instance.lowChipPanelText.text = GameSerializeClassesCollection.instance.userChipBalance.message;
            UIManagerScript.instance.lowChipPanel.gameObject.SetActive(true);
        }

        else
        {
            if (isZeroChips)
            {
                steps = Convert.ToInt32((GameSerializeClassesCollection.instance.userChipBalance.max_buy_in - GameSerializeClassesCollection.instance.userChipBalance.min_buy_in) / (1000 * GameSerializeClassesCollection.instance.userChipBalance.stepper));
            }
            else
            {
                steps = Convert.ToInt32(GameSerializeClassesCollection.instance.userChipBalance.max_buy_in / (GameSerializeClassesCollection.instance.userChipBalance.min_buy_in * GameSerializeClassesCollection.instance.userChipBalance.stepper));
            }
            UIManagerScript.instance.newBuyInSlider.minValue = 0;
            UIManagerScript.instance.newBuyInSlider.maxValue = steps;

            UIManagerScript.instance.topUpPanelSocialPoker.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = string.Format("{0:n0}", GameSerializeClassesCollection.instance.userChipBalance.min_buy_in);           // "$" + GameSerializeClassesCollection.instance.userChipBalance.buyIn;
            UIManagerScript.instance.topUpPanelSocialPoker.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = string.Format("{0:n0}", GameSerializeClassesCollection.instance.userChipBalance.min_buy_in);
            UIManagerScript.instance.topUpPanelSocialPoker.GetChild(0).GetChild(3).GetChild(0).GetComponent<Text>().text = string.Format("{0:n0}", GameSerializeClassesCollection.instance.userChipBalance.max_buy_in);
            UIManagerScript.instance.topUpPanelSocialPoker.GetChild(0).GetChild(4).GetChild(0).GetComponent<Text>().text = string.Format("{0:n0}", GameSerializeClassesCollection.instance.userChipBalance.balance);         //"$" + GameSerializeClassesCollection.instance.userChipBalance.balance;
            UIManagerScript.instance.topUpPanelSocialPoker.gameObject.SetActive(true);
        }
    }

    void OnGettingFriendList(SocketIOEvent socketIOEvent)
    {
        print(">>>>>> socket data" + socketIOEvent.data.ToString());

        FriendandSocialScript.instance.GettingFriendListFromSocket(socketIOEvent.data.ToString());
    }


    void OnGettingPendingList(SocketIOEvent socketIOEvent)
    {
        print(">>>>>> socket data" + socketIOEvent.data.ToString());

        FriendandSocialScript.instance.GettingPendingListFromSocket(socketIOEvent.data.ToString());
    }

    public void GetInviteFromUser(SocketIOEvent socketIOEvent)
    {
        FriendandSocialScript.instance.ShowInviteRequestPopUp(socketIOEvent.data.ToString(), false);

        Debug.Log(">>invite listner Data: " + socketIOEvent.data.ToString());

    }

    public void StandUpPlayerListner(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__stand_up_player: " + data);
        GameSerializeClassesCollection.instance.standUpPlayerReceiver = JsonUtility.FromJson<GameSerializeClassesCollection.StandUpPlayerReceiver>(data);

        if (tableID == GameSerializeClassesCollection.instance.standUpPlayerReceiver.tableNumber)
        {
            GameSerializeClassesCollection.instance.onCurrentPlayerLeft.playerName = GameSerializeClassesCollection.instance.standUpPlayerReceiver.playerName;
            GameSerializeClassesCollection.instance.onCurrentPlayerLeft.clientId = GameSerializeClassesCollection.instance.standUpPlayerReceiver.clientId.ToString();
            GameSerializeClassesCollection.instance.onCurrentPlayerLeft.seatId = GameSerializeClassesCollection.instance.standUpPlayerReceiver.seatId;
            GameSerializeClassesCollection.instance.onCurrentPlayerLeft.isGuest = GameSerializeClassesCollection.instance.standUpPlayerReceiver.isGuest;


            GameObject currplayer = GameObject.Find(GameSerializeClassesCollection.instance.onCurrentPlayerLeft.playerName);
            if (currplayer.GetComponent<PokerPlayerController>().isLocalPlayer)
            {
                STObserverEmitter(false);
                GameManagerScript.instance.DeActiveLocalPlayer(currplayer);
            }
            else
            {
                PlayersGenerator.instance.CurrentPlayerLeft();
            }
        }
    }
    void SwitchTables(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__switch_table: " + data);
        GameSerializeClassesCollection.instance.switchTableListener = JsonUtility.FromJson<GameSerializeClassesCollection.SwitchTableListener>(data);

        GameManagerScript.instance.SwitchTables(GameSerializeClassesCollection.instance.switchTableListener.ticket);
    }

    public void STObserverPlayerListner(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print(">> STObserverPlayerListner: " + data);
    }

    void TokenRevoked(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("TokenRevoked: " + data);
        UIManagerScript.instance.TableToPokerUI(6);
    }

    public void OnCheckEnableDetail(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__enable_detail : " + data);
        FriendandSocialScript.instance.TableNotExistFunc();
    }

    public void CheckPlayerExistListner(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__check_player_exist: " + data);
        GameSerializeClassesCollection.instance.checkPlayerExistListner = new GameSerializeClassesCollection.CheckPlayerExistListner();
        GameSerializeClassesCollection.instance.checkPlayerExistListner = JsonUtility.FromJson<GameSerializeClassesCollection.CheckPlayerExistListner>(data);
    }


    IEnumerator topupCorotine;
    void LastPlayerLeft(SocketIOEvent socketIOEvent)
    {
        print(">>>>>> LastPlayerLeft");

        //topupCorotine = TopupBtnClickedCorotine();
        //StartCoroutine(topupCorotine);
    }
    IEnumerator TopupBtnClickedCorotine()
    {
        float time = 40;
        //.........................................HOLD ON ..................................................................//
        while (true)
        {
            if (time < 0.2f)
            {
                StandUpChipsEmptyEmitter();
                break;
            }
            print("isTopupBtnClicked: " + UIManagerScript.instance.isTopupBtnClicked);

            if (UIManagerScript.instance.isTopupBtnClicked)
            {
                GameManagerScript.instance.CalculateChips();
                yield return new WaitForSeconds(0.2f);
                string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.topUp);
                socket.Emit("__topup_chip_balance", new JSONObject(data));
                print("__topup_chip_balance " + data);
                break;
            }

            if (time < 1.1f)
            {
                UIManagerScript.instance.topUpPanelSocialPoker.gameObject.SetActive(false);
                UIManagerScript.instance.isTopupBtnClicked = false;
                break;
            }

            print("Holding TopupBtnClickedCorotine........." + time);
            time -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        UIManagerScript.instance.topUpPanelSocialPoker.gameObject.SetActive(false);
        UIManagerScript.instance.isTopupBtnClicked = false;
        GameManagerScript.instance.isTopUp = false;

    }

    void StopTopupCorotineFun()
    {
        StartCoroutine(StopTopupCorotine());
    }

    IEnumerator StopTopupCorotine()
    {
        print("StopCoroutine...topupCorotine.");

        yield return new WaitForSeconds(0.5f);

        GameManagerScript.instance.isTopUp = false;

        if (topupCorotine != null)
        {
            print("StopCoroutine...topupCorotine.");
            StopCoroutine(topupCorotine);
        }
    }

    #endregion


    #region Emiter Methods  
    //......................................Emiiter Methods............................................//


    //Sending Invite Btn Request, Check whether seat is available or not...
    GameObject currentInvitee;
    public void SendInviteBtnReq(GameObject temp, bool isvideo, string userName, string tableNumber, bool pushN)
    {
        currentInvitee = temp;
        GameSerializeClassesCollection.instance._st_inviteInstance.isVideo = GameManagerScript.instance.isVideoTable;
        GameSerializeClassesCollection.instance._st_inviteInstance.username = userName;
        GameSerializeClassesCollection.instance._st_inviteInstance.tableNumber = tableID;
        GameSerializeClassesCollection.instance._st_inviteInstance.pushNotification = pushN;
        GameSerializeClassesCollection.instance._st_inviteInstance.senderUserName = AccessGallery.instance.profileName[0].text;
        GameSerializeClassesCollection.instance._st_inviteInstance.token = Communication.instance.playerToken;
        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance._st_inviteInstance);
        // socket.Emit("__st_invite_friends", new JSONObject(data));
        socket.Emit("__check_st_invite_friends", new JSONObject(data));
        //  socket.Emit("__st_invite_friends");
        Debug.Log(">>__check_st_invite_friends" + data);
    }


    //Sending Reserve Seat Request...

    string currTicket;
    bool currIsVideo;
    bool isJoinorInvite;
    internal string Chk_userName;
    public void SendReserveSeatRequest(string ticket, bool isJoinVideo, bool isjoinorinvite, int reserveSeatId, string chk_name)
    {
        currTicket = ticket;
        currIsVideo = isJoinVideo;
        isJoinorInvite = isjoinorinvite;
        Chk_userName = chk_name;
        GameSerializeClassesCollection.instance.reserveInstance.ticket = ticket;
        if (isjoinorinvite)
        {
            Debug.Log(">>>>" + "__reserve_seat  : joining reserve case...");
            GameSerializeClassesCollection.instance.reserveInstance.seatId = reserveSeatId;
        }
        else
        {
            Debug.Log(">>>>" + "__reserve_seat  : invite reserve case...");
            GameSerializeClassesCollection.instance.reserveInstance.seatId = reserveSeatId;

        }
        GameSerializeClassesCollection.instance.reserveInstance.token = Communication.instance.playerToken;
        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.reserveInstance);
        socket.Emit("__reserve_seat", new JSONObject(data));
        Debug.Log(">>>>" + "__reserve_seat" + data);
    }


    //After sending wait request..
    GameObject currentWaitListUser;
    public void OnSendingWaitRequest(string ticket, GameObject temp)
    {
        currentWaitListUser = temp;
        GameSerializeClassesCollection.instance.waitInstance.ticket = ticket;
        GameSerializeClassesCollection.instance.waitInstance.tableNumber = tableID;
        GameSerializeClassesCollection.instance.waitInstance.token = Communication.instance.playerToken;

        if (!string.IsNullOrEmpty(temp.GetComponent<MyFriendListPrefabHandler>().userName))
            GameSerializeClassesCollection.instance.waitInstance.reciever_name = temp.GetComponent<MyFriendListPrefabHandler>().userName;
        else
            GameSerializeClassesCollection.instance.waitInstance.reciever_name = temp.GetComponent<MyFriendListPrefabHandler>().profileName.ToString();

        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.waitInstance);
        socket.Emit("__waiting_list", new JSONObject(data));
        Debug.Log(">>>>" + "__waiting_list" + data);

        if (GameManagerScript.instance.activeTable != null)
        {
            if (GameManagerScript.instance.NonVideoTable.activeInHierarchy)
            {
                FriendandSocialScript.instance.tableType = eTableType.nonvideo;
            }
            else if (GameManagerScript.instance.videoTable.activeInHierarchy)
            {
                FriendandSocialScript.instance.tableType = eTableType.video;
            }
            else
            {
                FriendandSocialScript.instance.tableType = eTableType.lobby;
            }
        }
        else
        {
            FriendandSocialScript.instance.tableType = eTableType.lobby;
            Debug.Log(">> Table type not defined");
        }

        if (FriendandSocialScript.instance.tableType == eTableType.nonvideo)
        {
            UIManagerScript.instance.menu_panelUIButton.SetActive(true);
            FriendandSocialScript.instance.OnClickCloseBtnP();
            FriendandSocialScript.instance.OnClickBackBtn();
            Debug.Log(">>Tabletype is nonvideo");
        }
        else if (FriendandSocialScript.instance.tableType == eTableType.video)
        {
            UIManagerScript.instance.menu_panelUIButton.SetActive(true);
            FriendandSocialScript.instance.OnClickCloseBtn();
            Debug.Log(">>Tabletype is video");
        }
        else if (FriendandSocialScript.instance.tableType == eTableType.lobby)
        {
            // FriendandSocialScript.instance.OnClickBackBtn();
            Debug.Log(">> in the lobby");
        }
    }

    // After click on online friend join button...
    GameObject currentJoiner;
    public void SendOnlineFriendJoinBtnReq(string ticket, GameObject temp)
    {
        currentJoiner = temp;
        GameSerializeClassesCollection.instance.joinfriendInstance.table_id = tableID;
        GameSerializeClassesCollection.instance.joinfriendInstance.ticket = ticket;
        GameSerializeClassesCollection.instance.joinfriendInstance.token = Communication.instance.playerToken;
        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.joinfriendInstance);
        socket.Emit("__join_friends", new JSONObject(data));
        Debug.Log(">>>>" + "__join_friends" + data);
    }

    public void AddFriendOnTable(string recipientName, string id, string senderName, string senderId)
    {
        GameSerializeClassesCollection.instance.addfrindRequest.sender_name = senderName;
        GameSerializeClassesCollection.instance.addfrindRequest.sender_id = senderId;
        GameSerializeClassesCollection.instance.addfrindRequest.tableNumber = GameSerializeClassesCollection.instance.observeTable.ticket;
        GameSerializeClassesCollection.instance.addfrindRequest.recipient = id;
        GameSerializeClassesCollection.instance.addfrindRequest.recipient_name = recipientName;
        GameSerializeClassesCollection.instance.addfrindRequest.token = Communication.instance.playerToken;
        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.addfrindRequest);
        socket.Emit("__send_friend_request", new JSONObject(data));

        // Communication.instance.PostData("http://23.23.68.112:5000/api/v1/user/send-friend-request", data, friendResponse);
        Debug.Log(">>>> sendingfriend request" + "name " + recipientName + id + Communication.instance.playerToken);

    }

    void friendResponse(string response)
    {
        Debug.Log(">>>>>friend response" + response);
    }

    // it emits join 
    void JoinTableEmitter(string data)
    {
        socket.Emit("__join", new JSONObject(data));
    }

    public void StartTablePrepareGameEmitter()
    {
        GameSerializeClassesCollection.instance.startTheTable.token = GameSerializeClassesCollection.instance.observeTable.token;
        GameSerializeClassesCollection.instance.startTheTable.ticket = GameSerializeClassesCollection.instance.observeTable.ticket;
        GameSerializeClassesCollection.instance.startTheTable.table_type = GameSerializeClassesCollection.instance.observeTable.table_type;

        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.startTheTable);
        print("PrepareGame " + data);
        socket.Emit("__prepare_game", new JSONObject(data));
    }

    string chips;
    public void TopUpEmitter()
    {
        // StopTopupCorotineFun();

        SocialTournamentScript.instance.isTopUpPanelOn = false;
        isTopUpClick = true;
        SocialTournamentScript.instance.StopTimer();
        for (int i = 0; i < GameManagerScript.instance.playersParent.transform.childCount; i++)
        {
            if (GameManagerScript.instance.playersParent.transform.GetChild(i).childCount == 2)
            {
                if (GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetComponent<PokerPlayerController>().isLocalPlayer)
                {
                    chips = GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetComponent<PokerPlayerController>().chipsText.GetComponent<Text>().text;
                    chips = chips.Replace(",", "");
                    print(chips);
                }
            }
        }
        print("BuyIn: " + GameSerializeClassesCollection.instance.userChipBalance.buyIn);
        GameSerializeClassesCollection.instance.topUp.token = GameSerializeClassesCollection.instance.observeTable.token;
        GameSerializeClassesCollection.instance.topUp.tableNumber = GameSerializeClassesCollection.instance.observeTable.ticket;
        if (isInitialSliderState)
        {
            GameSerializeClassesCollection.instance.topUp.buyIn = GameSerializeClassesCollection.instance.userChipBalance.buyIn;
        }
        else
        {
            GameSerializeClassesCollection.instance.topUp.buyIn = GameSerializeClassesCollection.instance.userChipBalance.min_buy_in;
        }
        GameSerializeClassesCollection.instance.topUp.chips = int.Parse(chips);


        GameSerializeClassesCollection.instance.topUp.isZeroChips = isZeroChips;

        if (isZeroChips)// && GameManagerScript.instance.totalPlayersSitting <= 2)
        {
            //string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.topUp);
            //socket.Emit("__topup_chip_balance", new JSONObject(data));
            //print("__topup_chip_balance " + data);
        }
        else
        {
            GameManagerScript.instance.isTopUp = true;

            UIManagerScript.instance.lowChipPanel.gameObject.SetActive(true);
            UIManagerScript.instance.lowChipPanelText.text = "Top-Up successfull!! Chips will be added after this hand";

            UIManagerScript.instance.topUpChipBtn.transform.GetComponent<Button>().interactable = false;
            UIManagerScript.instance.topUpChipBtn.transform.GetChild(0).GetComponent<Text>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
            UIManagerScript.instance.topUpChipBtn.transform.GetChild(1).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
        }
    }

    public void SendPostBlind()
    {
        GameSerializeClassesCollection.instance.postBlindData.is_post_blind = true;
        GameSerializeClassesCollection.instance.postBlindData.token = GameSerializeClassesCollection.instance.observeTable.token; ;
        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.postBlindData);
        print("__post_blind " + data);
        socket.Emit("__post_blind", new JSONObject(data));
    }

    public void ManualTopUpEmitter()
    {
        GameSerializeClassesCollection.instance.localPlayerExitHandler.token = GameSerializeClassesCollection.instance.observeTable.token;
        string data2 = JsonUtility.ToJson(GameSerializeClassesCollection.instance.localPlayerExitHandler);
        socket.Emit("__manual_topup", new JSONObject(data2));
    }

    public void DisbandTableEmitter(bool is_disband, bool is_disband_repeat)
    {
        GameSerializeClassesCollection.instance.disbandTable.token = GameSerializeClassesCollection.instance.observeTable.token;
        GameSerializeClassesCollection.instance.disbandTable.ticket = GameSerializeClassesCollection.instance.observeTable.ticket;
        GameSerializeClassesCollection.instance.disbandTable.table_type = GameSerializeClassesCollection.instance.observeTable.table_type;
        GameSerializeClassesCollection.instance.disbandTable.is_disband = is_disband;
        GameSerializeClassesCollection.instance.disbandTable.is_disband_repeat = is_disband_repeat;

        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.disbandTable);
        print("Disband Table " + data);
        socket.Emit("__disband_table", new JSONObject(data));
    }

    // Buy In Auth Accept and Decline
    public void BuyInAuthActionEmitter(bool acceptAll)
    {
        string data;
        if (acceptAll)
        {
            data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.buyInAuthActionAcceptAll);
        }
        else
        {
            data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.buyInAuthAction);
        }

        print("Buy In Auth Action " + data);
        socket.Emit("__buyin_auth_action", new JSONObject(data));
    }

    public void StraddleEmitter()
    {
        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.straddle);
        print("Buy In Auth Action " + data);
        socket.Emit("__mississippi_straddle", new JSONObject(data));
    }

    public void JoinWaitListEmitter()
    {
        print("__join_waiting_list ");
        socket.Emit("__join_waiting_list");
    }

    public void ThrowableEmitter()
    {
        GameSerializeClassesCollection.instance.throwables.source = SocialTournamentScript.instance.throwableSource;
        GameSerializeClassesCollection.instance.throwables.destination = SocialTournamentScript.instance.throwableDestination;
        GameSerializeClassesCollection.instance.throwables.amount = SocialTournamentScript.instance.throwableCharge;
        GameSerializeClassesCollection.instance.throwables.ticket = GameSerializeClassesCollection.instance.observeTable.ticket;
        GameSerializeClassesCollection.instance.throwables.token = GameSerializeClassesCollection.instance.observeTable.token;
        GameSerializeClassesCollection.instance.throwables.animation = SocialTournamentScript.instance.animationName;
        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.throwables);
        print("__st_throwables" + data);
        socket.Emit("__st_throwables", new JSONObject(data));
    }

    public void ChatEmitter(string message)
    {
        GameSerializeClassesCollection.instance.chat.token = GameSerializeClassesCollection.instance.observeTable.token;
        GameSerializeClassesCollection.instance.chat.tableNumber = GameSerializeClassesCollection.instance.observeTable.ticket;
        GameSerializeClassesCollection.instance.chat.message = message;

        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.chat);
        print("__chat" + data);
        socket.Emit("__chat", new JSONObject(data));
    }

    public void ChatHistoryEmitter()
    {
        GameSerializeClassesCollection.instance.chatHistory.tableNumber = GameSerializeClassesCollection.instance.observeTable.ticket;
        GameSerializeClassesCollection.instance.chatHistory.token = GameSerializeClassesCollection.instance.observeTable.token;

        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.chatHistory);
        print("__chat_message" + data);
        socket.Emit("__chat_message", new JSONObject(data));
    }

    string Chips;
    string playerCurrentChips;
    public bool isTopUpClick;
    public void OnManualTopUpEmitter()
    {
        isTopUpClick = true;
        SocialTournamentScript.instance.isTopUpPanelOn = false;
        SocialTournamentScript.instance.StopTimer();

        UIManagerScript.instance.newBuyInSlider.value = 0;
        for (int i = 0; i < GameManagerScript.instance.playersParent.transform.childCount; i++)
        {
            if (GameManagerScript.instance.playersParent.transform.GetChild(i).childCount == 2)
            {
                if (GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetComponent<PokerPlayerController>().isLocalPlayer)
                {
                    Chips = GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetComponent<PokerPlayerController>().chipsText.GetComponent<Text>().text;
                    playerCurrentChips = Chips.Replace(",", "");
                }
            }
        }

        GameSerializeClassesCollection.instance.onManualBuyIn.token = GameSerializeClassesCollection.instance.observeTable.token;
        GameSerializeClassesCollection.instance.onManualBuyIn.tableNumber = GameSerializeClassesCollection.instance.observeTable.ticket;
        GameSerializeClassesCollection.instance.onManualBuyIn.chips = System.Convert.ToInt32(playerCurrentChips);
        GameSerializeClassesCollection.instance.onManualBuyIn.user_image = ApiHitScript.instance.updatedUserImageUrl;
        GameSerializeClassesCollection.instance.onManualBuyIn.isZeroChips = isZeroChips;

        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.onManualBuyIn);
        print("__add_manual_topup: " + data);
        socket.Emit("__add_manual_topup", new JSONObject(data));
    }

    public void GetFriendList(string fJson)
    {
        Debug.Log(">>fjson " + fJson);
        socket.Emit("__friends_list", new JSONObject(fJson));
    }


    public void GetPendingList(string pJson)
    {
        Debug.Log(">>pJson" + pJson);
        socket.Emit("__pending_friends_list", new JSONObject(pJson));
    }

    public void SendInviteToUser(bool isvideo, string userName, string tableNumber, bool pushN)
    {

        GameSerializeClassesCollection.instance.inviteInstance.isVideo = GameManagerScript.instance.isVideoTable;
        GameSerializeClassesCollection.instance.inviteInstance.username = userName;
        GameSerializeClassesCollection.instance.inviteInstance.tableNumber = tableID;
        GameSerializeClassesCollection.instance.inviteInstance.pushNotification = pushN;
        GameSerializeClassesCollection.instance.inviteInstance.token = GameSerializeClassesCollection.instance.observeTable.token; ;
        GameSerializeClassesCollection.instance.inviteInstance.senderUserName = AccessGallery.instance.profileName[0].text;
        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.inviteInstance);
        socket.Emit("__invite_friends", new JSONObject(data));

        Debug.Log(">>invite Emit Data: " + data);
    }
    public void StandUpEmitter()
    {
        GameSerializeClassesCollection.instance.standUpPlayer.token = GameSerializeClassesCollection.instance.observeTable.token;
        GameSerializeClassesCollection.instance.standUpPlayer.tableNumber = GameSerializeClassesCollection.instance.observeTable.ticket;

        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.standUpPlayer);
        socket.Emit("__stand_up", new JSONObject(data));
        Debug.Log(">>__stand_up: " + data);
        UIManagerScript.instance.standUpButton.SetActive(false);
        //GameManagerScript.instance.isStandUp = true;
    }

    public void STObserverEmitter(bool resume)
    {
        GameSerializeClassesCollection.instance.standUpPlayer.resume = resume;
        GameSerializeClassesCollection.instance.standUpPlayer.token = GameSerializeClassesCollection.instance.observeTable.token;
        GameSerializeClassesCollection.instance.standUpPlayer.tableNumber = GameSerializeClassesCollection.instance.observeTable.ticket;

        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.standUpPlayer);
        socket.Emit("__st_observer", new JSONObject(data));
        Debug.Log(">>__st_observer: " + data);

    }

    public void SitInEmitter(int seatId)
    {
        GameSerializeClassesCollection.instance.sitInPlayerEmitter.token = GameSerializeClassesCollection.instance.observeTable.token;
        GameSerializeClassesCollection.instance.sitInPlayerEmitter.tableNumber = GameSerializeClassesCollection.instance.observeTable.ticket;
        GameSerializeClassesCollection.instance.sitInPlayerEmitter.user_image = ApiHitScript.instance.updatedUserImageUrl;
        GameSerializeClassesCollection.instance.sitInPlayerEmitter.seatId = seatId;

        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.sitInPlayerEmitter);
        socket.Emit("__sit_in", new JSONObject(data));            //Update
        Debug.Log(">>__sit_in: " + data);
    }

    public void StandUpChipsEmptyEmitter()
    {
        StopTopupCorotineFun();

        SocialTournamentScript.instance.isTopUpPanelOn = false;
        UIManagerScript.instance.emptypChipsPanel.SetActive(false);
        UIManagerScript.instance.emptypChipsLessBuyInPanel.SetActive(false);
        GameSerializeClassesCollection.SitInPlayerEmitter sitInPlayerEmitter = new GameSerializeClassesCollection.SitInPlayerEmitter();
        sitInPlayerEmitter.token = GameSerializeClassesCollection.instance.observeTable.token;
        sitInPlayerEmitter.tableNumber =  GameSerializeClassesCollection.instance.observeTable.ticket;
        sitInPlayerEmitter.seatId =  localPlayerSeatId;

       //GameSerializeClassesCollection.instance.sitInPlayerEmitter.token =        GameSerializeClassesCollection.instance.observeTable.token;
       // GameSerializeClassesCollection.instance.sitInPlayerEmitter.tableNumber = GameSerializeClassesCollection.instance.observeTable.ticket;
       // GameSerializeClassesCollection.instance.sitInPlayerEmitter.seatId =      localPlayerSeatId;
       // string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.sitInPlayerEmitter);

        string data = JsonUtility.ToJson(sitInPlayerEmitter);
        socket.Emit("__stand_up_empty", new JSONObject(data));
        Debug.Log(">>__stand_up_empty: " + data);
    }
    public void SwitchTableEmitter()
    {
        GameManagerScript.instance.isSwitchForTableSwitch = true;

        GameSerializeClassesCollection.instance.switchTableEmitter.token = GameSerializeClassesCollection.instance.observeTable.token;
        GameSerializeClassesCollection.instance.switchTableEmitter.tableNumber = GameSerializeClassesCollection.instance.observeTable.ticket;
        GameSerializeClassesCollection.instance.switchTableEmitter.switch_player_chips = localPlayer.transform.GetComponent<PokerPlayerController>().player.chips;

        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.switchTableEmitter);

        if (GameManagerScript.instance.totalPlayersSitting == 1)
        {

            //GameManagerScript.instance.isSwitchForTableSwitch = false;
            socket.Emit("__switch_table", new JSONObject(data));
            print("__switch_table" + data);
        }
        else
        {
            if (GameManagerScript.instance.isStandUp)
            {
                //GameManagerScript.instance.isSwitchForTableSwitch = false;
                socket.Emit("__switch_table", new JSONObject(data));
                print("__switch_table" + data);
            }
            else
            {
                UIManagerScript.instance.lowChipPanel.gameObject.SetActive(true);
                UIManagerScript.instance.lowChipPanelText.text = LanguageManager.Instance.GetTextValue("Your table will be switched");//"Your table will be switched after this hand";
                GameManagerScript.instance.isSwitchForTableSwitch = true;
            }
        }

    }
    public void CheckPlayerExist(string ticket)
    {
        GameSerializeClassesCollection.instance.checkPlayerExistEmitter.token = Communication.instance.playerToken;
        GameSerializeClassesCollection.instance.checkPlayerExistEmitter.ticket = ticket;
        GameSerializeClassesCollection.instance.checkPlayerExistEmitter.check_username = Chk_userName;

        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.checkPlayerExistEmitter);
        socket.Emit("__check_player_exist", new JSONObject(data));
        print("__check_player_exist" + data);
    }



    //......................................Emiiter Methods............................................//
    #endregion

    public IEnumerator SendInternetSpeed()
    {
        yield return new WaitForSeconds(5f);
        while (true)
        {
            GameSerializeClassesCollection.instance.internetSpeed.token = GameSerializeClassesCollection.instance.observeTable.token;
            GameSerializeClassesCollection.instance.internetSpeed.tableNumber = GameSerializeClassesCollection.instance.observeTable.ticket;
            GameSerializeClassesCollection.instance.internetSpeed.speed = ImageDownloadScript.instance.speed;

            string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.internetSpeed);
            //print("internetSpeed " + data);
            socket.Emit("__internet_speed", new JSONObject(data));
            yield return new WaitForSeconds(30f);
        }
    }

    public void FriendsConnectServer(Action<bool> _callback)
    {
        StartCoroutine(ConnectToServerForInviteFriend(_callback));
    }

    IEnumerator ConnectToServerForInviteFriend(Action<bool> _callback)
    {
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(TimeoutChecker(6));
        while (true)
        {
            socket.Connect();
            if (socket.wsConnected)
            {
                break;
            }
            else if (timedOut)
            {
                UnSubscribeToServerEvents();
                StopConnectToServer();
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.1f);

        if (socket.wsConnected)
        {
            _callback(true);
        }
        else
        {
            _callback(false);
        }
    }

    //.......ManualTopUpSlider......//
    bool isInitialSliderState;
    public void ManualTopUpSlider()
    {
        isInitialSliderState = true;
        if(isZeroChips)
        {
            if (UIManagerScript.instance.newBuyInSlider.value > 0 && UIManagerScript.instance.newBuyInSlider.value < UIManagerScript.instance.newBuyInSlider.maxValue)
            {
                UIManagerScript.instance.topUpPanelSocialPoker.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = (GameSerializeClassesCollection.instance.userChipBalance.min_buy_in + (UIManagerScript.instance.newBuyInSlider.value * 1000 * GameSerializeClassesCollection.instance.userChipBalance.stepper)).ToString();
            }
            else if (UIManagerScript.instance.newBuyInSlider.value == 0)
            {
                UIManagerScript.instance.topUpPanelSocialPoker.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.userChipBalance.min_buy_in.ToString();
            }
            else if (UIManagerScript.instance.newBuyInSlider.value == UIManagerScript.instance.newBuyInSlider.maxValue)
            {
                UIManagerScript.instance.topUpPanelSocialPoker.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.userChipBalance.max_buy_in.ToString();
            }
        }
        else
        {
            if (UIManagerScript.instance.newBuyInSlider.value > 0 && UIManagerScript.instance.newBuyInSlider.value < UIManagerScript.instance.newBuyInSlider.maxValue)
            {
                UIManagerScript.instance.topUpPanelSocialPoker.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = (UIManagerScript.instance.newBuyInSlider.value * GameSerializeClassesCollection.instance.userChipBalance.min_buy_in * GameSerializeClassesCollection.instance.userChipBalance.stepper).ToString();
            }
            else if (UIManagerScript.instance.newBuyInSlider.value == 0)
            {
                UIManagerScript.instance.topUpPanelSocialPoker.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.userChipBalance.min_buy_in.ToString();
            }
            else if (UIManagerScript.instance.newBuyInSlider.value == UIManagerScript.instance.newBuyInSlider.maxValue)
            {
                UIManagerScript.instance.topUpPanelSocialPoker.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.userChipBalance.max_buy_in.ToString();
            }
        }

        GameSerializeClassesCollection.instance.userChipBalance.buyIn = Convert.ToInt32(UIManagerScript.instance.topUpPanelSocialPoker.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text);
    
    }

    public void AddBtn()
    {
        isInitialSliderState = true;

        UIManagerScript.instance.newBuyInSlider.value++;

        if (isZeroChips)
        {
            UIManagerScript.instance.topUpPanelSocialPoker.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = (GameSerializeClassesCollection.instance.userChipBalance.min_buy_in + (UIManagerScript.instance.newBuyInSlider.value * 1000 * GameSerializeClassesCollection.instance.userChipBalance.stepper)).ToString();

        }
        else
        {

            UIManagerScript.instance.topUpPanelSocialPoker.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = ((UIManagerScript.instance.newBuyInSlider.value * GameSerializeClassesCollection.instance.userChipBalance.min_buy_in * GameSerializeClassesCollection.instance.userChipBalance.stepper)).ToString();

        }
        GameSerializeClassesCollection.instance.userChipBalance.buyIn = Convert.ToInt32(UIManagerScript.instance.topUpPanelSocialPoker.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text);

    }
    public void SubtractBtn()
    {
        UIManagerScript.instance.newBuyInSlider.value--;

        if (UIManagerScript.instance.newBuyInSlider.value == 0)
        {
            UIManagerScript.instance.topUpPanelSocialPoker.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.userChipBalance.min_buy_in.ToString();
        }
        else
        {
            if (isZeroChips)
            {
                UIManagerScript.instance.topUpPanelSocialPoker.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = (GameSerializeClassesCollection.instance.userChipBalance.min_buy_in + (UIManagerScript.instance.newBuyInSlider.value * 1000 * GameSerializeClassesCollection.instance.userChipBalance.stepper)).ToString();
            }
            else
            {
                UIManagerScript.instance.topUpPanelSocialPoker.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = ((UIManagerScript.instance.newBuyInSlider.value * GameSerializeClassesCollection.instance.userChipBalance.min_buy_in * GameSerializeClassesCollection.instance.userChipBalance.stepper)).ToString();
            }
        }
        GameSerializeClassesCollection.instance.userChipBalance.buyIn = Convert.ToInt32(UIManagerScript.instance.topUpPanelSocialPoker.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text);
    }

    IEnumerator ExitTable()
    {
        yield return new WaitForSeconds(2f);
        UIManagerScript.instance.TableToPokerUI(0);
    }
}
