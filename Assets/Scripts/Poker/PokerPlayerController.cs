using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;

public class PokerPlayerController : MonoBehaviour
{
    [Header("Player Property")]
    public PlayerOnRoundStart player;

    [Serializable]
    public class PlayerOnRoundStart
    {
        public string playerName;
        public string message;
        public int chips;
        public bool folded;
        public bool allIn;
        public string[] cards;
        public bool isSurvive;
        public int reloadCount;
        public int seatId;
        public int roundBet;
        public int clientId;
        public int bet;
        public bool isOnline;
        public bool isHuman;
        public string user_image;
    }

    [Header("Local Player Cards")]
    public GameObject card1;
    public GameObject card2;
    [Space]
    [Space]

    public Transform localTrans;
    public Color localPlayerThemeColor;
    public Image localPlayerTheme;
    public bool isLocalPlayer;
    public bool isBeforeLocalPlayer;
    public bool isAfterLocalPlayer;
    public int timesAnimation;

    [Header("Player's Common UI")]
    public Transform commonUi;
    [Space]
    public Transform chipTextParent;
    public Transform playerNameText;
    public Transform chipsText;
    public Transform chipTextNewUITemp; // temp variable for chips amount update
    [Space]
    public Transform betTextParent;
    public Transform betText;
    public Transform betchipTotalAmount;
    [Space]
    [Space]
    public Transform foldImage;
    public Transform callImage;
    public Transform raiseImage;
    public Transform betImage;
    public Transform checkImage;
    public Transform allInImage;
    [Space]
    public Transform cardImage;
    public Transform micImage;
    public Transform chair;
    //public Transform takeButton;
    public Transform takeSeat;
    public Transform dealerButton;
    public Transform dealerButtonFirstChild;
    public Transform timerImage;
    public Transform SB;
    public Transform BB;
    public Transform LocalCardsPanel;
    public Transform winnerText;
    public Transform profileImg;
    public Transform nonVideoEmptySeat;
    [Space]
    public Transform playerHoleCards;
    public Transform playerShowCards;
    public GameObject holecard1;
    public GameObject holecard2;
    public Transform koBounty;
    public Text koBountyText;
    public GameObject muteOn;
    public Transform playerExclude;

    private void OnEnable()
    {
        StartCoroutine(Initlize());
    }

    private void Update()
    {
        if (chipTextNewUITemp != null && commonUi != null)
        {
            chipTextNewUITemp.GetComponent<Text>().text = chipsText.GetComponent<Text>().text;
        }

    }

    // It Initlizes after spawning, reshuffling.
    IEnumerator Initlize()
    {
        GameManagerScript.instance.isPlayerGenerating = true;
        yield return new WaitForSeconds(0.1f);
        transform.SetAsFirstSibling();
        transform.localScale = new Vector3(0, 0, 0);

        if (GameManagerScript.instance.isVideoTable)
        {
            transform.GetChild(1).gameObject.SetActive(true);
            commonUi = transform.parent.gameObject.transform.GetChild(1);
            chair = commonUi.GetChild(0);
            takeSeat = commonUi.GetChild(20);
            micImage = commonUi.GetChild(1);
            chipTextParent = commonUi.GetChild(3);
            chipsText = commonUi.GetChild(3).GetChild(0);
            chipTextNewUITemp = commonUi.GetChild(3).GetChild(4).GetChild(0).GetChild(1);
            playerNameText = commonUi.GetChild(3).GetChild(2);
            callImage = commonUi.GetChild(4);
            raiseImage = commonUi.GetChild(5);
            betImage = commonUi.GetChild(6);
            checkImage = commonUi.GetChild(7);
            allInImage = commonUi.GetChild(8);
            cardImage = commonUi.GetChild(17);
            SB = commonUi.GetChild(10);
            BB = commonUi.GetChild(11);
            playerHoleCards = commonUi.GetChild(12);
            betTextParent = commonUi.GetChild(13);
            betText = commonUi.GetChild(13).GetChild(0).GetChild(0);
            betchipTotalAmount = commonUi.GetChild(13).GetChild(0).GetChild(3);
            foldImage = commonUi.GetChild(14);

            timerImage = commonUi.GetChild(9);
            dealerButton = commonUi.GetChild(18);
            profileImg = commonUi.GetChild(15);
            if (commonUi.GetChild(18).childCount > 0)
            {
                dealerButtonFirstChild = commonUi.GetChild(18).GetChild(0);
            }
            else
            {
                dealerButtonFirstChild = GameManagerScript.instance.dealer;
            }
            koBounty = commonUi.GetChild(21);
            koBountyText = koBounty.transform.GetChild(0).GetChild(0).GetComponent<Text>();
            muteOn = commonUi.GetChild(22).gameObject;
            playerExclude = commonUi.GetChild(23);

            LocalCardsPanel = commonUi.GetChild(19);
            //takeButton.gameObject.SetActive(false);
            takeSeat.gameObject.SetActive(false);

            playerNameText.transform.GetComponent<Text>().text = "" + player.playerName;
            chipsText.transform.GetComponent<Text>().text = string.Format("{0:n0}", player.chips); //"" + player.chips;
            betText.transform.GetComponent<Text>().text = "" + player.bet;
            betchipTotalAmount.transform.GetComponent<Text>().text = "" + player.bet;

            localTrans.localPosition = Vector3.zero;
            if (timesAnimation == GameManagerScript.instance.timesAnimLocalPlayer && isLocalPlayer)
            {
                ActivatePlayerText();

            }
            else if (timesAnimation == GameManagerScript.instance.timesAnimAfterLocalPlayer && isAfterLocalPlayer)
            {
                ActivatePlayerText();
            }
            else if (timesAnimation == GameManagerScript.instance.timesAnimBeforeLocalPlayer && isBeforeLocalPlayer)
            {
                ActivatePlayerText();
            }

            if (GameManagerScript.instance.chairAnimForLocalPlayer && isLocalPlayer && timesAnimation < GameManagerScript.instance.timesAnimLocalPlayer)
            {
                //print("chair.transform.parent.parent.name" + chair.transform.parent.parent.name);
                ChairAnimation.instance.StartAnimatePlayerChairCorutine(chair, transform, chair.transform.parent.parent.name);
                timesAnimation++;
            }
            if (GameManagerScript.instance.chairAnimForAfterPlayer && isAfterLocalPlayer && timesAnimation < GameManagerScript.instance.timesAnimAfterLocalPlayer)
            {
                //ChairAnimation.instance.StartAnimatePlayerChairCorutine(chair, transform/*, chair.transform.parent.parent.name*/);
                timesAnimation++;
            }
            if (GameManagerScript.instance.chairAnimForBeforeLocalPlayer && isBeforeLocalPlayer && timesAnimation < GameManagerScript.instance.timesAnimBeforeLocalPlayer)
            {
                //ChairAnimation.instance.StartAnimatePlayerChairCorutine(chair, transform/*, chair.transform.parent.parent.name*/);
                timesAnimation++;
            }

            if (GameManagerScript.instance.chairAnimForLocalPlayer /*|| GameManagerScript.instance.chairAnimForBeforeLocalPlayer || GameManagerScript.instance.chairAnimForAfterPlayer*/)
            {
                yield return new WaitForSeconds(2f);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
                localTrans.localPosition = Vector3.one;
            }

            chipTextParent.gameObject.SetActive(true);
            playerExclude.gameObject.SetActive(true);
            micImage.gameObject.SetActive(false);
            player.cards = new string[2];

            if (Table.instance.table.status != 0 && player.isSurvive)
            {
                cardImage.gameObject.SetActive(true);
                cardImage.GetChild(0).gameObject.SetActive(true);
            }

            if (isLocalPlayer)
            {
                ShowLocalPlayerCards();
                GameManagerScript.instance.isPlayerExcluded = true;
            }
            print("Video player Name --Initlize coroutine ends-- " + player.playerName);
        }
        else
        {
            transform.GetChild(1).gameObject.SetActive(false);
            commonUi = transform.parent.gameObject.transform.GetChild(1);
            takeSeat = commonUi.GetChild(0);
            betTextParent = commonUi.GetChild(2);
            betText = commonUi.GetChild(2).GetChild(0).GetChild(0);

            chipTextParent = commonUi.GetChild(8);
            chipsText = commonUi.GetChild(8).GetChild(0).GetChild(1);
            playerNameText = commonUi.GetChild(8).GetChild(0).GetChild(0);
            timerImage = commonUi.GetChild(8).GetChild(0).GetChild(3);

            callImage = commonUi.GetChild(3);
            raiseImage = commonUi.GetChild(4);
            betImage = commonUi.GetChild(5);
            checkImage = commonUi.GetChild(6);
            allInImage = commonUi.GetChild(7);
            cardImage = commonUi.GetChild(9);

            dealerButton = commonUi.GetChild(10);
            foldImage = commonUi.GetChild(11);
            SB = commonUi.GetChild(12);
            BB = commonUi.GetChild(13);

            playerHoleCards = commonUi.GetChild(14);
            playerShowCards = commonUi.GetChild(15);
            nonVideoEmptySeat = commonUi.GetChild(16);
            profileImg = commonUi.GetChild(1);
            koBounty = commonUi.GetChild(17);
            koBountyText = koBounty.transform.GetChild(1).GetComponent<Text>();
            playerExclude = commonUi.GetChild(19);

            takeSeat.gameObject.SetActive(false);
            timerImage.gameObject.SetActive(false);
            nonVideoEmptySeat.gameObject.SetActive(false);

            profileImg.gameObject.SetActive(true);
            chipTextParent.gameObject.SetActive(true);
            playerExclude.gameObject.SetActive(true);

            if (Table.instance.table.status != 0 && player.isSurvive)
            {
                cardImage.gameObject.SetActive(true);
                cardImage.GetChild(0).gameObject.SetActive(true);
            }

            playerNameText.transform.GetComponent<Text>().text = "" + player.playerName;
            chipsText.transform.GetComponent<Text>().text = string.Format("{0:n0}", player.chips); // "" + player.chips;
            betText.transform.GetComponent<Text>().text = "" + player.bet;
            player.cards = new string[2];
            if (isLocalPlayer)
            {
                ShowLocalPlayerCards();
                GameManagerScript.instance.isPlayerExcluded = true;
            }
            //else
            //{
            //    GetImage(player.user_image, PlayerImageCallback);             
            //}
            GetImage(player.user_image, PlayerImageCallback);             //Update
            print("Non video player Name --Initlize coroutine Ends-- " + player.playerName);
        }

        GameManagerScript.instance.isPlayerGenerating = false;

    }

    void PlayerImageCallback(Sprite resonse)
    {
        if (resonse != null)
        {
            profileImg.GetChild(1).GetChild(0).GetComponent<Image>().sprite = resonse;
        }
    }

    void ActivatePlayerText()
    {
        chair.gameObject.SetActive(false);
        localTrans.localScale = new Vector3(1, 1, 1);
        chipTextParent.gameObject.SetActive(true);
        //micImage.gameObject.SetActive(true);
        micImage.gameObject.SetActive(false);
    }

    // It Update player's values after action performing, new round, etc.
    public float betChipValueForVideoTable;

    public void UpdateValues()
    {
        if (GameManagerScript.instance.isVideoTable)
        {
            //betTextParent.gameObject.SetActive(true);
            float finalvalue = betChipValueForVideoTable + player.bet;
            //betchipTotalAmount.transform.GetComponent<Text>().text = "" + finalvalue;// + player.bet;  //

            string value1 = GameManagerScript.instance.KiloFormat(player.bet);
            betchipTotalAmount.transform.GetComponent<Text>().text = value1;
            //betchipTotalAmount.transform.GetComponent<Text>().text = "" + player.bet;


            print("betchipTotalAmount........." + betchipTotalAmount.GetComponent<Text>().text + " player.playerName " + player.playerName);
        }
        playerNameText.transform.GetComponent<Text>().text = "" + player.playerName;
        //chipsText.transform.GetComponent<Text>().text = "$" + string.Format("{0:n0}", player.chips);
        chipsText.transform.GetComponent<Text>().text = string.Format("{0:n0}", player.chips);
        string value2 = GameManagerScript.instance.KiloFormat(player.bet);

        if (player.bet != 0)
        {
            betText.transform.GetComponent<Text>().text = value2;
        }

        //betText.transform.GetComponent<Text>().text = "" + player.bet;

        if (TournamentGameDetail.instance.currentSelectedKOBounty)
        {
            koBounty.gameObject.SetActive(true);
            koBountyText.text = "" + TournamentGameDetail.instance.bounty;
            //if (GameManagerScript.instance.isVideoTable)
            //{
            //    koBounty.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "" + TournamentGameDetail.instance.bounty;
            //}
            //else
            //{
            //    koBounty.transform.GetChild(1).GetComponent<Text>().text = "" + TournamentGameDetail.instance.bounty;
            //}

        }
        if (isLocalPlayer)
        {
            cardImage.gameObject.SetActive(false);
            cardImage.GetChild(0).gameObject.SetActive(false);
            print("local player msg : " + player.message);
            ShowLocalPlayerCards();
        }
        if(player.isSurvive)
        {
            if (player.allIn == true)
            {
                ShowPlayerAction("allin");
            }
            else if (player.folded == true)
            {
                ShowPlayerAction("fold");
            }
        }


    }

    // It shows the action (command) any user performed. Calls after action performed listener
    public void ShowPlayerAction(string action)
    {
        if (action == "fold")
        {
            TurnOffActionUI(true);
            foldImage.gameObject.SetActive(true);
            allInImage.gameObject.SetActive(false);
            cardImage.gameObject.SetActive(false);
            cardImage.GetChild(0).gameObject.SetActive(false);
            playerHoleCards.gameObject.SetActive(false);
            if (!GameManagerScript.instance.isVideoTable)
            {
                playerHoleCards.gameObject.SetActive(false);
                playerShowCards.gameObject.SetActive(false);
            }
            if (PlayerPrefs.GetInt("SoundOffOn") == 0)
            {
                SoundManager.instance.PlayPomeSound(AudioClipCollection.instance.foldSFX);
            }
            print("Command Hided in show player action........");
            if (isLocalPlayer)
            {
                UIManagerScript.instance.HideCommands();
            }
        }
        if (action == "call")
        {
            TurnOffActionUI(false);
            callImage.gameObject.SetActive(true);
            betTextParent.gameObject.SetActive(true);
            if (PlayerPrefs.GetInt("SoundOffOn") == 0)
            {
                SoundManager.instance.PlayPomeSound(AudioClipCollection.instance.chipsDropSFX);
            }
        }
        if (action == "raise")
        {
            TurnOffActionUI(false);
            raiseImage.gameObject.SetActive(true);
            betTextParent.gameObject.SetActive(true);
            if (!GameManagerScript.instance.isVideoTable)
            {
                //raiseImage.GetChild(2).GetComponent<Text>().text = "Raise " + GameSerializeClassesCollection.instance.actionClass2.amount;

                string value = GameManagerScript.instance.KiloFormat(GameSerializeClassesCollection.instance.actionClass2.amount);
                raiseImage.GetChild(2).GetComponent<Text>().text = "RAISE " + value;

            }
            if (PlayerPrefs.GetInt("SoundOffOn") == 0)
            {
                SoundManager.instance.PlayPomeSound(AudioClipCollection.instance.chipsDropSFX);
            }
        }

        if (action == "bet")
        {
            TurnOffActionUI(false);
            betImage.gameObject.SetActive(true);
            betTextParent.gameObject.SetActive(true);
            if (PlayerPrefs.GetInt("SoundOffOn") == 0)
            {
                SoundManager.instance.PlayPomeSound(AudioClipCollection.instance.chipsDropSFX);
            }
        }

        if (action == "check")
        {
            TurnOffActionUI(false);
            checkImage.gameObject.SetActive(true);
            if (PlayerPrefs.GetInt("SoundOffOn") == 0)
            {
                SoundManager.instance.PlayPomeSound(AudioClipCollection.instance.checkSFX);
            }
        }

        if (action == "allin")
        {
            TurnOffActionUI(false);
            allInImage.gameObject.SetActive(true);

            if (player.bet != 0)
            {
                betTextParent.gameObject.SetActive(true);
            }

            SoundManager.instance.PlayPomeSound(AudioClipCollection.instance.chipsDropSFX);
        }
    }

    // Calls After each round i.e flop, river and so on
    public void TurnOffActionUI(bool checkForNextDealUpdation)
    {
        SB.gameObject.SetActive(false);
        BB.gameObject.SetActive(false);
        callImage.gameObject.SetActive(false);
        raiseImage.gameObject.SetActive(false);
        betImage.gameObject.SetActive(false);
        checkImage.gameObject.SetActive(false);
        if (checkForNextDealUpdation)
        {
            betTextParent.GetComponent<FunctionsContainer>().InvokeFunctions();           // FOR CHIP ANIM
            DeactiveBetChipsCoroutine();
        }
    }

    public void DeactiveBetChipsCoroutine()
    {
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine("DeactiveBetChips");
        }
    }

    public IEnumerator DeactiveBetChips()
    {
        if (betTextParent.gameObject.activeInHierarchy)
        {
            SoundManager.instance.PlayPomeSound(AudioClipCollection.instance.betChipDraggingSFX);
        }

        betText.transform.GetComponent<Text>().text = "";
        if (GameManagerScript.instance.isVideoTable)
        {
            yield return new WaitForSeconds(0.6f);
        }
        else
        {
            yield return new WaitForSeconds(0.8f);
        }

        betTextParent.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        Table.instance.UpdateTotalPot();
        betChipValueForVideoTable = 0;
        player.bet = 0;
        betText.transform.GetComponent<Text>().text = "" + player.bet;
        //UpdateValues();

    }

    // Checks if local player is current active  player and then activate commands otherwise hide commands. Calls After Next chance.
    public void ActiveCommands()
    {
        if (isLocalPlayer)
        {
            print("Commands active" + player.playerName);
            UIManagerScript.instance.ShowCommands();
        }
        else
        {
            print("Hide Commands" + player.playerName);
            UIManagerScript.instance.HideCommands();
        }
    }

    // It calls only in start of new hand once. Used to turn off various images at the start of each hand.
    public void TurnOnDealer(int id)
    {
        print("Turn On Dealer, SB, BB " + player.playerName);
        if (id == 1)
        {
            print("Its dealer " + player.playerName);

            if (!GameManagerScript.instance.isVideoTable)
            {
                for (int i = 0; i < GameManagerScript.instance.playersParent.transform.childCount; i++)
                {
                    if (GameManagerScript.instance.playersParent.transform.GetChild(i).childCount > 1)
                    {
                        GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetComponent<PokerPlayerController>().dealerButton.gameObject.SetActive(false);
                    }
                }
                UIManagerScript.instance.AnimateDealer(dealerButton, dealerButton.gameObject);
            }

            if (GameManagerScript.instance.isVideoTable)
            {
                for (int i = 0; i < UIManagerScript.instance.lowestLayerPanel.transform.childCount; i++)
                {
                    if (UIManagerScript.instance.lowestLayerPanel.transform.GetChild(i).name == "Delaer Image")
                    {
                        UIManagerScript.instance.lowestLayerPanel.transform.GetChild(i).GetComponent<SaveDealerParent>().dealerParent.gameObject.SetActive(false);
                        UIManagerScript.instance.lowestLayerPanel.transform.GetChild(i).SetParent(UIManagerScript.instance.lowestLayerPanel.transform.GetChild(i).GetComponent<SaveDealerParent>().dealerParent);
                    }
                }
                dealerButtonFirstChild.SetParent(UIManagerScript.instance.lowestLayerPanel.transform);
                GameManagerScript.instance.dealer = dealerButtonFirstChild;

                UIManagerScript.instance.AnimateDealer(dealerButton, dealerButtonFirstChild.gameObject);
                //dealerButton.gameObject.SetActive(true);
            }
        }

        else if (id == 2)
        {
            //if (GameManagerScript.instance.isVideoTable)
            //{
            SB.gameObject.SetActive(true);
            //}
            betTextParent.gameObject.SetActive(true);
        }

        else if (id == 3)
        {
            //if (GameManagerScript.instance.isVideoTable)
            //{
            BB.gameObject.SetActive(true);
            //}
            betTextParent.gameObject.SetActive(true);
        }
        else if (id == 4)
        {
            if (player.bet != 0)
            {
                print("Turn On PLayer Bet.." + player.playerName);
                //betchipTotalAmount.transform.GetComponent<Text>().text = "" + player.bet;

                string value = GameManagerScript.instance.KiloFormat(player.bet);
                betchipTotalAmount.transform.GetComponent<Text>().text = value;
                betTextParent.gameObject.SetActive(true);
            }
        }
    }

    // It calls only in end of new hand once. Used to turn off various images at the end of each hand.
    public void TurnOffDealer()
    {
        print("TurnOff Dealer and player card Starts..." + player.playerName);
        TurnOffWinnerUI();
        foldImage.gameObject.SetActive(false);
        allInImage.gameObject.SetActive(false);

        if (GameManagerScript.instance.isVideoTable)
        {
            dealerButton.gameObject.SetActive(false); dealerButtonFirstChild.SetParent(dealerButton); /*dealerButtonFirstChild.SetSiblingIndex(18);*/
            //........................//
            if (holecard1 != null && holecard2 != null)
            {
                holecard1.transform.SetParent(UIManagerScript.instance.cards);
                holecard2.transform.SetParent(UIManagerScript.instance.cards);

                holecard1.transform.localPosition = Vector3.zero;
                holecard2.transform.localPosition = Vector3.zero;
                holecard1.transform.localRotation = Quaternion.Euler(0, 0, 0);
                holecard2.transform.localRotation = Quaternion.Euler(0, 0, 0);
                holecard1.transform.localScale = Vector3.one;
                holecard2.transform.localScale = Vector3.one;

                holecard1.transform.GetChild(1).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                holecard2.transform.GetChild(1).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

                holecard1 = null;
                holecard2 = null;

                playerHoleCards.gameObject.SetActive(false);
                //print("OnHandComple playerHoleCards");
                if (winnerText != null)
                {
                    //winnerText.SetParent(playerHoleCards);
                    LocalCardsPanel.gameObject.SetActive(false);
                    winnerText.SetParent(LocalCardsPanel);
                    winnerText.localPosition = new Vector2(0, 70f);
                    winnerText.localScale = Vector3.one;
                    winnerText.gameObject.SetActive(false);
                }

            }
            //................. For One Player Showdown.......//
            if (holecard1 == null && holecard2 == null)
            {
                playerHoleCards.gameObject.SetActive(false);
                //print(" playerHoleCards");
                if (winnerText != null)
                {
                    //winnerText.SetParent(playerHoleCards);
                    LocalCardsPanel.gameObject.SetActive(false);
                    winnerText.SetParent(LocalCardsPanel);
                    winnerText.localPosition = new Vector2(0, 70f);
                    winnerText.localScale = Vector3.one;
                    winnerText.gameObject.SetActive(false);
                }
            }
            //.........................//
        }
        else
        {
            raiseImage.GetChild(2).GetComponent<Text>().text = "";
            dealerButton.gameObject.SetActive(false);
            if (winnerText != null)
            {
                winnerText.SetParent(playerShowCards);
                winnerText.SetSiblingIndex(2);
                winnerText.localPosition = winnerTextTransform.localPosition;
                winnerText.localEulerAngles = winnerTextTransform.localEulerAngles;
                winnerText.localScale = winnerTextTransform.localScale;
                //winnerText.localPosition = new Vector2(0, 77f);
                //winnerText.localScale = Vector3.one;
                winnerText.gameObject.SetActive(false);
                winnerText = null;
            }
            playerShowCards.gameObject.SetActive(false);

            //................................//
            if (holecard1 != null && holecard2 != null)
            {
                holecard1.transform.SetParent(UIManagerScript.instance.cards);
                holecard2.transform.SetParent(UIManagerScript.instance.cards);

                holecard1.transform.localPosition = Vector3.zero;
                holecard2.transform.localPosition = Vector3.zero;
                holecard1.transform.localRotation = Quaternion.Euler(0, 0, 0);
                holecard2.transform.localRotation = Quaternion.Euler(0, 0, 0);
                holecard1.transform.localScale = Vector3.one;
                holecard2.transform.localScale = Vector3.one;

                holecard1.transform.GetChild(1).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                holecard2.transform.GetChild(1).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

                holecard1 = null;
                holecard2 = null;

                playerHoleCards.gameObject.SetActive(false);
                //print("OnHandComple playerHoleCards");
                if (winnerText != null)
                {
                    winnerText.SetParent(playerHoleCards);
                    //winnerText.SetParent(LocalCardsPanel);
                    winnerText.localPosition = new Vector2(0, 70f);
                    winnerText.localScale = Vector3.one;
                    winnerText.gameObject.SetActive(false);
                }

            }
            //................. For One Player Showdown.......//
            if (holecard1 == null && holecard2 == null)
            {
                playerHoleCards.gameObject.SetActive(false);
                //print("OnHandComple playerHoleCards");
                if (winnerText != null)
                {
                    winnerText.SetParent(playerHoleCards);
                    //winnerText.SetParent(LocalCardsPanel);
                    winnerText.localPosition = new Vector2(0, 70f);
                    winnerText.localScale = Vector3.one;
                    winnerText.gameObject.SetActive(false);
                }
            }
            //...............................//
        }

        //................. For One Player Showdown.......//
        if (card1 != null && card2 != null)
        {
            card1.transform.SetParent(UIManagerScript.instance.cards);
            card2.transform.SetParent(UIManagerScript.instance.cards);

            card1.transform.localPosition = Vector3.zero;
            card2.transform.localPosition = Vector3.zero;

            card1.transform.localScale = Vector3.one;
            card2.transform.localScale = Vector3.one;
            card1.transform.localRotation = Quaternion.Euler(0, 0, 0);
            card2.transform.localRotation = Quaternion.Euler(0, 0, 0);

            card1.transform.GetChild(1).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            card2.transform.GetChild(1).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

            card1 = null;
            card2 = null;

            if (winnerText != null)
            {
                LocalCardsPanel.gameObject.SetActive(false);
                winnerText.SetParent(LocalCardsPanel);
                winnerText.localPosition = new Vector2(0, 70f);
                winnerText.localScale = Vector3.one;
                winnerText.gameObject.SetActive(false);
            }
        }

        betText.transform.GetComponent<Text>().text = "" + 0;
        if (betchipTotalAmount != null)
        {
            betchipTotalAmount.transform.GetComponent<Text>().text = "" + player.bet;
        }

        cardImage.gameObject.SetActive(false);
        cardImage.GetChild(0).gameObject.SetActive(false);
        print("TurnOff Dealer and player card Ends.." + player.playerName);
    }

    // This Function shows local player's cards.
    public void ShowLocalPlayerCards()
    {
        if (player.isSurvive)
        {
            StartCoroutine(ShowLocalPlayerCardsCoroutine());
        }
    }

    IEnumerator ShowLocalPlayerCardsCoroutine()
    {
        while (true)
        {
            if (CardShuffleAnimation.instance.isAnimationComplete)
            {
                break;
            }
            yield return new WaitForSeconds(0.5f);
        }

        //yield return new WaitForSeconds(0.5f);
        cardImage.gameObject.SetActive(false);
        cardImage.GetChild(0).gameObject.SetActive(false);

        if (player.cards.Length == 2 && card1 == null && card2 == null)
        {
            card1 = GameObject.Find(player.cards[0]);
            card2 = GameObject.Find(player.cards[1]);
            if (!player.folded)
            {
                playerHoleCards.gameObject.SetActive(true);
            }

            print("Hole Card true" + player.playerName);
        }

        if (card1 != null & card2 != null)
        {
            card1.transform.SetParent(playerHoleCards.GetChild(0));
            card2.transform.SetParent(playerHoleCards.GetChild(1));

            card1.transform.localPosition = Vector3.zero;
            card2.transform.localPosition = Vector3.zero;

            card1.transform.localEulerAngles = Vector3.zero;
            card2.transform.localEulerAngles = Vector3.zero;

            card1.transform.localScale = new Vector2(0.5f, 0.5f);
            card2.transform.localScale = new Vector2(0.5f, 0.5f);
            if (!GameManagerScript.instance.isVideoTable)
            {
                card1.transform.localScale = new Vector2(0.35f, 0.35f);
                card2.transform.localScale = new Vector2(0.35f, 0.35f);
            }
            print("Local player Hole Card true" + player.playerName);
        }
    }

    // This Function shows remote player's cards in showdown.
    public void ShowRemotePlayerCards(string[] holeCard)
    {
        print("Show player cards.." + player.playerName);
        allInImage.gameObject.SetActive(false);

        if (!isLocalPlayer && GameManagerScript.instance.isVideoTable)                       // video table : non-local player cards
        {
            holecard1 = GameObject.Find(holeCard[0]);
            holecard2 = GameObject.Find(holeCard[1]);

            if (holecard1 != null & holecard2 != null)
            {
                holecard1.transform.SetParent(LocalCardsPanel.GetChild(0));
                holecard2.transform.SetParent(LocalCardsPanel.GetChild(1));

                holecard1.transform.localPosition = Vector3.zero;
                holecard2.transform.localPosition = Vector3.zero;

                holecard1.transform.localScale = new Vector2(0.5f, 0.5f);
                holecard2.transform.localScale = new Vector2(0.5f, 0.5f);

                holecard1.transform.GetChild(1).GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 1f);
                holecard2.transform.GetChild(1).GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 1f);

                StartCoroutine(CardFlipAnim(holecard1, holecard2));
            }
        }

        if (isLocalPlayer && GameManagerScript.instance.isVideoTable)                       // video table : local player cards
        {
            if (card1 != null & card2 != null)
            {
                card1.transform.SetParent(LocalCardsPanel.GetChild(0));
                card2.transform.SetParent(LocalCardsPanel.GetChild(1));

                card1.transform.localPosition = Vector3.zero;
                card2.transform.localPosition = Vector3.zero;

                card1.transform.localScale = new Vector2(0.5f, 0.5f);
                card2.transform.localScale = new Vector2(0.5f, 0.5f);

                card1.transform.GetChild(1).GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 1f);
                card2.transform.GetChild(1).GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 1f);

                LocalCardsPanel.gameObject.SetActive(true);
                print("Local Card true");
            }
        }

        if (!GameManagerScript.instance.isVideoTable)
        {
            if (isLocalPlayer)                                                              // Non-video table : local player cards
            {
                card1.transform.SetParent(playerShowCards.GetChild(0));
                card2.transform.SetParent(playerShowCards.GetChild(1));

                card1.transform.localEulerAngles = Vector3.zero;
                card2.transform.localEulerAngles = Vector3.zero;

                card1.transform.localPosition = Vector3.zero;
                card2.transform.localPosition = Vector3.zero;

                card1.transform.localScale = new Vector2(0.35f, 0.35f);
                card2.transform.localScale = new Vector2(0.35f, 0.35f);

                card1.transform.GetChild(1).GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 1f);
                card2.transform.GetChild(1).GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 1f);

                playerShowCards.gameObject.SetActive(true);
                cardImage.gameObject.SetActive(false);
                cardImage.GetChild(0).gameObject.SetActive(false);
            }
            else                                                                                         // Non-video table : non local player cards
            {
                holecard1 = GameObject.Find(holeCard[0]);
                holecard2 = GameObject.Find(holeCard[1]);

                holecard1.transform.SetParent(playerShowCards.GetChild(0));
                holecard2.transform.SetParent(playerShowCards.GetChild(1));

                holecard1.transform.localPosition = Vector3.zero;
                holecard2.transform.localPosition = Vector3.zero;

                holecard1.transform.localEulerAngles = Vector3.zero;
                holecard2.transform.localEulerAngles = Vector3.zero;

                holecard1.transform.localScale = new Vector2(0.35f, 0.35f);
                holecard2.transform.localScale = new Vector2(0.35f, 0.35f);

                holecard1.transform.GetChild(1).GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 1f);
                holecard2.transform.GetChild(1).GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 1f);

                StartCoroutine(CardFlipAnim(holecard1, holecard2));
            }

        }
        print("Show pplayer cards ends  " + player.playerName);
    }

    IEnumerator CardFlipAnim(GameObject card1, GameObject card2)
    {
        yield return null;

        AnimateTransformFunctions.ins.AnimateTransform(cardImage, new Vector3(0, 0, 0), new Vector3(0, 90, 0), 0.2f, AnimateTransformFunctions.TransformTypes.Rotation, AnimateTransformFunctions.AnimAxis.World, "EaseOut");
        yield return new WaitForSeconds(0.2f);

        cardImage.gameObject.SetActive(false);
        cardImage.GetChild(0).gameObject.SetActive(false);
        cardImage.transform.localEulerAngles = Vector3.zero;

        if (!GameManagerScript.instance.isVideoTable)
        {
            playerShowCards.gameObject.SetActive(true);
        }
        else
        {
            playerHoleCards.gameObject.SetActive(true);
            LocalCardsPanel.gameObject.SetActive(true);
        }
        AnimateTransformFunctions.ins.AnimateTransform(card1.transform, new Vector3(0, 90, 0), new Vector3(0, 0, 0), 0.3f, AnimateTransformFunctions.TransformTypes.Rotation, AnimateTransformFunctions.AnimAxis.World, "EaseOut");
        AnimateTransformFunctions.ins.AnimateTransform(card2.transform, new Vector3(0, 90, 0), new Vector3(0, 0, 0), 0.3f, AnimateTransformFunctions.TransformTypes.Rotation, AnimateTransformFunctions.AnimAxis.World, "EaseOut");

        print("Hole Card true ");
    }

    public Transform winnerTextTransform;
    public void ShowWinner(float winMoney, string potType, string winText, bool _isMoneyAdd)
    {
        print("isMoneyAdd---------------------------------------------- " + _isMoneyAdd);
        print("player ---------------------------------------------- " + player.playerName);
        winText = winText.ToUpper();
        //allInImage.gameObject.SetActive(false);

        if (GameManagerScript.instance.isVideoTable)
        {
            if (LocalCardsPanel.childCount > 2)
            {
                winnerText = LocalCardsPanel.GetChild(2);

                if (_isMoneyAdd)
                {
                    if (GameManagerScript.instance.isVideoTable)
                    {
                        chipsText.transform.GetComponent<Text>().text = string.Format("{0:n0}", player.chips);
                        winnerText.GetChild(2).GetChild(3).GetChild(0).GetComponent<Text>().text = "+ " + winMoney;
                        winnerText.GetChild(2).GetChild(3).gameObject.SetActive(true);
                    }
                }

                winnerText.gameObject.SetActive(true);
                winnerText.SetParent(UIManagerScript.instance.winPanel.transform.GetChild(0));
                winnerText.SetSiblingIndex(1);
                winnerText.GetChild(5).GetChild(0).gameObject.SetActive(true);
                AnimateChips(winnerText.GetChild(5).GetChild(0), potType, winnerText.GetChild(5).GetChild(1), winText);
            }

        }
        else
        {
            if (playerShowCards.childCount > 2)
            {
                winnerText = playerShowCards.GetChild(2);
                winnerTextTransform = winnerText;
                winnerText.gameObject.SetActive(true);
                winnerText.SetParent(UIManagerScript.instance.winPanel.transform.GetChild(0));

                if (_isMoneyAdd)
                {
                    chipsText.transform.GetComponent<Text>().text = string.Format("{0:n0}", player.chips);
                    winnerText.GetChild(4).GetChild(2).gameObject.SetActive(true);
                    winnerText.GetChild(4).GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = "+" + winMoney;
                }
                AnimateChips(winnerText.GetChild(4).GetChild(0), potType, winnerText.GetChild(4).GetChild(1), winText);
            }
        }
    }

    //..........................................................New Win Animation............................................................................//

    public List<Transform> ChipToAnimate;
    Transform startGobj;

    public void AnimateChips(Transform chipsParent, string startIndex, Transform destination, string winText)
    {
        for (int i = 0; i < chipsParent.childCount; i++)
        {
            ChipToAnimate.Add(chipsParent.GetChild(i));
        }

        startGobj = StartPostion(startIndex, winText);

        StartCoroutine(Animate(ChipToAnimate, startGobj, destination));
    }

    IEnumerator Animate(List<Transform> chipsToAnimate, Transform start, Transform destination)
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < chipsToAnimate.Count; i++)
        {
            chipsToAnimate[i].localPosition = start.localPosition;
            chipsToAnimate[i].gameObject.SetActive(true);
            AnimateTransformFunctions.ins.AnimateTransform(chipsToAnimate[i], start.position, destination.position, 0.8f, AnimateTransformFunctions.TransformTypes.Position, AnimateTransformFunctions.AnimAxis.World, "EaseOut");
            yield return new WaitForSeconds(0.07f);
        }

        if (GameManagerScript.instance.isVideoTable)
        {
            destination.parent.GetChild(2).gameObject.SetActive(true);
            winnerText.GetChild(0).GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            //winnerText.GetChild(2).gameObject.SetActive(true);
            profileImg.GetChild(0).gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(1f);

        if (GameManagerScript.instance.isVideoTable)
        {
            destination.parent.GetChild(2).gameObject.SetActive(false);
            winnerText.GetChild(0).GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            //winnerText.GetChild(2).gameObject.SetActive(false);

            profileImg.GetChild(0).gameObject.SetActive(false);
        }

        //yield return new WaitForSeconds(0.4f);

        for (int i = 0; i < chipsToAnimate.Count; i++)
        {
            chipsToAnimate[i].gameObject.SetActive(false);
            chipsToAnimate[i].localPosition = start.localPosition;

        }
        chipsToAnimate.Clear();

    }

    Transform startPos;
    Transform StartPostion(string str, string winText)
    {
        TurnOffWinnerUI();
        if (WinningLogic.instance.onePlayerShowdown)
        {
            winText = "";
        }
        if (GameManagerScript.instance.isVideoTable)
        {
            if (str == "MP")
            {
                startPos = GameManagerScript.instance.start[0].transform;
                winnerText.GetChild(1).GetChild(4).gameObject.SetActive(true);
                winnerText.GetChild(1).GetChild(4).GetChild(0).GetComponent<Text>().text = winText;
            }
            else if (str == "MP2")
            {
                startPos = GameManagerScript.instance.start[0].transform;
                winnerText.GetChild(1).GetChild(5).gameObject.SetActive(true);
                winnerText.GetChild(1).GetChild(5).GetChild(0).GetComponent<Text>().text = winText;
            }
            else if (str == "SP1")
            {
                startPos = GameManagerScript.instance.start[1].transform;
                winnerText.GetChild(1).GetChild(3).gameObject.SetActive(true);
                winnerText.GetChild(1).GetChild(3).GetChild(0).GetComponent<Text>().text = winText;
            }
            else if (str == "SP2")
            {
                startPos = GameManagerScript.instance.start[2].transform;
                winnerText.GetChild(1).GetChild(2).gameObject.SetActive(true);
                winnerText.GetChild(1).GetChild(2).GetChild(0).GetComponent<Text>().text = winText;
            }
            else if (str == "SP3")
            {
                startPos = GameManagerScript.instance.start[3].transform;
                winnerText.GetChild(1).GetChild(1).gameObject.SetActive(true);
                winnerText.GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>().text = winText;
            }
            else if (str == "SP4")
            {
                startPos = GameManagerScript.instance.start[4].transform;
                winnerText.GetChild(1).GetChild(0).gameObject.SetActive(true);
                winnerText.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = winText;
            }
            else if (str == "SP5")
            {
                startPos = GameManagerScript.instance.start[5].transform;
            }
            return startPos;
        }
        else
        {

            if (str == "MP")
            {
                startPos = GameManagerScript.instance.start[6].transform;
                winnerText.GetChild(3).GetChild(4).gameObject.SetActive(true);

                //winnerText.GetChild(4).GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = winText;
            }
            else if (str == "MP2")
            {
                startPos = GameManagerScript.instance.start[6].transform;
                winnerText.GetChild(3).GetChild(5).gameObject.SetActive(true);
                //winnerText.GetChild(1).GetChild(3).GetChild(0).GetComponent<Text>().text = winText;
            }
            else if (str == "SP1")
            {
                startPos = GameManagerScript.instance.start[7].transform;
                winnerText.GetChild(3).GetChild(3).gameObject.SetActive(true);
                //winnerText.GetChild(1).GetChild(3).GetChild(0).GetComponent<Text>().text = winText;
            }
            else if (str == "SP2")
            {
                startPos = GameManagerScript.instance.start[8].transform;
                winnerText.GetChild(3).GetChild(2).gameObject.SetActive(true);
                //winnerText.GetChild(1).GetChild(2).GetChild(0).GetComponent<Text>().text = winText;
            }
            else if (str == "SP3")
            {
                startPos = GameManagerScript.instance.start[9].transform;
                winnerText.GetChild(3).GetChild(1).gameObject.SetActive(true);
                //winnerText.GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>().text = winText;
            }
            else if (str == "SP4")
            {
                startPos = GameManagerScript.instance.start[10].transform;
                winnerText.GetChild(3).GetChild(0).gameObject.SetActive(true);
                //winnerText.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = winText;
            }
            else if (str == "SP5")
            {
                startPos = GameManagerScript.instance.start[11].transform;
            }

            UIManagerScript.instance.winPanel.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = winText.ToUpper();
            return startPos;
            //return null;
        }
    }

    void TurnOffWinnerUI()
    {
        if (winnerText != null)
        {
            if (GameManagerScript.instance.isVideoTable)
            {
                for (int i = 0; i < winnerText.GetChild(1).childCount; i++)
                {
                    winnerText.GetChild(1).GetChild(i).gameObject.SetActive(false);
                }
            }
            else
            {
                for (int i = 0; i < winnerText.GetChild(3).childCount; i++)
                {
                    winnerText.GetChild(3).GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }


    //.................................................................New Win Animation Ends.....................................................................//

    public void ResetWinner()
    {
        if (GameManagerScript.instance.isVideoTable)
        {
            //print("ResetWinner......888");
            if (winnerText != null)
            {
                winnerText.SetParent(LocalCardsPanel);
                winnerText.localPosition = new Vector2(0, 70f);
                winnerText.localScale = Vector3.one;
                //winnerText.gameObject.SetActive(false);
                print("ResetWinner Ends......888");
            }
        }
        else
        {
            if (winnerText != null)
            {
                winnerText.SetParent(playerShowCards);
                winnerText.SetSiblingIndex(2);
                winnerText.localPosition = winnerTextTransform.localPosition;
                winnerText.localEulerAngles = winnerTextTransform.localEulerAngles;
                winnerText.localScale = winnerTextTransform.localScale;

            }
        }
    }

    // This function initialize each player's timer image and start timer coroutine.
    public void Timer(int time)
    {
        if (GameManagerScript.instance.timerImage != null)
        {
            GameManagerScript.instance.timerImage.gameObject.SetActive(false);
        }
        GameManagerScript.instance.timerImage = timerImage;
        try
        {
            GameManagerScript.instance.StopCoroutineTimer();
        }
        catch
        {
            print("Already Stopped" + player.playerName);
        }
        GameManagerScript.instance.StartCoroutineTimer(time, isLocalPlayer);
    }

    // This function stops timer coroutine.
    public void TimerOFF()
    {
        timerImage.gameObject.SetActive(false);
        if (isLocalPlayer)
        {
            //UIManagerScript.instance.localPlayerMenu.GetChild(0).GetChild(6).gameObject.SetActive(false);
        }
    }

    public void CardOn()
    {
        if (player.isSurvive)
        {
            cardImage.gameObject.SetActive(true);
            cardImage.GetChild(0).gameObject.SetActive(true);
            playerExclude.gameObject.SetActive(false);

            if (isLocalPlayer)
            {
                GameManagerScript.instance.isStandUp = false;
                GameManagerScript.instance.isStandupClicked = false;
                GameManagerScript.instance.isPlayerExcluded = false;
                UIManagerScript.instance.observingUI.SetActive(false);
                GameManagerScript.instance.isObserver = false;
                GameManagerScript.instance.isHandEnd = false;

                for (int i = 0; i < UIManagerScript.instance.allOtherBottomPanelBtn.Count; i++)
                {
                    UIManagerScript.instance.allOtherBottomPanelBtn[i].SetActive(true);
                }
            }
        }
        if (player.bet != 0)
        {
            betTextParent.gameObject.SetActive(true);
        }

        if (GameManagerScript.instance.isTournament && TournamentManagerScript.instance.breaktimer)
        {
            cardImage.gameObject.SetActive(false);
            cardImage.GetChild(0).gameObject.SetActive(false);
        }
    }

    // Reset values before shuffling seat
    public void ResetValues(int StopTimerCoroutine)                    // if StopTimerCoroutine == 1 then YES, else NO.
    {
        print("Enter In ResetValues " + player.playerName);
        try 
        {

            if (chipTextParent != null && GameManagerScript.instance.isVideoTable)
            {
                print("Enter In ResetValues video");
                chipTextParent.gameObject.SetActive(false);
                betTextParent.gameObject.SetActive(false);
                cardImage.gameObject.SetActive(false);
                cardImage.GetChild(0).gameObject.SetActive(false);
                micImage.gameObject.SetActive(false);
                //dealerButton.gameObject.SetActive(false); dealerButtonFirstChild.SetParent(dealerButton);
                timerImage.gameObject.SetActive(false);
                muteOn.gameObject.SetActive(false);
                transform.localScale = new Vector3(0, 0, 0);

                chair.gameObject.SetActive(true);
                print(chair.parent.parent.name);

                playerExclude.gameObject.SetActive(false);

                profileImg.GetChild(17).GetChild(0).GetChild(0).gameObject.SetActive(true);
                profileImg.GetChild(17).GetChild(0).GetChild(1).gameObject.SetActive(false);
                profileImg.GetChild(17).GetChild(1).GetChild(0).gameObject.SetActive(true);
                profileImg.GetChild(17).GetChild(1).GetChild(1).gameObject.SetActive(false);
                TurnOffActionUI(true);
                TimerOFF();
            }
            else if (!GameManagerScript.instance.isVideoTable)
            {
                print("Enter In ResetValues non-video " + player.playerName);
                chipTextParent.gameObject.SetActive(false);
                betTextParent.gameObject.SetActive(false);
                cardImage.gameObject.SetActive(false);
                dealerButton.gameObject.SetActive(false);
                SB.gameObject.SetActive(false);
                BB.gameObject.SetActive(false);
                timerImage.gameObject.SetActive(false);
                profileImg.gameObject.SetActive(false);
                nonVideoEmptySeat.gameObject.SetActive(true);

                playerExclude.gameObject.SetActive(false);
                TurnOffActionUI(true);
                TimerOFF();

                //Destroy(profileImg.GetChild(1).GetChild(0).GetComponent<Image>().sprite);
            }
            if (StopTimerCoroutine == 1)
            {
                try
                {
                    GameManagerScript.instance.StopCoroutineTimer();
                }
                catch
                {
                    print("Already Stopped");
                }
            }

            if (GameManagerScript.instance.isObserver && !GameManagerScript.instance.isTournament)
            {
                takeSeat.gameObject.SetActive(true);
                if (!GameManagerScript.instance.isVideoTable)
                {
                    nonVideoEmptySeat.gameObject.SetActive(false);
                }
            }
            else if (GameManagerScript.instance.isTournament)
            {
                takeSeat.gameObject.SetActive(false);
            }
        }
        
        catch
        {
            print("takeSeat Error");
        }

        print("Exit In ResetValues " + player.playerName);
    }

    public void PlayerDeactivate(bool isClearList)
    {
        ResetValues(0);
        TurnOffDealer();
        if (GameManagerScript.instance.isVideoTable)
        {
            if (chipTextParent != null)
            {
                if (isLocalPlayer)
                {
                    if (isClearList)
                    {
                        for (int i = 0; i < PlayersGenerator.instance.videoPanelsForAllClient.Count; i++)
                        {
                            GameObject objTrans = PlayersGenerator.instance.videoPanelsForAllClient[i].gameObject;
                            PlayersGenerator.instance.videoPanelsForAllClient.Remove(PlayersGenerator.instance.videoPanelsForAllClient[i]);
                            Destroy(objTrans);
                        }
                    }

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

                chipTextParent.gameObject.SetActive(false);
                betTextParent.gameObject.SetActive(false);
                micImage.gameObject.SetActive(false);
                cardImage.gameObject.SetActive(false);
                cardImage.GetChild(0).gameObject.SetActive(false);
                playerHoleCards.gameObject.SetActive(false);

                foldImage.gameObject.SetActive(false);
                callImage.gameObject.SetActive(false);
                raiseImage.gameObject.SetActive(false);
                betImage.gameObject.SetActive(false);
                checkImage.gameObject.SetActive(false);
                allInImage.gameObject.SetActive(false);
                //dealerButton.gameObject.SetActive(false); dealerButtonFirstChild.SetParent(dealerButton); /*dealerButton.SetSiblingIndex(18);*/
                timerImage.gameObject.SetActive(false);
                SB.gameObject.SetActive(false);
                BB.gameObject.SetActive(false);
                muteOn.gameObject.SetActive(false);
                isLocalPlayer = false;
                isAfterLocalPlayer = false;
                isBeforeLocalPlayer = false;
                timesAnimation = 0;

                if (GameManagerScript.instance.isObserver)
                {
                    chair.gameObject.SetActive(true);
                }

                for (int i = 0; i < PlayersGenerator.instance.videoPanelPlayers.Count; i++)
                {
                    if (player.playerName == PlayersGenerator.instance.videoPanelPlayers[i].name)
                    {
                        PlayersGenerator.instance.videoPanelPlayers.Remove(PlayersGenerator.instance.videoPanelPlayers[i]);
                    }
                }

                for (int i = 0; i < PlayersGenerator.instance.videoPanelsForAllClient.Count; i++)
                {
                    if (player.clientId == int.Parse(PlayersGenerator.instance.videoPanelsForAllClient[i].name))
                    {
                        GameObject objTrans = PlayersGenerator.instance.videoPanelsForAllClient[i].gameObject;
                        PlayersGenerator.instance.videoPanelsForAllClient.Remove(PlayersGenerator.instance.videoPanelsForAllClient[i]);
                        Destroy(objTrans);

                    }
                }
                koBounty.gameObject.SetActive(false);
                if (winnerText != null)
                {
                    LocalCardsPanel.gameObject.SetActive(false);
                    winnerText.SetParent(LocalCardsPanel);
                    winnerText.localPosition = new Vector2(0, 70f);
                    winnerText.localScale = Vector3.one;
                    winnerText.gameObject.SetActive(false);
                }
                profileImg.gameObject.SetActive(false);

            }
        }
        else
        {
            print("Enter IF In Player Deactive " + player.playerName);
            chipTextParent.gameObject.SetActive(false);
            betTextParent.gameObject.SetActive(false);
            cardImage.gameObject.SetActive(false);
            cardImage.GetChild(0).gameObject.SetActive(false);
            playerHoleCards.gameObject.SetActive(false);

            foldImage.gameObject.SetActive(false);
            callImage.gameObject.SetActive(false);
            raiseImage.gameObject.SetActive(false);
            betImage.gameObject.SetActive(false);
            checkImage.gameObject.SetActive(false);
            allInImage.gameObject.SetActive(false);
            //dealerButton.gameObject.SetActive(false);
            timerImage.gameObject.SetActive(false);
            SB.gameObject.SetActive(false);
            BB.gameObject.SetActive(false);
            isLocalPlayer = false;
            isAfterLocalPlayer = false;
            isBeforeLocalPlayer = false;
            timesAnimation = 0;
            koBounty.gameObject.SetActive(false);

            print("Exit Else In Player Deactive " + player.playerName);
        }
        PlayerClassReset();
        transform.SetParent(GameObject.Find("PlayerPool").transform);
        transform.gameObject.name = "PlayerPrefab";
        transform.gameObject.SetActive(false);
        print("Exit In Player Deactive Complete " + player.playerName);
    }

    public void StartSecondHand()
    {
        if (isLocalPlayer)
        {
            cardImage.gameObject.SetActive(false);
            cardImage.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void ResetPlayerOnTableExit(bool isResume)
    {
        ResetValues(1);
        TurnOffDealer();
        if (GameManagerScript.instance.isVideoTable)
        {
            chipTextParent.gameObject.SetActive(false);
            betTextParent.gameObject.SetActive(false);
            micImage.gameObject.SetActive(false);
            cardImage.gameObject.SetActive(false);
            playerHoleCards.gameObject.SetActive(false);

            foldImage.gameObject.SetActive(false);
            callImage.gameObject.SetActive(false);
            raiseImage.gameObject.SetActive(false);
            betImage.gameObject.SetActive(false);
            checkImage.gameObject.SetActive(false);
            allInImage.gameObject.SetActive(false);
            dealerButton.gameObject.SetActive(false); dealerButtonFirstChild.SetParent(dealerButton); /*dealerButton.SetSiblingIndex(18);*/
            timerImage.gameObject.SetActive(false);
            SB.gameObject.SetActive(false);
            BB.gameObject.SetActive(false);
            chair.gameObject.SetActive(true);
            takeSeat.gameObject.SetActive(true);

            if (isLocalPlayer)
            {
                if (!isResume)
                {
                    try
                    {
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
                    catch
                    {
                        print("No Local Card panel");
                    }
                }

                LocalCardsPanel.gameObject.SetActive(false);

                LocalCardsPanel = null;
            }
            koBounty.gameObject.SetActive(false);
            muteOn.gameObject.SetActive(false);

        }
        else
        {
            chipTextParent.gameObject.SetActive(false);
            betTextParent.gameObject.SetActive(false);
            cardImage.gameObject.SetActive(false);
            playerHoleCards.gameObject.SetActive(false);

            foldImage.gameObject.SetActive(false);
            callImage.gameObject.SetActive(false);
            raiseImage.gameObject.SetActive(false);
            betImage.gameObject.SetActive(false);
            checkImage.gameObject.SetActive(false);
            allInImage.gameObject.SetActive(false);
            dealerButton.gameObject.SetActive(false);
            timerImage.gameObject.SetActive(false);
            SB.gameObject.SetActive(false);
            BB.gameObject.SetActive(false);
            isLocalPlayer = false;
            isAfterLocalPlayer = false;
            isBeforeLocalPlayer = false;
            timesAnimation = 0;
            koBounty.gameObject.SetActive(false);

            //Destroy(profileImg.GetChild(1).GetChild(0).GetComponent<Image>().sprite);
        }

        commonUi = null;
        chipTextParent = null;
        playerNameText = null;
        chipsText = null;
        betTextParent = null;
        betText = null;
        betchipTotalAmount = null;
        foldImage = null;
        callImage = null;
        raiseImage = null;
        betImage = null;
        checkImage = null;
        allInImage = null;
        cardImage = null;
        micImage = null;
        chair = null;
        dealerButton = null;
        dealerButtonFirstChild = null;
        timerImage = null;
        SB = null;
        BB = null;
        LocalCardsPanel = null;
        winnerText = null;
        profileImg = null;

        playerHoleCards = null;
        holecard1 = null;
        holecard2 = null;
        takeSeat = null;
        chair = null;
        card1 = null;
        card2 = null;
        muteOn = null;
        playerExclude = null;
        isLocalPlayer = false;
        isAfterLocalPlayer = false;
        isBeforeLocalPlayer = false;
        timesAnimation = 0;
        PlayerClassReset();
        transform.SetParent(GameObject.Find("PlayerPool").transform);
        transform.gameObject.name = "PlayerPrefab";
        transform.gameObject.SetActive(false);
    }

    void PlayerClassReset()
    {
        player.playerName = null;
        player.message = null;
        player.chips = 0;
        player.folded = false;
        player.allIn = false;
        player.cards = null;
        player.isSurvive = false;
        player.reloadCount = 0;
        player.seatId = 0;
        player.roundBet = 0;
        player.clientId = 0;
        player.bet = 0;
        player.isOnline = false;
        player.isHuman = false;
    }

    internal Sprite imageSprite;

    public void GetImage(string url, System.Action<Sprite> _callback)
    {
        StartCoroutine(GetImageCoroutine(url, _callback));
    }

    Texture2D tex;
    public IEnumerator GetImageCoroutine(string url, System.Action<Sprite> _callback)
    {
        if (string.IsNullOrEmpty(url))
        {
            Debug.Log(": Player URL NULL....... in poker player controller :");
            yield return null;
        }
        else
        {
            UnityWebRequest wwwWebRequest = UnityWebRequestTexture.GetTexture(url);
            yield return wwwWebRequest.SendWebRequest();

            //Debug.Log(": Execute :");

            if (wwwWebRequest.isDone)
            {
                try
                {
                    tex = ((DownloadHandlerTexture)(wwwWebRequest.downloadHandler)).texture;
                    imageSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);

                    _callback(imageSprite);
                }
                catch (Exception e)
                {
                    _callback(Registration.instance.defaultPlayerImage.sprite);
                }

            }
            else
            {
                yield break;
            }
        }
    }
}
