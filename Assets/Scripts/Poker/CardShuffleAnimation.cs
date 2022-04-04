using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardShuffleAnimation : MonoBehaviour
{
    public static CardShuffleAnimation instance;
    public Transform cardStackPosition;
    public Transform cardToAnimate;
    public List<GameObject> totalPlayersCommonUI;

    public Transform panel;
    public bool isAnimationComplete;

    public int shuffleCardCount = 0;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

        checkfriendStatusURL = ServerChanger.instance.domainURL + "api/v1/user/check-friend-status";

        isAnimationComplete = true;
        // StartCardShuffle();
        //AllPlayers();
        //ProfilePanelFlip(panel);
    }

    //public void AllPlayers()
    //{
    //    for (int i = 0; i < GameManagerScript.instance.playersParent.transform.childCount; i++)
    //    {
    //        if (GameManagerScript.instance.playersParent.transform.GetChild(i).childCount > 1)
    //        {
    //            totalPlayersCommonUI.Add(GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(1).gameObject);
    //        }
    //    }
    //}

    int cardPosition;

    void CheckCards()
    {
        if (GameManagerScript.instance.isVideoTable)
        {
            cardPosition = 17;
        }

        else
        {
            cardPosition = 9;
        }
    }

    IEnumerator CardShuffleCouroutine()
    {
        isAnimationComplete = false;
        print("Card Animation starts");
        //while (true)
        //{
        //    if (GameManagerScript.instance.isShuffflingComplete)
        //    {
        //        break;
        //    }
        //    yield return new WaitForSeconds(0.1f);
        //}
        CheckCards();
        yield return new WaitForSeconds(0.2f);
        print(" totalPlayersCommonUI.Count" + totalPlayersCommonUI.Count);
        cardToAnimate.gameObject.SetActive(true);                // CARD ANIM

        for (int i = 0; i < totalPlayersCommonUI.Count; i++)
        {
            if (PlayerPrefs.GetInt("SoundOffOn") == 0)
            {
                SoundManager.instance.PlayPomeSound(AudioClipCollection.instance.shuffleSFX);
            }// CARD ANIM
            AnimateTransformFunctions.ins.AnimateTransform(cardToAnimate, cardStackPosition.position, totalPlayersCommonUI[i].transform.GetChild(cardPosition).position, 0.1f, AnimateTransformFunctions.TransformTypes.Position, AnimateTransformFunctions.AnimAxis.World, "EaseIn");      // CARD ANIM
            yield return new WaitForSeconds(0.1f);

            //  SoundManager.instance.PlayPomeSound(AudioClipCollection.instance.shuffleSFX);
            // AnimateTransformFunctions.ins.AnimateTransform(cardToAnimate, cardStackPosition.position, totalPlayersCommonUI[i].transform.GetChild(cardPosition).position, 0.2f, AnimateTransformFunctions.TransformTypes.Position, AnimateTransformFunctions.AnimAxis.World, "EaseIn");
            // yield return new WaitForSeconds(0.2f);
            // totalPlayersCommonUI[i].transform.GetChild(cardPosition).gameObject.SetActive(true);

            totalPlayersCommonUI[i].transform.GetChild(cardPosition).gameObject.SetActive(true);

        }
        print(" totalPlayersCommonUI.Count 2" + totalPlayersCommonUI.Count);
        for (int i = 0; i < totalPlayersCommonUI.Count; i++)
        {
            if (PlayerPrefs.GetInt("SoundOffOn") == 0)
            {
                SoundManager.instance.PlayPomeSound(AudioClipCollection.instance.shuffleSFX);
            }
            // CARD ANIM
            AnimateTransformFunctions.ins.AnimateTransform(cardToAnimate, cardStackPosition.position, totalPlayersCommonUI[i].transform.GetChild(cardPosition).position, 0.1f, AnimateTransformFunctions.TransformTypes.Position, AnimateTransformFunctions.AnimAxis.World, "EaseIn");    // CARD ANIM
            yield return new WaitForSeconds(0.1f);

          //  SoundManager.instance.PlayPomeSound(AudioClipCollection.instance.shuffleSFX);
           // AnimateTransformFunctions.ins.AnimateTransform(cardToAnimate, cardStackPosition.position, totalPlayersCommonUI[i].transform.GetChild(cardPosition).position, 0.2f, AnimateTransformFunctions.TransformTypes.Position, AnimateTransformFunctions.AnimAxis.World, "EaseIn");
          //  yield return new WaitForSeconds(0.2f);

            totalPlayersCommonUI[i].transform.GetChild(cardPosition).GetChild(0).gameObject.SetActive(true);
           

        }
        cardToAnimate.gameObject.SetActive(false);
        isAnimationComplete = true;
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < GameManagerScript.instance.playersParent.transform.childCount; i++)
        {
            if (GameManagerScript.instance.playersParent.transform.GetChild(i).childCount > 1)
            {
                try
                {
                    GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).transform.GetComponent<PokerPlayerController>().StartSecondHand();
                }
                catch
                {
                    print("Some Error in CardShuffleCouroutine CardShuffleAnimation.cs");
                }
            }
        }
        //totalPlayersCommonUI[0].transform.GetChild(cardPosition).gameObject.SetActive(false);
        print("Card Animation ends");
    }

    public void StartCardShuffle()
    {
        print("StartCardShuffle :  " + shuffleCardCount);
        //if (shuffleCardCount == 0)
        //{
        //    totalPlayersCommonUI.Clear();
        //    AllPlayers();
        //}


        StartCoroutine(CardShuffleCouroutine());
        
    }


    public void ProfilePanelFlip(Transform profilePanel)
    {
        if (Table.instance.table.status != 0)
        {
            profileImage = profilePanel.GetComponent<PokerPlayerController>().profileImg;
            StartCoroutine(ProfilePanelFlipCoroutine(profilePanel.GetComponent<PokerPlayerController>().commonUi.transform.GetChild(15)));
            profilePanel.GetComponent<PokerPlayerController>().commonUi.transform.GetChild(15).GetChild(1).GetComponent<Text>().text = profilePanel.GetComponent<PokerPlayerController>().player.playerName;
            profilePanel.GetComponent<PokerPlayerController>().commonUi.transform.GetChild(15).GetChild(3).GetComponent<Text>().text = "" + profilePanel.GetComponent<PokerPlayerController>().player.clientId;
         
            //GameSerializeClassesCollection.instance.clubId.club_id = ClubManagement.instance._clubID;
            GameSerializeClassesCollection.instance.clubId.username = profilePanel.GetComponent<PokerPlayerController>().player.playerName;
            string body = JsonUtility.ToJson(GameSerializeClassesCollection.instance.clubId);
            print(body);
            localProfilePanel = profilePanel.GetComponent<PokerPlayerController>().commonUi.transform.GetChild(15).gameObject;
            ShowFriendStatus(profilePanel.GetComponent<PokerPlayerController>());
            if (profilePanel.GetComponent<PokerPlayerController>().isLocalPlayer)
            {
                localProfilePanel.transform.GetChild(17).GetChild(0).gameObject.SetActive(true);
                localProfilePanel.transform.GetChild(17).GetChild(1).gameObject.SetActive(false);
                localProfilePanel.transform.GetChild(17).GetChild(2).gameObject.SetActive(false);
                localProfilePanel.transform.GetChild(17).GetChild(3).gameObject.SetActive(false);
            }
            else
            {
                localProfilePanel.transform.GetChild(17).GetChild(0).gameObject.SetActive(false);
                localProfilePanel.transform.GetChild(17).GetChild(1).gameObject.SetActive(true);
                localProfilePanel.transform.GetChild(17).GetChild(2).gameObject.SetActive(true);
                localProfilePanel.transform.GetChild(17).GetChild(3).gameObject.SetActive(true);
            }
            Communication.instance.GetImage(profilePanel.GetComponent<PokerPlayerController>().player.user_image, ProfileImageCallback);
            Communication.instance.PostData(UIManagerScript.instance.statsUrl, body, OpenProfilePanel);
        }
    }

    Transform profileImage;
    void ProfileImageCallback(Sprite sprite)
    {
        if (sprite != null)
        {
            print("ProfileImage texture");
            profileImage.GetChild(0).GetChild(0).GetComponent<Image>().sprite = sprite;
        }
    }

    GameObject localProfilePanel;


    public void OpenProfilePanel(string response)
    {
        if (string.IsNullOrEmpty(response))
        {
            print("Some error!!.");
        }
        else
        {
            print("response" + response);

            GameSerializeClassesCollection.instance.playerStats = JsonUtility.FromJson<GameSerializeClassesCollection.PlayerStats>(response);
            if (GameManagerScript.instance.isTournament)
            {

                localProfilePanel.transform.GetChild(6).GetComponent<Text>().text = GameSerializeClassesCollection.instance.playerStats.vpip_tournament + "%";
                localProfilePanel.transform.GetChild(8).GetComponent<Text>().text = GameSerializeClassesCollection.instance.playerStats.pfr_tournament + "%";
                localProfilePanel.transform.GetChild(10).GetComponent<Text>().text = GameSerializeClassesCollection.instance.playerStats.threeBet_tournament + "%";
                localProfilePanel.transform.GetChild(12).GetComponent<Text>().text = GameSerializeClassesCollection.instance.playerStats.cbet_tournament + "%";
            }
            else
            {
                localProfilePanel.transform.GetChild(6).GetComponent<Text>().text = GameSerializeClassesCollection.instance.playerStats.vpip + "%";
                localProfilePanel.transform.GetChild(8).GetComponent<Text>().text = GameSerializeClassesCollection.instance.playerStats.prf + "%";
                localProfilePanel.transform.GetChild(10).GetComponent<Text>().text = GameSerializeClassesCollection.instance.playerStats.threeBet + "%";
                localProfilePanel.transform.GetChild(12).GetComponent<Text>().text = GameSerializeClassesCollection.instance.playerStats.cbet + "%";
            }

            localProfilePanel.transform.GetChild(4).GetComponent<Text>().text = GameSerializeClassesCollection.instance.playerStats.country;
        }
    }


    //public void OpenProfilePanel(string response)
    //{
    //    if (string.IsNullOrEmpty(response))
    //    {
    //        print("Some error!!.");
    //    }
    //    else
    //    {
    //        print("response" + response);

    //        GameSerializeClassesCollection.instance.playerStats = JsonUtility.FromJson<GameSerializeClassesCollection.PlayerStats>(response);
    //        localProfilePanel.transform.GetChild(6).GetComponent<Text>().text = "" + GameSerializeClassesCollection.instance.playerStats.vpip + "%";
    //        localProfilePanel.transform.GetChild(8).GetComponent<Text>().text = "" + GameSerializeClassesCollection.instance.playerStats.pfr + "%";
    //        localProfilePanel.transform.GetChild(10).GetComponent<Text>().text = "" + GameSerializeClassesCollection.instance.playerStats.bet + "%";
    //        localProfilePanel.transform.GetChild(12).GetComponent<Text>().text = "" + GameSerializeClassesCollection.instance.playerStats.c_bet + "%";
    //        localProfilePanel.transform.GetChild(12).GetComponent<Text>().text = "" + GameSerializeClassesCollection.instance.playerStats.c_bet + "%";
    //        localProfilePanel.transform.GetChild(4).GetComponent<Text>().text = "" + GameSerializeClassesCollection.instance.playerStats.country;
    //    }
    //}

    IEnumerator ProfilePanelFlipCoroutine(Transform profilePanel)
    {
        //yield return new WaitForSeconds(0.2f);
        if (!profilePanel.gameObject.activeInHierarchy)
        {

            profilePanel.gameObject.SetActive(true);
            AnimateTransformFunctions.ins.AnimateTransform(profilePanel, new Vector3(0, 90, 0), new Vector3(0, 0, 0), 0.5f, AnimateTransformFunctions.TransformTypes.Rotation, AnimateTransformFunctions.AnimAxis.Local, "Linear");
        }
        else
        {
            AnimateTransformFunctions.ins.AnimateTransform(profilePanel, new Vector3(0, 0, 0), new Vector3(0, 90, 0), 0.5f, AnimateTransformFunctions.TransformTypes.Rotation, AnimateTransformFunctions.AnimAxis.Local, "Linear");
            yield return new WaitForSeconds(0.52f);
            profilePanel.gameObject.SetActive(false);
        }
    }


    #region AddingFriendButton functionality for video table....

    PokerPlayerController currentController;

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
        currentController = controller;
        RecipientClass reference = new RecipientClass();
        reference.client_id = "" + controller.player.clientId;

        string data = JsonUtility.ToJson(reference);
        Debug.Log(">>chkstatusresponse" + data);




        if (controller.isLocalPlayer)
        {
            // NonVideoProfilePanel.transform.GetChild(0).GetChild(15).gameObject.SetActive(false);
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

                        localProfilePanel.transform.GetChild(17).GetChild(2).GetComponent<Button>().interactable = false;
                        localProfilePanel.transform.GetChild(17).GetChild(2).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                        localProfilePanel.transform.GetChild(17).GetChild(2).gameObject.transform.GetChild(1).gameObject.SetActive(false);
                        localProfilePanel.transform.GetChild(17).GetChild(2).gameObject.transform.GetChild(2).gameObject.SetActive(true);

                    }
                    else if (obj.data[0].status == 1)
                    {

                        // user is already a friend..

                        localProfilePanel.transform.GetChild(17).GetChild(2).GetComponent<Button>().interactable = false;
                        localProfilePanel.transform.GetChild(17).GetChild(2).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                        localProfilePanel.transform.GetChild(17).GetChild(2).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        localProfilePanel.transform.GetChild(17).GetChild(2).gameObject.transform.GetChild(2).gameObject.SetActive(false);

                    }
                    else
                        print(">>Status Data is incorrect...");
                }
                else
                {
                    //user is not a friend...


                    localProfilePanel.transform.GetChild(17).GetChild(2).GetComponent<Button>().interactable = true;
                    localProfilePanel.transform.GetChild(17).GetChild(2).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    localProfilePanel.transform.GetChild(17).GetChild(2).gameObject.transform.GetChild(1).gameObject.SetActive(false);
                    localProfilePanel.transform.GetChild(17).GetChild(2).gameObject.transform.GetChild(2).gameObject.SetActive(false);
                }
                // NonVideoProfilePanel.transform.GetChild(0).GetChild(15).gameObject.SetActive(true);
            }
            else
            {

                print(">>no response Data is from server...");

            }

        }
        else
        { print(">>no response Data is from server..."); }

    }


    public void OnClickAddFriendBtn()
    {
        localProfilePanel.transform.GetChild(17).GetChild(2).GetComponent<Button>().interactable = false;
        localProfilePanel.transform.GetChild(17).GetChild(2).gameObject.transform.GetChild(0).gameObject.SetActive(false);
        localProfilePanel.transform.GetChild(17).GetChild(2).gameObject.transform.GetChild(1).gameObject.SetActive(false);
        localProfilePanel.transform.GetChild(17).GetChild(2).gameObject.transform.GetChild(2).gameObject.SetActive(true);
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
            TournamentManagerScript.instance.AddFriendOnTable(currentController.player.playerName, "" + currentController.player.clientId, senderName, senderId);
        else
            PokerNetworkManager.instance.AddFriendOnTable(currentController.player.playerName, "" + currentController.player.clientId, senderName, senderId);


    }



    #endregion

}
