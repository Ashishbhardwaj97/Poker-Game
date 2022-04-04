using SmartLocalization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SpinRotate : MonoBehaviour
{
    public static SpinRotate instance;

    private string spinAmountUrl;
    private string spinWheelListUrl;
    private string spinTimeUrl;
    public GameObject spinWheelBoard;
    public float spinwheelspeed;
    public GameObject spinWheelAmount;
    public GameObject tapToSpinBtn;
    public GameObject spinWheelBtn;
    public GameObject timerStrip;
    public GameObject congratulationText;
    public bool isWheelRotate;
    public Button crossBtn;

    public int randomSpeed;
    public int minSpeed;
    public int maxSpeed;
    public Text[] spinTheWheelValues;
    public Transform spinAmount;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //ResumeApp();
        spinwheelspeed = 0;
        minSpeed = 70;
        maxSpeed = 90;
        //spinAmountUrl = ServerChanger.instance.domainURL + "api/v1/user/add-social-chips";
        spinAmountUrl = ServerChanger.instance.domainURL + "api/v1/user/add-spin-wheel-chips";
        spinTimeUrl = ServerChanger.instance.domainURL + "api/v1/user/get-spin-time";
        spinWheelListUrl = ServerChanger.instance.domainURL + "api/v1/user/spin-the-wheel-list";
    }

    void Update()
    {

        if (spinwheelspeed > 0)
        {
            spinwheelspeed = spinwheelspeed - randomSpeed * Time.deltaTime;
            if (spinwheelspeed <= 0)
            {
                spinWheelAmount.SetActive(true);
                spinwheelspeed = 0;
                StartCoroutine(Delay());
                isWheelRotate = false;
                crossBtn.interactable = true;   
            }
        }
        spinWheelBoard.transform.Rotate(0, 0, -(spinwheelspeed * Time.deltaTime));
    }



    public void TaptoSpinwheel()
    {
        timerStrip.SetActive(true);
        spinWheelBtn.GetComponent<Button>().interactable = false;
        tapToSpinBtn.GetComponent<Button>().interactable = false;
        spinWheelAmount.SetActive(false);
        spinwheelspeed = 400;
        //StopRunningTimeCoroutine();
        //StartRunningTimeCoroutine();
        isWheelRotate = true;
        crossBtn.interactable = false;
        randomSpeed = Random.Range(minSpeed, maxSpeed);
    }

    #region Time Count Down

    //..........By Basant......//

    public Text timerText;
    public double totalTime = 3600f;

    Coroutine rotine;
    public double remainingTime;
    public void StopRunningTimeCoroutine()
    {
        if (rotine != null)
        {
            StopCoroutine(rotine);
        }
    }
    public void StartRunningTimeCoroutine()
    {
        rotine = StartCoroutine(RemainingTimerValue(totalTime, timerText));
    }

    public IEnumerator RemainingTimerValue(double _totalTableRemainingTime, Text _text)
    {
        int totalTableRemainingTime = (int)_totalTableRemainingTime;
        while (true)
        {
            if (totalTableRemainingTime > 0)
            {
                totalTableRemainingTime -= 1;
                remainingTime = totalTableRemainingTime;
                RemainingDisplayTime(totalTableRemainingTime, _text);
                yield return new WaitForSeconds(1);
                if (totalTableRemainingTime < 59)
                {
                    Debug.Log("Timer has Ended!....");
                    _text.text = "00:00:00";
                    timerStrip.SetActive(false);
                    spinWheelBtn.GetComponent<Button>().interactable = true;
                    tapToSpinBtn.GetComponent<Button>().interactable = true;
                    break;
                }
            }

            //else
            //{
            //    Debug.Log("Timer has Ended!");
            //    _text.text = "00:00:00";
            //    timerStrip.SetActive(false);
            //    spinWheelBtn.GetComponent<Button>().interactable = true;
            //    tapToSpinBtn.GetComponent<Button>().interactable = true;
            //    break;
            //}
        }
    }

    // To Display the Time
    void RemainingDisplayTime(float timeToDisplay, Text _text)
    {
        timeToDisplay += 1;

        float hours = Mathf.FloorToInt(timeToDisplay / 3600);
        float minutes = Mathf.FloorToInt(timeToDisplay / 60) % 60;
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        _text.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);

    }

    #endregion

    #region Calculate Time Difference

    string oldTime;

    double timeDiff;
    double _timeRemaining;

    private void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            //print("come back to app....");
            GetSpinTime();
        }
    }

    //public void ResumeApp()
    //{
    //    if (PlayerPrefs.HasKey("OLD_TIME"))
    //    {
    //        oldTime = PlayerPrefs.GetString("OLD_TIME");
    //        timeDiff = ClubManagement.instance.TimeDifference(oldTime);

    //        timeDiff = Math.Abs(timeDiff);

    //    }

    //    if (PlayerPrefs.HasKey("TIME_DIFF"))
    //    {
    //        string _timeDiff = PlayerPrefs.GetString("TIME_DIFF");

    //        if (!string.IsNullOrEmpty(_timeDiff))
    //        {
    //            _timeRemaining = double.Parse(_timeDiff);
    //        }

    //        if (_timeRemaining < timeDiff)
    //        {
    //            print("Enable spin .........");

    //            spinWheelBtn.GetComponent<Button>().interactable = true;
    //            tapToSpinBtn.GetComponent<Button>().interactable = true;
    //            timerText.text = "00:00";
    //            timerStrip.SetActive(false);
    //        }

    //        else
    //        {
    //            if (_timeRemaining - timeDiff < 59)
    //            {
    //                print("Enable spin .........");
    //                spinWheelBtn.GetComponent<Button>().interactable = true;
    //                tapToSpinBtn.GetComponent<Button>().interactable = true;
    //                timerText.text = "00:00";
    //                timerStrip.SetActive(false);
    //            }

    //            else
    //            {
    //                print("Not Enable spin .........");

    //                spinWheelBtn.GetComponent<Button>().interactable = false;
    //                tapToSpinBtn.GetComponent<Button>().interactable = false;

    //                double timeRemain = _timeRemaining - timeDiff;
    //                StopRunningTimeCoroutine();
    //                rotine = StartCoroutine(RemainingTimerValue(timeRemain, timerText));
    //                timerStrip.SetActive(true);
    //            }
    //        }
    //    }
    //}
    #endregion
    //............................................................//

    [Serializable]
    public class SpinRequest
    {
        public int social_chips;
        public string timeZone;

    }
    [SerializeField] SpinRequest spinRequest;


    [Serializable]
    public class SpinResponse
    {
        public bool error;
        public string diamond;
    }
    [SerializeField] SpinResponse spinResponse;

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        //isSpin = false;
        SpinStopAmountRequest();
    }

    public void SpinStopAmountRequest()
    {
        spinRequest.social_chips = WheelPin._instanceSpinWheel.spinvalue;
        spinRequest.timeZone = Registration.instance.timeZone;
        string body = JsonUtility.ToJson(spinRequest);

        print("SpinWheel body--" + body);

        Communication.instance.PostData(spinAmountUrl, body, SpinStopAmountRequestCallBack);
    }

    //social_chips
    public void SpinStopAmountRequestCallBack(string response)
    {
        print("Spin Response: " + response);
        if (string.IsNullOrEmpty(response))
        {
            print("error");
            Cashier.instance.toastMsg.text = LanguageManager.Instance.GetTextValue("internet check");//SocialProfile._instance.networkErrorMsg;
            Cashier.instance.toastMsgPanel.SetActive(true);
        }

        else
        {
            spinResponse = JsonUtility.FromJson<SpinResponse>(response);
            if (!spinResponse.error)
            {
                congratulationText.SetActive(true);
                tapToSpinBtn.SetActive(false);
                //congratulationText.transform.GetChild(0).GetComponent<Text>().text = "$" + (spinRequest.social_chips / 1000).ToString();
                congratulationText.transform.GetChild(0).GetComponent<Text>().text = (spinRequest.social_chips / 1000).ToString();

                PlayerPrefs.SetString("SOCIAL_CHIPS", spinResponse.diamond);

                for (int i = 0; i < ApiHitScript.instance.socialChips.Count; i++)                                     //Updating Social Chips
                {
                    //ApiHitScript.instance.socialChips[i].text = "$" + spinResponse.diamond;
                    ApiHitScript.instance.socialChips[i].text = spinResponse.diamond;
                }
                GetSpinTime();
            }
            else
            {
                print("Spin wheel Not Success..");
            }

        }
    }

    #region GetSpinTime

    [Serializable]
    public class TimeSpinResponse
    {
        public bool error;
        public string dateTime;
    }
    [SerializeField] TimeSpinResponse timeSpinResponse;

    public void GetSpinTime()
    {
        if (!string.IsNullOrEmpty(Communication.instance.playerToken))
        {
            if (!string.IsNullOrEmpty(spinTimeUrl))
            {
                ClubManagement.instance.loadingPanel.SetActive(true);
                Communication.instance.PostData(spinTimeUrl, GetSpinTimeRequestCallBack);
            }
        }
    }

    Coroutine spinTimeRotine;
    double timeDiff1;
    public void GetSpinTimeRequestCallBack(string response)
    {
        print("GetSpinTime Response: " + response);
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (!string.IsNullOrEmpty(response))
        {
            timeSpinResponse = JsonUtility.FromJson<TimeSpinResponse>(response);

            if (!timeSpinResponse.error)
            {
                print("GetSpinTime Response: " + response);

                timeDiff1 = ClubManagement.instance.TimeDifference(timeSpinResponse.dateTime);

                StopSpinTimeCoroutine();

                if (timeDiff1 > 0)
                {
                    //print("Time Left");    
                    spinTimeRotine = StartCoroutine(RemainingTimerValue(timeDiff1, timerText));
                    timerStrip.SetActive(true);
                    spinWheelBtn.GetComponent<Button>().interactable = false;
                }
                else
                {
                    //print("Time Completed");
                    timerText.text = "00:00:00";
                    timerStrip.SetActive(false);
                    spinWheelBtn.GetComponent<Button>().interactable = true;
                    tapToSpinBtn.GetComponent<Button>().interactable = true;
                }
            }
        }

        else
        {
            print("Unable to fetch time from Server...");
        }
    }

    public void StopSpinTimeCoroutine()
    {
        if (spinTimeRotine != null)
        {
            StopCoroutine(spinTimeRotine);
        }
    }
    #endregion

    #region Get Spin The Wheel Value List

    [Serializable]
    public class SpinWheelListResponse
    {
        public bool error;
        public Data[] data;
    }

    [Serializable]
    public class Data
    {
        public float chips;
    }

    [SerializeField] SpinWheelListResponse spinWheelListResponse;

    public void SpinChipsListRequest()
    {
        ClubManagement.instance.loadingPanel.SetActive(true);
        Communication.instance.GetData(spinWheelListUrl, SpinChipsListCallback);
    }

    void SpinChipsListCallback(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);

        if (!string.IsNullOrEmpty(response))
        {
            spinWheelListResponse = JsonUtility.FromJson<SpinWheelListResponse>(response);

            print("SpinChipsListCallback ........ : " + response);

            if (!spinWheelListResponse.error)
            {
                for (int i = 0; i < spinWheelListResponse.data.Length; i++)
                {
                    spinTheWheelValues[i].text = CurrencyConversion.instance.ChangeValuestoMillions(spinWheelListResponse.data[i].chips);
                    spinAmount.GetChild(i).gameObject.name = spinWheelListResponse.data[i].chips.ToString();
                }
            }
        }
    }


    #endregion
}