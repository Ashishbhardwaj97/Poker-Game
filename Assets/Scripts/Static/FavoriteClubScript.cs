using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FavoriteClubScript : MonoBehaviour
{
    public static FavoriteClubScript instance;
    [Header("GameObject References")]
    public GameObject uploadText;
    public GameObject shop;
    public GameObject shopPopUp;
    public GameObject registerImage;
    public GameObject registerImageMask;
    public GameObject totalFees;
    public GameObject totalGames;
    public GameObject totalWinning;
    public GameObject chipClaimBack;
    public GameObject chipSendOut;
    public GameObject totalFeesAgent;
    public GameObject totalGamesAgent;
    public GameObject totalWinningAgent;
    public GameObject chipClaimBackAgent;
    public GameObject chipSendOutAgent;
    public GameObject agentFees;
    public GameObject agentWinning;
    public GameObject agentCredit;
    public GameObject downlineChips;
    public GameObject createClubImage;
    public GameObject stepText;
    public GameObject verificationPanelResendBtn;
    public GameObject otpVerifyResendBtn;
    public GameObject registrationSignInBtn;

    public GameObject favClubPanel;
    public GameObject favClubScrollContent;
    public GameObject favClubListingObject;
    public GameObject favClubScrollObject;
    public GameObject noFavClubScrollObject;

    public GameObject homePanel;
    public GameObject gameQuitPanel;

    [Header("List Properties")]
    public List<GameObject> ownerList;
    public List<GameObject> partnerList;
    public List<GameObject> accountantList;
    public List<GameObject> managerList;
    public List<GameObject> agentList;
    public List<GameObject> playerList;

    //private string favoriteListURL;
    //private string favoriteClubUrl;

    private int myFavClubCount;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        //if (Screen.width == 1440 && Screen.height == 3120 || Screen.width == 1080 && Screen.height == 2340 )
        //{
        //    shop.transform.GetChild(10).localPosition = new Vector3(393, transform.localPosition.y - 200, 0);
        //    shop.transform.GetChild(11).localPosition = new Vector3(0, transform.localPosition.y - 200, 0);
        //    shop.transform.GetChild(12).localPosition = new Vector3(-393, transform.localPosition.y - 200, 0);

        //    shopPopUp.transform.GetChild(12).localPosition = new Vector3(393, transform.localPosition.y - 200, 0);
        //    shopPopUp.transform.GetChild(13).localPosition = new Vector3(0, transform.localPosition.y - 200, 0);
        //    shopPopUp.transform.GetChild(14).localPosition = new Vector3(-393, transform.localPosition.y - 200, 0);

        //    stepText.transform.localPosition = new Vector3(-14, 716.5f, 0);
        //    createClubImage.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 370);
        //    uploadText.transform.localPosition = new Vector3(-10, transform.localPosition.y + 180, 0);
        //    //registerImage.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
        //    registerImageMask.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);

        //    //totalFees.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 340);
        //    //totalGames.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 340);
        //    //totalWinning.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 340);
        //    //chipClaimBack.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 340);
        //    //chipSendOut.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 340);

        //    //totalFeesAgent.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 340);
        //    //totalGamesAgent.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 340);
        //    //totalWinningAgent.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 340);
        //    //chipClaimBackAgent.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 340);
        //    //chipSendOutAgent.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 340);

        //    //agentFees.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 340);
        //    //agentWinning.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 340);
        //    //agentCredit.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 340);
        //    //downlineChips.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 340);
        //}

        //if (Screen.width == 1440 && Screen.height == 2960 || Screen.width == 1080 && Screen.height == 2160 || Screen.width == 1080 && Screen.height == 2220 || Screen.width == 1080 && Screen.height == 2280 || Screen.width == 750)
        //{
        //    uploadText.transform.localPosition = new Vector3(-10, transform.localPosition.y + 130, 0);
        //    createClubImage.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 370);

        //    //totalFees.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 330);
        //    //totalGames.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 330);
        //    //totalWinning.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 330);
        //    //chipClaimBack.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 330);
        //    //chipSendOut.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 330);

        //    //totalFeesAgent.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 330);
        //    //totalGamesAgent.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 330);
        //    //totalWinningAgent.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 330);
        //    //chipClaimBackAgent.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 330);
        //    //chipSendOutAgent.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 330);

        //    //agentFees.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 330);
        //    //agentWinning.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 330);
        //    //agentCredit.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 330);
        //    //downlineChips.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 330);
        //}

        //if (Screen.width == 1284 && Screen.height == 2778 || Screen.width == 828 && Screen.height == 1792 || Screen.width == 1125 && Screen.height == 2436 /*|| Screen.width == 1080 && Screen.height == 2280 || Screen.width == 750*/)
        //{
        //    uploadText.transform.localPosition = new Vector3(-20, 165, 0);
        //    stepText.transform.localPosition = new Vector3(-25, 716.5f, 0);
        //}

        //if(Screen.width > 1500)
        //{
        //    verificationPanelResendBtn.transform.localPosition = new Vector3(100, -45, 0);
        //    registrationSignInBtn.transform.localPosition = new Vector3(250, -750, 0);
        //    uploadText.transform.localPosition = new Vector3(18, 8.5f, 0);
        //    otpVerifyResendBtn.transform.localPosition = new Vector3(180, -450, 0);
        //}

        //favoriteClubUrl = ServerChanger.instance.domainURL + "api/v1/club/favorite-club";
        //favoriteListURL = ServerChanger.instance.domainURL + "api/v1/club/favorite-list";
        myFavClubCount = 1;

        favClubItems = new List<GameObject>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (homePanel.activeInHierarchy)
            {
                gameQuitPanel.SetActive(true);
            }
        }
    }

    public void QuitApp()
    {
        Application.Quit();
    }

    #region Click Favorite club in club listing

    [Serializable]
    class FavClub
    {
        public string club_id;
        public int is_favorite;
    }

    [Serializable]
    class FavClubResponse
    {
        public bool error;
    }

    [SerializeField] FavClub favClub;
    public void ClickOnFavClub(Transform panel)
    {
        panel.GetChild(0).GetChild(11).GetChild(0).gameObject.SetActive(false);
        panel.GetChild(0).GetChild(11).GetChild(1).gameObject.SetActive(true);

        favClub.club_id = panel.GetChild(0).GetChild(1).GetComponent<Text>().text;
        favClub.is_favorite = 1;

        string body = JsonUtility.ToJson(favClub);
        print(body);
        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(favoriteClubUrl, body, ClickOnFavClubProcess);
    }

    public void ClickOnUnFavClub(Transform panel)
    {
        panel.GetChild(0).GetChild(11).GetChild(0).gameObject.SetActive(true);
        panel.GetChild(0).GetChild(11).GetChild(1).gameObject.SetActive(false);

        favClub.club_id = panel.GetChild(0).GetChild(1).GetComponent<Text>().text;
        favClub.is_favorite = 0;

        string body = JsonUtility.ToJson(favClub);
        print(body);
        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(favoriteClubUrl, body, ClickOnFavClubProcess);
    }

    [SerializeField] FavClubResponse favClubResponse;

    void ClickOnFavClubProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("Some error in Send out ...!");
        }
        else
        {
            print("response" + response);
            favClubResponse = JsonUtility.FromJson<FavClubResponse>(response);
            
            if (!favClubResponse.error)
            {
                print("successful");
            }
            else
            {
                print("Un-successful");
            }
        }
    }

    #endregion

    #region click on Play Section

    public List<GameObject> favClubItems;
    [Serializable]
    public class FavoriteListResponse
    {
        public bool error;
        public FavoriteList[] favoriteList;
    }


    [Serializable]
    public class FavoriteList
    {
        public string club_name;
        public string club_id;
        public int joining;
        public int no_of_member;
        public string upload_logo;
        public string welcome_msg;
        public string city;
        public string country;
        public int totalChipRequest;
        public int chips;
        public string agent_id;
        public int is_favorite;
        public string club_member_role;
        public int agent_joining;
        public int agent_totalChipRequest;
    }

    [SerializeField] FavoriteListResponse favoriteListResponse;
    public void ClickOnPlayBtton()
    {
        Uimanager.instance.isClubListingPage = false;
        Uimanager.instance.isHomePage = true;
        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(favoriteListURL, FavoriteClubResponse);
    }

    void FavoriteClubResponse(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("Some error in Send out ...!");
        }
        else
        {
            favoriteListResponse = JsonUtility.FromJson<FavoriteListResponse>(response);
            print("response" + response);

            if (!favoriteListResponse.error)
            {
                if (favoriteListResponse.favoriteList.Length != myFavClubCount)
                {
                    for (int i = myFavClubCount; i < favoriteListResponse.favoriteList.Length; i++)
                    {
                        myFavClubCount++;
                        GenerateMyFavClubItem();
                    }
                }
                favClubImage.Clear();
                ownerList.Clear();
                partnerList.Clear();
                accountantList.Clear();
                managerList.Clear();
                agentList.Clear();
                playerList.Clear();
                for (int i = 0; i < favoriteListResponse.favoriteList.Length; i++)
                {
                    favClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].club_name + "      ";
                    favClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(1).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].club_id;
                    favClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = (favoriteListResponse.favoriteList[i].joining + favoriteListResponse.favoriteList[i].totalChipRequest).ToString();
                    
                    string str = favoriteListResponse.favoriteList[i].club_member_role;
                    
                    favClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(3).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].club_member_role;
                    if (!string.IsNullOrEmpty(str))
                    {
                        if (str == "Accountant")
                        {
                            favClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = str.Substring(0, 2);
                        }
                        else if (str == "Partner")
                        {
                            favClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = str.Substring(0, 2);
                        }
                        else
                        {
                            favClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = str.Substring(0, 1);
                        }
                        
                        favClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(10).GetChild(0).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].club_member_role;
                    }
                    favClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(4).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].no_of_member.ToString();

                    favClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(5).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].agent_id;

                    favClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(6).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].country;
                    favClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(7).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].city;
                    favClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(8).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].joining.ToString();
                    favClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(9).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].totalChipRequest.ToString();

                    favClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(13).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].agent_joining.ToString();
                    favClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(14).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].agent_totalChipRequest.ToString();

                    if (ClubManagement.instance.currentSelectedClubObject != null)
                    {
                        ClubManagement.instance.memberReqTotalCount = ClubManagement.instance.currentSelectedClubObject.transform.GetChild(0).GetChild(8).GetComponent<Text>().text;
                        ClubManagement.instance.chipReqTotalCount = ClubManagement.instance.currentSelectedClubObject.transform.GetChild(0).GetChild(9).GetComponent<Text>().text;

                        ClubManagement.instance.memberReqTotalCountForAgent = ClubManagement.instance.currentSelectedClubObject.transform.GetChild(0).GetChild(13).GetComponent<Text>().text;
                        ClubManagement.instance.chipReqTotalCountForAgent = ClubManagement.instance.currentSelectedClubObject.transform.GetChild(0).GetChild(14).GetComponent<Text>().text;
                    }

                    favClubImage.Add(favoriteListResponse.favoriteList[i].upload_logo);

                    string memberRole = favoriteListResponse.favoriteList[i].club_member_role;

                    if (memberRole == "Player")
                    {
                        //myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(2).gameObject.SetActive(false);
                        playerList.Add(favClubScrollContent.transform.GetChild(i).gameObject);
                    }

                    else if (memberRole == "Owner")
                    {
                        //myClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(2).gameObject.SetActive(true);
                        ownerList.Add(favClubScrollContent.transform.GetChild(i).gameObject);
                    }
                    else if (memberRole == "Manager")
                    {
                        managerList.Add(favClubScrollContent.transform.GetChild(i).gameObject);
                    }
                    else if (memberRole == "Agent")
                    {
                        agentList.Add(favClubScrollContent.transform.GetChild(i).gameObject);
                    }
                    else if (memberRole == "Partner")
                    {
                        partnerList.Add(favClubScrollContent.transform.GetChild(i).gameObject);
                    }
                    else if (memberRole == "Accountant")
                    {
                        accountantList.Add(favClubScrollContent.transform.GetChild(i).gameObject);
                    }
                }

                if (favoriteListResponse.favoriteList.Length <= 6)
                {
                    for (int i = 0; i < favClubScrollObject.transform.GetChild(0).GetChild(0).childCount; i++)
                    {
                        favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).gameObject.SetActive(false);
                    }

                    for (int i = 0; i < favoriteListResponse.favoriteList.Length; i++)
                    {
                        favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].club_name;
                        favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetChild(1).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].club_id;
                        favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = (favoriteListResponse.favoriteList[i].joining + favoriteListResponse.favoriteList[i].totalChipRequest).ToString();
                        favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetChild(3).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].club_member_role;
                        favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetChild(10).GetChild(0).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].club_member_role;
                        favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetChild(4).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].no_of_member.ToString();

                        favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetChild(5).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].agent_id;

                        favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetChild(6).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].country;
                        favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetChild(7).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].city;
                        favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetChild(8).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].joining.ToString();
                        favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetChild(9).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].totalChipRequest.ToString();

                        favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetChild(13).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].agent_joining.ToString();
                        favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetChild(14).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].agent_totalChipRequest.ToString();

                        favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).gameObject.SetActive(true);
                    }
                }
                else
                {
                    for (int i = 0; i < 6; i++)
                    {
                        favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].club_name;
                        favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetChild(1).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].club_id;
                        favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = (favoriteListResponse.favoriteList[i].joining + favoriteListResponse.favoriteList[i].totalChipRequest).ToString();
                        favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetChild(3).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].club_member_role;
                        favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetChild(10).GetChild(0).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].club_member_role;
                        favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetChild(4).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].no_of_member.ToString();

                        favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetChild(5).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].agent_id;

                        //favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetChild(6).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].country;
                        favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetChild(7).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].city;
                        favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetChild(8).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].joining.ToString();
                        favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetChild(9).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].totalChipRequest.ToString();
                        favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetChild(13).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].agent_joining.ToString();
                        favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetChild(14).GetComponent<Text>().text = favoriteListResponse.favoriteList[i].agent_totalChipRequest.ToString();

                        favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).gameObject.SetActive(true);
                    }
                    favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(6).gameObject.SetActive(true);

                }

                favClubScrollObject.SetActive(true);

                favClubListingObject.SetActive(false);
                noFavClubScrollObject.SetActive(false);

                UpdateFavClubImage();
            }
            else
            {
                print("something went wrong");
                noFavClubScrollObject.SetActive(true);
                favClubScrollObject.SetActive(false);
                favClubListingObject.SetActive(false);
            }

        }
    }

    void GenerateMyFavClubItem()
    {
        GameObject scrollItemObj = Instantiate(favClubPanel);
        scrollItemObj.transform.SetParent(favClubScrollContent.transform, false);

        favClubItems.Add(scrollItemObj);
    }

    public void ResetGeneratedFavClubItem()
    {
        if (favClubItems.Count > 0)
        {
            for (int i = 0; i < favClubItems.Count; i++)
            {
                Destroy(favClubItems[i]);
            }

            favClubItems.Clear();
            myFavClubCount = 1;
        }
    }

    #region update Fav Club Image

    private int p;
    private int totalImageCountForFavClub;
    private int countForFavClub;
    private int previousCountForFavClub;

    public List<string> favClubImage;
    [SerializeField]
    public List<FavClubImageInSequence> favClubImageInSequence;

    public void ResetFavoriteClubImages()
    {
        p = 0;
        totalImageCountForFavClub = 0;
        countForFavClub = 0;
        previousCountForFavClub = 0;
        favClubImage.Clear();
        favClubImageInSequence.Clear();
    }

    void UpdateFavClubImage()
    {
        if (favClubImage.Count == previousCountForFavClub)
        {
            return;
        }

        favClubImageInSequence.Clear();
        p = 0;
        totalImageCountForFavClub = 0;
        previousCountForFavClub = 0;

        for (int i = 0; i < favClubImage.Count; i++)
        {
            favClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(11).GetComponent<Image>().sprite = Registration.instance.defaultClubImage.sprite;

            if (i < 6)
            {
                favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Image>().sprite = Registration.instance.defaultClubImageWithHeart.sprite;
            }
        }

        favClubImageInSequence = new List<FavClubImageInSequence>();

        for (int i = 0; i < favClubImage.Count; i++)
        {
            if (!string.IsNullOrEmpty(favClubImage[i]))
            {
                print("UpdatePlayerImageFor Club ......");
                favClubImageInSequence.Add(new FavClubImageInSequence());
                ClubManagement.instance.loadingPanel.SetActive(true);
                favClubImageInSequence[p].imgUrl = favClubImage[i];
                favClubImageInSequence[p].FavClubImageProcess(favClubImage[i]);

                p = p + 1;

            }

            previousCountForFavClub++;
        }
    }

    public void ApplyImageInFavClub()
    {
        print(p);
        print(totalImageCountForFavClub);
        if (p == totalImageCountForFavClub)
        {
            countForFavClub = 0;
            ClubManagement.instance.loadingPanel.SetActive(false);

            for (int i = 0; i < favClubImage.Count; i++)
            {
                favClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(11).GetComponent<Image>().sprite = Registration.instance.defaultClubImage.sprite;

                if (i < 6)
                {
                    favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Image>().sprite = Registration.instance.defaultClubImageWithHeart.sprite;
                }
            }

            for (int i = 0; i < favClubImage.Count; i++)
            {
                if (!string.IsNullOrEmpty(favClubImage[i]))
                {
                    favClubScrollContent.transform.GetChild(i).GetChild(0).GetChild(11).GetComponent<Image>().sprite = favClubImageInSequence[countForFavClub].imgPic;

                    if (i < 6)
                    {
                        favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Image>().sprite = favClubImageInSequence[countForFavClub].imgPic;
                        favClubScrollObject.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetChild(11).GetComponent<Image>().sprite = favClubImageInSequence[countForFavClub].imgPic;
                    }

                    countForFavClub++;
                }


            }

        }
    }

    [Serializable]
    public class FavClubImageInSequence
    {
        public string imgUrl;
        public Sprite imgPic;

        public void FavClubImageProcess(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                Communication.instance.GetImage(url, FavClubImageResponse);
            }
        }

        void FavClubImageResponse(Sprite response)
        {
            if (response != null)
            {
                imgPic = response;

                if (instance.favClubScrollObject.activeInHierarchy)
                {
                    instance.totalImageCountForFavClub++;
                    instance.ApplyImageInFavClub();
                }

            }
        }
    }

    #endregion

    #endregion

    public void OpenFavoriteFilter()
    {
        favClubListingObject.transform.GetChild(10).gameObject.SetActive(true);
        favClubListingObject.transform.GetChild(11).gameObject.SetActive(true);
    }

    public void CloseFavoriteFilter()
    {
        favClubListingObject.transform.GetChild(10).gameObject.SetActive(false);
        favClubListingObject.transform.GetChild(11).gameObject.SetActive(false);
    }
}
