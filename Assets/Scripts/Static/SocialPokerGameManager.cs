using SmartLocalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialPokerGameManager : MonoBehaviour
{
    public static SocialPokerGameManager instance;
    [Header("GameObject Reference")]
    public GameObject homePage;
    public GameObject gamePage;
    public GameObject tournamentPage;
    public GameObject profilePage;
    public GameObject leaderPage;
    public GameObject buyChipsPage;
    public GameObject buyChipsBtn;
    public GameObject homeScreen;
    public GameObject spinWheelScreen;
    public GameObject exitPanel;
    public GameObject videoSelectionOutline;

    public List<GameObject> bottomNavigationBtn;
    public List<GameObject> videoNonVideoBtn;
    public List<GameObject> bottomUI;
    
    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (gamePage.activeInHierarchy)
        {
            buyChipsBtn.SetActive(true);
        }
        else
        {
            buyChipsBtn.SetActive(false);
        }

        if (buyChipsPage.activeInHierarchy)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                exitPanel.SetActive(false);
                buyChipsPage.SetActive(false);
            }
        }

        if (homePage.activeInHierarchy)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                exitPanel.SetActive(true);
            }
        }
        else if (gamePage.activeInHierarchy || tournamentPage.activeInHierarchy || profilePage.activeInHierarchy || leaderPage.activeInHierarchy)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                if (!TablePreferenceScript.instance.tableMatchObj.activeInHierarchy)
                {
                    exitPanel.SetActive(false);
                    StartCoroutine(DelayClickHome());
                }
            }
        }
    }
    IEnumerator DelayClickHome()
    {
        yield return new WaitForSeconds(0.2f);

        ClickHome();
    }

    public void ClickSpinwheelButton()
    {        
        if (!string.IsNullOrEmpty(Registration.instance.timeZone))
        {
            spinWheelScreen.SetActive(true);
            SpinRotate.instance.SpinChipsListRequest();
        }
        else
        {
            Cashier.instance.toastMsg.text = LanguageManager.Instance.GetTextValue("allow location");
            Cashier.instance.toastMsgPanel.SetActive(true);
        }

    }
    public void ClickSpinwheelButtonClose()
    {
        if (!SpinRotate.instance.isWheelRotate)
        {
            spinWheelScreen.SetActive(false);
            SpinRotate.instance.congratulationText.SetActive(false);
            SpinRotate.instance.tapToSpinBtn.SetActive(true);
        }
    }
    public void ClickHome()
    {
        if (string.IsNullOrEmpty(Registration.instance.timeZone))
        {
            StartCoroutine(Registration.instance.DetectCountry());
        }
        SocialProfile._instance.SocialChips();
        ClickBottomNavigationTab(bottomNavigationBtn[0].transform.GetChild(1).gameObject);
        EnableSingleObj(homePage);
        ConnectToSocialMediaScript.instance.GetLinkRequest();

        if (LatLongScript.instance.latitude == 0)
        {
            LatLongScript.instance.GettingLatLong();
        }
    }
    
    public void ClickGames()
    {
        if (string.IsNullOrEmpty(Registration.instance.timeZone))
        {
            StartCoroutine(Registration.instance.DetectCountry());
        }
        SocialGame.instance.ClickGameList();
        SocialProfile._instance.SocialChips();
        
    }

    public void EnableGamesPage()
    {
        ClickBottomNavigationTab(bottomNavigationBtn[1].transform.GetChild(1).gameObject);
        EnableSingleObj(gamePage);
        SocialGame.instance.configTableSlider.value = 0;
    }

    public void ClickTournament()
    {
        GameSerializeClassesCollection.instance.tournament.tournament_status = 0;
        GameSerializeClassesCollection.instance.tournament = null;
        //GameSerializeClassesCollection.Tournament newTour = new GameSerializeClassesCollection.Tournament();
        //GameSerializeClassesCollection.instance.tournament = newTour;
        GameSerializeClassesCollection.instance.tournament = new GameSerializeClassesCollection.Tournament();



        if (string.IsNullOrEmpty(Registration.instance.timeZone))
        {
            StartCoroutine(Registration.instance.DetectCountry());
        }
        SocialTournamentScript.instance.ClickUpcomingTab();
    }

    public void EnableTournamentPage()
    {
        ClickBottomNavigationTab(bottomNavigationBtn[2].transform.GetChild(1).gameObject);
        EnableSingleObj(tournamentPage);
    }

    public void ClickLeaders()
    {
        LeadershipScript.instance.ResetAllValuesForImage4();
        LeadershipScript.instance.ClickCashGameTab();
    }

    public void EnableLeadersPage()
    {
        ClickBottomNavigationTab(bottomNavigationBtn[3].transform.GetChild(1).gameObject);
        EnableSingleObj(leaderPage);
    }

    public void ClickProfile()
    {
        TablePreferenceScript.instance.tableMatchObj.SetActive(false);
        ClickBottomNavigationTab(bottomNavigationBtn[4].transform.GetChild(1).gameObject);
        EnableSingleObj(profilePage);

    }

    void ClickBottomNavigationTab(GameObject obj)
    {
        for (int i = 0; i < bottomNavigationBtn.Count; i++)
        {
            bottomNavigationBtn[i].transform.GetChild(1).gameObject.SetActive(false);
        }
        obj.SetActive(true);
    }

    public void ClickVideoNonVideo(GameObject obj)
    {
        for (int i = 0; i < videoNonVideoBtn.Count; i++)
        {
            videoNonVideoBtn[i].transform.GetChild(1).gameObject.SetActive(false);
        }
        obj.SetActive(true);
        if (obj.CompareTag("video"))
        {
            SocialGame.instance.currentIsVideo = true;
        }
        else
        {
            SocialGame.instance.currentIsVideo = false;
        }
    }

    void EnableSingleObj(GameObject objectUI)
    {
        for (int i = 0; i < bottomUI.Count; i++)
        {
            bottomUI[i].SetActive(false);
        }
        objectUI.SetActive(true);
    }

    #region Throwabales Animation

    public GameObject selectedDrag;
    public GameObject animObj;
    internal Vector3 endPosition;
    public Vector3 dropPosition;
    Animator animator;
    float animLength;
    public Transform objToAnimate;
    public Vector3 sourcePosition;

    public void PlayThrowableAnimation(string anim, string destination, string source)
    {

        if (GameManagerScript.instance.isVideoTable)
        {
            for (int i = 0; i < GameManagerScript.instance.playersParent.transform.childCount; i++)
            {
                if (GameManagerScript.instance.playersParent.transform.GetChild(i).childCount == 2)
                {
                    if (GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).gameObject.name == source)
                    {
                        sourcePosition = GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetChild(1).gameObject.transform.position;

                        //........ Throwable ui button animation from source to destination..........//
                        for (int j = 0; j < GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetChild(5).GetChild(1).childCount; j++)
                        {
                            if (GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetChild(5).GetChild(1).GetChild(j).gameObject.name == anim)
                            {
                                print("objToAnimate........");
                                objToAnimate = GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetChild(5).GetChild(1).GetChild(j);
                            }
                        }
                    }

                    if (GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).gameObject.name == destination)
                    {
                        dropPosition = GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetChild(1).gameObject.transform.position;

                        //Assign animation object
                        for (int j = 0; j < GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetChild(5).GetChild(0).childCount; j++)
                        {
                            if (GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetChild(5).GetChild(0).GetChild(j).GetChild(0).gameObject.name == anim)
                            {
                                print("animObj........");
                                animObj = GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetChild(5).GetChild(0).GetChild(j).gameObject;
                                animator = GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetChild(5).GetChild(0).GetChild(j).GetComponentInChildren<Animator>();
                            }
                        }

                    }
                }
            }

            objToAnimate.transform.position = sourcePosition;
            objToAnimate.gameObject.SetActive(true);
            AnimateTransformFunctions.ins.AnimateTransform(objToAnimate, sourcePosition, dropPosition, 0.5f, AnimateTransformFunctions.TransformTypes.Position, AnimateTransformFunctions.AnimAxis.World, "EaseIn");    // CARD ANIM
            StartCoroutine(ThrowabeAnimation(0.5f));
        }
        else
        {
            //........non video table.....//

            for (int i = 0; i < GameManagerScript.instance.playersParent.transform.childCount; i++)
            {
                if (GameManagerScript.instance.playersParent.transform.GetChild(i).childCount == 2)
                {
                    if (GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).gameObject.name == source)
                    {
                        sourcePosition = GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(1).GetChild(1).gameObject.transform.position;

                        //........ Throwable ui button animation from source to destination..........//
                        for (int j = 0; j < GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(1).GetChild(18).GetChild(1).childCount; j++)
                        {
                            if (GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(1).GetChild(18).GetChild(1).GetChild(j).gameObject.name == anim)
                            {
                                print("objToAnimate........");
                                objToAnimate = GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(1).GetChild(18).GetChild(1).GetChild(j);
                            }
                        }
                    }

                    if (GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).gameObject.name == destination)
                    {
                        dropPosition = GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(1).GetChild(1).gameObject.transform.position;

                        //Assign animation object
                        for (int j = 0; j < GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(1).GetChild(18).GetChild(0).childCount; j++)
                        {
                            if (GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(1).GetChild(18).GetChild(0).GetChild(j).GetChild(0).gameObject.name == anim)
                            {
                                print("animObj........");
                                animObj = GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(1).GetChild(18).GetChild(0).GetChild(j).gameObject;
                                animator = GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(1).GetChild(18).GetChild(0).GetChild(j).GetComponentInChildren<Animator>();
                            }
                        }

                    }
                }
            }

            objToAnimate.transform.position = sourcePosition;
            objToAnimate.gameObject.SetActive(true);
            AnimateTransformFunctions.ins.AnimateTransform(objToAnimate, sourcePosition, dropPosition, 0.5f, AnimateTransformFunctions.TransformTypes.Position, AnimateTransformFunctions.AnimAxis.World, "EaseIn");    // CARD ANIM
            StartCoroutine(ThrowabeAnimation(0.5f));
        }
    }

    IEnumerator ThrowabeAnimation(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        objToAnimate.gameObject.SetActive(false);
        if (animObj != null)
        {
            animObj.transform.position = dropPosition;
            animObj.SetActive(true);
        }
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

        animLength = clips[0].length;

        StartCoroutine(ResetThrowableBtn(animLength));
}

    IEnumerator ResetThrowableBtn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        print("ResetThrowableBtn.....");

        animObj.SetActive(false);
        ResetThrowableAnimation();
    }

    public void ResetThrowableAnimation()
    {
        for (int i = 0; i < GameManagerScript.instance.playersParent.transform.childCount; i++)
        {
            if (GameManagerScript.instance.playersParent.transform.GetChild(i).childCount == 2)
            {
                for (int j = 0; j < GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetChild(5).GetChild(0).childCount; j++)
                {
                    if (GameManagerScript.instance.isVideoTable)
                    {
                        GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetChild(5).GetChild(0).GetChild(j).gameObject.SetActive(false);
                        GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetChild(5).GetChild(1).GetChild(j).gameObject.SetActive(false);
                    }
                    else
                    {
                        GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(1).GetChild(18).GetChild(0).GetChild(j).gameObject.SetActive(false);
                        GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(1).GetChild(18).GetChild(1).GetChild(j).gameObject.SetActive(false);
                    }
                }
            }
        }
    }
    #endregion
}