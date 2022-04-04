using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinningLogic : MonoBehaviour
{
    public static WinningLogic instance;

    float rank;
    int seatID;
    int seatidlocal1;
    int playerPosInData;
    string name;
    string winCombination;
    public string[] final_cards;
    public List<string> final_cards2;
    public List<string> final_cardsList;
    public GameObject[] winner_cards;
    public GameObject[] winner_cards_2;
    //public List<GameObject> winner_cards2;
    public int totalPlayerInHand;
    public int totalPlayerCanWin;
    public int totalPlayerWinners;
    public int winMoney;

    public string winData;
    public bool mainPortEnter;
    public bool isMoneyAdd;
    public bool isRoundPotReset;
    public bool isOnlyMainPot;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        mainPortEnter = false;
        isRoundPotReset = true;
        isOnlyMainPot = true;
    }

    int totatFoldedPlayer;
    public bool onePlayerShowdown;
    public IEnumerator OnHandCompleteMain;
    public void OnHandComplete(GameSerializeClassesCollection.WinningHand winningHand)
    {
        GameManagerScript.instance.isHandEnd = true;
        winFunctions = MainPot(winningHand);
        winFunctions1 = Sidepot1(winningHand);
        winFunctions2 = Sidepot2(winningHand);
        winFunctions3 = Sidepot3(winningHand);
        winFunctions4 = Sidepot4(winningHand);
        StartWinCo(winningHand);
    }

    void StartWinCo(GameSerializeClassesCollection.WinningHand winningHand)
    {
        for (int i = 0; i < winningHand.players.Length; i++)
        {
            if (winningHand.players[i].cards.Length != 0)
            {

                GameObject player = GameObject.Find(winningHand.players[i].playerName);
                if (player != null)
                {
                    player.GetComponent<PokerPlayerController>().player.chips = winningHand.players[i].chips;
                    //player.GetComponent<PokerPlayerController>().chipsText.transform.GetComponent<Text>().text = string.Format("{0:n0}", winningHand.players[i].chips);  // "" + winningHand.players[i].chips;
                }
            }
        }

        OnHandCompleteMain = OnHandCompleteCorotine(winningHand);
        StartCoroutine(OnHandCompleteMain);
    }

    public void StopWinCo()
    {
        StopCoroutine(OnHandCompleteMain);
    }

    IEnumerator OnHandCompleteCorotine(GameSerializeClassesCollection.WinningHand winningHand)
    {
        yield return null;
        //...................................................... All Player Bet to Main Pot Anim ....................................................................//
        for (int i = 0; i < GameManagerScript.instance.playersParent.transform.childCount; i++)
        {
            if (GameManagerScript.instance.playersParent.transform.GetChild(i).childCount > 1)
            {
                try
                {
                    GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).transform.GetComponent<PokerPlayerController>().betTextParent.GetComponent<FunctionsContainer>().InvokeFunctions();    // FOR CHIP ANIM
                    GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).transform.GetComponent<PokerPlayerController>().DeactiveBetChipsCoroutine();
                }
                catch
                {
                    print("Some error in GameReset in WinningLogic.cs, DeactiveBetChipsCoroutine");
                }
            }
        }
        //..........................................................................................................................//

        PlayerActionManagement.instance.ComandExecuted();
        yield return new WaitForSeconds(0.8f);
        //StartCoroutine(GameReset());

        print("OnHandComplete start");
        //................. For One Player Showdown.......//
        totatFoldedPlayer = 0;
        onePlayerShowdown = false;
        for (int i = 0; i < winningHand.players.Length; i++)
        {
            if (winningHand.players[i].folded == true || winningHand.players[i].isSurvive == false)
            {
                totatFoldedPlayer++;
            }
        }

        if (totatFoldedPlayer == winningHand.players.Length)
        {
            //UIManagerScript.instance.lowChipPanel.gameObject.SetActive(true);
            UIManagerScript.instance.lowChipPanelText.text = "ERROR !!!";
            //return;
        }

        if (totatFoldedPlayer == winningHand.players.Length - 1)
        {
            onePlayerShowdown = true;
        }
        //................. For One Player Showdown.......//
        isRoundPotReset = false;
        GameManagerScript.instance.UpdateTable(winningHand.table);

        while (true)
        {
            if (Table.instance.cardOpened == 5 || onePlayerShowdown)
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.7f);

        for (int i = 0; i < winningHand.players.Length; i++)
        {
            if (winningHand.players[i].cards.Length != 0)
            {

                GameObject player = GameObject.Find(winningHand.players[i].playerName);
                if (player != null)
                {
                    player.GetComponent<PokerPlayerController>().player.chips = winningHand.players[i].chips;
                    //player.GetComponent<PokerPlayerController>().chipsText.transform.GetComponent<Text>().text = string.Format("{0:n0}", winningHand.players[i].chips);  // "" + winningHand.players[i].chips;
                }
            }
        }
        yield return new WaitForSeconds(0.8f);
        print("OnHandComplete Show Player Cards");
        for (int i = 0; i < winningHand.players.Length; i++)
        {
            if (winningHand.players[i].cards.Length != 0 && winningHand.players[i].folded == false)
            {
                try
                {
                    int actualSeatId = GameManagerScript.instance.GetActualSeatID(winningHand.players[i].seatId);
                    if (!onePlayerShowdown)
                    {
                        GameManagerScript.instance.playersParent.transform.GetChild(actualSeatId).GetChild(0).transform.GetComponent<PokerPlayerController>().ShowRemotePlayerCards(winningHand.players[i].cards);
                    }
                }
                catch
                {
                    print("Some error in OnHandComplete WinningLogic.cs " + winningHand.players[i].cards.Length);
                    print("Some error in OnHandComplete WinningLogic.cs " + winningHand.players[i].playerName);
                }
            }
        }
        yield return new WaitForSeconds(0.5f);
        print("OnHandComplete Show Player Cards complete");

        final_cards = new string[5];

        for (int i = 0; i < winningHand.players.Length; i++)
        {
            if (winningHand.players[i].cards.Length != 0 && winningHand.players[i].folded == false)
            {
                if (winningHand.players[i].winMoney != 0)
                {
                    totalPlayerWinners++;
                    winMoney = winningHand.players[i].winMoney;
                }
            }
        }
        final_cards2.Clear();
        final_cardsList.Clear();

        UIManagerScript.instance.winPanel.SetActive(true);

        SidepotCheck(winningHand);

        if (!onePlayerShowdown)
        {
            //UIManagerScript.instance.winPanel.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            //UIManagerScript.instance.winPanel.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = winCombination.ToUpper();
        }
        else
        {
            UIManagerScript.instance.winPanel.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        }

        Table.instance.UpdateTotalPot();
        print("OnHandComplete Ends");
    }

    public void StopWinningAnim()
    {
        try
        {
            StopCoroutine(winFunctions);
            StopCoroutine(winFunctions1);
            StopCoroutine(winFunctions2);
            StopCoroutine(winFunctions3);
            StopCoroutine(winFunctions4);
        }
        catch
        {
            print("Winner Already Stopped");
        }
    }

    public IEnumerator winFunctions;
    public IEnumerator winFunctions1;
    public IEnumerator winFunctions2;
    public IEnumerator winFunctions3;
    public IEnumerator winFunctions4;
    public void SidepotCheck(GameSerializeClassesCollection.WinningHand winningHand)
    {
        isOnlyMainPot = false;
        int sidePots = winningHand.table.sidePots.Length;

        print("sidePots_Countttttttttttttt -  " + sidePots);

        switch (sidePots)
        {
            case 0:
                print("sidePots -0--  ");
                StartCoroutine(winFunctions);
                //StartCoroutine(MainPot(winningHand));
                isOnlyMainPot = true;
                break;
            case 1:
                print("sidePots -1--  ");
                //StartCoroutine(Sidepot1(winningHand));
                StartCoroutine(winFunctions1);

                break;
            case 2:
                print("sidePots -2--  ");
                //StartCoroutine(Sidepot2(winningHand));
                StartCoroutine(winFunctions2);

                break;
            case 3:
                print("sidePots -3--  ");
                //StartCoroutine(Sidepot3(winningHand));
                StartCoroutine(winFunctions3);

                break;
            case 4:
                print("sidePots -4--  ");
                //StartCoroutine(Sidepot4(winningHand));
                StartCoroutine(winFunctions4);

                break;
            default:

                break;
        }
    }

    IEnumerator MainPot(GameSerializeClassesCollection.WinningHand winningHand)
    {
        print("main potttttttttttttt");

        final_cards2.Clear();
        final_cardsList.Clear();
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < winningHand.players.Length; i++)
        {
            if (winningHand.players[i].folded == false)
            {
                if (winningHand.players[i].winMoney != 0 && winningHand.players[i].MP == true)
                {
                    isMoneyAdd = true;

                    rank = winningHand.players[i].hand.rank;
                    seatID = winningHand.players[i].seatId;
                    name = winningHand.players[i].playerName;
                    winCombination = winningHand.players[i].hand.message;

                    for (int j = 0; j < winningHand.players[i].hand.final_cards.Length; j++)
                    {
                        final_cardsList.Add(winningHand.players[i].hand.final_cards[j]);
                    }
                    seatidlocal1 = GameManagerScript.instance.GetActualSeatID(winningHand.players[i].seatId);
                    mainPortEnter = true;

                    try
                    {
                        if (isOnlyMainPot == true)
                        {
                            GameManagerScript.instance.playersParent.transform.GetChild(seatidlocal1).GetChild(0).GetComponent<PokerPlayerController>().ShowWinner(winningHand.players[i].winMoney, "MP2", winningHand.players[i].hand.message, isMoneyAdd);
                        }
                        else
                        {
                            GameManagerScript.instance.playersParent.transform.GetChild(seatidlocal1).GetChild(0).GetComponent<PokerPlayerController>().ShowWinner(winningHand.players[i].winMoney, "MP", winningHand.players[i].hand.message, isMoneyAdd);
                        }
                    }

                    catch
                    {
                        print("Error in Main pot winner.");
                    }

                    if (!onePlayerShowdown)
                    {
                        HighlightCards(winningHand.players[i].hand.message);
                        yield return new WaitForSeconds(1.5f);
                        Table.instance.DeactiveStacks();
                        yield return new WaitForSeconds(3.3f);
                    }
                    else
                    {
                        yield return new WaitForSeconds(1.5f);
                        Table.instance.DeactiveStacks();
                        yield return new WaitForSeconds(3.3f);
                        DeHighlightCards();
                    }

                    if (isMoneyAdd)
                    {
                        //Table.instance.DeactiveStacks();
                        //yield return new WaitForSeconds(4.8f);
                        DeHighlightCards();
                    }
                }
            }
        }
        yield return new WaitForSeconds(0.1f);
        //GameManagerScript.instance.UpdateTable(winningHand.table);
        isRoundPotReset = true;
        Table.instance.UpdateTotalPot();
        Table.instance.RoundPot.transform.parent.gameObject.SetActive(false);

        for (int i = 0; i < winningHand.players.Length; i++)
        {
            if (winningHand.players[i].cards.Length != 0 /*&& !winningHand.players[i].newJoinee*/)
            {

                GameObject player = GameObject.Find(winningHand.players[i].playerName);
                if (player != null)
                {
                    player.GetComponent<PokerPlayerController>().player.chips = winningHand.players[i].chips;
                    player.GetComponent<PokerPlayerController>().chipsText.transform.GetComponent<Text>().text = /*"$" + */string.Format("{0:n0}", winningHand.players[i].chips);  // "" + winningHand.players[i].chips;
                }
            }
        }
        print("Called in winnlogic..........");
        StartCoroutine(GameReset());
    }
    IEnumerator Sidepot1(GameSerializeClassesCollection.WinningHand winningHand)
    {
        print("Sidepot-11111");
        final_cards2.Clear();
        final_cardsList.Clear();

        //yield return new WaitForSeconds(1f);

        for (int i = 0; i < winningHand.players.Length; i++)
        {
            if (winningHand.players[i].folded == false /*&& !winningHand.players[i].newJoinee*/ && winningHand.players[i].winMoney != 0)
            {
                if (winningHand.players[i].SP1 == true)
                {
                    //...........................//
                    if (!winningHand.players[i].MP)
                    {
                        isMoneyAdd = true;
                    }
                    else
                    {
                        isMoneyAdd = false;
                    }
                    //................................//
                    seatidlocal1 = GameManagerScript.instance.GetActualSeatID(winningHand.players[i].seatId);

                    for (int j = 0; j < winningHand.players[i].hand.final_cards.Length; j++)
                    {
                        final_cardsList.Add(winningHand.players[i].hand.final_cards[j]);
                    }

                    try
                    {
                        GameManagerScript.instance.playersParent.transform.GetChild(seatidlocal1).GetChild(0).GetComponent<PokerPlayerController>().ShowWinner(winningHand.players[i].winMoney, "SP1", winningHand.players[i].hand.message, isMoneyAdd);
                    }

                    catch
                    {
                        print("Error in Sidepot1 winner.");
                    }
                    playerPosInData = i;
                    if (!onePlayerShowdown)
                    {
                        HighlightCards(winningHand.players[i].hand.message);
                        yield return new WaitForSeconds(1.5f);

                        Table.instance.sidePotParent.transform.GetChild(0).gameObject.SetActive(false);
                        Table.instance.sidePotParent.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = "";
                        Table.instance.sidePotParentWin.transform.GetChild(0).gameObject.SetActive(false);
                        Table.instance.sidePotParentWin.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = "";
                        yield return new WaitForSeconds(3.3f);
                    }
                    else
                    {
                        yield return new WaitForSeconds(1.5f);

                        Table.instance.sidePotParent.transform.GetChild(0).gameObject.SetActive(false);
                        Table.instance.sidePotParent.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = "";
                        Table.instance.sidePotParentWin.transform.GetChild(0).gameObject.SetActive(false);
                        Table.instance.sidePotParentWin.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = "";
                        yield return new WaitForSeconds(3.3f);
                        DeHighlightCards();
                    }

                    if (isMoneyAdd)
                    {
                        //yield return new WaitForSeconds(4.8f);
                        DeHighlightCards();

                    }
                }
            }
        }

        yield return new WaitForSeconds(0.1f);
        //Table.instance.sidePotParent.transform.GetChild(0).gameObject.SetActive(false);
        //Table.instance.sidePotParent.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = "";
        DeHighlightCards();
        //GameManagerScript.instance.playersParent.transform.GetChild(seatidlocal1).GetChild(0).GetComponent<PokerPlayerController>().ResetWinner();
        //if (GameManagerScript.instance.playersParent.transform.GetChild(seatidlocal1).GetChild(0).GetComponent<PokerPlayerController>() != null)
        //{
        //    GameManagerScript.instance.playersParent.transform.GetChild(seatidlocal1).GetChild(0).GetComponent<PokerPlayerController>().ResetWinner();
        //}
        try
        {
            GameManagerScript.instance.playersParent.transform.GetChild(seatidlocal1).GetChild(0).GetComponent<PokerPlayerController>().ResetWinner();
        }
        catch
        {
            print("Error in Sidepot1 winner.");
        }
        StartCoroutine(MainPot(winningHand));
    }
    IEnumerator Sidepot2(GameSerializeClassesCollection.WinningHand winningHand)
    {
        print("Sidepot-222222");

        final_cards2.Clear();
        final_cardsList.Clear();

        //yield return new WaitForSeconds(1f);

        for (int i = 0; i < winningHand.players.Length; i++)
        {

            if (winningHand.players[i].folded == false /*&& !winningHand.players[i].newJoinee*/ && winningHand.players[i].winMoney != 0)
            {
                if (winningHand.players[i].SP2 == true)
                {
                    //..............................//

                    if (!winningHand.players[i].SP1)
                    {
                        if (!winningHand.players[i].MP)
                        {
                            isMoneyAdd = true;
                        }
                        else
                        {
                            isMoneyAdd = false;
                        }
                    }
                    else
                    {
                        isMoneyAdd = false;
                    }

                    seatidlocal1 = GameManagerScript.instance.GetActualSeatID(winningHand.players[i].seatId);
                    for (int j = 0; j < winningHand.players[i].hand.final_cards.Length; j++)
                    {
                        final_cardsList.Add(winningHand.players[i].hand.final_cards[j]);
                    }

                    try
                    {
                        GameManagerScript.instance.playersParent.transform.GetChild(seatidlocal1).GetChild(0).GetComponent<PokerPlayerController>().ShowWinner(winningHand.players[i].winMoney, "SP2", winningHand.players[i].hand.message, isMoneyAdd);
                        //UIManagerScript.instance.winPanel.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = winningHand.players[i].hand.message.ToUpper();
                    }

                    catch
                    {
                        print("Error in Sidepot2 winner.");
                    }

                    if (!onePlayerShowdown)
                    {
                        HighlightCards(winningHand.players[i].hand.message);
                        yield return new WaitForSeconds(1.5f);

                        Table.instance.sidePotParent.transform.GetChild(1).gameObject.SetActive(false);
                        Table.instance.sidePotParent.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>().text = "";
                        Table.instance.sidePotParentWin.transform.GetChild(1).gameObject.SetActive(false);
                        Table.instance.sidePotParentWin.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>().text = "";
                        yield return new WaitForSeconds(3.3f);

                    }
                    else
                    {
                        yield return new WaitForSeconds(1.5f);

                        Table.instance.sidePotParent.transform.GetChild(1).gameObject.SetActive(false);
                        Table.instance.sidePotParent.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>().text = "";
                        Table.instance.sidePotParentWin.transform.GetChild(1).gameObject.SetActive(false);
                        Table.instance.sidePotParentWin.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>().text = "";
                        yield return new WaitForSeconds(3.3f);
                        DeHighlightCards();
                    }

                    if (isMoneyAdd)
                    {
                        //yield return new WaitForSeconds(4.8f);
                        DeHighlightCards();
                    }
                }
            }
        }
        yield return new WaitForSeconds(0.1f);
        //Table.instance.sidePotParent.transform.GetChild(1).gameObject.SetActive(false);
        //Table.instance.sidePotParent.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>().text = "";
        DeHighlightCards();
        //GameManagerScript.instance.playersParent.transform.GetChild(seatidlocal1).GetChild(0).GetComponent<PokerPlayerController>().ResetWinner();
        //if (GameManagerScript.instance.playersParent.transform.GetChild(seatidlocal1).GetChild(0).GetComponent<PokerPlayerController>() != null)
        //{
        //    GameManagerScript.instance.playersParent.transform.GetChild(seatidlocal1).GetChild(0).GetComponent<PokerPlayerController>().ResetWinner();
        //}
        try
        {
            GameManagerScript.instance.playersParent.transform.GetChild(seatidlocal1).GetChild(0).GetComponent<PokerPlayerController>().ResetWinner();
        }
        catch
        {
            print("Error in Sidepot2 winner.");
        }
        StartCoroutine(Sidepot1(winningHand));
    }
    IEnumerator Sidepot3(GameSerializeClassesCollection.WinningHand winningHand)
    {
        print("Sidepot-333333");

        final_cards2.Clear();
        final_cardsList.Clear();

        //yield return new WaitForSeconds(1f);

        for (int i = 0; i < winningHand.players.Length; i++)
        {
            if (winningHand.players[i].folded == false /*&& !winningHand.players[i].newJoinee*/ && winningHand.players[i].winMoney != 0)
            {
                if (winningHand.players[i].SP3 == true)
                {
                    //..........................//
                    if (!winningHand.players[i].SP2)
                    {
                        if (!winningHand.players[i].SP1)
                        {
                            if (!winningHand.players[i].MP)
                            {
                                isMoneyAdd = true;
                            }
                            else
                            {
                                isMoneyAdd = false;
                            }
                        }
                        else
                        {
                            isMoneyAdd = false;
                        }
                    }
                    else
                    {
                        isMoneyAdd = false;
                    }

                    //............................//

                    seatidlocal1 = GameManagerScript.instance.GetActualSeatID(winningHand.players[i].seatId);
                    for (int j = 0; j < winningHand.players[i].hand.final_cards.Length; j++)
                    {
                        final_cardsList.Add(winningHand.players[i].hand.final_cards[j]);
                    }

                    try
                    {
                        GameManagerScript.instance.playersParent.transform.GetChild(seatidlocal1).GetChild(0).GetComponent<PokerPlayerController>().ShowWinner(winningHand.players[i].winMoney, "SP3", winningHand.players[i].hand.message, isMoneyAdd);
                        //UIManagerScript.instance.winPanel.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = winningHand.players[i].hand.message.ToUpper();
                    }
                    catch
                    {
                        print("Error in Sidepot3 winner.");
                    }

                    if (!onePlayerShowdown)
                    {
                        HighlightCards(winningHand.players[i].hand.message);
                        yield return new WaitForSeconds(1.5f);
                        Table.instance.sidePotParent.transform.GetChild(2).gameObject.SetActive(false);
                        Table.instance.sidePotParent.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<Text>().text = "";
                        Table.instance.sidePotParentWin.transform.GetChild(2).gameObject.SetActive(false);
                        Table.instance.sidePotParentWin.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<Text>().text = "";
                        yield return new WaitForSeconds(3.3f);
                    }
                    else
                    {
                        yield return new WaitForSeconds(1.5f);
                        Table.instance.sidePotParent.transform.GetChild(2).gameObject.SetActive(false);
                        Table.instance.sidePotParent.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<Text>().text = "";
                        Table.instance.sidePotParentWin.transform.GetChild(2).gameObject.SetActive(false);
                        Table.instance.sidePotParentWin.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<Text>().text = "";
                        yield return new WaitForSeconds(3.3f);
                        DeHighlightCards();
                    }

                    if (isMoneyAdd)
                    {
                        //yield return new WaitForSeconds(4.8f);
                        DeHighlightCards();
                    }

                }
            }
        }
        yield return new WaitForSeconds(0.1f);
        //Table.instance.sidePotParent.transform.GetChild(2).gameObject.SetActive(false);
        //Table.instance.sidePotParent.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<Text>().text = "";
        DeHighlightCards();
        //if (GameManagerScript.instance.playersParent.transform.GetChild(seatidlocal1).GetChild(0).GetComponent<PokerPlayerController>() != null)
        //{
        //    GameManagerScript.instance.playersParent.transform.GetChild(seatidlocal1).GetChild(0).GetComponent<PokerPlayerController>().ResetWinner();
        //}
        try
        {
            GameManagerScript.instance.playersParent.transform.GetChild(seatidlocal1).GetChild(0).GetComponent<PokerPlayerController>().ResetWinner();
        }
        catch
        {
            print("Error in Sidepot3 winner.");
        }
        StartCoroutine(Sidepot2(winningHand));
    }
    IEnumerator Sidepot4(GameSerializeClassesCollection.WinningHand winningHand)
    {
        print("Sidepot-44444");

        final_cards2.Clear();
        final_cardsList.Clear();

        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < winningHand.players.Length; i++)
        {
            if (winningHand.players[i].folded == false /*&& !winningHand.players[i].newJoinee*/ && winningHand.players[i].winMoney != 0)
            {
                if (winningHand.players[i].SP4 == true)
                {
                    //..........................//
                    if (!winningHand.players[i].SP3)
                    {
                        if (!winningHand.players[i].SP2)
                        {
                            if (!winningHand.players[i].SP1)
                            {
                                if (!winningHand.players[i].MP)
                                {
                                    isMoneyAdd = true;
                                }
                                else
                                {
                                    isMoneyAdd = false;
                                }
                            }
                            else
                            {
                                isMoneyAdd = false;
                            }
                        }
                        else
                        {
                            isMoneyAdd = false;
                        }
                    }
                    else
                    {
                        isMoneyAdd = false;
                    }

                    seatidlocal1 = GameManagerScript.instance.GetActualSeatID(winningHand.players[i].seatId);

                    for (int j = 0; j < winningHand.players[i].hand.final_cards.Length; j++)
                    {
                        final_cardsList.Add(winningHand.players[i].hand.final_cards[j]);
                    }

                    try
                    {
                        GameManagerScript.instance.playersParent.transform.GetChild(seatidlocal1).GetChild(0).GetComponent<PokerPlayerController>().ShowWinner(winningHand.players[i].winMoney, "SP4", winningHand.players[i].hand.message, isMoneyAdd);
                        //UIManagerScript.instance.winPanel.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = winningHand.players[i].hand.message.ToUpper();
                    }
                    catch
                    {
                        print("Error in Sidepot4 winner.");
                    }

                    if (!onePlayerShowdown)
                    {
                        HighlightCards(winningHand.players[i].hand.message);
                        yield return new WaitForSeconds(1.5f);

                        Table.instance.sidePotParent.transform.GetChild(3).gameObject.SetActive(false);
                        Table.instance.sidePotParent.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<Text>().text = "";
                        Table.instance.sidePotParentWin.transform.GetChild(3).gameObject.SetActive(false);
                        Table.instance.sidePotParentWin.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<Text>().text = "";
                        yield return new WaitForSeconds(3.3f);
                    }
                    else
                    {
                        yield return new WaitForSeconds(1.5f);

                        Table.instance.sidePotParent.transform.GetChild(3).gameObject.SetActive(false);
                        Table.instance.sidePotParent.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<Text>().text = "";
                        Table.instance.sidePotParentWin.transform.GetChild(3).gameObject.SetActive(false);
                        Table.instance.sidePotParentWin.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<Text>().text = "";
                        yield return new WaitForSeconds(3.3f);
                        DeHighlightCards();
                    }

                    if (isMoneyAdd)
                    {
                        //yield return new WaitForSeconds(4.8f);
                        DeHighlightCards();
                    }
                }
            }
        }
        yield return new WaitForSeconds(0.1f);

        DeHighlightCards();
        try
        {
            GameManagerScript.instance.playersParent.transform.GetChild(seatidlocal1).GetChild(0).GetComponent<PokerPlayerController>().ResetWinner();
        }
        catch
        {
            print("Error in Sidepot4 winner.");
        }
        StartCoroutine(Sidepot3(winningHand));
    }


    public void HighlightCards(string winComb)
    {
        winner_cards = new GameObject[final_cardsList.Count];
        winner_cards_2 = new GameObject[final_cardsList.Count];
        print("HighlightCards Starts " + final_cardsList.Count);

        for (int i = 0; i < final_cardsList.Count; i++)
        {
            winner_cards[i] = GameObject.Find(final_cardsList[i]);
            winner_cards[i].transform.GetChild(2).gameObject.SetActive(true);
            winner_cards[i].transform.GetChild(1).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

            winner_cards[i].transform.position = new Vector3(winner_cards[i].transform.position.x, winner_cards[i].transform.position.y + 35, winner_cards[i].transform.position.z);

            winner_cards[i].transform.SetParent(UIManagerScript.instance.winPanel.transform.GetChild(0));
            if (!GameManagerScript.instance.isVideoTable)
            {
                if (winner_cards[i].transform.localScale.y >= 0.8f)
                {
                    winner_cards[i].transform.localScale = new Vector3(0.7f, 0.8f, 0);
                }
            }
            else
            {
                winner_cards[i].transform.localEulerAngles = new Vector3(0f, 0f, 0);
            }
        }

        UIManagerScript.instance.newWinnerCards.SetActive(true);
        UIManagerScript.instance.newWinnerCards.transform.GetChild(3).GetComponent<Text>().text = winComb.ToUpper();
        print("New WInner......" + UIManagerScript.instance.newWinnerCards.transform.GetChild(0).childCount);

        int j = 0;

        for (int i = 0; i < UIManagerScript.instance.newWinnerCards.transform.GetChild(0).childCount;)
        {
            if (final_cardsList[j] == UIManagerScript.instance.newWinnerCards.transform.GetChild(0).GetChild(i).gameObject.name)
            {
                winner_cards_2[j] = UIManagerScript.instance.newWinnerCards.transform.GetChild(0).GetChild(i).gameObject;
                j++;
                if (j == 5)
                {
                    break;
                }
                else
                {
                    i = 0;
                    continue;
                }
            }
            i++;
        }
        try
        {
            for (int i = 0; i < final_cardsList.Count; i++)
            {
                if (UIManagerScript.instance.newWinnerCards.transform.GetChild(1).GetChild(i) != null)
                {
                    winner_cards_2[i].transform.SetParent(UIManagerScript.instance.newWinnerCards.transform.GetChild(1).GetChild(i));
                    winner_cards_2[i].transform.localPosition = Vector3.zero;
                    winner_cards_2[i].transform.localScale = new Vector3(0.4f, 0.6f, 0);
                }
            }
        }
        catch (Exception e)
        {

            print("winner_cards_2 HighlightCards" + e);
        }


        print("HighlightCards ends");
    }

    void DeHighlightCards()
    {
        try
        {
            //............Card.......................//
            for (int i = 0; i < winner_cards.Length; i++)
            {
                print("Enter DeHighlight Cards if: " + i);
                winner_cards[i].transform.GetChild(2).gameObject.SetActive(false);
                // winner_cards[i].transform.position = new Vector3(winner_cards[i].transform.position.x, winner_cards[i].transform.position.y - 10, winner_cards[i].transform.position.z);
                winner_cards[i].transform.position = new Vector3(winner_cards[i].transform.position.x, winner_cards[i].transform.position.y - 35, winner_cards[i].transform.position.z);
                winner_cards[i] = null;
            }
            for (int i = 0; i < final_cardsList.Count; i++)
            {
                winner_cards_2[i].transform.SetParent(UIManagerScript.instance.newWinnerCards.transform.GetChild(0));
                winner_cards_2[i].transform.position = new Vector3(0, 0, 0);
                winner_cards_2[i] = null;
            }
            Table.instance.card1.transform.GetChild(1).GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 1f);
            Table.instance.card2.transform.GetChild(1).GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 1f);
            Table.instance.card3.transform.GetChild(1).GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 1f);
            Table.instance.card4.transform.GetChild(1).GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 1f);
            Table.instance.card5.transform.GetChild(1).GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 1f);
        }
        catch
        {
            print("No cards to highlight");
        }
        final_cards2.Clear();
        final_cardsList.Clear();
        UIManagerScript.instance.newWinnerCards.SetActive(false);
        //........................................//
    }

    public void DeHighlightCardsAndReset()
    {
        try
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < UIManagerScript.instance.newWinnerCards.transform.GetChild(1).GetChild(i).childCount; j++)
                {
                    UIManagerScript.instance.newWinnerCards.transform.GetChild(1).GetChild(i).GetChild(j).SetParent(UIManagerScript.instance.newWinnerCards.transform.GetChild(0));
                }
            }

            for (int i = 0; i < winner_cards_2.Length; i++)
            {
                winner_cards_2[i].transform.SetParent(UIManagerScript.instance.newWinnerCards.transform.GetChild(0));
                winner_cards_2[i].transform.position = new Vector3(0, 0, 0);
                winner_cards_2[i] = null;
            }

            UIManagerScript.instance.newWinnerCards.SetActive(false);

            for (int i = 0; i < winner_cards.Length; i++)
            {
                winner_cards[i].transform.GetChild(2).gameObject.SetActive(false);
                winner_cards[i].transform.SetParent(UIManagerScript.instance.cards);
                winner_cards[i].transform.localScale = new Vector2(1, 1);
                winner_cards[i].transform.localPosition = Vector3.zero;

                winner_cards[i] = null;
            }

        }
        catch
        {
            print("No cards to DeHighlightCardsAndReset");
        }
    }

    public IEnumerator GameReset()
    {

        print("game reset start");
        Table.instance.cardOpened = 0;
        GameManagerScript.instance.isHandEnd = false;
        totalPlayerInHand = 0;
        totalPlayerCanWin = 0;
        totalPlayerWinners = 0;
        Table.instance.DeactiveStacks();
        DeHighlightCardsAndReset();
        final_cards2.Clear();
        final_cardsList.Clear();
        UIManagerScript.instance.winPanel.SetActive(false);
        UIManagerScript.instance.winPanel.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = "";

        if (GameManagerScript.instance.HandRankPanel != null)
        {
            GameManagerScript.instance.HandRankPanel.transform.GetChild(0).GetComponent<Text>().text = "";
            GameManagerScript.instance.HandRankPanel.SetActive(false);
        }

        winCombination = "";
        UIManagerScript.instance.ResetCardsPosition();
        Table.instance.ResetSidePot();
        Table.instance.roundNameFromGameClass = "";
        onePlayerShowdown = false;

        yield return new WaitForSeconds(0.35f);
        for (int i = 0; i < GameManagerScript.instance.playersParent.transform.childCount; i++)
        {
            if (GameManagerScript.instance.playersParent.transform.GetChild(i).childCount > 1)
            {
                try
                {
                    GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).transform.GetComponent<PokerPlayerController>().TurnOffActionUI(false);
                    GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).transform.GetComponent<PokerPlayerController>().TurnOffDealer();
                }
                catch
                {
                    print("Some error in GameReset in WinningLogic.cs, TurnOffActionUI");
                }
            }
        }

        UIManagerScript.instance.topUpChipBtn.transform.GetComponent<Button>().interactable = true;
        UIManagerScript.instance.topUpChipBtn.transform.GetChild(0).GetComponent<Text>().color = new Color(1, 1, 1, 1);
        UIManagerScript.instance.topUpChipBtn.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 1);
        print("GameReset Ends");
    }

    //.......................................................CODE ENDS...............................................................//

}
