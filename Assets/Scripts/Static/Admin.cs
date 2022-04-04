using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Admin : MonoBehaviour
{
    public static Admin instance;
    
    [Header("GameObject Reference")]
    public GameObject contryList;
    public GameObject contryFadeUI;
    public GameObject adminPanel;
    public GameObject menuFadePanel;
    public GameObject ownerMenuPanel;
    public GameObject homePagePanel;
    public GameObject clubDataUI;
    public GameObject allTableList;
    public GameObject playerWinList;
    public GameObject feeList;
    public GameObject playerStatsUI;
    public GameObject oneDay;
    public GameObject exportDataScreen;
    public GameObject exportRecordScreen;
    public GameObject clubMemberToggle;
    public GameObject crossUnionToggle;
    public GameObject managerToggle;
    public GameObject chipsToggle;

    public List<GameObject> selectedUIBtn;
    public List<GameObject> selectedTableUIBtn;
    public List<GameObject> highlightFilterUI;
    public List<GameObject> gamesObjList;
    public List<GameObject> winningObjList;
    public List<GameObject> feeObjList;
    public List<GameObject> adminDashboardTab;
    public List<GameObject> agentDashboardTab;
    public List<GameObject> daysImagesList;
    public List<GameObject> highlightExportUI;

    private GameObject scrollItemObj;

    [Header("Transform Reference")]
    public Transform countryPanel;
    public Transform countryContent;
    public Transform gamesContent;
    public Transform gamesPanel;
    public Transform playerWinContent;
    public Transform playerWinPanel;
    public Transform feeContent;
    public Transform feePanel;
    public Transform adminNotificationBtn;
    public Transform agentNotificationBtn;
    public Transform playerNotificationBtn;
    public Transform recordContent;
    public Transform recordPanel;

    [Header("Image Reference")]
    public Image clubImage;

    [Header("Text Reference")]
    public Text country;
    public Text gamesVal;
    public Text playerWinVal;
    public Text feeVal;
    public Text gamesText;
    public Text playerWinText;
    public Text feeText;
    public Text clubDate;
    public Text exportDate;
    public List<Text> statsText;
    public List<Text> agentDashboardText;

    [Header("InputField Reference")]
    public InputField email;
    public InputField clubName;
    public InputField city;
    public InputField emailInputField;

    private int tableItemCount;
    private int playerWinningCount;
    private int feesCount;
    //private string clubAdminUrl;

    private bool notificationStatus;

    //private string editClubUrl;
    //private string clubNotificationUrl;
    //private string clubDataUrl;
    internal string uploadImgURL;
    internal string uploadImgName;
    //internal string statsUrl;
    //private string agentAdminUrl;
    //private string exportDataUrl;

    [Serializable]
    public class ClubNotification
    {
        public string club_id;
        public bool status;
    }

    [Serializable]
    public class EditClubReq
    {
        public string club_name;
        public string club_id;
        public string country;
        public string city;
        public string upload_logo;
    }
    [Serializable]
    public class EditClubRes
    {
        public bool error;
    }

    [Serializable]
    public class ClubDataReq
    {
        public int status;
        public string start_date;
        public string end_date;
        public string club_id;
    }

    [Serializable]
    public class ClubDataRes
    {
        public bool error;
        public int totalGames;
        public int totalplayerWining;
        public int totalFee;
        public RegularClubData[] allGames;
        public PlayerWinningData[] playerWining;
        public FeeData[] fee;
    }

    [Serializable]
    public class RegularClubData
    {
        public string table_type;
        public int small_blind;
        public int big_blind;
        public int min_buy_in;
        public int totalTableGame;
        public string game_type;
        public string table_name;
        public bool video_mode;

    }

    [Serializable]
    public class PlayerWinningData
    {
        public string winner_name;
        public string user_image;
        public string client_id;
        public string table_type;
        public int winner_money;
    }

    [Serializable]
    public class FeeData
    {
        public string username;
        public string user_image;
        public string client_id;
        public string table_type;
        public int fees;
    }

    [Header("Serialize Properties")]

    public List<GameObject> allTableItemObjList;
    public List<GameObject> playerWinItemObjList;
    public List<GameObject> feeItemObjList;

    [SerializeField] ClubNotification clubNotification;
    [SerializeField] EditClubReq editClubReq;
    [SerializeField] EditClubRes editClubRes;
    [SerializeField] ClubDataReq clubDataReq;
    [SerializeField] ClubDataRes clubDataRes;
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //clubNotificationUrl = ServerChanger.instance.domainURL + "api/v1/club/update-notification-status";
        //editClubUrl = ServerChanger.instance.domainURL + "api/v1/club/club-update";
        //clubDataUrl = ServerChanger.instance.domainURL + "api/v1/pokertable/club-data";
        //statsUrl = ServerChanger.instance.domainURL + "api/v1/club/my-statistics";
        //clubAdminUrl = ServerChanger.instance.domainURL + "api/v1/club/club-admin";
        //agentAdminUrl = ServerChanger.instance.domainURL + "api/v1/club/agent-admin";
        //exportHistoryUrl = ServerChanger.instance.domainURL + "api/v1/club/export-history-list";
        //exportDataUrl = ServerChanger.instance.domainURL + "api/v1/club/export-data";

        recordItemList = new List<GameObject>();
        gamesObjList = new List<GameObject>();
        winningObjList = new List<GameObject>();
        feeObjList = new List<GameObject>();

        tableItemCount = 1;
        playerWinningCount = 1;
        feesCount = 1;
        recordCount = 1;
        NoOfDays(1);
    }

    public void OpenDisbandPanel()
    {
        adminPanel.transform.GetChild(0).GetChild(8).gameObject.SetActive(true);
        adminPanel.transform.GetChild(0).GetChild(9).gameObject.SetActive(true);
    }

    public void CloseDisbandPanel()
    {
        adminPanel.transform.GetChild(0).GetChild(8).gameObject.SetActive(false);
        adminPanel.transform.GetChild(0).GetChild(9).gameObject.SetActive(false);
    }

    public void ClubNotificationButton(GameObject obj)
    {
        if (!obj.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            obj.transform.GetChild(1).gameObject.SetActive(false);
            obj.transform.GetChild(0).gameObject.SetActive(true);
            notificationStatus = false;
        }
        else
        {
            obj.transform.GetChild(0).gameObject.SetActive(false);
            obj.transform.GetChild(1).gameObject.SetActive(true);
            notificationStatus = true;
        }
        Notification();
    }

    public void Notification()
    {
        ClubManagement.instance.loadingPanel.SetActive(true);
        clubNotification.club_id = ClubManagement.instance.clubId.text;
        clubNotification.status = notificationStatus;
        string body = JsonUtility.ToJson(clubNotification);
        print("ASHISH BHARDWAJ" + "body : " + body);
        //Communication.instance.PostData(clubNotificationUrl, body, NotificationProcess);
    }

    public void NotificationProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        print(response);
        if (string.IsNullOrEmpty(response))
        {
            print("ASHISH BHARDWAJ" + "error");
        }
        else
        {
            print("Response" + response);
            print("DONE");
        }
    }

    #region Edit Club 

    public void ClickEditClub()
    {

        adminPanel.transform.GetChild(0).gameObject.SetActive(false);
        adminPanel.transform.GetChild(2).gameObject.SetActive(true);
        email.text = Profile._instance.email.text;
        clubName.text = ClubManagement.instance._clubName.Trim();
        country.text = Profile._instance.country.text;
        city.text = Profile._instance.city.text;
        clubImage.sprite = ClubManagement.instance.clubImageLogo.transform.GetComponent<Image>().sprite;
    }

    public void ClickEditClubBackButton()
    {
        adminPanel.transform.GetChild(0).gameObject.SetActive(true);
        adminPanel.transform.GetChild(2).gameObject.SetActive(false);
        Registration.instance.DestroyContryGeneratedList();
    }

    public void ClickOnEditClubSubmitBtn()
    {
        editClubReq.club_id = ClubManagement.instance._clubID;
        editClubReq.club_name = clubName.text;
        editClubReq.country = country.text;
        editClubReq.city = city.text;

        if (!string.IsNullOrEmpty(uploadImgName))
        {
            editClubReq.upload_logo = uploadImgName;
        }

        string body = JsonUtility.ToJson(editClubReq);
        print("EditClub body....." + body);
        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(editClubUrl, body, EditClubProcess);
    }

    void EditClubProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);

        if (!string.IsNullOrEmpty(response))
        {
            print("response......" + response);

            editClubRes = JsonUtility.FromJson<EditClubRes>(response);
            if (!editClubRes.error)
            {
                print("update sucessfull.......");
                ClickEditClubBackButton();
            }
        }
    }

    #endregion

    #region Club Data

    #region Filter in club data

    public void ClickIncreasingFilter()
    {
        highlightFilterUI[0].SetActive(true);
        highlightFilterUI[1].SetActive(false);

        if (allTableList.activeInHierarchy)
        {
            if (gamesObjList.Count > 0)
            {
                for (int i = 0; i < gamesObjList.Count; i++)
                {
                    gamesObjList[i].transform.SetParent(transform);
                    gamesObjList[i].transform.SetParent(gamesContent);
                }
            }
        }
        else if (playerWinList.activeInHierarchy)
        {
            if (winningObjList.Count > 0)
            {
                for (int i = 0; i < winningObjList.Count; i++)
                {
                    winningObjList[i].transform.SetParent(transform);
                    winningObjList[i].transform.SetParent(playerWinContent);
                }
            }
        }
        else
        {
            if (feeObjList.Count > 0)
            {
                for (int i = 0; i < feeObjList.Count; i++)
                {
                    feeObjList[i].transform.SetParent(transform);
                    feeObjList[i].transform.SetParent(feeContent);
                }
            }
        }
    }

    public void ClickDecreasingFilter()
    {
        highlightFilterUI[0].SetActive(false);
        highlightFilterUI[1].SetActive(true);

        if (allTableList.activeInHierarchy)
        {
            if (gamesObjList.Count > 0)
            {
                for (int i = gamesObjList.Count - 1; i >= 0; i--)
                {
                    gamesObjList[i].transform.SetParent(transform);
                    gamesObjList[i].transform.SetParent(gamesContent);
                }
            }
        }
        else if (playerWinList.activeInHierarchy)
        {
            if (winningObjList.Count > 0)
            {
                for (int i = winningObjList.Count - 1; i >= 0; i--)
                {
                    winningObjList[i].transform.SetParent(transform);
                    winningObjList[i].transform.SetParent(playerWinContent);
                }
            }
        }
        else
        {
            if (feeObjList.Count > 0)
            {
                for (int i = feeObjList.Count - 1; i >= 0; i--)
                {
                    feeObjList[i].transform.SetParent(transform);
                    feeObjList[i].transform.SetParent(feeContent);
                }
            }
        }
    }

    #endregion

    #region Export Screen

    [Serializable]
    public class Toggles
    {
        public bool club_member;
        public bool diamond;
        public bool manager;
        public bool club_data;
    }

    [Serializable]
    public class ExportData
    {
        public string club_id;
        public string start_date;
        public string end_date;
        public string email;
        public List<Toggles> arr;
    }
    [SerializeField] ExportData exportData;

    [Serializable]
    public class ExportDataResponse
    {
        public bool error;
    }
    [SerializeField] ExportDataResponse exportDataResponse;

    public void ExportDataRequest()
    {
        exportData.club_id = ClubManagement.instance._clubID;

        startDate = exportDate.text.Substring(0, 10);
        startDate = startDate.Trim();
        print("START1 +" + startDate);
        startDate = DMYToMDY(startDate);
        print("START2 +" + startDate);
        startDate = ConvertDateTime(startDate);
        print("START3 +" + startDate);

        endDate = exportDate.text.Substring(12);
        endDate = endDate.Trim();
        endDate = DMYToMDY(endDate);
        endDate = ConvertDateTime(endDate);

        exportData.start_date = startDate;
        exportData.end_date = endDate;
        exportData.email = emailInputField.text;
        exportData.arr.Clear();
        exportData.arr.Add(new Toggles());
        if(clubMemberToggle.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            exportData.arr[0].club_member = false;
        }
        else
        {
            exportData.arr[0].club_member = true;
        }

        if (chipsToggle.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            exportData.arr[0].diamond = false;
        }
        else
        {
            exportData.arr[0].diamond = true;
        }

        if (managerToggle.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            exportData.arr[0].manager = false;
        }
        else
        {
            exportData.arr[0].manager = true;
        }

        exportData.arr[0].club_data = true;

        string body = JsonUtility.ToJson(exportData);
        print(body);
        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(exportDataUrl, body, ExportDataProcess);
    }

    void ExportDataProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (!string.IsNullOrEmpty(response))
        {
            print("response" + response);

            if (!exportDataResponse.error)
            {
                print("success");
                ResetExportPageOnBackButton();
                Cashier.instance.toastMsg.text = "File Successfully Sent.";
                Cashier.instance.toastMsgPanel.SetActive(true);
            }
            else
            {
                print("fail");
                Cashier.instance.toastMsg.text = "Please Try Again.";
                Cashier.instance.toastMsgPanel.SetActive(true);
            }
        }
    }

    public void ResetExportPageOnBackButton()
    {
        clubDataUI.transform.GetChild(1).gameObject.SetActive(false);
        emailInputField.text = string.Empty;

        clubMemberToggle.transform.GetChild(0).gameObject.SetActive(true);
        clubMemberToggle.transform.GetChild(1).gameObject.SetActive(false);
        crossUnionToggle.transform.GetChild(0).gameObject.SetActive(true);
        crossUnionToggle.transform.GetChild(1).gameObject.SetActive(false);
        managerToggle.transform.GetChild(0).gameObject.SetActive(true);
        managerToggle.transform.GetChild(1).gameObject.SetActive(false);
        chipsToggle.transform.GetChild(0).gameObject.SetActive(true);
        chipsToggle.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void ExportToggleButtons(GameObject obj)
    {
        if(obj.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            obj.transform.GetChild(0).gameObject.SetActive(false);
            obj.transform.GetChild(1).gameObject.SetActive(true);
        }
        else if(obj.transform.GetChild(1).gameObject.activeInHierarchy)
        {
            obj.transform.GetChild(0).gameObject.SetActive(true);
            obj.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void ClickExportDataTab()
    {
        highlightExportUI[0].SetActive(true);
        highlightExportUI[1].SetActive(false);
        exportDataScreen.SetActive(true);
        exportRecordScreen.SetActive(false);
    }

    public void ClickExportBtn()
    {
        if (string.IsNullOrEmpty(emailInputField.text) || !emailInputField.GetComponent<ValidateInput>().isValidInput)
        {
            emailInputField.GetComponent<ValidateInput>().Validate(emailInputField.text);
            emailInputField.Select();
            Debug.Log("in-corrent validate.....");
        }
        else
        {
            ExportDataRequest();
            Debug.Log("corrent validate.....");
        }
    }

    #region Export History Data

    //private string exportHistoryUrl;
    private int recordCount;
    public List<GameObject> recordItemList;

    [Serializable]
    public class ExportRecordRequest
    {
        public string club_id;
    }

    [Serializable]
    public class ExportRecordResponse
    {
        public bool error;
        public ExportRecordData[] data;
    }
    [Serializable]
    public class ExportRecordData
    {
        public bool error;
        public string export_name;
    }

    [SerializeField] ExportRecordRequest exportRecordRequest;
    [SerializeField] ExportRecordResponse exportRecordResponse;


    public void ClickExportRecord()
    {
        highlightExportUI[0].SetActive(false);
        highlightExportUI[1].SetActive(true);
        exportDataScreen.SetActive(false);
        exportRecordScreen.SetActive(true);

        exportRecordRequest.club_id = ClubManagement.instance._clubID;
        string body = JsonUtility.ToJson(exportRecordRequest);
        print(body);
        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(exportHistoryUrl, body, ExportRecordProcess);
    }

    void ExportRecordProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (!string.IsNullOrEmpty(response))
        {
            print("response" + response);
            exportRecordResponse = JsonUtility.FromJson<ExportRecordResponse>(response);

            if (!exportRecordResponse.error)
            {
                if (exportRecordResponse.data.Length != recordCount)
                {
                    for (int i = recordCount; i < exportRecordResponse.data.Length; i++)
                    {
                        recordCount++;
                        GenerateExportRecordItem();
                    }
                }

                for (int i = 0; i < exportRecordResponse.data.Length; i++)
                {
                    recordContent.GetChild(i).GetChild(0).GetComponent<Text>().text = exportRecordResponse.data[i].export_name;
                    recordContent.GetChild(i).gameObject.SetActive(true);
                }
            }
        }

    }

    void GenerateExportRecordItem()
    {
        GameObject scrollItemObj = Instantiate(recordPanel.gameObject);
        scrollItemObj.transform.SetParent(recordContent, false);

        recordItemList.Add(scrollItemObj);
    }

    public void DestroyExportRecordItem()
    {
        if (recordItemList.Count > 0)
        {
            for (int i = 0; i < recordItemList.Count; i++)
            {
                Destroy(recordItemList[i]);
            }
            recordItemList.Clear();
            recordCount = 1;
        }
    }

    #endregion

    #endregion

    public void ClickOnClubData()
    {
        menuFadePanel.SetActive(false);
        ownerMenuPanel.SetActive(false);
        homePagePanel.SetActive(false);
        clubDataUI.SetActive(true);

        CalenderDaysImage(oneDay);
        ClubDataRequest(1);
        ClickGames();
    }

    public void ClickBackBtnFromClubData()
    {
        clubDataUI.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<Text>().text = DateTime.Now.ToString("dd/MM/yyyy") + " " + "-" + " " + DateTime.Now.ToString("dd/MM/yyyy");
        clubDataUI.SetActive(false);
        homePagePanel.SetActive(true);

        for (int i = 0; i < highlightFilterUI.Count; i++)
        {
            highlightFilterUI[i].SetActive(false);
        }
    }

    public void ClickGames()
    {
        for (int i = 0; i < selectedUIBtn.Count; i++)
        {
            selectedUIBtn[i].SetActive(false);
        }

        for (int i = 0; i < selectedTableUIBtn.Count; i++)
        {
            selectedTableUIBtn[i].SetActive(false);
        }

        selectedUIBtn[0].SetActive(true);
        selectedTableUIBtn[0].SetActive(true);
        allTableList.SetActive(true);
        playerWinList.SetActive(false);
        feeList.SetActive(false);
    }

    public void ClickPlayerWin()
    {
        for (int i = 0; i < selectedUIBtn.Count; i++)
        {
            selectedUIBtn[i].SetActive(false);
        }

        for (int i = 0; i < selectedTableUIBtn.Count; i++)
        {
            selectedTableUIBtn[i].SetActive(false);
        }

        selectedUIBtn[1].SetActive(true);
        selectedTableUIBtn[0].SetActive(true);

        allTableList.SetActive(false);
        playerWinList.SetActive(true);
        feeList.SetActive(false);
    }

    public void ClickFee()
    {
        for (int i = 0; i < selectedUIBtn.Count; i++)
        {
            selectedUIBtn[i].SetActive(false);
        }

        for (int i = 0; i < selectedTableUIBtn.Count; i++)
        {
            selectedTableUIBtn[i].SetActive(false);
        }

        selectedUIBtn[2].SetActive(true);
        selectedTableUIBtn[0].SetActive(true);

        allTableList.SetActive(false);
        playerWinList.SetActive(false);
        feeList.SetActive(true);
    }

    public void ClickAll()
    {
        for (int i = 0; i < selectedTableUIBtn.Count; i++)
        {
            selectedTableUIBtn[i].SetActive(false);
        }
        selectedTableUIBtn[0].SetActive(true);

        if (allTableList.activeInHierarchy)
        {
            for (int i = 0; i < gamesContent.childCount; i++)
            {
                if (gamesContent.GetChild(i).GetChild(5).GetComponent<Text>().text == "regular-table" ||
                    gamesContent.GetChild(i).GetChild(5).GetComponent<Text>().text == "tournament-table"
                    )
                {
                    gamesContent.GetChild(i).gameObject.SetActive(true);
                }
            }

        }
        else if (playerWinList.activeInHierarchy)
        {
            for (int i = 0; i < playerWinContent.childCount; i++)
            {
                if (playerWinContent.GetChild(i).GetChild(4).GetComponent<Text>().text == "regular-table" ||
                    playerWinContent.GetChild(i).GetChild(4).GetComponent<Text>().text == "tournament-table"
                    )
                {
                    playerWinContent.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
        else if (feeList.activeInHierarchy)
        {
            for (int i = 0; i < feeContent.childCount; i++)
            {
                if (feeContent.GetChild(i).GetChild(4).GetComponent<Text>().text == "regular-table" ||
                    feeContent.GetChild(i).GetChild(4).GetComponent<Text>().text == "tournament-table"
                    )
                {
                    feeContent.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
    }

    public void ClickRegular()
    {
        ClickRegularTable(true);

        for (int i = 0; i < selectedTableUIBtn.Count; i++)
        {
            selectedTableUIBtn[i].SetActive(false);
        }
        selectedTableUIBtn[1].SetActive(true);
    }

    public void ClickMTT()
    {
        ClickRegularTable(false);

        for (int i = 0; i < selectedTableUIBtn.Count; i++)
        {
            selectedTableUIBtn[i].SetActive(false);
        }
        selectedTableUIBtn[2].SetActive(true);
    }

    public void ClickRegularTable(bool isRegular)
    {
        if (allTableList.activeInHierarchy)
        {
            for (int i = 0; i < gamesContent.childCount; i++)
            {
                if (gamesContent.GetChild(i).GetChild(5).GetComponent<Text>().text == "regular-table")
                {
                    gamesContent.GetChild(i).gameObject.SetActive(isRegular);
                }
                else if (gamesContent.GetChild(i).GetChild(5).GetComponent<Text>().text == "tournament-table")
                {
                    gamesContent.GetChild(i).gameObject.SetActive(!isRegular);
                }
            }

        }
        else if (playerWinList.activeInHierarchy)
        {
            for (int i = 0; i < playerWinContent.childCount; i++)
            {
                if (playerWinContent.GetChild(i).GetChild(4).GetComponent<Text>().text == "regular-table")
                {
                    playerWinContent.GetChild(i).gameObject.SetActive(isRegular);
                }
                else if (playerWinContent.GetChild(i).GetChild(4).GetComponent<Text>().text == "tournament-table")
                {
                    playerWinContent.GetChild(i).gameObject.SetActive(!isRegular);
                }
            }
        }
        else if (feeList.activeInHierarchy)
        {
            for (int i = 0; i < feeContent.childCount; i++)
            {
                if (feeContent.GetChild(i).GetChild(4).GetComponent<Text>().text == "regular-table")
                {
                    feeContent.GetChild(i).gameObject.SetActive(isRegular);
                }
                else if (feeContent.GetChild(i).GetChild(4).GetComponent<Text>().text == "tournament-table")
                {
                    feeContent.GetChild(i).gameObject.SetActive(!isRegular);
                }
            }
        }
    }

    public void UpdateDateOnExport()
    {
        exportDate.text = clubDate.text;
    }

    public void CalenderDaysImage(GameObject obj)
    {
        for (int i = 0; i < daysImagesList.Count; i++)
        {
            daysImagesList[i].transform.GetChild(0).gameObject.SetActive(false);
        }
        obj.transform.GetChild(0).gameObject.SetActive(true);
    }

    private string startDate;
    private string endDate;
    DateTime date;

    public string DMYToMDY(string input)
    {
        return Regex.Replace(input,
        @"\b(?<day>\d{1,2})/(?<month>\d{1,2})/(?<year>\d{2,4})\b",
        "${month}/${day}/${year}");
    }
    public string ConvertDateTime(string _date)
    {
        date = DateTime.Parse(_date);
        return date.ToString("yyyy-MM-dd");
    }

    public void ClubDataRequest(int status)
    {
        print(status);
        clubDataReq.club_id = ClubManagement.instance._clubID;
        clubDataReq.status = status;
        if (status == 4)
        {
            startDate = DMYToMDY(CalendarController3._calendarInstance._target);
            startDate = ConvertDateTime(startDate);
            endDate = DMYToMDY(CalendarController3._calendarInstance._target1);
            endDate = ConvertDateTime(endDate);

            clubDataReq.start_date = startDate;
            clubDataReq.end_date = endDate;
        }
        else
        {
            clubDataReq.start_date = "";
            clubDataReq.end_date = "";
        }

        string body = JsonUtility.ToJson(clubDataReq);
        print(body);
        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(clubDataUrl, body, ClickSetDataProcess);
    }

   
    public void ClickSetDataProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);

        if (!string.IsNullOrEmpty(response))
        {
            print("response......" + response);

            clubDataRes = JsonUtility.FromJson<ClubDataRes>(response);
            if (!clubDataRes.error)
            {
                print("sucessfull.......");

                gamesVal.text = clubDataRes.totalGames.ToString();
                playerWinVal.text = clubDataRes.totalplayerWining.ToString();
                feeVal.text = clubDataRes.totalFee.ToString();

                //..............Tabel Data...........//
                if (clubDataRes.allGames.Length > 0)
                {
                    if (clubDataRes.allGames.Length != tableItemCount)
                    {
                        for (int i = tableItemCount; i < clubDataRes.allGames.Length; i++)
                        {
                            tableItemCount++;
                            GenerateTableItem();
                        }
                    }
                    gamesObjList.Clear();
                    for (int i = 0; i < clubDataRes.allGames.Length; i++)
                    {
                        gamesContent.GetChild(i).GetChild(0).GetComponent<Text>().text = clubDataRes.allGames[i].table_name;
                        gamesContent.GetChild(i).GetChild(1).GetComponent<Text>().text = clubDataRes.allGames[i].small_blind.ToString();
                        gamesContent.GetChild(i).GetChild(2).GetComponent<Text>().text = clubDataRes.allGames[i].big_blind.ToString();
                        gamesContent.GetChild(i).GetChild(3).GetComponent<Text>().text = clubDataRes.allGames[i].min_buy_in.ToString();

                        gamesContent.GetChild(i).GetChild(4).GetChild(0).GetComponent<Text>().text = clubDataRes.allGames[i].game_type;
                        gamesContent.GetChild(i).GetChild(5).GetComponent<Text>().text = clubDataRes.allGames[i].table_type; //disable obj
                        gamesContent.GetChild(i).GetChild(6).GetChild(0).GetComponent<Text>().text = clubDataRes.allGames[i].totalTableGame.ToString();

                        bool isVideo = clubDataRes.allGames[i].video_mode;

                        if (isVideo)
                        {
                            gamesContent.GetChild(i).GetChild(7).gameObject.SetActive(true);
                            gamesContent.GetChild(i).GetChild(8).gameObject.SetActive(false);
                        }
                        else
                        {
                            gamesContent.GetChild(i).GetChild(7).gameObject.SetActive(false);
                            gamesContent.GetChild(i).GetChild(8).gameObject.SetActive(true);
                        }

                        gamesContent.GetChild(i).gameObject.SetActive(true);
                        gamesObjList.Add(gamesContent.GetChild(i).gameObject);
                    }

                }
                else
                {
                    DestroyGeneratedTableItem();
                    gamesContent.GetChild(0).gameObject.SetActive(false);
                    gamesContent.GetChild(0).GetChild(5).GetComponent<Text>().text = "";
                }

                //..............Player winning Data...........//

                if (clubDataRes.playerWining.Length > 0)
                {
                    if (clubDataRes.playerWining.Length != tableItemCount)
                    {
                        for (int i = playerWinningCount; i < clubDataRes.playerWining.Length; i++)
                        {
                            playerWinningCount++;
                            GeneratePlayerWinItem();
                        }
                    }

                    userImage.Clear();
                    winningObjList.Clear();
                    for (int i = 0; i < clubDataRes.playerWining.Length; i++)
                    {
                        playerWinContent.GetChild(i).GetChild(1).GetComponent<Text>().text = clubDataRes.playerWining[i].winner_name;
                        playerWinContent.GetChild(i).GetChild(2).GetComponent<Text>().text = clubDataRes.playerWining[i].client_id;
                        playerWinContent.GetChild(i).GetChild(3).GetChild(0).GetComponent<Text>().text = clubDataRes.playerWining[i].winner_money.ToString();
                        playerWinContent.GetChild(i).GetChild(4).GetComponent<Text>().text = clubDataRes.playerWining[i].table_type; //disable obj

                        playerWinContent.GetChild(i).gameObject.SetActive(true);
                        userImage.Add(clubDataRes.playerWining[i].user_image);
                        winningObjList.Add(playerWinContent.GetChild(i).gameObject);
                    }

                    UpdatePlayerImage();

                }

                else
                {
                    DestroyGeneratedPlayerWinItem();
                    playerWinContent.GetChild(0).gameObject.SetActive(false);
                    playerWinContent.GetChild(0).GetChild(4).GetComponent<Text>().text = "";
                }

                //..............Fees Data...........//

                if (clubDataRes.fee.Length > 0)
                {
                    if (clubDataRes.fee.Length != tableItemCount)
                    {
                        for (int i = feesCount; i < clubDataRes.fee.Length; i++)
                        {
                            feesCount++;
                            GenerateFeeItem();
                        }
                    }

                    userImageInFee.Clear();
                    feeObjList.Clear();
                    for (int i = 0; i < clubDataRes.fee.Length; i++)
                    {
                        feeContent.GetChild(i).GetChild(1).GetComponent<Text>().text = clubDataRes.fee[i].username;
                        feeContent.GetChild(i).GetChild(2).GetComponent<Text>().text = clubDataRes.fee[i].client_id;
                        feeContent.GetChild(i).GetChild(3).GetChild(0).GetComponent<Text>().text = clubDataRes.fee[i].fees.ToString();
                        feeContent.GetChild(i).GetChild(4).GetComponent<Text>().text = clubDataRes.fee[i].table_type; //disable obj

                        feeContent.GetChild(i).gameObject.SetActive(true);
                        userImageInFee.Add(clubDataRes.fee[i].user_image);
                        feeObjList.Add(feeContent.GetChild(i).gameObject);
                    }

                    UpdatePlayerImageInFee();

                }

                else
                {
                    DestroyGeneratedFeeItem();
                    feeContent.GetChild(0).gameObject.SetActive(false);
                    feeContent.GetChild(0).GetChild(4).GetComponent<Text>().text = "";
                }
            }
        }
    }

    #region update Player Image In Player Win

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
                playerWinContent.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
            }

            for (int i = 0; i < userImage.Count; i++)
            {
                if (!string.IsNullOrEmpty(userImage[i]))
                {
                    print("i = " + i);
                    playerWinContent.GetChild(i).GetChild(0).GetComponent<Image>().sprite = playerImageInSequence[count].imgPic;
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

    #region update Player Image In Fee

    public List<string> userImageInFee;

    [SerializeField]
    private List<PlayerImageInSequenceInFee> playerImageInSequenceInFee;

    private int k1 = 0;
    private int totalImageCountInFee;
    private int countInFee = 0;

    private int previousCountForMemberListInFee;
    void UpdatePlayerImageInFee()
    {
        if (userImageInFee.Count == previousCountForMemberListInFee)
        {
            return;
        }

        k1 = 0;
        previousCountForMemberListInFee = 0;
        totalImageCountInFee = 0;
        playerImageInSequenceInFee.Clear();

        playerImageInSequenceInFee = new List<PlayerImageInSequenceInFee>();

        for (int i = 0; i < userImageInFee.Count; i++)
        {
            if (!string.IsNullOrEmpty(userImageInFee[i]))
            {
                playerImageInSequenceInFee.Add(new PlayerImageInSequenceInFee());
                ClubManagement.instance.loadingPanel.SetActive(true);
                playerImageInSequenceInFee[k1].imgUrl = userImageInFee[i];
                playerImageInSequenceInFee[k1].ImageProcess(userImageInFee[i]);

                k1 = k1 + 1;

            }
            previousCountForMemberListInFee++;
        }
    }

    public void ApplyImageInFee()
    {
        print(k1);
        print(totalImageCountInFee);
        if (k1 == totalImageCountInFee)
        {
            countInFee = 0;
            ClubManagement.instance.loadingPanel.SetActive(false);

            for (int i = 0; i < userImageInFee.Count; i++)
            {
                feeContent.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
            }

            for (int i = 0; i < userImageInFee.Count; i++)
            {
                if (!string.IsNullOrEmpty(userImageInFee[i]))
                {
                    print("i = " + i);
                    feeContent.GetChild(i).GetChild(0).GetComponent<Image>().sprite = playerImageInSequenceInFee[countInFee].imgPic;
                    countInFee++;
                }
            }

        }
    }

    [Serializable]
    public class PlayerImageInSequenceInFee
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
                instance.totalImageCountInFee++;
                instance.ApplyImageInFee();

            }
        }
    }

    public void ResetAllValuesForImageInFee()
    {
        totalImageCountInFee = 0;
        k1 = 0;
        countInFee = 0;
        previousCountForMemberListInFee = 0;
        userImageInFee.Clear();
        playerImageInSequenceInFee.Clear();
    }

    #endregion

    public void GenerateTableItem()
    {
        scrollItemObj = Instantiate(gamesPanel.gameObject);
        scrollItemObj.transform.SetParent(gamesContent, false);
        allTableItemObjList.Add(scrollItemObj);
    }

    public void GeneratePlayerWinItem()
    {
        scrollItemObj = Instantiate(playerWinPanel.gameObject);
        scrollItemObj.transform.SetParent(playerWinContent, false);
        playerWinItemObjList.Add(scrollItemObj);
    }

    public void GenerateFeeItem()
    {
        scrollItemObj = Instantiate(feePanel.gameObject);
        scrollItemObj.transform.SetParent(feeContent, false);
        feeItemObjList.Add(scrollItemObj);
    }

    public void DestroyGeneratedTableItem()
    {
        if (allTableItemObjList.Count > 0)
        {
            for (int i = 0; i < allTableItemObjList.Count; i++)
            {
                Destroy(allTableItemObjList[i]);
            }
            allTableItemObjList.Clear();
            tableItemCount = 1;
        }
    }

    public void DestroyGeneratedPlayerWinItem()
    {
        if (playerWinItemObjList.Count > 0)
        {
            for (int i = 0; i < playerWinItemObjList.Count; i++)
            {
                Destroy(playerWinItemObjList[i]);
            }
            playerWinItemObjList.Clear();
            playerWinningCount = 1;
            ResetAllValuesForImage();
        }
    }

    public void DestroyGeneratedFeeItem()
    {
        if (feeItemObjList.Count > 0)
        {
            for (int i = 0; i < feeItemObjList.Count; i++)
            {
                Destroy(feeItemObjList[i]);
            }
            feeItemObjList.Clear();
            feesCount = 1;
            ResetAllValuesForImageInFee();
        }
    }

    #endregion

    #region Player stats

    [Serializable]
    public class PlayerStats
    {
        public bool error;
        public int total_games;
        public float vpip;
        public float pfr;
        public float bet;
        public float c_bet;
        public string message;

    }

    [Serializable]
    public class ClubId
    {
        public string club_id;
        public string username;
    }

    [SerializeField] PlayerStats playerStats;
    [SerializeField] ClubId clubId;

    public void ClickPlayerStats()
    {
        clubId.club_id = ClubManagement.instance._clubID;
        clubId.username = AccessGallery.instance.profileName[0].text;
        string body = JsonUtility.ToJson(clubId);
        print(body);

        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(statsUrl, body, PlayerStatsProcess);
    }

    public void PlayerStatsProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);

        if (!string.IsNullOrEmpty(response))
        {
            print(response);
            playerStats = JsonUtility.FromJson<PlayerStats>(response);

            if (!playerStats.error)
            {
                statsText[0].text = playerStats.total_games.ToString();
                statsText[1].text = playerStats.vpip.ToString() + "%";
                statsText[2].text = playerStats.pfr.ToString() + "%";
                statsText[3].text = playerStats.bet.ToString() + "%";
                statsText[4].text = playerStats.c_bet.ToString() + "%";
                playerStatsUI.SetActive(true);
            }
        }
    }

    #endregion

    #region Admin dashboard

    [Serializable]
    public class AdminDashboardRequest
    {
        public string club_id;
        public int status;
    }

    [Serializable]
    public class AdminDashboardResponse
    {
        public bool error;
        public int wining;
        public int games;
        public int fees;
        public bool notification;
    }

    [SerializeField] AdminDashboardRequest adminDashboardRequest;
    [SerializeField] AdminDashboardResponse adminDashboardResponse;

    public void ClickAdminDashboardTab(Transform panel)
    {
        for (int i = 0; i < adminDashboardTab.Count; i++)
        {
            adminDashboardTab[i].SetActive(false);
        }
        panel.GetChild(0).gameObject.SetActive(true);

        if (panel.GetChild(1).GetComponent<Text>().text == "Today")
        {
            ClickAdminDashboardRequest(1);
        }
        else if (panel.GetChild(1).GetComponent<Text>().text == "This Week")
        {
            ClickAdminDashboardRequest(2);
        }
        else if (panel.GetChild(1).GetComponent<Text>().text == "Last Week")
        {
            ClickAdminDashboardRequest(3);
        }
        else if (panel.GetChild(1).GetComponent<Text>().text == "History")
        {
            ClickAdminDashboardRequest(4);
        }
    }

    public void ClickAdminDashboardRequest(int status)
    {
        adminDashboardRequest.club_id = ClubManagement.instance._clubID;
        adminDashboardRequest.status = status;

        string body = JsonUtility.ToJson(adminDashboardRequest);
        print(body);
        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(clubAdminUrl, body, ClickAdminDashboardProcess);
    }

    void ClickAdminDashboardProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);

        if (!string.IsNullOrEmpty(response))
        {
            print(response);
            adminDashboardResponse = JsonUtility.FromJson<AdminDashboardResponse>(response);

            if (!adminDashboardResponse.error)
            {
                gamesText.text = adminDashboardResponse.games.ToString();
                playerWinText.text = adminDashboardResponse.wining.ToString();
                feeText.text = adminDashboardResponse.fees.ToString();

                if (adminDashboardResponse.notification)
                {
                    adminNotificationBtn.GetChild(0).gameObject.SetActive(false);
                    adminNotificationBtn.GetChild(1).gameObject.SetActive(true);

                    playerNotificationBtn.GetChild(0).gameObject.SetActive(false);
                    playerNotificationBtn.GetChild(1).gameObject.SetActive(true);
                }
                else
                {
                    adminNotificationBtn.GetChild(0).gameObject.SetActive(true);
                    adminNotificationBtn.GetChild(1).gameObject.SetActive(false);

                    playerNotificationBtn.GetChild(0).gameObject.SetActive(true);
                    playerNotificationBtn.GetChild(1).gameObject.SetActive(false);
                }
            }

        }

    }

    #endregion

    #region Admin dashboard For Agent

    public void ClickAgentDashboardTab(Transform panel)
    {
        for (int i = 0; i < agentDashboardTab.Count; i++)
        {
            agentDashboardTab[i].SetActive(false);
        }
        panel.GetChild(0).gameObject.SetActive(true);

        if (panel.GetChild(1).GetComponent<Text>().text == "Today")
        {
            ClickAgentDashboardRequest(1);
        }
        else if (panel.GetChild(1).GetComponent<Text>().text == "This Week")
        {
            ClickAgentDashboardRequest(2);
        }
        else if (panel.GetChild(1).GetComponent<Text>().text == "Last Week")
        {
            ClickAgentDashboardRequest(3);
        }
        else if (panel.GetChild(1).GetComponent<Text>().text == "History")
        {
            ClickAgentDashboardRequest(4);
        }
    }

    [Serializable]
    public class AgentDashboardRequest
    {
        public string club_id;
        public string agent_id;
        public int status;
    }

    [Serializable]
    public class AgentDashboardResponse
    {
        public bool error;
        public int downlinePlayers;
        public int agentCredit;
        public int downlineChips;
        public int wining;
        public int fees;
        public bool notification;
    }

    [SerializeField] AgentDashboardRequest agentDashboardRequest;
    [SerializeField] AgentDashboardResponse agentDashboardResponse;

    public void ClickAgentDashboardRequest(int status)
    {
        agentDashboardRequest.club_id = ClubManagement.instance._clubID;
        agentDashboardRequest.status = status;
        agentDashboardRequest.agent_id = ClubManagement.instance.currentSelectedAgentId;

        string body = JsonUtility.ToJson(agentDashboardRequest);
        print(body);
        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(agentAdminUrl, body, ClickAgentDashboardProcess);
    }

    void ClickAgentDashboardProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);

        if (!string.IsNullOrEmpty(response))
        {
            print(response);
            agentDashboardResponse = JsonUtility.FromJson<AgentDashboardResponse>(response);

            if (!agentDashboardResponse.error)
            {
                agentDashboardText[0].text = agentDashboardResponse.fees.ToString();
                agentDashboardText[1].text = agentDashboardResponse.agentCredit.ToString();
                agentDashboardText[2].text = agentDashboardResponse.wining.ToString();
                agentDashboardText[3].text = agentDashboardResponse.downlinePlayers.ToString();
                agentDashboardText[4].text = agentDashboardResponse.downlineChips.ToString();

                if (agentDashboardResponse.notification)
                {
                    agentNotificationBtn.GetChild(0).gameObject.SetActive(false);
                    agentNotificationBtn.GetChild(1).gameObject.SetActive(true);
                }
                else
                {
                    agentNotificationBtn.GetChild(0).gameObject.SetActive(true);
                    agentNotificationBtn.GetChild(1).gameObject.SetActive(false);
                }
            }

        }

    }

    #endregion

    #region ClubData StartDate

    public string todaysDate;
    public string prevDate;
    public string prevMonthDate;
    public int totalDays;

    public void NoOfDays(int days)
    {
        if (days == 1)
        {
            totalDays = 1;
        }
        else if (days == 7)
        {
            totalDays = 2;
        }
        else if (days == 14)
        {
            totalDays = 3;
        }
        print(totalDays);

        todaysDate = DateTime.Now.ToString("dd");
        prevDate = (int.Parse(todaysDate) - days).ToString();
        if ((int.Parse(todaysDate) - days) > 0)
        {
            clubDataUI.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<Text>().text = prevDate + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("yyyy") + " " + "-" + " " + todaysDate + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("yyyy");
        }
        else
        {
            if (DateTime.Now.ToString("MM") == "03" || DateTime.Now.ToString("MM") == "05" || DateTime.Now.ToString("MM") == "07" || DateTime.Now.ToString("MM") == "08" || DateTime.Now.ToString("MM") == "10" || DateTime.Now.ToString("MM") == "12")
            {
                print("31st....");
                prevMonthDate = (31 - days + int.Parse(todaysDate)).ToString();
                clubDataUI.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<Text>().text = prevMonthDate + "/" + (int.Parse(DateTime.Now.ToString("MM")) - 1).ToString() + "/" + DateTime.Now.ToString("yyyy") + " " + "-" + " " + todaysDate + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("yyyy");
            }
            else if (DateTime.Now.ToString("MM") == "04" || DateTime.Now.ToString("MM") == "06" || DateTime.Now.ToString("MM") == "09" || DateTime.Now.ToString("MM") == "11")
            {
                print("30th....");
                prevMonthDate = (30 - days + int.Parse(todaysDate)).ToString();
                clubDataUI.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<Text>().text = prevMonthDate + "/" + (int.Parse(DateTime.Now.ToString("MM")) - 1).ToString() + "/" + DateTime.Now.ToString("yyyy") + " " + "-" + " " + todaysDate + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("yyyy");
            }
            else if (DateTime.Now.ToString("MM") == "02")
            {
                print("28th....");
                prevMonthDate = (28 - days + int.Parse(todaysDate)).ToString();
                clubDataUI.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<Text>().text = prevMonthDate + "/" + (int.Parse(DateTime.Now.ToString("MM")) - 1).ToString() + "/" + DateTime.Now.ToString("yyyy") + " " + "-" + " " + todaysDate + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("yyyy");
            }
            else if (DateTime.Now.ToString("MM") == "01")
            {
                print("31th....1st");
                prevMonthDate = (31 - days + int.Parse(todaysDate)).ToString();
                clubDataUI.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<Text>().text = prevMonthDate + "/" + "12" + "/" + (int.Parse(DateTime.Now.ToString("yyyy")) - 1).ToString() + " " + "-" + " " + todaysDate + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("yyyy");
            }
        }
    }
    #endregion
}