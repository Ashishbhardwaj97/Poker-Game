using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerChanger : MonoBehaviour
{
    public static ServerChanger instance;

    public string domainURL, staging_domainURL, dev_domainURL, awsDev_domainURL, awsLive_domainURL, awsQA_domainURL;
    public bool isStaging, isDev, isAWS;
    public bool isAWS_Live, isAWS_QA;

    private void Awake()
    {
        instance = this;

        staging_domainURL = "http://poker-stage.kelltontech.net:7000/";
        //staging_domainURL = "http://poker-stage.kelltontech.net:5000/";
        dev_domainURL = "http://poker-dev.kelltontech.net:5000/";

        awsDev_domainURL = "http://23.23.68.112:5000/";
        awsLive_domainURL = "https://poker-face.kelltontech.net:7000/";
        //awsQA_domainURL = "http://poker-qa.kelltontech.net:5000/";
        awsQA_domainURL = "https://poker-qa.kelltontech.net:6050/";  //....new.....//

        if (PokerSceneManagement.instance.IsServerChangerSceneRestart)
        {
            domainURL = PokerSceneManagement.instance.NewdomainURLFinal;
            isAWS = PokerSceneManagement.instance.isAWS;
            isAWS_Live = PokerSceneManagement.instance.isAWS_Live;
            isAWS_QA = PokerSceneManagement.instance.isAWS_QA;
            //PokerSceneManagement.instance.IsServerChangerSceneRestart = false;

        }
        else
        {
            if (isDev && !isAWS && !isStaging)
            {
                domainURL = dev_domainURL;
            }
            else if (isStaging && !isDev && !isAWS)
            {
                domainURL = staging_domainURL;
            }
            else if (isAWS && !isDev && !isStaging)
            {
                domainURL = awsDev_domainURL;
            }
            else if (isAWS_Live)
            {
                domainURL = awsLive_domainURL;
            }
            else if (isAWS_QA)
            {
                domainURL = awsQA_domainURL;
            }
        }

   
    }

    public void ClickOnServerChangeSubmitBtn()
    {
        if (isAWS)
        {
            domainURL = awsDev_domainURL;
            StartCoroutine(PokerSceneManagement.instance.ServerChangerSceneRestart(domainURL, 1));
        }
        else if (isAWS_Live)
        {
            domainURL = awsLive_domainURL;
            StartCoroutine(PokerSceneManagement.instance.ServerChangerSceneRestart(domainURL, 2));
        }
        else if (isAWS_QA)
        {
            domainURL = awsQA_domainURL;
            StartCoroutine(PokerSceneManagement.instance.ServerChangerSceneRestart(domainURL, 3));
        }
    }

    public void ClickRadioBtn(Transform radioPanel)
    {
        for (int i = 0; i < radioPanel.parent.parent.childCount; i++)
        {
            radioPanel.parent.parent.GetChild(i).GetChild(1).GetChild(0).gameObject.SetActive(false);
        }
        if (radioPanel.GetChild(0).gameObject.activeInHierarchy)
        {
            radioPanel.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            radioPanel.GetChild(0).gameObject.SetActive(true);

        }

        if (radioPanel.gameObject.CompareTag("live"))
        {
            isAWS = false;
            isAWS_Live = true;
            isAWS_QA = false;
        }
        else if (radioPanel.gameObject.CompareTag("dev"))
        {
            isAWS = true;
            isAWS_Live = false;
            isAWS_QA = false;
        }
        else
        {
            isAWS = false;
            isAWS_Live = false;
            isAWS_QA = true;
        }
    }


    public GameObject switchServerPanel;
    public void ClickSwitchServer()
    {
        if (isAWS)
        {
            switchServerPanel.transform.GetChild(2).GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(true);
            switchServerPanel.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(0).gameObject.SetActive(false);
            switchServerPanel.transform.GetChild(2).GetChild(2).GetChild(1).GetChild(0).gameObject.SetActive(false);

        }
        else if (isAWS_Live)
        {
            switchServerPanel.transform.GetChild(2).GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(false);
            switchServerPanel.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(0).gameObject.SetActive(true);
            switchServerPanel.transform.GetChild(2).GetChild(2).GetChild(1).GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            switchServerPanel.transform.GetChild(2).GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(false);
            switchServerPanel.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(0).gameObject.SetActive(false);
            switchServerPanel.transform.GetChild(2).GetChild(2).GetChild(1).GetChild(0).gameObject.SetActive(true);
        }
    }
}
