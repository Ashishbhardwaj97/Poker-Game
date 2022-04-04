using SmartLocalization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeadershipScript : MonoBehaviour
{
    public static LeadershipScript instance;

    

    [Header("GameObject Reference")]
    public GameObject leaderWinningContent;
    public GameObject leaderGamesContent;
    public GameObject winningBtn;
    public GameObject winningScrollView;
    public GameObject winningImage;
    public GameObject gamesBtn;
    public GameObject gamesScrollView;
    public GameObject gamesImage;

    public GameObject tourneyWinningContent;
    public GameObject tourneyPlayedContent;
    public GameObject totalCashesContent;

    public GameObject cashGameScrollUI;
    public GameObject tournamentScrollUI;

    [Header("Transform Reference")]
    public Transform cashGameBtn;
    public Transform tournamentBtn;

    [Header("List GameObject Reference")]
    public List<GameObject> tournamentScrollCommonUI;
    public List<GameObject> tournamentImagesCommonUI;
    public List<Transform> tournamentTabBtn;

    private string cashGameUrl;
    private string tournamentGameUrl;

    private void Awake()
    {
        instance = this;
    }
   
    void Start()
    {
        cashGameUrl = ServerChanger.instance.domainURL + "api/v1/game/leader-board";
        tournamentGameUrl = ServerChanger.instance.domainURL + "api/v1/game/tourney-leader-board";
    }

    #region Cash Games Leaderboard

    [Serializable]
    public class LeadershipResponse
    {
        public bool error;
        public Winning[] wining;
        public Games[] played;
    }

    [Serializable]
    public class Winning
    {
        public string user_image;
        public string username;
        public string first_name;
        public string last_name;
        public string city;
        public string country;
        public int winner_chips;
        public string chips;
    }

    [Serializable]
    public class Games
    {
        public int nlh_hands;
        public string user_image;
        public string username;
        public string first_name;
        public string last_name;
        public string city;
        public string country;
    }

    [Header("List Properties")]
    [SerializeField] LeadershipResponse leadershipResponse;

    public void LeaderBoardRequest()
    {
        ClubManagement.instance.loadingPanel.SetActive(true);

        Communication.instance.PostData(cashGameUrl, LeaderBoardCallback);
    }

    void LeaderBoardCallback(string response)
    {
        print("res : "+response);
        ClubManagement.instance.loadingPanel.SetActive(false);

        if (string.IsNullOrEmpty(response))
        {
            Cashier.instance.toastMsg.text = LanguageManager.Instance.GetTextValue("internet check");//SocialProfile._instance.networkErrorMsg;
            Cashier.instance.toastMsgPanel.SetActive(true);
            SocialPokerGameManager.instance.ClickHome();
        }

        else
        {
            leadershipResponse = JsonUtility.FromJson<LeadershipResponse>(response);

            if (!leadershipResponse.error)
            {
                print("successfull.....");

                SocialPokerGameManager.instance.EnableLeadersPage();
                for (int i = 0; i < leaderWinningContent.transform.childCount; i++)
                {
                    leaderWinningContent.transform.GetChild(i).gameObject.SetActive(false);
                }
                userImage.Clear();
                for (int i = 0; i < leadershipResponse.wining.Length; i++)
                {
                    if (i < 10)
                    {
                        leaderWinningContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text = leadershipResponse.wining[i].username;
                        leaderWinningContent.transform.GetChild(i).GetChild(4).GetComponent<Text>().text = leadershipResponse.wining[i].city + ", " + leadershipResponse.wining[i].country;
                        leaderWinningContent.transform.GetChild(i).GetChild(5).GetComponent<Text>().text = leadershipResponse.wining[i].chips.ToString();
                        leaderWinningContent.transform.GetChild(i).gameObject.SetActive(true);
                        userImage.Add(leadershipResponse.wining[i].user_image);
                    }

                }
                UpdatePlayerImage();

                for (int i = 0; i < leaderGamesContent.transform.childCount; i++)
                {
                    leaderGamesContent.transform.GetChild(i).gameObject.SetActive(false);
                }
                userImage1.Clear();
                for (int i = 0; i < leadershipResponse.played.Length; i++)
                {
                    if (i < 10)
                    {
                        leaderGamesContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text = leadershipResponse.played[i].username;
                        //leaderGamesContent.transform.GetChild(i).GetChild(4).GetComponent<Text>().text = leadershipResponse.played[i].city + ", " + leadershipResponse.wining[i].country;
                        leaderGamesContent.transform.GetChild(i).GetChild(5).GetComponent<Text>().text = leadershipResponse.played[i].nlh_hands.ToString();
                        leaderGamesContent.transform.GetChild(i).gameObject.SetActive(true);
                        userImage1.Add(leadershipResponse.played[i].user_image);
                    }
                }
                UpdatePlayerImage1();
            }
        }
    }

    #region update Player Image

    public List<string> userImage;

    [SerializeField]
    private List<PlayerImageInSequence> playerImageInSequence;

    private int k = 0;
    private int totalImageCount;
    private int count = 0;

    private int previousCountForMemberList;
    void UpdatePlayerImage()
    {
        if (userImage.Count == previousCountForMemberList)
        {
            return;
        }

        k = 0;
        previousCountForMemberList = 0;
        totalImageCount = 0;
        playerImageInSequence.Clear();

        playerImageInSequence = new List<PlayerImageInSequence>();

        for (int i = 0; i < userImage.Count; i++)
        {
            if (!string.IsNullOrEmpty(userImage[i]))
            {
                playerImageInSequence.Add(new PlayerImageInSequence());
                ClubManagement.instance.loadingPanel.SetActive(true);
                playerImageInSequence[k].imgUrl = userImage[i];
                playerImageInSequence[k].ImageProcess(userImage[i]);

                k = k + 1;

            }
            previousCountForMemberList++;
        }
    }

    public void ApplyImage()
    {
        print(k);
        print(totalImageCount);
        if (k == totalImageCount)
        {
            count = 0;
            ClubManagement.instance.loadingPanel.SetActive(false);

            for (int i = 0; i < userImage.Count; i++)
            {
                leaderWinningContent.transform.GetChild(i).GetChild(2).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
            }

            for (int i = 0; i < userImage.Count; i++)
            {
                if (!string.IsNullOrEmpty(userImage[i]))
                {
                    print("i = " + i);
                    leaderWinningContent.transform.GetChild(i).GetChild(2).GetComponent<Image>().sprite = playerImageInSequence[count].imgPic;
                    count++;
                }
            }

        }
    }

    [Serializable]
    public class PlayerImageInSequence
    {
        public string imgUrl;
        public Sprite imgPic;

        public void ImageProcess(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                Communication.instance.GetImage(url, ImageResponse);
            }
        }

        void ImageResponse(Sprite response)
        {
            if (response != null)
            {
                imgPic = response;

                instance.totalImageCount++;
                instance.ApplyImage();

            }
        }
    }

    public void ResetAllValuesForImage()
    {
        totalImageCount = 0;
        k = 0;
        count = 0;
        previousCountForMemberList = 0;
        userImage.Clear();
        playerImageInSequence.Clear();

    }

    #endregion

    #region update Player Image 1

    public List<string> userImage1;

    [SerializeField]
    private List<PlayerImageInSequence1> playerImageInSequence1;

    private int k1 = 0;
    private int totalImageCount1;
    private int count1 = 0;

    private int previousCountForMemberList1;
    void UpdatePlayerImage1()
    {
        if (userImage1.Count == previousCountForMemberList1)
        {
            return;
        }

        k1 = 0;
        previousCountForMemberList1 = 0;
        totalImageCount1 = 0;
        playerImageInSequence1.Clear();

        playerImageInSequence1 = new List<PlayerImageInSequence1>();

        for (int i = 0; i < userImage1.Count; i++)
        {
            if (!string.IsNullOrEmpty(userImage1[i]))
            {
                playerImageInSequence1.Add(new PlayerImageInSequence1());
                ClubManagement.instance.loadingPanel.SetActive(true);
                playerImageInSequence1[k1].imgUrl = userImage1[i];
                playerImageInSequence1[k1].ImageProcess(userImage1[i]);

                k1 = k1 + 1;

            }
            previousCountForMemberList1++;
        }
    }

    public void ApplyImage1()
    {
        print(k1);
        print(totalImageCount1);
        if (k1 == totalImageCount1)
        {
            count1 = 0;
            ClubManagement.instance.loadingPanel.SetActive(false);

            for (int i = 0; i < userImage1.Count; i++)
            {
                leaderGamesContent.transform.GetChild(i).GetChild(2).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
            }

            for (int i = 0; i < userImage1.Count; i++)
            {
                if (!string.IsNullOrEmpty(userImage1[i]))
                {
                    print("i = " + i);
                    print("count1 = " + count1);

                    leaderGamesContent.transform.GetChild(i).GetChild(2).GetComponent<Image>().sprite = playerImageInSequence1[count1].imgPic;
                    count1++;
                }
            }

        }
    }

    [Serializable]
    public class PlayerImageInSequence1
    {
        public string imgUrl;
        public Sprite imgPic;

        public void ImageProcess(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                Communication.instance.GetImage(url, ImageResponse);
            }
        }

        void ImageResponse(Sprite response)
        {
            if (response != null)
            {
                imgPic = response;
                print("imag name : " + imgPic.name);
                instance.totalImageCount1++;
                instance.ApplyImage1();

            }
        }
    }

    public void ResetAllValuesForImage1()
    {
        totalImageCount1 = 0;
        k1 = 0;
        count1 = 0;
        previousCountForMemberList1 = 0;
        userImage1.Clear();
        playerImageInSequence1.Clear();

    }

    #endregion

    public void ClickOnWinnings()
    {
        winningBtn.transform.GetChild(1).GetComponent<Image>().color = new Color32(10,254,238,255);
        winningImage.SetActive(true);
        winningScrollView.SetActive(true);

        gamesBtn.transform.GetChild(1).GetComponent<Image>().color = new Color32(34, 38, 43, 255);
        gamesImage.SetActive(false);
        gamesScrollView.SetActive(false);
    }

    public void ClickOnGames()
    {
        winningBtn.transform.GetChild(1).GetComponent<Image>().color = new Color32(34, 38, 43, 255);
        winningImage.SetActive(false);
        winningScrollView.SetActive(false);

        gamesBtn.transform.GetChild(1).GetComponent<Image>().color = new Color32(10, 254, 238, 255);
        gamesImage.SetActive(true);
        gamesScrollView.SetActive(true);
    }

    #endregion

    #region Tournament Leaderboard

    [Serializable]
    public class TournamentLeadershipResponse
    {
        public bool error;
        public TourneyWining[] tourneyWining;
        public TourneyPlayed[] tourneyplayed;
        public TourneyCashes[] cashes;
        public string message;
    }

    [Serializable]
    public class TourneyWining
    {
        public int winner_chips;
        public string user_image;
        public string username;
        public string first_name;
        public string last_name;
        public string city;
        public string country;
        public string chips;
    }

    [Serializable]
    public class TourneyPlayed
    {
        public int nlh_tournament_played;
        public string user_image;
        public string username;
        public string first_name;
        public string last_name;
        public string city;
        public string country;
    }

    [Serializable]
    public class TourneyCashes
    {
        public int cash;
        public string user_image;
        public string username;
        public string first_name;
        public string last_name;
        public string city;
        public string country;
    }

    [SerializeField] TournamentLeadershipResponse tournamentLeadershipResponse;

    public void TournamentLeaderBoardRequest()
    {

        ClubManagement.instance.loadingPanel.SetActive(true);

        Communication.instance.PostData(tournamentGameUrl, TournamentLeaderBoardCallback);
    }

    void TournamentLeaderBoardCallback(string response)
    {
        print("res : " + response);
        ClubManagement.instance.loadingPanel.SetActive(false);

        tournamentLeadershipResponse = JsonUtility.FromJson<TournamentLeadershipResponse>(response);

        if (!tournamentLeadershipResponse.error)
        {
            print("successfull.....Tournament LeaderBoard... ");

            for (int i = 0; i < tourneyWinningContent.transform.childCount; i++)
            {
                tourneyWinningContent.transform.GetChild(i).gameObject.SetActive(false);
            }
            userImage2.Clear();
            for (int i = 0; i < tournamentLeadershipResponse.tourneyWining.Length; i++)
            {
                if (i < 10)
                {
                    tourneyWinningContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text = tournamentLeadershipResponse.tourneyWining[i].username;
                    tourneyWinningContent.transform.GetChild(i).GetChild(4).GetComponent<Text>().text = tournamentLeadershipResponse.tourneyWining[i].city + ", " + tournamentLeadershipResponse.tourneyWining[i].country;
                    tourneyWinningContent.transform.GetChild(i).GetChild(5).GetComponent<Text>().text = tournamentLeadershipResponse.tourneyWining[i].chips;
                    tourneyWinningContent.transform.GetChild(i).gameObject.SetActive(true);
                    userImage2.Add(tournamentLeadershipResponse.tourneyWining[i].user_image);
                }
            }
            UpdatePlayerImage2();

            for (int i = 0; i < tourneyPlayedContent.transform.childCount; i++)
            {
                tourneyPlayedContent.transform.GetChild(i).gameObject.SetActive(false);
            }
            userImage3.Clear();
            for (int i = 0; i < tournamentLeadershipResponse.tourneyplayed.Length; i++)
            {
                if (i < 10)
                {
                    tourneyPlayedContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text = tournamentLeadershipResponse.tourneyplayed[i].username;
                    tourneyPlayedContent.transform.GetChild(i).GetChild(4).GetComponent<Text>().text = tournamentLeadershipResponse.tourneyplayed[i].city + ", " + tournamentLeadershipResponse.tourneyplayed[i].country;
                    tourneyPlayedContent.transform.GetChild(i).GetChild(5).GetComponent<Text>().text = tournamentLeadershipResponse.tourneyplayed[i].nlh_tournament_played.ToString();
                    tourneyPlayedContent.transform.GetChild(i).gameObject.SetActive(true);
                    userImage3.Add(tournamentLeadershipResponse.tourneyplayed[i].user_image);
                }
            }
            UpdatePlayerImage3();

            for (int i = 0; i < totalCashesContent.transform.childCount; i++)
            {
                totalCashesContent.transform.GetChild(i).gameObject.SetActive(false);
            }
            userImage4.Clear();
            for (int i = 0; i < tournamentLeadershipResponse.cashes.Length; i++)
            {
                if (i < 10)
                {
                    totalCashesContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text = tournamentLeadershipResponse.cashes[i].username;
                    totalCashesContent.transform.GetChild(i).GetChild(4).GetComponent<Text>().text = tournamentLeadershipResponse.cashes[i].city + ", " + tournamentLeadershipResponse.cashes[i].country;
                    totalCashesContent.transform.GetChild(i).GetChild(5).GetComponent<Text>().text = tournamentLeadershipResponse.cashes[i].cash.ToString();
                    totalCashesContent.transform.GetChild(i).gameObject.SetActive(true);
                    userImage4.Add(tournamentLeadershipResponse.cashes[i].user_image);
                }
            }
            UpdatePlayerImage4();
        }
    }

    #region update Player Image 2

    public List<string> userImage2;

    [SerializeField]
    private List<PlayerImageInSequence2> playerImageInSequence2;

    private int k2 = 0;
    private int totalImageCount2;
    private int count2 = 0;

    private int previousCountForMemberList2;
    void UpdatePlayerImage2()
    {
        if (userImage2.Count == previousCountForMemberList2)
        {
            return;
        }

        k2 = 0;
        previousCountForMemberList2 = 0;
        totalImageCount2 = 0;
        playerImageInSequence2.Clear();

        playerImageInSequence2 = new List<PlayerImageInSequence2>();

        for (int i = 0; i < userImage2.Count; i++)
        {
            if (!string.IsNullOrEmpty(userImage2[i]))
            {
                playerImageInSequence2.Add(new PlayerImageInSequence2());
                ClubManagement.instance.loadingPanel.SetActive(true);
                playerImageInSequence2[k2].imgUrl = userImage2[i];
                playerImageInSequence2[k2].ImageProcess(userImage2[i]);

                k2 = k2 + 1;

            }
            previousCountForMemberList2++;
        }
    }

    public void ApplyImage2()
    {
        print(k2);
        print(totalImageCount2);
        if (k2 == totalImageCount2)
        {
            count2 = 0;
            ClubManagement.instance.loadingPanel.SetActive(false);

            for (int i = 0; i < userImage2.Count; i++)
            {
                tourneyWinningContent.transform.GetChild(i).GetChild(2).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
            }

            for (int i = 0; i < userImage2.Count; i++)
            {
                if (!string.IsNullOrEmpty(userImage2[i]))
                {

                    tourneyWinningContent.transform.GetChild(i).GetChild(2).GetComponent<Image>().sprite = playerImageInSequence2[count2].imgPic;
                    count2++;
                }
            }

        }
    }

    [Serializable]
    public class PlayerImageInSequence2
    {
        public string imgUrl;
        public Sprite imgPic;

        public void ImageProcess(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                Communication.instance.GetImage(url, ImageResponse);
            }
        }

        void ImageResponse(Sprite response)
        {
            if (response != null)
            {
                imgPic = response;
                print("imag name : " + imgPic.name);
                instance.totalImageCount2++;
                instance.ApplyImage2();

            }
        }
    }

    public void ResetAllValuesForImage2()
    {
        totalImageCount2 = 0;
        k2 = 0;
        count2 = 0;
        previousCountForMemberList2 = 0;
        userImage2.Clear();
        playerImageInSequence2.Clear();

    }

    #endregion

    #region update Player Image 3

    public List<string> userImage3;

    [SerializeField]
    private List<PlayerImageInSequence3> playerImageInSequence3;

    private int k3 = 0;
    private int totalImageCount3;
    private int count3 = 0;

    private int previousCountForMemberList3;
    void UpdatePlayerImage3()
    {
        if (userImage3.Count == previousCountForMemberList3)
        {
            return;
        }

        k3 = 0;
        previousCountForMemberList3 = 0;
        totalImageCount3 = 0;
        playerImageInSequence3.Clear();

        playerImageInSequence3 = new List<PlayerImageInSequence3>();

        for (int i = 0; i < userImage3.Count; i++)
        {
            if (!string.IsNullOrEmpty(userImage3[i]))
            {
                playerImageInSequence3.Add(new PlayerImageInSequence3());
                ClubManagement.instance.loadingPanel.SetActive(true);
                playerImageInSequence3[k3].imgUrl = userImage3[i];
                playerImageInSequence3[k3].ImageProcess(userImage3[i]);

                k3 = k3 + 1;

            }
            previousCountForMemberList3++;
        }
    }

    public void ApplyImage3()
    {

        if (k3 == totalImageCount3)
        {
            count3 = 0;
            ClubManagement.instance.loadingPanel.SetActive(false);

            for (int i = 0; i < userImage3.Count; i++)
            {
                tourneyPlayedContent.transform.GetChild(i).GetChild(2).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
            }

            for (int i = 0; i < userImage3.Count; i++)
            {
                if (!string.IsNullOrEmpty(userImage3[i]))
                {

                    tourneyPlayedContent.transform.GetChild(i).GetChild(2).GetComponent<Image>().sprite = playerImageInSequence3[count3].imgPic;
                    count3++;
                }
            }

        }
    }

    [Serializable]
    public class PlayerImageInSequence3
    {
        public string imgUrl;
        public Sprite imgPic;

        public void ImageProcess(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                Communication.instance.GetImage(url, ImageResponse);
            }
        }

        void ImageResponse(Sprite response)
        {
            if (response != null)
            {
                imgPic = response;
                print("imag name : " + imgPic.name);
                instance.totalImageCount3++;
                instance.ApplyImage3();

            }
        }
    }

    public void ResetAllValuesForImage3()
    {
        totalImageCount3 = 0;
        k3 = 0;
        count3 = 0;
        previousCountForMemberList3 = 0;
        userImage3.Clear();
        playerImageInSequence3.Clear();

    }

    #endregion

    #region update Player Image 4

    public List<string> userImage4;

    [SerializeField]
    private List<PlayerImageInSequence4> playerImageInSequence4;

    private int k4 = 0;
    private int totalImageCount4;
    private int count4 = 0;

    private int previousCountForMemberList4;
    void UpdatePlayerImage4()
    {
        if (userImage4.Count == previousCountForMemberList4)
        {
            return;
        }

        k4 = 0;
        previousCountForMemberList4 = 0;
        totalImageCount4 = 0;
        playerImageInSequence4.Clear();

        playerImageInSequence4 = new List<PlayerImageInSequence4>();

        for (int i = 0; i < userImage4.Count; i++)
        {
            if (!string.IsNullOrEmpty(userImage4[i]))
            {
                playerImageInSequence4.Add(new PlayerImageInSequence4());
                ClubManagement.instance.loadingPanel.SetActive(true);
                playerImageInSequence4[k4].imgUrl = userImage4[i];
                playerImageInSequence4[k4].ImageProcess(userImage4[i]);

                k4 = k4 + 1;

            }
            previousCountForMemberList4++;
        }
    }

    public void ApplyImage4()
    {

        if (k4 == totalImageCount4)
        {
            count4 = 0;
            ClubManagement.instance.loadingPanel.SetActive(false);

            for (int i = 0; i < userImage4.Count; i++)
            {
                totalCashesContent.transform.GetChild(i).GetChild(2).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
            }

            for (int i = 0; i < userImage4.Count; i++)
            {
                if (!string.IsNullOrEmpty(userImage4[i]))
                {

                    totalCashesContent.transform.GetChild(i).GetChild(2).GetComponent<Image>().sprite = playerImageInSequence4[count4].imgPic;
                    count4++;
                }
            }

        }
    }

    [Serializable]
    public class PlayerImageInSequence4
    {
        public string imgUrl;
        public Sprite imgPic;

        public void ImageProcess(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                Communication.instance.GetImage(url, ImageResponse);
            }
        }

        void ImageResponse(Sprite response)
        {
            if (response != null)
            {
                imgPic = response;
                print("imag name : " + imgPic.name);
                instance.totalImageCount4++;
                instance.ApplyImage4();

            }
        }
    }

    public void ResetAllValuesForImage4()
    {
        ResetAllValuesForImage1();
        ResetAllValuesForImage2();
        ResetAllValuesForImage3();
        ResetAllValuesForImage();
        totalImageCount4 = 0;
        k4 = 0;
        count4 = 0;
        previousCountForMemberList4 = 0;
        userImage4.Clear();
        playerImageInSequence4.Clear();

    }

    #endregion

    public void ClickCashGameTab()
    {
        cashGameBtn.GetChild(0).gameObject.SetActive(true);
        cashGameBtn.GetChild(1).gameObject.SetActive(true);
        cashGameBtn.GetChild(2).gameObject.SetActive(false);
        cashGameBtn.GetComponent<Button>().enabled = false;
        cashGameScrollUI.SetActive(true);

        tournamentBtn.GetChild(0).gameObject.SetActive(false);
        tournamentBtn.GetChild(1).gameObject.SetActive(false);
        tournamentBtn.GetChild(2).gameObject.SetActive(true);
        tournamentBtn.GetComponent<Button>().enabled = true;
        tournamentScrollUI.SetActive(false);

        LeaderBoardRequest();
        ClickOnWinnings();
    }

    public void ClickTournamentTab()
    {
        tournamentBtn.GetChild(0).gameObject.SetActive(true);
        tournamentBtn.GetChild(1).gameObject.SetActive(true);
        tournamentBtn.GetChild(2).gameObject.SetActive(false);
        tournamentBtn.GetComponent<Button>().enabled = false;
        tournamentScrollUI.SetActive(true);
        
        cashGameBtn.GetChild(0).gameObject.SetActive(false);
        cashGameBtn.GetChild(1).gameObject.SetActive(false);
        cashGameBtn.GetChild(2).gameObject.SetActive(true);
        cashGameBtn.GetComponent<Button>().enabled = true;
        cashGameScrollUI.SetActive(false);

        TournamentLeaderBoardRequest();
        ClickTournamentCommonTab(tournamentTabBtn[0]);
    }

    public void ClickTournamentCommonTab(Transform selectedTab)
    {
        for (int i = 0; i < tournamentTabBtn.Count; i++)
        {
            tournamentTabBtn[i].GetChild(1).gameObject.SetActive(false);
            tournamentTabBtn[i].GetChild(2).gameObject.SetActive(false);
            tournamentTabBtn[i].GetComponent<Button>().enabled = true;
            tournamentScrollCommonUI[i].SetActive(false);
            tournamentImagesCommonUI[i].SetActive(false);
        }
        selectedTab.GetChild(1).gameObject.SetActive(true);
        selectedTab.GetChild(2).gameObject.SetActive(false);
        selectedTab.GetComponent<Button>().enabled = false;

        if (selectedTab.CompareTag("Winning"))
        {
            tournamentScrollCommonUI[0].SetActive(true);
            tournamentImagesCommonUI[0].SetActive(true);
        }
        else if (selectedTab.CompareTag("TourneyPlayed"))
        {
            tournamentScrollCommonUI[1].SetActive(true);
            tournamentImagesCommonUI[1].SetActive(true);
        }
        else if (selectedTab.CompareTag("Cash"))
        {
            tournamentScrollCommonUI[2].SetActive(true);
            tournamentImagesCommonUI[2].SetActive(true);
        }
    }

    #endregion

}