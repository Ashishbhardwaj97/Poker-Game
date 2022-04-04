using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DownlineManagementScript : MonoBehaviour
{
    public static DownlineManagementScript instance;
    [Header("GameObject Reference")]
    public GameObject downlineManagmentPanel;
    public GameObject assignDownlineContent;
    public GameObject assignDownlinePanel;
    public GameObject assignDownlineMember;
    public GameObject downlineMemberInfo;
    public GameObject assignDownlineMemberSelectedImg;
    public GameObject downlineMemberInfoSelectedImg;
    public GameObject downlineMemberContent;
    public GameObject downlineMemberPanel;
    public GameObject creditTransferPanel;
    public GameObject chipSentoutPanel;
    public GameObject chipClaimBackPanel;
    public GameObject tradeHistoryContent;
    public GameObject tradeHistoryPanel;
    public GameObject tracePlayerObj;
    public GameObject tracePlayerContent;
    public GameObject tracePlayerPanel;
    public GameObject observingTableContent;
    public GameObject observingTablePanel;

    [Header("InputField Reference")]
    public InputField sendOutChipInputField;
    public InputField claimBackChipInputField;
    public InputField assignDownlineSearchInputField;
    public InputField downlineMemberSearchInputField;

    //private string assignDownlineListUrl;
    //private string downlineMemberListUrl;
    //private string assignMemberUrl;
    //private string deallocateDownlineUrl;
    //private string agentSendClaimUrl;
    //private string agentTradeHistoryUrl;
    //private string tracePlayerUrl;

    int tradeaHistoryConut;
    int tracePlayerObjCount;

    public List<int> oldHour;
    public List<int> oldMin;
    public List<int> oldHour1;
    public List<int> oldMin1;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
        //assignDownlineListUrl = ServerChanger.instance.domainURL + "api/v1/club/assign-downline-list";
        //downlineMemberListUrl = ServerChanger.instance.domainURL + "api/v1/club/downline-members-list";
        //assignMemberUrl = ServerChanger.instance.domainURL + "api/v1/club/assign-member";
        //deallocateDownlineUrl = ServerChanger.instance.domainURL + "api/v1/club/deallocate-downline";
        //agentSendClaimUrl = ServerChanger.instance.domainURL + "api/v1/club/agent-send-claim-chips";
        //agentTradeHistoryUrl = ServerChanger.instance.domainURL + "api/v1/club/agent-trade-history";
        //tracePlayerUrl = ServerChanger.instance.domainURL + "api/v1/pokertable/trace-player";

        assignDownlineObjList = new List<GameObject>();
        downlineObjList = new List<GameObject>();
        tracePlayerGeneratedObj = new List<GameObject>();
        observingTableGeneratedObj = new List<GameObject>();
        assignDownlineCount = 1;
        downlineCount = 1;
        tradeaHistoryConut = 1;
        tracePlayerObjCount = 1;
        observingTableObjCount = 1;

        oldHour = new List<int>();
        oldMin = new List<int>();
        oldHour1 = new List<int>();
        oldMin1 = new List<int>();

        
    }
    
    #region Downline Management

    public void ClickOnBackButton()
    {
        ClubManagement.instance.memberProfileForAgent.SetActive(true);
        downlineManagmentPanel.SetActive(false);
        ResetAllValuesForDownline();
        ResetAllValuesForImage();

        ResetGeneratedAssignItems();
        ResetGeneratedDownlineItems();
    }

    public void ClickOnDownlineManagement()
    {
        ClubManagement.instance.memberProfileForAgent.SetActive(false);
        downlineManagmentPanel.SetActive(true);

        ClickOnAssignDownline();
    }

    public void ClickOnAssignDownline()
    {
        assignDownlineMember.SetActive(true);        
        assignDownlineMemberSelectedImg.SetActive(true);

        downlineMemberInfo.SetActive(false);
        downlineMemberInfoSelectedImg.SetActive(false);

        ClickAssignDownlineMember();
        
    }

    public void ClickOnDownlineMemberInfo()
    {
        assignDownlineMember.SetActive(false);
        assignDownlineMemberSelectedImg.SetActive(false);

        downlineMemberInfo.SetActive(true);
        downlineMemberInfoSelectedImg.SetActive(true);

        ClickDownlineMemberInfo();
    }

    #region Assign Downline Member list

    [Serializable]
    class AssgignDownlineRequest
    {
        public string club_id;
        public string agent_id;
    }

    [Serializable]
    class AssignDownlineResponse
    {
        public bool error;
        public AssignDownlineList[] assignDownlineList;
    }

    [Serializable]
    class AssignDownlineList
    {
        public string user_id;
        public string username;
        public string user_image;
        public string client_id;
        public string chips;
        public string agent_id;
    }

    [Header("Properties")]
    [SerializeField] AssgignDownlineRequest assgignDownlineRequest;
    [SerializeField] AssignDownlineResponse assignDownlineResponse;
    void ClickAssignDownlineMember()
    {
        assgignDownlineRequest.club_id = ClubManagement.instance._clubID;
        _selectedAgentId = ClubManagement.instance.memberProfileForAgent.GetComponent<MemberProfile>().detailPanel.GetChild(8).GetComponent<Text>().text;
        assgignDownlineRequest.agent_id = _selectedAgentId;
        string body = JsonUtility.ToJson(assgignDownlineRequest);

        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(assignDownlineListUrl, body, AssignDownlineMemberProcess);

        for (int i = 0; i < assignDownlineContent.transform.childCount; i++)
        {
            assignDownlineContent.transform.GetChild(i).gameObject.SetActive(false);
        }
        
        downlineMemberSearchInputField.transform.gameObject.SetActive(false);
        assignDownlineSearchInputField.transform.gameObject.SetActive(true);
        assignDownlineSearchInputField.text = string.Empty;
    }

    void AssignDownlineMemberProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("error");
        }
        else
        {
            print(response);
            assignDownlineResponse = JsonUtility.FromJson<AssignDownlineResponse>(response);
            if (!assignDownlineResponse.error)
            {
                if (assignDownlineResponse.assignDownlineList.Length > 0)
                {
                    if (assignDownlineResponse.assignDownlineList.Length != assignDownlineCount)
                    {
                        for (int i = assignDownlineCount; i < assignDownlineResponse.assignDownlineList.Length; i++)
                        {
                            assignDownlineCount++;
                            GenerateAssignDownlineMember();
                        }
                    }

                    userImage.Clear();
                    SearchingScript.instance.memberListNameInAssignDownline.Clear();
                    for (int i = 0; i < assignDownlineResponse.assignDownlineList.Length; i++)
                    {
                        assignDownlineContent.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = assignDownlineResponse.assignDownlineList[i].username;
                        assignDownlineContent.transform.GetChild(i).GetChild(2).GetComponent<Text>().text = assignDownlineResponse.assignDownlineList[i].client_id;
                        assignDownlineContent.transform.GetChild(i).GetChild(4).GetComponent<Text>().text = assignDownlineResponse.assignDownlineList[i].user_id;
                        assignDownlineContent.transform.GetChild(i).GetChild(6).GetComponent<Text>().text = assignDownlineResponse.assignDownlineList[i].agent_id;
                        assignDownlineContent.transform.GetChild(i).GetChild(3).GetChild(1).GetComponent<Text>().text = assignDownlineResponse.assignDownlineList[i].chips;

                        userImage.Add(assignDownlineResponse.assignDownlineList[i].user_image);

                        if (!string.IsNullOrEmpty(assignDownlineResponse.assignDownlineList[i].agent_id))
                        {
                            assignDownlineContent.transform.GetChild(i).GetChild(5).GetChild(1).gameObject.SetActive(true);
                        }
                        else
                        {
                            assignDownlineContent.transform.GetChild(i).GetChild(5).GetChild(1).gameObject.SetActive(false);
                        }

                        assignDownlineContent.transform.GetChild(i).gameObject.SetActive(true);
                        SearchingScript.instance.memberListNameInAssignDownline.Add(assignDownlineContent.transform.GetChild(i));
                    }

                    UpdatePlayerImage();

                }
            }

        }
    }

    public List<GameObject> assignDownlineObjList;
    private int assignDownlineCount;
    GameObject scrollItemObj;
    void GenerateAssignDownlineMember()
    {
        scrollItemObj = Instantiate(assignDownlinePanel);
        scrollItemObj.transform.SetParent(assignDownlineContent.transform, false);
        assignDownlineObjList.Add(scrollItemObj);
    }

    void ResetGeneratedAssignItems()
    {
        if (assignDownlineObjList.Count > 0)
        {
            for (int i = 0; i < assignDownlineObjList.Count; i++)
            {
                Destroy(assignDownlineObjList[i]);
            }
            assignDownlineObjList.Clear();
            assignDownlineCount = 1;
        }
    }

    #region update Player Image In AssignDownline Member list

    public List<string> userImage;
    
    [SerializeField]
    private List<PlayerImageInSequence> playerImageInSequence;

    private int k = 0;
    private int totalImageCount;
    private int count = 0;
    private int previousCountForMemberList;
    public void ResetAllValuesForImage()
    {
        totalImageCount = 0;
        k = 0;
        count = 0;
        previousCountForMemberList = 0;
        userImage.Clear();
        playerImageInSequence.Clear();
    }

    void UpdatePlayerImage()
    {
        //if (userImage.Count == previousCountForMemberList)
        //{
        //    return;
        //}
        
        k = 0;
        previousCountForMemberList = 0;
        totalImageCount = 0;
        playerImageInSequence.Clear();

        for (int i = 0; i < userImage.Count; i++)
        {
            assignDownlineContent.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
        }

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
        if (k == totalImageCount)
        {
            count = 0;
            ClubManagement.instance.loadingPanel.SetActive(false);

            for (int i = 0; i < userImage.Count; i++)
            {
                assignDownlineContent.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
            }

            for (int i = 0; i < userImage.Count; i++)
            {
                if (!string.IsNullOrEmpty(userImage[i]))
                {
                   
                    assignDownlineContent.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = playerImageInSequence[count].imgPic;
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

                if (instance.assignDownlineContent.activeInHierarchy)
                {
                    instance.totalImageCount++;
                    instance.ApplyImage();
                }
                else if (instance.tradeHistoryContent.activeInHierarchy)
                {
                    instance.totalImageCountForTradeHistory++;
                    instance.ApplyImageInTradeHistory();
                }

            }
        }
    }

    #endregion

    #endregion

    #region Downline Member list

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
        public string username;
        public string user_image;
        public string client_id;
        public string chips;
    }

    [SerializeField] DownlineMemberRequest downlineMemberRequest;
    [SerializeField] DownlineMemberResponse downlineMemberResponse;

    string _selectedAgentId;
    void ClickDownlineMemberInfo()
    {
        downlineMemberRequest.club_id = ClubManagement.instance._clubID;
        _selectedAgentId = ClubManagement.instance.memberProfileForAgent.GetComponent<MemberProfile>().detailPanel.GetChild(8).GetComponent<Text>().text;
        downlineMemberRequest.agent_id = _selectedAgentId;

        string body = JsonUtility.ToJson(downlineMemberRequest);
        print(body);
        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(downlineMemberListUrl, body, DownlineMemberInfoProcess);

        for (int i = 0; i < downlineMemberContent.transform.childCount; i++)
        {
            downlineMemberContent.transform.GetChild(i).gameObject.SetActive(false);
        }
       
        downlineMemberSearchInputField.transform.gameObject.SetActive(true);
        assignDownlineSearchInputField.transform.gameObject.SetActive(false);
        downlineMemberSearchInputField.text = string.Empty;
    }

    void DownlineMemberInfoProcess(string response)
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
                    if (downlineMemberResponse.downlineMembers.Length != downlineCount)
                    {
                        for (int i = downlineCount; i < downlineMemberResponse.downlineMembers.Length; i++)
                        {
                            downlineCount++;
                            GenerateDownlineMember();
                        }
                    }

                    userImageForDownline.Clear();
                    SearchingScript.instance.memberListNameInDownlineMember.Clear();
                    for (int i = 0; i < downlineMemberResponse.downlineMembers.Length; i++)
                    {
                        downlineMemberContent.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = downlineMemberResponse.downlineMembers[i].username;
                        downlineMemberContent.transform.GetChild(i).GetChild(2).GetComponent<Text>().text = downlineMemberResponse.downlineMembers[i].client_id;
                        downlineMemberContent.transform.GetChild(i).GetChild(4).GetComponent<Text>().text = downlineMemberResponse.downlineMembers[i].user_id;
                        downlineMemberContent.transform.GetChild(i).GetChild(3).GetChild(1).GetComponent<Text>().text = downlineMemberResponse.downlineMembers[i].chips;

                        userImageForDownline.Add(downlineMemberResponse.downlineMembers[i].user_image);
                        downlineMemberContent.transform.GetChild(i).gameObject.SetActive(true);

                        SearchingScript.instance.memberListNameInDownlineMember.Add(downlineMemberContent.transform.GetChild(i));
                    }
                    UpdatePlayerImageForDownline();
                }
            }
        }
    }

    public List<GameObject> downlineObjList;
    private int downlineCount;    
    
    void GenerateDownlineMember()
    {
        scrollItemObj = Instantiate(downlineMemberPanel);
        scrollItemObj.transform.SetParent(downlineMemberContent.transform, false);
        downlineObjList.Add(scrollItemObj);
    }

    void ResetGeneratedDownlineItems()
    {
        if (downlineObjList.Count > 0)
        {
            for (int i = 0; i < downlineObjList.Count; i++)
            {
                Destroy(downlineObjList[i]);
            }
            downlineObjList.Clear();
            downlineCount = 1;
        }
    }

    #region update Player Image In Downline Member list

    public List<string> userImageForDownline;

    [SerializeField]
    private List<PlayerImageInSequenceForDownline> playerImageInSequenceForDownline;

    private int l = 0;
    private int totalImageCountForDownline;
    private int countForDownline = 0;
    private int previousCountForDownline;
    public void ResetAllValuesForDownline()
    {
        totalImageCountForDownline = 0;
        l = 0;
        countForDownline = 0;
        previousCountForDownline = 0;
        userImageForDownline.Clear();
        playerImageInSequenceForDownline.Clear();
    }

    void UpdatePlayerImageForDownline()
    {
        //if (userImageForDownline.Count == previousCountForDownline)
        //{
        //    return;
        //}

        l = 0;
        previousCountForDownline = 0;
        totalImageCountForDownline = 0;
        playerImageInSequenceForDownline.Clear();

        for (int i = 0; i < userImageForDownline.Count; i++)
        {
            downlineMemberContent.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
        }

        playerImageInSequenceForDownline = new List<PlayerImageInSequenceForDownline>();

        for (int i = 0; i < userImageForDownline.Count; i++)
        {
            if (!string.IsNullOrEmpty(userImageForDownline[i]))
            {
                playerImageInSequenceForDownline.Add(new PlayerImageInSequenceForDownline());
                ClubManagement.instance.loadingPanel.SetActive(true);
                playerImageInSequenceForDownline[l].imgUrl = userImageForDownline[i];
                playerImageInSequenceForDownline[l].ImageProcess(userImageForDownline[i]);

                l = l + 1;

            }
            previousCountForDownline++;
        }
    }

    [Serializable]
    public class PlayerImageInSequenceForDownline
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
                instance.totalImageCountForDownline++;
                instance.ApplyImageForDownline();
            }
        }
    }

    void ApplyImageForDownline()
    {        
        if (l == totalImageCountForDownline)
        {
            countForDownline = 0;
            ClubManagement.instance.loadingPanel.SetActive(false);

            for (int i = 0; i < userImageForDownline.Count; i++)
            {
                downlineMemberContent.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
            }

            for (int i = 0; i < userImageForDownline.Count; i++)
            {
                if (!string.IsNullOrEmpty(userImageForDownline[i]))
                {
                    print("countForDownline = " + i);
                    downlineMemberContent.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = playerImageInSequenceForDownline[countForDownline].imgPic;
                    countForDownline++;
                }

            }

        }
    }

    #endregion

    #endregion

    #region Assign/Deallocate Players as Downline members

    [Serializable]
    public class UpdateAssignMember
    {
        public bool error;
    }
    
    [Serializable]
    class DeallocateDownlineList
    {
        public List<DeallocateDownline> deallocateDownline;
    }

    [Serializable]
    class AssignMemberList
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

    [Serializable]
    class DeallocateDownline
    {
        public string user_id;
        public string club_id;       
    }

    [SerializeField] AssignMemberList assignMemberList;
    [SerializeField] DeallocateDownlineList deallocateDownlineList;
    [SerializeField] UpdateAssignMember updateAssignMember;
    [SerializeField] UpdateAssignMember deallocateDownlineResponse;

    int j = 0;
    int j1 = 0;
    public void ClickOnConfirmAssignMember()
    {
        assignMemberList.assignMembers.Clear();
        deallocateDownlineList.deallocateDownline.Clear();
        j = 0;
        j1 = 0;
        string _agentID = ClubManagement.instance.memberProfileForAgent.transform.GetComponent<MemberProfile>().detailPanel.GetChild(8).GetComponent<Text>().text;
        for (int i = 0; i < assignDownlineContent.transform.childCount; i++)
        {
            if (assignDownlineContent.transform.GetChild(i).GetChild(5).GetChild(1).gameObject.activeInHierarchy)
            {
                assignMemberList.assignMembers.Add(new AssignMembers());

                assignMemberList.assignMembers[j].user_id = assignDownlineContent.transform.GetChild(i).GetChild(4).GetComponent<Text>().text;
                assignMemberList.assignMembers[j].club_id = ClubManagement.instance._clubID;
                assignMemberList.assignMembers[j].agent_id = _agentID;
                j = j + 1;
            }
            else
            {
                deallocateDownlineList.deallocateDownline.Add(new DeallocateDownline());
                deallocateDownlineList.deallocateDownline[j1].user_id = assignDownlineContent.transform.GetChild(i).GetChild(4).GetComponent<Text>().text;
                deallocateDownlineList.deallocateDownline[j1].club_id = ClubManagement.instance._clubID;
                j1 = j1 + 1;
            }

        }

        string body = JsonUtility.ToJson(assignMemberList);
        string body1 = JsonUtility.ToJson(deallocateDownlineList);

        print(body);
        print(body1);

        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(assignMemberUrl, body, AssignDownlineProcess);
        //Communication.instance.PostData(deallocateDownlineUrl, body1, DeallocateDownlineProcess);

    }

    void DeallocateDownlineProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("Some error ...!");
        }
        else
        {
            print("response" + response);

            deallocateDownlineResponse = JsonUtility.FromJson<UpdateAssignMember>(response);

            if (!deallocateDownlineResponse.error)
            {

            }
        }
    }

    void AssignDownlineProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("Some error ...!");
        }
        else
        {
            print("response" + response);

            updateAssignMember = JsonUtility.FromJson<UpdateAssignMember>(response);

            if (!updateAssignMember.error)
            {
                
            }
        }
    }

    #endregion

    #endregion

    #region Credit Transfer

    public void ClickCreditTransferBackButton()
    {
        ClubManagement.instance.memberProfileForAgent.SetActive(true);
        creditTransferPanel.SetActive(false);
    }

    public void ClickOnCreditTransfer()
    {
        ClubManagement.instance.memberProfileForAgent.SetActive(false);
        creditTransferPanel.SetActive(true);
        ResetTradeHistoryObj();

        AgentDataCopy();
        AgentTradeHistory();
    }

    public void ClickSendOutButton()
    {       
        chipSentoutPanel.SetActive(true);
        chipClaimBackPanel.SetActive(false);

        chipSentoutPanel.transform.GetChild(1).GetChild(2).GetChild(1).GetComponent<Text>().text = creditTransferPanel.transform.GetChild(2).GetChild(3).GetChild(1).GetComponent<Text>().text; // agent chips

    }

    public void ClickClaimBackButton()
    {
        chipSentoutPanel.SetActive(false);
        chipClaimBackPanel.SetActive(true);
        chipClaimBackPanel.transform.GetChild(1).GetChild(2).GetChild(1).GetComponent<Text>().text = creditTransferPanel.transform.GetChild(2).GetChild(3).GetChild(1).GetComponent<Text>().text;
    }

    void AgentDataCopy()
    {
       Transform _detailPanel = ClubManagement.instance.memberProfileForAgent.transform.GetComponent<MemberProfile>().detailPanel;

        creditTransferPanel.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = _detailPanel.GetChild(0).GetComponent<Image>().sprite;  //profile pic

        string _userName = _detailPanel.GetChild(1).GetComponent<Text>().text;

        _userName = _userName.Replace(" ", string.Empty);
        _userName = _userName + "      ";
        creditTransferPanel.transform.GetChild(2).GetChild(1).GetComponent<Text>().text = _userName;       //user name
        creditTransferPanel.transform.GetChild(2).GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = _detailPanel.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text; //role
        creditTransferPanel.transform.GetChild(2).GetChild(2).GetComponent<Text>().text = _detailPanel.GetChild(3).GetComponent<Text>().text;       // user id
        creditTransferPanel.transform.GetChild(2).GetChild(3).GetChild(1).GetComponent<Text>().text = _detailPanel.GetChild(12).GetComponent<Text>().text; // agent chips

        chipSentoutPanel.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = _userName;      //user name
        chipSentoutPanel.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = _detailPanel.GetChild(3).GetComponent<Text>().text;      //user id
        
        chipClaimBackPanel.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = _userName;    //user name
        chipClaimBackPanel.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = _detailPanel.GetChild(3).GetComponent<Text>().text;    // user id
        
    }

    #region Agent Trade History

    [Serializable]
    class TradeHistoryRequest
    {
        public string club_id;
        public string user_id;
    }

    [Serializable]
    class TradeHistoryResponse
    {
        public bool error;
        public TradeHistory[] tradeHistory;
        
    }

    [Serializable]
    class TradeHistory
    {
        public string sender_username;
        public string username;
        public string sender_client_id;
        public string client_id;
        public string receiver_club_role;
        public string sender_club_role;
        public string createdAt;
        public string sender_user_image;
        public string user_image;
        public int chips;
        public int txn_type;

    }

    [SerializeField] TradeHistoryRequest tradeHistoryRequest;
    [SerializeField] TradeHistoryResponse tradeHistoryResponse;

    
    void AgentTradeHistory()
    {
        tradeHistoryRequest.club_id = ClubManagement.instance._clubID;
        string _userID = ClubManagement.instance.memberProfileForAgent.transform.GetComponent<MemberProfile>().currentUserID;
        tradeHistoryRequest.user_id = _userID;

        string body = JsonUtility.ToJson(tradeHistoryRequest);
        print(body);

        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(agentTradeHistoryUrl, body, AgentTradeHistoryProcess);
    }

    void AgentTradeHistoryProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("Some error ...!");
        }
        else
        {
            print("response" + response);

            tradeHistoryResponse = JsonUtility.FromJson<TradeHistoryResponse>(response);

            if (!tradeHistoryResponse.error)
            {
                if (tradeHistoryResponse.tradeHistory.Length > 0)
                {
                    if (tradeHistoryResponse.tradeHistory.Length != tradeaHistoryConut)
                    {
                        for (int i = tradeaHistoryConut; i < tradeHistoryResponse.tradeHistory.Length; i++)
                        {
                            tradeaHistoryConut++;
                            GenerateTradeHistoryItem();
                        }
                    }

                    userImageForTradeHistory.Clear();
                    for (int i = 0; i < tradeHistoryResponse.tradeHistory.Length; i++)
                    {
                        tradeHistoryContent.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = tradeHistoryResponse.tradeHistory[i].sender_username + "      ";
                        tradeHistoryContent.transform.GetChild(i).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = tradeHistoryResponse.tradeHistory[i].sender_club_role;
                        tradeHistoryContent.transform.GetChild(i).GetChild(2).GetComponent<Text>().text = tradeHistoryResponse.tradeHistory[i].username + "      ";
                        tradeHistoryContent.transform.GetChild(i).GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = tradeHistoryResponse.tradeHistory[i].receiver_club_role;
                        tradeHistoryContent.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = tradeHistoryResponse.tradeHistory[i].sender_client_id;
                        tradeHistoryContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text = tradeHistoryResponse.tradeHistory[i].client_id;
                        tradeHistoryContent.transform.GetChild(i).GetChild(4).GetChild(1).GetComponent<Text>().text = tradeHistoryResponse.tradeHistory[i].chips.ToString();

                        string date = ClubManagement.instance.ConvertDateTime(tradeHistoryResponse.tradeHistory[i].createdAt);
                        tradeHistoryContent.transform.GetChild(i).GetChild(5).GetComponent<Text>().text = date;
                        
                        if (tradeHistoryResponse.tradeHistory[i].txn_type == 1)     //.....Claim-back........//
                        {
                            tradeHistoryContent.transform.GetChild(i).GetChild(6).localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
                        }
                        else if (tradeHistoryResponse.tradeHistory[i].txn_type == 2)     //.....Send-out........//
                        {
                            tradeHistoryContent.transform.GetChild(i).GetChild(6).localRotation = Quaternion.Euler(Vector3.zero);
                        }
                        tradeHistoryContent.transform.GetChild(i).gameObject.SetActive(true);

                        userImageForTradeHistory.Add(tradeHistoryResponse.tradeHistory[i].user_image);
                        userImageForTradeHistory.Add(tradeHistoryResponse.tradeHistory[i].sender_user_image);
                        
                    }

                    UpdatePlayerImageForTradeHistory();
                }
            }
        }
    }

    public List<GameObject> tradeHistoryList;
    void GenerateTradeHistoryItem()
    {
        scrollItemObj = Instantiate(tradeHistoryPanel);
        scrollItemObj.transform.SetParent(tradeHistoryContent.transform, false);
        tradeHistoryList.Add(scrollItemObj);
    }

    void ResetTradeHistoryObj()
    {
        if (tradeHistoryList.Count > 0)
        {
            for (int i = 0; i < tradeHistoryList.Count; i++)
            {
                Destroy(tradeHistoryList[i]);
            }
            tradeHistoryList.Clear();
            tradeaHistoryConut = 1;
        }

        tradeHistoryPanel.SetActive(false);
    }

    #region Update player image in Trade History

    private int m;
    private int totalImageCountForTradeHistory;
    private int countForTradeHistory;

    private int previousCountForTradeHistory;

    public List<string> userImageForTradeHistory;
    [SerializeField]
    public List<PlayerImageInSequence> playerImageInSequenceForTradeHistory;

    public void ResetPlayerImageInTradeHistory()
    {
        m = 0;
        totalImageCountForTradeHistory = 0;
        countForTradeHistory = 0;
        previousCountForTradeHistory = 0;
        userImageForTradeHistory.Clear();
        playerImageInSequenceForTradeHistory.Clear();
    }

    void UpdatePlayerImageForTradeHistory()
    {
        if (userImageForTradeHistory.Count == previousCountForTradeHistory)
        {
            return;
        }

        for (int i = 0; i < userImageForTradeHistory.Count / 2; i++)
        {
            print(i);
            tradeHistoryContent.transform.GetChild(i).GetChild(7).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
            tradeHistoryContent.transform.GetChild(i).GetChild(8).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
        }

        playerImageInSequenceForTradeHistory.Clear();
        m = 0;
        totalImageCountForTradeHistory = 0;
        previousCountForTradeHistory = 0;

        playerImageInSequenceForTradeHistory = new List<PlayerImageInSequence>();

        for (int i = 0; i < userImageForTradeHistory.Count; i++)
        {
            if (!string.IsNullOrEmpty(userImageForTradeHistory[i]))
            {
                print("UpdatePlayerImageFor Trade ......");
                playerImageInSequenceForTradeHistory.Add(new PlayerImageInSequence());
                ClubManagement.instance.loadingPanel.SetActive(true);
                playerImageInSequenceForTradeHistory[m].imgUrl = userImageForTradeHistory[i];
                playerImageInSequenceForTradeHistory[m].ImageProcess(userImageForTradeHistory[i]);

                m = m + 1;

            }

            previousCountForTradeHistory++;
        }
    }

    public void ApplyImageInTradeHistory()
    {
        print(m);
        print(totalImageCountForTradeHistory);
        if (m == totalImageCountForTradeHistory)
        {
            countForTradeHistory = 0;

            ClubManagement.instance.loadingPanel.SetActive(false);

            for (int i = 0; i < userImageForTradeHistory.Count / 2; i++)
            {
                print(i);
                tradeHistoryContent.transform.GetChild(i).GetChild(7).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
                tradeHistoryContent.transform.GetChild(i).GetChild(8).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
            }

            for (int i = 0; i < userImageForTradeHistory.Count; i++)
            {
                if (!string.IsNullOrEmpty(userImageForTradeHistory[i]))
                {
                    if (countForTradeHistory < playerImageInSequenceForTradeHistory.Count)
                    {
                        if (i % 2 == 0)
                        {
                            tradeHistoryContent.transform.GetChild(i / 2).GetChild(7).GetComponent<Image>().sprite = playerImageInSequenceForTradeHistory[countForTradeHistory].imgPic;

                        }
                        else
                        {
                            tradeHistoryContent.transform.GetChild((int)(i / 2)).GetChild(8).GetComponent<Image>().sprite = playerImageInSequenceForTradeHistory[countForTradeHistory].imgPic;
                        }
                        countForTradeHistory = countForTradeHistory + 1;
                    }
                }
            }

        }
    }

    #endregion

    #endregion

    #region Sendout/Claimback chips

    [Serializable]
    class SendOutClaimBackChipsRequest
    {
        public string club_id;
        public string receiver_user_id;
        public string agent_id;
        public int txn_type;
        public int chips;
        public string receiver_club_role;
        public string sender_club_role;
    }

    [Serializable]
    class SendOutClaimBackResponse
    {
        public bool error;
    }

    [SerializeField] SendOutClaimBackChipsRequest sendoutChipsRequest;
    [SerializeField] SendOutClaimBackChipsRequest claimBackChipsRequest;

    [SerializeField] SendOutClaimBackResponse sendOutClaimBackResponse;
    public void ClickSendOutChips()
    {
        string _userID = ClubManagement.instance.memberProfileForAgent.transform.GetComponent<MemberProfile>().currentUserID;
        string _agentID = ClubManagement.instance.memberProfileForAgent.transform.GetComponent<MemberProfile>().detailPanel.GetChild(8).GetComponent<Text>().text;

        sendoutChipsRequest.club_id = ClubManagement.instance._clubID;       
        sendoutChipsRequest.receiver_user_id = _userID;
        sendoutChipsRequest.agent_id = _agentID;
        sendoutChipsRequest.txn_type = 2;
        sendoutChipsRequest.chips = Convert.ToInt32(sendOutChipInputField.text);
        sendoutChipsRequest.receiver_club_role = "Agent";
        sendoutChipsRequest.sender_club_role = ClubManagement.instance.currentSelectedClubRole;

        string body = JsonUtility.ToJson(sendoutChipsRequest);
        print(body);
        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(agentSendClaimUrl, body, SendOutClaimBackChipsProcess);
    }

    public void ClickClaimBackChips()
    {
        string _userID = ClubManagement.instance.memberProfileForAgent.transform.GetComponent<MemberProfile>().currentUserID;
        string _agentID = ClubManagement.instance.memberProfileForAgent.transform.GetComponent<MemberProfile>().detailPanel.GetChild(8).GetComponent<Text>().text;

        claimBackChipsRequest.club_id = ClubManagement.instance._clubID;
        claimBackChipsRequest.receiver_user_id = _userID;
        claimBackChipsRequest.agent_id = _agentID;
        claimBackChipsRequest.txn_type = 1;
        claimBackChipsRequest.chips = Convert.ToInt32(claimBackChipInputField.text);
        claimBackChipsRequest.receiver_club_role = "Agent";
        claimBackChipsRequest.sender_club_role = ClubManagement.instance.currentSelectedClubRole;

        string body = JsonUtility.ToJson(claimBackChipsRequest);
        print(body);
        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(agentSendClaimUrl, body, SendOutClaimBackChipsProcess);
    }

    void SendOutClaimBackChipsProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("Some error ...!");
        }
        else
        {
            print("response" + response);

            sendOutClaimBackResponse = JsonUtility.FromJson<SendOutClaimBackResponse>(response);

            if (!sendOutClaimBackResponse.error)
            {
                ClubManagement.instance.memberProfileForAgent.transform.GetComponent<MemberProfile>().MemberProfileReq();
                
                chipClaimBackPanel.SetActive(false);
                chipSentoutPanel.SetActive(false);

                claimBackChipInputField.text = string.Empty;
                sendOutChipInputField.text = string.Empty;

                AgentTradeHistory();
            }
        }
    }



    #endregion

    #endregion

    #region Trace Player

    public void ClickTracePlayerBackBtn()
    {
        selectedMemberProfile.SetActive(true);
        tracePlayerObj.SetActive(false);

        ResetTracePlayerObj();
        ResetObservingTableObj();
        isPlayingTime = false;

        isPlayingTime1 = false;

        oldHour.Clear();
        oldMin.Clear();
        oldHour1.Clear();
        oldMin1.Clear();

        CancelInvoke("CalculateTime1");
        CancelInvoke("CalculateTime");
    }

    public void ResetPlayingPanelsOnBackButton()
    {
        for (int i = 0; i < tracePlayerContent.transform.childCount; i++)
        {
            tracePlayerContent.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
            tracePlayerContent.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
        }
    }

    public void ResetObservingPanelsOnBackButton()
    {
        for (int i = 0; i < observingTableContent.transform.childCount; i++)
        {
            observingTableContent.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
            observingTableContent.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
        }
    }

    internal GameObject selectedMemberProfile;
    public void ClickTracePlayer()
    {
        if (ClubManagement.instance.memberProfileForAgent.activeInHierarchy)
        {
            ClubManagement.instance.memberProfileForAgent.SetActive(false);
            selectedMemberProfile = ClubManagement.instance.memberProfileForAgent;
        }
        else
        {
            ClubManagement.instance.memberProfile.SetActive(false);
            selectedMemberProfile = ClubManagement.instance.memberProfile;
        }
        tracePlayerObj.SetActive(true);
        ClickPlayingTableList();
        
    }

    public void ClickPlayingTableList()
    {
        tracePlayerObj.transform.GetChild(2).GetChild(0).GetChild(0).gameObject.SetActive(true);
        tracePlayerObj.transform.GetChild(2).GetChild(1).GetChild(0).gameObject.SetActive(false);

        tracePlayerObj.transform.GetChild(3).gameObject.SetActive(true);
        tracePlayerObj.transform.GetChild(4).gameObject.SetActive(false);

        CopyDataInTracePlayer();
        ClickTracePlayerRequest();
        
    }

    public void ClickObservingTableList()
    {
        tracePlayerObj.transform.GetChild(2).GetChild(0).GetChild(0).gameObject.SetActive(false);
        tracePlayerObj.transform.GetChild(2).GetChild(1).GetChild(0).gameObject.SetActive(true);

        tracePlayerObj.transform.GetChild(3).gameObject.SetActive(false);
        tracePlayerObj.transform.GetChild(4).gameObject.SetActive(true);
    }

    #region Trace players
    public bool isPlayingTime;

    void CopyDataInTracePlayer()
    {
        Transform _detailPanel = selectedMemberProfile.transform.GetComponent<MemberProfile>().detailPanel;
        Sprite _flag = selectedMemberProfile.transform.GetComponent<MemberProfile>().countryFlag.sprite;
        string userName = _detailPanel.GetChild(1).GetComponent<Text>().text;
        userName = userName.Replace(" ", string.Empty);

        for (int i = 0; i < tracePlayerContent.transform.childCount; i++)
        {
            tracePlayerContent.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().sprite = _detailPanel.GetChild(0).GetComponent<Image>().sprite;//profile pic
            tracePlayerContent.transform.GetChild(i).GetChild(0).GetChild(1).GetComponent<Text>().text = userName + "        ";//user name
            tracePlayerContent.transform.GetChild(i).GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>().sprite = _flag;          //flag
            tracePlayerContent.transform.GetChild(i).GetChild(0).GetChild(2).GetComponent<Text>().text = _detailPanel.GetChild(3).GetComponent<Text>().text;//userID

            tracePlayerContent.transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<Image>().sprite = _detailPanel.GetChild(0).GetComponent<Image>().sprite;//profile pic
            tracePlayerContent.transform.GetChild(i).GetChild(1).GetChild(1).GetComponent<Text>().text = userName + "        ";//user name
            tracePlayerContent.transform.GetChild(i).GetChild(1).GetChild(1).GetChild(0).GetComponent<Image>().sprite = _flag;          //flag
            tracePlayerContent.transform.GetChild(i).GetChild(1).GetChild(2).GetComponent<Text>().text = _detailPanel.GetChild(3).GetComponent<Text>().text;//userID
        }

        for (int i = 0; i < observingTableContent.transform.childCount; i++)
        {
            observingTableContent.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().sprite = _detailPanel.GetChild(0).GetComponent<Image>().sprite;//profile pic
            observingTableContent.transform.GetChild(i).GetChild(0).GetChild(1).GetComponent<Text>().text = userName + "        ";//user name
            observingTableContent.transform.GetChild(i).GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>().sprite = _flag;          //flag
            observingTableContent.transform.GetChild(i).GetChild(0).GetChild(2).GetComponent<Text>().text = _detailPanel.GetChild(3).GetComponent<Text>().text;//userID

            observingTableContent.transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<Image>().sprite = _detailPanel.GetChild(0).GetComponent<Image>().sprite;//profile pic
            observingTableContent.transform.GetChild(i).GetChild(1).GetChild(1).GetComponent<Text>().text = userName + "        ";//user name
            observingTableContent.transform.GetChild(i).GetChild(1).GetChild(1).GetChild(0).GetComponent<Image>().sprite = _flag;          //flag
            observingTableContent.transform.GetChild(i).GetChild(1).GetChild(2).GetComponent<Text>().text = _detailPanel.GetChild(3).GetComponent<Text>().text;//userID
        }

    }

    [Serializable]
    class TracePlayerRequest
    {
        public string username;
        public string club_id;
    }

    [Serializable]
    class TracePlayerResponse
    {
        public bool error;
        public PlayingTable[] playingTable;
        public ObserverTable[] Observertable;
    }

    [Serializable]
    class PlayingTable
    {
        public string table_type;
        public string end_time;
        public string game_type;
        public string table_name;
        public string table_id;
        public string start_time;
        public string rule_id;

        public int small_blind;
        public int big_blind;
        public int min_buy_in;
        public int min_auto_start;
        public int blinds_up;
        public int table_size;
        public int action_time;

        public bool mississippi_straddle;
        public bool auto_start;
        public bool buy_in_authorization;
        public bool video_mode;
        public PlayerTime[] playerTime;
    }

    [Serializable]
    class ObserverTable
    {
        public string table_type;
        public string end_time;
        public string game_type;
        public string table_name;
        public string table_id;
        public string start_time;
        public string rule_id;

        public int small_blind;
        public int big_blind;
        public int min_buy_in;
        public int min_auto_start;
        public int blinds_up;
        public int table_size;
        public int action_time;

        public bool mississippi_straddle;
        public bool auto_start;
        public bool buy_in_authorization;
        public bool video_mode;
        public PlayerTime[] playerTime;
    }
    [Serializable]
    class PlayerTime
    {
        public string playerName;
        public double joinedAt;
    }

    [SerializeField] TracePlayerRequest tracePlayerRequest;
    [SerializeField] TracePlayerResponse tracePlayerResponse;
    public void ClickTracePlayerRequest()
    {
        string str = selectedMemberProfile.transform.GetComponent<MemberProfile>().detailPanel.GetChild(1).GetComponent<Text>().text;
        str = str.Replace(" ", string.Empty);
        tracePlayerRequest.username = str;
        tracePlayerRequest.club_id = ClubManagement.instance._clubID;
        string body = JsonUtility.ToJson(tracePlayerRequest);
        print(body);
        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(tracePlayerUrl, body, TracePlayerProcess);
    }
   
    void TracePlayerProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("Some error ...!");
        }
        else
        {
            print("response" + response);

            tracePlayerResponse = JsonUtility.FromJson<TracePlayerResponse>(response);

            if (!tracePlayerResponse.error)
            {
                string userName = tracePlayerContent.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text;
                userName = userName.Replace(" ", string.Empty);
                if (tracePlayerResponse.playingTable.Length > 0)
                {
                    if (tracePlayerResponse.playingTable.Length != tracePlayerObjCount)
                    {
                        for (int i = tracePlayerObjCount; i < tracePlayerResponse.playingTable.Length; i++)
                        {
                            tracePlayerObjCount++;
                            GenerateTracePlayerObj();
                        }
                    }

                    for (int i = 0; i < tracePlayerResponse.playingTable.Length; i++)
                    {
                        tracePlayerContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.table_type = tracePlayerResponse.playingTable[i].table_type;
                        tracePlayerContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.end_time = tracePlayerResponse.playingTable[i].end_time;
                        tracePlayerContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.game_type = tracePlayerResponse.playingTable[i].game_type;
                        tracePlayerContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.table_name = tracePlayerResponse.playingTable[i].table_name;
                        tracePlayerContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.small_blind = tracePlayerResponse.playingTable[i].small_blind;
                        tracePlayerContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.big_blind = tracePlayerResponse.playingTable[i].big_blind;
                        tracePlayerContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.min_buy_in = tracePlayerResponse.playingTable[i].min_buy_in;
                        tracePlayerContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.min_auto_start = tracePlayerResponse.playingTable[i].min_auto_start;
                        tracePlayerContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.blinds_up = tracePlayerResponse.playingTable[i].blinds_up;
                        tracePlayerContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.table_size = tracePlayerResponse.playingTable[i].table_size;
                        tracePlayerContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.action_time = tracePlayerResponse.playingTable[i].action_time;
                        tracePlayerContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.start_time = tracePlayerResponse.playingTable[i].start_time;
                        tracePlayerContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.rule_id = tracePlayerResponse.playingTable[i].rule_id;
                        tracePlayerContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.table_id = tracePlayerResponse.playingTable[i].table_id;
                        tracePlayerContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.mississippi_straddle = tracePlayerResponse.playingTable[i].mississippi_straddle;
                        tracePlayerContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.auto_start = tracePlayerResponse.playingTable[i].auto_start;
                        tracePlayerContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.buy_in_authorization = tracePlayerResponse.playingTable[i].buy_in_authorization;
                        tracePlayerContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.video_mode = tracePlayerResponse.playingTable[i].video_mode;
                    }

                    for (int i = 0; i < tracePlayerResponse.playingTable.Length; i++)
                    {
                        tracePlayerContent.transform.GetChild(i).GetChild(0).GetChild(3).GetComponent<Text>().text = tracePlayerResponse.playingTable[i].table_name + "      ";
                        tracePlayerContent.transform.GetChild(i).GetChild(0).GetChild(4).GetChild(0).GetComponent<Text>().text = tracePlayerResponse.playingTable[i].game_type;                        
                        tracePlayerContent.transform.GetChild(i).GetChild(0).GetChild(5).GetComponent<Text>().text = tracePlayerResponse.playingTable[i].small_blind.ToString("0.##");
                        tracePlayerContent.transform.GetChild(i).GetChild(0).GetChild(6).GetComponent<Text>().text = tracePlayerResponse.playingTable[i].big_blind.ToString("0.##");

                        tracePlayerContent.transform.GetChild(i).GetChild(1).GetChild(3).GetComponent<Text>().text = tracePlayerResponse.playingTable[i].table_name + "      ";
                        tracePlayerContent.transform.GetChild(i).GetChild(1).GetChild(4).GetChild(0).GetComponent<Text>().text = tracePlayerResponse.playingTable[i].game_type;                        
                        tracePlayerContent.transform.GetChild(i).GetChild(1).GetChild(5).GetComponent<Text>().text = tracePlayerResponse.playingTable[i].small_blind.ToString("0.##");
                        tracePlayerContent.transform.GetChild(i).GetChild(1).GetChild(6).GetComponent<Text>().text = tracePlayerResponse.playingTable[i].big_blind.ToString("0.##");

                        if (tracePlayerResponse.playingTable[i].video_mode)
                        {
                            tracePlayerContent.transform.GetChild(i).GetChild(0).GetChild(8).gameObject.SetActive(true);
                            tracePlayerContent.transform.GetChild(i).GetChild(0).GetChild(9).gameObject.SetActive(false);

                            tracePlayerContent.transform.GetChild(i).GetChild(1).GetChild(8).gameObject.SetActive(true);
                            tracePlayerContent.transform.GetChild(i).GetChild(1).GetChild(9).gameObject.SetActive(false);
                        }
                        else
                        {
                            tracePlayerContent.transform.GetChild(i).GetChild(0).GetChild(8).gameObject.SetActive(false);
                            tracePlayerContent.transform.GetChild(i).GetChild(0).GetChild(9).gameObject.SetActive(true);

                            tracePlayerContent.transform.GetChild(i).GetChild(1).GetChild(8).gameObject.SetActive(false);
                            tracePlayerContent.transform.GetChild(i).GetChild(1).GetChild(9).gameObject.SetActive(true);
                        }

                        for (int j = 0; j < tracePlayerResponse.playingTable[i].playerTime.Length; j++)
                        {
                            if (userName == tracePlayerResponse.playingTable[i].playerTime[j].playerName)
                            {
                                
                                if (!string.IsNullOrEmpty(tracePlayerResponse.playingTable[i].playerTime[j].joinedAt.ToString()))
                                {
                                    DateTime _date = new DateTime(Convert.ToInt64(tracePlayerResponse.playingTable[i].playerTime[j].joinedAt.ToString()));

                                    string _joiningTime = _date.ToString("hh:mm");
                                    
                                    tracePlayerContent.transform.GetChild(i).GetChild(1).GetChild(7).GetChild(1).GetComponent<Text>().text = _joiningTime;
                                }
                                else
                                {
                                    tracePlayerContent.transform.GetChild(i).GetChild(1).GetChild(7).GetChild(1).GetComponent<Text>().text = string.Empty;
                                }
                            }
                        }
                      
                        tracePlayerContent.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    //CancelInvoke("CalculateTime");
                    //CalculateTime();
                }

                else
                {
                    ResetTracePlayerObj();
                    tracePlayerContent.transform.GetChild(0).gameObject.SetActive(false);
                }

                if (tracePlayerResponse.Observertable.Length > 0)
                {
                    if (tracePlayerResponse.Observertable.Length != observingTableObjCount)
                    {
                        for (int i = observingTableObjCount; i < tracePlayerResponse.Observertable.Length; i++)
                        {
                            observingTableObjCount++;
                            GenerateObservingTableObj();
                        }
                    }

                    for (int i = 0; i < tracePlayerResponse.Observertable.Length; i++)
                    {
                        observingTableContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.table_type = tracePlayerResponse.Observertable[i].table_type;
                        observingTableContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.end_time = tracePlayerResponse.Observertable[i].end_time;
                        observingTableContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.game_type = tracePlayerResponse.Observertable[i].game_type;
                        observingTableContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.table_name = tracePlayerResponse.Observertable[i].table_name;
                        observingTableContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.small_blind = tracePlayerResponse.Observertable[i].small_blind;
                        observingTableContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.big_blind = tracePlayerResponse.Observertable[i].big_blind;
                        observingTableContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.min_buy_in = tracePlayerResponse.Observertable[i].min_buy_in;
                        observingTableContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.min_auto_start = tracePlayerResponse.Observertable[i].min_auto_start;
                        observingTableContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.blinds_up = tracePlayerResponse.Observertable[i].blinds_up;
                        observingTableContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.table_size = tracePlayerResponse.Observertable[i].table_size;
                        observingTableContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.action_time = tracePlayerResponse.Observertable[i].action_time;
                        observingTableContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.start_time = tracePlayerResponse.Observertable[i].start_time;
                        observingTableContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.rule_id = tracePlayerResponse.Observertable[i].rule_id;
                        observingTableContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.table_id = tracePlayerResponse.Observertable[i].table_id;
                        observingTableContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.mississippi_straddle = tracePlayerResponse.Observertable[i].mississippi_straddle;
                        observingTableContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.auto_start = tracePlayerResponse.Observertable[i].auto_start;
                        observingTableContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.buy_in_authorization = tracePlayerResponse.Observertable[i].buy_in_authorization;
                        observingTableContent.transform.GetChild(i).GetComponent<TableInfoScript>().tableData.video_mode = tracePlayerResponse.Observertable[i].video_mode;
                    }

                    for (int i = 0; i < tracePlayerResponse.Observertable.Length; i++)
                    {
                        observingTableContent.transform.GetChild(i).GetChild(0).GetChild(3).GetComponent<Text>().text = tracePlayerResponse.Observertable[i].table_name + "      ";
                        observingTableContent.transform.GetChild(i).GetChild(0).GetChild(4).GetChild(0).GetComponent<Text>().text = tracePlayerResponse.Observertable[i].game_type;
                        observingTableContent.transform.GetChild(i).GetChild(0).GetChild(5).GetComponent<Text>().text = tracePlayerResponse.Observertable[i].small_blind.ToString("0.##");
                        observingTableContent.transform.GetChild(i).GetChild(0).GetChild(6).GetComponent<Text>().text = tracePlayerResponse.Observertable[i].big_blind.ToString("0.##");

                        observingTableContent.transform.GetChild(i).GetChild(1).GetChild(3).GetComponent<Text>().text = tracePlayerResponse.Observertable[i].table_name + "      ";
                        observingTableContent.transform.GetChild(i).GetChild(1).GetChild(4).GetChild(0).GetComponent<Text>().text = tracePlayerResponse.Observertable[i].game_type;
                        observingTableContent.transform.GetChild(i).GetChild(1).GetChild(5).GetComponent<Text>().text = tracePlayerResponse.Observertable[i].small_blind.ToString("0.##");
                        observingTableContent.transform.GetChild(i).GetChild(1).GetChild(6).GetComponent<Text>().text = tracePlayerResponse.Observertable[i].big_blind.ToString("0.##");

                        if (tracePlayerResponse.Observertable[i].video_mode)
                        {
                            observingTableContent.transform.GetChild(i).GetChild(0).GetChild(8).gameObject.SetActive(true);
                            observingTableContent.transform.GetChild(i).GetChild(0).GetChild(9).gameObject.SetActive(false);

                            observingTableContent.transform.GetChild(i).GetChild(1).GetChild(8).gameObject.SetActive(true);
                            observingTableContent.transform.GetChild(i).GetChild(1).GetChild(9).gameObject.SetActive(false);
                        }
                        else
                        {
                            observingTableContent.transform.GetChild(i).GetChild(0).GetChild(8).gameObject.SetActive(false);
                            observingTableContent.transform.GetChild(i).GetChild(0).GetChild(9).gameObject.SetActive(true);

                            observingTableContent.transform.GetChild(i).GetChild(1).GetChild(8).gameObject.SetActive(false);
                            observingTableContent.transform.GetChild(i).GetChild(1).GetChild(9).gameObject.SetActive(true);
                        }

                        for (int j = 0; j < tracePlayerResponse.Observertable[i].playerTime.Length; j++)
                        {
                            if (userName == tracePlayerResponse.Observertable[i].playerTime[j].playerName)
                            {
                                if (!string.IsNullOrEmpty(tracePlayerResponse.Observertable[i].playerTime[j].joinedAt.ToString()))
                                {
                                    DateTime date = new DateTime(Convert.ToInt64(tracePlayerResponse.Observertable[i].playerTime[j].joinedAt.ToString()));
                                    string _joiningTime = date.ToString("hh:mm");
                                    
                                    observingTableContent.transform.GetChild(i).GetChild(1).GetChild(7).GetChild(1).GetComponent<Text>().text = _joiningTime;
                                }
                                else
                                {
                                    observingTableContent.transform.GetChild(i).GetChild(1).GetChild(7).GetChild(1).GetComponent<Text>().text = string.Empty;
                                }
                            }
                        }
                        
                        observingTableContent.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    //CancelInvoke("CalculateTime1");
                    //CalculateTime1();
                }

                else
                {
                    ResetObservingTableObj();
                    observingTableContent.transform.GetChild(0).gameObject.SetActive(false);
                }

                CancelInvoke("CalculateTime1");
                CancelInvoke("CalculateTime");
            }
        }
    }

    public void CalculateTime()
    {
        for (int i = 0; i < tracePlayerContent.transform.childCount; i++)
        {
            if (!isPlayingTime)
            {
                if (tracePlayerContent.transform.GetChild(i).gameObject.activeInHierarchy)
                {
                    if (!string.IsNullOrEmpty(tracePlayerContent.transform.GetChild(i).GetChild(1).GetChild(7).GetChild(1).GetComponent<Text>().text))
                    {
                        oldHour.Add(Convert.ToInt32(tracePlayerContent.transform.GetChild(i).GetChild(1).GetChild(7).GetChild(1).GetComponent<Text>().text.Substring(0, 2)));
                        oldMin.Add(Convert.ToInt32(tracePlayerContent.transform.GetChild(i).GetChild(1).GetChild(7).GetChild(1).GetComponent<Text>().text.Substring(3, 2)));

                        tracePlayerContent.transform.GetChild(i).GetChild(1).GetChild(7).GetChild(1).GetComponent<Text>().text = Substract(DateTime.Now, oldHour[i], oldMin[i], 0);
                    }
                }

            }
            else
            {
                if (tracePlayerContent.transform.GetChild(i).gameObject.activeInHierarchy)
                {
                    if (!string.IsNullOrEmpty(tracePlayerContent.transform.GetChild(i).GetChild(1).GetChild(7).GetChild(1).GetComponent<Text>().text))
                    {
                        tracePlayerContent.transform.GetChild(i).GetChild(1).GetChild(7).GetChild(1).GetComponent<Text>().text = Substract(DateTime.Now, oldHour[i], oldMin[i], 0);
                    }
                }
            }
        }
        isPlayingTime = true;
        if (tracePlayerObj.activeInHierarchy)
        {
            InvokeRepeating("CalculateTime", 60f, 60f);
        }
    }

    bool isPlayingTime1 = false;

    public void CalculateTime1()
    {
        for (int i = 0; i < observingTableContent.transform.childCount; i++)
        {
            if (!isPlayingTime1)
            {
                if (observingTableContent.transform.GetChild(i).gameObject.activeInHierarchy)
                {
                    if (!string.IsNullOrEmpty(observingTableContent.transform.GetChild(i).GetChild(1).GetChild(7).GetChild(1).GetComponent<Text>().text))
                    {
                        oldHour1.Add(Convert.ToInt32(observingTableContent.transform.GetChild(i).GetChild(1).GetChild(7).GetChild(1).GetComponent<Text>().text.Substring(0, 2)));
                        oldMin1.Add(Convert.ToInt32(observingTableContent.transform.GetChild(i).GetChild(1).GetChild(7).GetChild(1).GetComponent<Text>().text.Substring(3, 2)));

                        observingTableContent.transform.GetChild(i).GetChild(1).GetChild(7).GetChild(1).GetComponent<Text>().text = Substract(DateTime.Now, oldHour1[i], oldMin1[i], 0);
                    }
                }

            }
            else
            {
                if (observingTableContent.transform.GetChild(i).gameObject.activeInHierarchy)
                {
                    if (!string.IsNullOrEmpty(observingTableContent.transform.GetChild(i).GetChild(1).GetChild(7).GetChild(1).GetComponent<Text>().text))
                    {
                        observingTableContent.transform.GetChild(i).GetChild(1).GetChild(7).GetChild(1).GetComponent<Text>().text = Substract(DateTime.Now, oldHour1[i], oldMin1[i], 0);
                    }
                }
            }
        }
        isPlayingTime1 = true;
        if (tracePlayerObj.activeInHierarchy)
        {
            InvokeRepeating("CalculateTime1", 60f, 60f);
        }
    }

    public string Substract(DateTime now, int hours, int minutes, int seconds)
    {
        TimeSpan T1 = new TimeSpan(hours, minutes, seconds);

        DateTime dateTime = now.Subtract(T1);
        string str = dateTime.ToString("HH:mm");
        return str;
    }


    public List<GameObject> tracePlayerGeneratedObj;
    void GenerateTracePlayerObj()
    {
        scrollItemObj = Instantiate(tracePlayerPanel);
        scrollItemObj.transform.SetParent(tracePlayerContent.transform, false);
        tracePlayerGeneratedObj.Add(scrollItemObj);
    }

    void ResetTracePlayerObj()
    {
        if (tracePlayerGeneratedObj.Count > 0)
        {
            for (int i = 0; i < tracePlayerGeneratedObj.Count; i++)
            {
                Destroy(tracePlayerGeneratedObj[i]);
            }
            tracePlayerGeneratedObj.Clear();
            tracePlayerObjCount = 1;
        }
    }

    public List<GameObject> observingTableGeneratedObj;
    int observingTableObjCount;
    void GenerateObservingTableObj()
    {
        scrollItemObj = Instantiate(observingTablePanel);
        scrollItemObj.transform.SetParent(observingTableContent.transform, false);
        observingTableGeneratedObj.Add(scrollItemObj);
    }

    void ResetObservingTableObj()
    {
        if (observingTableGeneratedObj.Count > 0)
        {
            for (int i = 0; i < observingTableGeneratedObj.Count; i++)
            {
                Destroy(observingTableGeneratedObj[i]);
            }
            observingTableGeneratedObj.Clear();
            observingTableObjCount = 1;
        }
    }

    #endregion

    #endregion
}
