using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PokerSceneManagement : GenericSingletonClass<PokerSceneManagement>
{
    //public static PokerSceneManagement instance;
    [SerializeField]
    List<string> currentwaitlistUser;
    public List<string> CurrentWaitListUser { get { return currentwaitlistUser; } set { currentwaitlistUser = value; } }

    public GameObject loadingOnRestart;
    //private void Awake()
    //{
    //    if (instance != null)
    //    {
    //        Destroy(this.gameObject);
    //    }
    //    else
    //    {

    //        instance = this;

    //        DontDestroyOnLoad(this.gameObject);
    //    }
    //}

    public void RestartScene()
    {
        StartCoroutine(Restart());
    }
    string tourID;
    public bool isSceneRestart;
    public bool istour;
    public bool isTournamentVideo;
    public bool isRegister;
    IEnumerator Restart()
    {
        isSceneRestart = true;
        istour = GameManagerScript.instance.isTournament;
        tourID = SocialTournamentScript.instance.tournament_ID;
        loadingOnRestart.SetActive(true);
        isTournamentVideo = SocialTournamentScript.instance.isTournamentVideo;
        isRegister = SocialTournamentScript.instance.isRegistered;
        //UIManagerScript.instance.loading2.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(0);
        ClubManagement.instance.loadingPanel.SetActive(true);

        while (!operation.isDone)
        {
            yield return null;
            print("1111" + operation.isDone);
            if (!istour)
            {
                ApiHitScript.instance.SplashScreenCanvas.transform.GetChild(3).gameObject.SetActive(true);
            }
            else
            {
                if (operation.isDone)
                {
                    SocialTournamentScript.instance.isTournamentVideo = isTournamentVideo;
                    SocialTournamentScript.instance.OpenGameDetailPageAfterRegistration(tourID, isRegister);
                    ClubManagement.instance.loadingPanel.SetActive(false);
                }
            }
        }
    }

    public bool IsServerChangerSceneRestart;
    public bool isAWS;
    public bool isAWS_Live;
    public bool isAWS_QA;
    public string NewdomainURLFinal;
    public IEnumerator ServerChangerSceneRestart(string NewdomainURL, int Sever)
    {
        NewdomainURLFinal = NewdomainURL;
        IsServerChangerSceneRestart = true;
        ClubManagement.instance.loadingPanel.SetActive(true);
        if(Sever == 1)
        {
            isAWS = true;
            isAWS_Live = false;
            isAWS_QA = false;
        }
        else if(Sever == 2)
        {
            isAWS = false;
            isAWS_Live = true;
            isAWS_QA = false;
        }
        else if (Sever == 3)
        {
            isAWS_QA = true;
            isAWS = false;
            isAWS_Live = false;
        }
        AsyncOperation operation = SceneManager.LoadSceneAsync(0);
        while (!operation.isDone)
        {
            yield return null;
            print("1111" + operation.isDone);

            if (operation.isDone)
            {
                //ClubManagement.instance.loadingPanel.SetActive(false);
            }

        }
    }

}
