using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllTableUI : MonoBehaviour
{
    [Header("Video Table UI")]
    public Transform tableParent;
    public Transform footer;
    public Transform cards;
    public Transform observingPanel;
    //public Transform observingPanelNonVideo;
    public Transform tableStartButton;
    //public Text LocalPlayerMessage;
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
    public Transform buyInPanel;
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
    public Transform popUpPanel;
    public Text popUpPanelText;
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
    public GameObject tournamentObservingUI;

    public GameObject topUpChipBtn;
    public GameObject switchTableBtn;
    public GameObject standUpBtn;
    public GameObject emptypChipsPanel;
    public GameObject emptypChipsLessBuyInPanel;
    public GameObject accountBalanceZeroPanel;

    public Text breakTimeText;
    public Transform tableInfo;
    public Transform waitOtherPlayerPanel;
    [Space]
    public List<GameObject> betAndRaiseHighlightImage;
    public List<GameObject> allOtherBottomPanelBtn;

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
    [Header("Table InnerPanels")]
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
    public Transform winPanelTournmentKO;
    public Transform winPanelTournment;
    public Transform gameLeftPanelTournment;
    public Transform gameLeftPanelSatelite;
    public Transform addOnRebuyButtonPanelTournment;
    [Space]
    public Transform closeRaisePanel;
    [Space]
    [Space]

    public GameObject VideoReportPanel;
    public GameObject VideoReportPanelTournament;
    //........................ GameManager Script......................//
    public GameObject playersParent;
    public GameObject observerContent;
    //.................................................................//

    //........................ Table Script...................................//
    public Text gameStartCounter;
    public Text tableNumber;
    public Text tableType;
    public Text blind;
    public Text TotalPot;
    public Text RoundPot;
    public Text Id;
    public GameObject sidePotParent;
    public GameObject stackParent; 
    //.................................................................//

    public Transform cardStackPosition;
    public Transform cardToAnimate;

    public GameObject newWinnerCards;
    public Transform newdealer;
    [Space]
    public Transform animCard1;
    public Transform animCard4;
    public Transform animCard5; 

    public Text runningTimeText, remainingTimeText; // Table Menu Time Text UI
    public Text tableName, tableBlinds; // Table Menu Time Text UI

    public Text totalPotWin;
    public Text mainPotWin;
    public GameObject stackParentWin;

    public GameObject sidePotParentWin;

    [Space]
    [Header("Tournament Info Panel Texts")]
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
    public Slider newBuyInSlider;
    public GameObject menu_panelUIButton;
    public GameObject standUpButton; 
    public GameObject friendBtn;

    public GameObject alarmClock;
    public Text alarmClockTime;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            quitPanel.SetActive(true);
        }
    }

    private void OnEnable()
    {
        AssignActiveTable();
        if (Table.instance.RoundPot != null)
        {
            Table.instance.RoundPot.transform.parent.gameObject.SetActive(false);
        }
        
    }

    void AssignActiveTable()
    {
        UIManagerScript.instance.menu_panelUIButton = menu_panelUIButton;
        UIManagerScript.instance.tableParent = tableParent;
        UIManagerScript.instance.footer = footer;
        UIManagerScript.instance.cards = cards;
        UIManagerScript.instance.observingPanel = observingPanel;
        UIManagerScript.instance.tableStartButton = tableStartButton;

        UIManagerScript.instance.raisePanel = raisePanel;
        UIManagerScript.instance.raiseSlider = raiseSlider;
        UIManagerScript.instance.betPanel = betPanel;
        UIManagerScript.instance.betSlider = betSlider;

        UIManagerScript.instance.topUpPanelSocialPoker = buyInPanel;
        UIManagerScript.instance.buyInSlider = buyInSlider;
        UIManagerScript.instance.minBuy = minBuy;
        UIManagerScript.instance.maxBuy = maxBuy;

        UIManagerScript.instance.buyInPanel2 = buyInPanel2;
        UIManagerScript.instance.buyInSlider2 = buyInSlider2;
        UIManagerScript.instance.minBuy2 = minBuy2;
        UIManagerScript.instance.maxBuy2 = maxBuy2;
        UIManagerScript.instance.buyInSlider2Percent = buyInSlider2Percent;

        UIManagerScript.instance.topUpPanel = topUpPanel;
        UIManagerScript.instance.topUpSlider = topUpSlider;
        UIManagerScript.instance.minBuyTopUp = minBuyTopUp;
        UIManagerScript.instance.maxBuyTopUp = maxBuyTopUp;

        UIManagerScript.instance.topUpChipBtn = topUpChipBtn;
        UIManagerScript.instance.switchTableBtn = switchTableBtn;
        UIManagerScript.instance.standUpBtn = standUpBtn;
        UIManagerScript.instance.emptypChipsPanel = emptypChipsPanel;
        UIManagerScript.instance.emptypChipsLessBuyInPanel = emptypChipsLessBuyInPanel;
        UIManagerScript.instance.accountBalanceZeroPanel = accountBalanceZeroPanel;

        UIManagerScript.instance.lowChipPanel = popUpPanel;
        UIManagerScript.instance.lowChipPanelText = popUpPanelText;

        UIManagerScript.instance.disbandPanel = disbandPanel;
        UIManagerScript.instance.disbandToggle1 = disbandToggle1;
        UIManagerScript.instance.disbandToggle2 = disbandToggle2;

        UIManagerScript.instance.winPanel = winPanel;
        UIManagerScript.instance.quitPanel = quitPanel;
        UIManagerScript.instance.tableMenuPanel = tableMenuPanel;

        UIManagerScript.instance.tableMenuPanel_regular = tableMenuPanel_regular;
        UIManagerScript.instance.tableMenuPanel_tournament = tableMenuPanel_tournament;


        UIManagerScript.instance.postBlind = postBlind;
        UIManagerScript.instance.joinWaitList = joinWaitList;
        UIManagerScript.instance.lowestLayerPanel = lowestLayerPanel;
        UIManagerScript.instance.straddlePanel = straddlePanel;
        UIManagerScript.instance.straddlePanelSymbol = straddlePanelSymbol;

        UIManagerScript.instance.tipPanel = tipPanel;
        UIManagerScript.instance.addOnPanel = addOnPanel;
        UIManagerScript.instance.rebuyPanel = rebuyPanel;
        UIManagerScript.instance.loadingPanel = loadingPanel;
        UIManagerScript.instance.mttSideInfoPanel = mttSideInfoPanel;
        UIManagerScript.instance.mttSideInfoPanelSymbol = mttSideInfoPanelSymbol;
        UIManagerScript.instance.tableInfo = tableInfo;
        UIManagerScript.instance.betAndRaiseHighlightImage = betAndRaiseHighlightImage;
        UIManagerScript.instance.allOtherBottomPanelBtn = allOtherBottomPanelBtn;

        UIManagerScript.instance.innerPanelRankListingPanel = innerPanelRankListingPanel;
        UIManagerScript.instance.innerPanelRankListingContent = innerPanelRankListingContent;
        UIManagerScript.instance.innerPanelTableListingPanel = innerPanelTableListingPanel;
        UIManagerScript.instance.innerPanelTableListingContent = innerPanelTableListingContent;
        UIManagerScript.instance.innerPanelPrizeListingPanel = innerPanelPrizeListingPanel;
        UIManagerScript.instance.innerPanelPrizeListingContent = innerPanelPrizeListingContent;
        UIManagerScript.instance.innerPanelBlindsListingPanel = innerPanelBlindsListingPanel;
        UIManagerScript.instance.innerPanelBlindsListingContent = innerPanelBlindsListingContent;
        UIManagerScript.instance.tournamentIncreaseTimer = tournamentIncreaseTimer;
        UIManagerScript.instance.levelTime = levelTime;
        UIManagerScript.instance.breakTimePanel = breakTimePanel;
        UIManagerScript.instance.tournamentInfoButton = tournamentInfoButton;
        UIManagerScript.instance.observingUI = tournamentObservingUI;
        UIManagerScript.instance.breakTimeText = breakTimeText;

        UIManagerScript.instance.winPanelTournmentKO = winPanelTournmentKO;
        UIManagerScript.instance.winPanelTournment = winPanelTournment;
        UIManagerScript.instance.gameLeftPanelTournment = gameLeftPanelTournment;
        UIManagerScript.instance.gameLeftPanelSatelite = gameLeftPanelSatelite;
        UIManagerScript.instance.addOnRebuyButtonPanelTournment = addOnRebuyButtonPanelTournment;
        UIManagerScript.instance.closeRaisePanel = closeRaisePanel;
        UIManagerScript.instance.waitOtherPlayerPanel = waitOtherPlayerPanel;
        UIManagerScript.instance.VideoReportPanel = VideoReportPanel;
        UIManagerScript.instance.VideoReportPanelTournament = VideoReportPanelTournament;

        UIManagerScript.instance.newWinnerCards = newWinnerCards;
        UIManagerScript.instance.newdealer = newdealer;

        UIManagerScript.instance.animCard1 = animCard1;
        UIManagerScript.instance.animCard4 = animCard4;
        UIManagerScript.instance.animCard5 = animCard5;

        UIManagerScript.instance.totalPotWin = totalPotWin;
        UIManagerScript.instance.mainPotWin = mainPotWin;
        UIManagerScript.instance.stackParentWin = stackParentWin;

        UIManagerScript.instance.tournamentName = tournamentName;
        UIManagerScript.instance.tournamentID = tournamentID;
        UIManagerScript.instance.position = position;
        UIManagerScript.instance.level = level;
        UIManagerScript.instance.remaining = remaining;
        UIManagerScript.instance.lateRegistration = lateRegistration;
        UIManagerScript.instance.prizePool = prizePool;
        UIManagerScript.instance.onGoing = onGoing;
        UIManagerScript.instance.avgStack = avgStack;
        UIManagerScript.instance.smallestStack = smallestStack;
        UIManagerScript.instance.rebuys = rebuys;
        UIManagerScript.instance.addOns = addOns;
        UIManagerScript.instance.largestStack = largestStack;
        UIManagerScript.instance.totalBuyIns = totalBuyIns;
        UIManagerScript.instance.nextLevelTimer = nextLevelTimer;
        UIManagerScript.instance.currentLevel = currentLevel;
        UIManagerScript.instance.nextLevel = nextLevel;
        UIManagerScript.instance.infoPanelNavigationTabList = infoPanelNavigationTabList;
        UIManagerScript.instance.infoPanelTabList = infoPanelTabList;
        UIManagerScript.instance.rankingPanel = rankingPanel;
        UIManagerScript.instance.rankingContent = rankingContent;
        UIManagerScript.instance.prizePanel = prizePanel;
        UIManagerScript.instance.prizeContent = prizeContent;
        UIManagerScript.instance.tablePanel = tablePanel;
        UIManagerScript.instance.tableContent = tableContent;
        UIManagerScript.instance.tablePage = tablePage;
        UIManagerScript.instance.blindsPanel = blindsPanel;
        UIManagerScript.instance.blindsContent = blindsContent;
        UIManagerScript.instance.newBuyInSlider = newBuyInSlider;

        UIManagerScript.instance.timerCountdown = timerCountdown;
        UIManagerScript.instance.timerCountdown2 = timerCountdown2;

        UIManagerScript.instance.standUpButton = standUpButton;
        UIManagerScript.instance.friendBtn = friendBtn;

        UIManagerScript.instance.alarmClock = alarmClock;
        UIManagerScript.instance.alarmClockTime = alarmClockTime;

        GameManagerScript.instance.playersParent = playersParent;
        GameManagerScript.instance.observerContent = observerContent;

        CardShuffleAnimation.instance.cardStackPosition = cardStackPosition;
        CardShuffleAnimation.instance.cardToAnimate = cardToAnimate;

        Table.instance.TotalPot = TotalPot;
        Table.instance.RoundPot = RoundPot;
        //Table.instance.LocalPlayerMessage = LocalPlayerMessage;

       
        
        Table.instance.gameStartCounter = gameStartCounter;
        Table.instance.tableNumber = tableNumber;
        Table.instance.tableType = tableType;
        Table.instance.blind = blind;
        Table.instance.Id = Id;
        Table.instance.sidePotParent = sidePotParent;
        Table.instance.stackParent = stackParent;
        Table.instance.sidePotParentWin = sidePotParentWin;



        if (!GameManagerScript.instance.isDirectLogIn)
        {
            try
            {
                TableTimer.instance.tableStartTimeString = ClubManagement.instance.currentSelectedTableStartTime.Substring(0, 5);
                TableTimer.instance.tableEndTimeString = ClubManagement.instance.currentSelectedTableEndTime;

                if (GameManagerScript.instance.isVideoTable)
                {

                    TableTimer.instance.tableName = TableTimer.instance.tableNameVideo;
                    TableTimer.instance.tableBlinds = TableTimer.instance.tableBlindsVideo;
                    TableTimer.instance.runningTimeText = TableTimer.instance.runningTimeTextVideo;
                    TableTimer.instance.remainingTimeText = TableTimer.instance.remainingTimeTextVideo;



                }
                else
                {
                    TableTimer.instance.tableName = TableTimer.instance.tableNameNonVideo;
                    TableTimer.instance.tableBlinds = TableTimer.instance.tableBlindsNonVideo;
                    TableTimer.instance.runningTimeText = TableTimer.instance.runningTimeTextNonVideo;
                    TableTimer.instance.remainingTimeText = TableTimer.instance.remainingTimeTextNonVideo;

                }

                TableTimer.instance.tableName.text = ClubManagement.instance.currentSelectedTableName;
                TableTimer.instance.tableBlinds.text = ClubManagement.instance.currentSelectedTableBlinds;
                if (ClubManagement.instance.currentSelectedTableType == "regular")
                {
                    TableTimer.instance.StartRunningTimeCoroutine();
                    TableTimer.instance.StartRemainingTimeCoroutine();
                }
            }
            catch
            {
                print("ERROR in Assigning TableTimer ..................");
            }
        }
    }

    //[Header("Video Table UI")]
    //public Transform activeTableParent;
    //public Transform activefooter;
    //public Transform activecards;
    //public Transform activeobservingPanel;
    //public Transform activeobservingPanelNonVideo;
    //public Transform activetableStartButton;
    //[Space]
    //[Header("Raise")]
    //public Transform activeraisePanel;
    //public Transform activeraiseSlider;
    //[Space]
    //[Header("Buy In Panel")]
    //public Transform activebuyInPanel;
    //public Transform activebuyInSlider;
    //public Text activeminBuy;
    //public Text activemaxBuy;
    //[Space]
    //[Header("Buy In Panel 2")]
    //public Transform activebuyInPanel2;
    //public Transform activebuyInSlider2;
    //public Text activeminBuy2;
    //public Text activemaxBuy2;
    //public Transform activebuyInSlider2Percent;
    ////public Text buyInSliderPerText;
    //[Space]
    //[Header("Top Up Panel")]
    //public Transform activetopUpPanel;
    //public Transform activetopUpSlider;
    //public Text activeminBuyTopUp;
    //public Text activemaxBuyTopUp;
    //[Space]
    //[Header("Pop Up Panel")]
    //public Transform activepopUpPanel;
    //public Text activepopUpPanelText;
    //[Space]
    //[Space]
    //[Header("Disband Panel")]
    //public Transform activedisbandPanel;
    //public Toggle activedisbandToggle1;
    //public Toggle activedisbandToggle2;
    //[Space]
    //public GameObject activewinPanel;
    //public GameObject activequitPanel;
    //public GameObject activetableMenuPanel;
    //public GameObject activeobserverContent;
    //public GameObject activepostBlind;
    //public GameObject activelowestLayerPanel; 


}
