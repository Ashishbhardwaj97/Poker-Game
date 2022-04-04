using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//using LightBuzz.Archiver;
using System.Text;

public class ReportAbuseScript : MonoBehaviour
{


    public enum TableStatus { OFF, Regular, Tournament }
    public TableStatus tableStatus;

    public static ReportAbuseScript instance;

    // Urls...
    public string reportAbuseURL;
    public string reportViolationURL;
    public string chkBlockedURL;



    public bool dataReady;


    //Regular Video Table Related UI Ref..
    public List<GameObject> reportPanelRegularList;
    public GameObject VideoReportPanel;
    public Button nextBtnReg;
    public GameObject commentPanelReg;
    public InputField inputFieldReg;

    public string currentSelectedViolation;
    public string inputDataFromUserReg;

    //Tournament Video Table Related UI Ref..
    public List<GameObject> reportPanelTourList;
    public GameObject VideoReportPanelTournament;
    public Button nextBtnTour;
    public GameObject commentPanelTour;
    public InputField inputFieldTour;

    public string inputDataFromUserTour;
    public string currentselectedviolationTour;

    internal PokerPlayerController playerInfoRef;

    public Texture2D sampleTex;



    #region Report Class Data from Server...
    [System.Serializable]
    public class ReportDataFromServer
    {
        public bool error;
        public List<ArrData> data;
        public string message;
        public int statusCode;

    }
    [System.Serializable]
    public class ArrData
    {
        public string _id;
        public string violation_type;

    }
    [SerializeField]
    public ReportDataFromServer reportData;

    #endregion


    #region formdata for getting violation type
    [System.Serializable]
    public class JsonToServer
    {
        public string table_type;

    }
    [SerializeField]
    public JsonToServer detail;

    #endregion








    #region formdata to post for report violation...
    [System.Serializable]
    public class ReportViolationClass
    {
        public string violation_type;
        public string reporter_id;
        public string violator_id;
        public string reporter_comment;
        public string table_type;
        public string table_id;
        public string game_id;
        public string game_type;
        public string seat_id;
        public string file;

    }
    [SerializeField]
    public ReportViolationClass ReportViolationClassRef;






    #endregion


    #region CheckforBlockedUserData


    public class CheckforBlockedUser
    {

        public bool error;
        public string message;
        public int statusCode;
        public List<Data> data;


    }


    public class Data
    {
        public List<string> table_type;
        public string _id;
        public int violation_count;
        public string violation_rule_id;
        public string violator_id;
        public string block_date_from;
        public string block_date_to;
        public string createdAt;
        public string updatedAt;
        public float _v;

    }

    CheckforBlockedUser chkblockuserRef;


    #endregion

    string source;
    string destination;


    //


    [System.Serializable]
    public class ReporterDataSet
    {

        public List<DataSet> ReporteeDatalist;
    }

    [System.Serializable]
    public class DataSet
    {

        public string Reportee;
        public string violator;



        public DataSet(string par1, string par2)
        {
            this.Reportee = par1;
            this.violator = par2;

        }

    }

   


    public ReporterDataSet ReportDataRef;
    string key = "DateNow";
    string saveFile;


    //




    private void Awake()
    {
        source = Application.dataPath + "/" + "screenshot.png";
        destination = Application.dataPath + "/" + "screenshotzip.zip";
        instance = this;
  

      

       

    }


    public string jsonToServer;
    // Start is called before the first frame update
    void Start()
    {
        
        
        reportAbuseURL = ServerChanger.instance.domainURL + "api/v1/user/violation-types";
        reportViolationURL = ServerChanger.instance.domainURL + "api/v1/user/report-violation";
        tableStatus = TableStatus.OFF;



        // chkBlockedURL = ServerChanger.instance.domainURL + "api/v1/user/check-blocked-user";



        saveFile = Application.persistentDataPath + "/ReporteeData.json";
        int day = int.Parse(System.DateTime.Now.ToString("dd"));
        if (PlayerPrefs.HasKey(key)) 
        {
            if (PlayerPrefs.GetInt(key) == day)
            {
                Debug.Log(">>>>same date exist.");
                LoadDataFromFile();
            }
            else
            {
                Debug.Log(">>>>New Date Recorded..");
                ReportDataRef.ReporteeDatalist.Clear();
                PlayerPrefs.SetInt(key, day);
            }
        
        }
        else
        {
            PlayerPrefs.SetInt(key, day);

            Debug.Log(">>>>new key has been recorded.");
            if (File.Exists(saveFile))
            {
                string filecontents = File.ReadAllText(saveFile);
                ReportDataRef = JsonUtility.FromJson<ReporterDataSet>(filecontents);
                Debug.Log(">>>>file has been deleted..." + filecontents);

               // File.Delete(saveFile);
            }


         
        }
      
       

    }


 

    public void LoadDataFromFile()
    {


        if (File.Exists(saveFile))
        {
         
            string filecontents = File.ReadAllText(saveFile);
            ReportDataRef = JsonUtility.FromJson<ReporterDataSet>(filecontents);

            Debug.Log(">>>>Reportee Data Exist.." + filecontents);
        }
        else
        {
            Debug.Log(">>>>Reportee Data do not Exist..");
            ReportDataRef.ReporteeDatalist.Clear();
        }



    }

    public void SaveDataToFile()
    {
        string ReporteeData = JsonUtility.ToJson(ReportDataRef);

        File.WriteAllText(saveFile, ReporteeData);
        Debug.Log(">>>>Reportee Data has been saved.."+ReporteeData);
    }





 public void VideoReportResponse(string response) 
    {
        print(response);

        ClubManagement.instance.loadingPanel.SetActive(false);

        if (string.IsNullOrEmpty(response))
        {
            print("Some error in video report data! ");
        }
        else
        {
            print("response" + response);

            reportData = JsonUtility.FromJson<ReportDataFromServer>(response);


            if (!reportData.error)
            {
                print("data correct...");
                if (reportPanelTourList.Count != reportData.data.Count)
                {

                    Debug.Log("Report Data from server is not equal to current options...");
                  //  return;

                }
                for (int i = 0; i < reportPanelRegularList.Count || i < reportPanelTourList.Count; i++)
                {
                    if (!GameManagerScript.instance.isTournament)
                    {
                        if (i < reportPanelRegularList.Count && i < reportData.data.Count)
                        {


                            reportPanelRegularList[i].GetComponentInChildren<Text>().text = reportData.data[i].violation_type;
                            reportPanelRegularList[i].gameObject.name = reportData.data[i]._id;
                            reportPanelRegularList[i].SetActive(true);



                        }
                        else
                        {
                            if(i<reportPanelRegularList.Count)
                            reportPanelRegularList[i].SetActive(false);


                        }
                    }
                    else if (GameManagerScript.instance.isTournament)
                    {
                        if (i < reportPanelTourList.Count && i < reportData.data.Count)
                        {
                            reportPanelTourList[i].GetComponentInChildren<Text>().text = reportData.data[i].violation_type;
                            reportPanelTourList[i].gameObject.name = reportData.data[i]._id;
                            reportPanelTourList[i].SetActive(true);
                        }
                        else
                        {
                            if (i < reportPanelTourList.Count)
                                reportPanelTourList[i].SetActive(false);
                        }
                    }
                }
                //   ClubManagement.instance.loadingPanel.SetActive(false);
                if (GameManagerScript.instance.isTournament)
                {
                    Debug.Log("1");

                    VideoReportPanelTournament.transform.parent.gameObject.SetActive(true);
                    nextBtnTour.gameObject.GetComponent<Image>().color = Color.grey;
                    tableStatus = TableStatus.Tournament;
                }
                else
                {

                    Debug.Log("2");
                    VideoReportPanel.transform.parent.gameObject.SetActive(true);
                    nextBtnReg.gameObject.GetComponent<Image>().color = Color.grey;
                    tableStatus = TableStatus.Regular;
                }
                inputFieldReg.text = "";
                inputFieldTour.text = "";
            }
            else
            {
                print("data incorrect...");

            }
        }
    }


    public void OnButtonSelected(Text textData)
    {
       
        Button btn = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
       
        
        if (tableStatus == TableStatus.Regular)
        {
            currentSelectedViolation = btn.gameObject.name;
            nextBtnReg.enabled = true;
            nextBtnReg.interactable = true;
            nextBtnReg.gameObject.GetComponent<Image>().color = Color.white;
        }
        else if (tableStatus == TableStatus.Tournament)
        {
            currentselectedviolationTour = btn.gameObject.name;
            nextBtnTour.enabled = true;
            nextBtnTour.interactable = true;
            nextBtnTour.gameObject.GetComponent<Image>().color = Color.white;
        }
        else
            Debug.Log("No Table is Activated : Video Report Script");

    }



    public void OnSelect()
    {
       
        nextBtnReg.enabled = false;
        nextBtnReg.interactable = false;
        nextBtnReg.gameObject.GetComponent<Image>().color = Color.grey;
        nextBtnTour.enabled = false;
        nextBtnTour.interactable = false;
        nextBtnTour.gameObject.GetComponent<Image>().color = Color.grey;

    }


    public void FillReportData(string localPlayerID, string localPlayerSeatID, string violatorPlayerID, string violatorSeatID, bool isTournament, string tournamentID, string tableID)
    {
        ReportViolationClassRef.reporter_id = localPlayerID;
        ReportViolationClassRef.violator_id = violatorPlayerID;
        ReportViolationClassRef.table_id = tableID;
        ReportViolationClassRef.game_id = "";
        ReportViolationClassRef.game_type = "NLH";
        ReportViolationClassRef.seat_id = violatorSeatID;



    }


    public void OnClickNextBtnReportPanel() 
    {
        Button btn = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

        if (tableStatus == TableStatus.Regular && currentSelectedViolation != null)
        {
            VideoReportPanel.SetActive(false);
            commentPanelReg.SetActive(true);
        }
        else if (tableStatus == TableStatus.Tournament && currentselectedviolationTour != null)
        {
            VideoReportPanelTournament.SetActive(false);
            commentPanelTour.SetActive(true);
        }
       

    }


    public void OnClickSubmitBtn()
    {

        if (tableStatus == TableStatus.Regular && currentSelectedViolation != null)
        {
            inputDataFromUserReg = inputFieldReg.text;
            VideoReportPanel.transform.parent.gameObject.SetActive(false);
            VideoReportPanel.SetActive(true);
            commentPanelReg.SetActive(false);
          
        }
        else if (tableStatus == TableStatus.Tournament && currentselectedviolationTour != null)
        {
            inputDataFromUserTour = inputFieldTour.text;
            VideoReportPanelTournament.transform.parent.gameObject.SetActive(false);
            VideoReportPanelTournament.SetActive(true);
            commentPanelTour.SetActive(false);
        }
        inputFieldReg.text = "";
        inputFieldTour.text = "";

      postDataCor=StartCoroutine(PostReportViolation());

    }



   

    public IEnumerator PostReportViolation() 
    {
        if (tableStatus == TableStatus.Regular)
        {
            ReportViolationClassRef.table_type = "video";
            ReportViolationClassRef.violation_type = currentSelectedViolation;
            ReportViolationClassRef.reporter_comment = inputDataFromUserReg;
        }
        else if (tableStatus == TableStatus.Tournament)
        {
            ReportViolationClassRef.table_type = "tournament";
            ReportViolationClassRef.violation_type = currentselectedviolationTour;
            ReportViolationClassRef.reporter_comment = inputDataFromUserTour;
        }



        while (!dataReady)
        {
            yield return new WaitForSeconds(1);
            Debug.Log(">>>>>>>>>>>>waiting for image...");
        }

        string jsonData = JsonUtility.ToJson(ReportViolationClassRef);
        Debug.Log(">>>>>>>>>>>>ReportviolationData : " + jsonData);


        // Post if no data available...
        if (ReportDataRef.ReporteeDatalist.Count == 0)
        {

            Communication.instance.PostData(reportViolationURL, jsonData, ReportViolationResponse);

           
            DataSet rObject = new DataSet(ReportViolationClassRef.reporter_id, ReportViolationClassRef.violator_id);
           
            ReportDataRef.ReporteeDatalist.Add(rObject);
            SaveDataToFile();
            Debug.Log(">>>>Reportee list is empty..");
        }
        else
        {
            //Check if the current user logged in  is the  same as the prvious user ... 
            if (ReportDataRef.ReporteeDatalist[0].Reportee.Equals(ReportViolationClassRef.reporter_id)) 
            {
                bool violatorExist = false;

                for (int i = 0; i < ReportDataRef.ReporteeDatalist.Count; i++) 
                {

                    if (ReportDataRef.ReporteeDatalist[i].violator.Equals(ReportViolationClassRef.violator_id))
                    {
                        violatorExist = true;
                        break;
                   
                    
                    }
                
                
                }

                if (violatorExist)
                {
                    // Do nothing,violator already been reported....
                    Debug.Log(">>>>Do nothing,violator already been reported");

                }
                else
                {
                    //new violator is been reported...

                    Communication.instance.PostData(reportViolationURL, jsonData, ReportViolationResponse);


                    DataSet rObject = new DataSet(ReportViolationClassRef.reporter_id, ReportViolationClassRef.violator_id);

                    ReportDataRef.ReporteeDatalist.Add(rObject);
                    SaveDataToFile();
                    Debug.Log(">>>>new violator is been reported");
                }

            }
            else
            {

                // Post if new user has login to the app...


                ReportDataRef.ReporteeDatalist.Clear();

                Communication.instance.PostData(reportViolationURL, jsonData, ReportViolationResponse);

                DataSet rObject = new DataSet(ReportViolationClassRef.reporter_id, ReportViolationClassRef.violator_id);

                ReportDataRef.ReporteeDatalist.Add(rObject);
                SaveDataToFile();
                Debug.Log(">>>>new user has login to the app data reported");

            }
        
        }



    }

    Coroutine cor;
    Coroutine postDataCor;
    public void TakeScreenShot()
    {

       cor= StartCoroutine(TakeScreenshot());

    }
    public GameObject screenshotPreview;
    

    public void StopExecution() 
    {

        if (cor != null)
            StopCoroutine(cor);

        if (postDataCor != null)
            StopCoroutine(postDataCor);

        Debug.Log("Couroutine Stopped");

        dataReady = false;

    }



    public void ReportViolationResponse(string response)
    {
        Debug.Log(">>>>>>response report violation " + response);

    }



    


    public IEnumerator TakeScreenshot()
    {


        string imageName = "screenshot.png";

        // Take the screenshot
        ScreenCapture.CaptureScreenshot(imageName);

        //Wait for 4 frames
        for (int i = 0; i < 5; i++)
        {
            yield return null;
        }

        // Read the data from the file
        byte[] data = File.ReadAllBytes(Application.persistentDataPath + "/" + imageName);
 
        Texture2D newTex =new Texture2D(Screen.width,Screen.height);
        newTex.LoadImage(data);
        newTex= ScaleTexture(newTex, 900, 512);

       
        byte[] jpgForm= newTex.EncodeToJPG();

        string base64Data = Convert.ToBase64String(jpgForm);


        Debug.Log("base64String : " + base64Data);

        ReportViolationClassRef.file = base64Data;






        dataReady = true;

        //// Create a sprite
        //Sprite screenshotSprite = Sprite.Create(newTex, new Rect(0, 0, newTex.width, newTex.height), new Vector2(0.5f, 0.5f));

        //// Set the sprite to the screenshotPreview
        //screenshotPreview.GetComponent<Image>().sprite = screenshotSprite


        //ReportViolationClassRef.violation_type = "611b5ef0f897f5e027cf834f";
        //ReportViolationClassRef.reporter_id = "107728";
        //ReportViolationClassRef.violator_id = "896547";
        //ReportViolationClassRef.reporter_comment = "hiding face..";
        //ReportViolationClassRef.table_type = "video";
        //ReportViolationClassRef.table_id = "0C0F83E4";
        //ReportViolationClassRef.game_id = "";
        //ReportViolationClassRef.game_type = "NLH";
        //ReportViolationClassRef.seat_id = "2";


        //string jsonData = JsonUtility.ToJson(ReportViolationClassRef);
        //Debug.Log(">>>>>>>>>>>>ReportviolationData : " + jsonData);

        //Communication.instance.PostData(reportViolationURL, jsonData, ReportViolationResponse);
       
    }


    private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
        Color[] rpixels = result.GetPixels(0);
        float incX = (1.0f / (float)targetWidth);
        float incY = (1.0f / (float)targetHeight);
        for (int px = 0; px < rpixels.Length; px++)
        {
            rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth), incY * ((float)Mathf.Floor(px / targetWidth)));
        }
        result.SetPixels(rpixels, 0);
        result.Apply();
        return result;
    }


}


