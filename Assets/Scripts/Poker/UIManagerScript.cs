using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SmartLocalization;

public class UIManagerScript : MonoBehaviour
{
    public GameObject menu_panelUIButton;
    public Transform localPlayerMenu;
    public Transform tableParent;
    public Transform footer;
    public Transform cards;
    public Transform observingPanel;
    public Transform tableStartButton;
    public Transform NonVideoProfilePanel;
    public GameObject loading2;
    public GameObject newWinnerCards;
    public Transform newdealer;
    public string statsUrl;

    public GameObject topUpChipBtn;
    public GameObject switchTableBtn;
    public GameObject standUpBtn;
    public GameObject friendBtn;

    public GameObject alarmClock;
    public Text alarmClockTime;

    [Space]
    [Header("Raise")]
    public Transform raisePanel;
    public Transform raiseSlider;
    [Space]
    [Header("Bet")]
    public Transform betPanel;
    public Transform betSlider;
    [Space]
    [Header("Buy In Panel")]
    public Transform topUpPanelSocialPoker;
    public Transform buyInSlider;
    public Text minBuy;
    public Text maxBuy;
    [Space]
    [Header("Buy In Panel 2")]
    public Transform buyInPanel2;
    public Transform buyInSlider2;
    public Text minBuy2;
    public Text maxBuy2;
    public Transform buyInSlider2Percent;
    //public Text buyInSliderPerText;
    [Space]
    [Header("Top Up Panel")]
    public Transform topUpPanel;
    public Transform topUpSlider;
    public Text minBuyTopUp;
    public Text maxBuyTopUp;
    [Space]
    [Header("Pop Up Panel")]
    public Transform lowChipPanel;
    public Text lowChipPanelText;
    [Space]
    [Space]
    [Header("Disband Panel")]
    public Transform disbandPanel;
    public Toggle disbandToggle1;
    public Toggle disbandToggle2;
    [Space]
    public GameObject winPanel;
    public GameObject quitPanel;
    public GameObject tableMenuPanel;
    public GameObject emptypChipsPanel;
    public GameObject emptypChipsLessBuyInPanel;
    public GameObject accountBalanceZeroPanel;

    public GameObject tableMenuPanel_regular;
    public GameObject tableMenuPanel_tournament;

    //public GameObject observerContent;
    public GameObject postBlind;
    public GameObject joinWaitList;
    public GameObject lowestLayerPanel;
    public GameObject straddlePanel;
    public GameObject straddlePanelSymbol;
    public GameObject tipPanel;
    public GameObject addOnPanel;
    public GameObject rebuyPanel;
    public GameObject loadingPanel;
    public GameObject mttSideInfoPanel;
    public GameObject mttSideInfoPanelSymbol;
    public GameObject breakTimePanel;
    public GameObject tournamentInfoButton;
    public GameObject observingUI;
    public Text breakTimeText;
    public Transform tableInfo;
    public Transform tournmtWinnerPanel;
    public Transform waitOtherPlayerPanel;
    [Space]
    public Transform winPanelTournmentKO;
    public Transform winPanelTournment;
    public Transform gameLeftPanelTournment;
    public Transform gameLeftPanelSatelite;
    public Transform addOnRebuyButtonPanelTournment;
    [Space]
    //public bool isPlayerShuffle;
    public List<GameObject> betAndRaiseHighlightImage;
    public List<GameObject> allOtherBottomPanelBtn;
    public static UIManagerScript instance;

    [Header("TournamentTimer Text")]
    public Text tournamentIncreaseTimer;
    public Text levelTime;

    [Space]
    [Header("Ranking InnerPanels")]
    internal int innerPanelrankCount;
    internal GameObject innerPanelScrollRankItemObj;
    public Transform innerPanelRankListingPanel;
    public Transform innerPanelRankListingContent;
    public List<GameObject> innerPanelRankList;

    [Space]
    [Header("Tables InnerPanels")]
    internal int innerPanelTableCount;
    internal GameObject scrollInnerPanelTableItemObj;
    public Transform innerPanelTableListingPanel;
    public Transform innerPanelTableListingContent;
    public List<GameObject> innerPanelTableList;

    [Space]
    [Header("Prize/Rewards InnerPanels")]
    internal int innerPanelPrizeCount;
    internal GameObject scrollInnerPanelPrizeItemObj;
    public Transform innerPanelPrizeListingPanel;
    public Transform innerPanelPrizeListingContent;
    public List<GameObject> innerPanelPrizeList;

    [Space]
    [Header("Blinds InnerPanels")]
    internal int innerPanelBlindsCount;
    internal GameObject scrollInnerPanelBlindsItemObj;
    public Transform innerPanelBlindsListingPanel;
    public Transform innerPanelBlindsListingContent;
    public List<GameObject> innerPanelBlindsList;

    [Space]
    public Transform closeRaisePanel;

    public Texture2D videoPoorConnectionTexture;

    public GameObject videoPanelParent;
    [Space]

    public GameObject VideoReportPanel;
    public GameObject VideoReportPanelTournament;
    [Space]
    public Transform animCard1;
    public Transform animCard4;
    public Transform animCard5;

    public Text totalPotWin;
    public Text mainPotWin;
    public GameObject stackParentWin;

    [Space]
    [Header("Tournament Info Panel")]
    public Text tournamentName;
    public Text tournamentID;
    public Text position;
    public Text level;
    public Text remaining;
    public Text lateRegistration;
    public Text prizePool;
    public Text onGoing;
    public Text avgStack;
    public Text smallestStack;
    public Text rebuys;
    public Text addOns;
    public Text largestStack;
    public Text totalBuyIns;
    public Text nextLevelTimer;
    public Text currentLevel;
    public Text nextLevel;

    [Space]
    [Header("Top-Up Panel Timer")]
    public Text timerCountdown;
    public Text timerCountdown2;

    [Space]
    [Header("Tournament Info Panel Lists")]
    public List<GameObject> infoPanelNavigationTabList;
    public List<GameObject> infoPanelTabList;

    [Space]
    [Header("Tournament Info Panel Gameobjects")]
    public GameObject rankingPanel;
    public GameObject rankingContent;
    public GameObject prizePanel;
    public GameObject prizeContent;
    public GameObject tablePanel;
    public GameObject tableContent;
    public GameObject tablePage;
    public GameObject blindsPanel;
    public GameObject blindsContent;
    public GameObject standUpButton;
    public Slider newBuyInSlider;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        statsUrl = ServerChanger.instance.domainURL + "api/v1/user/user-Statistics";
        checkfriendStatusURL = ServerChanger.instance.domainURL + "api/v1/user/check-friend-status";
        innerPanelrankCount = 1;
        innerPanelTableCount = 1;
        innerPanelTableCount = 1;
        innerPanelBlindsCount = 1;
        //HideCommands();
    }

    private void Update()
    {
        //......................................Raise And Bet Slider.....................................................................//
        if (raiseSlider.gameObject.activeInHierarchy)
        {
            string str = GameManagerScript.instance.KiloFormat((int)raiseSlider.GetComponent<Slider>().value);
            raiseSlider.parent.GetChild(1).GetChild(0).GetComponent<Text>().text = str;
            //raiseSlider.parent.GetChild(1).GetChild(0).GetComponent<Text>().text = "" + raiseSlider.GetComponent<Slider>().value;
        }
        if (betSlider.gameObject.activeInHierarchy)
        {
            string str2 = GameManagerScript.instance.KiloFormat((int)betSlider.GetComponent<Slider>().value);
            betSlider.parent.GetChild(1).GetChild(0).GetComponent<Text>().text = str2;
            //betSlider.parent.GetChild(1).GetChild(0).GetComponent<Text>().text = "" + betSlider.GetComponent<Slider>().value;
        }
        //........................................................................................................................//


        //......................................Buy-In Slider.....................................................................//
        if (buyInSlider.GetComponent<Slider>().value > GameSerializeClassesCollection.instance.chipBalance.chipBalance || buyInSlider2.GetComponent<Slider>().value > GameSerializeClassesCollection.instance.chipBalance.chipBalance)
        {
            buyInSlider.GetComponent<Slider>().value = GameSerializeClassesCollection.instance.chipBalance.chipBalance;
            buyInSlider2.GetComponent<Slider>().value = GameSerializeClassesCollection.instance.chipBalance.chipBalance;
        }
        buyInSlider.GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = "" + buyInSlider.GetComponent<Slider>().value;
        buyInSlider2.GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = "" + buyInSlider2.GetComponent<Slider>().value;
        buyInSlider2Percent.GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = "" + buyInSlider2Percent.GetComponent<Slider>().value;

        //........................................................................................................................//

        //......................................Top-UP Slider.....................................................................//
        if (topUpSlider.GetComponent<Slider>().value > GameSerializeClassesCollection.instance.topUpChipBalance.chipBalance)
        {
            topUpSlider.GetComponent<Slider>().value = GameSerializeClassesCollection.instance.topUpChipBalance.chipBalance;
        }
        topUpSlider.GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = "" + topUpSlider.GetComponent<Slider>().value;
        //........................................................................................................................//

        if (GameManagerScript.instance.activeTable != null && GameManagerScript.instance.activeTable.activeInHierarchy)
        {
            if (GameManagerScript.instance.totalPlayersSitting == 1)
            {
                if (GameManagerScript.instance.isTournament)
                {
                    waitOtherPlayerPanel.GetChild(2).GetChild(0).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("Waiting to merge");//"Waiting to merge the table...";
                    if (!winPanelTournment.gameObject.activeInHierarchy)
                    {
                        waitOtherPlayerPanel.gameObject.SetActive(true);
                        if (GameManagerScript.instance.HandRankPanel != null)
                        {
                            GameManagerScript.instance.HandRankPanel.SetActive(false);
                        }
                    }
                    else
                    {
                        waitOtherPlayerPanel.gameObject.SetActive(false);
                    }
                }
                else
                {
                    waitOtherPlayerPanel.GetChild(2).GetChild(0).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("waiting other player");//"Waiting for other players to join...";
                    waitOtherPlayerPanel.gameObject.SetActive(true);
                    if (GameManagerScript.instance.HandRankPanel != null)
                    {
                        GameManagerScript.instance.HandRankPanel.SetActive(false);
                    }
                }
            }
            else
            {
                waitOtherPlayerPanel.gameObject.SetActive(false);
            }
        }


    }

    //......................................On-Click Functions.....................................................................//
    public void OnclickBuyInToggle()
    {
        if (topUpPanelSocialPoker.GetChild(0).GetChild(5).GetChild(0).GetComponent<Toggle>().isOn)
        {
            topUpPanelSocialPoker.gameObject.SetActive(false);
            buyInPanel2.gameObject.SetActive(true);
            minBuy2.text = minBuy.text;
            maxBuy2.text = maxBuy.text;
            buyInSlider2.GetComponent<Slider>().value = buyInSlider.GetComponent<Slider>().value;
            buyInPanel2.GetChild(0).GetChild(5).GetChild(0).GetComponent<Toggle>().isOn = true;
        }
    }
    public void OnclickBuyInToggle2()
    {
        if (!buyInPanel2.GetChild(0).GetChild(5).GetChild(0).GetComponent<Toggle>().isOn)
        {
            buyInPanel2.gameObject.SetActive(false);
            topUpPanelSocialPoker.gameObject.SetActive(true);
            minBuy.text = minBuy2.text;
            maxBuy.text = maxBuy2.text;
            buyInSlider.GetComponent<Slider>().value = buyInSlider2.GetComponent<Slider>().value;
            topUpPanelSocialPoker.GetChild(0).GetChild(5).GetChild(0).GetComponent<Toggle>().isOn = false;
        }
    }
    //............................................................................................................................//

    //public void CheckRaiseSlider()
    //{
    //    float maxValue = raiseSlider.GetComponent<Slider>().maxValue;
    //    float diff = maxValue * (0.4f);
    //    if (raiseSlider.GetComponent<Slider>().value <= diff)
    //    {
    //        raiseSlider.GetComponent<Slider>().value = diff; 
    //        raisePanel.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = "" + (raiseSlider.GetComponent<Slider>().minValue );
    //    }
    //    else
    //    {
    //        raisePanel.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = "" + (raiseSlider.GetComponent<Slider>().value);
    //    }
    //}

    public void TurnOff2x3x4x()
    {

        for (int i = 0; i < betAndRaiseHighlightImage.Count; i++)
        {
            betAndRaiseHighlightImage[i].SetActive(false);
        }
    }

    //......................................On-Click Functions.....................................................................//
    public void OpenRebuyAddOn(string gameObjectName)
    {
        addOnRebuyButtonPanelTournment.gameObject.SetActive(true);
        if (gameObjectName == "rebuy")
        {
            addOnRebuyButtonPanelTournment.GetChild(0).GetChild(1).gameObject.SetActive(false);
            addOnRebuyButtonPanelTournment.GetChild(0).GetChild(0).gameObject.SetActive(true);
        }
        else if (gameObjectName == "addon")
        {
            addOnRebuyButtonPanelTournment.GetChild(0).GetChild(0).gameObject.SetActive(false);
            addOnRebuyButtonPanelTournment.GetChild(0).GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            addOnRebuyButtonPanelTournment.gameObject.SetActive(false);
        }
    }

    public void OpenAddOnRebuyPanel(string gameObjectName)
    {
        addOnRebuyButtonPanelTournment.gameObject.SetActive(false);
        if (gameObjectName == "rebuy")
        {
            rebuyPanel.SetActive(true);
        }
        else if (gameObjectName == "addon")
        {
            addOnPanel.SetActive(true);
        }
    }

    //............It check Current blinds level and closes rebuy button for observer.....................//
    public void CheckLateRegLevel()
    {
        if (TournamentScript.instance.lateRegVal == Table.instance.table.currentLevel + 1)
        {
            addOnRebuyButtonPanelTournment.gameObject.SetActive(false);
            addOnRebuyButtonPanelTournment.GetChild(0).GetChild(0).gameObject.SetActive(false);
        }
    }

    public void CloseRaiseAndBet()
    {
        closeRaisePanel.gameObject.SetActive(false);
        print("Raise Panel close......");
        footer.GetChild(0).GetChild(2).gameObject.SetActive(false);
        footer.GetChild(0).GetChild(4).gameObject.SetActive(false);
        footer.GetChild(0).GetChild(0).gameObject.SetActive(true);
        if (GameManagerScript.instance.isVideoTable)
        {
            raiseSlider.parent.gameObject.SetActive(false);
            betSlider.parent.gameObject.SetActive(false);
        }

    }

    public void ClickOnRaiseTimes(GameObject gameObject)
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
            StartCoroutine(OFFRaise());
        }
        else
        {
            TurnOff2x3x4x();
            gameObject.SetActive(true);
        }
    }
    IEnumerator OFFRaise()
    {
        yield return new WaitForSeconds(0.1f);

        GameManagerScript.instance.timesRaise = 1;
        if (raisePanel.gameObject.activeInHierarchy)
        {
            raiseSlider.GetComponent<Slider>().value = GameManagerScript.instance.minBet;
        }
        if (betPanel.gameObject.activeInHierarchy)
        {
            betSlider.GetComponent<Slider>().value = GameManagerScript.instance.minBet;
        }
    }

    public void ClickOnRaiseTimesValue(int times)
    {
        print("click......1" + times);
        GameManagerScript.instance.timesRaise = times;
        if (raisePanel.gameObject.activeInHierarchy)
        {
            print("click......2" + times);

            //raiseSlider.GetComponent<Slider>().value = GameManagerScript.instance.minBet * times;
            if (times == 1)
            {
                raiseSlider.GetComponent<Slider>().value = raiseSlider.GetComponent<Slider>().maxValue;
            }
            else
            {
                raiseSlider.GetComponent<Slider>().value = (GameManagerScript.instance.reRaise * times) - GameManagerScript.instance.currentPlayerTableBet;
            }
        }
        GameManagerScript.instance.timesRaise = 1;

        if (betPanel.gameObject.activeInHierarchy)
        {
            Table.instance.TotalPot.text = SocialGame.instance.ConvertChipsToString(Table.instance.TotalPot.text);

            print("kkkkkkkkklllllllllll00000-- " + Table.instance.TotalPot.text);

            float totalPotvalue;

            if (times == 3)
            {
                print("kkkkkkkkkllllllllllll11111111111-- " + Table.instance.TotalPot.text);

                totalPotvalue = (float.Parse(Table.instance.TotalPot.text.Replace("Pot: ", string.Empty)) * 2) / times;
                betSlider.GetComponent<Slider>().value = totalPotvalue;
            }
            if (times == 2)
            {
                totalPotvalue = float.Parse(Table.instance.TotalPot.text.Replace("Pot: ", string.Empty)) / times;
                betSlider.GetComponent<Slider>().value = totalPotvalue;
            }
            if (times == 1)
            {
                totalPotvalue = float.Parse(Table.instance.TotalPot.text.Replace("Pot: ", string.Empty));
                betSlider.GetComponent<Slider>().value = totalPotvalue;
                print("Total Pot Value " + totalPotvalue);
            }
            if (times == 0)
            {
                betSlider.GetComponent<Slider>().value = betSlider.GetComponent<Slider>().maxValue;
                //print("Total Pot Value " + totalPotvalue);
            }
        }
    }

    public void ExitButton()                                              // On Click Exit Button On Video Table
    {
        quitPanel.SetActive(true);
        tableMenuPanel.SetActive(false);
    }

    public void CloseButton(GameObject gameObject)                        // On Click Cancel or cross Button. Takes any Gameobject and Deactives it. 
    {
        gameObject.SetActive(false);

    }

    public void OpenButton(GameObject gameObject)                        // On Click Cancel or cross Button. Takes any Gameobject and Deactives it. 
    {
        StartCoroutine(OpenMenuPanel());
        print("Click");
        TableWebAPICommunication.instance.StartGetObserverCoroutine();
    }

    IEnumerator OpenMenuPanel()
    {
        yield return null;

        if (GameManagerScript.instance.isTournament)
        {
            tableMenuPanel_tournament.SetActive(true);
            tableMenuPanel_regular.SetActive(false);

        }
        else
        {
            tableMenuPanel_regular.SetActive(true);
            tableMenuPanel_tournament.SetActive(false);

            if (GameManagerScript.instance.isSwitchForJoinFriends || GameManagerScript.instance.isSwitchForTableSwitch ||
                         GameManagerScript.instance.isPlayerExcluded || GameManagerScript.instance.isStandupClicked ||
                         FriendandSocialScript.instance.isSeatReserved)
            {

                if (GameManagerScript.instance.totalPlayersSitting >= 1 && !GameManagerScript.instance.isSwitchForTableSwitch)
                {

                    switchTableBtn.transform.GetComponent<Button>().interactable = true;
                    switchTableBtn.transform.GetChild(0).GetComponent<Text>().color = new Color(1f, 1f, 1f, 1f);
                    switchTableBtn.transform.GetChild(1).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

                    standUpBtn.transform.GetComponent<Button>().interactable = false;
                    standUpBtn.transform.GetChild(0).GetComponent<Text>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                    standUpBtn.transform.GetChild(1).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);

                    topUpChipBtn.transform.GetComponent<Button>().interactable = false;
                    topUpChipBtn.transform.GetChild(0).GetComponent<Text>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                    topUpChipBtn.transform.GetChild(1).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                }
                else
                {
                    switchTableBtn.transform.GetComponent<Button>().interactable = false;
                    switchTableBtn.transform.GetChild(0).GetComponent<Text>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                    switchTableBtn.transform.GetChild(1).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);

                    standUpBtn.transform.GetComponent<Button>().interactable = false;
                    standUpBtn.transform.GetChild(0).GetComponent<Text>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                    standUpBtn.transform.GetChild(1).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);

                    topUpChipBtn.transform.GetComponent<Button>().interactable = false;
                    topUpChipBtn.transform.GetChild(0).GetComponent<Text>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                    topUpChipBtn.transform.GetChild(1).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);

                    if(!FriendandSocialScript.instance.isSeatReserved)
                    {
                        friendBtn.transform.GetComponent<Button>().interactable = false;
                        friendBtn.transform.GetChild(0).GetComponent<Text>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                        friendBtn.transform.GetChild(1).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                    }
                }
            }
            else
            {

                switchTableBtn.transform.GetComponent<Button>().interactable = true;
                switchTableBtn.transform.GetChild(0).GetComponent<Text>().color = new Color(1f, 1f, 1f, 1f);
                switchTableBtn.transform.GetChild(1).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

                standUpBtn.transform.GetComponent<Button>().interactable = true;
                standUpBtn.transform.GetChild(0).GetComponent<Text>().color = new Color(1f, 1f, 1f, 1f);
                standUpBtn.transform.GetChild(1).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

                topUpChipBtn.transform.GetComponent<Button>().interactable = true;
                topUpChipBtn.transform.GetChild(0).GetComponent<Text>().color = new Color(1f, 1f, 1f, 1f);
                topUpChipBtn.transform.GetChild(1).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

                friendBtn.transform.GetComponent<Button>().interactable = true;
                friendBtn.transform.GetChild(0).GetComponent<Text>().color = new Color(1f, 1f, 1f, 1f);
                friendBtn.transform.GetChild(1).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

                if (GameManagerScript.instance.totalPlayersSitting == 1)
                {
                    standUpBtn.transform.GetComponent<Button>().interactable = false;
                    standUpBtn.transform.GetChild(0).GetComponent<Text>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                    standUpBtn.transform.GetChild(1).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);

                    topUpChipBtn.transform.GetComponent<Button>().interactable = false;
                    topUpChipBtn.transform.GetChild(0).GetComponent<Text>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                    topUpChipBtn.transform.GetChild(1).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                }

            }

        }
    }

    public void OpenMenuButton()                                          // On Click Open Menu Button On Video Table
    {
        TableWebAPICommunication.instance.StartGetObserverCoroutine();
        tableMenuPanel.SetActive(true);
    }

    public void QuitButton()                                              // On Click Exit Confirm Button On Video Table
    {
        CommonFunctions.instance.QuitApp();
    }

    public void TableToPokerUI(int response)                                          // On Click Exit Confirm Button On Video Table
    {
        try
        {
            GameManagerScript.instance.totalPlayersSitting = 0;
            GameManagerScript.instance.isObserver = false;
            AgoraInit.instance.ResetUserJoinCount(0);

            quitPanel.SetActive(false);

            if (GameManagerScript.instance.isTournament)
            {
                print("EXIT>>>>>>>>>>");
                if (TournamentManagerScript.instance.timeSubscribeEvent == 1)
                {
                    TournamentManagerScript.instance.UnSubscribeToServerEvents();
                }
                SocialTournamentScript.instance.RegisteredTournamentRequest();
            }
            else
            {
                GameManagerScript.instance.chairAnimForLocalPlayer = true;
                PokerNetworkManager.instance.UnSubscribeToServerEvents();

            }
            StartCoroutine(TableToPokerUICoroutine(response));

        }
        catch
        {
            StartCoroutine(TableToPokerUICoroutine(response));
            print("Error in Table To Poker UI Function");
        }

    }

    IEnumerator TableToPokerUICoroutine(int response)
    {
        loadingPanel.SetActive(true);
        loadingPanel.transform.GetChild(1).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("Exiting to lobby");
        if (GameManagerScript.instance.isChanceActive)
        {
            yield return new WaitForSeconds(1.5f);
            GameManagerScript.instance.isCheckActive = false;
            PlayerActionManagement.instance.PlayerActionEmitter("fold");
            yield return new WaitForSeconds(0.5f);

        }
        if (!GameManagerScript.instance.isTournament)
        {
            if (GameManagerScript.instance.networkManager.activeInHierarchy)
            {
                if (response != 1 || response != 2)
                {
                    GameSerializeClassesCollection.instance.localPlayerExitHandler.token = GameSerializeClassesCollection.instance.observeTable.token;
                    string data2 = JsonUtility.ToJson(GameSerializeClassesCollection.instance.localPlayerExitHandler);
                    print("__exit_handle " + data2);
                    PokerNetworkManager.instance.socket.Emit("__exit_handle", new JSONObject(data2));
                }
                //GameSerializeClassesCollection.instance.localPlayerConfirmation.playerName = AccessGallery.instance.profileName[0].text;
                //GameSerializeClassesCollection.instance.localPlayerConfirmation.ticket = PokerNetworkManager.instance.tableID;
                //GameSerializeClassesCollection.instance.localPlayerConfirmation.seatId = PokerNetworkManager.instance.localPlayerSeatId;
                //string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.localPlayerConfirmation);
                //print("local Player Exit Confirmation " + data);
                //PokerNetworkManager.instance.socket.Emit("__left_player_details", new JSONObject(data));
            }
        }
        yield return new WaitForSeconds(0.5f);
        if (GameManagerScript.instance.isTournament)
        {
            print("disconnect1");
            //TournamentManagerScript.instance.socket.Emit("disconnect");
            TournamentManagerScript.instance.socket.Close();
        }
        else
        {
            print("disconnect2");
            //PokerNetworkManager.instance.socket.Emit("disconnect");
            if (GameManagerScript.instance.networkManager.activeInHierarchy)
            {
                PokerNetworkManager.instance.socket.Close();
            }
        }
        AgoraInit.instance.LeaveChannel();

        if (!GameManagerScript.instance.isVideoTable)
        {
            Chat.instance.CloseChat();
        }

        //yield return new WaitForSeconds(2);

        loadingPanel.SetActive(false);
        loadingPanel.transform.GetChild(1).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("Reconnecting");//"Reconnecting...";
        if (GameManagerScript.instance.activeTable != null)
        {
            GameManagerScript.instance.activeTable.gameObject.SetActive(false);
        }
        Screen.orientation = ScreenOrientation.Portrait;

        yield return new WaitForSeconds(1f);
        SocialGame.instance.pokerUICanvas.GetComponent<Canvas>().enabled = false;
        //Screen.orientation = ScreenOrientation.Portrait;

        //SocialGame.instance.pokerUICanvas.SetActive(true);
        //SocialGame.instance.img_L_P.SetActive(true);

        ClubManagement.instance.loadingPanel.SetActive(true);

        //yield return new WaitForSeconds(1);
        if (response == 1)
        {
            Cashier.instance.toastMsgPanel.SetActive(true);
            Cashier.instance.toastMsg.text = "No internet. Please check your connection and join the table back to continue playing";
        }

        if (response == 2)
        {
            Cashier.instance.toastMsgPanel.SetActive(true);
            Cashier.instance.toastMsg.text = "Unstable Internet, Please check your internet or join the non-video table to continue playing";
        }

        if (response == 3)
        {
            //Cashier.instance.toastMsgPanel.SetActive(true);
            //Cashier.instance.toastMsg.text = "GAME OVER !!";
        }
        if (response == 4)
        {
            //Cashier.instance.toastMsgPanel.SetActive(true);
            //Cashier.instance.toastMsg.text = "You Are Removed from the Table by Server !!";
        }

        if (response == 5)
        {
            //Cashier.instance.toastMsgPanel.SetActive(true);
            //Cashier.instance.toastMsg.text = " We are Getting Event error in Player Generation !!";
        }
        print("UNSuB......1");


        GameManagerScript.instance.isPlayerGenerating = false;

        //yield return new WaitForSeconds(0.5f);
        try
        {
            waitOtherPlayerPanel.gameObject.SetActive(false);
            tableStartButton.gameObject.SetActive(false);
            postBlind.gameObject.SetActive(false);
        }
        catch
        {
            print("Error......");
        }

        print("Command Hided table to poker........");
        HideCommands();
        GameManagerScript.instance.ResetTable(true);
        Table.instance.ResetSidePot();
        if (GameManagerScript.instance.HandRankPanel != null)
        {
            GameManagerScript.instance.HandRankPanel.transform.GetChild(0).GetComponent<Text>().text = "";
            GameManagerScript.instance.HandRankPanel.SetActive(false);
        }

        //yield return new WaitForSeconds(0.5f);

        ClubManagement.instance.loadingPanel.SetActive(true);

        if (!GameManagerScript.instance.isDirectLogIn)
        {
            TableTimer.instance.StopRunningTimeCoroutine();
            TableTimer.instance.StopRemainingTimeCoroutine();
            //Uimanager.instance.MenuCanvas.SetActive(true);
        }

        try
        {
            GameManagerScript.instance.StopCoroutineTimer();
        }
        catch
        {
            print("Timer already Stopped");
            if (!GameManagerScript.instance.isDirectLogIn)
            {
                //ClubManagement.instance.loadingPanel.SetActive(false);
            }
        }
        try
        {
            ResetCardsPosition();
            GameManagerScript.instance.ResetTableOnExit(false);
        }

        catch
        {
            print("Timer already Stopped");
            if (!GameManagerScript.instance.isDirectLogIn)
            {
                //ClubManagement.instance.loadingPanel.SetActive(false);
            }
        }
        GameManagerScript.instance.dealerName = null;
        GameManagerScript.instance.dealerSeatid = 0;
        GameManagerScript.instance.localSeatIDDummy = 0;
        GameManagerScript.instance.localSeatID = 0;
        GameManagerScript.instance.addPlayers.Clear();

        topUpPanel.gameObject.SetActive(false);
        winPanel.GetComponent<Image>().color = new Color(0, 0, 0, 0.6f);
        //winPanel.transform.GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, 1);
        winPanel.gameObject.SetActive(false);
        topUpPanelSocialPoker.gameObject.SetActive(false);
        buyInPanel2.gameObject.SetActive(false);
        raisePanel.gameObject.SetActive(false);
        betPanel.gameObject.SetActive(false);
        joinWaitList.gameObject.SetActive(false);
        addOnRebuyButtonPanelTournment.gameObject.SetActive(false);
        //........................Only Tournment Panels.................................//
        winPanelTournmentKO.gameObject.SetActive(false);
        winPanelTournment.gameObject.SetActive(false);
        gameLeftPanelTournment.gameObject.SetActive(false);
        closeRaisePanel.gameObject.SetActive(false);
        //SocialGame.instance.pokerUICanvas.SetActive(true);
        Screen.orientation = ScreenOrientation.Portrait;
        SocialGame.instance.tournamentDetialPanel.SetActive(false);
        //..............................................................................//
        //yield return new WaitForSeconds(0.5f);

        if (!GameManagerScript.instance.isDirectLogIn)
        {
            //ClubManagement.instance.loadingPanel.SetActive(false);
        }

        try
        {
            PlayersGenerator.instance.videoPanelPlayers.Clear();
            for (int i = 0; i < PlayersGenerator.instance.videoPanelsForAllClient.Count; i++)
            {
                Destroy(PlayersGenerator.instance.videoPanelsForAllClient[i].gameObject);
            }
            PlayersGenerator.instance.videoPanelsForAllClient.Clear();
        }
        catch
        {
            print("Problem In video");
        }

        GameManagerScript.instance.activeTable = null;
        if (ImageDownloadScript.instance.startTimeCorotine != null)
        {
            StopCoroutine(ImageDownloadScript.instance.startTimeCorotine);
        }

        ChairAnimation.instance.ResetChairPosition(true);
        //SocialProfile._instance.SocialChips();
        if (response == 6)
        {
            Cashier.instance.toastMsg.text = LanguageManager.Instance.GetTextValue("session expired"); /*SocialTournamentScript.instance.login_error*/
            Cashier.instance.toastMsgPanel.SetActive(true);
            Uimanager.instance.SignOut();
        }

        //yield return new WaitForSeconds(1f);
        //try
        //{
        //    StopAllCoroutines();
        //}
        //catch
        //{
        //    print("Error in stopping coroutine...");
        //}
        print("UImanager Exit");
        PokerSceneManagement.instance.RestartScene();
        //SceneManager.LoadScene(0);
        //StartCoroutine(ExitGame());
    }

    //IEnumerator ExitGame()
    //{
    //    if (GameManagerScript.instance.isTournament)
    //    {
    //        yield return new WaitForSeconds(0.1f);
    //        GameManagerScript.instance.Tournamentsocket.Close();
    //        GameManagerScript.instance.tournamentManager.SetActive(false);
    //        GameManagerScript.instance.SocketReset();

    //        yield return new WaitForSeconds(0.2f);
    //        //SocialGame.instance.pokerUICanvas.SetActive(true);
    //        //SocialTournamentScript.instance.ClickUpcomingTab();
    //        //SocialTournamentScript.instance.OpenGameDetailPageAfterRegistration(SocialTournamentScript.instance.tournament_ID);
    //    }
    //}

    public void ReShuffleTable()
    {
        //if()
        UIManagerScript.instance.breakTimePanel.SetActive(false);
        loadingPanel.SetActive(true);
        loadingPanel.transform.GetChild(1).GetComponent<Text>().text = "Reshuffling Table...";
        SocialTournamentScript.instance.CloseAllInfoPanels();
        Table.instance.cardOpened = 0;
        GameManagerScript.instance.totalPlayersSitting = 0;
        tableStartButton.gameObject.SetActive(false);
        postBlind.gameObject.SetActive(false);
        //AgoraInit.instance.LeaveChannel();
        print("Command Hided in reshuffle........");
        HideCommands();
        GameManagerScript.instance.ResetTable(false);
        Table.instance.ResetSidePot();
        if (GameManagerScript.instance.HandRankPanel != null)
        {
            GameManagerScript.instance.HandRankPanel.transform.GetChild(0).GetComponent<Text>().text = "";
            GameManagerScript.instance.HandRankPanel.SetActive(false);
        }
        try
        {
            ResetCardsPosition();
            GameManagerScript.instance.ResetTableOnExit(false);
        }

        catch
        {
            print("Timer already Stopped");
            if (!GameManagerScript.instance.isDirectLogIn)
            {
                ClubManagement.instance.loadingPanel.SetActive(false);
            }
        }
        GameManagerScript.instance.localSeatIDDummy = 0;
        GameManagerScript.instance.localSeatID = 0;
        GameManagerScript.instance.addPlayers.Clear();
        //.................//
        quitPanel.SetActive(false);
        topUpPanel.gameObject.SetActive(false);
        //winPanel.GetComponent<Image>().color = new Color(0, 0, 0, 0.6f);
        //winPanel.transform.GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, 1);
        winPanel.gameObject.SetActive(false);
        topUpPanelSocialPoker.gameObject.SetActive(false);
        buyInPanel2.gameObject.SetActive(false);
        raisePanel.gameObject.SetActive(false);
        betPanel.gameObject.SetActive(false);
        //..........................//


        try
        {
            if (GameManagerScript.instance.isVideoTable)
            {
                AgoraInit.instance.LeaveChannel();
                TournamentManagerScript.instance.observerList.Clear();

                for (int i = 0; i < videoPanelParent.transform.childCount; i++)
                {
                    Destroy(videoPanelParent.transform.GetChild(i).gameObject);
                }
                AgoraInit.instance.ResetUserJoinCount(0);
                PlayersGenerator.instance.videoPanelPlayers.Clear();
                for (int i = 0; i < PlayersGenerator.instance.videoPanelsForAllClient.Count; i++)
                {
                    Destroy(PlayersGenerator.instance.videoPanelsForAllClient[i].gameObject);
                }
                PlayersGenerator.instance.videoPanelsForAllClient.Clear();

                Destroy(GameManagerScript.instance.hostPlayerVideoPanel);
                GameObject go = new GameObject();
                go.name = "HostPlayerVideoPanel";
                go.AddComponent<RawImage>();
                GameManagerScript.instance.hostPlayerVideoPanel = go;
                GameManagerScript.instance.hostPlayerVideoPanel.transform.SetParent(UIManagerScript.instance.tableParent.parent);
                GameManagerScript.instance.hostPlayerVideoPanel.transform.localPosition = Vector3.zero;
                GameManagerScript.instance.hostPlayerVideoPanel.transform.localEulerAngles = new Vector3(0, 0, 0);
                GameManagerScript.instance.hostPlayerVideoPanel.transform.localScale = new Vector3(1, 1, 1);
                GameManagerScript.instance.hostPlayerVideoPanel.transform.GetComponent<RawImage>().color = new Color(1, 1, 1, 0);
                GameManagerScript.instance.hostPlayerVideoPanel.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(300f, 180f);
            }

        }
        catch
        {
            print("Problem In video");
        }
        GameManagerScript.instance.TableSetEmptytrue();
        GameManagerScript.instance.tournamentManager.gameObject.SetActive(true);

        loadingPanel.SetActive(false);
    }

    public void OpenPlayerName(GameObject gameObject)
    {
        ShowFriendStatus(gameObject.transform.parent.GetChild(0).GetComponent<PokerPlayerController>());
        loadingPanel.transform.GetChild(1).GetComponent<Text>().text = "";
        loadingPanel.SetActive(true);
        //loadingPanel.SetActive(true);
        //GameSerializeClassesCollection.instance.clubId.club_id = ClubManagement.instance._clubID;
        GameSerializeClassesCollection.instance.clubId.username = gameObject.transform.parent.GetChild(0).GetComponent<PokerPlayerController>().player.playerName;
        print(GameSerializeClassesCollection.instance.clubId.username);
        string body = JsonUtility.ToJson(GameSerializeClassesCollection.instance.clubId);
        print("PlayerInfo" + body);
        Communication.instance.PostData(statsUrl, body, OpenProfilePanel);
        localProfilePanel = gameObject;
        //NonVideoProfilePanel.transform.gameObject.SetActive(true);
        print("player Name: " + gameObject.transform.parent.GetChild(0).name);
    }

    GameObject localProfilePanel;
    public string flagImageString;
    public string playerImageString;


    public void OpenProfilePanel(string response)
    {
        //loadingPanel.SetActive(false);

        if (string.IsNullOrEmpty(response))
        {
            print("Some error!!.");
            lowChipPanel.gameObject.SetActive(true);
            lowChipPanelText.text = "Oops some error occured!!";
            NonVideoProfilePanel.transform.gameObject.SetActive(false);
            loadingPanel.SetActive(false);
        }
        else
        {
            print("response" + response);
            NonVideoProfilePanel.transform.gameObject.SetActive(true);
            GameSerializeClassesCollection.instance.playerStats = JsonUtility.FromJson<GameSerializeClassesCollection.PlayerStats>(response);
            NonVideoProfilePanel.transform.GetChild(0).GetChild(5).GetComponent<Text>().text = localProfilePanel.transform.parent.GetChild(0).GetComponent<PokerPlayerController>().player.playerName;
            NonVideoProfilePanel.transform.GetChild(0).GetChild(4).GetComponent<Text>().text = localProfilePanel.transform.parent.GetChild(0).GetComponent<PokerPlayerController>().player.playerName + "      ";
            NonVideoProfilePanel.transform.GetChild(0).GetChild(7).GetComponent<Text>().text = "" + localProfilePanel.transform.parent.GetChild(0).GetComponent<PokerPlayerController>().player.clientId;
            if (GameManagerScript.instance.isTournament)
            {
                NonVideoProfilePanel.transform.GetChild(0).GetChild(9).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.playerStats.vpip_tournament + "%";
                NonVideoProfilePanel.transform.GetChild(0).GetChild(10).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.playerStats.pfr_tournament + "%";
                NonVideoProfilePanel.transform.GetChild(0).GetChild(11).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.playerStats.threeBet_tournament + "%";
                NonVideoProfilePanel.transform.GetChild(0).GetChild(12).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.playerStats.cbet_tournament + "%";
                NonVideoProfilePanel.transform.GetChild(0).GetChild(13).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.playerStats.totalTour;
            }
            else
            {
                NonVideoProfilePanel.transform.GetChild(0).GetChild(9).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.playerStats.vpip + "%";
                NonVideoProfilePanel.transform.GetChild(0).GetChild(10).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.playerStats.prf + "%";
                NonVideoProfilePanel.transform.GetChild(0).GetChild(11).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.playerStats.threeBet + "%";
                NonVideoProfilePanel.transform.GetChild(0).GetChild(12).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.playerStats.cbet + "%";
                NonVideoProfilePanel.transform.GetChild(0).GetChild(13).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.playerStats.totalGame;
            }
            NonVideoProfilePanel.transform.GetChild(0).GetChild(8).GetComponent<Text>().text = "" + GameSerializeClassesCollection.instance.playerStats.city;
            flagImageString = GameSerializeClassesCollection.instance.playerStats.country;
            if (!string.IsNullOrEmpty(flagImageString))
            {
                loadingPanel.transform.GetChild(1).GetComponent<Text>().text = "";
                loadingPanel.SetActive(true);
                Communication.instance.GetImage(flagImageString, FlagImageProcess);
            }

            playerImageString = GameSerializeClassesCollection.instance.playerStats.user_image;
            if (!string.IsNullOrEmpty(playerImageString))
            {
                loadingPanel.transform.GetChild(1).GetComponent<Text>().text = "";
                loadingPanel.SetActive(true);
                Communication.instance.GetImage(playerImageString, PlayerImageProcess);
            }
            NonVideoProfilePanel.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<Image>().sprite = localProfilePanel.transform.parent.GetChild(0).GetComponent<PokerPlayerController>().profileImg.GetChild(1).GetChild(0).GetComponent<Image>().sprite;
        }
    }

    #region AddingFriendButton functionality for non video table....

    string checkfriendStatusURL;
    [Serializable]
    public class RecipientClass
    {

        public string client_id;

    }
    [Serializable]
    public class StatusResponseFromServerClass
    {
        public bool error;
        public string message;
        public int statusCode;
        public List<DataResponse> data;



    }
    [Serializable]
    public class DataResponse
    {
        public string _id;
        public string recipient;
        public string requester;
        public int status;
        public string createdAt;
        public string updatedAt;
        public int _v;
    }

    public void ShowFriendStatus(PokerPlayerController controller)
    {

        RecipientClass reference = new RecipientClass();
        reference.client_id = "" + controller.player.clientId;

        string data = JsonUtility.ToJson(reference);
        Debug.Log(">>chkstatusresponse" + data);

        if (controller.isLocalPlayer)
        {
            NonVideoProfilePanel.transform.GetChild(0).GetChild(15).gameObject.SetActive(false);
        }
        else
        {
            Communication.instance.PostData(checkfriendStatusURL, data, ResponseFromFriendStatus);

        }

    }

    public void ResponseFromFriendStatus(string response)
    {
        Debug.Log(">>chkstatusresponse" + response);

        if (!string.IsNullOrEmpty(response))
        {
            StatusResponseFromServerClass obj = JsonUtility.FromJson<StatusResponseFromServerClass>(response);
            if (!obj.error)
            {
                if (obj.data.Count > 0)
                {
                    if (obj.data[0].status == 0)
                    {
                        // user request is pending..

                        NonVideoProfilePanel.transform.GetChild(0).GetChild(15).gameObject.GetComponent<Button>().interactable = false;
                        NonVideoProfilePanel.transform.GetChild(0).GetChild(15).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                        NonVideoProfilePanel.transform.GetChild(0).GetChild(15).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        NonVideoProfilePanel.transform.GetChild(0).GetChild(15).gameObject.transform.GetChild(2).gameObject.SetActive(false);
                    }
                    else if (obj.data[0].status == 1)
                    {

                        // user is already a friend..
                        NonVideoProfilePanel.transform.GetChild(0).GetChild(15).gameObject.GetComponent<Button>().interactable = false;
                        NonVideoProfilePanel.transform.GetChild(0).GetChild(15).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                        NonVideoProfilePanel.transform.GetChild(0).GetChild(15).gameObject.transform.GetChild(1).gameObject.SetActive(false);
                        NonVideoProfilePanel.transform.GetChild(0).GetChild(15).gameObject.transform.GetChild(2).gameObject.SetActive(true);
                    }
                    else
                        print(">>Status Data is incorrect...");
                }
                else
                {
                    //user is not a friend...
                    NonVideoProfilePanel.transform.GetChild(0).GetChild(15).gameObject.GetComponent<Button>().interactable = true;
                    NonVideoProfilePanel.transform.GetChild(0).GetChild(15).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    NonVideoProfilePanel.transform.GetChild(0).GetChild(15).gameObject.transform.GetChild(1).gameObject.SetActive(false);
                    NonVideoProfilePanel.transform.GetChild(0).GetChild(15).gameObject.transform.GetChild(2).gameObject.SetActive(false);
                }
                NonVideoProfilePanel.transform.GetChild(0).GetChild(15).gameObject.SetActive(true);
            }
            else
            { }

        }
        else
        { }

    }


    public void OnClickAddFriendBtn()
    {
        NonVideoProfilePanel.transform.GetChild(0).GetChild(15).gameObject.GetComponent<Button>().interactable = false;
        NonVideoProfilePanel.transform.GetChild(0).GetChild(15).gameObject.transform.GetChild(0).gameObject.SetActive(false);
        NonVideoProfilePanel.transform.GetChild(0).GetChild(15).gameObject.transform.GetChild(1).gameObject.SetActive(true);

        string senderName = "";
        string senderId = "";

        for (int i = 0; i < GameManagerScript.instance.playersParent.transform.childCount; i++)
        {
            if (GameManagerScript.instance.playersParent.transform.GetChild(i).childCount == 2)
            {
                if (GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetComponent<PokerPlayerController>().isLocalPlayer)
                {
                    senderName = GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetComponent<PokerPlayerController>().player.playerName;
                    senderId = "" + GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetComponent<PokerPlayerController>().player.clientId;
                }
            }
        }

        if (GameManagerScript.instance.isTournament)
            TournamentManagerScript.instance.AddFriendOnTable(localProfilePanel.transform.parent.GetChild(0).GetComponent<PokerPlayerController>().player.playerName, "" + localProfilePanel.transform.parent.GetChild(0).GetComponent<PokerPlayerController>().player.clientId, senderName, senderId);
        else
            PokerNetworkManager.instance.AddFriendOnTable(localProfilePanel.transform.parent.GetChild(0).GetComponent<PokerPlayerController>().player.playerName, "" + localProfilePanel.transform.parent.GetChild(0).GetComponent<PokerPlayerController>().player.clientId, senderName, senderId);



    }


    #endregion

    public void FlagImageProcess(Sprite sprite)
    {
        loadingPanel.SetActive(false);

        if (sprite != null)
        {
            NonVideoProfilePanel.transform.GetChild(0).GetChild(4).GetChild(0).GetComponent<Image>().sprite = sprite;
        }
    }

    public void PlayerImageProcess(Sprite sprite)
    {
        loadingPanel.SetActive(false);
        print("ASHISHImage");
        if (sprite != null)
        {
            NonVideoProfilePanel.transform.GetChild(0).GetChild(3).GetComponent<Image>().sprite = sprite;
            print("ASHISHImage2");
        }
    }

    public void ResetOnReFocus(bool isVideoTableMinimize)
    {
        print("ResetOnReFocus Started");
        Table.instance.cardOpened = 0;
        GameManagerScript.instance.totalPlayersSitting = 0;
        WinningLogic.instance.isRoundPotReset = true;
        CardShuffleAnimation.instance.totalPlayersCommonUI.Clear();
        HideCommands();
        newWinnerCards.SetActive(false);
        if (GameManagerScript.instance.HandRankPanel != null)
        {
            GameManagerScript.instance.HandRankPanel.transform.GetChild(0).GetComponent<Text>().text = "";
        }
        tableStartButton.gameObject.SetActive(false);
        postBlind.gameObject.SetActive(false);
        winPanel.SetActive(false);
        Table.instance.ResetSidePot();
        GameManagerScript.instance.localSeatIDDummy = 0;
        GameManagerScript.instance.localSeatID = 0;
        GameManagerScript.instance.addPlayers.Clear();

        if (GameManagerScript.instance.isVideoTable)
        {
            if (isVideoTableMinimize)
            {
                VideoPanelRemove();
            }
            PlayersGenerator.instance.videoPanelPlayers.Clear();
        }
        //PlayersGenerator.instance.videoPanelPlayers.Clear();
        try
        {
            ResetCardsPosition();
            GameManagerScript.instance.ResetTableOnExit(true);
        }

        catch
        {
            print("Timer already Stopped");
            if (!GameManagerScript.instance.isDirectLogIn)
            {
                ClubManagement.instance.loadingPanel.SetActive(false);
            }
        }

        if (!GameManagerScript.instance.isObserver && GameManagerScript.instance.isTournament)
        {
            GameManagerScript.instance.TableSetEmptytrue();

        }
        UIManagerScript.instance.breakTimePanel.SetActive(false);
        print("ResetOnReFocus ENDED");
    }

    public void VideoPanelRemove()
    {
        GameManagerScript.instance.hostPlayerVideoPanel.transform.SetParent(videoPanelParent.transform);
        for (int i = 0; i < PlayersGenerator.instance.videoPanelPlayers.Count; i++)
        {
            PlayersGenerator.instance.videoPanelPlayers.Remove(PlayersGenerator.instance.videoPanelPlayers[i]);
        }

        for (int i = 0; i < PlayersGenerator.instance.videoPanelsForAllClient.Count; i++)
        {
            PlayersGenerator.instance.videoPanelsForAllClient[i].SetParent(videoPanelParent.transform);
        }

    }
    //............................................................................................................................//

    public void OnClickMicrophone(GameObject gameObject)
    {
        if (gameObject.transform.parent.parent.parent.parent.GetChild(0).GetComponent<PokerPlayerController>().isLocalPlayer)
        {
            if (gameObject.transform.GetChild(0).gameObject.activeInHierarchy)
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
                gameObject.transform.GetChild(1).gameObject.SetActive(true);
                AgoraInit.instance.MuteUnmuteLocalPlayer(true);
            }
            else
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                gameObject.transform.GetChild(1).gameObject.SetActive(false);
                AgoraInit.instance.MuteUnmuteLocalPlayer(false);
            }
        }
        //............................Video Mute functionality...........................//
        else
        {
            if (gameObject.transform.GetChild(0).gameObject.activeInHierarchy)
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
                gameObject.transform.GetChild(1).gameObject.SetActive(true);
                AgoraInit.instance.TurnUserVOllOnOff((uint)gameObject.transform.parent.parent.parent.parent.GetChild(0).GetComponent<PokerPlayerController>().player.clientId, 0);
            }
            else
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                gameObject.transform.GetChild(1).gameObject.SetActive(false);
                AgoraInit.instance.TurnUserVOllOnOff((uint)gameObject.transform.parent.parent.parent.parent.GetChild(0).GetComponent<PokerPlayerController>().player.clientId, 100);
            }
        }
        //............................Video Mute functionality...........................//
    }

    //.....................................To TurnOff Take Seat Button after local player join..................................//
    public void OnPlayerJoin()
    {
        print("OnPlyerJoin...");
        StartCoroutine(OnPlayerJoinCoroutine());
    }

    IEnumerator OnPlayerJoinCoroutine()
    {
        yield return new WaitForSeconds(0.01f);

        for (int i = 0; i < GameManagerScript.instance.playersParent.transform.childCount; i++)
        {
            if (GameManagerScript.instance.playersParent.transform.GetChild(i).childCount > 1)
            {
                if (!GameManagerScript.instance.isVideoTable)
                {

                }
                else
                {
                    continue;
                }
            }
            else
            {
                if (GameManagerScript.instance.isVideoTable)
                {

                    GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetChild(20).gameObject.SetActive(false);
                }
                else
                {
                    GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(false);
                    GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetChild(16).gameObject.SetActive(true);
                }
            }
        }

        try
        {
            GameManagerScript.instance.StopCoroutineTimer();
        }
        catch (Exception e)
        {
            print("already stopped" + e);
        }
    }

    //.....................................To Show Commands to local player....................................................//
    public void ShowCommands()
    {
        if (PlayerPrefs.GetInt("SoundOffOn") == 0)
        {
            SoundManager.instance.PlayPomeSound(AudioClipCollection.instance.turnSFX);
        }
        footer.GetChild(0).GetChild(0).gameObject.SetActive(true);
        GameManagerScript.instance.isChanceActive = true;
    }

    //.....................................To Hide Commands to local player..................................................//
    public void HideCommands()
    {
        print("Command Hided........");
        GameManagerScript.instance.isChanceActive = false;
        GameManagerScript.instance.isCheckActive = false;
        footer.GetChild(0).GetChild(0).gameObject.SetActive(false);
        footer.GetChild(0).GetChild(2).gameObject.SetActive(false);
        footer.GetChild(0).GetChild(4).gameObject.SetActive(false);
        for (int i = 0; i < footer.GetChild(0).GetChild(0).childCount; i++)
        {
            footer.GetChild(0).GetChild(0).GetChild(i).gameObject.SetActive(false);
        }
        if (alarmClock != null)
        {
            alarmClock.SetActive(false);
            SoundManager.instance.alarmClocksound.volume = 0;
            SoundManager.instance.alarmClocksound.Stop();
        }
    }

    //.....................................To Reset Comunity Cards Position..................................................//
    public void ResetCardsPosition()
    {
        for (int i = 0; i < 5; i++)
        {
            if (tableParent.GetChild(5).GetChild(i).childCount >= 1)
            {
                for (int j = 0; j < tableParent.GetChild(5).GetChild(i).childCount; j++)
                {
                    tableParent.GetChild(5).GetChild(i).GetChild(j).gameObject.SetActive(true);
                    tableParent.GetChild(5).GetChild(i).GetChild(j).localScale = new Vector2(1, 1);
                    tableParent.GetChild(5).GetChild(i).GetChild(j).SetParent(cards);
                }

            }
        }

        try
        {

            Table.instance.card1.transform.SetParent(cards);
            Table.instance.card2.transform.SetParent(cards);
            Table.instance.card3.transform.SetParent(cards);
            Table.instance.card4.transform.SetParent(cards);
            Table.instance.card5.transform.SetParent(cards);

            Table.instance.card1.transform.localPosition = Vector3.zero;
            Table.instance.card2.transform.localPosition = Vector3.zero;
            Table.instance.card3.transform.localPosition = Vector3.zero;
            Table.instance.card4.transform.localPosition = Vector3.zero;
            Table.instance.card5.transform.localPosition = Vector3.zero;

            Table.instance.card1.transform.localEulerAngles = Vector3.zero;
            Table.instance.card2.transform.localEulerAngles = Vector3.zero;
            Table.instance.card3.transform.localEulerAngles = Vector3.zero;
            Table.instance.card4.transform.localEulerAngles = Vector3.zero;
            Table.instance.card5.transform.localEulerAngles = Vector3.zero;

            Table.instance.card1.transform.localScale = Vector3.zero;
            Table.instance.card2.transform.localScale = Vector3.zero;
            Table.instance.card3.transform.localScale = Vector3.zero;
            Table.instance.card4.transform.localScale = Vector3.zero;
            Table.instance.card5.transform.localScale = Vector3.zero;

        }
        catch (Exception e)
        {
            print("Not Assigned" + e);
        }

        Table.instance.card1 = null;
        Table.instance.card2 = null;
        Table.instance.card3 = null;
        Table.instance.card4 = null;
        Table.instance.card5 = null;

        for (int i = 0; i < cards.childCount; i++)
        {
            cards.GetChild(i).transform.localPosition = Vector3.zero;
            cards.GetChild(i).transform.localEulerAngles = Vector3.zero;
            cards.GetChild(i).transform.localScale = Vector3.one;
            cards.GetChild(i).GetChild(1).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        }

        Table.instance.TotalPot.text = "";

        if (!GameManagerScript.instance.isVideoTable)
        {
            Table.instance.TotalPot.text = Table.instance.TotalPot.text.Replace("Pot: ", string.Empty);
        }

        Table.instance.RoundPot.text = "";
        Table.instance.previousPotAmount = 0;
        Table.instance.table.sidePots = new int[0];
    }

    //................................ It activates raise panel .....................//
    public void RaisePanelActive(bool active, int index)
    {
        if (active)
        {
            footer.GetChild(0).GetChild(0).gameObject.SetActive(false);
            if (index == 1)
            {
                footer.GetChild(0).GetChild(2).gameObject.SetActive(true);
            }
            if (index == 2)
            {
                footer.GetChild(0).GetChild(4).gameObject.SetActive(true);
            }

            closeRaisePanel.gameObject.SetActive(true);

        }
        else
        {

            footer.GetChild(0).GetChild(0).gameObject.SetActive(true);
            if (index == 1)
            {
                footer.GetChild(0).GetChild(2).gameObject.SetActive(false);
            }
            if (index == 2)
            {
                footer.GetChild(0).GetChild(4).gameObject.SetActive(false);
            }

            closeRaisePanel.gameObject.SetActive(false);
        }
    }

    public float raiseSliderValue;
    public float betSliderValue;

    public void RaisePlusMinusButton(bool istrue)
    {
        if (GameManagerScript.instance.bigBlindAmount != 0)
        {
            if (istrue)
            {
                if (raisePanel.gameObject.activeInHierarchy)
                {
                    //raiseSlider.GetComponent<Slider>().value++;
                    float num1 = raiseSlider.GetComponent<Slider>().value + (GameManagerScript.instance.bigBlindAmount / 10);
                    raiseSlider.GetComponent<Slider>().value = num1;
                    raiseSliderValue = num1;
                }

                if (betPanel.gameObject.activeInHierarchy)
                {
                    //betSlider.GetComponent<Slider>().value++;
                    float num1Bet = betSlider.GetComponent<Slider>().value + (GameManagerScript.instance.bigBlindAmount / 10);
                    betSlider.GetComponent<Slider>().value = num1Bet;
                    betSliderValue = num1Bet;
                }
            }
            else
            {
                if (raisePanel.gameObject.activeInHierarchy)
                {
                    //raiseSlider.GetComponent<Slider>().value--;
                    float num1 = raiseSlider.GetComponent<Slider>().value - (GameManagerScript.instance.bigBlindAmount / 10);
                    raiseSlider.GetComponent<Slider>().value = num1;
                    raiseSliderValue = num1;
                }

                if (betPanel.gameObject.activeInHierarchy)
                {
                    //betSlider.GetComponent<Slider>().value--;
                    float num1 = betSlider.GetComponent<Slider>().value - (GameManagerScript.instance.bigBlindAmount / 10);
                    betSlider.GetComponent<Slider>().value = num1;
                    betSliderValue = num1;
                }
            }
        }
    }

    public void NewSliderValue(int betOrRaise)
    {
        print("Value Changed................");

        if (betOrRaise == 1 && GameManagerScript.instance.bigBlindAmount != 0 && raiseSlider.GetComponent<Slider>().value >= GameManagerScript.instance.bigBlindAmount && raiseSlider.GetComponent<Slider>().value <= raiseSlider.GetComponent<Slider>().maxValue)                                 // For Raise Slider => betOrRaise == 1
        {
            if (raiseSlider.GetComponent<Slider>().value >= (raiseSlider.GetComponent<Slider>().maxValue - 4))
            {
                raiseSlider.GetComponent<Slider>().value = raiseSlider.GetComponent<Slider>().maxValue;
            }
            else
            {
                float num = raiseSlider.GetComponent<Slider>().value;
                int num2 = (int)num / (GameManagerScript.instance.bigBlindAmount / 10);
                num2 *= (GameManagerScript.instance.bigBlindAmount / 10);
                raiseSlider.GetComponent<Slider>().value = num2;
            }

        }

        if (betOrRaise == 2 && GameManagerScript.instance.bigBlindAmount != 0 && betSlider.GetComponent<Slider>().value >= GameManagerScript.instance.bigBlindAmount && betSlider.GetComponent<Slider>().value <= betSlider.GetComponent<Slider>().maxValue)                                   // For Bet Slider => betOrRaise == 2
        {
            if (betSlider.GetComponent<Slider>().value >= (betSlider.GetComponent<Slider>().maxValue - 4))
            {
                betSlider.GetComponent<Slider>().value = betSlider.GetComponent<Slider>().maxValue;
            }
            else
            {
                float numBet = betSlider.GetComponent<Slider>().value;
                int num2Bet = (int)numBet / (GameManagerScript.instance.bigBlindAmount / 10);
                num2Bet *= (GameManagerScript.instance.bigBlindAmount / 10);
                betSlider.GetComponent<Slider>().value = num2Bet;
            }
        }
    }

    #region CLUB VERSION CODE
    public void BuyInFunctionality()
    {
        topUpPanelSocialPoker.gameObject.SetActive(true);

        maxBuy.text = "" + GameSerializeClassesCollection.instance.chipBalance.maxBuyIn;
        minBuy.text = "" + GameSerializeClassesCollection.instance.chipBalance.minBuyIn;

        buyInSlider.GetComponent<Slider>().maxValue = GameSerializeClassesCollection.instance.chipBalance.maxBuyIn;
        buyInSlider2.GetComponent<Slider>().maxValue = GameSerializeClassesCollection.instance.chipBalance.maxBuyIn;
        buyInSlider.GetComponent<Slider>().minValue = GameSerializeClassesCollection.instance.chipBalance.minBuyIn;
        buyInSlider2.GetComponent<Slider>().minValue = GameSerializeClassesCollection.instance.chipBalance.minBuyIn;

        topUpPanelSocialPoker.GetChild(0).GetChild(3).GetComponent<Text>().text = "" + GameSerializeClassesCollection.instance.chipBalance.chipBalance;
        buyInPanel2.GetChild(0).GetChild(3).GetComponent<Text>().text = "" + GameSerializeClassesCollection.instance.chipBalance.chipBalance;

        buyInSlider.GetComponent<Slider>().value = GameSerializeClassesCollection.instance.chipBalance.minBuyIn;
        buyInSlider2.GetComponent<Slider>().value = GameSerializeClassesCollection.instance.chipBalance.minBuyIn;
        buyInSlider.GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = "" + buyInSlider.GetComponent<Slider>().value;
        buyInSlider2Percent.GetComponent<Slider>().value = 40f;
    }

    //......................................On-Click Buy-IN.....................................................................//
    public void TableJoin()
    {
        if (buyInSlider.GetComponent<Slider>().value >= GameSerializeClassesCollection.instance.chipBalance.minBuyIn)
        {
            if (topUpPanelSocialPoker.GetChild(0).GetChild(5).GetChild(0).GetComponent<Toggle>().isOn || buyInPanel2.GetChild(0).GetChild(5).GetChild(0).GetComponent<Toggle>().isOn)
            {
                if (topUpPanelSocialPoker.gameObject.activeInHierarchy)
                {
                    PokerNetworkManager.instance.SitOnTable(buyInSlider.GetComponent<Slider>().value, true, buyInSlider2Percent.GetComponent<Slider>().value);
                }
                else
                {
                    PokerNetworkManager.instance.SitOnTable(buyInSlider2.GetComponent<Slider>().value, true, buyInSlider2Percent.GetComponent<Slider>().value);
                }
            }
            else
            {
                PokerNetworkManager.instance.SitOnTable(buyInSlider.GetComponent<Slider>().value, false, 0);
            }
            topUpPanelSocialPoker.gameObject.SetActive(false);
            buyInPanel2.gameObject.SetActive(false);
        }

        else
        {
            lowChipPanel.gameObject.SetActive(true);
        }
    }

    public void TopUpFunctionality()
    {
        topUpPanel.gameObject.SetActive(true);
        maxBuyTopUp.text = "" + GameSerializeClassesCollection.instance.topUpChipBalance.maxBuyIn;
        minBuyTopUp.text = "" + GameSerializeClassesCollection.instance.topUpChipBalance.minBuyIn;

        topUpSlider.GetComponent<Slider>().maxValue = GameSerializeClassesCollection.instance.topUpChipBalance.maxBuyIn;
        topUpSlider.GetComponent<Slider>().minValue = GameSerializeClassesCollection.instance.topUpChipBalance.minBuyIn;

        topUpPanel.GetChild(0).GetChild(3).GetChild(0).GetComponent<Text>().text = "" + GameSerializeClassesCollection.instance.topUpChipBalance.chipBalance;

        topUpSlider.GetComponent<Slider>().value = GameSerializeClassesCollection.instance.topUpChipBalance.minBuyIn;
        topUpSlider.GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = "" + topUpSlider.GetComponent<Slider>().value;
    }

    //......................................On-Click Top-Up.....................................................................//

    public bool isTopupBtnClicked;

    public void TopUP()
    {
        isTopupBtnClicked = true;
        print("isTopupBtnClicked: " + isTopupBtnClicked);
        PokerNetworkManager.instance.isZeroChips = false;
        PokerNetworkManager.instance.TopUpEmitter();
        topUpPanelSocialPoker.gameObject.SetActive(false);
    }

    public void PostBlind()
    {
        PokerNetworkManager.instance.SendPostBlind();
        postBlind.SetActive(false);
    }

    public void OnclickDisbandTable()
    {
        if (disbandToggle1.isOn)
        {
            disbandPanel.gameObject.SetActive(false);
            PokerNetworkManager.instance.DisbandTableEmitter(true, false);
        }
        else if (disbandToggle2.isOn)
        {
            disbandPanel.gameObject.SetActive(false);
            PokerNetworkManager.instance.DisbandTableEmitter(false, true);
        }
        else
        {
            disbandPanel.gameObject.SetActive(false);
        }
    }

    //...............................................Mississipie Straddle...........................................................//
    public void StraddleSliderValue()
    {
        if (straddlePanel.transform.GetChild(4).GetComponent<Slider>().value == 2)
        {
            straddlePanel.transform.GetChild(4).GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = "2x";
        }
        else if (straddlePanel.transform.GetChild(4).GetComponent<Slider>().value == 3)
        {
            straddlePanel.transform.GetChild(4).GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = "3x";
        }
        else if (straddlePanel.transform.GetChild(4).GetComponent<Slider>().value == 4)
        {
            straddlePanel.transform.GetChild(4).GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = "4x";
        }
        else if (straddlePanel.transform.GetChild(4).GetComponent<Slider>().value == 5)
        {
            straddlePanel.transform.GetChild(4).GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = "5x";
        }
    }

    public void InnerStraddleButton()
    {
        if (straddlePanel.transform.GetChild(2).GetChild(1).GetChild(0).gameObject.activeInHierarchy)
        {
            straddlePanel.transform.GetChild(2).GetChild(1).GetChild(0).gameObject.SetActive(false);
            straddlePanel.transform.GetChild(2).GetChild(1).GetChild(1).gameObject.SetActive(true);
        }

        else
        {
            straddlePanel.transform.GetChild(2).GetChild(1).GetChild(0).gameObject.SetActive(true);
            straddlePanel.transform.GetChild(2).GetChild(1).GetChild(1).gameObject.SetActive(false);
        }
    }

    internal int num;
    public void ClickOnStraddleSymbol()
    {
        straddlePanelSymbol.transform.GetChild(1).gameObject.SetActive(true);
        straddlePanelSymbol.transform.GetChild(0).gameObject.SetActive(false);
        if (straddlePanel.transform.GetChild(2).GetChild(1).GetChild(0).gameObject.activeInHierarchy)
        {
            num = 0;
        }
        else
        {
            num = 1;
        }

    }

    public void ClickOnCancelOrClose()
    {
        if (num == 0)
        {
            straddlePanel.transform.GetChild(2).GetChild(1).GetChild(0).gameObject.SetActive(true);
            straddlePanel.transform.GetChild(2).GetChild(1).GetChild(1).gameObject.SetActive(false);
            straddlePanelSymbol.transform.GetChild(1).gameObject.SetActive(false);
            straddlePanelSymbol.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            straddlePanel.transform.GetChild(2).GetChild(1).GetChild(0).gameObject.SetActive(false);
            straddlePanel.transform.GetChild(2).GetChild(1).GetChild(1).gameObject.SetActive(true);
            straddlePanelSymbol.transform.GetChild(1).gameObject.SetActive(true);
            straddlePanelSymbol.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void ResetStraddleOnExit()
    {
        SocialProfile._instance.SocialChips();
        straddlePanelSymbol.transform.GetChild(1).gameObject.SetActive(false);
        straddlePanelSymbol.transform.GetChild(0).gameObject.SetActive(true);

        straddlePanel.transform.GetChild(2).GetChild(1).GetChild(0).gameObject.SetActive(true);
        straddlePanel.transform.GetChild(2).GetChild(1).GetChild(1).gameObject.SetActive(false);
    }
    //..............................................................................................................................//

    #endregion


    public void JoinWaitList()
    {
        PokerNetworkManager.instance.JoinWaitListEmitter();
        joinWaitList.SetActive(false);
    }

    public void ClickOnVideoReport(GameObject gameObject)                   //Edited by Mohit
    {
        gameObject.transform.parent.parent.gameObject.SetActive(false);

        gameObject.transform.parent.parent.gameObject.transform.localRotation = Quaternion.Euler(0, 90, 0);
        ReportAbuseScript.instance.dataReady = false;

        try
        {

            ReportAbuseScript.instance.TakeScreenShot();
        }
        catch (Exception e)
        {
            Debug.Log("Can't take screenshot.Exception Occurs : " + e);
        }

        ClubManagement.instance.loadingPanel.SetActive(true);
        if (GameManagerScript.instance.isTournament)
        {
            ReportAbuseScript.instance.detail.table_type = "tournament";
        }
        else
        {
            ReportAbuseScript.instance.detail.table_type = "video";

        }
        ReportAbuseScript.instance.jsonToServer = JsonUtility.ToJson(ReportAbuseScript.instance.detail);
        Communication.instance.PostData(ReportAbuseScript.instance.reportAbuseURL, ReportAbuseScript.instance.jsonToServer, ReportAbuseScript.instance.VideoReportResponse); ;


        PokerPlayerController playerInfo = gameObject.transform.parent.parent.parent.parent.GetChild(0).GetComponent<PokerPlayerController>();
        ReportAbuseScript.instance.playerInfoRef = playerInfo;
        string reportID = AccessGallery.instance.profileId[0].text;


        ReportAbuseScript.instance.FillReportData(reportID, GameManagerScript.instance.localPlayerSeatid.ToString(), playerInfo.player.clientId.ToString(), playerInfo.player.seatId.ToString(), GameManagerScript.instance.isTournament, SocialTournamentScript.instance.tournament_ID, GameSerializeClassesCollection.instance.observeTable.ticket);
    }

    public void AnimateDealer(Transform end, GameObject realDealer)
    {
        StartCoroutine(AnimateDealerCo(end, realDealer));
    }

    IEnumerator AnimateDealerCo(Transform end, GameObject realDealer)
    {
        newdealer.gameObject.SetActive(true);
        AnimateTransformFunctions.ins.AnimateTransform(newdealer, newdealer.position, end.position, 0.3f, AnimateTransformFunctions.TransformTypes.Position, AnimateTransformFunctions.AnimAxis.World, "EaseOut");
        yield return new WaitForSeconds(0.4f);
        realDealer.SetActive(true);
        //WinChipAnim.instance.dealer.gameObject.SetActive(false);
    }

    public void ClickOnStandUp()
    {
        PokerNetworkManager.instance.StandUpEmitter();
        lowChipPanel.gameObject.SetActive(true);
        lowChipPanelText.text = LanguageManager.Instance.GetTextValue("You will be stand up");//"You will be stand up in next hand";

        GameManagerScript.instance.isStandupClicked = true;
    }

    public void ActivateSitIn()
    {
        StartCoroutine(ActivateSitInCo());
    }

    IEnumerator ActivateSitInCo()
    {
        yield return new WaitForSeconds(0.6f);
        GameManagerScript.instance.isObserver = true;
        observingUI.SetActive(true);
        GameManagerScript.instance.isStandUp = true;

        for (int i = 0; i < GameManagerScript.instance.playersParent.transform.childCount; i++)
        {
            if (GameManagerScript.instance.playersParent.transform.GetChild(i).childCount == 1 && GameManagerScript.instance.playersParent.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                print(">> Sit INNNNNNNN..2");

                if (GameManagerScript.instance.isVideoTable)
                {
                    GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetChild(20).gameObject.SetActive(true);
                }
                else
                {
                    GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(true);
                    GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetChild(16).gameObject.SetActive(false);
                }
            }
        }
    }

    public void SitInButtonOnClicked(int seatId)
    {
        if(GameManagerScript.instance.isHandEnd)
        {
            lowChipPanel.gameObject.SetActive(true);
            lowChipPanelText.text = "Sit in not allowed!! Click after winner declaration ended..";
            //lowChipPanelText.text = LanguageManager.Instance.GetTextValue("Sit in  not allowed!! Click after winner declaration ended..");
            return;
        }

        PokerNetworkManager.instance.SitInEmitter(seatId);
        TurnOffSitIn();
        if (GameManagerScript.instance.isVideoTable)
        {
            for (int i = 0; i < PlayersGenerator.instance.videoPanelsForAllClient.Count; i++)
            {
                GameObject objTrans = PlayersGenerator.instance.videoPanelsForAllClient[i].gameObject;
                PlayersGenerator.instance.videoPanelsForAllClient.Remove(PlayersGenerator.instance.videoPanelsForAllClient[i]);
                Destroy(objTrans);
            }
        }

        lowChipPanel.gameObject.SetActive(true);
        lowChipPanelText.text = LanguageManager.Instance.GetTextValue("You will be seated");// "You will be seated in next hand";
    }

    public void TurnOffSitIn()
    {
        for (int i = 0; i < GameManagerScript.instance.playersParent.transform.childCount; i++)
        {
            if (GameManagerScript.instance.playersParent.transform.GetChild(i).childCount == 1 && GameManagerScript.instance.playersParent.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                print(">> Sit INNNNNNNN..2");

                if (GameManagerScript.instance.isVideoTable)
                {
                    GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetChild(20).gameObject.SetActive(false);
                }
                else
                {
                    GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(false);
                    //GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetChild(16).gameObject.SetActive(true);
                }
            }
        }
    }

}
