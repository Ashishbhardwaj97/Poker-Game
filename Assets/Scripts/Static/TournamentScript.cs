using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
public class TournamentScript : MonoBehaviour
{
    public static TournamentScript instance;

    [Header("Text References")]
    public Text tableSizeText;
    public Text actionTimeText;
    public Text feesText;
    public Text buyInText;
    public Text reBuyText;
    public Text addOnText;
    public Text addOnBreakText;
    public Text bountyBuyInText;
    public Text startingChipsText;
    public Text bindsUpText;
    public Text lateRegText;
    public Text breakText;
    public Text inEveryMinText;
    public Text playerNumberLeftText;
    public Text playerNumberRightText;
    public Text maxTableSizeText;
    public Text sateliteTournamentText;

    [Header("Slider References")]
    public Slider tableSizeSlider;
    public Slider actionTimeSlider;
    public Slider feesSlider;
    public Slider buyInSlider;
    public Slider reBuySlider;
    public Slider addOnSlider;
    public Slider addOnBreakSlider;
    public Slider bountyBuyInSlider;
    public Slider startingChipsSlider;
    public Slider blindsUpSlider;
    public Slider lateRegSlider;
    public Slider breakSlider;
    public Slider inEveryMinSlider;
    public RangeSlider playerNumberRangeSlider;

    [Header("Input Field References")]
    public InputField tableNameInputField;
    public InputField tableDescInputField;
    public InputField prizePoolInputField;

    [Header("Transform References")]
    public Transform standardObj;
    public Transform turboObj;
    public Transform payoutStructureObj;
    public Transform dropDownArrowObj;
    public Transform lineObj;
    public Transform videoModeObj;
    public Transform sateliteObj;
    public Transform koBountyObj;
    public Transform prizePoolObj;
    public Transform startDateTimeObj;

    [Header("GameObject References")]
    public GameObject tournamentObj;
    public GameObject createTableRegObj;
    public GameObject satelitePopupObj;
    public GameObject sateliteTablePanel;
    public GameObject sateliteTableContent;
    public GameObject sateliteToggleButton;
    private GameObject scrollItemObj;

    internal bool videoModeBool;
    public bool seteliteBool;
    internal bool prizePoolBool;
    internal bool kOBountyBool;

    internal double prizePoolAmount;
    internal string blindStuctureVal;
    internal string payoutStuctureVal;
    internal string startDateVal;
    internal string startTimeVal;
    public int lateRegVal;
    public int rebuyVal;
    private int sateliteTableCount;
    public string addOnVal;
    public int buyInVal;
    public int blindUpVal;
    public int feeVal;
    //internal string createTableUrl;
    internal string currSelectedSatelliteRuleId;
    internal string currSelectedSatelliteTime;
    internal List<GameObject> generatedSateliteTableItems;

    [Serializable]
    public class TournamentCreateTableReq
    {
        public string club_id;
        public string game_type;
        public string table_type;
        public string table_name;
        public string description;
        public bool video_mode;
        public bool satelite;
        public int table_size;
        public int action_time;
        public int fee;
        public int min_buy_in;
        public int rebuy;
        public double addon;
        public int addon_break;
        public double bounty_buyin;
        public bool ko_bounty;
        public bool prize_pool;
        public double prize_pool_amount;
        public string blind_structure;
        public string payout_structure;
        public int starting_chips;
        public int blinds_up;
        public int late_registration;
        public int min_player_number;
        public int max_player_number;
        public int game_break;
        public int every_minute;
        public string start_date;
        public string start_time;
        public string main_tounament_id;
        public Users[] users;
    }

    [Serializable]
    public class Users { }

    [Serializable]
    public class CreateTableCheck
    {
        public bool error;
        public Errors errors;
    }

    [SerializeField] TournamentCreateTableReq tournamentCreateTableReq;
    [SerializeField] CreateTableCheck createTableCheck;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        blindStuctureVal = "standard";
        payoutStuctureVal = "heavy";
        //createTableUrl = ServerChanger.instance.domainURL + "api/v1/pokertable/create-rules";
        sateliteTableCount = 1;
        generatedSateliteTableItems = new List<GameObject>();
        boutyVal = 1f / 4f;
    }

    public void ClickOnBackButton()
    {
        tournamentObj.SetActive(false);
        createTableRegObj.SetActive(true);
        createTableRegObj.transform.GetChild(1).gameObject.SetActive(true);
        createTableRegObj.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(0).gameObject.SetActive(false);
        createTableRegObj.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
        createTableRegObj.transform.GetChild(1).GetChild(1).GetChild(1).GetChild(0).gameObject.SetActive(false);
        ResetCreateTableUI();
    }

    public void BlindStructureButton(Transform _transform)
    {
        
        standardObj.GetChild(1).gameObject.SetActive(false);
        turboObj.GetChild(1).gameObject.SetActive(false);
        _transform.GetChild(1).gameObject.SetActive(true);
        blindStuctureVal = _transform.tag;
    }

    public void PayoutStructureButton(Transform _transform)
    {
        payoutStructureObj.GetChild(0).GetChild(0).gameObject.SetActive(false);
        payoutStructureObj.GetChild(0).GetChild(1).gameObject.SetActive(false);
        payoutStructureObj.GetChild(0).GetChild(2).gameObject.SetActive(false);
        payoutStructureObj.GetChild(0).GetChild(3).gameObject.SetActive(false);
        

        if (_transform.CompareTag("Heavy"))
        {
            payoutStructureObj.GetChild(0).GetChild(1).gameObject.SetActive(true);
            payoutStuctureVal = "heavy";
        }
        else if (_transform.CompareTag("Balanced"))
        {
            payoutStructureObj.GetChild(0).GetChild(2).gameObject.SetActive(true);
            payoutStuctureVal = "balanced";
        }
        else if (_transform.CompareTag("Flat"))
        {
            payoutStructureObj.GetChild(0).GetChild(3).gameObject.SetActive(true);
            payoutStuctureVal = "flat";
        }

        _transform.GetChild(0).gameObject.SetActive(false);
        _transform.GetChild(1).gameObject.SetActive(true);

        payoutStructureObj.GetChild(1).gameObject.SetActive(false);
        payoutStructureObj.GetChild(2).gameObject.SetActive(false);
        dropDownArrowObj.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        lineObj.gameObject.SetActive(false);

        payoutStructureObj.GetChild(0).GetChild(6).gameObject.SetActive(false);
    }

    public void ClickOnPayoutStructureDropDown()
    {
        if (payoutStructureObj.GetChild(0).GetChild(1).gameObject.activeInHierarchy)
        {
            payoutStructureObj.GetChild(0).GetChild(1).gameObject.SetActive(true);
            payoutStructureObj.GetChild(1).GetChild(0).gameObject.SetActive(true);
            payoutStructureObj.GetChild(2).GetChild(0).gameObject.SetActive(true);

            payoutStructureObj.GetChild(1).GetChild(1).gameObject.SetActive(false);
            payoutStructureObj.GetChild(2).GetChild(1).gameObject.SetActive(false);
        }
        else if (payoutStructureObj.GetChild(0).GetChild(2).gameObject.activeInHierarchy)
        {
            payoutStructureObj.GetChild(0).GetChild(0).gameObject.SetActive(true);
            payoutStructureObj.GetChild(1).GetChild(0).gameObject.SetActive(true);
            payoutStructureObj.GetChild(2).GetChild(0).gameObject.SetActive(true);

            payoutStructureObj.GetChild(0).GetChild(1).gameObject.SetActive(false);
            payoutStructureObj.GetChild(2).GetChild(1).gameObject.SetActive(false);

        }
        else if (payoutStructureObj.GetChild(0).GetChild(3).gameObject.activeInHierarchy)
        {
            payoutStructureObj.GetChild(0).GetChild(0).gameObject.SetActive(true);
            payoutStructureObj.GetChild(1).GetChild(0).gameObject.SetActive(true);
            payoutStructureObj.GetChild(2).GetChild(0).gameObject.SetActive(true);

            payoutStructureObj.GetChild(0).GetChild(1).gameObject.SetActive(false);
            payoutStructureObj.GetChild(1).GetChild(1).gameObject.SetActive(false);
        }

        if (payoutStructureObj.GetChild(1).gameObject.activeInHierarchy)
        {
            payoutStructureObj.GetChild(1).gameObject.SetActive(false);
            payoutStructureObj.GetChild(2).gameObject.SetActive(false);
            dropDownArrowObj.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            lineObj.gameObject.SetActive(false);
            payoutStructureObj.GetChild(0).GetChild(6).gameObject.SetActive(false);
        }
        else
        {
            payoutStructureObj.GetChild(1).gameObject.SetActive(true);
            payoutStructureObj.GetChild(2).gameObject.SetActive(true);
            dropDownArrowObj.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
            lineObj.gameObject.SetActive(true);
            payoutStructureObj.GetChild(0).GetChild(6).gameObject.SetActive(true);
        }

        payoutStructureObj.GetChild(0).GetChild(2).gameObject.SetActive(false);
        payoutStructureObj.GetChild(0).GetChild(3).gameObject.SetActive(false);
    }

    public void VideoModeImage(GameObject gameObject)
    {
        if (!gameObject.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
            videoModeBool = false;
        }
        else
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
            videoModeBool = true;
        }

        if (videoModeBool)
        {
            tableSizeSlider.maxValue = 8;
            maxTableSizeText.text = "8";
        }
        else
        {
            tableSizeSlider.maxValue = 9;
            maxTableSizeText.text = "9";
        }
    }

    public void SeteliteImage(GameObject gameObject)
    {
        if (!gameObject.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
            seteliteBool = false;
            sateliteTournamentText.gameObject.SetActive(false);
            koBountyObj.GetChild(2).GetComponent<Button>().interactable = true;

        }
        else
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
            seteliteBool = true;
            GenerateSateliteTableItems();
            satelitePopupObj.gameObject.SetActive(true);
            koBountyObj.GetChild(2).GetComponent<Button>().interactable = false;

        }

    }

    public void PrizePoolImage(GameObject gameObject)
    {
        if (!gameObject.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
            prizePoolBool = false;
            prizePoolInputField.interactable = false;
        }
        else
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
            prizePoolBool = true;
            prizePoolInputField.interactable = true;
        }

    }
    public void KOBountyImage(GameObject gameObject)
    {
        if (!gameObject.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
            kOBountyBool = false;
            bountyBuyInSlider.interactable = false;
        }
        else
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
            kOBountyBool = true;
            bountyBuyInSlider.interactable = true;
        }

    }

    public void UpdateTableSizeVal()
    {
        tableSizeText.text = Convert.ToInt32(tableSizeSlider.value).ToString();
    }

    public void UpdateActionTimeVal()
    {
        actionTimeText.text = Convert.ToInt32(actionTimeSlider.value * 5).ToString();
    }

    public void UpdateFeesVal()
    {
        feesText.text = Convert.ToInt32(feesSlider.value).ToString();
    }
    public void UpdateBuyInVal()
    {
        buyInText.text = Convert.ToInt32(buyInSlider.value * 5).ToString();
    }

    int reBuyVal;
    public void UpdateReBuyVal()
    {
        reBuyText.text = Convert.ToInt32(reBuySlider.value).ToString();

        if (reBuyText.text == "6")
        {
            reBuyText.text = "No Limit";
            reBuyVal = 50;
        }
        else
        {
            reBuyVal = Convert.ToInt32(reBuySlider.value);
        }
    }
    public void UpdateAddOnVal()
    {
        addOnText.text = Convert.ToInt32(addOnSlider.value).ToString();

        if (addOnText.text == "1")
        {
            addOnText.text = "1x";
        }
        else if (addOnText.text == "2")
        {
            addOnText.text = "1.2x";
        }
        else if (addOnText.text == "3")
        {
            addOnText.text = "1.5x";
        }
        else if (addOnText.text == "4")
        {
            addOnText.text = "2x";
        }
        else if (addOnText.text == "5")
        {
            addOnText.text = "2.5x";
        }
        else if (addOnText.text == "6")
        {
            addOnText.text = "3x";
        }
        else if (addOnText.text == "7")
        {
            addOnText.text = "3.5x";
        }
        else if (addOnText.text == "8")
        {
            addOnText.text = "4x";
        }
    }

    double boutyVal;
    public void UpdateAddOnBreakVal()
    {
        addOnBreakText.text = Convert.ToInt32(addOnBreakSlider.value).ToString();
    }
    public void UpdateBountyBuyInVal()
    {
        bountyBuyInText.text = Convert.ToInt32(bountyBuyInSlider.value).ToString();

        if (bountyBuyInText.text == "0")
        {
            bountyBuyInText.text = "1/4";
            boutyVal = 1f / 4f;
        }
        else if (bountyBuyInText.text == "1")
        {
            bountyBuyInText.text = "1/3";
            boutyVal = Math.Round((1f / 3f), 2);
        }
        if (bountyBuyInText.text == "2")
        {
            bountyBuyInText.text = "1/2";
            boutyVal = 1f / 2f;
        }
        if (bountyBuyInText.text == "3")
        {
            bountyBuyInText.text = "2/3";
            boutyVal = Math.Round((2f / 3f), 2);
        }
    }
    public void UpdateStartingChipsVal()
    {
        startingChipsText.text = Convert.ToInt32(startingChipsSlider.value).ToString();

        if (startingChipsText.text == "1000")
        {
            startingChipsText.text = "1k";
        }
        else if (startingChipsText.text == "1001")
        {
            startingChipsText.text = "1.5k";
        }
        else if (startingChipsText.text == "1002")
        {
            startingChipsText.text = "2k";
        }
        else if (startingChipsText.text == "1003")
        {
            startingChipsText.text = "3k";
        }
        else if (startingChipsText.text == "1004")
        {
            startingChipsText.text = "5k";
        }
        else if (startingChipsText.text == "1005")
        {
            startingChipsText.text = "7.5k";
        }
        else if (startingChipsText.text == "1006")
        {
            startingChipsText.text = "10k";
        }
        else if (startingChipsText.text == "1007")
        {
            startingChipsText.text = "12k";
        }
        else if (startingChipsText.text == "1008")
        {
            startingChipsText.text = "15k";
        }
        else if (startingChipsText.text == "1009")
        {
            startingChipsText.text = "20k";
        }
        else if (startingChipsText.text == "1010")
        {
            startingChipsText.text = "25k";
        }
        else if (startingChipsText.text == "1011")
        {
            startingChipsText.text = "30k";
        }
        else if (startingChipsText.text == "1012")
        {
            startingChipsText.text = "35k";
        }
        else if (startingChipsText.text == "1013")
        {
            startingChipsText.text = "40k";
        }
        else if (startingChipsText.text == "1014")
        {
            startingChipsText.text = "45k";
        }
        else if (startingChipsText.text == "1015")
        {
            startingChipsText.text = "50k";
        }
        else if (startingChipsText.text == "1016")
        {
            startingChipsText.text = "60k";
        }
        else if (startingChipsText.text == "1017")
        {
            startingChipsText.text = "75k";
        }
        else if (startingChipsText.text == "1018")
        {
            startingChipsText.text = "100k";
        }
    }
    public void UpdateBindsUpVal()
    {
        bindsUpText.text = Convert.ToInt32(blindsUpSlider.value).ToString();

        if (bindsUpText.text == "0")
        {
            bindsUpText.text = "2";
        }
        else if (bindsUpText.text == "1")
        {
            bindsUpText.text = "3";
        }
        else if (bindsUpText.text == "2")
        {
            bindsUpText.text = "4";
        }
        else if (bindsUpText.text == "3")
        {
            bindsUpText.text = "5";
        }
        else if (bindsUpText.text == "4")
        {
            bindsUpText.text = "6";
        }
        else if (bindsUpText.text == "5")
        {
            bindsUpText.text = "7";
        }
        else if (bindsUpText.text == "6")
        {
            bindsUpText.text = "8";
        }
        else if (bindsUpText.text == "7")
        {
            bindsUpText.text = "10";
        }
        else if (bindsUpText.text == "8")
        {
            bindsUpText.text = "12";
        }
        else if (bindsUpText.text == "9")
        {
            bindsUpText.text = "15";
        }
        else if (bindsUpText.text == "10")
        {
            bindsUpText.text = "20";
        }
        else if (bindsUpText.text == "11")
        {
            bindsUpText.text = "30";
        }
        else if (bindsUpText.text == "12")
        {
            bindsUpText.text = "45";
        }
    }
    public void UpdateLateRegistrationVal()
    {
        lateRegText.text = Convert.ToInt32(lateRegSlider.value).ToString();
    }
    public void UpdateBreakVal()
    {
        breakText.text = Convert.ToInt32(breakSlider.value).ToString();
    }
    public void UpdateInEveryMinVal()
    {
        inEveryMinText.text = Convert.ToInt32(inEveryMinSlider.value).ToString();
    }
    public void UpdatePlayerNumberVal()
    {
        playerNumberLeftText.text = Convert.ToInt32(playerNumberRangeSlider.LowValue).ToString();
        playerNumberRightText.text = Convert.ToInt32(playerNumberRangeSlider.HighValue).ToString();

        if (playerNumberLeftText.text == "0")
        {
            playerNumberLeftText.text = "2";
        }
        else if (playerNumberLeftText.text == "1")
        {
            playerNumberLeftText.text = "2";
        }
        else if (playerNumberLeftText.text == "2")
        {
            playerNumberLeftText.text = "3";
        }
        else if (playerNumberLeftText.text == "3")
        {
            playerNumberLeftText.text = "4";
        }
        else if (playerNumberLeftText.text == "4")
        {
            playerNumberLeftText.text = "5";
        }
        else if (playerNumberLeftText.text == "5")
        {
            playerNumberLeftText.text = "6";
        }
        else if (playerNumberLeftText.text == "6")
        {
            playerNumberLeftText.text = "7";
        }
        else if (playerNumberLeftText.text == "7")
        {
            playerNumberLeftText.text = "8";
        }
        else if (playerNumberLeftText.text == "8")
        {
            playerNumberLeftText.text = "9";
        }
        else if (playerNumberLeftText.text == "9")
        {
            playerNumberLeftText.text = "10";
        }

        //if (playerNumberLeftText.text == "0")
        //{
        //    playerNumberLeftText.text = "5";
        //}
        //else if (playerNumberLeftText.text == "1")
        //{
        //    playerNumberLeftText.text = "10";
        //}
        //else if (playerNumberLeftText.text == "2")
        //{
        //    playerNumberLeftText.text = "20";
        //}
        //else if (playerNumberLeftText.text == "3")
        //{
        //    playerNumberLeftText.text = "30";
        //}
        //else if (playerNumberLeftText.text == "4")
        //{
        //    playerNumberLeftText.text = "40";
        //}
        //else if (playerNumberLeftText.text == "5")
        //{
        //    playerNumberLeftText.text = "50";
        //}
        //else if (playerNumberLeftText.text == "6")
        //{
        //    playerNumberLeftText.text = "60";
        //}
        //else if (playerNumberLeftText.text == "7")
        //{
        //    playerNumberLeftText.text = "100";
        //}
        //else if (playerNumberLeftText.text == "8")
        //{
        //    playerNumberLeftText.text = "200";
        //}
        //else if (playerNumberLeftText.text == "9")
        //{
        //    playerNumberLeftText.text = "300";
        //}
        //else if (playerNumberLeftText.text == "10")
        //{
        //    playerNumberLeftText.text = "400";
        //}
        //else if (playerNumberLeftText.text == "11")
        //{
        //    playerNumberLeftText.text = "500";
        //}
        //else if (playerNumberLeftText.text == "12")
        //{
        //    playerNumberLeftText.text = "800";
        //}
        //else if (playerNumberLeftText.text == "13")
        //{
        //    playerNumberLeftText.text = "1000";
        //}
        //else if (playerNumberLeftText.text == "14")
        //{
        //    playerNumberLeftText.text = "2000";
        //}
        //else if (playerNumberLeftText.text == "15")
        //{
        //    playerNumberLeftText.text = "3000";
        //}
        //else if (playerNumberLeftText.text == "16")
        //{
        //    playerNumberLeftText.text = "4000";
        //}
        //else if (playerNumberLeftText.text == "17")
        //{
        //    playerNumberLeftText.text = "5000";
        //}

        //..............//

        if (playerNumberRightText.text == "0")
        {
            playerNumberRightText.text = "2";
        }
        else if (playerNumberRightText.text == "1")
        {
            playerNumberRightText.text = "2";
        }
        else if (playerNumberRightText.text == "2")
        {
            playerNumberRightText.text = "3";
        }
        else if (playerNumberRightText.text == "3")
        {
            playerNumberRightText.text = "4";
        }
        else if (playerNumberRightText.text == "4")
        {
            playerNumberRightText.text = "5";
        }
        else if (playerNumberRightText.text == "5")
        {
            playerNumberRightText.text = "5";
        }
        else if (playerNumberRightText.text == "6")
        {
            playerNumberRightText.text = "7";
        }
        else if (playerNumberRightText.text == "7")
        {
            playerNumberRightText.text = "8";
        }
        else if (playerNumberRightText.text == "8")
        {
            playerNumberRightText.text = "9";
        }
        else if (playerNumberRightText.text == "9")
        {
            playerNumberRightText.text = "10";
        }

        //if (playerNumberRightText.text == "0")
        //{
        //    playerNumberRightText.text = "2";
        //}
        //else if (playerNumberRightText.text == "1")
        //{
        //    playerNumberRightText.text = "10";
        //}
        //else if (playerNumberRightText.text == "2")
        //{
        //    playerNumberRightText.text = "20";
        //}
        //else if (playerNumberRightText.text == "3")
        //{
        //    playerNumberRightText.text = "30";
        //}
        //else if (playerNumberRightText.text == "4")
        //{
        //    playerNumberRightText.text = "40";
        //}
        //else if (playerNumberRightText.text == "5")
        //{
        //    playerNumberRightText.text = "50";
        //}
        //else if (playerNumberRightText.text == "6")
        //{
        //    playerNumberRightText.text = "60";
        //}
        //else if (playerNumberRightText.text == "7")
        //{
        //    playerNumberRightText.text = "100";
        //}
        //else if (playerNumberRightText.text == "8")
        //{
        //    playerNumberRightText.text = "200";
        //}
        //else if (playerNumberRightText.text == "9")
        //{
        //    playerNumberRightText.text = "300";
        //}
        //else if (playerNumberRightText.text == "10")
        //{
        //    playerNumberRightText.text = "400";
        //}
        //else if (playerNumberRightText.text == "11")
        //{
        //    playerNumberRightText.text = "500";
        //}
        //else if (playerNumberRightText.text == "12")
        //{
        //    playerNumberRightText.text = "800";
        //}
        //else if (playerNumberRightText.text == "13")
        //{
        //    playerNumberRightText.text = "1000";
        //}
        //else if (playerNumberRightText.text == "14")
        //{
        //    playerNumberRightText.text = "2000";
        //}
        //else if (playerNumberRightText.text == "15")
        //{
        //    playerNumberRightText.text = "3000";
        //}
        //else if (playerNumberRightText.text == "16")
        //{
        //    playerNumberRightText.text = "4000";
        //}
        //else if (playerNumberRightText.text == "17")
        //{
        //    playerNumberRightText.text = "5000";
        //}
    }

    public GameObject[] allDayButtonList;
    public void ResetDayColor()
    {
        allDayButtonList = GameObject.FindGameObjectsWithTag("day");

        for (int i = 0; i < allDayButtonList.Length; i++)
        {
            var colors = allDayButtonList[i].transform.GetComponent<Button>().colors;
            colors.normalColor = new Color32(255, 255, 255, 0);
            allDayButtonList[i].transform.GetComponent<Button>().colors = colors;
        }
    }

    public void ClickOnCreateTable()
    {
        tournamentCreateTableReq.club_id = ClubManagement.instance._clubID;
        tournamentCreateTableReq.game_type = "NLH";
        tournamentCreateTableReq.table_type = "tournament-table";
        tournamentCreateTableReq.table_name = tableNameInputField.text;
        tournamentCreateTableReq.description = tableDescInputField.text;
        tournamentCreateTableReq.video_mode = videoModeBool;
        tournamentCreateTableReq.table_size = Convert.ToInt32(tableSizeSlider.value);
        tournamentCreateTableReq.action_time = Convert.ToInt32(actionTimeSlider.value * 5);
        tournamentCreateTableReq.fee = Convert.ToInt32(feesSlider.value);
        feeVal = Convert.ToInt32(feesSlider.value);
        tournamentCreateTableReq.min_buy_in = Convert.ToInt32(buyInSlider.value);
        tournamentCreateTableReq.rebuy = reBuyVal;
        buyInVal = tournamentCreateTableReq.min_buy_in;
        rebuyVal = reBuyVal;
        string _addon = addOnText.text;
        addOnVal = _addon;
        if (_addon.Length >= 2)
        {
            _addon = _addon.Remove(_addon.Length - 1, 1);
        }
        tournamentCreateTableReq.addon = Math.Round(float.Parse(_addon), 2);
        tournamentCreateTableReq.addon_break = Convert.ToInt32(addOnBreakSlider.value);

        tournamentCreateTableReq.bounty_buyin = boutyVal;

        tournamentCreateTableReq.ko_bounty = kOBountyBool;
        tournamentCreateTableReq.prize_pool = prizePoolBool;
        if (!string.IsNullOrEmpty(prizePoolInputField.text))
        {
            tournamentCreateTableReq.prize_pool_amount = Math.Round(double.Parse(prizePoolInputField.text), 2);
            prizePoolAmount = tournamentCreateTableReq.prize_pool_amount;
        }
        tournamentCreateTableReq.blind_structure = blindStuctureVal;
        tournamentCreateTableReq.payout_structure = payoutStuctureVal;

        string _chips = startingChipsText.text;
        _chips = _chips.Remove(_chips.Length - 1, 1);
        tournamentCreateTableReq.starting_chips = (int)(float.Parse(_chips) * 1000f);

        tournamentCreateTableReq.blinds_up = Convert.ToInt32(bindsUpText.text);
        blindUpVal = Convert.ToInt32(bindsUpText.text);

        tournamentCreateTableReq.late_registration = Convert.ToInt32(lateRegSlider.value);
        lateRegVal = Convert.ToInt32(lateRegSlider.value);
        tournamentCreateTableReq.min_player_number = Convert.ToInt32(playerNumberLeftText.text);
        tournamentCreateTableReq.max_player_number = Convert.ToInt32(playerNumberRightText.text);

        tournamentCreateTableReq.game_break = Convert.ToInt32(breakSlider.value);
        tournamentCreateTableReq.every_minute = Convert.ToInt32(inEveryMinSlider.value);
        tournamentCreateTableReq.start_date = startDateVal;
        tournamentCreateTableReq.start_time = startTimeVal;
        tournamentCreateTableReq.satelite = seteliteBool;

        
        if (seteliteBool)
        {
            tournamentCreateTableReq.main_tounament_id = currSelectedSatelliteRuleId;
        }

        if (currSelectedSatelliteTime != null)
        {
            print("Final "+currSelectedSatelliteTime.Substring(11));
            tournamentTime = ConvertTimeToSeconds(currSelectedSatelliteTime.Substring(11));
        }

        if (seteliteBool && (tournamentTime - sateliteTime) >= 3600)
        {
            string body = JsonUtility.ToJson(tournamentCreateTableReq);
            print(body);
            ClubManagement.instance.loadingPanel.SetActive(true);
            //Communication.instance.PostData(createTableUrl, body, CreateTableProcess);
        }

        else if (!seteliteBool)
        {
            string body = JsonUtility.ToJson(tournamentCreateTableReq);
            print(body);
            ClubManagement.instance.loadingPanel.SetActive(true);
            //Communication.instance.PostData(createTableUrl, body, CreateTableProcess);
        }

        else
        {
            tournamentObj.transform.GetChild(2).gameObject.SetActive(true);
        }
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

    void CreateTableProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("error");
        }
        else
        {
            print("" + response);

            createTableCheck = JsonUtility.FromJson<CreateTableCheck>(response);

            if (!createTableCheck.error)
            {
                print("table create successful.......");
                print("ClubManagement.instance.clubId........" + ClubManagement.instance.clubId.text);

                ClubManagement.instance.DestroyGeneratedTableItem();
                ClubManagement.instance.ResetRegularTableItem();
                ClubManagement.instance.ResetTournamentTableItem();
                tournamentObj.SetActive(false);
                ClubManagement.instance.ClickOnClubDetails(ClubManagement.instance.clubId);
                ClubManagement.instance.myTableScrollContent.SetActive(true);
                ClubManagement.instance.clubHomeScreenScrollList.SetActive(true);
                ResetCreateTableUI();
            }
            else
            {
                Cashier.instance.toastMsg.text = createTableCheck.errors.properties.message;
                Cashier.instance.toastMsgPanel.SetActive(true);
                print("table create Unsuccessful.......");

            }


        }

    }

    public void ResetCreateTableUI()
    {
        tableNameInputField.text = string.Empty;
        tableDescInputField.text = string.Empty;
        prizePoolInputField.text = string.Empty;

        koBountyObj.GetChild(2).GetComponent<Button>().interactable = true;
        tableSizeSlider.value = tableSizeSlider.minValue;
        actionTimeSlider.value = actionTimeSlider.minValue;
        feesSlider.value = feesSlider.minValue;
        buyInSlider.value = buyInSlider.minValue;
        reBuySlider.value = reBuySlider.minValue;
        addOnSlider.value = addOnSlider.minValue;
        addOnBreakSlider.value = addOnBreakSlider.minValue;
        bountyBuyInSlider.value = bountyBuyInSlider.minValue;
        startingChipsSlider.value = startingChipsSlider.minValue;
        blindsUpSlider.value = blindsUpSlider.minValue;
        lateRegSlider.value = lateRegSlider.minValue;
        breakSlider.value = breakSlider.minValue;
        inEveryMinSlider.value = inEveryMinSlider.minValue;
        playerNumberRangeSlider.LowValue = playerNumberRangeSlider.MinValue;
        playerNumberRangeSlider.HighValue = playerNumberRangeSlider.MaxValue;

        tableSizeText.text = "2";
        actionTimeText.text = "0";
        feesText.text = "0";
        buyInText.text = "0";
        reBuyText.text = "0";
        addOnText.text = "0";
        addOnBreakText.text = "0";
        bountyBuyInText.text = "0";
        startingChipsText.text = "1k";
        bindsUpText.text = "2";
        lateRegText.text = "2";
        breakText.text = "2";
        inEveryMinText.text = "10";
        playerNumberLeftText.text = "2";
        playerNumberRightText.text = "10";

        videoModeObj.GetChild(0).gameObject.SetActive(true);
        videoModeObj.GetChild(1).gameObject.SetActive(false);

        sateliteObj.GetChild(0).gameObject.SetActive(true);
        sateliteObj.GetChild(1).gameObject.SetActive(false);

        koBountyObj.GetChild(0).gameObject.SetActive(true);
        koBountyObj.GetChild(1).gameObject.SetActive(false);

        prizePoolObj.GetChild(0).gameObject.SetActive(true);
        prizePoolObj.GetChild(1).gameObject.SetActive(false);

        startDateTimeObj.GetChild(0).GetComponent<Text>().text = "DD/MM/YY DAY     HH:MM";

        ClickOnPayoutStructureDropDown();

        payoutStructureObj.GetChild(0).GetChild(1).gameObject.SetActive(false);
        payoutStructureObj.GetChild(1).GetChild(1).gameObject.SetActive(false);
        payoutStructureObj.GetChild(2).GetChild(1).gameObject.SetActive(false);

        payoutStructureObj.GetChild(0).GetChild(1).gameObject.SetActive(true);
        payoutStructureObj.GetChild(1).GetChild(0).gameObject.SetActive(true);
        payoutStructureObj.GetChild(2).GetChild(0).gameObject.SetActive(true);

        BlindStructureButton(standardObj);
        sateliteTournamentText.gameObject.SetActive(false);
    }

    public void ClickSateliteTournamentName(Text name)
    {
        sateliteTournamentText.text = name.text;
        currSelectedSatelliteRuleId = name.transform.parent.GetChild(1).GetComponent<Text>().text;
        currSelectedSatelliteTime = name.transform.parent.GetChild(2).GetComponent<Text>().text;
        sateliteTournamentText.gameObject.SetActive(true);
        satelitePopupObj.SetActive(false);
    }

    public void ClickSateliteCloseBtn()
    {
        satelitePopupObj.SetActive(false);
        SeteliteImage(sateliteToggleButton);
    }

    void GenerateSateliteTableItems()
    {
        if (ClubManagement.instance.sateliteTableList.Count > 0)
        {

            if (ClubManagement.instance.sateliteTableList.Count != sateliteTableCount)
            {
                for (int i = sateliteTableCount; i < ClubManagement.instance.sateliteTableList.Count; i++)
                {
                    sateliteTableCount++;
                    GenerateSateliteTableItem();
                }
            }

            for (int i = 0; i < ClubManagement.instance.sateliteTableList.Count; i++)
            {
                sateliteTableContent.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = ClubManagement.instance.sateliteTableList[i];
                sateliteTableContent.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = ClubManagement.instance.sateliteTableListRuleId[i];
                sateliteTableContent.transform.GetChild(i).GetChild(2).GetComponent<Text>().text = ClubManagement.instance.sateliteTableListTime[i];

                sateliteTableContent.transform.GetChild(i).gameObject.SetActive(true);
            }

        }
    }

    public void GenerateSateliteTableItem()
    {
        scrollItemObj = Instantiate(sateliteTablePanel);
        scrollItemObj.transform.SetParent(sateliteTableContent.transform, false);
        generatedSateliteTableItems.Add(scrollItemObj);
    }

    public void DestroySateliteTableItem()
    {
        if (generatedSateliteTableItems.Count > 0)
        {
            for (int i = 0; i < generatedSateliteTableItems.Count; i++)
            {
                Destroy(generatedSateliteTableItems[i]);
            }
            generatedSateliteTableItems.Clear();
            sateliteTableCount = 1;
            sateliteTableContent.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public double ConvertTimeToSeconds(string _time)
    {
        string time = _time + ":00";
        double seconds = TimeSpan.Parse(time).TotalSeconds;
        print("WWW" + seconds);
        return seconds;
    }
    double sateliteTime;
    double tournamentTime;
    public void ConvertTimeToSecondsForCalender(string _time)
    {
        string time = _time + ":00";
        double seconds = TimeSpan.Parse(time).TotalSeconds;
        print("WWW" + seconds);
        sateliteTime = seconds;
    }
}
