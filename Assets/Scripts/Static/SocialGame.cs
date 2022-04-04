using SmartLocalization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SocialGame : MonoBehaviour
{
    public static SocialGame instance;

    public Transform SocialPokerButtons;
    public GameObject pokerUICanvas;
    public GameObject tournamentDetialPanel;
    public GameObject disclaimerPanel;

    public Transform gameList;
    private string socialGameListUrl;

    internal string currentStakeType;
    internal int currentBuyIn;
    internal int currentSmallBlind;
    internal int currentBigBlind;
    internal float currentFees;
    internal float currentFeeCap;
    public bool currentIsVideo;
    internal double currentMinBuyIn;
    internal double currentMaxBuyIn;

    public Slider configTableSlider;

    public GameObject TableMatchOnOff;
    public bool isTableMatchPreference;

    [Serializable]
    public class GameListResponse
    {
        public bool error;
        public GameList[] gameList;
        public int statusCode;
    }

    [Serializable]
    public class GameList
    {
        public string stake_type;
        public string buyin;
        public string small_blind;
        public string big_blind;
        public float fees;
        public float fee_cap;
        public double min_buy_in;
        public double max_buy_in;
    }

    [SerializeField] GameListResponse gameListResponse;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        socialGameListUrl = ServerChanger.instance.domainURL + "api/v1/socialtable/game-list";
        currentIsVideo = true;
    }

    [Serializable]
    class GameListRequest
    {
        public bool video;
    }
    [SerializeField] GameListRequest gameListRequest;

    public void ClickGameList()
    {
        configTableSlider.value = 0;
        gameListRequest.video = true;
        string body = JsonUtility.ToJson(gameListRequest);
        print(body);
        ClubManagement.instance.loadingPanel.SetActive(true);
        SocialPokerGameManager.instance.ClickVideoNonVideo(SocialPokerGameManager.instance.videoSelectionOutline);
        Communication.instance.PostData(socialGameListUrl, body, ClickGameListProcess);
    }

    public void ClickGameListNonVideo()
    {
        configTableSlider.value = 0;
        gameListRequest.video = false;
        string body = JsonUtility.ToJson(gameListRequest);
        print(body);
        print(socialGameListUrl);
        Communication.instance.PostData(socialGameListUrl, body, ClickGameListProcess);
    }

    //public void ClickGameList()
    //{
    //    print(socialGameListUrl);
    //    Communication.instance.PostData(socialGameListUrl, ClickGameListProcess);
    //}

    void ClickGameListProcess(string response)
    {
        pokerUICanvas.GetComponent<Canvas>().enabled = true;
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            Cashier.instance.toastMsg.text = LanguageManager.Instance.GetTextValue("internet check");//SocialProfile._instance.networkErrorMsg;
            Cashier.instance.toastMsgPanel.SetActive(true);
            SocialPokerGameManager.instance.ClickHome();
            print("error");
        }
        else
        {
            print("" + response);
            gameListResponse = JsonUtility.FromJson<GameListResponse>(response);
            if (!gameListResponse.error)
            {
                print("success....");

                configTableSlider.maxValue = gameListResponse.gameList.Length - 1;

                stackVal.text = gameListResponse.gameList[0].small_blind + "/" + gameListResponse.gameList[0].big_blind;
                buyInVal.text = LanguageManager.Instance.GetTextValue("Buy in :") + " " + gameListResponse.gameList[0].buyin;

                currentStakeType = gameListResponse.gameList[0].stake_type;

                currentBuyIn = ConvertCurrencyInIntger(gameListResponse.gameList[0].buyin);
                currentBigBlind = ConvertCurrencyInIntger(gameListResponse.gameList[0].big_blind);
                currentSmallBlind = ConvertCurrencyInIntger(gameListResponse.gameList[0].small_blind);
                currentFees = gameListResponse.gameList[0].fees;
                currentFeeCap = gameListResponse.gameList[0].fee_cap;
                currentMinBuyIn = gameListResponse.gameList[0].min_buy_in;
                currentMaxBuyIn = gameListResponse.gameList[0].max_buy_in;


                if (PlayerPrefs.GetInt(ApiHitScript.TableMatch, 0) == 1)  //......on
                {
                    TableMatchOnOff.transform.GetChild(2).gameObject.SetActive(false);
                    TableMatchOnOff.transform.GetChild(1).gameObject.SetActive(true);
                    isTableMatchPreference = true;
                }
                else  //....off
                {
                    TableMatchOnOff.transform.GetChild(1).gameObject.SetActive(false);
                    TableMatchOnOff.transform.GetChild(2).gameObject.SetActive(true);
                    isTableMatchPreference = false;
                }

                SocialPokerGameManager.instance.EnableGamesPage();

            }

            else
            {
                if (gameListResponse.statusCode == 403)
                {
                    Cashier.instance.toastMsg.text = LanguageManager.Instance.GetTextValue("session expired"); //SocialTournamentScript.instance.login_error;
                    Cashier.instance.toastMsgPanel.SetActive(true);
                    Uimanager.instance.SignOut();
                }
            }
        }
    }


    //void ClickGameListProcess(string response)
    //{
    //    ClubManagement.instance.loadingPanel.SetActive(false);
    //    if (string.IsNullOrEmpty(response))
    //    {
    //        print("error");
    //    }
    //    else
    //    {
    //        print("" + response);
    //        gameListResponse = JsonUtility.FromJson<GameListResponse>(response);
    //        if (!gameListResponse.error)
    //        {
    //            print("success....");

    //            configTableSlider.maxValue = gameListResponse.gameList.Length - 1;

    //            stackVal.text = gameListResponse.gameList[0].small_blind + "/" + gameListResponse.gameList[0].big_blind;
    //            buyInVal.text = gameListResponse.gameList[0].buyin;

    //            currentStakeType = gameListResponse.gameList[0].stake_type;

    //            //currentBuyIn = ConvertCurrencyInIntger(gameListResponse.gameList[0].buyin);
    //            //currentBigBlind = ConvertCurrencyInIntger(gameListResponse.gameList[0].big_blind);
    //            //currentSmallBlind = ConvertCurrencyInIntger(gameListResponse.gameList[0].small_blind);

    //            currentBuyIn = ConvertCurrencyInIntger(gameListResponse.gameList[i].buyin);
    //            currentBigBlind = ConvertCurrencyInIntger(gameListResponse.gameList[i].big_blind);
    //            currentSmallBlind = ConvertCurrencyInIntger(gameListResponse.gameList[i].small_blind);


    //            //stackVal.text = gameListResponse.gameList[0].small_blind.ToString() + "/" + gameListResponse.gameList[0].big_blind.ToString();
    //            //buyInVal.text = gameListResponse.gameList[0].buyin.ToString();

    //            //currentStakeType = gameListResponse.gameList[0].stake_type;
    //            //currentBuyIn = gameListResponse.gameList[0].buyin;
    //            //currentBigBlind = gameListResponse.gameList[0].big_blind;
    //            //currentSmallBlind = gameListResponse.gameList[0].small_blind;

    //            //for (int i = 0; i < gameListResponse.gameList.Length; i++)
    //            //{
    //            //    gameList.GetChild(i).GetComponent<CashGameTableProperty>().stakeType = gameListResponse.gameList[i].stake_type;
    //            //    gameList.GetChild(i).GetComponent<CashGameTableProperty>().buyIn = gameListResponse.gameList[i].buyin;
    //            //    gameList.GetChild(i).GetComponent<CashGameTableProperty>().smallBlind = gameListResponse.gameList[i].small_blind;
    //            //    gameList.GetChild(i).GetComponent<CashGameTableProperty>().bigBlind = gameListResponse.gameList[i].big_blind;

    //            //    gameList.GetChild(i).GetChild(0).GetComponent<Text>().text = "LOW ("+ gameListResponse.gameList[i].small_blind +"/"+ gameListResponse.gameList[i].big_blind+") (Buy-In:" + gameListResponse.gameList[i].buyin + ")";
    //            //}

    //        }

    //    }
    //}

    public int ConvertCurrencyInIntger(string str)
    {
        int n = 0;

        if (str.Contains("K"))
        {
            str = str.Replace("K", string.Empty);
            n = int.Parse(str);
            n = n * 1000;
        }
        else if (str.Contains("k"))
        {
            str = str.Replace("k", string.Empty);
            n = int.Parse(str);
            n = n * 1000;
        }
        else if (str.Contains("M"))
        {
            str = str.Replace("M", string.Empty);
            n = int.Parse(str);
            n = n * 1000000;
        }
        else
        {
            n = int.Parse(str);
        }
        return n;
    }

    //public void ClickOnTable(Transform panel)
    //{
    //   currentStakeType = panel.GetComponent<CashGameTableProperty>().stakeType;
    //   currentBuyIn = panel.GetComponent<CashGameTableProperty>().buyIn;
    //   currentBigBlind = panel.GetComponent<CashGameTableProperty>().bigBlind;
    //   currentSmallBlind = panel.GetComponent<CashGameTableProperty>().smallBlind;
    //}

    public Text stackVal;
    public Text buyInVal;

    //
    public void ClickConfigSlider()
    {
        for (int i = 0; i < gameListResponse.gameList.Length; i++)
        {
            if (configTableSlider.value == i)
            {
                stackVal.text = gameListResponse.gameList[i].small_blind.ToString() + "/" + gameListResponse.gameList[i].big_blind.ToString();
                buyInVal.text = LanguageManager.Instance.GetTextValue("Buy in :") + " " + gameListResponse.gameList[i].buyin.ToString();

                currentStakeType = gameListResponse.gameList[i].stake_type;
                //currentBuyIn = gameListResponse.gameList[i].buyin;
                //currentBigBlind = gameListResponse.gameList[i].big_blind;
                //currentSmallBlind = gameListResponse.gameList[i].small_blind;

                currentBuyIn = ConvertCurrencyInIntger(gameListResponse.gameList[i].buyin);
                currentBigBlind = ConvertCurrencyInIntger(gameListResponse.gameList[i].big_blind);
                currentSmallBlind = ConvertCurrencyInIntger(gameListResponse.gameList[i].small_blind);
                currentFees = gameListResponse.gameList[i].fees;
                currentFeeCap = gameListResponse.gameList[i].fee_cap;
                currentMinBuyIn = gameListResponse.gameList[i].min_buy_in;
                currentMaxBuyIn = gameListResponse.gameList[i].max_buy_in;
            }

        }
    }

    //public void ClickConfigSlider()
    //{
    //    for (int i = 0; i < gameListResponse.gameList.Length; i++)
    //    {
    //        if (configTableSlider.value == i)
    //        {
    //            stackVal.text = gameListResponse.gameList[i].small_blind.ToString() + "/" + gameListResponse.gameList[i].big_blind.ToString();
    //            buyInVal.text = gameListResponse.gameList[i].buyin.ToString();

    //            currentStakeType = gameListResponse.gameList[i].stake_type;
    //            currentBuyIn = gameListResponse.gameList[i].buyin;
    //            currentBigBlind = gameListResponse.gameList[i].big_blind;
    //            currentSmallBlind = gameListResponse.gameList[i].small_blind;
    //        }

    //    }
    //}

    public void IncreaseConfigSlider()
    {
        configTableSlider.value++;
        ClickConfigSlider();
    }

    public void decreaseConfigSlider()
    {
        configTableSlider.value--;
        ClickConfigSlider();
    }

    public Button joinRegularTableBtn;
    IEnumerator EnablejoinRegularTableBtn()
    {
        yield return new WaitForSeconds(5f);
        joinRegularTableBtn.interactable = true;
    }
    public string VideoCount_SignUp = "videocount";
    public GameObject tickImage;
    public bool isTickImage;

    public void CheckForDisclaimer()
    {
        //print("disclaimerpanel" + PlayerPrefs.GetString("disclaimerpanel"));

        if (currentIsVideo && PlayerPrefs.GetString("disclaimerpanel") == "1")
        {
            PlayerPrefs.SetInt(VideoCount_SignUp, PlayerPrefs.GetInt(VideoCount_SignUp) + 1);
            print("PANELCOUNT...." + PlayerPrefs.GetInt(VideoCount_SignUp));
            if (PlayerPrefs.GetInt(VideoCount_SignUp, 0) <= 5)
            {
                disclaimerPanel.SetActive(true);
            }
            else
            {
                PlayerPrefs.SetString("disclaimerpanel", "2");
                StartGame();
            }
        }
        else
        {
            StartGame();
        }
    }
    public void CloseDisclaimer()
    {
        PlayerPrefs.SetInt(VideoCount_SignUp, PlayerPrefs.GetInt(VideoCount_SignUp) - 1);
        disclaimerPanel.SetActive(false);
    }

    public void SwitchToClassicTable()
    {
        if (isTickImage)
        {
            PlayerPrefs.SetString("disclaimerpanel", "2");
        }
        currentIsVideo = false;
        StartGame();
    }

    public void DontShowDisclaimer(GameObject obj)
    {
        if (!obj.activeInHierarchy)
        {
            isTickImage = true;
            tickImage.SetActive(true);
        }
        else
        {
            isTickImage = false;
            tickImage.SetActive(false);
        }
    }

    public void StartGame()
    {
        ClubManagement.instance.loadingPanel.SetActive(true);
        if (isTickImage)
        {
            PlayerPrefs.SetString("disclaimerpanel", "2");
            //StartGame();
        }

        //if (PlayerPrefs.GetInt("SoundOffOn") == 0)
        //{
        //    SoundManager.instance.SoundButtonNonVideo.transform.GetChild(2).gameObject.SetActive(false);
        //    SoundManager.instance.SoundButtonNonVideo.transform.GetChild(3).gameObject.SetActive(true);
        //    SoundManager.instance.SoundButtonVideo.transform.GetChild(2).gameObject.SetActive(false);
        //    SoundManager.instance.SoundButtonVideo.transform.GetChild(3).gameObject.SetActive(true);

        //}
        //else if (PlayerPrefs.GetInt("SoundOffOn") == 1)
        //{
        //    SoundManager.instance.SoundButtonNonVideo.transform.GetChild(2).gameObject.SetActive(true);
        //    SoundManager.instance.SoundButtonNonVideo.transform.GetChild(3).gameObject.SetActive(false);
        //    SoundManager.instance.SoundButtonVideo.transform.GetChild(2).gameObject.SetActive(true);
        //    SoundManager.instance.SoundButtonVideo.transform.GetChild(3).gameObject.SetActive(false);
        //}

        if (!GameManagerScript.instance.isTournament)
        {
            joinRegularTableBtn.interactable = false;
            StartCoroutine(EnablejoinRegularTableBtn());

            //if (GameManagerScript.instance.isSocialPokerDev)
            //{
            //    GameManagerScript.instance.socket.url = GameManagerScript.instance.socialPokerDevUrl;
            //}
            //if (GameManagerScript.instance.isSocialPokerStage)
            //{
            //    GameManagerScript.instance.socket.url = GameManagerScript.instance.socialPokerStageUrl;
            //}

            //if (ServerChanger.instance.isAWS)
            //{
            //    GameManagerScript.instance.socket.url = GameManagerScript.instance.socialPokerUrl;
            //}
            //if (ServerChanger.instance.isAWS_Live)
            //{
            //    GameManagerScript.instance.socket.url = GameManagerScript.instance.socialPokerUrl;
            //}

            GameManagerScript.instance.socket.url = GameManagerScript.instance.socialPokerUrl;

            GameSerializeClassesCollection.instance.enterInSocialGame.stake_type = currentStakeType;
            GameSerializeClassesCollection.instance.enterInSocialGame.token = Communication.instance.playerToken;
            GameSerializeClassesCollection.instance.enterInSocialGame.buyin = currentBuyIn;
            GameSerializeClassesCollection.instance.enterInSocialGame.small_blind = currentSmallBlind;
            GameSerializeClassesCollection.instance.enterInSocialGame.big_blind = currentBigBlind;
            GameSerializeClassesCollection.instance.enterInSocialGame.fees = currentFees;
            GameSerializeClassesCollection.instance.enterInSocialGame.fee_cap = currentFeeCap;
            GameSerializeClassesCollection.instance.enterInSocialGame.user_image = ApiHitScript.instance.updatedUserImageUrl;
            //GameSerializeClassesCollection.instance.enterInSocialGame.video = true;

            GameSerializeClassesCollection.instance.enterInSocialGame.min_buy_in = currentMinBuyIn;
            GameSerializeClassesCollection.instance.enterInSocialGame.max_buy_in = currentMaxBuyIn;
            GameSerializeClassesCollection.instance.enterInSocialGame.user_table_match_preference = isTableMatchPreference;

            if (currentIsVideo)
            {
                GameSerializeClassesCollection.instance.enterInSocialGame.video = true;
                GameManagerScript.instance.isVideoTable = true;
            }
            else
            {
                GameSerializeClassesCollection.instance.enterInSocialGame.video = false;
                GameManagerScript.instance.isVideoTable = false;
            }

            string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.enterInSocialGame);
            print("Data...." + data);
            GameManagerScript.instance.Tournamentsocket.gameObject.SetActive(false);
            GameManagerScript.instance.tournamentManager.SetActive(false);
            GameManagerScript.instance.socket.gameObject.SetActive(true);
            GameManagerScript.instance.networkManager.SetActive(true);
            PokerNetworkManager.instance.EnterInPokerSocialTable(data);
            //socket.Emit("__watch_table", new JSONObject(data));
        }

        else
        {
            //if (GameManagerScript.instance.isSocialPokerDev)
            //{

            //    GameManagerScript.instance.Tournamentsocket.url = "ws://23.23.68.112:7575/socket.io/?EIO=3&transport=websocket";
            //    //GameManagerScript.instance.Tournamentsocket.url = "wss://poker-face.kelltontech.net:6565/socket.io/?EIO=3&transport=websocket"; //.....for aws live...//
            //    print(GameManagerScript.instance.Tournamentsocket.url);
            //}
            //if (GameManagerScript.instance.isSocialPokerStage)
            //{
            //    GameManagerScript.instance.Tournamentsocket.url = GameManagerScript.instance.socialPokerTournamentStageUrl;
            //}

            //if (ServerChanger.instance.isAWS)
            //{
            //    GameManagerScript.instance.Tournamentsocket.url = GameManagerScript.instance.socialPokerTournamentUrl;
            //}

            //if (ServerChanger.instance.isAWS_Live)
            //{
            //    GameManagerScript.instance.Tournamentsocket.url = GameManagerScript.instance.socialPokerTournamentUrl;
            //}

            GameManagerScript.instance.Tournamentsocket.url = GameManagerScript.instance.socialPokerTournamentUrl;

            if (SocialTournamentScript.instance.isTournamentVideo)
            {
                GameManagerScript.instance.isVideoTable = true;
            }
            else
            {
                GameManagerScript.instance.isVideoTable = false;
            }


            GameManagerScript.instance.socket.gameObject.SetActive(false);
            GameManagerScript.instance.networkManager.SetActive(false);
            GameManagerScript.instance.Tournamentsocket.gameObject.SetActive(true);
            GameManagerScript.instance.tournamentManager.SetActive(true);
            TournamentManagerScript.instance.ConnectToServer();
        }
    }

    public string ConvertChipsToString(string str)
    {
        if (str.Contains("$"))
        {
            str = str.Replace("$", string.Empty);
        }
        if (str.Contains(","))
        {
            str = str.Replace(",", string.Empty);
        }

        if (str.Contains("K"))
        {
            str = str.Replace("K", string.Empty);
            float totalPotvalue = float.Parse(str);
            totalPotvalue *= 1000;
            str = totalPotvalue.ToString();
        }

        if (str.Contains("M"))
        {
            str = str.Replace("M", string.Empty);
            float totalPotvalue = float.Parse(str);
            totalPotvalue *= 1000000;
            str = totalPotvalue.ToString();
        }

        return str;
    }

    public void TableMatchUI()
    {
        //...................................................//

        if(TableMatchOnOff.transform.GetChild(1).gameObject.activeInHierarchy)//...off...//
        {
            TableMatchOnOff.transform.GetChild(1).gameObject.SetActive(false);
            TableMatchOnOff.transform.GetChild(2).gameObject.SetActive(true);

            isTableMatchPreference = false;

            PlayerPrefs.SetInt(ApiHitScript.TableMatch, ApiHitScript.TableMatch_False);

        }
        else //...on...//
        {
            TableMatchOnOff.transform.GetChild(2).gameObject.SetActive(false);
            TableMatchOnOff.transform.GetChild(1).gameObject.SetActive(true);

            isTableMatchPreference = true;
            PlayerPrefs.SetInt(ApiHitScript.TableMatch, ApiHitScript.TableMatch_True);
        }


        //...................................................//
    }

}
