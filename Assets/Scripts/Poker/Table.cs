using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Table : MonoBehaviour
{
    public static Table instance;

    [Header("Table Status")]
    public TableInfo table;

    [Serializable]
    public class TableInfo
    {
        public string tableNumber;
        public int status;
        public string roundName;
        public string[] board;
        public int roundCount;
        public int raiseCount;
        public int betCount;
        public int totalBet;
        public int initChips;
        public int currentPlayer;
        public string currentPlayerName;
        public int maxReloadCount;
        public int[] sidePots;
        public string smallBlindUp;
        public string bigBlindUp;
        public int currentLevel;
        [SerializeField]
        public SmallAndBigBlindInfo smallBlind;
        [SerializeField]
        public SmallAndBigBlindInfo bigBlind;
        [SerializeField]
        public SmallAndBigBlindInfo dealer;
        public int commandTimeout;
        public int[] realSidePots;
    }

    [Serializable]
    public class SmallAndBigBlindInfo
    {
        public string playerName;
        public int seatId;
        public int amount;
    }

    [Header("Game Status")]
    public Game game;

    [Serializable]
    public class Game
    {
        public Transform[] board;
        public string roundName;
        public int roundCount;
        public int raiseCount;
        public int betCount;
    }
    [Space]
    [Space]
    // public int gameStartCounter;
    public Text gameStartCounter;
    public Text tableNumber;
    public Text tableType;
    public Text blind;
    public Text TotalPot;
    public Text RoundPot;
    public Text LocalPlayerMessage;
    public Text Id;
    public float previousPotAmount = 0;
    public GameObject sidePotParent;
    public GameObject stackParent;
    public GameObject sidePotParentWin;

    [Header("Community Cards")]
    public GameObject card1;
    public GameObject card2;
    public GameObject card3;
    public GameObject card4;
    public GameObject card5;

    // Temprorary for OnePlayeShowdown
    public string roundNameFromGameClass;



    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        table.board = new string[5];
        print("GameObject Name " + this.transform.gameObject.name);
    }

    public IEnumerator TurnOffCounter()
    {
        yield return new WaitForSeconds(0.5f);
        gameStartCounter.enabled = false;
    }

    public IEnumerator OpenCardStarts;

    public void UpdateTableValues()
    {

        #region OlD Code
        //if (table.board.Length == 0)
        //{
        //    return;
        //}

        //if (table.board.Length == 3)
        //{
        //    card1 = GameObject.Find(table.board[0]);
        //    card2 = GameObject.Find(table.board[1]);
        //    card3 = GameObject.Find(table.board[2]);
        //    card1.transform.SetParent(UIManagerScript.instance.tableParent.GetChild(5).GetChild(0));
        //    card2.transform.SetParent(UIManagerScript.instance.tableParent.GetChild(5).GetChild(1));
        //    card3.transform.SetParent(UIManagerScript.instance.tableParent.GetChild(5).GetChild(2));
        //    card1.transform.localPosition = Vector3.zero;
        //    card2.transform.localPosition = Vector3.zero;
        //    card3.transform.localPosition = Vector3.zero;

        //    card1.transform.localEulerAngles = Vector3.zero;
        //    card2.transform.localEulerAngles = Vector3.zero;
        //    card3.transform.localEulerAngles = Vector3.zero;

        //    if (GameManagerScript.instance.isVideoTable)
        //    {
        //        card1.transform.localScale = new Vector2(0.70f, 0.70f);
        //        card2.transform.localScale = new Vector2(0.70f, 0.70f);
        //        card3.transform.localScale = new Vector2(0.70f, 0.70f);

        //    }
        //    else
        //    {
        //        card1.transform.localScale = new Vector2(0.7f, 0.8f);
        //        card2.transform.localScale = new Vector2(0.7f, 0.8f);
        //        card3.transform.localScale = new Vector2(0.7f, 0.8f);
        //    }
        //}

        //else if (table.board.Length == 4)
        //{
        //    //UIManagerScript.instance.tableParent.GetChild(5).GetChild(3).gameObject.SetActive(true);
        //    OpenFourCards();
        //}

        //else if (table.board.Length == 5)
        //{
        //    //UIManagerScript.instance.tableParent.GetChild(5).GetChild(4).gameObject.SetActive(true);

        //    if (card1 == null)
        //    {
        //        OpenFiveCards();
        //    }

        //    else if (card4 == null)
        //    {
        //        OpenTwoCards();
        //    }

        //    else
        //    {
        //        card5 = GameObject.Find(table.board[4]);
        //        card5.transform.SetParent(UIManagerScript.instance.tableParent.GetChild(5).GetChild(4));
        //        card5.transform.localPosition = Vector3.zero;
        //        card5.transform.localEulerAngles = Vector3.zero;

        //        if (GameManagerScript.instance.isVideoTable)
        //        {
        //            card5.transform.localScale = new Vector2(0.70f, 0.70f);
        //        }
        //        else
        //        {
        //            card5.transform.localScale = new Vector2(0.7f, 0.8f);
        //        }
        //    }
        //}

        //SoundManager.instance.PlayPomeSound(AudioClipCollection.instance.communityCardOpenSFX);
        #endregion

        //................. For One Player Showdown.......//
        if (WinningLogic.instance.onePlayerShowdown)
        {
            print("Showdwon One Player");
            if (roundNameFromGameClass == "Deal")
            {

                card1.SetActive(false);
                card2.SetActive(false);
                card3.SetActive(false);
                card4.SetActive(false);
                card5.SetActive(false);
            }

            if (roundNameFromGameClass == "Flop")
            {
                card4.SetActive(false);
                card5.SetActive(false);
            }
            if (roundNameFromGameClass == "Turn")
            {
                card5.SetActive(false);
            }

            UIManagerScript.instance.winPanel.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            UIManagerScript.instance.winPanel.transform.GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, 0);

        }
        //................. For One Player Showdown.......//
        else
        {
            try
            {
                OpenCardStarts = OpenCards();
                StartCoroutine(OpenCardStarts);
            }
            catch
            {
                print("Error in opening communiy cards.... ");
            }
        }
    }

    public int cardOpened;
    public bool isAnim;
    IEnumerator OpenCards()
    {
        print("table.board.Length......" + table.board.Length);
        if (table.board.Length != 0)
        {
            if (table.board.Length >= 3)
            {
                card1 = GameObject.Find(table.board[0]);
                card2 = GameObject.Find(table.board[1]);
                card3 = GameObject.Find(table.board[2]);
            }
            if (table.board.Length >= 4)
            {
                card4 = GameObject.Find(table.board[3]);
            }
            if (table.board.Length == 5)
            {
                card5 = GameObject.Find(table.board[4]);
            }
        }

        if (table.board.Length >= 3)
        {
            if (cardOpened < 3)
            {
                if (PlayerPrefs.GetInt("SoundOffOn") == 0)
                {
                    SoundManager.instance.PlayPomeSound(AudioClipCollection.instance.communityCardOpenSFX);
                }
                card1.transform.SetParent(UIManagerScript.instance.tableParent.GetChild(5).GetChild(0));
                card2.transform.SetParent(UIManagerScript.instance.tableParent.GetChild(5).GetChild(1));
                card3.transform.SetParent(UIManagerScript.instance.tableParent.GetChild(5).GetChild(2));

                card1.transform.localEulerAngles = Vector3.zero;
                card2.transform.localEulerAngles = Vector3.zero;
                card3.transform.localEulerAngles = Vector3.zero;

                if (GameManagerScript.instance.isVideoTable)
                {
                    card1.transform.localScale = new Vector2(0.70f, 0.70f);
                    card2.transform.localScale = new Vector2(0.70f, 0.70f);
                    card3.transform.localScale = new Vector2(0.70f, 0.70f);

                }
                else
                {
                    card1.transform.localScale = new Vector2(0.7f, 0.8f);
                    card2.transform.localScale = new Vector2(0.7f, 0.8f);
                    card3.transform.localScale = new Vector2(0.7f, 0.8f);
                }
                cardOpened = 3;

                if (GameManagerScript.instance.isGameResumed)
                {
                    card1.transform.localPosition = Vector3.zero;
                    card2.transform.localPosition = Vector3.zero;
                    card3.transform.localPosition = Vector3.zero;
                    isAnim = false;
                }
                else
                {
                    isAnim = true;
                    yield return new WaitForSeconds(0.8f);
                    AnimateCommunityCards();
                    yield return new WaitForSeconds(1.5f);

                }
            }
        }

        if (table.board.Length >= 4)
        {
            if (cardOpened < 4)
            {
                if (PlayerPrefs.GetInt("SoundOffOn") == 0)
                {
                    SoundManager.instance.PlayPomeSound(AudioClipCollection.instance.communityCardOpenSFX);
                }
                card4.transform.SetParent(UIManagerScript.instance.tableParent.GetChild(5).GetChild(3));
                card4.transform.localEulerAngles = Vector3.zero;

                if (GameManagerScript.instance.isVideoTable)
                {
                    card4.transform.localScale = new Vector2(0.70f, 0.70f);
                }
                else
                {
                    card4.transform.localScale = new Vector2(0.7f, 0.8f);
                }
                cardOpened = 4;
                if (GameManagerScript.instance.isGameResumed)
                {
                    card4.transform.localPosition = Vector3.zero;
                    isAnim = false;
                }
                else
                {
                    isAnim = true;
                    yield return new WaitForSeconds(0.8f);
                    AnimateCommunityCards();
                    yield return new WaitForSeconds(1.1f);
                }
            }
        }

        if (table.board.Length == 5)
        {
            if (cardOpened < 5)
            {
                if (PlayerPrefs.GetInt("SoundOffOn") == 0)
                {
                    SoundManager.instance.PlayPomeSound(AudioClipCollection.instance.communityCardOpenSFX);
                }
                card5.transform.SetParent(UIManagerScript.instance.tableParent.GetChild(5).GetChild(4));
                card5.transform.localEulerAngles = Vector3.zero;

                if (GameManagerScript.instance.isVideoTable)
                {
                    card5.transform.localScale = new Vector2(0.70f, 0.70f);
                }
                else
                {
                    card5.transform.localScale = new Vector2(0.7f, 0.8f);
                }
                cardOpened = 5;
                if (GameManagerScript.instance.isGameResumed)
                {
                    card5.transform.localPosition = Vector3.zero;
                    isAnim = false;
                }
                else
                {
                    isAnim = true;
                    yield return new WaitForSeconds(0.6f);
                    AnimateCommunityCards();
                }
            }
        }
    }

    IEnumerator VideoCardAnim;
    IEnumerator NonVideoCardAnim;

    public void AnimateCommunityCards()
    {
        if (!GameManagerScript.instance.isVideoTable && isAnim)
        {
            isAnim = false;
            NonVideoCardAnim = AnimateCommunityCardsNonVideo();
            StartCoroutine(NonVideoCardAnim);
        }
        else if (isAnim)
        {
            isAnim = false;
            VideoCardAnim = AnimateCommunityCardsVideo();
            StartCoroutine(VideoCardAnim);
        }
    }

    public void StopCardAnim()
    {
        if (GameManagerScript.instance.isVideoTable)
        {
            StopCoroutine(VideoCardAnim);
        }
        else
        {
            StopCoroutine(NonVideoCardAnim);
        }
        UIManagerScript.instance.animCard1.gameObject.SetActive(false);
        UIManagerScript.instance.animCard4.gameObject.SetActive(false);
        UIManagerScript.instance.animCard5.gameObject.SetActive(false);
    }

    IEnumerator AnimateCommunityCardsNonVideo()
    {
        if (cardOpened == 3)
        {
            UIManagerScript.instance.animCard1.gameObject.SetActive(true);
            UIManagerScript.instance.animCard1.GetChild(0).localEulerAngles = new Vector3(0, 0, 0);
            StartCoroutine(RotationAnimationCoroutine2(UIManagerScript.instance.animCard1.GetChild(0), new Vector3(0, -105, 0), 0.45f));
            yield return new WaitForSeconds(0.4f);
            UIManagerScript.instance.animCard1.gameObject.SetActive(false);

            card1.transform.localPosition = Vector3.zero;
            card1.transform.localEulerAngles = new Vector3(0, 80, 0);
            StartCoroutine(RotationAnimationCoroutine2(card1.transform, new Vector3(0, 0, 0), 0.45f));
            yield return new WaitForSeconds(0.45f);

            card1.transform.localEulerAngles = Vector3.zero;
            card2.transform.localEulerAngles = Vector3.zero;
            card3.transform.localEulerAngles = Vector3.zero;

            AnimateTransformFunctions.ins.AnimateTransform(card2.transform, UIManagerScript.instance.tableParent.GetChild(5).GetChild(0).position, UIManagerScript.instance.tableParent.GetChild(5).GetChild(1).position, 0.3f, AnimateTransformFunctions.TransformTypes.Position, AnimateTransformFunctions.AnimAxis.World, "EaseOut");
            AnimateTransformFunctions.ins.AnimateTransform(card3.transform, UIManagerScript.instance.tableParent.GetChild(5).GetChild(0).position, UIManagerScript.instance.tableParent.GetChild(5).GetChild(2).position, 0.5f, AnimateTransformFunctions.TransformTypes.Position, AnimateTransformFunctions.AnimAxis.World, "EaseOut");
            yield return new WaitForSeconds(0.5f);
            card1.transform.localPosition = Vector3.zero;
            card2.transform.localPosition = Vector3.zero;
            card3.transform.localPosition = Vector3.zero;
        }

        else if (cardOpened == 4)
        {
            UIManagerScript.instance.animCard4.gameObject.SetActive(true);
            UIManagerScript.instance.animCard4.GetChild(0).localEulerAngles = new Vector3(0, 0, 0);
            StartCoroutine(RotationAnimationCoroutine2(UIManagerScript.instance.animCard4.GetChild(0), new Vector3(0, -90, 0), 0.55f));
            yield return new WaitForSeconds(0.55f);
            UIManagerScript.instance.animCard4.gameObject.SetActive(false);

            card4.transform.localPosition = Vector3.zero;
            card4.transform.localEulerAngles = new Vector3(0, 90, 0);
            StartCoroutine(RotationAnimationCoroutine2(card4.transform, new Vector3(0, 0, 0), 0.55f));
        }

        else if (cardOpened == 5)
        {
            UIManagerScript.instance.animCard5.gameObject.SetActive(true);
            UIManagerScript.instance.animCard5.GetChild(0).localEulerAngles = new Vector3(0, 0, 0);
            StartCoroutine(RotationAnimationCoroutine2(UIManagerScript.instance.animCard5.GetChild(0), new Vector3(0, -76, 0), 0.55f));
            yield return new WaitForSeconds(0.55f);
            UIManagerScript.instance.animCard5.gameObject.SetActive(false);

            card5.transform.localPosition = Vector3.zero;
            card5.transform.localEulerAngles = new Vector3(0, 105, 0);
            StartCoroutine(RotationAnimationCoroutine2(card5.transform, new Vector3(0, 0, 0), 0.55f));
        }
    }

    IEnumerator AnimateCommunityCardsVideo()
    {
        if (cardOpened == 3)
        {
            UIManagerScript.instance.animCard1.gameObject.SetActive(true);
            AnimateTransformFunctions.ins.AnimateTransform(UIManagerScript.instance.animCard1.GetChild(0), new Vector3(0, 0, 0), new Vector3(0, -90, 0), 0.45f, AnimateTransformFunctions.TransformTypes.Rotation, AnimateTransformFunctions.AnimAxis.World, "EaseOut");
            yield return new WaitForSeconds(0.45f);
            UIManagerScript.instance.animCard1.gameObject.SetActive(false);
            card1.transform.localPosition = Vector3.zero;
            AnimateTransformFunctions.ins.AnimateTransform(card1.transform, new Vector3(0, 90, 0), new Vector3(0, 0, 0), 0.45f, AnimateTransformFunctions.TransformTypes.Rotation, AnimateTransformFunctions.AnimAxis.World, "EaseOut");
            yield return new WaitForSeconds(0.45f);

            card1.transform.localEulerAngles = Vector3.zero;
            card2.transform.localEulerAngles = Vector3.zero;
            card3.transform.localEulerAngles = Vector3.zero;
            AnimateTransformFunctions.ins.AnimateTransform(card2.transform, UIManagerScript.instance.tableParent.GetChild(5).GetChild(0).position, UIManagerScript.instance.tableParent.GetChild(5).GetChild(1).position, 0.3f, AnimateTransformFunctions.TransformTypes.Position, AnimateTransformFunctions.AnimAxis.World, "EaseOut");
            AnimateTransformFunctions.ins.AnimateTransform(card3.transform, UIManagerScript.instance.tableParent.GetChild(5).GetChild(0).position, UIManagerScript.instance.tableParent.GetChild(5).GetChild(2).position, 0.5f, AnimateTransformFunctions.TransformTypes.Position, AnimateTransformFunctions.AnimAxis.World, "EaseOut");
            yield return new WaitForSeconds(0.5f);

            card1.transform.localPosition = Vector3.zero;
            card2.transform.localPosition = Vector3.zero;
            card3.transform.localPosition = Vector3.zero;
        }

        else if (cardOpened == 4)
        {
            UIManagerScript.instance.animCard4.gameObject.SetActive(true);
            UIManagerScript.instance.animCard4.GetChild(0).localEulerAngles = new Vector3(0, 0, 0);
            StartCoroutine(RotationAnimationCoroutine2(UIManagerScript.instance.animCard4.GetChild(0), new Vector3(0, -90, 0), 0.55f));
            yield return new WaitForSeconds(0.55f);
            UIManagerScript.instance.animCard4.gameObject.SetActive(false);

            card4.transform.localPosition = Vector3.zero;
            card4.transform.localEulerAngles = new Vector3(0, 90, 0);
            StartCoroutine(RotationAnimationCoroutine2(card4.transform, new Vector3(0, 0, 0), 0.55f));
        }

        else if (cardOpened == 5)
        {
            UIManagerScript.instance.animCard5.gameObject.SetActive(true);
            UIManagerScript.instance.animCard5.GetChild(0).localEulerAngles = new Vector3(0, 0, 0);
            StartCoroutine(RotationAnimationCoroutine2(UIManagerScript.instance.animCard5.GetChild(0), new Vector3(0, -90, 0), 0.55f));
            yield return new WaitForSeconds(0.55f);
            UIManagerScript.instance.animCard5.gameObject.SetActive(false);

            card5.transform.localPosition = Vector3.zero;
            card5.transform.localEulerAngles = new Vector3(0, 90, 0);
            StartCoroutine(RotationAnimationCoroutine2(card5.transform, new Vector3(0, 0, 0), 0.55f));
        }
    }

    public IEnumerator RotationAnimationCoroutine2(Transform animateObject, Vector3 TargetRot, float time)
    {
        float animValue;
        float evaluateValue = 0;

        Quaternion startRotation = Quaternion.Euler(animateObject.localEulerAngles);
        Quaternion destinationRotation = Quaternion.Euler(TargetRot);

        while (evaluateValue < 1)
        {
            evaluateValue += 1 / time * Time.deltaTime;
            if (evaluateValue > 1)
            {
                evaluateValue = 1;
            }
            animValue = ChairAnimation.instance.animationCurve.Evaluate(evaluateValue);

            animateObject.localRotation = Quaternion.Lerp(startRotation, destinationRotation, animValue);
            yield return null;
        }
    }

    public void UpdateTotalPot()
    {
        try
        {
            if (!RoundPot.transform.parent.gameObject.activeInHierarchy)
            {
                if (table.totalBet != 0)
                {
                    UIManagerScript.instance.mainPotWin.transform.parent.gameObject.SetActive(false);
                    //RoundPot.transform.parent.gameObject.SetActive(true);
                }
            }

            if (WinningLogic.instance.isRoundPotReset)
            {
                string value = GameManagerScript.instance.KiloFormat(table.totalBet);
                TotalPot.text = value;

                print("TotalPot.text.................." + TotalPot.text);
                if (table.totalBet == 0)
                {
                    TotalPot.text = /*"Pot: "*/"";
                }
                StackCal(table.totalBet);
                if (!GameManagerScript.instance.isVideoTable)
                {
                    Table.instance.TotalPot.text = TotalPot.text.Replace("Pot: ", string.Empty);
                }
            }

            RoundPot.text = TotalPot.text;
            if (table.sidePots.Length != 0)
            {
                string value3 = GameManagerScript.instance.KiloFormat(table.realSidePots[0]);
                RoundPot.text = value3;
            }

        }
        catch
        {
            print("Error in update pot");
        }

        UIManagerScript.instance.totalPotWin.text = TotalPot.text;
        UIManagerScript.instance.mainPotWin.text = RoundPot.text;
        if (RoundPot.transform.parent.gameObject.activeInHierarchy)
        {
            UIManagerScript.instance.mainPotWin.transform.parent.gameObject.SetActive(true);
        }
    }

    public void StackCal(int val)
    {
        int value;
        if (GameManagerScript.instance.isTournament)
        {
            //value = GameSerializeClassesCollection.instance.tournament.buyIn;
            string startValue = SocialGame.instance.ConvertChipsToString(GameSerializeClassesCollection.instance.tournament.new_starting_chips);
            value = int.Parse(startValue);
        }
        else
        {
            value = (int)GameSerializeClassesCollection.instance.enterInSocialGame.buyin;
        }
        int value2 = value / 3;
        int l1 = value / 3;
        int l2 = l1 * 2;
        int l3 = l2 * 3;
        DeactiveStacks();
        if (val > l2)
        {
            stackParent.transform.GetChild(2).gameObject.SetActive(true);
            UIManagerScript.instance.stackParentWin.transform.GetChild(2).gameObject.SetActive(true);
        }
        else if (val > l1)
        {
            stackParent.transform.GetChild(1).gameObject.SetActive(true);
            UIManagerScript.instance.stackParentWin.transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (val < l1)
        {
            stackParent.transform.GetChild(0).gameObject.SetActive(true);
            UIManagerScript.instance.stackParentWin.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            stackParent.transform.GetChild(0).gameObject.SetActive(true);
            UIManagerScript.instance.stackParentWin.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void DeactiveStacks()
    {
        for (int i = 0; i < stackParent.transform.childCount; i++)
        {
            stackParent.transform.GetChild(i).gameObject.SetActive(false);
        }
        UIManagerScript.instance.stackParentWin.transform.GetChild(0).gameObject.SetActive(false);
        UIManagerScript.instance.stackParentWin.transform.GetChild(1).gameObject.SetActive(false);
        UIManagerScript.instance.stackParentWin.transform.GetChild(2).gameObject.SetActive(false);
    }

    public void UpdateRoundPot(int value)
    {
        try
        {
            TotalPot.text = SocialGame.instance.ConvertChipsToString(TotalPot.text);
            float totalPotvalue = float.Parse(TotalPot.text) + value;

            string value2 = GameManagerScript.instance.KiloFormat((int)totalPotvalue);
            TotalPot.text = value2;

            if (!GameManagerScript.instance.isVideoTable)
            {
                TotalPot.text = Table.instance.TotalPot.text.Replace("Pot: ", string.Empty);
            }
            print("Total Pot " + TotalPot.text + " Round Bet " + RoundPot.text);
            StackCal(table.totalBet);
            if (!RoundPot.transform.parent.gameObject.activeInHierarchy)
            {
                RoundPot.text = TotalPot.text;
                if (table.sidePots.Length != 0)
                {
                    string value3 = GameManagerScript.instance.KiloFormat(table.realSidePots[0]);
                    RoundPot.text = value3;
                }
            }
        }
        catch
        {
            print("Error in update pot");
        }
    }

    public void TableSidePots()
    {
        StartCoroutine(DelaySidePot());
    }

    IEnumerator DelaySidePot()
    {
        yield return new WaitForSeconds(0.6f);
        if (table.sidePots.Length != 0)
        {
            RoundPot.transform.parent.gameObject.SetActive(true);
            ResetSidePot();
            for (int i = 0; i < table.sidePots.Length; i++)
            {
                sidePotParent.transform.GetChild(i).gameObject.SetActive(true);
                string value = GameManagerScript.instance.KiloFormat(table.sidePots[i]);
                sidePotParent.transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>().text = value;
                //sidePotParent.transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>().text = "" + table.sidePots[i];

                sidePotParentWin.transform.GetChild(i).gameObject.SetActive(true);
                string value2 = GameManagerScript.instance.KiloFormat(table.sidePots[i]);
                sidePotParentWin.transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>().text = value2;


            }
        }
    }

    public void ResetSidePot()
    {
        for (int i = 0; i < sidePotParent.transform.childCount; i++)
        {
            sidePotParent.transform.GetChild(i).gameObject.SetActive(false);
            sidePotParent.transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>().text = "";
        }

        for (int i = 0; i < sidePotParentWin.transform.childCount; i++)
        {
            sidePotParentWin.transform.GetChild(i).gameObject.SetActive(false);
            sidePotParentWin.transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>().text = "";
        }
    }

    public void UpdateTableInfos(GameSerializeClassesCollection.Basicdata roundStartInfoObj)
    {
        UIManagerScript.instance.tableInfo.GetChild(2).GetChild(0).GetComponent<Text>().text = roundStartInfoObj.table.smallBlind.amount.ToString() + " " + "/" + " " + roundStartInfoObj.table.bigBlind.amount.ToString();
        UIManagerScript.instance.tableInfo.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = roundStartInfoObj.table.smallBlindUp + " " + "/" + " " + roundStartInfoObj.table.bigBlindUp;
    }

    //public void UpdateAllSidePanelValues()
    //{
    //    LargestStack();
    //    SmallestStack();
    //    AverageStack();
    //    TotalBuyIn();
    //    PlayerPosition();
    //}

    //public IEnumerator levelCountDownCorotine;
    //public void LevelAndBlinds(GameSerializeClassesCollection.Basicdata roundStartInfoObj)
    //{
    //    UpdateTableInfos(roundStartInfoObj);

    //    levelCountDownCorotine = RemainingTimerValue((TournamentScript.instance.blindUpVal * 60), UIManagerScript.instance.levelTime);
    //    StartlevelCountDownCorotine();

    //    UIManagerScript.instance.mttSideInfoPanel.transform.GetChild(2).GetChild(1).GetComponent<Text>().text = (roundStartInfoObj.table.currentLevel + 1).ToString();
    //    UIManagerScript.instance.mttSideInfoPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = roundStartInfoObj.table.smallBlindUp + "/" + roundStartInfoObj.table.bigBlindUp + "(" + (roundStartInfoObj.table.currentLevel + 2) + ")";
    //    UIManagerScript.instance.mttSideInfoPanel.transform.GetChild(0).GetChild(3).GetComponent<Text>().text = roundStartInfoObj.table.smallBlind.amount + "/" + roundStartInfoObj.table.bigBlind.amount + "(" + (roundStartInfoObj.table.currentLevel + 1) + ")";
    //    table.currentLevel = roundStartInfoObj.table.currentLevel;
    //}

    //public void LargestStack()
    //{
    //    print("Largest Stack");
    //    UIManagerScript.instance.mttSideInfoPanel.transform.GetChild(4).GetChild(1).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttRankingListingData.data[0].tournament_chips;
    //}

    //public void SmallestStack()
    //{
    //    int i = GameSerializeClassesCollection.instance.mttRankingListingData.data.Length;
    //    UIManagerScript.instance.mttSideInfoPanel.transform.GetChild(5).GetChild(1).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttRankingListingData.data[i-1].tournament_chips;
    //}

    //public void AverageStack()
    //{
    //    int sum = 0;
    //    for (int i = 0; i < GameSerializeClassesCollection.instance.mttRankingListingData.data.Length; i++)
    //    {
    //        sum = sum + int.Parse(GameSerializeClassesCollection.instance.mttRankingListingData.data[i].tournament_chips);
    //    }
    //    try
    //    {
    //        UIManagerScript.instance.mttSideInfoPanel.transform.GetChild(3).GetChild(1).GetComponent<Text>().text = (sum / GameSerializeClassesCollection.instance.mttRankingListingData.data.Length).ToString("F2");
    //    }
    //    catch
    //    {
    //        print("Divide by Zero...");
    //    }
    //}

    //public void TotalBuyIn()
    //{
    //    UIManagerScript.instance.mttSideInfoPanel.transform.GetChild(3).GetChild(4).GetComponent<Text>().text = (TournamentScript.instance.buyInVal * GameSerializeClassesCollection.instance.mttRankingListingData.data.Length).ToString();
    //}

    //public void PlayerPosition()
    //{
    //    for (int i = 0; i < GameManagerScript.instance.playersParent.transform.childCount; i++)
    //    {
    //        print("ClientID: "+GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetComponent<PokerPlayerController>().player.clientId);
    //        if (GameManagerScript.instance.playersParent.transform.GetChild(i).childCount > 1)
    //        {

    //            for (int j = 1; j <= GameSerializeClassesCollection.instance.mttRankingListingData.data.Length; j++)
    //            {
    //                try
    //                {
    //                    if (GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetComponent<PokerPlayerController>().player.clientId == GameSerializeClassesCollection.instance.mttRankingListingData.data[j - 1].client_id)
    //                    {
    //                        UIManagerScript.instance.mttSideInfoPanel.transform.GetChild(1).GetChild(6).GetComponent<Text>().text = j.ToString();
    //                        break;
    //                    }
    //                }
    //                catch
    //                {
    //                    print("Some Error in PlayerPosition Table.cs");
    //                }
    //            }
    //        }
    //    }
    //}

    //public DateTime UnixTimeToDateTime(long unixtime)
    //{
    //    System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
    //    dtDateTime = dtDateTime.AddMilliseconds(unixtime).ToLocalTime();
    //    return dtDateTime;
    //}

    //public IEnumerator RemainingTimerValue(double _totalTableRemainingTime, Text _text)
    //{
    //    int totalTableRemainingTime = (int)_totalTableRemainingTime;
    //    while (true)
    //    {
    //        if (totalTableRemainingTime >= 0)
    //        {
    //            totalTableRemainingTime -= 1;
    //            RemainingDisplayTime(totalTableRemainingTime, _text);
    //            yield return new WaitForSeconds(1);
    //        }

    //        else
    //        {
    //            StoplevelCountDownCorotine();
    //            totalTableRemainingTime = 0;
    //            yield return new WaitForSeconds(1);
    //            StartlevelCountDownCorotine();
    //            print("Repeat....");
    //            break;
    //        }
    //    }
    //}

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
    }

    //public void StartlevelCountDownCorotine()
    //{
    //    print("Null");
    //    if (levelCountDownCorotine != null)
    //    {
    //        print("NotNull");
    //        StartCoroutine(levelCountDownCorotine);
    //    }
    //}

    //public void StoplevelCountDownCorotine()
    //{
    //    if (levelCountDownCorotine != null)
    //    {
    //        StopCoroutine(levelCountDownCorotine);
    //    }
    //}


    public IEnumerator breakCountDownCorotine;
    public IEnumerator RemainingBreakTimerValue(double _totalTableRemainingTime, Text _text)
    {
        int totalTableRemainingTime = (int)_totalTableRemainingTime;
        while (true)
        {
            if (totalTableRemainingTime >= 0)
            {
                totalTableRemainingTime -= 1;
                RemainingBreakDisplayTime(totalTableRemainingTime, _text);
                yield return new WaitForSeconds(1);
            }

            else
            {
                print("End....");
                _text.text = "00:00:00";
                UIManagerScript.instance.breakTimePanel.SetActive(false);
                StopBreakCountDownCorotine();
                break;
            }
        }
    }

    void RemainingBreakDisplayTime(double _timeToDisplay, Text _text)
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
    }

    public void StartBreakCountDownCorotine()
    {
        if (breakCountDownCorotine != null)
        {
            StartCoroutine(breakCountDownCorotine);
        }
    }

    public void StopBreakCountDownCorotine()
    {
        if (breakCountDownCorotine != null)
        {
            StopCoroutine(breakCountDownCorotine);
            UIManagerScript.instance.addOnRebuyButtonPanelTournment.GetChild(0).GetChild(1).gameObject.SetActive(false);
            UIManagerScript.instance.addOnRebuyButtonPanelTournment.gameObject.SetActive(false);
        }
    }
    string tableNo;
    public void GetDynamicTableNumber(GameObject panel)
    {
        tableNo = panel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text;
        print(tableNo);
        TournamentManagerScript.instance.ObserveTournament2(tableNo);
    }

    void OpenFourCards()
    {
        card1 = GameObject.Find(table.board[0]);
        card2 = GameObject.Find(table.board[1]);
        card3 = GameObject.Find(table.board[2]);
        card4 = GameObject.Find(table.board[3]);

        card1.transform.SetParent(UIManagerScript.instance.tableParent.GetChild(5).GetChild(0));
        card2.transform.SetParent(UIManagerScript.instance.tableParent.GetChild(5).GetChild(1));
        card3.transform.SetParent(UIManagerScript.instance.tableParent.GetChild(5).GetChild(2));
        card4.transform.SetParent(UIManagerScript.instance.tableParent.GetChild(5).GetChild(3));

        card1.transform.localPosition = Vector3.zero;
        card2.transform.localPosition = Vector3.zero;
        card3.transform.localPosition = Vector3.zero;
        card4.transform.localPosition = Vector3.zero;

        card1.transform.localEulerAngles = Vector3.zero;
        card2.transform.localEulerAngles = Vector3.zero;
        card3.transform.localEulerAngles = Vector3.zero;
        card4.transform.localEulerAngles = Vector3.zero;

        if (GameManagerScript.instance.isVideoTable)
        {
            card1.transform.localScale = new Vector2(0.70f, 0.70f);
            card2.transform.localScale = new Vector2(0.70f, 0.70f);
            card3.transform.localScale = new Vector2(0.70f, 0.70f);
            card4.transform.localScale = new Vector2(0.70f, 0.70f);
        }
        else
        {
            card1.transform.localScale = new Vector2(0.7f, 0.8f);
            card2.transform.localScale = new Vector2(0.7f, 0.8f);
            card3.transform.localScale = new Vector2(0.7f, 0.8f);
            card4.transform.localScale = new Vector2(0.7f, 0.8f);
        }
    }

    void OpenFiveCards()
    {
        card1 = GameObject.Find(table.board[0]);
        card2 = GameObject.Find(table.board[1]);
        card3 = GameObject.Find(table.board[2]);
        card4 = GameObject.Find(table.board[3]);
        card5 = GameObject.Find(table.board[4]);

        card1.transform.SetParent(UIManagerScript.instance.tableParent.GetChild(5).GetChild(0));
        card2.transform.SetParent(UIManagerScript.instance.tableParent.GetChild(5).GetChild(1));
        card3.transform.SetParent(UIManagerScript.instance.tableParent.GetChild(5).GetChild(2));
        card4.transform.SetParent(UIManagerScript.instance.tableParent.GetChild(5).GetChild(3));
        card5.transform.SetParent(UIManagerScript.instance.tableParent.GetChild(5).GetChild(4));

        card1.transform.localPosition = Vector3.zero;
        card2.transform.localPosition = Vector3.zero;
        card3.transform.localPosition = Vector3.zero;
        card4.transform.localPosition = Vector3.zero;
        card5.transform.localPosition = Vector3.zero;

        card1.transform.localEulerAngles = Vector3.zero;
        card2.transform.localEulerAngles = Vector3.zero;
        card3.transform.localEulerAngles = Vector3.zero;
        card4.transform.localEulerAngles = Vector3.zero;
        card5.transform.localEulerAngles = Vector3.zero;

        if (GameManagerScript.instance.isVideoTable)
        {
            card1.transform.localScale = new Vector2(0.70f, 0.70f);
            card2.transform.localScale = new Vector2(0.70f, 0.70f);
            card3.transform.localScale = new Vector2(0.70f, 0.70f);
            card4.transform.localScale = new Vector2(0.70f, 0.70f);
            card5.transform.localScale = new Vector2(0.70f, 0.70f);
        }
        else
        {
            card1.transform.localScale = new Vector2(0.7f, 0.8f);
            card2.transform.localScale = new Vector2(0.7f, 0.8f);
            card3.transform.localScale = new Vector2(0.7f, 0.8f);
            card4.transform.localScale = new Vector2(0.7f, 0.8f);
            card5.transform.localScale = new Vector2(0.7f, 0.8f);
        }
    }

    void OpenTwoCards()
    {
        card4 = GameObject.Find(table.board[3]);
        card4.transform.SetParent(UIManagerScript.instance.tableParent.GetChild(5).GetChild(3));
        card4.transform.localPosition = Vector3.zero;
        card4.transform.localEulerAngles = Vector3.zero;

        card5 = GameObject.Find(table.board[4]);
        card5.transform.SetParent(UIManagerScript.instance.tableParent.GetChild(5).GetChild(4));
        card5.transform.localPosition = Vector3.zero;
        card5.transform.localEulerAngles = Vector3.zero;
        if (GameManagerScript.instance.isVideoTable)
        {
            card4.transform.localScale = new Vector2(0.70f, 0.70f);
            card5.transform.localScale = new Vector2(0.70f, 0.70f);
        }

        else
        {
            card4.transform.localScale = new Vector2(0.7f, 0.8f);
            card5.transform.localScale = new Vector2(0.7f, 0.8f);
        }
    }

    #region Tournament InnerPanelRanking Listing
    public void InnerPanelRankingListing()
    {
        if (GameSerializeClassesCollection.instance.mttRankingListingData.data.Length != UIManagerScript.instance.innerPanelrankCount)
        {
            for (int i = UIManagerScript.instance.innerPanelrankCount; i < GameSerializeClassesCollection.instance.mttRankingListingData.data.Length; i++)
            {
                UIManagerScript.instance.innerPanelrankCount++;
                GenerateRankingMembersItem();
            }
        }

        for (int i = 0; i < GameSerializeClassesCollection.instance.mttRankingListingData.data.Length; i++)
        {
            UIManagerScript.instance.innerPanelRankListingContent.GetChild(i).GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
            UIManagerScript.instance.innerPanelRankListingContent.GetChild(i).GetChild(1).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttRankingListingData.data[i].username;
            //UIManagerScript.instance.innerPanelRankListingContent.GetChild(i).GetChild(2).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttRankingListingData.data[i].tournament_chips;
            //UIManagerScript.instance.innerPanelRankListingContent.GetChild(i).GetChild(3).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttRankingListingData.data[i].no_addon_times + "A" + "+" + GameSerializeClassesCollection.instance.mttRankingListingData.data[i].no_rebuy_times + "R";

            UIManagerScript.instance.innerPanelRankListingContent.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void GenerateRankingMembersItem()
    {
        UIManagerScript.instance.innerPanelScrollRankItemObj = Instantiate(UIManagerScript.instance.innerPanelRankListingPanel.gameObject);
        UIManagerScript.instance.innerPanelScrollRankItemObj.transform.SetParent(UIManagerScript.instance.innerPanelRankListingContent, false);
        UIManagerScript.instance.innerPanelRankList.Add(UIManagerScript.instance.innerPanelScrollRankItemObj);
    }

    public void ResetRankList()
    {
        if (UIManagerScript.instance.innerPanelRankList.Count > 0)
        {
            for (int i = 0; i < UIManagerScript.instance.innerPanelRankList.Count; i++)
            {
                Destroy(UIManagerScript.instance.innerPanelRankList[i]);
            }
            UIManagerScript.instance.innerPanelRankList.Clear();
            UIManagerScript.instance.innerPanelrankCount = 1;
        }
        for (int i = 0; i < UIManagerScript.instance.innerPanelRankListingContent.transform.childCount; i++)
        {
            UIManagerScript.instance.innerPanelRankListingContent.GetChild(i).gameObject.SetActive(false);
        }
    }
    #endregion

    #region Tournament InnerPanelTable Listing
    public void InnerPanelTableListing()
    {
        if (GameSerializeClassesCollection.instance.mttTableListingData.data.Length != UIManagerScript.instance.innerPanelTableCount)
        {
            for (int i = UIManagerScript.instance.innerPanelTableCount; i < GameSerializeClassesCollection.instance.mttTableListingData.data.Length; i++)
            {
                UIManagerScript.instance.innerPanelTableCount++;
                GenerateTableMembersItem();
            }
        }

        for (int i = 0; i < GameSerializeClassesCollection.instance.mttTableListingData.data.Length; i++)
        {
            //UIManagerScript.instance.innerPanelTableListingContent.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttTableListingData.data[i].table_no;
            //UIManagerScript.instance.innerPanelTableListingContent.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttTableListingData.data[i].current_players;
            //UIManagerScript.instance.innerPanelTableListingContent.GetChild(i).GetChild(2).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttTableListingData.data[i].min_chips;
            //UIManagerScript.instance.innerPanelTableListingContent.GetChild(i).GetChild(2).GetChild(1).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttTableListingData.data[i].max_chips;

            //UIManagerScript.instance.innerPanelTableListingContent.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void GenerateTableMembersItem()
    {
        UIManagerScript.instance.scrollInnerPanelTableItemObj = Instantiate(UIManagerScript.instance.innerPanelTableListingPanel.gameObject);
        UIManagerScript.instance.scrollInnerPanelTableItemObj.transform.SetParent(UIManagerScript.instance.innerPanelTableListingContent, false);
        UIManagerScript.instance.innerPanelTableList.Add(UIManagerScript.instance.scrollInnerPanelTableItemObj);
    }

    public void ResetTableList()
    {
        if (UIManagerScript.instance.innerPanelTableList.Count > 0)
        {
            for (int i = 0; i < UIManagerScript.instance.innerPanelTableList.Count; i++)
            {
                Destroy(UIManagerScript.instance.innerPanelTableList[i]);
            }
            UIManagerScript.instance.innerPanelTableList.Clear();
            UIManagerScript.instance.innerPanelTableCount = 1;
        }
        for (int i = 0; i < UIManagerScript.instance.innerPanelTableListingContent.transform.childCount; i++)
        {
            UIManagerScript.instance.innerPanelTableListingContent.GetChild(i).gameObject.SetActive(false);
        }
    }
    #endregion

    #region Tournament InnerPanelRewards Listing
    public void RewardsListing()
    {
        print("REWARDS1....");
        if (GameSerializeClassesCollection.instance.mttRewardListingData.data.Length != UIManagerScript.instance.innerPanelPrizeCount)
        {
            for (int i = UIManagerScript.instance.innerPanelPrizeCount; i < GameSerializeClassesCollection.instance.mttRewardListingData.data.Length; i++)
            {
                UIManagerScript.instance.innerPanelPrizeCount++;
                GenerateRewardsMembersItem();
            }
        }

        for (int i = 0; i < GameSerializeClassesCollection.instance.mttRewardListingData.data.Length; i++)
        {
            print("REWARDS2....");
            UIManagerScript.instance.innerPanelPrizeListingContent.GetChild(i).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttRewardListingData.data[i].rank.ToString();
            UIManagerScript.instance.innerPanelPrizeListingContent.GetChild(i).GetChild(1).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttRewardListingData.data[i].payout;

            UIManagerScript.instance.innerPanelPrizeListingContent.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void GenerateRewardsMembersItem()
    {
        UIManagerScript.instance.scrollInnerPanelPrizeItemObj = Instantiate(UIManagerScript.instance.innerPanelPrizeListingPanel.gameObject);
        UIManagerScript.instance.scrollInnerPanelPrizeItemObj.transform.SetParent(UIManagerScript.instance.innerPanelPrizeListingContent, false);
        UIManagerScript.instance.innerPanelPrizeList.Add(UIManagerScript.instance.scrollInnerPanelPrizeItemObj);
    }

    public void ResetRewardsList()
    {
        if (UIManagerScript.instance.innerPanelPrizeList.Count > 0)
        {
            for (int i = 0; i < UIManagerScript.instance.innerPanelPrizeList.Count; i++)
            {
                Destroy(UIManagerScript.instance.innerPanelPrizeList[i]);
            }
            UIManagerScript.instance.innerPanelPrizeList.Clear();
            UIManagerScript.instance.innerPanelPrizeCount = 1;
        }
        for (int i = 0; i < UIManagerScript.instance.innerPanelPrizeListingContent.transform.childCount; i++)
        {
            UIManagerScript.instance.innerPanelPrizeListingContent.GetChild(i).gameObject.SetActive(false);
        }
    }
    #endregion

    #region Tournament InnerPanelBlinds Listing
    public void BlindsListing()
    {
        print("blinds1....");
        if (GameSerializeClassesCollection.instance.mttBlindsListingData.blinds.Length != UIManagerScript.instance.innerPanelBlindsCount)
        {
            for (int i = UIManagerScript.instance.innerPanelBlindsCount; i < GameSerializeClassesCollection.instance.mttBlindsListingData.blinds.Length; i++)
            {
                UIManagerScript.instance.innerPanelBlindsCount++;
                GenerateBlindsMembersItem();
            }
        }

        for (int i = 0; i < GameSerializeClassesCollection.instance.mttBlindsListingData.blinds.Length; i++)
        {
            print("blinds2....");
            UIManagerScript.instance.innerPanelBlindsListingContent.GetChild(i).GetChild(1).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttBlindsListingData.blinds[i].sb.ToString() + "/" + GameSerializeClassesCollection.instance.mttBlindsListingData.blinds[i].bb.ToString();
            UIManagerScript.instance.innerPanelBlindsListingContent.GetChild(i).GetChild(0).GetComponent<Text>().text = (i + 1).ToString();

            UIManagerScript.instance.innerPanelBlindsListingContent.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void GenerateBlindsMembersItem()
    {
        UIManagerScript.instance.scrollInnerPanelBlindsItemObj = Instantiate(UIManagerScript.instance.innerPanelBlindsListingPanel.gameObject);
        UIManagerScript.instance.scrollInnerPanelBlindsItemObj.transform.SetParent(UIManagerScript.instance.innerPanelBlindsListingContent, false);
        UIManagerScript.instance.innerPanelBlindsList.Add(UIManagerScript.instance.scrollInnerPanelBlindsItemObj);
    }

    public void ResetBlindsList()
    {
        if (UIManagerScript.instance.innerPanelBlindsList.Count > 0)
        {
            for (int i = 0; i < UIManagerScript.instance.innerPanelBlindsList.Count; i++)
            {
                Destroy(UIManagerScript.instance.innerPanelBlindsList[i]);
            }
            UIManagerScript.instance.innerPanelBlindsList.Clear();
            UIManagerScript.instance.innerPanelBlindsCount = 1;
        }
        for (int i = 0; i < UIManagerScript.instance.innerPanelBlindsListingContent.transform.childCount; i++)
        {
            UIManagerScript.instance.innerPanelBlindsListingContent.GetChild(i).gameObject.SetActive(false);
        }
    }
    #endregion
}
