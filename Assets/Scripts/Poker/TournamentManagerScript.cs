using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;
using UnityEngine.UI;
using SmartLocalization;

public class TournamentManagerScript : MonoBehaviour
{
    public static TournamentManagerScript instance;
    public SocketIOComponent socket;
    private bool timedOut;
    public bool time = false;
    public object entry;
    public int timeSubscribeEvent;
    public bool breaktimer;
    public bool isLateRegistration;
    public bool isPlayerLeft;
    public string tournamentId;

    public string videoChannel;
    public GameObject allBottomButtons;
    public GameObject localPlayer;

    public List<int> observerList;

    public string tableNoObserver;
    public string tournamentIdObserver;
    public string videoChannelObserver;

    public void Awake()
    {
        instance = this;
        entry = new object();
    }

    private void Start()
    {
        //SubscribeToServerEvents();
    }

    public void SubscribeOnReJoin()
    {
        socket.On("__internet_handler", OnResumeAllClients);
        socket.On("__resume_minimise_clients", OnResumeAllClients);
        socket.On("__remove_minimise_player", RemoveMinimizePlayer);
    }

    public void SubscribeToServerEvents()
    {
        timeSubscribeEvent++;
        socket.On("__mtt_entries", OnMTTEntry);
        //socket.On("__before_mtt_start", OnTimer);  
        socket.On("__all_clients", OnAllPeer);
        socket.On("__new_client", OnNewPeer);
        //socket.On("__st_data_check", Check);
        socket.On("__new_client_2", OnNewPeer2);
        socket.On("__low_chip_balance", OnLowChipBalance);
        socket.On("__chip_balance", OnChipBalance);
        socket.On("__game_prepare", OnGamePrepare);
        socket.On("__game_start", OnGameStart);
        socket.On("__new_round", NewRound);
        socket.On("__new_round_2", OnNewRound);
        socket.On("__new_round_broadcast", OnNewRoundBroadcast);
        socket.On("__next_chance", OnNextChance);
        socket.On("__action_performed", OnActionPerformed);
        socket.On("__next_deal", OnNextDeal);
        socket.On("__next_bet", OnNextBet);
        socket.On("__round_end", OnHandEnd);
        socket.On("__topup_chip_balance", OnTopUpChipBalance);
        socket.On("__left_2", OnPlayerLeft2);
        socket.On("__left", OnPlayerLeft);
        socket.On("__current_left_player", OnCurrentPlayerLeft);
        socket.On("__current_table_left", OnCurrentTableLeft);
        socket.On("__start_reload", OnStartReload);
        socket.On("__game_over", GameOver);
        socket.On("__disband_table", OnDisbandTableMessage);
        socket.On("__error_table_size", OnErrorTableSize);
        socket.On("__tournament_table_list", OnTableListing);
        socket.On("__add_on", AddOnListener);
        socket.On("__rebuy_chips", RebuyChipsListener);
        socket.On("__tournament_player_ranking", TournamentRankingListener);
        socket.On("__tournament_rewards_ranking", TournamentRewardListener);
        socket.On("__mtt_final_table", FinalTableListener);
        socket.On("__mtt_winner_details", WinnerTableListener);
        socket.On("__game_break", OnGameBreak);                 //Not in use currently
        socket.On("__game_break_timer", OnGameBreakTimer);
        socket.On("__add_on_break", AddOnBreak);                //Not in use currently
        socket.On("__add_on_break_timer", AddOnBreakTimer);
        socket.On("__mtt_re_join", MttRejoin);
        socket.On("__blinds_up", BlindUp);
        socket.On("__mtt_rejoin_alert_handler", OnRejoinTournament);
        socket.On("__mtt_exit_alert_handler", OnExitTournament);
        //socket.On("__resume_minimise_clients", OnResumeAllClients);
        socket.On("__st_count_down", CountDownDetailPage);
        socket.On("__st_throwables", Throwable);
        socket.On("__chat", ChatListener);
        socket.On("__chat_message", ChatHistoryListener);
        socket.On("__st_revoke_token", TokenRevoked);
        socket.On("__st_player_existence", PlayerExist);
        socket.On("__late_registration", LateRegistration);
        socket.On("__send_friend_request", OnSendingFriendRequest);
        socket.On("__blind_details", BlindDetails);
        socket.On("__tournament_timer", TournamentOnwardsTimer);
        socket.On("__mtt_next_level_timer", NextLevelTimer);
        socket.On("__all_clients_observer", AllClientObserver);
        socket.On("__mtt_observer_client", MttObserverClient);
        socket.On("player_position", PlayerPosition);

        socket.On("__mtt_observer_shuffle", ObserverShuffle);
        socket.On("__update_tournament_status", UpdateTourStatus);

        socket.On("__st_final_table", FinalTableMessage);
        socket.On("__notify_game_break_timer", NotifyGameBreak);
        socket.On("__notify_addon_break_timer", NotifyAddOnBreak);


    }

    public void UnSubscribeToServerEvents()
    {
        timeSubscribeEvent--;
        socket.Off("__mtt_entries", OnMTTEntry);
        //socket.Off("__before_mtt_start", OnTimer);
        socket.Off("__all_clients", OnAllPeer);
        socket.Off("__new_client", OnNewPeer);
        socket.Off("__new_client_2", OnNewPeer2);
        socket.Off("__low_chip_balance", OnLowChipBalance);
        socket.Off("__chip_balance", OnChipBalance);
        socket.Off("__game_prepare", OnGamePrepare);
        socket.Off("__game_start", OnGameStart);
        socket.Off("__new_round", NewRound);
        socket.Off("__new_round_2", OnNewRound);
        socket.Off("__new_round_broadcast", OnNewRoundBroadcast);
        socket.Off("__next_chance", OnNextChance);
        socket.Off("__action_performed", OnActionPerformed);
        socket.Off("__next_deal", OnNextDeal);
        socket.Off("__next_bet", OnNextBet);
        socket.Off("__round_end", OnHandEnd);
        socket.Off("__topup_chip_balance", OnTopUpChipBalance);
        socket.Off("__left_2", OnPlayerLeft2);
        socket.Off("__left", OnPlayerLeft);
        socket.Off("__current_left_player", OnCurrentPlayerLeft);
        socket.Off("__current_table_left", OnCurrentTableLeft);
        socket.Off("__start_reload", OnStartReload);
        socket.Off("__game_over", GameOver);
        socket.Off("__disband_table", OnDisbandTableMessage);
        socket.Off("__error_table_size", OnErrorTableSize);
        socket.Off("__tournament_table_list", OnTableListing);
        socket.Off("__add_on", AddOnListener);
        socket.Off("__rebuy_chips", RebuyChipsListener);
        socket.Off("__tournament_player_ranking", TournamentRankingListener);
        socket.Off("__tournament_rewards_ranking", TournamentRewardListener);
        socket.Off("__mtt_final_table", FinalTableListener);
        socket.Off("__mtt_winner_details", WinnerTableListener);
        socket.Off("__game_break", OnGameBreak);
        socket.Off("__game_break_timer", OnGameBreakTimer);
        socket.Off("__add_on_break", AddOnBreak);
        socket.Off("__add_on_break_timer", AddOnBreakTimer);
        socket.Off("__mtt_re_join", MttRejoin);
        socket.Off("__blinds_up", BlindUp);
        socket.Off("__mtt_rejoin_alert_handler", OnRejoinTournament);
        socket.Off("__mtt_exit_alert_handler", OnExitTournament);
        socket.Off("__resume_minimise_clients", OnResumeAllClients);
        socket.Off("__internet_handler", OnResumeAllClients);
        socket.Off("__st_count_down", CountDownDetailPage);
        socket.Off("__st_throwables", Throwable);
        socket.Off("__chat", ChatListener);
        socket.Off("__chat_message", ChatHistoryListener);
        socket.Off("__st_revoke_token", TokenRevoked);
        socket.Off("__remove_minimise_player", RemoveMinimizePlayer);
        socket.Off("__st_player_existence", PlayerExist);
        socket.Off("__late_registration", LateRegistration);
        socket.Off("__send_friend_request", OnSendingFriendRequest);
        socket.Off("__blind_details", BlindDetails);
        socket.Off("__tournament_timer", TournamentOnwardsTimer);
        socket.Off("__mtt_next_level_timer", NextLevelTimer);
        socket.Off("__all_clients_observer", AllClientObserver);
        socket.Off("__mtt_observer_client", MttObserverClient);
        socket.Off("player_position", PlayerPosition);

        socket.Off("__mtt_observer_shuffle", ObserverShuffle);
        socket.Off("__update_tournament_status", UpdateTourStatus);

        socket.Off("__st_final_table", FinalTableMessage);
        socket.Off("__notify_game_break_timer", NotifyGameBreak);
        socket.Off("__notify_addon_break_timer", NotifyAddOnBreak);


    }

    #region ConnectToServer
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

    void StopConnectToServer()
    {
        StopCoroutine("ServerConnect");
        if (!GameManagerScript.instance.isDirectLogIn)
        {
            ClubManagement.instance.loadingPanel.SetActive(false);
            Cashier.instance.toastMsgPanel.SetActive(true);
            Cashier.instance.toastMsg.text = "Oops something went wrong. Please try again.";
        }
    }

    public void ConnectToServer()
    {
        StartCoroutine("ServerConnect");
    }

    IEnumerator ServerConnect()
    {
        StartCoroutine(TimeoutChecker(6));      //10sec    Update
        while (true)
        {
            socket.Connect();
            if (socket.wsConnected)
            {
                if (timeSubscribeEvent == 0)
                {
                    SubscribeToServerEvents();
                }
                break;
            }
            else if (timedOut)
            {
                StopConnectToServer();
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        print("Connect to Server");

        //GameManagerScript.instance.isObserver = true;
        //TournamentGameDetail.instance.RuleIdRequest();
        //GameManagerScript.instance.isVideoTable = ClubManagement.instance.currentSelectedTableIsVideoMode;
        //GameManagerScript.instance.InitlializeOnStart();
        if (socket.wsConnected)
        {
            print("socket conected for Tournament");

            StartCoroutine(SocialTournamentScript.instance.TournamentCheck());

            StartCoroutine(GameManagerScript.instance.Internet());
        }
    }
    #endregion

    #region Listener Methods 

    // sendingFriendRequest..

    void OnSendingFriendRequest(SocketIOEvent socketIOEvent)
    {
        //  print(">>>>>> socket data" +socketIOEvent.data.ToString());

        GameSerializeClassesCollection.instance.addfrindRequestTour = JsonUtility.FromJson<GameSerializeClassesCollection.AddingFriendParametersTournament>(socketIOEvent.data.ToString());
        FriendandSocialScript.instance.ShowAddFriendPopUp(null, socketIOEvent.data.ToString());
    }

    public bool isCountdown;
    void CountDownDetailPage(SocketIOEvent socketIOEvent)
    {
        isCountdown = true;
        string data = socketIOEvent.data.ToString();
        //print("__st_count_down : " + data);
        GameSerializeClassesCollection.instance.timerCountDown = JsonUtility.FromJson<GameSerializeClassesCollection.TimerCountDown>(data);
        if (GameSerializeClassesCollection.instance.timerCountDown.tournament_id == SocialTournamentScript.instance.tournament_ID && GameSerializeClassesCollection.instance.tournament.tournament_status == 0)
        {
            SocialTournamentScript.instance.timerText.text = (GameSerializeClassesCollection.instance.timerCountDown.hours + ":" + GameSerializeClassesCollection.instance.timerCountDown.minutes + ":" + GameSerializeClassesCollection.instance.timerCountDown.seconds).ToString();
        }

        if (int.Parse(GameSerializeClassesCollection.instance.timerCountDown.hours) == 0 && int.Parse(GameSerializeClassesCollection.instance.timerCountDown.minutes) == 0 && int.Parse(GameSerializeClassesCollection.instance.timerCountDown.seconds) <= 2 && GameSerializeClassesCollection.instance.tournament.tournament_status == 0)
        {
            TimerEmitter();
        }

        if (int.Parse(GameSerializeClassesCollection.instance.timerCountDown.hours) == 0 && int.Parse(GameSerializeClassesCollection.instance.timerCountDown.minutes) == 0 && int.Parse(GameSerializeClassesCollection.instance.timerCountDown.seconds) <= 3 && GameSerializeClassesCollection.instance.tournament.tournament_status == 0)
        {
            isCountdown = false;
            SocialTournamentScript.instance.CheckRegisterStatus();
        }

        if (int.Parse(GameSerializeClassesCollection.instance.timerCountDown.hours) == 0 && int.Parse(GameSerializeClassesCollection.instance.timerCountDown.minutes) == 0 && int.Parse(GameSerializeClassesCollection.instance.timerCountDown.seconds) <= 1 && GameSerializeClassesCollection.instance.tournament.tournament_status == 0 && SocialTournamentScript.instance.isRegistered && SocialTournamentScript.instance.tournamentGameDetailPanel.activeInHierarchy)
        {
            print("tournamentGameDetailPanel: False");
            print("STATUS: " + GameSerializeClassesCollection.instance.tournament.tournament_status);
            GameManagerScript.instance.InitlializeOnStart();
            SocialGame.instance.pokerUICanvas.SetActive(false);

            if (SocialTournamentScript.instance.isTournamentVideo)
            {
                Screen.orientation = ScreenOrientation.Landscape;
            }
        }
    }

    void TournamentRankingListener(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__tournament_player_ranking : " + data);
        GameSerializeClassesCollection.instance.mttRankingListingData = JsonUtility.FromJson<GameSerializeClassesCollection.MttRankingListingData>(data);
        if (GameSerializeClassesCollection.instance.mttRankingListingData.tournament_id == SocialTournamentScript.instance.tournament_ID)
        {
            SocialTournamentScript.instance.RankingListing();
            print("Ranking Listener");
        }
    }

    void FinalTableListener(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__mtt_final_table : " + data);
        //UIManagerScript.instance.lowChipPanel.gameObject.SetActive(true);
        UIManagerScript.instance.lowChipPanelText.text = LanguageManager.Instance.GetTextValue("Final Showdown");//"Final Showdown";
    }

    void PlayerExist(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__st_player_existence : " + data);
        GameSerializeClassesCollection.instance.playerExistListener = JsonUtility.FromJson<GameSerializeClassesCollection.PlayerExistListener>(data);

        if (GameSerializeClassesCollection.instance.playerExistListener.alreadyRegistered && GameSerializeClassesCollection.instance.playerExistListener.exist)
        {
            SocialTournamentScript.instance.TurnOnOffBottomPanelBtns(6, -1, false);
        }

        else if (GameSerializeClassesCollection.instance.playerExistListener.alreadyRegistered && !GameSerializeClassesCollection.instance.playerExistListener.exist)
        {
            SocialTournamentScript.instance.TurnOnOffBottomPanelBtns(7, -1, false);
        }

        else if (!GameSerializeClassesCollection.instance.playerExistListener.alreadyRegistered && !GameSerializeClassesCollection.instance.playerExistListener.exist && GameSerializeClassesCollection.instance.tournament.tournament_status == 1)
        {
            print("player NOT Exist");
            SocialTournamentScript.instance.TurnOnOffBottomPanelBtns(4, 5, false);
        }

    }

    void LateRegistration(SocketIOEvent socketIOEvent)
    {
        isLateRegistration = true;
        string data = socketIOEvent.data.ToString();
        print("__late_registration: " + data);

        GameSerializeClassesCollection.instance.lateRegistrationListener = JsonUtility.FromJson<GameSerializeClassesCollection.LateRegistrationListener>(data);

        if (GameSerializeClassesCollection.instance.lateRegistrationListener.error)
        {
            SocialTournamentScript.instance.CloseAllBottomButtons();
        }
        else
        {
            SocialTournamentScript.instance.CloseAllBottomButtons();
            SocialTournamentScript.instance.TurnOnOffBottomPanelBtns(6, -1, false);
        }
    }
    //void LateRegistration(SocketIOEvent socketIOEvent)
    //{
    //    isLateRegistration = true;

    //    SocialTournamentScript.instance.TurnOnOffBottomPanelBtns(6, -1, false);

    //    string data = socketIOEvent.data.ToString();
    //    print("__late_registration: " + data);

    //    GameSerializeClassesCollection.instance.lateRegistrationListener = JsonUtility.FromJson<GameSerializeClassesCollection.LateRegistrationListener>(data);
    //}

    void MttRejoin(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__mtt_re_join : " + data);
    }

    void BlindUp(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__blinds_up : " + data);
        GameSerializeClassesCollection.instance.mttBlindsListingData = JsonUtility.FromJson<GameSerializeClassesCollection.MttBlindsListingData>(data);
        Table.instance.BlindsListing();
    }

    void WinnerTableListener(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__mtt_winner_details : " + data);
        GameSerializeClassesCollection.instance.mttWinnerListingData = JsonUtility.FromJson<GameSerializeClassesCollection.MttWinnerListingData>(data);
        //UIManagerScript.instance.tournmtWinnerPanel.gameObject.SetActive(true);

        TournamentGameDetail.instance.WinRankListing();
    }

    void TournamentRewardListener(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__tournament_rewards_ranking : " + data);
        GameSerializeClassesCollection.instance.mttRewardListingData = JsonUtility.FromJson<GameSerializeClassesCollection.MttRewardListingData>(data);
        if (GameSerializeClassesCollection.instance.mttRewardListingData.tournament_id == SocialTournamentScript.instance.tournament_ID)
        {
            SocialTournamentScript.instance.RewardsListing();
        }
    }

    void OnMTTEntry(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__mtt_entries: " + data);

        GameSerializeClassesCollection.instance.tournament = JsonUtility.FromJson<GameSerializeClassesCollection.Tournament>(data);
        if (GameSerializeClassesCollection.instance.tournament.tournament_status == 3)
        {
            SocialTournamentScript.instance.TurnOnOffBottomPanelBtns(0, 0, true);
        }

        if (GameSerializeClassesCollection.instance.tournament.tournament_id == SocialTournamentScript.instance.tournament_ID && GameManagerScript.instance.activeTable == null)
        {
            print("UpdateGameDetailPage....");
            if (isCountdown || SocialTournamentScript.instance.isRegistered)
            {
                SocialTournamentScript.instance.UpdateGameDetailPage();
            }
           
        }

        if (GameSerializeClassesCollection.instance.tournament.tournament_id == SocialTournamentScript.instance.tournament_ID && GameManagerScript.instance.activeTable != null && UIManagerScript.instance.mttSideInfoPanel != null && UIManagerScript.instance.mttSideInfoPanel.activeInHierarchy)
        {
            print("UpdateSidePanelInfo....");
            SocialTournamentScript.instance.UpdateSidePanelInfo();
        }
    }

    //void OnTimer(SocketIOEvent socketIOEvent)
    //{
    //    string data = socketIOEvent.data.ToString();
    //    print("OnTimer Listener : " + data);
    //    GameSerializeClassesCollection.instance.timerData = JsonUtility.FromJson<GameSerializeClassesCollection.TimerData>(data);
    //    if (GameSerializeClassesCollection.instance.timerData.error)
    //    {
    //        TournamentGameDetail.instance.tableCancelledPanel.SetActive(true);
    //    }
    //}

    //void Check(SocketIOEvent socketIOEvent)
    //{
    //    string data = socketIOEvent.data.ToString();
    //    print("__st_data_check : " + data);
    //}

    public string tableNo;
    // __all_clients listener

    void OnAllPeer(SocketIOEvent socketIOEvent) //.....allready Player on Table...//
    {

        print("ALL CLIENTS");
        //GameManagerScript.instance.InitlializeOnStart();
        //SocialGame.instance.pokerUICanvas.SetActive(false);
        UIManagerScript.instance.loadingPanel.SetActive(true);
        UIManagerScript.instance.loadingPanel.transform.GetChild(1).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("Setting Up");
        //UIManagerScript.instance.mttSideInfoPanelSymbol.SetActive(true);
        //UIManagerScript.instance.tableInfo.gameObject.SetActive(true);

        //if (!GameManagerScript.instance.activeTable.activeInHierarchy)
        //{
        //    GameManagerScript.instance.activeTable.SetActive(true);
        //}

        string data = socketIOEvent.data.ToString();
        print("__all _ clients : " + data);

        GameSerializeClassesCollection.AllPlayers allPlayers = JsonUtility.FromJson<GameSerializeClassesCollection.AllPlayers>(data);
        //TableWebAPICommunication.instance.StartGetObserverCoroutine();
        tableNo = allPlayers.tableNumber.ToString();
        Table.instance.table.status = allPlayers.tableStatus;
        breaktimer = allPlayers.break_time;
        tournamentId = allPlayers.tournament_id;
        UIManagerScript.instance.tableInfo.gameObject.SetActive(false);
        videoChannel = tableNo + tournamentId;

        if (allPlayers.tournament_id == SocialTournamentScript.instance.tournament_ID)
        {
            //SocialTournamentScript.instance.bottomPanel.transform.GetChild(2).gameObject.SetActive(false);
            //SocialTournamentScript.instance.bottomPanel.transform.GetChild(3).gameObject.SetActive(true);

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
                        GameSerializeClassesCollection.instance.newLocalPlayer.player.tournament_id = allPlayers.players[i].tournament_id;

                        GameSerializeClassesCollection.instance.newLocalPlayer.player.user_image = allPlayers.players[i].user_image;

                        GameManagerScript.instance.chairAnimForLocalPlayer = false;

                        GameManagerScript.instance.localSeatIDDummy = GameSerializeClassesCollection.instance.newLocalPlayer.player.seatId;
                        //if (GameManagerScript.instance.isVideoTable)
                        //{
                        //  GameManagerScript.instance.OnVideoEngine(tableNo + GameSerializeClassesCollection.instance.objMTTJoinClass.tournament_id, GameSerializeClassesCollection.instance.newLocalPlayer.player.clientId);
                        //}
                        StartCoroutine(AssignSeat(0f, GameSerializeClassesCollection.instance.newLocalPlayer));
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
                if (/*allPlayers.basicData != null*/ allPlayers.basicData.players.Length != 0)
                {
                    CardShuffleAnimation.instance.isAnimationComplete = true;
                    GameManagerScript.instance.AllBasicDataUpdate(allPlayers.basicData);
                    Table.instance.UpdateTotalPot();
                    GameManagerScript.instance.UpdateTable(allPlayers.basicData.table);
                }
            }
            catch
            {
                CardShuffleAnimation.instance.isAnimationComplete = true;
                print("no basic data");
            }
            TournamentGameDetail.instance.TimerCompleted();

            try
            {
                if (!allPlayers.break_time)
                {
                    if (allPlayers.checkActionPlayer)
                    {
                        GameManagerScript.instance.minBet = allPlayers.currentActionPlayerDetails.player.minBet;
                        Table.instance.roundNameFromGameClass = allPlayers.currentActionPlayerDetails.game.roundName;
                        PlayerActionManagement.instance.ShowPlayerCommands(allPlayers.currentActionPlayerDetails.player, 1.5f);
                    }
                }
            }
            catch
            {
                print("Error in current Action PlayerDetails");
            }
            UIManagerScript.instance.loadingPanel.SetActive(false);
        }
    }

    // __new_client listener
    void OnNewPeer(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__new _ client: " + data);

        GameSerializeClassesCollection.Players localPlayer = JsonUtility.FromJson<GameSerializeClassesCollection.Players>(data);
        /*

      #if !UNITY_EDITOR
              if (GameManagerScript.instance.isVideoTable)
              {
                  //if (localPlayer.player.counter == 1)
                  //{
                  //StartCoroutine(InitialzeAgora(localPlayer.player.counter, localPlayer.player.clientId, tableNo + GameSerializeClassesCollection.instance.objMTTJoinClass.tournament_id));
                  GameManagerScript.instance.OnVideoEngine(tableNo + GameSerializeClassesCollection.instance.objMTTJoinClass.tournament_id, localPlayer.player.clientId);
                  //GameManagerScript.instance.OnVideoEngine(localPlayer.player.clientId, tableNo + GameSerializeClassesCollection.instance.objMTTJoinClass.tournament_id.ToString());
                  //}
              }
      #endif
              //PlayersGenerator.instance.InstantiateLocalPlayer(localPlayer);
        */
        StartCoroutine(AssignSeat(0, localPlayer));
    }

    IEnumerator AssignSeat(float delay, GameSerializeClassesCollection.Players localPlayer)
    {
        yield return new WaitForSeconds(delay);
        GameManagerScript.instance.localSeatIDDummy = localPlayer.player.seatId;

        if (tournamentId == SocialTournamentScript.instance.tournament_ID)
        {
            PlayersGenerator.instance.InstantiateLocalPlayer(localPlayer, false);
        }
    }
    // __new_client_2 listener
    void OnNewPeer2(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__new _ client _2: " + data);

        lock (entry)
        {
            GameSerializeClassesCollection.Players localPlayer = JsonUtility.FromJson<GameSerializeClassesCollection.Players>(data);
            if (localPlayer.tournament_id == SocialTournamentScript.instance.tournament_ID)
            {
                PlayersGenerator.instance.InstantiateNewRemotePlayer(localPlayer);
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
        UIManagerScript.instance.loadingPanel.SetActive(true);
        string data = socketIOEvent.data.ToString();
        print("Game Counter: " + data);
        GameSerializeClassesCollection.GamePrepare gamePrepare = JsonUtility.FromJson<GameSerializeClassesCollection.GamePrepare>(data);
        if (gamePrepare.countDown > 0)
        {
            Table.instance.gameStartCounter.enabled = true;
            Table.instance.gameStartCounter.text = gamePrepare.countDown.ToString();
            Table.instance.tableNumber.text = gamePrepare.tableNumber.ToString();
        }
        if (gamePrepare.countDown == 1)
        {
            /*
            
            //#if !UNITY_EDITOR
            if (GameManagerScript.instance.isVideoTable)
            {
                PlayersGenerator.instance.AssignVideoPanelsToPlayers();
            }
            //#endif

            */
        }
        StartCoroutine(Table.instance.TurnOffCounter());
    }

    // __game_start listener
    void OnGameStart(SocketIOEvent socketIOEvent)
    {
        UIManagerScript.instance.loadingPanel.SetActive(false);
        string data = socketIOEvent.data.ToString();
        print("Game Sart: " + data);
        GameSerializeClassesCollection.GameStart gameStart = JsonUtility.FromJson<GameSerializeClassesCollection.GameStart>(data);
        //UIManagerScript.instance.popUpPanel.gameObject.SetActive(true);
        //UIManagerScript.instance.popUpPanelText.text = gameStart.msg;
    }

    // __new_round_2 listener
    void OnNewRound(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("On New Round 2: " + data);
        GameSerializeClassesCollection.RoundStartInfo roundStartInfoObj = JsonUtility.FromJson<GameSerializeClassesCollection.RoundStartInfo>(data);
        // minBet = roundStartInfoObj.player.minBet;
        if (roundStartInfoObj.tournament_id == SocialTournamentScript.instance.tournament_ID)
        {
            PlayerActionManagement.instance.OnNewRound2(roundStartInfoObj);
        }
    }

    // __new_round_broadcast listener
    void OnNewRoundBroadcast(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("broadcast New Round Broadcast: " + data);
        GameSerializeClassesCollection.RoundStartInfo roundStartInfoObj = JsonUtility.FromJson<GameSerializeClassesCollection.RoundStartInfo>(data);

        if (roundStartInfoObj.tournament_id == SocialTournamentScript.instance.tournament_ID)
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
                print("Some error in OnNewRoundBroadcast TournamentNetworkManager.cs");
            }
        }
    }

    // __next_chance listener
    void OnNextChance(SocketIOEvent socketIOEvent)
    {
        UIManagerScript.instance.loadingPanel.SetActive(false);
        string data = socketIOEvent.data.ToString();
        print("Next chance: " + data);
        GameSerializeClassesCollection.NextChance nextChance = JsonUtility.FromJson<GameSerializeClassesCollection.NextChance>(data);

        if (nextChance.tournament_id == SocialTournamentScript.instance.tournament_ID)
        {
            GameManagerScript.instance.minBet = nextChance.player.minBet;
            Table.instance.roundNameFromGameClass = nextChance.game.roundName;
            PlayerActionManagement.instance.ShowPlayerCommands(nextChance.player, 0f);
        }
    }

    // __action_performed listener
    void OnActionPerformed(SocketIOEvent socketIOEvent)
    {
        UIManagerScript.instance.loadingPanel.SetActive(false);
        string data = socketIOEvent.data.ToString();
        print("On  Action Performed: " + data);
        GameSerializeClassesCollection.ActionPerformed actionPerformed = JsonUtility.FromJson<GameSerializeClassesCollection.ActionPerformed>(data);

        if (actionPerformed.tournament_id == SocialTournamentScript.instance.tournament_ID)
        {
            PlayerActionManagement.instance.ActionPerformedResult(actionPerformed);
        }
        //if (Table.instance.roundNameFromGameClass != "Deal")
        //{
        //    Table.instance.TotalPot.text = (int.Parse(Table.instance.TotalPot.text) + int.Parse(Table.instance.RoundPot.text)).ToString();
        //}

    }

    // __next_deal listener
    void OnNextDeal(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("Next Deal: " + data);
        GameSerializeClassesCollection.NextDealInfo nextDealInfo = JsonUtility.FromJson<GameSerializeClassesCollection.NextDealInfo>(data);

        if (nextDealInfo.tournament_id == SocialTournamentScript.instance.tournament_ID)
        {
            GameManagerScript.instance.UpdateTable(nextDealInfo.table);

            //UpdatePlayerBet(nextDealInfo);
            StartCoroutine(GameManagerScript.instance.UpdatePlayerOnNextDeal(nextDealInfo));
        }
    }

    // __next_bet listener
    void OnNextBet(SocketIOEvent socketIOEvent)
    {
        UIManagerScript.instance.loadingPanel.SetActive(false);
        string data = socketIOEvent.data.ToString();
        print("Next Bet: " + data);
        GameSerializeClassesCollection.NextChance nextChance = JsonUtility.FromJson<GameSerializeClassesCollection.NextChance>(data);

        if (nextChance.tournament_id == SocialTournamentScript.instance.tournament_ID)
        {
            GameManagerScript.instance.minBet = nextChance.player.minBet;
            Table.instance.roundNameFromGameClass = nextChance.game.roundName;
            PlayerActionManagement.instance.ShowPlayerCommands(nextChance.player, 0f);
        }
    }

    // __round_end listener
    void OnHandEnd(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("Hand end (__round_end): " + data);
        //GameSerializeClassesCollection.WinningHand winningHand = JsonUtility.FromJson<GameSerializeClassesCollection.WinningHand>(data);
        GameSerializeClassesCollection.instance.winningHand = JsonUtility.FromJson<GameSerializeClassesCollection.WinningHand>(data);

        if (GameSerializeClassesCollection.instance.winningHand.tournament_id == SocialTournamentScript.instance.tournament_ID)
        {
            print("Hand end (__round_end): " + GameSerializeClassesCollection.instance.winningHand.players[0].playerName);
            //print("Hand end (__round_end): " + GameSerializeClassesCollection.instance.winningHand.players[1].playerName);
            WinningLogic.instance.OnHandComplete(GameSerializeClassesCollection.instance.winningHand);
        }
    }

    // __topup_chip_balance listener
    void OnTopUpChipBalance(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__topup_chip_balance: " + data);

        GameSerializeClassesCollection.instance.topUpChipBalance = JsonUtility.FromJson<GameSerializeClassesCollection.ChipBalance>(data);
        //  UIManagerScript.instance.TopUpFunctionality();
    }

    void OnPlayerLeft(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("On Player Left : __left : " + data);
        //GameSerializeClassesCollection.Players playerLeft = JsonUtility.FromJson<GameSerializeClassesCollection.Players>(data);
        //PlayersGenerator.instance.OnPlayerLeft(playerLeft);
    }

    void OnPlayerLeft2(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print(" __left_2 : " + data);
        //GameSerializeClassesCollection.Players playerLeft = JsonUtility.FromJson<GameSerializeClassesCollection.Players>(data);
        //PlayersGenerator.instance.OnPlayerLeft(playerLeft);
    }

    void OnCurrentPlayerLeft(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__current_left_player : " + data);
        GameSerializeClassesCollection.instance.onCurrentPlayerLeft = JsonUtility.FromJson<GameSerializeClassesCollection.OnPlayerLeft>(data);
        print("__current_left_player isGuest : " + GameSerializeClassesCollection.instance.onCurrentPlayerLeft.isGuest);

        if (GameSerializeClassesCollection.instance.onCurrentPlayerLeft.tournament_id == SocialTournamentScript.instance.tournament_ID)
        {
            if (GameObject.Find(GameSerializeClassesCollection.instance.onCurrentPlayerLeft.playerName) != null)
            {
                GameObject currplayer = GameObject.Find(GameSerializeClassesCollection.instance.onCurrentPlayerLeft.playerName);
                if (currplayer.GetComponent<PokerPlayerController>().isLocalPlayer /*== int.Parse(GameSerializeClassesCollection.instance.onCurrentPlayerLeft.clientId)*/)
                {
                    if (TournamentScript.instance.seteliteBool)
                    {
                        UIManagerScript.instance.gameLeftPanelSatelite.GetChild(0).GetChild(4).GetChild(0).GetComponent<Text>().text = TournamentScript.instance.sateliteTournamentText.text;
                        UIManagerScript.instance.gameLeftPanelSatelite.gameObject.SetActive(true);
                    }
                    else
                    {

                        print("image........00000000000");
                        localPlayer = currplayer;
                        isPlayerLeft = true;
                        //UIManagerScript.instance.TableToPokerUI(0);
                        UIManagerScript.instance.gameLeftPanelTournment.gameObject.SetActive(true);

                        if (GameSerializeClassesCollection.instance.onCurrentPlayerLeft.user_image != null)
                        {
                            Communication.instance.GetImage(GameSerializeClassesCollection.instance.onCurrentPlayerLeft.user_image, ProfileImage);
                        }

                        UIManagerScript.instance.gameLeftPanelTournment.GetChild(0).GetChild(3).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.onCurrentPlayerLeft.playerName;
                        // UIManagerScript.instance.gameLeftPanelTournment.GetChild(0).GetChild(4).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.onCurrentPlayerLeft.clientId;
                        UIManagerScript.instance.gameLeftPanelTournment.GetChild(0).GetChild(5).GetChild(0).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.onCurrentPlayerLeft.rank.ToString() + "/" + GameSerializeClassesCollection.instance.onCurrentPlayerLeft.tournment_entries_count.ToString();
                        UIManagerScript.instance.gameLeftPanelTournment.GetChild(0).GetChild(9).GetChild(0).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.onCurrentPlayerLeft.prize.ToString();
                        print("image........111111111111");
                        print("image........" + GameSerializeClassesCollection.instance.onCurrentPlayerLeft.user_image);
                        

                    }
                    UIManagerScript.instance.ReShuffleTable();
                    //AgoraInit.instance.LeaveChannel();
                    print("Channel Leave");
                }
                else
                {
                    PlayersGenerator.instance.CurrentPlayerLeft();
                }
            }
        }
    }

    void OnCurrentTableLeft(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__current_table_left : " + data);
        GameSerializeClassesCollection.instance.onCurrentPlayerLeft = JsonUtility.FromJson<GameSerializeClassesCollection.OnPlayerLeft>(data);

        if (GameSerializeClassesCollection.instance.onCurrentPlayerLeft.tournament_id == SocialTournamentScript.instance.tournament_ID)
        {
            print("__current_table_left : " + GameSerializeClassesCollection.instance.onCurrentPlayerLeft.playerName);
            if (GameObject.Find(GameSerializeClassesCollection.instance.onCurrentPlayerLeft.playerName) != null)
            {
                GameObject currplayer = GameObject.Find(GameSerializeClassesCollection.instance.onCurrentPlayerLeft.playerName);
                if (currplayer.GetComponent<PokerPlayerController>().isLocalPlayer /*== int.Parse(GameSerializeClassesCollection.instance.onCurrentPlayerLeft.clientId)*/)
                {
                    localPlayer = currplayer;
                    UIManagerScript.instance.ReShuffleTable();
                    print("ReShuffleTable");
                }
                else
                {
                    PlayersGenerator.instance.CurrentPlayerLeft();
                    print("CurrentPlayerLeft");
                }
            }
        }
        //UIManagerScript.instance.isPlayerShuffle = true;
    }


    void MeLeft(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("me left : __me_left : " + data);
        //GameSerializeClassesCollection.instance.onCurrentPlayerLeft = JsonUtility.FromJson<GameSerializeClassesCollection.OnPlayerLeft>(data);
        //PlayersGenerator.instance.CurrentPlayerLeft();
    }

    void OnStartReload(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__start_reload : " + data);
        GameSerializeClassesCollection.instance.onNewRound = JsonUtility.FromJson<GameSerializeClassesCollection.OnNewRound>(data);
        //print("__start_reload : " + GameSerializeClassesCollection.instance.onstartReload.players[0].playerName);
        //print("__start_reload : " + GameSerializeClassesCollection.instance.onstartReload.players[1].playerName);
        //PlayerActionManagement.instance.StartReloadHand(GameSerializeClassesCollection.instance.onstartReload);
    }

    void NewRound(SocketIOEvent socketIOEvent)
    {
        MttEntyEmitter(false);
        UIManagerScript.instance.loadingPanel.SetActive(false);
        string data = socketIOEvent.data.ToString();
        print("New Round : " + data);
        GameSerializeClassesCollection.instance.onNewRound = JsonUtility.FromJson<GameSerializeClassesCollection.OnNewRound>(data);

        if (GameSerializeClassesCollection.instance.onNewRound.table.tournamentId == SocialTournamentScript.instance.tournament_ID)
        {
            print("New Round : " + GameSerializeClassesCollection.instance.onNewRound.players[0].playerName);
            //print("New Round : " + GameSerializeClassesCollection.instance.onNewRound.players[1].playerName);
            PlayerActionManagement.instance.NewRound(GameSerializeClassesCollection.instance.onNewRound);

            GameSerializeClassesCollection.Basicdata tableInfo = JsonUtility.FromJson<GameSerializeClassesCollection.Basicdata>(data);
            //Table.instance.LevelAndBlinds(tableInfo);
            UIManagerScript.instance.CheckLateRegLevel();
        }
        UIManagerScript.instance.breakTimePanel.SetActive(false);
    }

    void SeatOccupied(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("Seat Occupied : " + data);
        GameSerializeClassesCollection.instance.seatOccupied = JsonUtility.FromJson<GameSerializeClassesCollection.OnLowChipBalance>(data);
        //UIManagerScript.instance.lowChipPanel.gameObject.SetActive(true);
        UIManagerScript.instance.lowChipPanelText.text = LanguageManager.Instance.GetTextValue("Seat is already Taken");//"Seat is already Taken. Click on any other seat";
        UIManagerScript.instance.lowChipPanelText.text = GameSerializeClassesCollection.instance.seatOccupied.message;
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
        UIManagerScript.instance.lowChipPanel.gameObject.SetActive(true);
        UIManagerScript.instance.lowChipPanelText.text = GameSerializeClassesCollection.instance.disbandTableMessage.message;/* "No Seat Available";*/
    }

    void GameOver(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("Game Over : " + data);
        UIManagerScript.instance.TableToPokerUI(3);
    }

    void AddOnListener(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__add_on  : " + data);

        GameSerializeClassesCollection.instance.addOn = JsonUtility.FromJson<GameSerializeClassesCollection.AddOn>(data);
        if (!GameSerializeClassesCollection.instance.addOn.errorStatus)
        {
            UIManagerScript.instance.addOnPanel.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<Text>().text = GameManagerScript.instance.KiloFormat( GameSerializeClassesCollection.instance.addOn.fees).ToString();
            UIManagerScript.instance.addOnPanel.transform.GetChild(0).GetChild(1).GetChild(4).GetComponent<Text>().text = GameManagerScript.instance.KiloFormat( GameSerializeClassesCollection.instance.addOn.chips).ToString();
            UIManagerScript.instance.addOnPanel.transform.GetChild(0).GetChild(1).GetChild(8).GetChild(0).GetComponent<Text>().text = GameManagerScript.instance.KiloFormat( GameSerializeClassesCollection.instance.addOn.buyIn).ToString();
            UIManagerScript.instance.addOnPanel.SetActive(true);
        }
        else
        {
            print(GameSerializeClassesCollection.instance.addOn.errorStatus + "Low Chip Balance of Player");
        }
    }

    void RebuyChipsListener(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__rebuy_chips  : " + data);

        GameSerializeClassesCollection.instance.rebuyChips = JsonUtility.FromJson<GameSerializeClassesCollection.RebuyChips>(data);
        if (!GameSerializeClassesCollection.instance.rebuyChips.errorStatus)
        {
            UIManagerScript.instance.rebuyPanel.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<Text>().text = GameManagerScript.instance.KiloFormat(GameSerializeClassesCollection.instance.rebuyChips.fees).ToString();
            UIManagerScript.instance.rebuyPanel.transform.GetChild(0).GetChild(1).GetChild(4).GetComponent<Text>().text = GameManagerScript.instance.KiloFormat(GameSerializeClassesCollection.instance.rebuyChips.chips).ToString();
            UIManagerScript.instance.rebuyPanel.transform.GetChild(0).GetChild(1).GetChild(8).GetChild(0).GetComponent<Text>().text = GameManagerScript.instance.KiloFormat(GameSerializeClassesCollection.instance.rebuyChips.buyIn).ToString();
            UIManagerScript.instance.rebuyPanel.SetActive(true);
        }
        else
        {
            print(GameSerializeClassesCollection.instance.addOn.errorStatus + "Low Chip Balance of Player");
        }
    }

    void OnTableListing(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__tournament_table_list  : " + data);

        GameSerializeClassesCollection.instance.mttTableListingData = JsonUtility.FromJson<GameSerializeClassesCollection.MttTableListingData>(data);

        SocialTournamentScript.instance.istableListReceived = true;
        SocialTournamentScript.instance.TableListing();
    }

    void OnGameBreak(SocketIOEvent socketIOEvent)
    {

        string data = socketIOEvent.data.ToString();
        print("__tournament_Game_Break  : " + data);

        GameSerializeClassesCollection.instance.mttBreak = JsonUtility.FromJson<GameSerializeClassesCollection.MttBreak>(data);
        UIManagerScript.instance.breakTimePanel.transform.GetChild(1).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("Game Break"); //"Game Break";
        UIManagerScript.instance.breakTimePanel.SetActive(true);
        Table.instance.breakCountDownCorotine = Table.instance.RemainingBreakTimerValue((GameSerializeClassesCollection.instance.mttBreak.break_time * 60), UIManagerScript.instance.breakTimeText);
        Table.instance.StartBreakCountDownCorotine();
    }

    void OnGameBreakTimer(SocketIOEvent socketIOEvent)
    {

        string data = socketIOEvent.data.ToString();
        print("__game_break_timer : " + data);

        GameSerializeClassesCollection.instance.timerCountDown = JsonUtility.FromJson<GameSerializeClassesCollection.TimerCountDown>(data);
        if (GameSerializeClassesCollection.instance.timerCountDown.tournament_id == SocialTournamentScript.instance.tournament_ID)
        {
            UIManagerScript.instance.breakTimeText.text = "00:" + (GameSerializeClassesCollection.instance.timerCountDown.minutes + ":" + GameSerializeClassesCollection.instance.timerCountDown.seconds).ToString();
            UIManagerScript.instance.breakTimePanel.transform.GetChild(1).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("Tournament Break");//" Tournament Break"; 
            UIManagerScript.instance.breakTimePanel.SetActive(true);
        }

        if (int.Parse(GameSerializeClassesCollection.instance.timerCountDown.minutes) == 0 && int.Parse(GameSerializeClassesCollection.instance.timerCountDown.seconds) <= 0)
        {
            UIManagerScript.instance.breakTimePanel.SetActive(false);
            UIManagerScript.instance.addOnRebuyButtonPanelTournment.gameObject.SetActive(false);
        }
    }

    void AddOnBreak(SocketIOEvent socketIOEvent)
    {

        string data = socketIOEvent.data.ToString();
        print("__add_on_break  : " + data);

        GameSerializeClassesCollection.instance.mttBreak = JsonUtility.FromJson<GameSerializeClassesCollection.MttBreak>(data);
        UIManagerScript.instance.breakTimePanel.transform.GetChild(1).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("Add-on Break"); //"Add-On Break";
        UIManagerScript.instance.breakTimePanel.SetActive(true);
        Table.instance.breakCountDownCorotine = Table.instance.RemainingBreakTimerValue((GameSerializeClassesCollection.instance.mttBreak.add_break_time * 60), UIManagerScript.instance.breakTimeText);
        Table.instance.StartBreakCountDownCorotine();
    }

    void AddOnBreakTimer(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__add_on_break_timer: " + data);

        GameSerializeClassesCollection.instance.timerCountDown = JsonUtility.FromJson<GameSerializeClassesCollection.TimerCountDown>(data);
        if (GameSerializeClassesCollection.instance.timerCountDown.tournament_id == SocialTournamentScript.instance.tournament_ID)
        {
            UIManagerScript.instance.breakTimeText.text = "00:" + (GameSerializeClassesCollection.instance.timerCountDown.minutes + ":" + GameSerializeClassesCollection.instance.timerCountDown.seconds).ToString();
            UIManagerScript.instance.breakTimePanel.transform.GetChild(1).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("Add-on Break"); //"Add-On Break";
            UIManagerScript.instance.breakTimePanel.SetActive(true);
        }

        if (int.Parse(GameSerializeClassesCollection.instance.timerCountDown.minutes) == 0 && int.Parse(GameSerializeClassesCollection.instance.timerCountDown.seconds) <= 0)
        {
            UIManagerScript.instance.breakTimePanel.SetActive(false);
            UIManagerScript.instance.addOnRebuyButtonPanelTournment.gameObject.SetActive(false);
        }
    }

    void OnExitTournament(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("On Exit Tournament  : " + data);
    }

    void OnRejoinTournament(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("On Rejoin Tournament  : " + data);

        GameSerializeClassesCollection.instance.onCurrentPlayerLeft = JsonUtility.FromJson<GameSerializeClassesCollection.OnPlayerLeft>(data);

        if (GameSerializeClassesCollection.instance.onCurrentPlayerLeft.tournament_id == SocialTournamentScript.instance.tournament_ID)
        {
            if (GameObject.Find(GameSerializeClassesCollection.instance.onCurrentPlayerLeft.playerName) != null)
            {
                GameObject currplayer = GameObject.Find(GameSerializeClassesCollection.instance.onCurrentPlayerLeft.playerName);
                if (!currplayer.GetComponent<PokerPlayerController>().isLocalPlayer /*== int.Parse(GameSerializeClassesCollection.instance.onCurrentPlayerLeft.clientId)*/)
                {
                    if (GameManagerScript.instance.isVideoTable)
                    {
                        PlayersGenerator.instance.AssignVideoPanelsToPlayers();
                    }
                }

            }
        }
    }

    void Throwable(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__st_throwables: " + data);
        GameSerializeClassesCollection.instance.throwablesListener = JsonUtility.FromJson<GameSerializeClassesCollection.ThrowablesListener>(data);

        if (GameSerializeClassesCollection.instance.throwablesListener.tournament_id == SocialTournamentScript.instance.tournament_ID)
        {
            SocialPokerGameManager.instance.PlayThrowableAnimation(GameSerializeClassesCollection.instance.throwablesListener.animation, GameSerializeClassesCollection.instance.throwablesListener.destinationPlayerName, GameSerializeClassesCollection.instance.throwablesListener.sourcePlayerName);
        }
    }
    void ChatListener(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__chat: " + data);
        GameSerializeClassesCollection.instance.tourneyChatListener = JsonUtility.FromJson<GameSerializeClassesCollection.TourneyChatListener>(data);
        Chat.instance.CreateChatForRemoteUser();
    }

    void ChatHistoryListener(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__chat_message: " + data);
        GameSerializeClassesCollection.instance.tourneyChatHistoryListener = JsonUtility.FromJson<GameSerializeClassesCollection.TourneyChatHistoryListener>(data);

        if (GameSerializeClassesCollection.instance.tourneyChatHistoryListener.tournament_id == SocialTournamentScript.instance.tournament_ID)
        {
            Chat.instance.CreateChatHistory();
        }
    }
    void TokenRevoked(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("TokenRevoked: " + data);
        UIManagerScript.instance.TableToPokerUI(6);
    }

    void RemoveMinimizePlayer(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__remove_minimise_player: " + data);
        GameSerializeClassesCollection.instance.onResumePlayerLeft = JsonUtility.FromJson<GameSerializeClassesCollection.OnResumePlayerLeft>(data);
        //UIManagerScript.instance.gameLeftPanelTournment.gameObject.SetActive(true);

        if (GameSerializeClassesCollection.instance.onResumePlayerLeft.tournament_status == 1)
        {
            isPlayerLeft = true;
            Text txt1 = Table.instance.RoundPot;
            //txt1.text = GameSerializeClassesCollection.instance.tournament.ticket;
            txt1.text = GameSerializeClassesCollection.instance.onResumePlayerLeft.tableNumber;
            ObserveTableFromGameDetail(txt1);
        }
        else
        {
            UIManagerScript.instance.TableToPokerUI(0);
        }


        ClubManagement.instance.loadingPanel.SetActive(true);
    }

    void BlindDetails(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__blind_details: " + data);
        GameSerializeClassesCollection.instance.blindDetailListener = JsonUtility.FromJson<GameSerializeClassesCollection.BlindDetailListener>(data);

        SocialTournamentScript.instance.BlindDetailsListing();
    }

    public int timesCalled;
    void TournamentOnwardsTimer(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        //print("__tournament_timer: " + data);
        GameSerializeClassesCollection.instance.tournamentOnwardsTimerListener = JsonUtility.FromJson<GameSerializeClassesCollection.TournamentOnwardsTimerListener>(data);
        SocialTournamentScript.instance.timerText.text = (GameSerializeClassesCollection.instance.tournamentOnwardsTimerListener.hours + ":" + GameSerializeClassesCollection.instance.tournamentOnwardsTimerListener.minutes + ":" + GameSerializeClassesCollection.instance.tournamentOnwardsTimerListener.seconds).ToString();

        isCountdown = true;

        if (int.Parse(GameSerializeClassesCollection.instance.tournamentOnwardsTimerListener.hours) >= 0 || int.Parse(GameSerializeClassesCollection.instance.tournamentOnwardsTimerListener.minutes) >= 0 || int.Parse(GameSerializeClassesCollection.instance.tournamentOnwardsTimerListener.seconds) >= 18)
        {
            if (timesCalled < 1)
            {
                SocialTournamentScript.instance.CheckRegisterStatusAfterStart();
                timesCalled++;
            }
        }
        if (UIManagerScript.instance.onGoing != null)
        {
            UIManagerScript.instance.onGoing.text = (GameSerializeClassesCollection.instance.tournamentOnwardsTimerListener.hours + ":" + GameSerializeClassesCollection.instance.tournamentOnwardsTimerListener.minutes + ":" + GameSerializeClassesCollection.instance.tournamentOnwardsTimerListener.seconds).ToString();
        }
    }
    void NextLevelTimer(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        //print("__mtt_next_level_timer : " + data);
        GameSerializeClassesCollection.instance.nextLevelTimerListener = JsonUtility.FromJson<GameSerializeClassesCollection.TournamentNextLevelTimerListener>(data);
        if (GameSerializeClassesCollection.instance.nextLevelTimerListener.tournament_id == SocialTournamentScript.instance.tournament_ID)
        {
            UIManagerScript.instance.nextLevelTimer.text = (GameSerializeClassesCollection.instance.nextLevelTimerListener.minutes + ":" + GameSerializeClassesCollection.instance.nextLevelTimerListener.seconds).ToString();
            if (GameSerializeClassesCollection.instance.nextLevelTimerListener.minutes == "00" && GameSerializeClassesCollection.instance.nextLevelTimerListener.seconds == "01")
            {
                //print("MTT1....");
                //MttEntyEmitter();
            }
        }
    }

    void ProfileImage(Sprite sprite)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (sprite != null)
        {
            print("ProfileImage texture");
            UIManagerScript.instance.gameLeftPanelTournment.GetChild(0).GetChild(1).GetChild(0).GetComponent<RawImage>().texture = sprite.texture;
        }
    }

    void AllClientObserver(SocketIOEvent socketIOEvent)
    {
        print("ALL CLIENTS");
        GameManagerScript.instance.InitlializeOnStart();
        SocialGame.instance.pokerUICanvas.SetActive(false);

        GameManagerScript.instance.chairAnimForLocalPlayer = false;

        if (SocialTournamentScript.instance.isTournamentVideo)
        {
            Screen.orientation = ScreenOrientation.Landscape;
        }
        else
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }

        for (int i = 0; i < UIManagerScript.instance.allOtherBottomPanelBtn.Count; i++)
        {
            UIManagerScript.instance.allOtherBottomPanelBtn[i].SetActive(false);
        }

        UIManagerScript.instance.observingUI.SetActive(true);
        UIManagerScript.instance.loadingPanel.SetActive(true);
        UIManagerScript.instance.loadingPanel.transform.GetChild(1).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("Setting Up");
        GameManagerScript.instance.isObserver = true;

        string data = socketIOEvent.data.ToString();
        print("__all_clients_observer: " + data);

        GameSerializeClassesCollection.AllPlayers allPlayers = JsonUtility.FromJson<GameSerializeClassesCollection.AllPlayers>(data);

        tableNoObserver = allPlayers.tableNumber.ToString();
        tournamentIdObserver = allPlayers.tournament_id;
        videoChannelObserver = tableNoObserver + tournamentIdObserver;

        if (allPlayers.tournament_id == SocialTournamentScript.instance.tournament_ID)
        {
            try
            {
                for (int i = 0; i < allPlayers.players.Length; i++)
                {
                    PlayersGenerator.instance.InstantiateAllOtherPlayer(allPlayers.players[i]);
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
                    Table.instance.UpdateTotalPot();
                    GameManagerScript.instance.UpdateTable(allPlayers.basicData.table);
                }
            }
            catch
            {
                CardShuffleAnimation.instance.isAnimationComplete = true;
                print("no basic data");
            }

            try
            {
                if (!allPlayers.break_time)
                {
                    if (allPlayers.checkActionPlayer)
                    {
                        GameManagerScript.instance.minBet = allPlayers.currentActionPlayerDetails.player.minBet;
                        Table.instance.roundNameFromGameClass = allPlayers.currentActionPlayerDetails.game.roundName;
                        PlayerActionManagement.instance.ShowPlayerCommands(allPlayers.currentActionPlayerDetails.player, 1.5f);
                    }
                    else
                    {
                        UIManagerScript.instance.loadingPanel.SetActive(true);
                        UIManagerScript.instance.loadingPanel.transform.GetChild(1).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("Setting Up");
                    }
                }
            }
            catch
            {
                print("Error in current Action PlayerDetails");
            }
            //UIManagerScript.instance.loadingPanel.SetActive(false);
        }
    }

    void MttObserverClient(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__mtt_observer_client: " + data);
        GameSerializeClassesCollection.instance.mttObserverClientId = JsonUtility.FromJson<GameSerializeClassesCollection.MttObserverClientId>(data);

        if (GameSerializeClassesCollection.instance.mttObserverClientId.entry)
        {
            if (!observerList.Contains(GameSerializeClassesCollection.instance.mttObserverClientId.clientId))
            {
                observerList.Add(GameSerializeClassesCollection.instance.mttObserverClientId.clientId);
            }
        }
        else
        {
            observerList.Remove(GameSerializeClassesCollection.instance.mttObserverClientId.clientId);
        }

        if (AccessGallery.instance.profileName[0].text == GameSerializeClassesCollection.instance.mttObserverClientId.player_name)
        {
            if (GameManagerScript.instance.isVideoTable)
            {
                if (GameManagerScript.instance.isTournament)
                {
                    //GameManagerScript.instance.OnVideoEngine(TournamentManagerScript.instance.tableNo + GameSerializeClassesCollection.instance.objMTTJoinClass.tournament_id, localPlayer.player.clientId);
                    GameManagerScript.instance.OnVideoEngine(videoChannelObserver, GameSerializeClassesCollection.instance.mttObserverClientId.clientId);
                }
            }
            UIManagerScript.instance.OnPlayerJoin();
            //GameManagerScript.instance.TableSetEmptytrue();
        }
    }
    void PlayerPosition(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("player_position: " + data);
        GameSerializeClassesCollection.instance.playerposition = JsonUtility.FromJson<GameSerializeClassesCollection.PlayerPosition>(data);
        //UIManagerScript.instance.position.text = GameSerializeClassesCollection.instance.playerposition.position.ToString();

    }

    void ObserverShuffle(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__mtt_observer_shuffle: " + data);
        GameSerializeClassesCollection.instance.switchTableListener = JsonUtility.FromJson<GameSerializeClassesCollection.SwitchTableListener>(data);

        UIManagerScript.instance.ReShuffleTable();
        StartCoroutine(ShuffleObserverCo(GameSerializeClassesCollection.instance.switchTableListener.ticket));
    }

    void UpdateTourStatus(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        print("__update_tournament_status : " + data);
        GameSerializeClassesCollection.instance.updateTourStatusData = JsonUtility.FromJson<GameSerializeClassesCollection.UpdateTourStatusData>(data);
        //if (GameSerializeClassesCollection.instance.updateTourStatusData.tournament_id == SocialTournamentScript.instance.tournament_ID)
        //{
        //    GameSerializeClassesCollection.instance.tournament.tournament_status = GameSerializeClassesCollection.instance.updateTourStatusData.tournament_status;
        //}

        //SocialTournamentScript.instance.UpdateGameDetailPage();
    }
    void FinalTableMessage(SocketIOEvent socketIOEvent)
    {
        print("__st_final_table : " + socketIOEvent.data.ToString());
        GameSerializeClassesCollection.instance.finalTableMessageData = JsonUtility.FromJson<GameSerializeClassesCollection.FinalTableMessageData>(socketIOEvent.data.ToString());
        UIManagerScript.instance.lowChipPanel.gameObject.SetActive(true);
        UIManagerScript.instance.lowChipPanelText.text = GameSerializeClassesCollection.instance.finalTableMessageData.message; 
    }
    void NotifyGameBreak(SocketIOEvent socketIOEvent)
    {
        print("__notify_game_break_timer : " + socketIOEvent.data.ToString());
        GameSerializeClassesCollection.instance.notifyGameBreak = JsonUtility.FromJson<GameSerializeClassesCollection.NotifyGameBreak>(socketIOEvent.data.ToString());

        if (GameSerializeClassesCollection.instance.notifyGameBreak.tournament_id == SocialTournamentScript.instance.tournament_ID)
        {
            print("EnterNotifyGameBreak");
            if (GameSerializeClassesCollection.instance.notifyGameBreak.break_time < 10)
            {
                UIManagerScript.instance.lowChipPanelText.text = "You are now on a " + "0" + GameSerializeClassesCollection.instance.notifyGameBreak.break_time + " minutes break. Break timer will start as soon as the last table finishes this hand.";
            }
            else
            {
                UIManagerScript.instance.lowChipPanelText.text = "You are now on a " + GameSerializeClassesCollection.instance.notifyGameBreak.break_time + " minutes break. Break timer will start as soon as the last table finishes this hand.";
            }
            UIManagerScript.instance.lowChipPanel.gameObject.SetActive(true);
            UIManagerScript.instance.breakTimeText.text = ("00" + ":" + GameSerializeClassesCollection.instance.notifyGameBreak.break_time + ":" + "00").ToString();
            UIManagerScript.instance.breakTimePanel.transform.GetChild(1).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("Tournament Break");
            UIManagerScript.instance.breakTimePanel.SetActive(true);
        }
    }

    void NotifyAddOnBreak(SocketIOEvent socketIOEvent)
    {
        print("__notify_addon_break_timer : " + socketIOEvent.data.ToString());
        GameSerializeClassesCollection.instance.notifyAddOnBreak = JsonUtility.FromJson<GameSerializeClassesCollection.NotifyAddOnBreak>(socketIOEvent.data.ToString());

        if (GameSerializeClassesCollection.instance.notifyAddOnBreak.tournament_id == SocialTournamentScript.instance.tournament_ID)
        {
            print("EnterNotifyGameBreak");
            if (GameSerializeClassesCollection.instance.notifyAddOnBreak.add_break_time < 10)
            {
                UIManagerScript.instance.lowChipPanelText.text = "You are now on a " + "0" + GameSerializeClassesCollection.instance.notifyAddOnBreak.add_break_time + " minutes break. Break timer will start as soon as the last table finishes this hand.";
            }
            else
            {
                UIManagerScript.instance.lowChipPanelText.text = "You are now on a " + GameSerializeClassesCollection.instance.notifyAddOnBreak.add_break_time + " minutes break. Break timer will start as soon as the last table finishes this hand.";
            }
            UIManagerScript.instance.lowChipPanel.gameObject.SetActive(true);
            UIManagerScript.instance.breakTimeText.text = ("00" + ":" + GameSerializeClassesCollection.instance.notifyAddOnBreak.add_break_time + ":" + "00").ToString();
            UIManagerScript.instance.breakTimePanel.transform.GetChild(1).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("Tournament Break");
            UIManagerScript.instance.breakTimePanel.SetActive(true);
        }
    }


    #endregion

    #region Emitter Methods 

    public void AddFriendOnTable(string recipientName, string id, string senderName, string senderId)
    {

        GameSerializeClassesCollection.instance.addfrindRequestTour.sender_name = senderName;
        GameSerializeClassesCollection.instance.addfrindRequestTour.sender_id = senderId;
        GameSerializeClassesCollection.instance.addfrindRequestTour.tournament_id = tournamentId;
        GameSerializeClassesCollection.instance.addfrindRequestTour.recipient = id;
        GameSerializeClassesCollection.instance.addfrindRequestTour.recipient_name = recipientName;
        GameSerializeClassesCollection.instance.addfrindRequestTour.token = Communication.instance.playerToken;
        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.addfrindRequestTour);
        socket.Emit("__send_friend_request", new JSONObject(data));

        Debug.Log(">>>> sendingfriend request" + "name " + recipientName + id + GameSerializeClassesCollection.instance.observeTable.token);

    }

    void friendResponse(string response)
    {
        Debug.Log(">>>>>friend response" + response);


    }

    public void MttEntyEmitter(bool inside_game_detail)
    {
        GameSerializeClassesCollection.instance.objMTTJoinClass.tournament_id = SocialTournamentScript.instance.tournament_ID;
        GameSerializeClassesCollection.instance.objMTTJoinClass.token = Communication.instance.playerToken;
        GameSerializeClassesCollection.instance.objMTTJoinClass.isRegistered = SocialTournamentScript.instance.isRegistered;
        GameSerializeClassesCollection.instance.objMTTJoinClass.user_image = ApiHitScript.instance.updatedUserImageUrl;
        GameSerializeClassesCollection.instance.objMTTJoinClass.inside_game_detail = inside_game_detail; 

        string body = JsonUtility.ToJson(GameSerializeClassesCollection.instance.objMTTJoinClass);
        print("MTT Entries  " + body);
        socket.Emit("__mtt_entries", new JSONObject(body));
    }

    public void TimerEmitter()
    {
        GameSerializeClassesCollection.instance.timer.tournament_id = SocialTournamentScript.instance.tournament_ID;
        string body = JsonUtility.ToJson(GameSerializeClassesCollection.instance.timer);
        print("__before_mtt_start Entries  " + body);
        socket.Emit("__before_mtt_start", new JSONObject(body));

        //GameManagerScript.instance.isObserver = false;
    }

    public void IsRegisteredEmitter()
    {
        GameSerializeClassesCollection.instance.userRegisterTournament.isRegistred = SocialTournamentScript.instance.isRegistered;
        GameSerializeClassesCollection.instance.userRegisterTournament.token = Communication.instance.playerToken;
        GameSerializeClassesCollection.instance.userRegisterTournament.tournament_id = SocialTournamentScript.instance.tournament_ID;
        string body = JsonUtility.ToJson(GameSerializeClassesCollection.instance.userRegisterTournament);
        print("__mtt_registered event" + body);
        socket.Emit("__mtt_registered", new JSONObject(body));
        //StartCoroutine(MttRejoin());
    }

    public void CountDownDetailPageEmitter()
    {
        GameSerializeClassesCollection.instance.countDown.tournament_id = SocialTournamentScript.instance.tournament_ID;
        string body = JsonUtility.ToJson(GameSerializeClassesCollection.instance.countDown);
        print("__st_count_down event" + body);
        socket.Emit("__st_count_down", new JSONObject(body));
    }

    public void AddOnEmitter1()
    {
        GameSerializeClassesCollection.instance.addOnEmitter.tournament_id = SocialTournamentScript.instance.tournament_ID;
        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.addOnEmitter);
        socket.Emit("__add_on", new JSONObject(data));

        UIManagerScript.instance.addOnPanel.SetActive(false);
    }

    public void RebuyChipsEmitter1()
    {
        GameSerializeClassesCollection.instance.rebuyChipsEmitter.tournament_id = SocialTournamentScript.instance.tournament_ID;
        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.rebuyChipsEmitter);
        socket.Emit("__rebuy_chips", new JSONObject(data));

        UIManagerScript.instance.rebuyPanel.SetActive(false);
    }

    public void TableListEmitter()
    {
        if (GameSerializeClassesCollection.instance.tournament.tournament_status == 1)
        {
            SocialTournamentScript.instance.tablesPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
            
            GameSerializeClassesCollection.instance.tableListing.token = Communication.instance.playerToken;
            GameSerializeClassesCollection.instance.tableListing.tournament_id = SocialTournamentScript.instance.tournament_ID;
            string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.tableListing);
            print("__tournament_table_list " + data);
            socket.Emit("__tournament_table_list", new JSONObject(data));
        }
        else
        {
            SocialTournamentScript.instance.tablesPage.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
            
        }
    }

    public void RankingListEmitter()
    {
        //MttEntyEmitter();
        //if (GameSerializeClassesCollection.instance.tournament.tournament_status != 0)
        //{
        SocialTournamentScript.instance.ResetRankList();
        GameSerializeClassesCollection.instance.rankingListing.token = Communication.instance.playerToken;
        GameSerializeClassesCollection.instance.rankingListing.tournament_id = SocialTournamentScript.instance.tournament_ID;
        GameSerializeClassesCollection.instance.rankingListing.user_image = ApiHitScript.instance.updatedUserImageUrl;

        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.rankingListing);
        print("__tournament_player_ranking " + data);
        socket.Emit("__tournament_player_ranking", new JSONObject(data));
        //}
    }

    public void RewardListEmitter()
    {
        //MttEntyEmitter();
        //if (GameSerializeClassesCollection.instance.tournament.tournament_status != 0)
        //{
        print(TournamentScript.instance.payoutStuctureVal);
        if (!string.IsNullOrEmpty(GameSerializeClassesCollection.instance.tournament.payout_structure))
        {
            GameSerializeClassesCollection.instance.rewardListing.payout_structure = GameSerializeClassesCollection.instance.tournament.payout_structure;
        }
        else
        {
            GameSerializeClassesCollection.instance.rewardListing.payout_structure = "heavy";
        }
        GameSerializeClassesCollection.instance.rewardListing.tournament_id = SocialTournamentScript.instance.tournament_ID;
        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.rewardListing);
        print("__tournament_rewards_ranking" + data);
        socket.Emit("__tournament_rewards_ranking", new JSONObject(data));
        //}
    }
    public void ObserveTournament()
    {
        GameSerializeClassesCollection.instance.tableID.table_id = tableNo;
        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.tableID);
        print("__watch_tournament " + data);
        UIManagerScript.instance.gameLeftPanelTournment.gameObject.SetActive(false);
        GameManagerScript.instance.isObserver = true;
        UIManagerScript.instance.observingPanel.gameObject.SetActive(true);
        socket.Emit("__watch_tournament", new JSONObject(data));
    }

    public void ObserveTournament2(string tableNo)
    {
        print(GameSerializeClassesCollection.instance.tournament.tournament_status);
        print(GameManagerScript.instance.isObserver);
        print("UserName" + ApiHitScript.instance.userName.text);

        if (GameSerializeClassesCollection.instance.tournament.tournament_status != 0 && GameManagerScript.instance.isObserver)
        {
            GameSerializeClassesCollection.instance.tableID.table_id = tableNo;
            GameSerializeClassesCollection.instance.tableID.playerName = ApiHitScript.instance.userName.text;
            string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.tableID);
            print("__watch_tournament " + data);
            UIManagerScript.instance.gameLeftPanelTournment.gameObject.SetActive(false);
            GameManagerScript.instance.isObserver = true;
            UIManagerScript.instance.observingPanel.gameObject.SetActive(true);
            GameManagerScript.instance.TableSetEmptytrue();
            socket.Emit("__watch_tournament", new JSONObject(data));
        }
    }

    public void PlayerExistEmitter()
    {
        print("tournament_status -" + GameSerializeClassesCollection.instance.tournament.tournament_status);
        print("isRegistered- " + SocialTournamentScript.instance.isRegistered);
        if (GameSerializeClassesCollection.instance.tournament.tournament_status == 1 /*&& SocialTournamentScript.instance.isRegistered*/)
        {
            GameSerializeClassesCollection.instance.playerExist.tournament_status = GameSerializeClassesCollection.instance.tournament.tournament_status;
            GameSerializeClassesCollection.instance.playerExist.isRegistered = SocialTournamentScript.instance.isRegistered;
            GameSerializeClassesCollection.instance.playerExist.playerName = ApiHitScript.instance.userName.text;
            GameSerializeClassesCollection.instance.playerExist.tournament_id = SocialTournamentScript.instance.tournament_ID;
            string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.playerExist);
            print("__st_player_existence" + data);
            socket.Emit("__st_player_existence", new JSONObject(data));
        }
    }

    public IEnumerator MttRejoin()
    {
        yield return new WaitForSeconds(1f);

        print("Rejoin");
        print("STATUS" + GameSerializeClassesCollection.instance.tournament.tournament_status);
        print("BOOL" + SocialTournamentScript.instance.isRegistered);
        print("BOOL" + SocialTournamentScript.instance.tournament_ID);
        if (GameSerializeClassesCollection.instance.tournament.tournament_status == 1 && SocialTournamentScript.instance.isRegistered)
        {
            GameManagerScript.instance.isObserver = false;

            GameSerializeClassesCollection.instance.rejoin.tournament_status = GameSerializeClassesCollection.instance.tournament.tournament_status;
            GameSerializeClassesCollection.instance.rejoin.isRegistered = SocialTournamentScript.instance.isRegistered;
            GameSerializeClassesCollection.instance.rejoin.user_image = ApiHitScript.instance.updatedUserImageUrl;
            GameSerializeClassesCollection.instance.rejoin.tournament_id = SocialTournamentScript.instance.tournament_ID;
            string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.rejoin);
            print("__mtt_re_join" + data);
            socket.Emit("__mtt_re_join", new JSONObject(data));
        }
    }

    public void ThrowableEmitter()
    {
        GameSerializeClassesCollection.instance.throwables.source = SocialTournamentScript.instance.throwableSource;
        GameSerializeClassesCollection.instance.throwables.destination = SocialTournamentScript.instance.throwableDestination;
        GameSerializeClassesCollection.instance.throwables.amount = SocialTournamentScript.instance.throwableCharge;
        GameSerializeClassesCollection.instance.throwables.tournament_id = SocialTournamentScript.instance.tournament_ID;
        GameSerializeClassesCollection.instance.throwables.animation = SocialTournamentScript.instance.animationName;
        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.throwables);
        print("__st_throwables" + data);
        socket.Emit("__st_throwables", new JSONObject(data));
    }
    public void ChatEmitter(string message)
    {
        GameSerializeClassesCollection.instance.tourneyChat.token = Communication.instance.playerToken;//GameSerializeClassesCollection.instance.observeTable.token;
        GameSerializeClassesCollection.instance.tourneyChat.tournament_id = SocialTournamentScript.instance.tournament_ID;
        GameSerializeClassesCollection.instance.tourneyChat.message = message;

        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.tourneyChat);
        print("__chat" + data);
        socket.Emit("__chat", new JSONObject(data));
    }

    public void ChatHistoryEmitter()
    {
        GameSerializeClassesCollection.instance.tourneyChatHistory.tournament_id = SocialTournamentScript.instance.tournament_ID;

        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.tourneyChatHistory);
        print("__chat_message" + data);
        socket.Emit("__chat_message", new JSONObject(data));
    }

    public void LateRegistrationEmitter()
    {
        GameSerializeClassesCollection.instance.lateRegistrationEmitter.token = Communication.instance.playerToken;
        GameSerializeClassesCollection.instance.lateRegistrationEmitter.tournament_id = SocialTournamentScript.instance.tournament_ID;

        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.lateRegistrationEmitter);
        print("__late_registration" + data);
        socket.Emit("__late_registration", new JSONObject(data));
    }

    public void LateRegistrationEnterEmitter()
    {
        isLateRegistration = false;
        GameSerializeClassesCollection.instance.lateRegistrationEntryEmitter.token = Communication.instance.playerToken;
        GameSerializeClassesCollection.instance.lateRegistrationEntryEmitter.tournament_id = SocialTournamentScript.instance.tournament_ID;
        GameSerializeClassesCollection.instance.lateRegistrationEntryEmitter.ticket = GameSerializeClassesCollection.instance.lateRegistrationListener.ticket;
        GameSerializeClassesCollection.instance.lateRegistrationEntryEmitter.seat_id = GameSerializeClassesCollection.instance.lateRegistrationListener.seat_id;

        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.lateRegistrationEntryEmitter);
        print("__late_registration_entry" + data);
        socket.Emit("__late_registration_entry", new JSONObject(data));
    }
    public void BlindDetailEmitter()
    {
        GameSerializeClassesCollection.instance.blindDetailEmitter.blind_structure = GameSerializeClassesCollection.instance.tournament.blind_structure;
        GameSerializeClassesCollection.instance.blindDetailEmitter.tournament_id = SocialTournamentScript.instance.tournament_ID;
        GameSerializeClassesCollection.instance.blindDetailEmitter.token = Communication.instance.playerToken;

        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.blindDetailEmitter);
        print("__blind_details" + data);
        socket.Emit("__blind_details", new JSONObject(data));
    }

    public void TournamentOnwardsTimerEmitter()
    {
        GameSerializeClassesCollection.instance.tournamentOnwardsTimerEmitter.tournament_id = SocialTournamentScript.instance.tournament_ID;

        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.tournamentOnwardsTimerEmitter);
        print("__tournament_timer" + data);
        socket.Emit("__tournament_timer", new JSONObject(data));
    }


    Text observerTicket;
    IEnumerator ShuffleObserverCo(string ticket)
    {
        observerTicket = Table.instance.RoundPot;
        observerTicket.text = ticket;

        yield return new WaitForSeconds(1);
        MttObserver(observerTicket);
    }


    public void MttObserver(Text tableNumber)
    {
        print("exist:..... " + tableNumber);
        if (GameManagerScript.instance.isObserver)
        {
            print("exist:222..... " + tableNumber);
            if (tableNoObserver == tableNumber.text)
            {
                print("exist:.333.... " + tableNumber);
                if (UIManagerScript.instance.lowChipPanel != null)
                {
                    print("exist:.444.... " + tableNumber);
                    UIManagerScript.instance.lowChipPanel.gameObject.SetActive(true);
                    UIManagerScript.instance.lowChipPanelText.text = "You are viewing same table!!";
                }
                return;
            }
            print("exist:55555..... " + tableNumber);
            SocialTournamentScript.instance.tableCount = 1;
            print("exist: value " + GameSerializeClassesCollection.instance.playerExistListener.exist);
            SocialTournamentScript.instance.observingTableNumber = tableNumber.text;
            if (UIManagerScript.instance.mttSideInfoPanel != null && UIManagerScript.instance.mttSideInfoPanel.activeInHierarchy)
            {
                if (/*tableNoObserver != tableNumber.text &&*/ !GameSerializeClassesCollection.instance.playerExistListener.exist)
                {
                    UIManagerScript.instance.mttSideInfoPanel.SetActive(false);
                    UIManagerScript.instance.ReShuffleTable();
                }
            }
            print("exist:6666..... " + tableNumber);
            GameSerializeClassesCollection.instance.mttObserverEmitter.tournament_id = SocialTournamentScript.instance.tournament_ID;
            GameSerializeClassesCollection.instance.mttObserverEmitter.token = Communication.instance.playerToken;
            GameSerializeClassesCollection.instance.mttObserverEmitter.tableNumber = tableNumber.text;

            string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.mttObserverEmitter);
            if (!GameSerializeClassesCollection.instance.playerExistListener.exist || isPlayerLeft)
            {
                print("exist:.7777.... " + tableNumber);
                isPlayerLeft = false;
                print("__mtt_observer" + data);
                socket.Emit("__mtt_observer", new JSONObject(data));

                GameManagerScript.instance.isObserver = true;
            }
        }
        else
        {
            print("exist:8888..... " + tableNumber);
            if (UIManagerScript.instance.lowChipPanel != null)
            {
                print("exist:..9999... " + tableNumber);
                UIManagerScript.instance.lowChipPanel.gameObject.SetActive(true);
                UIManagerScript.instance.lowChipPanelText.text = "You are already in a game, so you can not observe other table!!";
                GameManagerScript.instance.isObserver = false;
            }
        }
    }

    public void ObserveTableFromGameDetail(Text tableNumber)
    {
        SocialTournamentScript.instance.tableCount = 1;
        print("exist: " + GameSerializeClassesCollection.instance.playerExistListener.exist);
        SocialTournamentScript.instance.observingTableNumber = tableNumber.text;
     
        GameSerializeClassesCollection.instance.mttObserverEmitter.tournament_id = SocialTournamentScript.instance.tournament_ID;
        GameSerializeClassesCollection.instance.mttObserverEmitter.token = Communication.instance.playerToken;
        GameSerializeClassesCollection.instance.mttObserverEmitter.tableNumber = tableNumber.text;

        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.mttObserverEmitter);
        if (!GameSerializeClassesCollection.instance.playerExistListener.exist || isPlayerLeft)
        {
            isPlayerLeft = false;
            print("__mtt_observer" + data);
            socket.Emit("__mtt_observer", new JSONObject(data));

            GameManagerScript.instance.isObserver = true;
        }
    }
    
    #endregion

    public void BlindsUp()
    {
        GameSerializeClassesCollection.instance.blindUpData.table_id = int.Parse(tableNo);
        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.blindUpData);
        print("__blinds_up" + data);
        socket.Emit("__blinds_up", new JSONObject(data));
    }

    void OnResumeAllClients(SocketIOEvent socketIOEvent)
    {
        //UIManagerScript.instance.loadingPanel.transform.GetChild(1).GetComponent<Text>().text = "Setting Up...";
        GameManagerScript.instance.isGameResumed = true;
        string data = socketIOEvent.data.ToString();
        if (GameManagerScript.instance.isObserver)
        {
            UIManagerScript.instance.OnPlayerJoin();
        }
        print("On Resume All Clients : " + data);

        GameSerializeClassesCollection.AllPlayers allPlayers = JsonUtility.FromJson<GameSerializeClassesCollection.AllPlayers>(data);

        string newVideoChannel = allPlayers.tableNumber + allPlayers.tournament_id;
        bool isResume = false;

        if (newVideoChannel != videoChannel)
        {
            UIManagerScript.instance.ReShuffleTable();
        }

        if (allPlayers.tournament_id == SocialTournamentScript.instance.tournament_ID)
        {
            breaktimer = allPlayers.break_time;

            if (!allPlayers.break_time)
            {
                UIManagerScript.instance.breakTimePanel.SetActive(false);
            }
            else
            {
                if (GameManagerScript.instance.HandRankPanel != null)
                {
                    GameManagerScript.instance.HandRankPanel.SetActive(false);
                }
                UIManagerScript.instance.breakTimeText.text = "00:00:00";
                UIManagerScript.instance.breakTimePanel.transform.GetChild(1).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("Tournament Break");
                UIManagerScript.instance.breakTimePanel.SetActive(true);
            }

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

                        if (GameManagerScript.instance.isVideoTable)
                        {
                            if (newVideoChannel == videoChannel)
                            {
                                isResume = true;
                                PlayersGenerator.instance.InstantiateLocalPlayer(GameSerializeClassesCollection.instance.newLocalPlayer, true);
                            }
                            else
                            {
                                isResume = false;
                                //UIManagerScript.instance.ReShuffleTable();
                                videoChannel = newVideoChannel;
                                PlayersGenerator.instance.InstantiateLocalPlayer(GameSerializeClassesCollection.instance.newLocalPlayer, false);
                            }
                        }
                        else
                        {
                            PlayersGenerator.instance.InstantiateLocalPlayer(GameSerializeClassesCollection.instance.newLocalPlayer, true);
                        }
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
                if (/*allPlayers.basicData != null*/ allPlayers.basicData.players.Length != 0)
                {
                    CardShuffleAnimation.instance.isAnimationComplete = true;
                    GameManagerScript.instance.AllBasicDataUpdate(allPlayers.basicData);

                    GameManagerScript.instance.UpdateTable(allPlayers.basicData.table);
                    Table.instance.UpdateTotalPot();
                    StartCoroutine(GameManagerScript.instance.FindLocalPlayerAndUpdateHand(allPlayers.basicData.players));

                }
            }
            catch
            {
                CardShuffleAnimation.instance.isAnimationComplete = true;
                print("no basic data");
            }

            try
            {
                if (!allPlayers.break_time)
                {
                    if (allPlayers.checkActionPlayer)
                    {
                        GameManagerScript.instance.minBet = allPlayers.currentActionPlayerDetails.player.minBet;
                        Table.instance.roundNameFromGameClass = allPlayers.currentActionPlayerDetails.game.roundName;
                        PlayerActionManagement.instance.ShowPlayerCommands(allPlayers.currentActionPlayerDetails.player, 1.5f);
                    }
                }
            }
            catch
            {
                print("Error in current Action PlayerDetails");
            }
            UIManagerScript.instance.loadingPanel.SetActive(false);
        }

        if (isResume)
        {
            StartCoroutine(GameManagerScript.instance.GenerateVideoOnResume());
        }
        else if (GameManagerScript.instance.isObserver)
        {
            StartCoroutine(GameManagerScript.instance.GenerateVideoOnResume());
        }

    }

    public void RevokeCurrPlayer()
    {
        GameSerializeClassesCollection.instance.mttRevokeEmitter.tournament_id = SocialTournamentScript.instance.tournament_ID;
        GameSerializeClassesCollection.instance.mttRevokeEmitter.token = Communication.instance.playerToken;
        GameSerializeClassesCollection.instance.mttRevokeEmitter.tableNumber = Table.instance.table.tableNumber.ToString();
        GameSerializeClassesCollection.instance.mttRevokeEmitter.playerName = ApiHitScript.instance.userName.text;
        GameSerializeClassesCollection.instance.mttRevokeEmitter.seat_id = localPlayer.transform.GetComponent<PokerPlayerController>().player.seatId;

        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.mttRevokeEmitter);

        print("__revoke_current_player" + data);
        socket.Emit("__revoke_current_player", new JSONObject(data));
    }

    //.............................Agora Video Intialize for Tournament...................//
    //IEnumerator InitialzeAgora(float delay, int clientId, string ticket)
    //{
    //    print("AGORA Engine"+ticket);
    //    print("AGORA Engine" + clientId);
    //    yield return new WaitForSeconds(delay);
    //    GameManagerScript.instance.OnVideoEngine(ticket, clientId);
    //}    


}