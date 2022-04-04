using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Members : MonoBehaviour
{
    public static Members instance;
    public GameObject memberList;
    public GameObject requestList;
    public GameObject requestImage;
    public GameObject memberImage;

    public GameObject memberCanvas;
    public GameObject clubHomePage;
    public GameObject joinClubCanvas;
    //public GameObject menuFadePanel;
    public GameObject menuPanel;
    public GameObject buttonsPanel;
    public GameObject searchInputPanel;
    int groupByCount=0;

    int statusCount=0;
    int feesCount=0;
    int winningCount=0;
    int gamesCount=0;
    //public Text clubName, clubId;
    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (memberList.activeInHierarchy)
        {
            memberCanvas.transform.GetChild(4).gameObject.SetActive(true);
            memberCanvas.transform.GetChild(5).gameObject.SetActive(true);
            memberCanvas.transform.GetChild(8).gameObject.SetActive(true);
        }
        else
        {
            memberCanvas.transform.GetChild(4).gameObject.SetActive(false);
            memberCanvas.transform.GetChild(5).gameObject.SetActive(false);
            memberCanvas.transform.GetChild(8).gameObject.SetActive(false);
        }
    }

    public void GroupByTick()
    {
        if (memberCanvas.transform.GetChild(5).GetChild(0).gameObject.activeInHierarchy)
        {
            memberCanvas.transform.GetChild(5).GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            memberCanvas.transform.GetChild(5).GetChild(0).gameObject.SetActive(true);
        }
    }

    public void StatusTick()
    {
        statusCount++;
        if (statusCount % 2 == 0)
        {
            memberCanvas.transform.GetChild(10).GetChild(0).GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            memberCanvas.transform.GetChild(10).GetChild(0).GetChild(2).gameObject.SetActive(true);
        }
    }

    public void FeesTick()
    {
        feesCount++;
        if (feesCount % 2 == 0)
        {
            memberCanvas.transform.GetChild(10).GetChild(1).GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            memberCanvas.transform.GetChild(10).GetChild(1).GetChild(2).gameObject.SetActive(true);
        }
    }

    public void WinningTick()
    {
        winningCount++;
        if (winningCount % 2 == 0)
        {
            memberCanvas.transform.GetChild(10).GetChild(2).GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            memberCanvas.transform.GetChild(10).GetChild(2).GetChild(2).gameObject.SetActive(true);
        }
    }

    public void GamesTick()
    {
        gamesCount++;
        if (gamesCount % 2 == 0)
        {
            memberCanvas.transform.GetChild(10).GetChild(3).GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            memberCanvas.transform.GetChild(10).GetChild(3).GetChild(2).gameObject.SetActive(true);
        }
    }

    public void OpenFilterPanel()
    {
        memberCanvas.transform.GetChild(10).gameObject.SetActive(true);
        memberCanvas.transform.GetChild(9).gameObject.SetActive(true);
    }

    public void CloseFilterPanel()
    {
        memberCanvas.transform.GetChild(10).gameObject.SetActive(false);
        memberCanvas.transform.GetChild(9).gameObject.SetActive(false);
    }

    public void OpenMembersList()
    {
        //buttonsPanel.transform.localPosition = new Vector3(0, 470, 0);
        //memberList.SetActive(true);
        requestList.SetActive(false);
        requestImage.SetActive(false);
        memberImage.SetActive(true);
        searchInputPanel.SetActive(true);
        searchInputPanel.transform.GetComponent<InputField>().text = string.Empty;
    }

    public void OpenRequestList()
    {
       // buttonsPanel.transform.localPosition = new Vector3(0, 576, 0);
        memberList.SetActive(false);
        //requestList.SetActive(true);
        requestImage.SetActive(true);
        memberImage.SetActive(false);
        searchInputPanel.SetActive(false);
    }

    public Text tempClubIDText;
    public void BackMemberCanvas()
    {
        ClubManagement.instance.ClickMyClubs();
        ApiHitScript.instance.clubPage.SetActive(false);
        memberCanvas.SetActive(false);
        clubHomePage.SetActive(true);
        transform.GetChild(5).GetChild(0).gameObject.SetActive(false);

        tempClubIDText.text = ClubManagement.instance._clubID;
        ClubManagement.instance.ClickOnClubDetails(tempClubIDText);
    }

    public void OpenJoinClub()
    {
        memberCanvas.SetActive(false);
        joinClubCanvas.SetActive(true);
    }

    public void OpenMenu()
    {
        //menuFadePanel.SetActive(true);
        menuPanel.SetActive(true);
    }
}
