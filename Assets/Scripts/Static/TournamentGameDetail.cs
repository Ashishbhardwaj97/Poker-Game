using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TournamentGameDetail : MonoBehaviour
{
    public static TournamentGameDetail instance;

    public GameObject detailImage;
    public GameObject tournamentCanvas;
    public GameObject pokerUICanvas;
    public GameObject tournamentGameDetail;
    public GameObject loaderPanel;
    public GameObject tableCancelledPanel;
    public Transform adminContent;
    public GameObject mttAdminObj;
    public GameObject bottomPanel;

    public List<GameObject> mTTGameScreens;
    public List<GameObject> mTTGameImages;
    public List<GameObject> mTTSidepanelGameScreens;
    public List<GameObject> mTTSidepanelGameImages;
    public List<GameObject> mTTSidepanelVideoGameScreens;
    public List<GameObject> mTTSidepanelVideoGameImages;

    //public string tournamentTableUrl;
    //public string tournamentRegisterUrl;
    //public string tournamentunregisterUrl;
    //public string tournamentEntriesUrl;
    //public string updateDateTimeUrl;
    //public string deleteTourneyUrl;

    //public float currSelectedFee;
    public bool currentSelectedKOBounty;
    public float currentSelectedBountyBuyIn;
    //public string currBuyIn;
    public bool isRegistered;
    public Text timerCountDownText;

    int tableCount;
    GameObject scrollTableItemObj;
    public Transform tableListingPanel;
    public Transform tableListingContent;
    public List<GameObject> tableList;

    int rankCount;
    GameObject scrollRankItemObj;
    public Transform RankListingPanel;
    public Transform RankListingContent;
    public List<GameObject> RankList;

    int rewardCount;
    GameObject scrollrewardItemObj;
    public Transform rewardListingPanel;
    public Transform rewardListingContent;
    public List<GameObject> rewardList;

    public IEnumerator currentCountDownCorotine;
    public IEnumerator tournamentCountDownCorotine;

    public Transform rankListingPanelKO;
    public Transform rankListingContentKO;

    private void Awake()
    {
        instance = this;
    }

    [Serializable]
    public class RuleId
    {
        public string rule_id;
    }

    [Serializable]
    public class TournamentDetails
    {
        public bool error;
        public bool flag;
        public Data data;
    }

    [Serializable]
    public class Data
    {
        public string rebuy;
        public string addon;
        public string prize_pool_amount;
        public string blind_structure;
        public string starting_chips;
        public string blinds_up;
        public string late_registration;
        public string game_type;
        public string table_name;
        public string table_size;
        public string start_date;
        public string start_time;
        public string min_buy_in;
        public float fee;
        public string rule_id;
        public int start_timestamp;
        public int min_player_number;
        public int max_player_number;
        public bool ko_bounty;
        public float bounty_buyin;
    }

    [Serializable]
    public class Unregister
    {
        public string rule_id;
        public bool error;
    }
    [SerializeField] Unregister unregister;

    [Serializable]
    public class Register
    {
        public string rule_id;
    }
    [SerializeField] Register register;

    [Serializable]
    public class RegisterResponse
    {
        public bool error;
        public Errors errors;
        public bool is_registerd;
    }

    [Serializable]
    public class Errors
    {
        public Properties properties;
    }

    [Serializable]
    public class Properties
    {
        public string message;
    }

    [SerializeField] RegisterResponse registerResponse;

    void Start()
    {
        //tournamentTableUrl = ServerChanger.instance.domainURL + "api/v1/pokertable/table-detail";
        //tournamentRegisterUrl = ServerChanger.instance.domainURL + "api/v1/pokertable/tournament-register";
        //tournamentunregisterUrl = ServerChanger.instance.domainURL + "api/v1/pokertable/tournament-unregister";
        //tournamentEntriesUrl = ServerChanger.instance.domainURL + "api/v1/pokertable/tournament-entries-list";
        //updateDateTimeUrl = ServerChanger.instance.domainURL + "api/v1/pokertable/update-start-time";
        //deleteTourneyUrl = ServerChanger.instance.domainURL + "api/v1/pokertable/delete-tournament";

        memberConut = 1;
        tableCount = 1;
        rankCount = 1;
        rewardCount = 1;
        winRankCount = 1;
        entryMemList = new List<GameObject>();
        winRankList = new List<GameObject>();
    }

    [SerializeField]
    public RuleId ruleId;

    public void RuleIdRequest()
    {
        print("Connect to Server2");
        loaderPanel.SetActive(true);
        tournamentCanvas.SetActive(true);
        pokerUICanvas.SetActive(false);
        ruleId.rule_id = ClubManagement.instance.currentSelectedRuleId;
        print("RULEID"+ruleId.rule_id);
        string body = JsonUtility.ToJson(ruleId);
        print("ASHISH BHARDWAJ" + "body : " + body);

        //Communication.instance.PostData(tournamentTableUrl, body, TournamentDataProcess);

        ClickOnTournamentEntry();
    }

    Coroutine timerGameDetail;

    [SerializeField] public TournamentDetails tournamentDetails;

    string entryFees;
    float deductedFees, prizePoolAfterdeduction;
    public float bounty;
    void TournamentDataProcess(string response)
    {
        loaderPanel.SetActive(false);
        detailImage.transform.GetChild(9).GetChild(1).gameObject.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("ASHISH BHARDWAJ" + "error");
        }
        else
        {
            print("Response" + response);
            tournamentDetails = JsonUtility.FromJson<TournamentDetails>(response);

            if (!tournamentDetails.error)
            {
                print("ASHISH BHARDWAJ" + tournamentDetails);
                detailImage.transform.GetChild(16).GetChild(0).GetComponent<Text>().text = tournamentDetails.data.rebuy;
                detailImage.transform.GetChild(17).GetChild(0).GetComponent<Text>().text = tournamentDetails.data.addon;
                detailImage.transform.GetChild(12).GetChild(0).GetComponent<Text>().text = tournamentDetails.data.prize_pool_amount;
                detailImage.transform.GetChild(15).GetChild(0).GetComponent<Text>().text = tournamentDetails.data.min_player_number + " " + "-" + " " + tournamentDetails.data.max_player_number;
                detailImage.transform.GetChild(19).GetChild(0).GetComponent<Text>().text = tournamentDetails.data.blind_structure;
                detailImage.transform.GetChild(18).GetChild(0).GetComponent<Text>().text = tournamentDetails.data.starting_chips;
                detailImage.transform.GetChild(3).GetChild(1).GetComponent<Text>().text = tournamentDetails.data.blinds_up;
                detailImage.transform.GetChild(4).GetChild(1).GetComponent<Text>().text = tournamentDetails.data.late_registration;
                detailImage.transform.GetChild(9).GetChild(0).GetComponent<Text>().text = tournamentDetails.data.game_type;
                detailImage.transform.GetChild(0).GetComponent<Text>().text = tournamentDetails.data.table_name;
                detailImage.transform.GetChild(10).GetChild(0).GetComponent<Text>().text = tournamentDetails.data.table_size;
               
                detailImage.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = ClubManagement.instance.currentSelectedStartDateTime + ":00";
                //currSelectedFee = tournamentDetails.data.fee;
                //currBuyIn = tournamentDetails.data.min_buy_in;

                currentSelectedKOBounty = tournamentDetails.data.ko_bounty;
                currentSelectedBountyBuyIn = tournamentDetails.data.bounty_buyin;

               
                deductedFees = (int.Parse(tournamentDetails.data.min_buy_in) * ((tournamentDetails.data.fee)) / 100);
                prizePoolAfterdeduction = (int.Parse(tournamentDetails.data.min_buy_in) - (int.Parse(tournamentDetails.data.min_buy_in) * ((tournamentDetails.data.fee)) / 100));
                bounty = prizePoolAfterdeduction * currentSelectedBountyBuyIn;

                if (currentSelectedKOBounty)
                {
                    entryFees = tournamentDetails.data.min_buy_in + "(" + Math.Round((prizePoolAfterdeduction - bounty), 1) + "+" + Math.Round(bounty, 1) + "+" + Math.Round(deductedFees, 1) + ")";
                    //detailImage.transform.GetChild(9).GetChild(0).GetComponent<Text>().text = tournamentDetails.data.game_type;
                    detailImage.transform.GetChild(9).GetChild(1).gameObject.SetActive(true);
                }
                else
                {
                    entryFees = tournamentDetails.data.min_buy_in + "(" + Math.Round(prizePoolAfterdeduction, 1) + "+" + Math.Round(deductedFees, 1) + ")";
                    detailImage.transform.GetChild(9).GetChild(1).gameObject.SetActive(false);
                }

                detailImage.transform.GetChild(11).GetChild(0).GetComponent<Text>().text = entryFees;

                //print("LATEREG"+tournamentDetails.data.late_registration);
                //print("CURRLEVEL"+Table.instance.table.currentLevel);
                //print("Status"+GameSerializeClassesCollection.instance.tournament.tournament_status);
                //print(tournamentDetails.flag);
                //if (GameSerializeClassesCollection.instance.tournament.tournament_status != 0 && tournamentDetails.flag && int.Parse(tournamentDetails.data.late_registration) >= Table.instance.table.currentLevel+1)
                //{
                //    print("Enter 1....");
                //    bottomPanel.transform.GetChild(1).gameObject.SetActive(false);
                //    bottomPanel.transform.GetChild(2).gameObject.SetActive(false);
                //    bottomPanel.transform.GetChild(3).gameObject.SetActive(false);
                //    bottomPanel.transform.GetChild(5).gameObject.SetActive(false);
                //    bottomPanel.transform.GetChild(6).gameObject.SetActive(false);
                //    bottomPanel.transform.GetChild(0).gameObject.SetActive(true);
                //}

                //else if (GameSerializeClassesCollection.instance.tournament.tournament_status != 0 && !tournamentDetails.flag && int.Parse(tournamentDetails.data.late_registration) >= Table.instance.table.currentLevel)
                //{
                //    bottomPanel.transform.GetChild(1).gameObject.SetActive(false);
                //    bottomPanel.transform.GetChild(2).gameObject.SetActive(true);
                //    bottomPanel.transform.GetChild(3).gameObject.SetActive(false);
                //    bottomPanel.transform.GetChild(5).gameObject.SetActive(false);
                //    bottomPanel.transform.GetChild(6).gameObject.SetActive(false);
                //    bottomPanel.transform.GetChild(0).gameObject.SetActive(false);
                //}

                if (tournamentDetails.flag)         //flag to check whether user is registered or not.
                {
                    print("Enter 222....");
                    isRegistered = true;
                    if (ClubManagement.instance.currentSelectedClubRole == "Owner" || ClubManagement.instance.currentSelectedClubRole == "Manager")
                    {
                        bottomPanel.transform.GetChild(1).gameObject.SetActive(true);
                        bottomPanel.transform.GetChild(2).gameObject.SetActive(false);
                        bottomPanel.transform.GetChild(3).gameObject.SetActive(true);
                        bottomPanel.transform.GetChild(5).gameObject.SetActive(false);
                        bottomPanel.transform.GetChild(6).gameObject.SetActive(false);
                        bottomPanel.transform.GetChild(0).gameObject.SetActive(false);
                    }

                    else
                    {
                        bottomPanel.transform.GetChild(1).gameObject.SetActive(false);
                        bottomPanel.transform.GetChild(2).gameObject.SetActive(false);
                        bottomPanel.transform.GetChild(3).gameObject.SetActive(false);
                        bottomPanel.transform.GetChild(5).gameObject.SetActive(false);
                        bottomPanel.transform.GetChild(6).gameObject.SetActive(true);
                        bottomPanel.transform.GetChild(0).gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (ClubManagement.instance.currentSelectedClubRole == "Owner" || ClubManagement.instance.currentSelectedClubRole == "Manager")
                    {
                        bottomPanel.transform.GetChild(1).gameObject.SetActive(true);
                        bottomPanel.transform.GetChild(2).gameObject.SetActive(true);
                        bottomPanel.transform.GetChild(3).gameObject.SetActive(false);
                        bottomPanel.transform.GetChild(6).gameObject.SetActive(false);
                        bottomPanel.transform.GetChild(5).gameObject.SetActive(false);
                        bottomPanel.transform.GetChild(0).gameObject.SetActive(false);
                    }
                    else
                    {
                        bottomPanel.transform.GetChild(1).gameObject.SetActive(false);
                        bottomPanel.transform.GetChild(2).gameObject.SetActive(false);
                        bottomPanel.transform.GetChild(3).gameObject.SetActive(false);
                        bottomPanel.transform.GetChild(5).gameObject.SetActive(true);
                        bottomPanel.transform.GetChild(6).gameObject.SetActive(false);
                        bottomPanel.transform.GetChild(0).gameObject.SetActive(false);
                    }
                }

                //........Copy data into Admin panel.......//
                adminContent.GetChild(1).GetComponent<Text>().text = entryFees;
                adminContent.GetChild(3).GetComponent<Text>().text = tournamentDetails.data.blinds_up;
                adminContent.GetChild(5).GetComponent<Text>().text = tournamentDetails.data.starting_chips;
                adminContent.GetChild(7).GetComponent<Text>().text = tournamentDetails.data.late_registration;
                adminContent.GetChild(9).GetComponent<Text>().text = ClubManagement.instance.currentSelectedStartDateTime + ":00";
            }

            else
            {
                print("Network Error....");
            }
        }
        TournamentManagerScript.instance.IsRegisteredEmitter();
    }

    public void ClickOnRegister()
    {
        tournamentGameDetail.transform.GetChild(1).gameObject.SetActive(true);
        tournamentGameDetail.transform.GetChild(1).GetChild(0).GetChild(1).GetChild(3).GetComponent<Text>().text = ClubManagement.instance.currentSelectedStartDateTime + ":00";
        tournamentGameDetail.transform.GetChild(1).GetChild(0).GetChild(1).GetChild(1).GetComponent<Text>().text = detailImage.transform.GetChild(11).GetChild(0).GetComponent<Text>().text;
    }

    public void ClickOnUnregister()
    {
        loaderPanel.SetActive(true);
        unregister.rule_id = ClubManagement.instance.currentSelectedRuleId;
        string body = JsonUtility.ToJson(unregister);
        print("ASHISH BHARDWAJ" + "body : " + body);
        //Communication.instance.PostData(tournamentunregisterUrl, body, UnregisterProcess);
    }

    public void UnregisterProcess(string response)
    {
        loaderPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("ASHISH BHARDWAJ" + "error");
        }
        else
        {
            print("Unregister Response" + response);
            unregister = JsonUtility.FromJson<Unregister>(response);
            if (unregister.error)
            {
                print("Error");
            }
            else
            {
                tournamentDetails.flag = false;
                isRegistered = false;
                if (ClubManagement.instance.currentSelectedClubRole == "Owner" || ClubManagement.instance.currentSelectedClubRole == "Manager")
                {
                    bottomPanel.transform.GetChild(3).gameObject.SetActive(false);
                    bottomPanel.transform.GetChild(2).gameObject.SetActive(true);
                }
                else
                {
                    bottomPanel.transform.GetChild(6).gameObject.SetActive(false);
                    bottomPanel.transform.GetChild(5).gameObject.SetActive(true);
                }
                //TournamentManagerScript.instance.MttEntyEmitter();
                TournamentManagerScript.instance.IsRegisteredEmitter();
            }
        }
    }

    public void ConfirmRegistration()
    {
        StartCoroutine(ConfirmRejisterCoroutine());
        //loaderPanel.SetActive(true);
        //tournamentGameDetail.transform.GetChild(1).gameObject.SetActive(false);
        //register.rule_id = ClubManagement.instance.currentSelectedRuleId;

        //string body = JsonUtility.ToJson(register);
        //print("ASHISH BHARDWAJ" + "body : " + body);
        //Communication.instance.PostData(tournamentRegisterUrl, body, RegisterProcess);
    }

    IEnumerator ConfirmRejisterCoroutine()
    {
        tournamentGameDetail.transform.GetChild(1).gameObject.SetActive(false);
        yield return StartCoroutine(TournmentBotConnect());
        yield return new WaitForSeconds(1f);
        loaderPanel.SetActive(true);
        register.rule_id = ClubManagement.instance.currentSelectedRuleId;

        string body = JsonUtility.ToJson(register);
        print("ASHISH BHARDWAJ" + "body : " + body);
        //Communication.instance.PostData(tournamentRegisterUrl, body, RegisterProcess);
    }

    public IEnumerator TournmentBotConnect()
    {
        print("Tournament bots ");
        loaderPanel.SetActive(true);
        for (int i = 0; i < 3; i++)
        {
            GameSerializeClassesCollection.instance.tournament_id.tournament_id = ClubManagement.instance.currentSelectedRuleId;
            string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.tournament_id);
            print("__bots_socket " + data);
            yield return new WaitForSeconds(1f);

            TournamentManagerScript.instance.socket.Emit("__bots_socket", new JSONObject(data));

            yield return new WaitForSeconds(1f);
            TournamentManagerScript.instance.socket.Emit("disconnect");
            TournamentManagerScript.instance.socket.Close();
            yield return new WaitForSeconds(1f);
            TournamentManagerScript.instance.socket.Connect();
        }
        yield return new WaitForSeconds(1f);
        loaderPanel.SetActive(false);
    }

    public void RegisterProcess(string response)
    {
        loaderPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("ASHISH BHARDWAJ" + "error");
        }
        else
        {
            print("Response" + response);
            registerResponse = JsonUtility.FromJson<RegisterResponse>(response);
            if (!registerResponse.error)
            {
                isRegistered = true;
                if (ClubManagement.instance.currentSelectedClubRole == "Owner" || ClubManagement.instance.currentSelectedClubRole == "Manager")
                {
                    bottomPanel.transform.GetChild(2).gameObject.SetActive(false);
                    bottomPanel.transform.GetChild(3).gameObject.SetActive(true);
                }
                else
                {
                    bottomPanel.transform.GetChild(6).gameObject.SetActive(true);
                    bottomPanel.transform.GetChild(5).gameObject.SetActive(false);
                }
                //TournamentManagerScript.instance.MttEntyEmitter();
                TournamentManagerScript.instance.IsRegisteredEmitter();
            }
            else
            {
                Cashier.instance.toastMsg.text = registerResponse.errors.properties.message;
                Cashier.instance.toastMsgPanel.SetActive(true);
            }
        }
    }

    public void TimerCompleted()
    {
        print(isRegistered);

        if (isRegistered)         //flag to check whether user is registered or not.
        {
            print("Registered");
            bottomPanel.transform.GetChild(1).gameObject.SetActive(false);
            bottomPanel.transform.GetChild(2).gameObject.SetActive(false);
            bottomPanel.transform.GetChild(3).gameObject.SetActive(false);
            bottomPanel.transform.GetChild(5).gameObject.SetActive(false);
            bottomPanel.transform.GetChild(6).gameObject.SetActive(false);
            bottomPanel.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            print("Not Registered");
            bottomPanel.transform.GetChild(1).gameObject.SetActive(false);
            bottomPanel.transform.GetChild(2).gameObject.SetActive(false);
            bottomPanel.transform.GetChild(3).gameObject.SetActive(false);
            bottomPanel.transform.GetChild(5).gameObject.SetActive(true);
            bottomPanel.transform.GetChild(6).gameObject.SetActive(false);
            bottomPanel.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void CloseAllButtons()
    {
        bottomPanel.transform.GetChild(1).gameObject.SetActive(false);
        bottomPanel.transform.GetChild(2).gameObject.SetActive(false);
        bottomPanel.transform.GetChild(3).gameObject.SetActive(false);
        bottomPanel.transform.GetChild(5).gameObject.SetActive(false);
        bottomPanel.transform.GetChild(6).gameObject.SetActive(false);
        bottomPanel.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void Enter()
    {
        tournamentCanvas.SetActive(false);
        if (GameManagerScript.instance.isVideoTable)
        {
            Screen.orientation = ScreenOrientation.Landscape;
        }
        pokerUICanvas.SetActive(true);
    }

    public void GameDetailBack()
    {
        StopCurrentCountDownCorotine();
        StopTournamentCountDownCorotine();
        TournamentManagerScript.instance.socket.Emit("disconnect");
        TournamentManagerScript.instance.socket.Close();
        pokerUICanvas.SetActive(true);
        tournamentCanvas.SetActive(false);

        TableTimer.instance.StopRemainingTimeCoroutine();
        TableTimer.instance.activeTournamentTimer = false;
        GameManagerScript.instance.Tournamentsocket.gameObject.SetActive(false);
        GameManagerScript.instance.tournamentManager.SetActive(false);
        ResetEntriesList();
        ResetTableList();
        ResetRankList();
        ResetRewardsList();
        Table.instance.ResetRankList();
        Table.instance.ResetTableList();
        Table.instance.ResetRewardsList();
        if (GameManagerScript.instance.activeTable != null)
        {
            GameManagerScript.instance.activeTable.SetActive(false);
        }
        ClubManagement.instance.ClickOnClubDetails(ClubManagement.instance.clubId);
    }

    public void StartTournamentCountDownCorotine()
    {
        if (tournamentCountDownCorotine != null)
        {
            StartCoroutine(tournamentCountDownCorotine);
        }
    }

    public void StopTournamentCountDownCorotine()
    {
        if (tournamentCountDownCorotine != null)
        {
            StopCoroutine(tournamentCountDownCorotine);
        }
    }

    public void StopCurrentCountDownCorotine()
    {
        if (currentCountDownCorotine != null)
        {
            StopCoroutine(currentCountDownCorotine);
        }
    }

    public void StartCurrentCountDownCorotine()
    {
        if (currentCountDownCorotine != null)
        {
            StartCoroutine(currentCountDownCorotine);
        }
    }

    public void TableCancelledOK()
    {
        TournamentManagerScript.instance.socket.Emit("disconnect");
        TournamentManagerScript.instance.socket.Close();
        pokerUICanvas.SetActive(true);
        tournamentCanvas.SetActive(false);
        ResetEntriesList();
        ResetTableList();
        ResetRankList();
        ResetRewardsList();
        Table.instance.ResetRankList();
        Table.instance.ResetTableList();
        Table.instance.ResetRewardsList();
        TableTimer.instance.StopRemainingTimeCoroutine();
        TableTimer.instance.activeTournamentTimer = false;
        GameManagerScript.instance.Tournamentsocket.gameObject.SetActive(false);
        GameManagerScript.instance.tournamentManager.SetActive(false);
        GameManagerScript.instance.videoTable.SetActive(false);
        GameManagerScript.instance.NonVideoTable.SetActive(false);
        GameManagerScript.instance.activeTable = null;

        Transform _parent = Uimanager.instance.currentTable.transform.parent;
        Uimanager.instance.currentTable.transform.SetParent(transform);
        Uimanager.instance.currentTable.transform.SetParent(_parent);
        GameObject objInMTT = FindGameObjectInMTT();

        if (objInMTT != null)
        {
            Transform parent = objInMTT.transform.parent;
            objInMTT.transform.SetParent(transform);
            objInMTT.transform.SetParent(parent);
            objInMTT.SetActive(false);
        }
    }

    public void MttSidePanelInfo()
    {
        //TournamentManagerScript.instance.MttEntyEmitter();
        TournamentManagerScript.instance.RankingListEmitter();
        UIManagerScript.instance.mttSideInfoPanel.transform.GetChild(4).GetChild(4).GetComponent<Text>().text = TournamentScript.instance.rebuyVal.ToString();
        UIManagerScript.instance.mttSideInfoPanel.transform.GetChild(2).GetChild(4).GetComponent<Text>().text = "Level" + " " + TournamentScript.instance.lateRegVal.ToString();
        UIManagerScript.instance.mttSideInfoPanel.transform.GetChild(5).GetChild(4).GetComponent<Text>().text = TournamentScript.instance.addOnVal;
    }

    #region Tournament Table Listing
    public void TableListing()
    {
        print("TableList: Ashish");
        if (GameSerializeClassesCollection.instance.mttTableListingData.data.Length != tableCount)
        {
            for (int i = tableCount; i < GameSerializeClassesCollection.instance.mttTableListingData.data.Length; i++)
            {
                tableCount++;
                GenerateTableMembersItem();
            }
        }

        for (int i = 0; i < GameSerializeClassesCollection.instance.mttTableListingData.data.Length; i++)
        {
            //tableListingContent.GetChild(i).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttTableListingData.data[i].table_no;
            //tableListingContent.GetChild(i).GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttTableListingData.data[i].current_players;
            //tableListingContent.GetChild(i).GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttTableListingData.data[i].min_chips;
            //tableListingContent.GetChild(i).GetChild(0).GetChild(2).GetChild(1).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttTableListingData.data[i].max_chips;

            //tableListingContent.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void GenerateTableMembersItem()
    {
        scrollItemObj = Instantiate(tableListingPanel.gameObject);
        scrollItemObj.transform.SetParent(tableListingContent, false);
        tableList.Add(scrollItemObj);
    }

    public void ResetTableList()
    {
        if (tableList.Count > 0)
        {
            for (int i = 0; i < tableList.Count; i++)
            {
                Destroy(tableList[i]);
            }
            tableList.Clear();
            tableCount = 1;
        }
        for(int i = 0; i < tableListingContent.transform.childCount; i++)
        {
            tableListingContent.GetChild(i).gameObject.SetActive(false);
        }
    }
    #endregion

    #region Tournament Ranking Listing
    public void RankingListing()
    {
        if (GameSerializeClassesCollection.instance.mttRankingListingData.data.Length != rankCount)
        {
            for (int i = rankCount; i < GameSerializeClassesCollection.instance.mttRankingListingData.data.Length; i++)
            {
                rankCount++;
                GenerateRankingMembersItem();
            }
        }

        for (int i = 0; i < GameSerializeClassesCollection.instance.mttRankingListingData.data.Length; i++)
        {
            //if (!currentSelectedKOBounty)
            //{
            //    RankListingContent.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
            //    RankListingContent.GetChild(i).GetChild(0).GetChild(1).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttRankingListingData.data[i].username;
            //    RankListingContent.GetChild(i).GetChild(0).GetChild(2).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttRankingListingData.data[i].tournament_chips;
            //    RankListingContent.GetChild(i).GetChild(0).GetChild(3).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttRankingListingData.data[i].no_addon_times + "A" + "+" + GameSerializeClassesCollection.instance.mttRankingListingData.data[i].no_rebuy_times + "R";

            //    RankListingContent.GetChild(i).gameObject.SetActive(true);
            //}
            //else
            //{
            //    rankListingContentKO.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
            //    rankListingContentKO.GetChild(i).GetChild(0).GetChild(1).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttRankingListingData.data[i].username;
            //    rankListingContentKO.GetChild(i).GetChild(0).GetChild(2).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttRankingListingData.data[i].tournament_chips;
            //    rankListingContentKO.GetChild(i).GetChild(0).GetChild(3).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttRankingListingData.data[i].no_addon_times + "A" + "+" + GameSerializeClassesCollection.instance.mttRankingListingData.data[i].no_rebuy_times + "R";

            //    rankListingContentKO.GetChild(i).GetChild(0).GetChild(4).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttRankingListingData.data[i].bounty.Length.ToString();

            //    rankListingContentKO.GetChild(i).gameObject.SetActive(true);
            //}
        }
    }

    public void GenerateRankingMembersItem()
    {
        if (!currentSelectedKOBounty)
        {
            scrollRankItemObj = Instantiate(RankListingPanel.gameObject);
            scrollRankItemObj.transform.SetParent(RankListingContent, false);
        }
        else
        {
            scrollRankItemObj = Instantiate(rankListingPanelKO.gameObject);
            scrollRankItemObj.transform.SetParent(rankListingContentKO, false);
        }
        RankList.Add(scrollRankItemObj);
    }

    public void ResetRankList()
    {
        if (RankList.Count > 0)
        {
            for (int i = 0; i < RankList.Count; i++)
            {
                Destroy(RankList[i]);
            }
            RankList.Clear();
            rankCount = 1;
        }
        for (int i = 0; i < RankListingContent.transform.childCount; i++)
        {
            RankListingContent.GetChild(i).gameObject.SetActive(false);
        }
    }


    #endregion

    #region Congratulation

    public Transform winRanklistPanel;
    public Transform winRanklistContent;
    public List<GameObject> winRankList;
    private int winRankCount;

    public void WinRankListing()
    {
        if (GameSerializeClassesCollection.instance.mttWinnerListingData.data[0].user_image != null)
        {
            Communication.instance.GetImage(GameSerializeClassesCollection.instance.mttWinnerListingData.data[0].user_image, GetWinnerImage);
        }

        if (currentSelectedKOBounty)
        {
            UIManagerScript.instance.winPanelTournmentKO.gameObject.SetActive(true);
            winRanklistContent = UIManagerScript.instance.winPanelTournmentKO.GetChild(0).GetChild(9).GetChild(0).GetChild(0).GetChild(0);
            winRanklistPanel = UIManagerScript.instance.winPanelTournmentKO.GetChild(0).GetChild(9).GetChild(0).GetChild(0).GetChild(0).GetChild(0);
        }
        else
        {
            UIManagerScript.instance.winPanelTournment.gameObject.SetActive(true);
            winRanklistContent = UIManagerScript.instance.winPanelTournment.GetChild(0).GetChild(7).GetChild(0).GetChild(0).GetChild(0);
            winRanklistPanel = UIManagerScript.instance.winPanelTournment.GetChild(0).GetChild(7).GetChild(0).GetChild(0).GetChild(0).GetChild(0);
        }

        if (GameSerializeClassesCollection.instance.mttWinnerListingData.data.Length != winRankCount)
        {
            for (int i = winRankCount; i < GameSerializeClassesCollection.instance.mttWinnerListingData.data.Length; i++)
            {
                winRankCount++;
                GenerateWinnerMembers();
            }
        }

        for (int i = 0; i < GameSerializeClassesCollection.instance.mttWinnerListingData.data.Length; i++)
        {
            winRanklistContent.GetChild(i).GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
            winRanklistContent.GetChild(i).GetChild(1).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttWinnerListingData.data[i].username;
            print("BOOL" + TournamentScript.instance.seteliteBool);
            if (TournamentScript.instance.seteliteBool)
            {
                print("Chips" + GameSerializeClassesCollection.instance.mttWinnerListingData.data[i].chips);
                print("Buyin" + GameSerializeClassesCollection.instance.mttWinnerListingData.main_tournament.min_buy_in);
                if (GameSerializeClassesCollection.instance.mttWinnerListingData.data[i].chips == GameSerializeClassesCollection.instance.mttWinnerListingData.main_tournament.min_buy_in)
                {
                    print("Enter1");
                    winRanklistContent.GetChild(i).GetChild(3).gameObject.SetActive(true);
                    winRanklistContent.GetChild(i).GetChild(2).gameObject.SetActive(false);
                }
                else
                {
                    print("Enter2");
                    winRanklistContent.GetChild(i).GetChild(2).gameObject.SetActive(true);
                    winRanklistContent.GetChild(i).GetChild(3).gameObject.SetActive(false);
                    winRanklistContent.GetChild(i).GetChild(2).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttWinnerListingData.data[i].chips.ToString();
                }
            }
            else
            {
                print("Enter3");
                winRanklistContent.GetChild(i).GetChild(2).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttWinnerListingData.data[i].chips_value;
            }
            //winRanklistContent.GetChild(i).GetChild(0).GetChild(3).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttWinnerListingData.data[i].no_addon_times + "A" + "+" + GameSerializeClassesCollection.instance.mttWinnerListingData.data[i].no_rebuy_times + "R";
            if (currentSelectedKOBounty)
            {
                //winRanklistContent.GetChild(i).GetChild(3).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttWinnerListingData.data[i].bounty.Length.ToString();
                winRanklistContent.GetChild(i).GetChild(3).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttWinnerListingData.data[i].player_bounty.ToString();
            }

            winRanklistContent.GetChild(i).gameObject.SetActive(true);
        }

        if (currentSelectedKOBounty)
        {
            UIManagerScript.instance.winPanelTournmentKO.GetChild(0).GetChild(3).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttWinnerListingData.data[0].username;
            UIManagerScript.instance.winPanelTournmentKO.GetChild(0).GetChild(4).GetComponent<Text>().text = "ID" + " " + GameSerializeClassesCollection.instance.mttWinnerListingData.data[0].client_id.ToString();
            //UIManagerScript.instance.winPanelTournmentKO.GetChild(0).GetChild(5).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttWinnerListingData.data[0].bounty.Length.ToString();
            UIManagerScript.instance.winPanelTournmentKO.GetChild(0).GetChild(5).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttWinnerListingData.data[0].knock_out.ToString();
            UIManagerScript.instance.winPanelTournmentKO.GetChild(0).GetChild(6).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttWinnerListingData.data[0].player_bounty.ToString();
            UIManagerScript.instance.winPanelTournmentKO.GetChild(0).GetChild(7).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttWinnerListingData.data[0].chips.ToString();
        }
        else
        {
            UIManagerScript.instance.winPanelTournment.GetChild(0).GetChild(3).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttWinnerListingData.data[0].username;
            UIManagerScript.instance.winPanelTournment.GetChild(0).GetChild(4).GetComponent<Text>().text = "ID" + " " + GameSerializeClassesCollection.instance.mttWinnerListingData.data[0].client_id.ToString();
            UIManagerScript.instance.winPanelTournment.GetChild(0).GetChild(5).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttWinnerListingData.data[0].chips.ToString();
        }
    }

    void GetWinnerImage(Sprite sprite)
    {
        if (sprite != null)
        {
            print("ProfileImage texture");
            UIManagerScript.instance.winPanelTournment.GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>().sprite = sprite;
        }
    }

    public void GenerateWinnerMembers()
    {
        scrollRankItemObj = Instantiate(winRanklistPanel.gameObject);
        scrollRankItemObj.transform.SetParent(winRanklistContent, false);
        winRankList.Add(scrollRankItemObj);
    }

    public void ResetWinnerList() 
    {
        try
        {
            if (winRankList.Count > 0)
            {
                for (int i = 0; i < winRankList.Count; i++)
                {
                    Destroy(winRankList[i]);
                }
                winRankList.Clear();
                winRankCount = 1;
            }
            for (int i = 0; i < winRanklistContent.transform.childCount; i++)
            {
                winRanklistContent.GetChild(i).gameObject.SetActive(false);
            }
        }
        catch
        {
            print("No Congratulation panel");
        }
        UIManagerScript.instance.winPanelTournment.gameObject.SetActive(false);
        UIManagerScript.instance.winPanelTournmentKO.gameObject.SetActive(false);
    }
    #endregion

    #region Tournament Rewards Listing
    public void RewardsListing()
    {
        if (GameSerializeClassesCollection.instance.mttRewardListingData.data.Length != rewardCount)
        {
            for (int i = rewardCount; i < GameSerializeClassesCollection.instance.mttRewardListingData.data.Length; i++)
            {
                rewardCount++;
                GenerateRewardsMembersItem();
            }
        }

        for (int i = 0; i < GameSerializeClassesCollection.instance.mttRewardListingData.data.Length; i++)
        {
            rewardListingContent.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttRewardListingData.data[i].rank.ToString();
            rewardListingContent.GetChild(i).GetChild(0).GetChild(1).GetComponent<Text>().text = GameSerializeClassesCollection.instance.mttRewardListingData.data[i].payout;

            rewardListingContent.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void GenerateRewardsMembersItem()
    {
        scrollrewardItemObj = Instantiate(rewardListingPanel.gameObject);
        scrollrewardItemObj.transform.SetParent(rewardListingContent, false);
        rewardList.Add(scrollrewardItemObj);
    }

    public void ResetRewardsList()
    {
        if (rewardList.Count > 0)
        {
            for (int i = 0; i < rewardList.Count; i++)
            {
                Destroy(rewardList[i]);
            }
            rewardList.Clear();
            rewardCount = 1;
        }
        for (int i = 0; i < rewardListingContent.transform.childCount; i++)
        {
            rewardListingContent.GetChild(i).gameObject.SetActive(false);
        }
    }
    #endregion

    #region Tournament Entry

    int memberConut;
    public Transform entryMemberPanel;
    public Transform entryMemberContent;
    public Text entriesCountText;

    GameObject scrollItemObj;
    public List<GameObject> entryMemList;

    [Serializable]
    public class EntryResponse
    {
        public bool error;
        public EntryData[] entriesData;
    }

    [Serializable]
    public class EntryData
    {
        public string username;
        public string client_id;
        public string user_image;
    }

    [SerializeField] EntryResponse entryResponse;

    public void ClickOnTournamentEntry()
    {
        //TournamentManagerScript.instance.MttEntyEmitter();

        ruleId.rule_id = ClubManagement.instance.currentSelectedRuleId;       
        string body = JsonUtility.ToJson(ruleId);
        print("body : " + body);
        //loaderPanel.SetActive(true);
        //Communication.instance.PostData(tournamentEntriesUrl, body, TournamentEntryProcess);
    }

    void TournamentEntryProcess(string response)
    {
        if (string.IsNullOrEmpty(response))
        {
            print("ASHISH BHARDWAJ" + "error");
        }
        else
        {
            loaderPanel.SetActive(false);
            print("MTT Entry Response" + response);
            entryResponse = JsonUtility.FromJson<EntryResponse>(response);

            if (!entryResponse.error)
            {
                if (entryResponse.entriesData.Length > 0)
                {
                    entriesCountText.text = entryResponse.entriesData.Length.ToString();
                    if (entryResponse.entriesData.Length != memberConut)
                    {
                        for (int i = memberConut; i < entryResponse.entriesData.Length; i++)
                        {
                            memberConut++;
                            GenerateEntryMembersItem();
                        }
                    }

                    userImage.Clear();

                    for (int i = 0; i < entryResponse.entriesData.Length; i++)
                    {
                        entryMemberContent.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = entryResponse.entriesData[i].username;
                        entryMemberContent.GetChild(i).GetChild(0).GetChild(1).GetComponent<Text>().text = entryResponse.entriesData[i].client_id;

                        userImage.Add(entryResponse.entriesData[i].user_image);
                        entryMemberContent.GetChild(i).gameObject.SetActive(true);
                    }

                    UpdatePlayerImage();
                }
                else
                {
                    entriesCountText.text = "0";
                }
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
                loaderPanel.SetActive(true);
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
            loaderPanel.SetActive(false);

            for (int i = 0; i < userImage.Count; i++)
            {
                entryMemberContent.GetChild(i).GetChild(0).GetChild(2).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
            }

            for (int i = 0; i < userImage.Count; i++)
            {
                if (!string.IsNullOrEmpty(userImage[i]))
                {
                    //print("i = " + i);
                    entryMemberContent.GetChild(i).GetChild(0).GetChild(2).GetComponent<Image>().sprite = playerImageInSequence[count].imgPic;
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


    void GenerateEntryMembersItem()
    {
        scrollItemObj = Instantiate(entryMemberPanel.gameObject);
        scrollItemObj.transform.SetParent(entryMemberContent, false);
        entryMemList.Add(scrollItemObj);
    }

    public void ResetEntriesList()
    {
        if (entryMemList.Count > 0)
        {
            for (int i = 0; i < entryMemList.Count; i++)
            {
                Destroy(entryMemList[i]);
            }
            entryMemList.Clear();
            memberConut = 1;
            ResetAllValuesForImage();
        }
    }
    #endregion

    #region Update Start Time 

    [Serializable]
    public class UpdateDateTimeReq
    {
        public string rule_id;
        public string start_date;
        public string start_time;
    }

    [SerializeField] UpdateDateTimeReq updateDateTimeReq;

    public void ClickUpdateDateTime(string date, string time)
    {
        updateDateTimeReq.rule_id = ClubManagement.instance.currentSelectedRuleId;
        updateDateTimeReq.start_date = date;
        updateDateTimeReq.start_time = time;

        string body = JsonUtility.ToJson(updateDateTimeReq);
        print(body);
        loaderPanel.SetActive(true);
        //Communication.instance.PostData(updateDateTimeUrl, body, UpdateDateTimeProcess);
    }

    void UpdateDateTimeProcess(string response)
    {
        if (string.IsNullOrEmpty(response))
        {
            print("ASHISH BHARDWAJ" + "error");
        }
        else
        {
            loaderPanel.SetActive(false);
            print("MTT Entry Response" + response);

            entryResponse = JsonUtility.FromJson<EntryResponse>(response);

            if (!entryResponse.error)
            {

            }
        }

    }
    #endregion

    #region Click on Head panel

    public void EnableSinglePanel(GameObject obj)
    {
        for (int i = 0; i < mTTGameScreens.Count; i++)
        {
            mTTGameScreens[i].SetActive(false);
        }

        obj.SetActive(true);
    }

    public void EnableSingleImage(GameObject img)
    {
        for (int i = 0; i < mTTGameImages.Count; i++)
        {
            mTTGameImages[i].SetActive(false);
        }
        img.SetActive(true);
    }

    public void ClickOnRanking()
    {
        if (currentSelectedKOBounty)
        {
            mTTGameScreens[2].transform.GetChild(0).gameObject.SetActive(false);
            mTTGameScreens[2].transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            mTTGameScreens[2].transform.GetChild(0).gameObject.SetActive(true);
            mTTGameScreens[2].transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    #endregion

    #region Click on MTT Sidepanel Head panel

    public void MTTEnableSinglePanel(GameObject obj)
    {
        for (int i = 0; i < mTTSidepanelGameScreens.Count; i++)
        {
            mTTSidepanelGameScreens[i].SetActive(false);
        }

        obj.SetActive(true);
    }

    public void MTTEnableSingleImage(GameObject img)
    {
        for (int i = 0; i < mTTSidepanelGameImages.Count; i++)
        {
            mTTSidepanelGameImages[i].SetActive(false);
        }
        img.SetActive(true);
    }

    public void MTTEnableVideoSinglePanel(GameObject obj)
    {
        for (int i = 0; i < mTTSidepanelVideoGameScreens.Count; i++)
        {
            mTTSidepanelVideoGameScreens[i].SetActive(false);
        }

        obj.SetActive(true);
    }

    public void MTTEnableVideoSingleImage(GameObject img)
    {
        for (int i = 0; i < mTTSidepanelVideoGameImages.Count; i++)
        {
            mTTSidepanelVideoGameImages[i].SetActive(false);
        }
        img.SetActive(true);
    }
    #endregion

    #region Delete Tourney

    public void ClickOnDeleteTourney()
    {
        ruleId.rule_id = ClubManagement.instance.currentSelectedRuleId;

        string body = JsonUtility.ToJson(ruleId);
        print("body : " + body);
        loaderPanel.SetActive(true);
        //Communication.instance.PostData(deleteTourneyUrl, body, DeleteTourneyProcess);
    }

    void DeleteTourneyProcess(string response)
    {
        loaderPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("ASHISH BHARDWAJ" + "error");
        }
        else
        {
            print("Response" + response);
            entryResponse = JsonUtility.FromJson<EntryResponse>(response);

            if (!entryResponse.error)
            {
                GameDetailBack();
                Uimanager.instance.currentTable.SetActive(false);
                Transform _parent = Uimanager.instance.currentTable.transform.parent;
                Uimanager.instance.currentTable.transform.SetParent(transform);
                Uimanager.instance.currentTable.transform.SetParent(_parent);

                GameObject objInMTT = FindGameObjectInMTT();

                if (objInMTT != null)
                {
                    Transform parent = objInMTT.transform.parent;
                    objInMTT.transform.SetParent(transform);
                    objInMTT.transform.SetParent(parent);
                    objInMTT.SetActive(false);
                }
            }
        }
    }

    GameObject FindGameObjectInMTT()
    {
        for (int i = 0; i < ClubManagement.instance.tournamentTableContent.transform.childCount; i++)
        {
            if (ClubManagement.instance.tournamentTableContent.transform.GetChild(i).gameObject.activeSelf)
            {
                if (ClubManagement.instance.tournamentTableContent.transform.GetChild(i).GetChild(27).GetComponent<Text>().text == ClubManagement.instance.currentSelectedRuleId)
                {
                    return ClubManagement.instance.tournamentTableContent.transform.GetChild(i).gameObject;
                }
            }
        }
        for (int i = 0; i < ClubManagement.instance.myTableScrollContent.transform.childCount; i++)
        {
            if (ClubManagement.instance.myTableScrollContent.transform.GetChild(i).gameObject.activeSelf)
            {
                if (ClubManagement.instance.myTableScrollContent.transform.GetChild(i).GetChild(27).GetComponent<Text>().text == ClubManagement.instance.currentSelectedRuleId)
                {
                    return ClubManagement.instance.myTableScrollContent.transform.GetChild(i).gameObject;
                }
            }
        }


        return null;
    }

    #endregion
}