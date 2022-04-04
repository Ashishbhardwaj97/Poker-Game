using SmartLocalization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionManagement : MonoBehaviour
{
    public static PlayerActionManagement instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

    }

    public void ShowPlayerCommands(GameSerializeClassesCollection.PlayerOnRoundStart playerInfo, float waitToShowCommand)
    {
        StartCoroutine(ShowPlayerCommandCoroutine(playerInfo, waitToShowCommand));
    }

    IEnumerator ShowPlayerCommandCoroutine(GameSerializeClassesCollection.PlayerOnRoundStart playerInfo, float waitToShowCommand)
    {

        ComandExecuted();
        while (true)
        {
            if (CardShuffleAnimation.instance.isAnimationComplete)
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(waitToShowCommand);
        try
        {
            if (GameObject.Find(playerInfo.playerName) != null)
            {
                GameManagerScript.instance.CheckActiveCommands(playerInfo);
                int actuallId = GameManagerScript.instance.GetActualSeatID(playerInfo.seatId);

                GameManagerScript.instance.playersParent.transform.GetChild(actuallId).GetChild(0).transform.GetComponent<PokerPlayerController>().ActiveCommands();
                GameManagerScript.instance.playersParent.transform.GetChild(actuallId).GetChild(0).transform.GetComponent<PokerPlayerController>().Timer(GameManagerScript.instance.commandTimer2);
            }
        }
        catch
        {
            print("No Player found to activate commands");
        }
    }

    public void OnNewRound2(GameSerializeClassesCollection.RoundStartInfo roundStartInfoObj)
    {
        StartCoroutine(NewRoundCoroutine2(roundStartInfoObj));
    }


    IEnumerator NewRoundCoroutine2(GameSerializeClassesCollection.RoundStartInfo roundStartInfoObj)
    {


        GameManagerScript.instance.commandTimer2 = roundStartInfoObj.table.commandTimeout;
        yield return new WaitForSeconds(0.1f);

        GameManagerScript.instance.UpdateTable(roundStartInfoObj.table);
        string value = GameManagerScript.instance.KiloFormat(roundStartInfoObj.table.totalBet);
        Table.instance.TotalPot.text = value;


        if (!GameManagerScript.instance.isVideoTable)
        {
            Table.instance.TotalPot.text = Table.instance.TotalPot.text.Replace("Pot: ", string.Empty);
        }

        yield return new WaitForSeconds(0.1f);
        GameManagerScript.instance.NewRoundStart(roundStartInfoObj);
    }

    public void ActionPerformedResult(GameSerializeClassesCollection.ActionPerformed actionPerformed)
    {
        print("Action Performed Result Start");
        GameManagerScript.instance.alarmsound = false;
        if (UIManagerScript.instance.tipPanel.activeInHierarchy)
        {
            UIManagerScript.instance.tipPanel.SetActive(false);
        }

        GameManagerScript.instance.UpdateTable(actionPerformed.table);
        ComandExecuted();
        Table.instance.UpdateRoundPot(actionPerformed.action.amount);
        int actuallId = GameManagerScript.instance.GetActualSeatID(actionPerformed.action.seatId);

        try
        {
            GameManagerScript.instance.playersParent.transform.GetChild(actuallId).GetChild(0).transform.GetComponent<PokerPlayerController>().player.chips = actionPerformed.action.chips;

            if (GameManagerScript.instance.isVideoTable)
            {
                GameManagerScript.instance.playersParent.transform.GetChild(actuallId).GetChild(0).transform.GetComponent<PokerPlayerController>().betChipValueForVideoTable = GameManagerScript.instance.playersParent.transform.GetChild(actuallId).GetChild(0).transform.GetComponent<PokerPlayerController>().player.bet;
                int value = GameManagerScript.instance.playersParent.transform.GetChild(actuallId).GetChild(0).transform.GetComponent<PokerPlayerController>().player.bet;
                int finalvalue = value + actionPerformed.action.amount;
                GameManagerScript.instance.playersParent.transform.GetChild(actuallId).GetChild(0).transform.GetComponent<PokerPlayerController>().player.bet = finalvalue;
            }
            else
            {

                int value = GameManagerScript.instance.playersParent.transform.GetChild(actuallId).GetChild(0).transform.GetComponent<PokerPlayerController>().player.bet;
                int finalvalue = value + actionPerformed.action.amount;
                GameManagerScript.instance.playersParent.transform.GetChild(actuallId).GetChild(0).transform.GetComponent<PokerPlayerController>().player.bet = finalvalue;
                GameSerializeClassesCollection.instance.actionClass2.amount = actionPerformed.action.amount;
            }

            GameManagerScript.instance.playersParent.transform.GetChild(actuallId).GetChild(0).transform.GetComponent<PokerPlayerController>().UpdateValues();
            GameManagerScript.instance.playersParent.transform.GetChild(actuallId).GetChild(0).transform.GetComponent<PokerPlayerController>().ShowPlayerAction(actionPerformed.action.action);
        }
        catch
        {
            print("Some Error in ActionPerformedResult PlayerActionManagement.cs");
        }
        print("Action Performed Result End");

    }

    public void ComandExecuted()
    {
        print("Hide in ComandExecuted");
        UIManagerScript.instance.HideCommands();
        GameManagerScript.instance.StopCoroutineTimer();

        for (int i = 0; i < GameManagerScript.instance.playersParent.transform.childCount; i++)
        {
            if (GameManagerScript.instance.playersParent.transform.GetChild(i).childCount > 1)
            {
                try
                {
                    GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).transform.GetComponent<PokerPlayerController>().TimerOFF();
                }
                catch
                {
                    print("Some Error in ComandExecuted PlayerActionManagement.cs");
                }
            }
        }

        if (UIManagerScript.instance.alarmClock != null)
        {
            UIManagerScript.instance.alarmClock.SetActive(false);
            SoundManager.instance.alarmClocksound.volume = 0;
            SoundManager.instance.alarmClocksound.Stop();
        }

        print("Command Executed");
    }

    public void NewRound(GameSerializeClassesCollection.OnNewRound newRound)
    {
        StartCoroutine(NewRoundCoroutine(newRound));
        GameManagerScript.instance.totalPlayersOnNewRound = 0;
    }

    IEnumerator NewRoundCoroutine(GameSerializeClassesCollection.OnNewRound newRound)
    {
        Table.instance.Id.text = "ID : " + newRound.table.tableNumber.ToString() + "-" + newRound.table.roundCount.ToString();
        GameManagerScript.instance.bigBlindAmount = newRound.table.bigBlind.amount;
        CardShuffleAnimation.instance.totalPlayersCommonUI.Clear();
        CardShuffleAnimation.instance.StartCardShuffle();

        if(UIManagerScript.instance.winPanel.activeInHierarchy)
        {
            StartCoroutine(WinningLogic.instance.GameReset());
        }

        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < newRound.players.Length; i++)
        {
            try
            {
                GameObject player = GameObject.Find(newRound.players[i].playerName);
                player.GetComponent<PokerPlayerController>().TurnOffActionUI(false);
                player.GetComponent<PokerPlayerController>().foldImage.gameObject.SetActive(false);
                player.GetComponent<PokerPlayerController>().allInImage.gameObject.SetActive(false);
                player.GetComponent<PokerPlayerController>().player.chips = newRound.players[i].chips;
                player.GetComponent<PokerPlayerController>().chipsText.transform.GetComponent<Text>().text = string.Format("{0:n0}", newRound.players[i].chips); // "" + newRound.players[i].chips;
                player.GetComponent<PokerPlayerController>().player.isSurvive = newRound.players[i].isSurvive;
                player.GetComponent<PokerPlayerController>().player.folded = newRound.players[i].folded;
                player.GetComponent<PokerPlayerController>().player.bet = newRound.players[i].bet;
                player.GetComponent<PokerPlayerController>().player.allIn = newRound.players[i].allIn;
                if (newRound.players[i].isSurvive)
                {
                    CardShuffleAnimation.instance.totalPlayersCommonUI.Add(player.transform.parent.transform.GetChild(1).gameObject);
                    player.GetComponent<PokerPlayerController>().playerExclude.gameObject.SetActive(false);

                    if (player.GetComponent<PokerPlayerController>().isLocalPlayer)
                    {
                        GameManagerScript.instance.isPlayerExcluded = false;
                        FriendandSocialScript.instance.isSeatReserved = false;
                    }

                }
                else
                {
                    player.GetComponent<PokerPlayerController>().playerExclude.gameObject.SetActive(true);
                    if (player.GetComponent<PokerPlayerController>().isLocalPlayer)
                    {
                        GameManagerScript.instance.isPlayerExcluded = true;
                    }
                }

                GameManagerScript.instance.totalPlayersOnNewRound++;
            }
            catch
            {
                print("Player Not Found");
            }
        }

        CardShuffleAnimation.instance.shuffleCardCount++;
        if (newRound.players.Length == 0)
        {
            CardShuffleAnimation.instance.shuffleCardCount = 0;
        }

        try
        {
            int dealerseatid = GameManagerScript.instance.GetActualSeatID(newRound.table.dealer.seatId);
            GameManagerScript.instance.dealerSeatid = dealerseatid + 1;
            GameManagerScript.instance.playersParent.transform.GetChild(dealerseatid).GetChild(0).transform.GetComponent<PokerPlayerController>().TurnOnDealer(1);
            if (GameManagerScript.instance.isVideoTable)
            {
                if (newRound.table.dealer.isStraddle)
                {
                    GameManagerScript.instance.playersParent.transform.GetChild(dealerseatid).GetChild(0).transform.GetComponent<PokerPlayerController>().dealerButtonFirstChild.GetChild(0).gameObject.SetActive(true);
                    GameManagerScript.instance.playersParent.transform.GetChild(dealerseatid).GetChild(0).transform.GetComponent<PokerPlayerController>().dealerButtonFirstChild.GetChild(0).GetChild(0).GetComponent<Text>().text = "Straddle " + newRound.table.dealer.amount.ToString();
                }
                else
                {
                    GameManagerScript.instance.playersParent.transform.GetChild(dealerseatid).GetChild(0).transform.GetComponent<PokerPlayerController>().dealerButtonFirstChild.GetChild(0).gameObject.SetActive(false);
                }
            }
            else
            {
                if (newRound.table.dealer.isStraddle)
                {
                    GameManagerScript.instance.playersParent.transform.GetChild(dealerseatid).GetChild(0).transform.GetComponent<PokerPlayerController>().dealerButton.GetChild(0).gameObject.SetActive(true);
                    GameManagerScript.instance.playersParent.transform.GetChild(dealerseatid).GetChild(0).transform.GetComponent<PokerPlayerController>().dealerButton.GetChild(0).GetChild(0).GetComponent<Text>().text = "Straddle " + newRound.table.dealer.amount.ToString();
                }
                else
                {

                    GameManagerScript.instance.playersParent.transform.GetChild(dealerseatid).GetChild(0).transform.GetComponent<PokerPlayerController>().dealerButton.GetChild(0).gameObject.SetActive(false);
                }

            }

        }
        catch
        {
            print("No Dealer");
        }
        yield return new WaitForSeconds(0.5f);
        try
        {
            int SBseatid = GameManagerScript.instance.GetActualSeatID(newRound.table.smallBlind.seatId);
            GameManagerScript.instance.playersParent.transform.GetChild(SBseatid).GetChild(0).transform.GetComponent<PokerPlayerController>().TurnOnDealer(2);
        }
        catch
        {
            print("No SB");
        }
        yield return new WaitForSeconds(0.2f);
        try
        {
            int BBseatid = GameManagerScript.instance.GetActualSeatID(newRound.table.bigBlind.seatId);
            GameManagerScript.instance.playersParent.transform.GetChild(BBseatid).GetChild(0).transform.GetComponent<PokerPlayerController>().TurnOnDealer(3);
        }
        catch
        {
            GameManagerScript.instance.bigBlindAmount = 10;
            print("No BB");
        }

        try
        {
            for (int i = 0; i < newRound.players.Length; i++)
            {
                try
                {

                    int seatid = GameManagerScript.instance.GetActualSeatID(newRound.players[i].seatId);
                    GameManagerScript.instance.playersParent.transform.GetChild(seatid).GetChild(0).transform.GetComponent<PokerPlayerController>().TurnOnDealer(4);

                }
                catch
                {
                    print("Player Not Found");
                }
            }

            GameManagerScript.instance.UpdateTable(newRound.table);
            string value = GameManagerScript.instance.KiloFormat(newRound.table.totalBet);
            Table.instance.TotalPot.text = value;
            Table.instance.StackCal(newRound.table.totalBet);

            if(GameManagerScript.instance.totalPlayersOnNewRound != GameManagerScript.instance.totalPlayersSitting)
            {
                UIManagerScript.instance.loadingPanel.SetActive(true);
                //UIManagerScript.instance.loadingPanel.transform.GetChild(1).GetComponent<Text>().text = "Fixing an issue..";//"Resuming Game...";

                ResetGameOnNewRound(true);
            }

        }
        catch
        {

        }
    }

    public void ResetGameOnNewRound(bool isResubscribeNeeded)
    {
        print("ResetGameOnNewRound Status 1 isResubscribeNeeded" + isResubscribeNeeded);

        Table.instance.DeactiveStacks();
        GameManagerScript.instance.isGameResumed = false;
        if (GameManagerScript.instance.isTournament)
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
            // GameSerializeClassesCollection.instance.switchTableEmitter.tableNumber = GameSerializeClassesCollection.instance.observeTable.ticket;
            string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.switchTableEmitter);
            print("__minimise emmiter..." + data);
            //PokerNetworkManager.instance.socket.Emit("__resume_minimise", new JSONObject(data));
            PokerNetworkManager.instance.socket.Emit("__minimise", new JSONObject(data));
            //PokerNetworkManager.instance.socket.Emit("__minimise");
            PokerNetworkManager.instance.UnSubscribeToServerEvents();
        }
        if (GameManagerScript.instance.isVideoTable)
        {
            AgoraInit.instance.MuteUnmuteLocalPlayer(true);
            AgoraInit.instance.MuteUnmuteAllRemotePlayer(true);
        }
        try
        {
            try
            {
                Table.instance.StopCardAnim();
            }
            catch (Exception e)
            {
                print("problem in Card Anim CO.." + e);
            }
            WinningLogic.instance.StopWinCo();
        }
        catch (Exception e)
        {
            print("problem in StopWinCo.." + e);
        }
        StopCoroutine(Table.instance.OpenCardStarts);

        if (isResubscribeNeeded)
        {
            StartCoroutine(ReSubscribeCalled());
        }

    }

    IEnumerator ReSubscribeCalled()
    {
        yield return new WaitForSeconds(0.8f);
        //UIManagerScript.instance.loadingPanel.transform.GetChild(1).GetComponent<Text>().text = "Issue fixed..";
        //yield return new WaitForSeconds(0.8f);
        UIManagerScript.instance.loadingPanel.transform.GetChild(1).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("Resuming Game");//"Resuming Game...";
        StartCoroutine(GameManagerScript.instance.ReSubscribe());
    }

    //...................................................Emitters..........................................................//
    //Assign This to Call, Raise, Bet Buttons
    public void PlayerActionEmitter(string actionName)
    {

        if (actionName == "fold" && GameManagerScript.instance.isCheckActive)
        {
            UIManagerScript.instance.tipPanel.SetActive(true);
            GameManagerScript.instance.isCheckActive = false;
            return;
        }

        if (actionName != "raise" && actionName != "bet")
        {
            GameSerializeClassesCollection.ActionClass actionNameObj = new GameSerializeClassesCollection.ActionClass();
            actionNameObj.action = actionName;

            if (GameManagerScript.instance.isTournament)
            {
                actionNameObj.tournament_id = SocialTournamentScript.instance.tournament_ID;
                actionNameObj.tableNumber = Table.instance.table.tableNumber;
                actionNameObj.token = Communication.instance.playerToken; 
            }
            else
            {
                actionNameObj.tableNumber = PokerNetworkManager.instance.tableID;
                actionNameObj.token = Communication.instance.playerToken; 
            }

            string data = JsonUtility.ToJson(actionNameObj);
            //print("Action Data " + data);

            ComandExecuted();
            if (GameManagerScript.instance.isTournament)
            {
                print("Action Data " + data);
                TournamentManagerScript.instance.socket.Emit("__action", new JSONObject(data));
            }
            else
            {
                print("Action Data " + data);
                PokerNetworkManager.instance.socket.Emit("__action", new JSONObject(data));
            }
            //SoundManager.instance.alarmsound.volume = 0;
            //StartCoroutine(AutoStopAlarmClockAnim());
            UIManagerScript.instance.alarmClock.SetActive(false);
            SoundManager.instance.alarmClocksound.volume = 0;
            SoundManager.instance.alarmClocksound.Stop();
        }

        if (actionName == "bet")
        {
            print("actionName send : " + actionName);
            UIManagerScript.instance.RaisePanelActive(true, 2);
        }

        if (actionName == "raise")
        {
            print("actionName send : " + actionName);
            UIManagerScript.instance.RaisePanelActive(true, 1);
        }

    }

    // it emits raise action
    public string actionName;
    float value;
    public void RaiseAndBetConfirmEmitter(int timesBet)
    {

        if (UIManagerScript.instance.raisePanel.gameObject.activeInHierarchy)
        {
            UIManagerScript.instance.RaisePanelActive(false, 1);
            value = UIManagerScript.instance.raiseSlider.GetComponent<Slider>().value;
        }

        if (UIManagerScript.instance.betPanel.gameObject.activeInHierarchy)
        {
            UIManagerScript.instance.RaisePanelActive(false, 2);
            value = UIManagerScript.instance.betSlider.GetComponent<Slider>().value;
        }

        if (GameManagerScript.instance.isTournament)
        {
            GameSerializeClassesCollection.instance.actionClass2.tournament_id = SocialTournamentScript.instance.tournament_ID;

            GameSerializeClassesCollection.instance.actionClass2.tableNumber = Table.instance.table.tableNumber;
        }
        else
        {
            GameSerializeClassesCollection.instance.actionClass2.tableNumber = PokerNetworkManager.instance.tableID;
        }

        GameSerializeClassesCollection.instance.actionClass2.action = actionName;
        if (timesBet == 1)
        {
            GameSerializeClassesCollection.instance.actionClass2.amount = (int)value;
        }

        if (GameSerializeClassesCollection.instance.actionClass2.amount >= GameManagerScript.instance.playersParent.transform.GetChild(GameManagerScript.instance.localPlayerSeatid - 1).GetChild(0).transform.GetComponent<PokerPlayerController>().player.chips)
        {
            GameSerializeClassesCollection.instance.actionClass2.action = "allin";
        }

        GameSerializeClassesCollection.instance.actionClass2.token = Communication.instance.playerToken;
        string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.actionClass2);
        UIManagerScript.instance.TurnOff2x3x4x();
        ComandExecuted();
        GameManagerScript.instance.timesRaise = 1;
        if (GameManagerScript.instance.isTournament)
        {
            print("Action Data " + data);
            TournamentManagerScript.instance.socket.Emit("__action", new JSONObject(data));
        }
        else
        {
            print("Action Data " + data);
            PokerNetworkManager.instance.socket.Emit("__action", new JSONObject(data));
        }
        //SoundManager.instance.alarmsound.volume = 0;

        UIManagerScript.instance.alarmClock.SetActive(false);
        SoundManager.instance.alarmClocksound.volume = 0;
        SoundManager.instance.alarmClocksound.Stop();
    }
    //........................................................................................................................//


}
