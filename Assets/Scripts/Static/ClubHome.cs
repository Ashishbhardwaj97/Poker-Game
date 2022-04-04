using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClubHome : MonoBehaviour
{
    public GameObject allImage;
    public GameObject allPanel;
    public GameObject regularScrollListPanel;
    public GameObject mttScrollListPanel;
    public GameObject regularImage;
    public GameObject mTTImage;
    //public GameObject regularPanel;
    public GameObject menuPanel;
    public GameObject cashier;
    public GameObject member;
    public GameObject admin;
    public GameObject adminAgent;
    public GameObject adminPlayer;
    public GameObject adminPanel;
    public GameObject statistics;
    public GameObject clubHomePage;
    public GameObject joinClubCanvas;

    public GameObject filterPanel;
    public GameObject tableText;
    public GameObject addButton;
    public GameObject allScrollList;
    public GameObject allScrollListViewPort;

    public GameObject clubHome;
    public GameObject clubListing;
    public GameObject menuFadePanel;
    public GameObject button;
    public GameObject socialMediaPanel;

    int openFilterCount = 0;

    public GameObject chipApplicationPanel;

    public void OpenApplicationPanel()
    {
        if (ClubManagement.instance.currentRoleInSelectedClub == 3 || ClubManagement.instance.currentRoleInSelectedClub == 5 || ClubManagement.instance.currentRoleInSelectedClub == 6)
        {
            //.....For Player....//
            chipApplicationPanel.SetActive(true);
            ClubManagement.instance.chipValApplicationPanel.text = ClubManagement.instance.individualClipBalance.text;
        }
        else
        {
            //.....For Owner, Manager and Agent....//
            Cashier.instance.toastMsg.text = "Tap <Cashier> button below to send yourself chips.";
            Cashier.instance.toastMsgPanel.SetActive(true);
        }
    }

    public void OpenApplicationPanelForAgent()
    {
        chipApplicationPanel.SetActive(true);
        ClubManagement.instance.chipValApplicationPanel.text = Cashier.instance.agentCreditBalance.text;
    }

    public void CloseApplicationPanel()
    {
        chipApplicationPanel.SetActive(false);
        ClubManagement.instance.amountInputField.text = string.Empty;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuPanel.activeInHierarchy)
            {
                menuPanel.SetActive(false);
                menuFadePanel.SetActive(false);
            }

            else if (admin.transform.GetChild(1).gameObject.activeInHierarchy)
            {
                admin.transform.GetChild(1).gameObject.SetActive(false);
                admin.transform.GetChild(0).gameObject.SetActive(true);
            }

            else if (admin.activeInHierarchy)
            {
                admin.SetActive(false);
                clubHomePage.SetActive(true);
            }

            else if (clubHome.transform.GetChild(7).gameObject.activeInHierarchy)
            {
                clubHome.transform.GetChild(7).gameObject.SetActive(false);
                clubHome.transform.GetChild(5).gameObject.SetActive(false);
            }

            else if (allPanel.activeInHierarchy)
            {
                clubHome.SetActive(false);
                clubListing.SetActive(true);
            }

            else if (member.transform.GetChild(10).gameObject.activeInHierarchy)
            {
                member.transform.GetChild(10).gameObject.SetActive(false);
                member.transform.GetChild(9).gameObject.SetActive(false);
            }

            else if (member.activeInHierarchy)
            {
                member.SetActive(false);
                clubHomePage.SetActive(true);
            }

            else if(cashier.transform.GetChild(9).gameObject.activeInHierarchy)
            {
                cashier.transform.GetChild(8).gameObject.SetActive(false);
                cashier.transform.GetChild(9).gameObject.SetActive(false);
            }

            else if (cashier.activeInHierarchy)
            {
                cashier.SetActive(false);
                clubHomePage.SetActive(true);
            }

            else if (clubHome.transform.GetChild(9).gameObject.activeInHierarchy)
            {
                clubHome.transform.GetChild(9).gameObject.SetActive(false);
                clubHome.transform.GetChild(1).gameObject.SetActive(true);
            }

            else if (clubHome.transform.GetChild(8).gameObject.activeInHierarchy)
            {
                clubHome.transform.GetChild(8).gameObject.SetActive(false);
            }

            else if (clubHome.transform.GetChild(10).gameObject.activeInHierarchy)
            {
                clubHome.transform.GetChild(10).gameObject.SetActive(false);
                member.SetActive(true);
            }

            else if (clubHome.transform.GetChild(11).gameObject.activeInHierarchy)
            {
                clubHome.transform.GetChild(11).gameObject.SetActive(false);
                clubHome.transform.GetChild(10).gameObject.SetActive(true);
            }

            else if (clubHome.transform.GetChild(12).gameObject.activeInHierarchy)
            {
                clubHome.transform.GetChild(12).gameObject.SetActive(false);
                clubHome.transform.GetChild(10).gameObject.SetActive(true);
            }

            else if (clubHome.transform.GetChild(13).gameObject.activeInHierarchy)
            {
                clubHome.transform.GetChild(13).gameObject.SetActive(false);
                DownlineManagementScript.instance.selectedMemberProfile.SetActive(true);
            }

            else if (clubHome.transform.GetChild(14).GetChild(10).gameObject.activeInHierarchy)
            {
                clubHome.transform.GetChild(14).GetChild(10).gameObject.SetActive(false);
                clubHome.transform.GetChild(14).GetChild(11).gameObject.SetActive(false);
            }

            else if (clubHome.transform.GetChild(14).gameObject.activeInHierarchy)
            {
                clubHome.transform.GetChild(14).gameObject.SetActive(false);
                clubHomePage.SetActive(true);
            }

            else if (clubHome.activeInHierarchy)
            {
                clubHome.SetActive(false);
                clubListing.SetActive(true);
            }
        }
        button.GetComponent<Button>().gameObject.SetActive(true);
    }

    public void All()
    {
        allImage.SetActive(true);
        regularImage.SetActive(false);
        //allPanel.SetActive(true);
        mTTImage.SetActive(false);
        //regularPanel.SetActive(false);
    }

    public void Regular()
    {
        allImage.SetActive(false);
        regularImage.SetActive(true);
        allPanel.SetActive(false);
        mTTImage.SetActive(false);
        //regularPanel.SetActive(true);
    }

    public void MTT()
    {
        allImage.SetActive(false);
        regularImage.SetActive(false);
        allPanel.SetActive(false);
        mTTImage.SetActive(true);
        //regularPanel.SetActive(true);
    }

    public void OpenAdmin()
    {
        admin.SetActive(true);
        member.SetActive(false);

        cashier.SetActive(false);
        menuPanel.SetActive(false);
        clubHomePage.SetActive(false);
        menuFadePanel.SetActive(false);
        clubHome.transform.GetChild(7).gameObject.SetActive(false);
        if (ClubManagement.instance.currentRoleInSelectedClub == 3/* || ClubManagement.instance.currentRoleInSelectedClub == 4*/)   //.....4 For Agent and 3 for Player
        {
            adminAgent.SetActive(false);
            adminPlayer.SetActive(true);
            adminPanel.SetActive(false);
            Admin.instance.ClickAdminDashboardRequest(2);           //1 for today
        }
        else if(ClubManagement.instance.currentRoleInSelectedClub == 4)
        {
            adminPlayer.SetActive(false);
            adminPanel.SetActive(false);
            adminAgent.SetActive(true);
            Admin.instance.ClickAgentDashboardRequest(2);   //1 for today ......   2 for this week
        }
        else
        {
            if (ClubManagement.instance.currentRoleInSelectedClub == 1)    //.......For owner
            {
                adminPlayer.SetActive(false);
                adminPanel.SetActive(true);
                adminAgent.SetActive(false);
                adminPanel.transform.GetChild(6).transform.GetChild(7).gameObject.SetActive(true);
                adminPanel.transform.GetChild(6).transform.GetChild(9).gameObject.SetActive(false);
            }
            else if (ClubManagement.instance.currentRoleInSelectedClub == 2 || ClubManagement.instance.currentRoleInSelectedClub == 5 || ClubManagement.instance.currentRoleInSelectedClub == 6) //......For Manager
            {
                adminPlayer.SetActive(false);
                adminPanel.SetActive(true);
                adminAgent.SetActive(false);
                adminPanel.transform.GetChild(6).transform.GetChild(7).gameObject.SetActive(false);
                adminPanel.transform.GetChild(6).transform.GetChild(9).gameObject.SetActive(true);
            }
            Admin.instance.ClickAdminDashboardRequest(2);          //1 for today

            adminPanel.transform.GetChild(6).gameObject.SetActive(true);
            adminPanel.transform.GetChild(7).gameObject.SetActive(false);
        }
    }

    public void BackAdmin()
    {
        admin.SetActive(false);
        clubHomePage.SetActive(true);
    }

    public void OpenStatistics()
    {
        statistics.SetActive(true);
        adminPanel.SetActive(false);
    }

    public void BackStatistics()
    {
        adminPanel.SetActive(true);
        statistics.SetActive(false);
    }

    public void OpenCashierCanvas()
    {
        member.SetActive(false);

        cashier.SetActive(true);
        menuPanel.SetActive(false);
        clubHomePage.SetActive(false);
        admin.SetActive(false);
    }

    public void OpenMemberCanvas()
    {
        cashier.SetActive(false);

        member.SetActive(true);
        menuPanel.SetActive(false);
        clubHomePage.SetActive(false);
        admin.SetActive(false);
    }

    public void SocialPanelCall()
    {
        socialMediaPanel.SetActive(true);
    }
    public void SocialPanelClose()
    {
        socialMediaPanel.SetActive(false);
    }

    public void OpenJoinClub()
    {
        clubHomePage.SetActive(false);
        joinClubCanvas.SetActive(true);
    }

    public Text totalMemberReqCount, totalChipReqCount;
    public void OpenMenu()
    {
        if (ClubManagement.instance.currentRoleInSelectedClub == 3)
        {
            menuFadePanel.SetActive(true);
            menuFadePanel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(1268.625f, 2400);
            clubHome.transform.GetChild(7).gameObject.SetActive(true);
            menuPanel.SetActive(false);
        }
        else //if (ClubManagement.instance.currentRoleInSelectedClub == 1 || ClubManagement.instance.currentRoleInSelectedClub == 2 || ClubManagement.instance.currentRoleInSelectedClub == 4)
        {
            if (ClubManagement.instance.currentRoleInSelectedClub == 4)     // For Agent
            {
                totalMemberReqCount.text = ClubManagement.instance.memberReqTotalCountForAgent;
                totalChipReqCount.text = ClubManagement.instance.chipReqTotalCountForAgent;
            }
            else                  //for Owner And Manager
            {
                totalMemberReqCount.text = ClubManagement.instance.memberReqTotalCount;
                totalChipReqCount.text = ClubManagement.instance.chipReqTotalCount;
            }

            if (Convert.ToInt32(totalMemberReqCount.text) > 0)
            {
                totalMemberReqCount.transform.parent.gameObject.SetActive(true);
            }
            else
            {
                totalMemberReqCount.transform.parent.gameObject.SetActive(false);
            }

            if (Convert.ToInt32(totalChipReqCount.text) > 0)
            {
                totalChipReqCount.transform.parent.gameObject.SetActive(true);
            }
            else
            {
                totalChipReqCount.transform.parent.gameObject.SetActive(false);
            }

            menuPanel.SetActive(true);
            menuFadePanel.SetActive(true);
            clubHome.transform.GetChild(7).gameObject.SetActive(false);
        }
    }

    public void CloseMenu()
    {
        if (ClubManagement.instance.isPlayer)
        {
            menuFadePanel.SetActive(false);
            menuFadePanel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(1268.625f, 1190);
            clubHome.transform.GetChild(7).gameObject.SetActive(false);
        }
        else
        {
            menuPanel.SetActive(false);
            menuFadePanel.SetActive(false);
        }
    }

    public void OpenDiamondExchangePanel()
    {
        if (ClubManagement.instance.currentRoleInSelectedClub == 5 || ClubManagement.instance.currentRoleInSelectedClub == 6)
        {
            //.....For Accountant and Partner....//
            if (ClubManagement.instance.currentRoleInSelectedClub == 5)
            {
                Cashier.instance.toastMsg.text = "Accountant cannot change this.";
            }
            else
            {
                Cashier.instance.toastMsg.text = "Partner cannot change this.";
            }

            Cashier.instance.toastMsgPanel.SetActive(true);
        }

        else
        {
            clubHome.transform.GetChild(8).gameObject.SetActive(true);
        }
    }


    public InputField diamondInputField;
    public Text chipPanelText;
    public void CloseDiamondExchangePanel()
    {
        clubHome.transform.GetChild(8).gameObject.SetActive(false);
        diamondInputField.text = "";
        chipPanelText.text = "0";
    }

    public void OpenFilterPanel()
    {
        openFilterCount++;
        if (Screen.width < 1500)
        {
            if (openFilterCount % 2 == 0)
            {
                tableText.transform.localPosition = new Vector3(-447.5f, -87, 0);
                addButton.transform.localPosition = new Vector3(510, -88, 0);
                allPanel.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
                regularScrollListPanel.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
                mttScrollListPanel.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);

                filterPanel.SetActive(false);
            }

            else
            {
                tableText.transform.localPosition = new Vector3(-447.5f, -370, 0);
                addButton.transform.localPosition = new Vector3(510, -370, 0);
                allPanel.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0, -250);
                regularScrollListPanel.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0, -250);
                mttScrollListPanel.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0, -250);
                filterPanel.SetActive(true);
            }
        }
        else
        {
            if (openFilterCount % 2 == 0)
            {
                tableText.transform.localPosition = new Vector3(-447.5f, -87, 0);
                addButton.transform.localPosition = new Vector3(510, -88, 0);
                allPanel.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
                regularScrollListPanel.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
                mttScrollListPanel.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);

                filterPanel.SetActive(false);
            }

            else
            {
                tableText.transform.localPosition = new Vector3(-447.5f, -290, 0);
                addButton.transform.localPosition = new Vector3(510, -290, 0);
                allPanel.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0, -250);
                regularScrollListPanel.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0, -250);
                mttScrollListPanel.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0, -250);
                filterPanel.SetActive(true);
            }
        }
    }
}