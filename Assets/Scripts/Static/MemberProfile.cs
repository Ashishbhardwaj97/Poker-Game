using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemberProfile : MonoBehaviour
{
    [Header("GameObject Reference")]
    public GameObject memberProfileUI;
    public GameObject membersUI;
    public GameObject memberProfileContent;
    public GameObject changeRolePanel;
    public GameObject changeRoleText;
    public GameObject roleScrollView;
    public GameObject confirmationPanel;
    public GameObject confirmationPanelForRemoveMember;
    public GameObject agentListPanel;
    public GameObject agentListContent;
    public GameObject confirmPanelForUplineAgent;   
    public GameObject memberButtonsPanel;
    public GameObject changeRolePanelForManager;
    public List<GameObject> uplineAgent;
    [Space]
    public GameObject reAssignDownlineMemberPanel;
    public GameObject reAssignAgentContent;
    public GameObject reAssignAgentPanel;
    public GameObject reAssignRoleScrollView;
    public GameObject reAssignDropDownPanel;

    [Header("Transform Reference")]
    public Transform detailPanel;

    [Header("Image Reference")]
    public Image countryFlag;

    [Header("Text Reference")]
    public Text claimCountText;
    public Text sendCountText;
    public Text totalFeesText;
    public Text totalWinText;
    public Text totalGamesText;
    public Text agentCreditChipsText;
    public Text agentDownlineChipsText;
    public Text agentTotalFeesText;
    public Text agentTotalWinText;


    //private string changeRoleUrl;
    //private string deleteClubMemberUrl;
    //private string assignMemberUrl;
    //private string memberProfileUrl;
    //private string agentListUrl;
    //private string memberSummeryUrl;
    //private string agentDashBoardUrl;
    //private string downlineMemberListUrl;

    private string uplineAgentName;
    private string changedRole;
    private string role = "";
    internal string currentUserID;

    private int roleScrollViewCount = 0;    
    private int reAssignRoleScrollViewCount = 0;
    internal int agentCreditBal;

    private int agentListCount;
    private int reAssignAgentListCount;

    private GameObject currentSelectedMember;
    internal GameObject selectedAgentObj;
    private GameObject scrollItemObj;

    

    [Serializable]
    class UpdateRole
    {
        public string old_role;
        public string user_id;
        public string club_id;
        public string new_role;
        public string agent_id;
        public string client_id;
    }

    [Serializable]
    class RemovePlayer
    {        
        public string user_id;
        public string club_id;
        public string club_member_role;
        public string agent_id;       
        public int type;       
    }

    [Serializable]
    public class UpdateRoleResponse
    {
        public bool error;
    }

    private void OnEnable()
    {
        //changeRoleUrl = ServerChanger.instance.domainURL + "api/v1/club/update-role";
        //deleteClubMemberUrl = ServerChanger.instance.domainURL + "api/v1/club/delete-club-member";
        //agentListUrl = ServerChanger.instance.domainURL + "api/v1/club/agent-list";
        //assignMemberUrl = ServerChanger.instance.domainURL + "api/v1/club/assign-member";
        //memberProfileUrl = ServerChanger.instance.domainURL + "api/v1/club/member-profile";
        //memberSummeryUrl = ServerChanger.instance.domainURL + "api/v1/club/member-summary";
        //downlineMemberListUrl = ServerChanger.instance.domainURL + "api/v1/club/downline-members-list";
        //agentDashBoardUrl = ServerChanger.instance.domainURL + "api/v1/club/agent-dashboard-summary";
        agentListCount = 1;
        reAssignAgentListCount = 1;
    }

    public void ChangeRolePanelForMangerModification(bool isOn)
    {
        if (isOn)   //........Move up........//
        {
            changeRolePanel.SetActive(false);
            changeRolePanelForManager.SetActive(true);

            memberButtonsPanel.transform.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.13f);
            memberButtonsPanel.transform.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.13f);
        }
        else
        {
            changeRolePanel.SetActive(true);
            changeRolePanelForManager.SetActive(false);

            memberButtonsPanel.transform.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.09079735f);
            memberButtonsPanel.transform.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.09079735f);
        }
    }

    public void WithoutChangeRolePanel(bool isOn)
    {
        if (isOn)   //........Move up........//
        {
            memberProfileUI.transform.GetChild(0).GetComponent<ScrollRect>().enabled = false;
            changeRoleText.SetActive(false);
            changeRolePanel.SetActive(false);

            memberButtonsPanel.transform.GetChild(4).gameObject.SetActive(false);
            memberButtonsPanel.transform.GetChild(5).gameObject.SetActive(false);

            memberButtonsPanel.transform.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.26f);
            memberButtonsPanel.transform.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.26f);
        }
        else
        {
            memberProfileUI.transform.GetChild(0).GetComponent<ScrollRect>().enabled = true;
            changeRoleText.SetActive(true);
            changeRolePanel.SetActive(true);

            memberButtonsPanel.transform.GetChild(4).gameObject.SetActive(true);
            memberButtonsPanel.transform.GetChild(5).gameObject.SetActive(true);

            memberButtonsPanel.transform.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.09079735f);
            memberButtonsPanel.transform.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.09079735f);
        }
    }


    #region Click on Member Profile

    public void ClickOnMemberProfile(Transform panel)
    {
        print("ClickOnMemberProfile.......");
        ClubManagement.instance.currentSelectedMemberProfile = panel.gameObject;
        WithoutChangeRolePanel(false);
        ChangeRolePanelForMangerModification(false);

        roleScrollView.SetActive(false);
        roleScrollViewCount = 0;

        detailPanel.GetChild(0).GetComponent<Image>().sprite = panel.GetChild(7).GetComponent<Image>().sprite;
        detailPanel.GetChild(1).GetComponent<Text>().text = panel.GetChild(1).GetComponent<Text>().text + "      ";                //userName
        detailPanel.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = panel.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text;  //Role in single char
        detailPanel.GetChild(2).GetComponent<Text>().text = panel.GetChild(0).GetComponent<Text>().text;                            // first-last name 
        detailPanel.GetChild(3).GetComponent<Text>().text = panel.GetChild(2).GetComponent<Text>().text;           // client id
        detailPanel.GetChild(4).GetComponent<Text>().text = panel.GetChild(8).GetComponent<Text>().text + ", " + panel.GetChild(9).GetComponent<Text>().text;
        detailPanel.GetChild(5).GetChild(1).GetComponent<Text>().text = panel.GetChild(3).GetChild(1).GetComponent<Text>().text;        // chips
        detailPanel.GetChild(12).GetComponent<Text>().text = panel.GetChild(14).GetComponent<Text>().text;        // agent chips
        detailPanel.GetChild(6).GetChild(0).GetComponent<Text>().text = panel.GetChild(4).GetChild(0).GetComponent<Text>().text;      // role
        detailPanel.GetChild(8).GetComponent<Text>().text = panel.GetChild(13).GetComponent<Text>().text;                               // agent id


        currentUserID = panel.GetChild(10).GetComponent<Text>().text;

        role = panel.GetChild(12).GetComponent<Text>().text;

        for (int i = 0; i < changeRolePanel.transform.childCount; i++)
        {
            changeRolePanel.transform.GetChild(i).GetChild(4).GetComponent<Button>().enabled = true;
        }
        memberButtonsPanel.transform.GetChild(4).GetComponent<Button>().enabled = true;
        DownlineManagementScript.instance.assignDownlineMember.transform.GetChild(1).GetChild(0).GetComponent<Button>().interactable = true;
        DownlineManagementScript.instance.creditTransferPanel.transform.GetChild(7).GetChild(0).GetComponent<Button>().interactable = true;
        DownlineManagementScript.instance.creditTransferPanel.transform.GetChild(7).GetChild(1).GetComponent<Button>().interactable = true;

        if (ClubManagement.instance.currentSelectedClubRole == "Owner")
        {
            if (role == "Owner")
            {
                EnableDisableUplineAgent(false);

                WithoutChangeRolePanel(true);
            }
            else if (role == "Manager")
            {
                EnableDisableUplineAgent(false);
            }
            else if (role == "Agent")
            {
                EnableDisableUplineAgent(false);
                memberButtonsPanel.transform.GetChild(6).gameObject.SetActive(true);
                memberButtonsPanel.transform.GetChild(7).gameObject.SetActive(true);
                memberButtonsPanel.transform.GetChild(8).gameObject.SetActive(true);
                memberButtonsPanel.transform.GetChild(9).gameObject.SetActive(true);

                ClickOnAgentDashBoard(1);       //.........For Agent Dashboad..........//
            }
            else if (role == "Partner")
            {
                EnableDisableUplineAgent(false);
            }
            else if (role == "Accountant")
            {
                EnableDisableUplineAgent(false);
            }
            else if (role == "Player")
            {
                EnableDisableUplineAgent(true);
                MemberProfileReq();  //.......... API Request for member profile for upline agent.........//
            }

        }

        if (ClubManagement.instance.currentSelectedClubRole == "Partner" || ClubManagement.instance.currentSelectedClubRole == "Accountant")
        {
            if (role == "Owner")
            {
                EnableDisableUplineAgent(false);

                WithoutChangeRolePanel(true);
            }
            else if (role == "Manager")
            {
                EnableDisableUplineAgent(false);
            }
            else if (role == "Agent")
            {
                EnableDisableUplineAgent(false);
                memberButtonsPanel.transform.GetChild(6).gameObject.SetActive(true);
                memberButtonsPanel.transform.GetChild(7).gameObject.SetActive(true);
                memberButtonsPanel.transform.GetChild(8).gameObject.SetActive(true);
                memberButtonsPanel.transform.GetChild(9).gameObject.SetActive(true);

                ClickOnAgentDashBoard(1);       //.........For Agent Dashboad..........//
            }
            else if (role == "Partner")
            {
                EnableDisableUplineAgent(false);
            }
            else if (role == "Accountant")
            {
                EnableDisableUplineAgent(false);
            }
            else if (role == "Player")
            {
                EnableDisableUplineAgent(true);
                MemberProfileReq();  //.......... API Request for member profile for upline agent.........//

                uplineAgent[1].transform.GetComponent<Button>().interactable = false;
            }

            for (int i = 0; i < changeRolePanel.transform.childCount; i++)
            {
                changeRolePanel.transform.GetChild(i).GetChild(4).GetComponent<Button>().enabled = false;
            }
            memberButtonsPanel.transform.GetChild(4).GetComponent<Button>().enabled = false;
            DownlineManagementScript.instance.assignDownlineMember.transform.GetChild(1).GetChild(0).GetComponent<Button>().interactable = false;
            DownlineManagementScript.instance.creditTransferPanel.transform.GetChild(7).GetChild(0).GetComponent<Button>().interactable = false;
            DownlineManagementScript.instance.creditTransferPanel.transform.GetChild(7).GetChild(1).GetComponent<Button>().interactable = false;
        }

        else if (ClubManagement.instance.currentSelectedClubRole == "Manager")
        {

            if (role == "Owner")
            {
                EnableDisableUplineAgent(false);
                WithoutChangeRolePanel(true);
            }
            else if (role == "Manager")
            {
                EnableDisableUplineAgent(false);
                WithoutChangeRolePanel(true);
            }
            else if (role == "Agent")
            {
                EnableDisableUplineAgent(false);
                ChangeRolePanelForMangerModification(true);

                memberButtonsPanel.transform.GetChild(6).gameObject.SetActive(true);
                memberButtonsPanel.transform.GetChild(7).gameObject.SetActive(true);
                memberButtonsPanel.transform.GetChild(8).gameObject.SetActive(true);
                memberButtonsPanel.transform.GetChild(9).gameObject.SetActive(true);

                memberButtonsPanel.transform.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.121f);
                memberButtonsPanel.transform.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.121f);

                ClickOnAgentDashBoard(1);       //.........For Agent Dashboad..........//
            }
            else if (role == "Partner")
            {
                EnableDisableUplineAgent(false);
                WithoutChangeRolePanel(true);
            }
            else if (role == "Accountant")
            {
                EnableDisableUplineAgent(false);
                WithoutChangeRolePanel(true);
            }
            else if (role == "Player")
            {
                EnableDisableUplineAgent(true);
                MemberProfileReq();  //.......... API Request for member profile for upline agent.........//
                ChangeRolePanelForMangerModification(true);
            }

        }

        else if (ClubManagement.instance.currentSelectedClubRole == "Agent")
        {
            WithoutChangeRolePanel(true);

            if (role == "Agent")
            {
                EnableDisableUplineAgent(false);
                memberProfileUI.transform.GetChild(0).GetComponent<ScrollRect>().enabled = true;

                memberButtonsPanel.transform.GetChild(6).gameObject.SetActive(false);
                memberButtonsPanel.transform.GetChild(7).gameObject.SetActive(false);
                memberButtonsPanel.transform.GetChild(8).gameObject.SetActive(false);
                memberButtonsPanel.transform.GetChild(9).gameObject.SetActive(false);

                memberButtonsPanel.transform.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.20f);
                memberButtonsPanel.transform.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.20f);

                ClickOnAgentDashBoard(1);       //.........For Agent Dashboad..........//
            }
            else if (role == "Player")
            {
                EnableDisableUplineAgent(true);
                MemberProfileReq();  //.......... API Request for member profile for upline agent.........//

                memberButtonsPanel.transform.GetChild(4).gameObject.SetActive(true);
                memberButtonsPanel.transform.GetChild(5).gameObject.SetActive(true);
            }

        }

        //.....For default selection.....//
        if (role != "Owner")        
        {
            if (role == "Player")
            {
                string str = role + "_";
                ChangeRole(GameObject.FindGameObjectWithTag(str));
            }
            else
            {
                ChangeRole(GameObject.FindGameObjectWithTag(role));
            }

            currentSelectedMember = panel.gameObject;
        }
        //..............................//

        //...........Country flag..........//

        string flagUrl = panel.GetChild(11).GetComponent<Text>().text;

        if (!string.IsNullOrEmpty(flagUrl))
        {
            ClubManagement.instance.loadingPanel.SetActive(true);
            Communication.instance.GetImage(flagUrl, ContryFlagProcess);
        }

        //.................................//

        ClickOnMemberSummery(1);        //.........For all summery..........//
        
    }
    void ContryFlagProcess(Sprite response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (response != null)
        {
            countryFlag.sprite = response;
        }
    }

    #region Member Profile API

    [Serializable]
    class ReqForMemberProfile
    {
        public string club_id;
        public string user_id;
    }

    [Serializable]
    class ResponseForMemberProfile
    {
        public bool error;
        public MemberProfileData[] memberProfileData;
    }

    [Serializable]
    class MemberProfileData
    {
        public string upline_agent_name;
        public int agent_credit_balance;
    }
    
    [Header("Properties")]
    public List<GameObject> agentObjList;
    public List<GameObject> reAssignAgentObjList;
    [SerializeField] ReqForMemberProfile reqForMemberProfile;
    [SerializeField] ResponseForMemberProfile responseForMemberProfile;
    public void MemberProfileReq()
    {
        reqForMemberProfile.club_id = ClubManagement.instance._clubID;
        reqForMemberProfile.user_id = currentUserID;

        string body = JsonUtility.ToJson(reqForMemberProfile);
        print(body);

        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(memberProfileUrl, body, MemberProfileReqProcess);
    }
    
    void MemberProfileReqProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("Some error ...!");
        }
        else
        {
            print("response" + response);
            responseForMemberProfile = JsonUtility.FromJson<ResponseForMemberProfile>(response);

            if (!responseForMemberProfile.error)
            {
                uplineAgentName = responseForMemberProfile.memberProfileData[0].upline_agent_name;
                agentCreditBal = responseForMemberProfile.memberProfileData[0].agent_credit_balance;
                print("uplineAgentName......." + uplineAgentName);

                DownlineManagementScript.instance.creditTransferPanel.transform.GetChild(2).GetChild(3).GetChild(1).GetComponent<Text>().text = agentCreditBal.ToString();


                if (!string.IsNullOrEmpty(uplineAgentName))
                {
                    uplineAgent[1].transform.GetChild(0).GetComponent<Text>().text = uplineAgentName;
                }
                else
                {
                    uplineAgent[1].transform.GetChild(0).GetComponent<Text>().text = "Select";
                }

            }

        }
    }

    #endregion

    void EnableDisableUplineAgent(bool _isEnable)
    {
        if (_isEnable)
        {
            for (int i = 0; i < uplineAgent.Count; i++)
            {
                uplineAgent[i].SetActive(true);
            }
            uplineAgent[1].transform.GetComponent<Button>().interactable = true;
        }
        else
        {
            for (int i = 0; i < uplineAgent.Count; i++)
            {
                uplineAgent[i].SetActive(false);
            }
        }
    }

    public void ClickOnBackButtonInMemberProfile()
    {
        membersUI.SetActive(true);
        memberProfileUI.SetActive(false);
        ClubManagement.instance.groupByRoleObj.transform.GetChild(0).gameObject.SetActive(false);

        ClubManagement.instance.ClickOnMembersList();
        ResetAgentListItems();
    }


    #endregion

    #region Click on change Role

    public void ChangeRole(GameObject obj)
    {
        for (int i = 0; i < changeRolePanel.transform.childCount; i++)
        {
            changeRolePanel.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(false);
            changeRolePanel.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);
            changeRolePanel.transform.GetChild(i).GetChild(3).gameObject.SetActive(false);
            changeRolePanel.transform.GetChild(i).GetChild(1).GetComponent<Text>().color = new Color32(156, 165, 180, 255);
        }

        for (int i = 0; i < changeRolePanelForManager.transform.childCount; i++)
        {
            changeRolePanelForManager.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(false);
            changeRolePanelForManager.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);
            changeRolePanelForManager.transform.GetChild(i).GetChild(3).gameObject.SetActive(false);
            changeRolePanelForManager.transform.GetChild(i).GetChild(1).GetComponent<Text>().color = new Color32(156, 165, 180, 255);
        }

        if (obj != null)
        {
            obj.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            obj.transform.GetChild(2).gameObject.SetActive(false);
            obj.transform.GetChild(3).gameObject.SetActive(true);
            obj.transform.GetChild(1).GetComponent<Text>().color = Color.white;

            changedRole = obj.name;
        }
    }

    public void OpenRoleScrollView()
    {
        roleScrollViewCount++;
        if (roleScrollViewCount % 2 == 0)
        {
            roleScrollView.SetActive(false);
        }
        else
        {
            roleScrollView.SetActive(true);
            ClickOnUplineAgent();
        }        
    }

    public void SelectRole(GameObject obj)
    {
        for (int i = 0; i < agentListContent.transform.childCount; i++)
        {
            agentListContent.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }

        obj.transform.GetChild(0).gameObject.SetActive(true);        
        uplineAgent[1].transform.GetChild(0).GetComponent<Text>().text = obj.transform.GetChild(1).GetComponent<Text>().text;
        roleScrollViewCount = 0;

        confirmPanelForUplineAgent.SetActive(true);
        selectedAgentObj = obj;

    }

    [SerializeField] UpdateRole updateRole;
    [SerializeField] UpdateRoleResponse updateRoleResponse;

    public void ChangeRoleConfirmation()
    {
        updateRole.old_role = role;
        updateRole.user_id = currentUserID;
        updateRole.club_id = ClubManagement.instance._clubID;        
        updateRole.new_role = changedRole;
        updateRole.agent_id = detailPanel.GetChild(8).GetComponent<Text>().text;
        updateRole.client_id = detailPanel.GetChild(3).GetComponent<Text>().text;

        string body = JsonUtility.ToJson(updateRole);
        print(body);
        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(changeRoleUrl, body, ChangeRoleProcess);
    }

    void ChangeRoleProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("Some error ...!");
        }
        else
        {
            print("response" + response);
            updateRoleResponse = JsonUtility.FromJson<UpdateRoleResponse>(response);

            if (!updateRoleResponse.error)
            {
                confirmationPanel.SetActive(false);
                
                detailPanel.GetChild(6).GetChild(0).GetComponent<Text>().text = changedRole;
                detailPanel.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = changedRole;

                string _role = changedRole;
               
                role = changedRole;
                ClubManagement.instance.currentSelectedMemberProfile.transform.GetChild(12).GetComponent<Text>().text= changedRole;
                if (changedRole == "Player")
                {
                    EnableDisableUplineAgent(true);

                    ClubManagement.instance.memberProfile.SetActive(true);
                    ClubManagement.instance.memberProfileForAgent.SetActive(false);
                    ClubManagement.instance.memberProfile.GetComponent<MemberProfile>().ClickOnMemberProfile(ClubManagement.instance.currentSelectedMemberProfile.transform);
                    //ClubManagement.instance.memberProfile.GetComponent<MemberProfile>().detailPanel.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = changedRole;
                    ClubManagement.instance.memberProfile.GetComponent<MemberProfile>().detailPanel.GetChild(8).GetComponent<Text>().text = string.Empty;


                }
                else if(changedRole == "Agent")
                {
                    EnableDisableUplineAgent(false);

                    ClubManagement.instance.memberProfile.SetActive(false);
                    ClubManagement.instance.memberProfileForAgent.SetActive(true);
                    ClubManagement.instance.memberProfileForAgent.GetComponent<MemberProfile>().ClickOnMemberProfile(ClubManagement.instance.currentSelectedMemberProfile.transform);
                   // ClubManagement.instance.memberProfileForAgent.GetComponent<MemberProfile>().detailPanel.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = changedRole;
                    string _agentID = ClubManagement.instance.currentSelectedMemberProfile.transform.GetChild(2).GetComponent<Text>().text;
                    ClubManagement.instance.memberProfileForAgent.GetComponent<MemberProfile>().detailPanel.GetChild(8).GetComponent<Text>().text = _agentID;
                }
                else
                {
                    EnableDisableUplineAgent(false);

                    ClubManagement.instance.memberProfile.SetActive(true);
                    ClubManagement.instance.memberProfileForAgent.SetActive(false);                    
                    ClubManagement.instance.memberProfile.GetComponent<MemberProfile>().ClickOnMemberProfile(ClubManagement.instance.currentSelectedMemberProfile.transform);
                    //ClubManagement.instance.memberProfile.GetComponent<MemberProfile>().detailPanel.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = changedRole;
                    ClubManagement.instance.memberProfile.GetComponent<MemberProfile>().detailPanel.GetChild(8).GetComponent<Text>().text = string.Empty;

                }

                if (changedRole == "Accountant")
                {
                    ClubManagement.instance.memberProfile.GetComponent<MemberProfile>().detailPanel.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = _role.Substring(0, 2);
                }
                else if (changedRole == "Partner")
                {
                    ClubManagement.instance.memberProfile.GetComponent<MemberProfile>().detailPanel.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = _role.Substring(0, 2);
                }
                else if (changedRole == "Agent")
                {
                    ClubManagement.instance.memberProfileForAgent.GetComponent<MemberProfile>().detailPanel.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = _role.Substring(0, 1);
                }
                else
                {
                    ClubManagement.instance.memberProfile.GetComponent<MemberProfile>().detailPanel.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = _role.Substring(0, 1);
                }
            }
            else
            {
                print("Some error ...!");
            }
            //.............MailBox...............//
            MailBoxScripts._instance.MailBoxCallCount();
            //.......................................//
        }
    }

    public void CancelButton()
    {
        if (role == "Player")
        {
            string str = role + "_";
            ChangeRole(GameObject.FindGameObjectWithTag(str));
        }
        else
        {
            ChangeRole(GameObject.FindGameObjectWithTag(role));
        }

        confirmationPanel.SetActive(false);
    }

    #endregion

    #region Remove Player From Member Profile

    [SerializeField] RemovePlayer removePlayer;
    public void ClickOnRemoveMember()
    {
        confirmationPanelForRemoveMember.SetActive(true);
    }
    public void RemoveMemberConfirmation()
    {
        removePlayer.club_id = ClubManagement.instance._clubID;
        removePlayer.user_id = currentUserID;
        removePlayer.club_member_role = changedRole;
        removePlayer.agent_id = detailPanel.GetChild(8).GetComponent<Text>().text;
        removePlayer.type = 2;
        string body = JsonUtility.ToJson(removePlayer);
        print(body);
        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(deleteClubMemberUrl, body, RemoveMemberProcess);
    }

    void RemoveMemberProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("Some error ...!");
        }
        else
        {
            print("response" + response);
            updateRoleResponse = JsonUtility.FromJson<UpdateRoleResponse>(response);

            if (!updateRoleResponse.error)
            {
                confirmationPanelForRemoveMember.SetActive(false);
                currentSelectedMember.transform.SetParent(gameObject.transform);
                currentSelectedMember.transform.SetParent(ClubManagement.instance.memberScrollContent.transform);
                currentSelectedMember.SetActive(false);
                ClickOnBackButtonInMemberProfile();
            }
            else
            {
                print("Some error ...!");
            }
            //.............MailBox...............//
            MailBoxScripts._instance.MailBoxCallCount();
            //.......................................//
        }
    }

    #endregion

    #region Agent List and Assign Agent

    [Serializable]
    class AgentList
    {
        public string club_id;
        public string agent_id;
    }

    [Serializable]
    class AgentListResponse
    {
        public bool error;
        public AgentListdata[] agentListdata;
    }

    [Serializable]
    class AgentListdata
    {
        public string agent_id;
        public string username;
        public string user_id;
        public string first_name;
        public string last_name;
    }

    [SerializeField] AgentList agentList;
    [SerializeField] AgentListResponse agentListResponse;

    public void ClickOnUplineAgent()
    {
        agentList.club_id = ClubManagement.instance._clubID;

        string body = JsonUtility.ToJson(agentList);
        print(body);

        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(agentListUrl, body, AgentListProcess);
    }

    void AgentListProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("Some error ...!");
        }
        else
        {
            print("response" + response);

            agentListResponse = JsonUtility.FromJson<AgentListResponse>(response);

            if (!agentListResponse.error)
            {
                print(agentListResponse.agentListdata.Length);

                if (agentListResponse.agentListdata.Length != agentListCount)
                {
                    for (int i = agentListCount; i < agentListResponse.agentListdata.Length; i++)
                    {
                        agentListCount++;
                        GenerateAgentListItems();
                    }

                }

                for (int i = 0; i < agentListResponse.agentListdata.Length; i++)
                {
                    agentListContent.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = agentListResponse.agentListdata[i].username;
                    agentListContent.transform.GetChild(i).GetChild(2).GetComponent<Text>().text = agentListResponse.agentListdata[i].agent_id;
                    agentListContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text = agentListResponse.agentListdata[i].user_id;

                    agentListContent.transform.GetChild(i).gameObject.SetActive(true);

                }

                for (int i = 0; i < agentListContent.transform.childCount; i++)
                {
                    agentListContent.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);

                    if (!string.IsNullOrEmpty(uplineAgentName))
                    {
                        if (agentListContent.transform.GetChild(i).GetChild(1).GetComponent<Text>().text == uplineAgentName)
                        {
                            agentListContent.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
                        }
                    }

                }

            }
            else
            {
                agentListContent.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = string.Empty;
                agentListContent.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = string.Empty;
                agentListContent.transform.GetChild(0).GetChild(3).GetComponent<Text>().text = string.Empty;

                agentListContent.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    void GenerateAgentListItems()
    {
        scrollItemObj = Instantiate(agentListPanel);
        scrollItemObj.transform.SetParent(agentListContent.transform, false);
        agentObjList.Add(scrollItemObj);
    }

    public void ResetAgentListItems()
    {
        agentListCount = 1;

        if (agentObjList.Count > 0)
        {
            for (int i = 0; i < agentObjList.Count; i++)
            {
                Destroy(agentObjList[i]);
            }

            agentObjList.Clear();
        }
    }

    [Serializable]
    class UplineMembers
    {
        public List<AssignMembers> assignMembers;
    }

    [Serializable]
    class AssignMembers
    {
        public string user_id;
        public string club_id;
        public string agent_id;
    }

    [SerializeField] UplineMembers uplineMembers;

    public void ClickOnConfirmUplineAgent()
    {
        uplineMembers.assignMembers.Clear();
        uplineMembers.assignMembers.Add(new AssignMembers());

        uplineMembers.assignMembers[0].user_id = currentUserID;
        uplineMembers.assignMembers[0].club_id = ClubManagement.instance._clubID;
        uplineMembers.assignMembers[0].agent_id = selectedAgentObj.transform.GetChild(2).GetComponent<Text>().text;

        string body = JsonUtility.ToJson(uplineMembers);

        print(body);

        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(assignMemberUrl, body, AssignMemberProcess);

    }

    [SerializeField] UpdateRoleResponse updateAssignMember;
    void AssignMemberProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("Some error ...!");
        }
        else
        {
            print("response" + response);

            updateAssignMember = JsonUtility.FromJson<UpdateRoleResponse>(response);

            if (!updateAssignMember.error)
            {
                confirmPanelForUplineAgent.SetActive(false);
                roleScrollView.SetActive(false);

                uplineAgentName = selectedAgentObj.transform.GetChild(1).GetComponent<Text>().text;

                ResetAgentListItems();
            }
        }
    }

    #endregion

    #region Member Summery

    [Serializable]
    class MemberSummeryRequest
    {
        public string user_id;
        public string club_id;
        public int type;
        public string start_date;
        public string end_date;

    }

    [Serializable]
    class MemberSummeryResponse
    {
        public bool error;
        public int claimCount;
        public string sendCount;
        public int totalFees;
        public int totalWin;
        public int totalGames;
    }

    [SerializeField] MemberSummeryRequest memberSummeryRequest;
    [SerializeField] MemberSummeryResponse memberSummeryResponse;

    public void ClickOnMemberSummery(int _type)
    {
        memberSummeryRequest.user_id = currentUserID;
        memberSummeryRequest.club_id = ClubManagement.instance._clubID;
        memberSummeryRequest.type = _type;

        if (_type == 3)
        {
            memberSummeryRequest.start_date = "10:09:2020";
            memberSummeryRequest.end_date = "10:09:2020";
        }
        else
        {
            memberSummeryRequest.start_date = "";
            memberSummeryRequest.end_date = "";
        }

        string body = JsonUtility.ToJson(memberSummeryRequest);
        print(body);

        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(memberSummeryUrl, body, MemberSummeryProcess);
    }

    void MemberSummeryProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("Some error ...!");
        }
        else
        {
            print("response" + response);
            memberSummeryResponse = JsonUtility.FromJson<MemberSummeryResponse>(response);

            if (!memberSummeryResponse.error)
            {
                claimCountText.text = memberSummeryResponse.claimCount.ToString();
                sendCountText.text = memberSummeryResponse.sendCount;
                totalFeesText.text = memberSummeryResponse.totalFees.ToString();
                totalWinText.text = memberSummeryResponse.totalWin.ToString();
                totalGamesText.text = memberSummeryResponse.totalGames.ToString();
            }
        }
    }

    #endregion

    #region Agent DashBoard 

    [Serializable]
    class AgentDashBoardRequest
    {
        public string user_id;
        public string club_id;
        public string agent_id;
        public int type;
        public string start_date;
        public string end_date;

    }

    [Serializable]
    class AgentDashBoardResponse
    {
        public bool error;
        public int agentCreditChips;
        public string downlineChips;
        public int totalFees;
        public int totalWin;
        //public int totalGames;
    }

    
    [SerializeField] AgentDashBoardRequest agentDashBoardRequest;
    [SerializeField] AgentDashBoardResponse agentDashBoardResponse;

    public void ClickOnAgentDashBoard(int _type)
    {
        agentDashBoardRequest.user_id = currentUserID;
        agentDashBoardRequest.club_id = ClubManagement.instance._clubID;
        agentDashBoardRequest.agent_id = detailPanel.GetChild(8).GetComponent<Text>().text;
        agentDashBoardRequest.type = _type;

        if (_type == 3)
        {
            agentDashBoardRequest.start_date = "10:09:2020";
            agentDashBoardRequest.end_date = "10:09:2020";
        }
        else
        {
            agentDashBoardRequest.start_date = "";
            agentDashBoardRequest.end_date = "";
        }

        string body = JsonUtility.ToJson(agentDashBoardRequest);
        print(body);

        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(agentDashBoardUrl, body, AgentDashBoardProcess);
    }

    void AgentDashBoardProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("Some error ...!");
        }
        else
        {
            print("response" + response);
            agentDashBoardResponse = JsonUtility.FromJson<AgentDashBoardResponse>(response);

            if (!agentDashBoardResponse.error)
            {
                agentCreditChipsText.text = agentDashBoardResponse.agentCreditChips.ToString();
                agentDownlineChipsText.text = agentDashBoardResponse.downlineChips;
                agentTotalFeesText.text = agentDashBoardResponse.totalFees.ToString();
                agentTotalWinText.text = agentDashBoardResponse.totalWin.ToString();
                
            }
        }
    }

    #endregion

    #region Check Agent downline members

    internal int downlineMemberCount;
    internal string currentAgentIdWhenRoleChanged;
    [Serializable]
    class DownlineMemberRequest
    {
        public string club_id;
        public string agent_id;
    }

    [Serializable]
    class DownlineMemberResponse
    {
        public bool error;
        public DownlineMembers[] downlineMembers;
    }

    [Serializable]
    class DownlineMembers
    {
        public string user_id;
    }

    [SerializeField] DownlineMemberRequest downlineMemberRequest;
    [SerializeField] DownlineMemberResponse downlineMemberResponse;

    public List<string> downlineUserId;
    public void FindDownlineMembers()
    {
        ReAssignUplineAgent();

        downlineMemberRequest.club_id = ClubManagement.instance._clubID;
        downlineMemberRequest.agent_id = detailPanel.GetChild(8).GetComponent<Text>().text;

        string body = JsonUtility.ToJson(downlineMemberRequest);
        print(body);
        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(downlineMemberListUrl, body, FindDownlineMembersProcess);

    }

    void FindDownlineMembersProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("error");
        }
        else
        {
            print(response);

            downlineMemberResponse = JsonUtility.FromJson<DownlineMemberResponse>(response);
            if (!downlineMemberResponse.error)
            {
                if (downlineMemberResponse.downlineMembers.Length > 0)
                {
                    downlineMemberCount = downlineMemberResponse.downlineMembers.Length;

                    if (downlineMemberCount > 0 && agentListResponse.agentListdata.Length > 0)
                    {
                        reAssignDownlineMemberPanel.SetActive(true);
                        reAssignRoleScrollView.SetActive(false);
                        for (int i = 0; i < downlineMemberResponse.downlineMembers.Length; i++)
                        {
                            downlineUserId.Add(downlineMemberResponse.downlineMembers[i].user_id);
                        }


                    }
                    else
                    {
                        //print("downlineMemberCount Zero hai ....");

                        if (confirmationPanel.activeInHierarchy)
                        {
                            ChangeRoleConfirmation();

                        }
                        else if (confirmationPanelForRemoveMember.activeInHierarchy)
                        {
                            RemoveMemberConfirmation();
                        }
                    }
                }

            }

            else
            {
                if (confirmationPanel.activeInHierarchy)
                {
                    ChangeRoleConfirmation();

                }
                else if (confirmationPanelForRemoveMember.activeInHierarchy)
                {
                    RemoveMemberConfirmation();
                }
            }
        }
    }

    [Serializable]
    class AssignMembersList
    {
        public List<AssignMembers> assignMembers;
    }

    [SerializeField] AssignMembersList uplineMembersList;

    public void ConfirmAssignMembers()
    {
        uplineMembersList.assignMembers.Clear();
        

        for (int i = 0; i < downlineMemberCount; i++)
        {
            uplineMembersList.assignMembers.Add(new AssignMembers());
            uplineMembersList.assignMembers[i].user_id = downlineUserId[i];
            uplineMembersList.assignMembers[i].club_id = ClubManagement.instance._clubID;
            if (selectedAgentObj != null)
            {
                uplineMembersList.assignMembers[i].agent_id = selectedAgentObj.transform.GetChild(2).GetComponent<Text>().text;
            }
            else
            {
                uplineMembersList.assignMembers[i].agent_id = "";
            }
           
        }


        string body = JsonUtility.ToJson(uplineMembersList);

        print(body);

        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(assignMemberUrl, body, ReAssignMemberProcess);
    }

    void ReAssignMemberProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("Some error ...!");
        }
        else
        {
            print("response" + response);

            updateAssignMember = JsonUtility.FromJson<UpdateRoleResponse>(response);

            if (!updateAssignMember.error)
            {                
                uplineAgentName = selectedAgentObj.transform.GetChild(1).GetComponent<Text>().text;

                if (confirmationPanel.activeInHierarchy)
                {
                    ChangeRoleConfirmation();
                    
                }
                else if (confirmationPanelForRemoveMember.activeInHierarchy)
                {
                    RemoveMemberConfirmation();
                }
                confirmationPanel.SetActive(false);
                confirmationPanelForRemoveMember.SetActive(false);
                reAssignDownlineMemberPanel.SetActive(false);
                ResetAgentListItemsForReAssignAgent();
            }
        }

    }

    #endregion

    #region Re-Assign Agent for Downline 

    [SerializeField] AgentList reAssignAgentList;
    public void ReAssignAgentOpenRoleScrollView()
    {
        reAssignRoleScrollViewCount++;
        if (reAssignRoleScrollViewCount % 2 == 0)
        {
            reAssignRoleScrollView.SetActive(false);
        }
        else
        {
            reAssignRoleScrollView.SetActive(true);
            ReAssignUplineAgent();
        }

    }
    public void ReAssignUplineAgent()
    {
        reAssignAgentList.club_id = ClubManagement.instance._clubID;
        reAssignAgentList.agent_id = detailPanel.GetChild(8).GetComponent<Text>().text;

        string body = JsonUtility.ToJson(reAssignAgentList);
        print(body);

        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(agentListUrl, body, ReAssignAgentListProcess);
    }

    void ReAssignAgentListProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("Some error ...!");
        }
        else
        {
            print("response" + response);

            agentListResponse = JsonUtility.FromJson<AgentListResponse>(response);

            if (!agentListResponse.error)
            {
                print(agentListResponse.agentListdata.Length);

                if (agentListResponse.agentListdata.Length > 0)
                {
                    if (agentListResponse.agentListdata.Length != reAssignAgentListCount)
                    {
                        for (int i = reAssignAgentListCount; i < agentListResponse.agentListdata.Length; i++)
                        {
                            reAssignAgentListCount++;
                            GenerateAgentListForReAssignDownline();
                        }

                    }

                    for (int i = 0; i < agentListResponse.agentListdata.Length; i++)
                    {
                        reAssignAgentContent.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = agentListResponse.agentListdata[i].username;
                        reAssignAgentContent.transform.GetChild(i).GetChild(2).GetComponent<Text>().text = agentListResponse.agentListdata[i].agent_id;
                        reAssignAgentContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text = agentListResponse.agentListdata[i].user_id;

                        reAssignAgentContent.transform.GetChild(i).gameObject.SetActive(true);

                    }

                    for (int i = 0; i < reAssignAgentContent.transform.childCount; i++)
                    {
                        reAssignAgentContent.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
                    }
                }
                else
                {
                    for (int i = 0; i < reAssignAgentContent.transform.childCount; i++)
                    {
                        reAssignAgentContent.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                reAssignAgentContent.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = string.Empty;
                reAssignAgentContent.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = string.Empty;
                reAssignAgentContent.transform.GetChild(0).GetChild(3).GetComponent<Text>().text = string.Empty;

                reAssignAgentContent.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    void GenerateAgentListForReAssignDownline()
    {
        scrollItemObj = Instantiate(reAssignAgentPanel);
        scrollItemObj.transform.SetParent(reAssignAgentContent.transform, false);

        reAssignAgentObjList.Add(scrollItemObj);
    }

    public void ResetAgentListItemsForReAssignAgent()
    {
        reAssignAgentListCount = 1;

        if (reAssignAgentObjList.Count > 0)
        {
            for (int i = 0; i < reAssignAgentObjList.Count; i++)
            {
                Destroy(reAssignAgentObjList[i]);
            }

            reAssignAgentObjList.Clear();
        }
    }
    
    public void ReAssignSelectRole(GameObject obj)
    {
        for (int i = 0; i < reAssignAgentContent.transform.childCount; i++)
        {
            reAssignAgentContent.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }

        obj.transform.GetChild(0).gameObject.SetActive(true);
        reAssignDropDownPanel.transform.GetChild(0).GetComponent<Text>().text = obj.transform.GetChild(1).GetComponent<Text>().text;
        reAssignRoleScrollViewCount = 0;

        reAssignRoleScrollView.SetActive(false);
        selectedAgentObj = obj;

        //ConfirmAssignMembers();
    }

    #endregion
}
