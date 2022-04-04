using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class ImageDownloadScript : MonoBehaviour
{
    public static ImageDownloadScript instance;

    [SerializeField] string url;

    public bool is100KB = true;
    public bool is80KB;
    public bool is60KB;
    public bool is20KB;
    public bool is10KB;

    string awsBaseUrl;
    string urlEndPoint;
    double fileSizeInKB;

    Texture2D tex;

    public double speed;
    public string internetSpeed;
    public string internetSpeedIn_kbps;

    DateTime oldTimer;
    DateTime newTimer;

    bool isResponse;


    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        awsBaseUrl = "https://d261mcojk20to7.cloudfront.net/"; //.....main

        //awsBaseUrl = "http://poker-dev-267253037.us-east-1.elb.amazonaws.com/"; //....for performance test
    
        if (is100KB)
        {
            urlEndPoint = "demo.png";
            fileSizeInKB = 100;
        }
        else if (is80KB)
        {
            urlEndPoint = "80kb.png";
            fileSizeInKB = 83;
        }
        else if (is60KB)
        {
            urlEndPoint = "60kb.png";
            fileSizeInKB = 63;
        }
        else if (is20KB)
        {
            urlEndPoint = "20kb.png";
            fileSizeInKB = 23;
        }
        else if (is10KB)
        {
            urlEndPoint = "10kb.png";
            fileSizeInKB = 13;
        }
        else
        {
            is100KB = true;
            urlEndPoint = "demo.png";
            fileSizeInKB = 100;
        }


        if (ServerChanger.instance.isAWS)
        {
            url = awsBaseUrl + urlEndPoint;
        }
        else
        {
            //url = ServerChanger.instance.domainURL + urlEndPoint;
            url = awsBaseUrl + urlEndPoint;
        }

    }

    public IEnumerator RepeatInternetAPIHit()
    {
        print("HitIt");
        yield return new WaitForSeconds(2f);
        while (true)
        {
            if (!isResponse)
            {
                GetImage(url, InternetHandling);
                isResponse = true;
            }
            yield return new WaitForSeconds(Random.Range(1f, 2f));
        }
    }

    void InternetHandling(double response)
    {
        if(response > 80)
        {
            GameManagerScript.instance.slowInternet = false;
            GameManagerScript.instance.noInternet = false;
            GameManagerScript.instance.fastInternet = true;
        }
        else if(response > 48 && response < 80)
        {
            GameManagerScript.instance.noInternet = false;
            GameManagerScript.instance.fastInternet = false;
            GameManagerScript.instance.slowInternet = true;
        }
        else 
        {
            GameManagerScript.instance.slowInternet = false;
            GameManagerScript.instance.fastInternet = false;
            GameManagerScript.instance.noInternet = true;
        }
    }


    public void GetImage(string url, Action<double> _callback)
    {
        //print("GetImage");
        StartCoroutine(GetImageCoroutine(url, _callback));
        //GameManagerScript.instance.InternetHandling();
        //oldTimer = DateTime.Now;
        //print("oldTimer........" + oldTimer);
    }

    public IEnumerator startTimeCorotine;
    
    public IEnumerator GetImageCoroutine(string url, Action<double> _callback)
    {
        UnityWebRequest wwwWebRequest = UnityWebRequestTexture.GetTexture(url);

        oldTimer = DateTime.Now;

        //string oldTimer2 = oldTimer.ToString("HH:mm:ss.fff");
        if (startTimeCorotine != null)
        {
            StopCoroutine(startTimeCorotine);
        }
                
        startTimeCorotine = StartTimer();
        StartCoroutine(startTimeCorotine);

        yield return wwwWebRequest.SendWebRequest();

        if (wwwWebRequest.isDone)
        {
            try
            {
                tex = ((DownloadHandlerTexture)(wwwWebRequest.downloadHandler)).texture;
                Destroy(tex);
                //imageSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
                isResponse = false;
                newTimer = DateTime.Now;

                string newTimer2 = newTimer.ToString("HH:mm:ss.fff");
                //print("newTimer2........" + newTimer2);

                TimeSpan timegap = newTimer - oldTimer;
                double totalMS = timegap.TotalMilliseconds - 10;
                //print("totalMS........" + totalMS);
                if (totalMS != 0)
                {
                    speed = fileSizeInKB * 800 / totalMS;
                }
                //print("speed......." + speed);
                internetSpeed = speed.ToString("f2") + " KB/s";
                internetSpeedIn_kbps = (speed * 8).ToString("f2") + " kbps";
                //print("InternetSpeed: " + speed * 8 + "kbps");

                speed *= 8;
                _callback(speed * 8);

            }
            catch (Exception e)
            {
                //Debug.Log("Image downloading UN-success");
                Debug.Log(e.Message);
                GameManagerScript.instance.slowInternet = false;
                GameManagerScript.instance.fastInternet = false;
                GameManagerScript.instance.noInternet = true;
                isException = true;
            }

        }
        else
        {
            Debug.Log(": Error: " + wwwWebRequest.error);

            GameManagerScript.instance.slowInternet = false;
            GameManagerScript.instance.fastInternet = false;
            GameManagerScript.instance.noInternet = true;
            yield break;
        }

    }

    [Header("Timer Initialisation")]
    public int currentTimerVal;
    //public int maxTimeLimitForFastInternet = 2;
    //public int maxTimeLimitForSlowInternet = 4;
    public int NoInternetLimit = 8;  
    public int maxTimeLimitForInternetCheck = 25;

    public IEnumerator StartTimer()
    {
        currentTimerVal = 0;
        //print("StartTimer");
        while (currentTimerVal <= maxTimeLimitForInternetCheck)
        {
            yield return new WaitForSeconds(1);
            
            if (!isResponse)
            {
                //print("response aa gya hai.....");
                //print("current time " + currentTimerVal);
                break;
            }

            currentTimerVal++;

            //if (currentTimerVal < maxTimeLimitForFastInternet)
            //{
            //    //print("fast internet");
            //    GameManagerScript.instance.slowInternet = false;
            //    GameManagerScript.instance.fastInternet = true;
            //    GameManagerScript.instance.noInternet = false;
            //}
            //else if (maxTimeLimitForFastInternet < currentTimerVal && currentTimerVal < maxTimeLimitForSlowInternet)
            //{
            //    //print("slow internet");
            //    GameManagerScript.instance.slowInternet = true;
            //    GameManagerScript.instance.fastInternet = false;
            //    GameManagerScript.instance.noInternet = false;
            //}
            /*else */ if (currentTimerVal == NoInternetLimit)
            {
                //print("no internet");
                GameManagerScript.instance.slowInternet = false;
                GameManagerScript.instance.fastInternet = false;
                GameManagerScript.instance.noInternet = true;
            }

            StartCoroutine(CheckInternetConnection((isConnected) => {
                
                //print("internet connection : " + isConnected);

                if (isConnected && isException)
                {
                    isException = false;
                    GetImage(url, InternetHandling);
                }
                
            }));

        }
    }

   public bool isException;
    IEnumerator CheckInternetConnection(Action<bool> action)
    {
        UnityWebRequest request = new UnityWebRequest("http://google.com");
        yield return request.SendWebRequest();
        if (request.error != null)
        {
            //Debug.Log("Error");
            action(false);
        }

        else
        {
            //Debug.Log("Success");
            action(true);
        }
    }
}
