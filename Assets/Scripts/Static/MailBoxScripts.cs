using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class MailBoxScripts : MonoBehaviour
{
    public static MailBoxScripts _instance;

    [Header("GameObject Reference")]
    public GameObject MailBoxMain;
    public GameObject MailBoxAuth_Y_N;
    public GameObject composeMsgUI;
    public GameObject mailBoxContent;
    public GameObject mailBoxPanel;
    public GameObject buyinContent;
    public GameObject buyinPanel;
    public GameObject BuyInMain;
    public GameObject buyinAuthKeyButtonNonVideo;
    public GameObject buyinAuthKeyButtonVideo;
    public GameObject buyInNotificationVideo;
    public GameObject buyInNotificationNonVideo;
    public GameObject memberScrollContent;
    public GameObject memberPanel;
    public GameObject inboxScrollList;
    public GameObject outboxScrollList;
    public GameObject outboxPopup;

    [Header("Text Reference")]
    public Text usernameOutboxPopup;
    public Text messageOutboxPopup;
    public Text dateOutboxPopup;

    [Header("Transform Reference")]
    public Transform outboxContent;
    public Transform outboxPanel;

    [Header("GameObject List Reference")]
    public List<GameObject> video_Nonvideo_Object;
    public List<GameObject> highlightMsgBgList;
    public List<GameObject> highlightInboxOutboxUI;
    public List<GameObject> highlightFiltersUI;

    private GameObject currentMsgObj;
    private GameObject scrollItemObj;
    private int outboxCount;
    //private string outboxUrl;

    [Header("Text References")]
    public Text mailCount;

    [Header("InputField References")]
    public InputField composeMsgInputField;

    //private string mailboxUrl;
    //private string mailboxCountUrl;
    //private string mailboxUpdatedUrl;
    //private string mailboxCloseButtonUrl;
    //private string mailbox_Accept_decline_Url;
    //private string buyInAuthTableUrl;
    //private string composeMsgUrl;
    //private string composeMemberListUrl;

    private string reciever_id;
    private string sender_id;
    private string ruled_id;

    private int mailboxCount;
    private int mailBoxCount;
    private int buyinCount;
    private int no;

    private int memberConut;
   // private int j = 0;
    private int memberIndex = 0;
    
    [Serializable]
    public class MailBoxResponse
    {
        public bool error;
        public int count;
        public ComposeMessageList[] composeMessageList;
    }
    [Serializable]
    public class ComposeMessageList
    {
        public string createdAt;
        public string subject;
        public string message;
        public bool is_read;
        public string _id;
        public string receiver_user_id;
        public string sender_user_id;
        public string rule_id;
    }

    [Serializable]
    public class MailboxcloseRequest
    {
        public string id;

    }

    [Serializable]
    public class Errors
    {
        public string message;
    }

    [Serializable]
    public class BuyInResponse
    {
        public bool error;

        public BuyInData[] data;
    }

    [Serializable]
    public class BuyInData
    {
        public string playerName;
        public string clientId;
        public string countryFlag;
        public string isAuthorised;
        public string chipBalance;

    }

    [Serializable]
    public class BuyInRequest
    {
        public string table_id;
        public string receiver_user_id;
        public string sender_user_id;
        public string status;
        public string rule_id;
    }

    [Serializable]
    public class ComposeMsgReq
    {
        public List<MembersData> create;
    }

    [Serializable]
    public class ComposeMsgRes
    {
        public bool error;
    }

    [Serializable]
    public class MembersData
    {
        public string receiver_user_id;
        public string message;
        public string receiver_username;
        public string club_id;
        public int message_type;
        public string sender_user_id;
    }

    [Serializable]
    public class MemberList
    {
        public int type;
        public string club_id;
        public string agent_id;
    }

    [Header("List Properties")]

    public List<GameObject> mailBoxGeneratedItemList;
    public List<GameObject> buyinGeneratedItemList;
    public List<GameObject> memList;
    public List<GameObject> outboxItemList;

    [SerializeField] BuyInResponse buyInResponse;
    [SerializeField] BuyInRequest buyInRequest;  
    [SerializeField] MailboxcloseRequest mailboxRequest;
    [SerializeField] MailBoxResponse mailBoxResponse;
    [SerializeField] MemberList memberList;
    [SerializeField] ClubManagement.GetMemberList getMemberList;
    [SerializeField] ComposeMsgReq composeMsgReq;
    [SerializeField] ComposeMsgRes composeMsgRes;

    void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        //mailboxUrl = ServerChanger.instance.domainURL + "api/v1/club/compose-message-list";
        //mailbox_Accept_decline_Url = ServerChanger.instance.domainURL + "api/v1/pokertable/invite-accept-decline";
        //mailboxCountUrl = ServerChanger.instance.domainURL + "api/v1/club/compose-message-count";
        //mailboxUpdatedUrl = ServerChanger.instance.domainURL + "api/v1/club/update-compose-message";
        //mailboxCloseButtonUrl = ServerChanger.instance.domainURL + "api/v1/club/delete-compose-message";
        //buyInAuthTableUrl = ServerChanger.instance.domainURL + "api/v1/pokertable/buyin-authorization-list";
        //composeMsgUrl = ServerChanger.instance.domainURL + "api/v1/club/create-compose-message";
        //composeMemberListUrl = ServerChanger.instance.domainURL + "api/v1/club/compose-club-role-list";
        //outboxUrl = ServerChanger.instance.domainURL + "api/v1/club/compose-message-outbox";

        mailBoxCount = 1;        
        buyinCount = 1;
        memberConut = 1;
        outboxCount = 1;

        mailBoxGeneratedItemList = new List<GameObject>();
        buyinGeneratedItemList = new List<GameObject>();
    }

    public void MailBoxCall()
    {
        if (mailboxCount > 0)
        {
            for (int i = 0; i < ApiHitScript.instance.mailBoxSymbol.Count; i++)
            {
                ApiHitScript.instance.mailBoxSymbol[i].SetActive(true);
            }
        }
    }

    public void MailboxShow()
    {
        MailBoxMain.SetActive(true);
        mailboxCount = 0;
        mailCount.text = mailBoxCount.ToString();

        for (int i = 0; i < ApiHitScript.instance.mailBoxSymbol.Count; i++)
        {
            ApiHitScript.instance.mailBoxSymbol[i].SetActive(false);
        }

        MailBoxList();
        OutboxRequest();
        ClickInboxBtn();
        MailBoxUpdate();
        if (ClubManagement.instance.isClubDetailPage)
        {
            composeMsgUI.SetActive(true);
        }
        else
        {
            composeMsgUI.SetActive(false);
        }
    }

    public void MailboxClose()
    {
        MailBoxMain.SetActive(false);
        ResetGenerateMailBoxItem();
       
    }

    public void MailBoxCloseButton(Text id)
    {
        currentMsgObj = id.transform.parent.gameObject;
        ClubManagement.instance.loadingPanel.SetActive(true);
        mailboxRequest.id = id.text;

        string body = JsonUtility.ToJson(mailboxRequest);
        print(body);
        //Communication.instance.PostData(mailboxCloseButtonUrl, body, MailboxCloseButtonCallback);
    }

    void MailboxCloseButtonCallback(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        print("response mail box" + response);
        mailBoxResponse = JsonUtility.FromJson<MailBoxResponse>(response);

        if (!mailBoxResponse.error)
        {
            print("Mail box close button correct...");
            
            currentMsgObj.gameObject.SetActive(false);
            currentMsgObj.transform.SetParent(transform);
            currentMsgObj.transform.SetParent(mailBoxContent.transform);

        }
        else
        {
            print("Mail box close button incorrect...");

        }
    }

    public void MailBoxUpdate()
    {
        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(mailboxUpdatedUrl, MailboxUpdateCallback);
    }

    void MailboxUpdateCallback(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        print("response mail box" + response);
        mailBoxResponse = JsonUtility.FromJson<MailBoxResponse>(response);

        if (!mailBoxResponse.error)
        {
            print("Mail box updated correct...");

        }
        else
        {
            print("Mail box updated incorrect...");

        }
    }

    public void MailBoxCallCount()
    {
        //if (!string.IsNullOrEmpty(mailboxCountUrl))
        //{
        //    ClubManagement.instance.loadingPanel.SetActive(true);
        //    //Communication.instance.PostData(mailboxCountUrl, MailboxCountCallback);
        //}
    }

    void MailboxCountCallback(string response)
    {

        ClubManagement.instance.loadingPanel.SetActive(false);
        print("response mail box count" + response);
        if (!string.IsNullOrEmpty(response))
        {
            mailBoxResponse = JsonUtility.FromJson<MailBoxResponse>(response);

            if (!mailBoxResponse.error)
            {
                print("Mail box count correct...");

                mailCount.text = mailBoxResponse.count.ToString();
                mailboxCount = mailBoxResponse.count;

                for (int i = 0; i < ApiHitScript.instance.mailBoxSymbol.Count; i++)
                {
                    ApiHitScript.instance.mailBoxSymbol[i].transform.GetChild(0).GetComponent<Text>().text = mailBoxResponse.count.ToString();
                }

                MailBoxCall();
            }
            else
            {
                print("Mail box incorrect...");

            }
        }
    }

    public void MailBoxList()
    {
        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(mailboxUrl, MailboxListCall);
    }

    void MailboxListCall(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("Some error in login! Please try after some time.");
        }
        else
        {
            print("response mail box" + response);

            mailBoxResponse = JsonUtility.FromJson<MailBoxResponse>(response);

            if (!mailBoxResponse.error)
            {
                print("MailboxListCall correct...");

                if (mailBoxResponse.composeMessageList.Length != mailBoxCount)
                {
                    for (int i = mailBoxCount; i < mailBoxResponse.composeMessageList.Length; i++)
                    {
                        mailBoxCount++;
                        GenerateMailBoxItem();
                    }
                }

                for (int i = 0; i < mailBoxResponse.composeMessageList.Length; i++)
                {
                    mailBoxContent.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = mailBoxResponse.composeMessageList[i].subject;
                    mailBoxContent.transform.GetChild(i).GetChild(2).GetComponent<Text>().text = mailBoxResponse.composeMessageList[i].message;

                    string _date = mailBoxResponse.composeMessageList[i].createdAt;
                    
                    mailBoxContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text = ConvertDateTime(_date);
                    mailBoxContent.transform.GetChild(i).GetChild(4).GetComponent<Text>().text = mailBoxResponse.composeMessageList[i]._id;
                    mailBoxContent.transform.GetChild(i).GetChild(5).GetComponent<Text>().text = mailBoxResponse.composeMessageList[i].receiver_user_id;
                    mailBoxContent.transform.GetChild(i).GetChild(6).GetComponent<Text>().text = mailBoxResponse.composeMessageList[i].sender_user_id;
                    mailBoxContent.transform.GetChild(i).GetChild(7).GetComponent<Text>().text = mailBoxResponse.composeMessageList[i].rule_id;
                    
                    mailBoxContent.transform.GetChild(i).gameObject.SetActive(true);
                }

            }
            
        }        
    }

    void GenerateMailBoxItem()
    {
        GameObject scrollItemObj = Instantiate(mailBoxPanel);
        scrollItemObj.transform.SetParent(mailBoxContent.transform, false);

        mailBoxGeneratedItemList.Add(scrollItemObj);
    }

    void ResetGenerateMailBoxItem()
    {
        if (mailBoxGeneratedItemList.Count > 0)
        {
            for (int i = 0; i < mailBoxGeneratedItemList.Count; i++)
            {
                Destroy(mailBoxGeneratedItemList[i]);
            }
            mailBoxGeneratedItemList.Clear();
            mailBoxCount = 1;
        }
    }

    public string ConvertDateTime(string jsonDate)
    {
        var dateStr = DateTime.Parse(jsonDate);
        string str = dateStr.ToString("MM/dd/yyyy hh:mm tt");
        return str;
    }

    public void BuyInTable()
    {
        ClubManagement.instance.loadingPanel.SetActive(true);
        buyInRequest.table_id = GameSerializeClassesCollection.instance.observeTable.ticket;
       
        string body = JsonUtility.ToJson(buyInRequest);
        print("BuyIn-Table body" + body);
        //Communication.instance.PostData(buyInAuthTableUrl, body, BuyInAuthTableCallback);
    }

    void BuyInAuthTableCallback(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        BuyInResponseProcess(response, true);

    }

    public void BuyInResponseProcess(string response, bool activeBuyInMain)
    {

        if (!string.IsNullOrEmpty(response))
        {
            print("response BuyIn-Table-----" + response);
            buyInResponse = JsonUtility.FromJson<BuyInResponse>(response);

            if (!buyInResponse.error)
            {
                print("BuyIn-Table correct...");

                if (buyInResponse.data.Length != buyinCount)
                {
                    for (int i = buyinCount; i < buyInResponse.data.Length; i++)
                    {
                        buyinCount++;
                        GenerateBuyInItem();
                    }
                }

                for (int i = 0; i < buyInResponse.data.Length; i++)
                {
                    buyinContent.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = buyInResponse.data[i].playerName + "      ";
                    buyinContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text = buyInResponse.data[i].clientId;
                    buyinContent.transform.GetChild(i).GetChild(4).GetChild(0).GetComponent<Text>().text = buyInResponse.data[i].chipBalance;

                    string flagUrl = buyInResponse.data[i].countryFlag;
                    no = i;

                    if (!string.IsNullOrEmpty(flagUrl))
                    {
                        ClubManagement.instance.loadingPanel.SetActive(true);
                        Communication.instance.GetImage(flagUrl, ContryFlagBuyinProcess);
                    }


                    buyinContent.transform.GetChild(i).GetChild(7).GetComponent<Text>().text = buyInResponse.data[i].isAuthorised;
                    buyinContent.transform.GetChild(i).gameObject.SetActive(true);

                }

                if (activeBuyInMain)
                {
                    BuyInMain.SetActive(true);
                }
                OnBuyInNotification(false);
                
            }
           
        }

    }

    
    void ContryFlagBuyinProcess(Sprite response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (response != null)
        {
            buyinContent.transform.GetChild(no).GetChild(1).GetChild(0).GetComponent<Image>().sprite = response;
        }
    }

    void GenerateBuyInItem()
    {
        GameObject scrollItemObj = Instantiate(buyinPanel);
        scrollItemObj.transform.SetParent(buyinContent.transform, false);

        buyinGeneratedItemList.Add(scrollItemObj);
    }

    void ResetGenerateBuyInItem()
    {
        if (buyinGeneratedItemList.Count > 0)
        {
            for (int i = 0; i < buyinGeneratedItemList.Count; i++)
            {
                Destroy(buyinGeneratedItemList[i]);
            }
            buyinGeneratedItemList.Clear();
            buyinCount = 1;
        }
    }

    public void BuyInBack()
    {
        BuyInMain.SetActive(false);
        ResetGenerateBuyInItem();
        OnBuyInNotification(false);
    }

    public void OnBuyInNotification( bool isactive)
    {
        buyInNotificationVideo.SetActive(isactive);
        buyInNotificationNonVideo.SetActive(isactive);
    }

    public void BuyInAuthKeyButton(bool isactive)
    {
        buyinAuthKeyButtonVideo.SetActive(isactive);
        buyinAuthKeyButtonNonVideo.SetActive(isactive);

    }
   
    
    public void BuyinAuthMailBox(GameObject subjectname)
    {
        if (subjectname.transform.GetChild(1).GetComponent<Text>().text == "Invitation for Game")
        {
            print("invitation for a Game");

            reciever_id = subjectname.transform.GetChild(5).GetComponent<Text>().text;
            sender_id = subjectname.transform.GetChild(6).GetComponent<Text>().text;
            ruled_id = subjectname.transform.GetChild(7).GetComponent<Text>().text;
            MailBoxAuth_Y_N.SetActive(true);
        }
        else
        {
            print("Non of above...");
        }
    }

    public void BuyinAuthMailBox_Y_N(int _statusNo)
    {
        ClubManagement.instance.loadingPanel.SetActive(true);

        buyInRequest.receiver_user_id = reciever_id;
        buyInRequest.sender_user_id = sender_id;
        buyInRequest.status = _statusNo.ToString();
        buyInRequest.rule_id = ruled_id.ToString();

        string body = JsonUtility.ToJson(buyInRequest);

        //Communication.instance.PostData(mailbox_Accept_decline_Url, body, BuyinAuthMailBox_Y_N_Callback);
    }

    void BuyinAuthMailBox_Y_N_Callback(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        print("BuyinAuthMailBox_Y_N" + response);

        mailBoxResponse = JsonUtility.FromJson<MailBoxResponse>(response);

        if (!mailBoxResponse.error)
        {
            print("BuyinAuthMailBox_Y_N correct...");
            MailBoxAuth_Y_N.SetActive(false);
        }
        else
        {
            print("BuyinAuthMailBox_Y_N incorrect...");

        }
    }

    public void BuyInAuthBack()
    {
        MailBoxAuth_Y_N.SetActive(false);
    }

    #region Compose Message

    public void ClickComposeMsg()
    {
        MailBoxMain.transform.GetChild(0).gameObject.SetActive(false);
        MailBoxMain.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void ClickBackBtnFromMemberList()
    {
        MailBoxMain.transform.GetChild(0).gameObject.SetActive(true);
        MailBoxMain.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void ClickBackBtnFromComposeMsg()
    {
        MailBoxMain.transform.GetChild(2).gameObject.SetActive(false);
        MailBoxMain.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void ClickDemoMessage(Transform btn)
    {
        for (int i = 0; i < highlightMsgBgList.Count; i++)
        {
            highlightMsgBgList[i].SetActive(false);
        }
        if (btn.GetChild(0).gameObject.activeInHierarchy)
        {
            btn.GetChild(0).gameObject.SetActive(false);
            composeMsgInputField.textComponent.text = string.Empty;
        }
        else
        {
            btn.GetChild(0).gameObject.SetActive(true);
            print(btn.GetChild(1).GetComponent<Text>().text);
            composeMsgInputField.text = btn.GetChild(1).GetComponent<Text>().text;
        }


    }

    public void ClickMemberSelection(Transform TickBtn)
    {
        if (TickBtn.GetChild(1).gameObject.activeInHierarchy)
        {
            TickBtn.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            TickBtn.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void ClickNextBtn()
    {
        MailBoxMain.transform.GetChild(1).gameObject.SetActive(false);
        MailBoxMain.transform.GetChild(2).gameObject.SetActive(true);

    }

    public void ClickSendBtn()
    {
        memberIndex = 0;
        composeMsgReq.create.Clear();
        for (int i = 0; i < memberScrollContent.transform.childCount; i++)
        {
            if (memberScrollContent.transform.GetChild(i).GetChild(4).GetChild(1).gameObject.activeSelf)
            {
                print("i : " + i);
                print("memberIndex : " + memberIndex);
                composeMsgReq.create.Add(new MembersData());
                composeMsgReq.create[memberIndex].club_id = ClubManagement.instance._clubID;
                composeMsgReq.create[memberIndex].receiver_user_id = memberScrollContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text;
                composeMsgReq.create[memberIndex].receiver_username = memberScrollContent.transform.GetChild(i).GetChild(1).GetComponent<Text>().text;
                composeMsgReq.create[memberIndex].sender_user_id = ClubManagement.instance.selectedClubUserId;
                composeMsgReq.create[memberIndex].message = composeMsgInputField.text;
                composeMsgReq.create[memberIndex].message_type = 1;
                memberIndex = memberIndex + 1;
            }

        }

        string body = JsonUtility.ToJson(composeMsgReq);
        print("ClickNextBtn....." + body);
        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(composeMsgUrl, body, ClickSendBtnProcess);
    }

    void ClickSendBtnProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (!string.IsNullOrEmpty(response))
        {
            print("response" + response);
            composeMsgRes = JsonUtility.FromJson<ComposeMsgRes>(response);

            if (!composeMsgRes.error)
            {
                MailBoxMain.transform.GetChild(2).gameObject.SetActive(false);
                MailBoxMain.transform.GetChild(1).gameObject.SetActive(false);
                MailBoxMain.transform.GetChild(0).gameObject.SetActive(true);
                MailBoxMain.SetActive(false);
            }
        }
        else
        {
            print("Some error ...!");

        }
    }

    public void ClickOnMembersList()
    {
        memberList.club_id = ClubManagement.instance._clubID;        
        memberList.agent_id = ClubManagement.instance.currentSelectedAgentId;

        if (ClubManagement.instance.currentSelectedClubRole == "Player")
        {
            memberList.type = 3;
        }
        else if (ClubManagement.instance.currentSelectedClubRole == "Agent")
        {
            memberList.type = 2;
        }
        else
        {
            memberList.type = 1;
        }

        string body = JsonUtility.ToJson(memberList);
        print(body);
        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(composeMemberListUrl, body, ClickOnMembersListProcess);

    }


    void ClickOnMembersListProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("error");
        }
        else
        {
            print("" + response);
            getMemberList = JsonUtility.FromJson<ClubManagement.GetMemberList>(response);

            if (!getMemberList.error)
            {
                if (getMemberList.data.Length != memberConut)
                {
                    for (int i = memberConut; i < getMemberList.data.Length; i++)
                    {
                        memberConut++;
                        GenerateMembersItem();
                    }
                }

                SearchingScript.instance.memberListNameInComposeMsg.Clear();
                userImage.Clear();
                
                for (int i = 0; i < getMemberList.data.Length; i++)
                {
                    memberScrollContent.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = getMemberList.data[i].username + "      ";
                    memberScrollContent.transform.GetChild(i).GetChild(2).GetComponent<Text>().text = getMemberList.data[i].client_id;
                    memberScrollContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text = getMemberList.data[i].user_id;

                    string _role = getMemberList.data[i].club_member_role;
                    if (!string.IsNullOrEmpty(_role))
                    {
                        if (_role == "Accountant" || _role == "Partner")
                        {
                            memberScrollContent.transform.GetChild(i).GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = _role.Substring(0, 2);
                        }
                        else
                        {
                            memberScrollContent.transform.GetChild(i).GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = _role.Substring(0, 1);
                        }

                    }

                    memberScrollContent.transform.GetChild(i).GetChild(4).GetChild(1).gameObject.SetActive(false);
                    userImage.Add(getMemberList.data[i].user_image);
                    memberScrollContent.transform.GetChild(i).gameObject.SetActive(true);
                    SearchingScript.instance.memberListNameInComposeMsg.Add(memberScrollContent.transform.GetChild(i));
                }

                UpdatePlayerImage();
            }
        }

    }

    #region update Player Image

    public List<string> userImage;

    [SerializeField]
    private List<PlayerImageInSequence> playerImageInSequence;

    private int k = 0;
    private int totalImageCount;
    private int count = 0;

    private int previousCountForMemberList;
    void UpdatePlayerImage()
    {
        if (userImage.Count == previousCountForMemberList)
        {
            return;
        }

        k = 0;
        previousCountForMemberList = 0;
        totalImageCount = 0;
        playerImageInSequence.Clear();

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
        print(k);
        print(totalImageCount);
        if (k == totalImageCount)
        {
            count = 0;
            ClubManagement.instance.loadingPanel.SetActive(false);

            for (int i = 0; i < userImage.Count; i++)
            {
                memberScrollContent.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Registration.instance.defaultPlayerImage.sprite;
            }

            for (int i = 0; i < userImage.Count; i++)
            {
                if (!string.IsNullOrEmpty(userImage[i]))
                {
                    print("i = " + i);
                    memberScrollContent.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = playerImageInSequence[count].imgPic;
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
                _instance.totalImageCount++;
                _instance.ApplyImage();

            }
        }
    }

    public void ResetAllValuesForImage()
    {
        totalImageCount = 0;
        k = 0;
        count = 0;
        previousCountForMemberList = 0;
        userImage.Clear();
        playerImageInSequence.Clear();
    }

    #endregion


    void GenerateMembersItem()
    {
        scrollItemObj = Instantiate(memberPanel);
        scrollItemObj.transform.SetParent(memberScrollContent.transform, false);
        memList.Add(scrollItemObj);
    }

    public void DestroyMemberListObject()
    {
        if (memList.Count > 0)
        {
            for (int i = 0; i < memList.Count; i++)
            {
                Destroy(memList[i]);
            }
            memList.Clear();
            memberConut = 1;

            ResetAllValuesForImage();
        }
    }

    #endregion

    #region Outbox

    [Serializable]
    public class OutboxResponse
    {
        public bool error;
        public Outbox[] outbox;
    }

    [Serializable]
    public class Outbox
    {
        public Receiver[] receiver;
        public string createdAt;
    }

    [Serializable]
    public class Receiver
    {
        public string receiver_username;
        public string message;
    }

    [SerializeField] OutboxResponse outboxResponse;
    public void ClickOutboxBtn()
    {
        outboxScrollList.SetActive(true);
        inboxScrollList.SetActive(false);
        highlightInboxOutboxUI[0].SetActive(false);
        highlightInboxOutboxUI[1].SetActive(true);
    }

    public void ClickInboxBtn()
    {
        outboxScrollList.SetActive(false);
        inboxScrollList.SetActive(true);
        highlightInboxOutboxUI[0].SetActive(true);
        highlightInboxOutboxUI[1].SetActive(false);
    }

    public void ClickOutboxSecondScreen(Transform panel)
    {
        outboxPopup.SetActive(true);

        messageOutboxPopup.text = panel.GetChild(2).GetComponent<Text>().text;
        dateOutboxPopup.text = panel.GetChild(3).GetComponent<Text>().text;
        usernameOutboxPopup.text = panel.GetChild(4).GetComponent<Text>().text;
    }

    void OutboxRequest()
    {
        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(outboxUrl, OutboxRequestProcess);
    }

    void OutboxRequestProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (!string.IsNullOrEmpty(response))
        {
            print("OutboxRequestProcess response" + response);
            outboxResponse = JsonUtility.FromJson<OutboxResponse>(response);

            if (!outboxResponse.error)
            {
                if (outboxResponse.outbox.Length != outboxCount)
                {
                    for (int i = outboxCount; i < outboxResponse.outbox.Length; i++)
                    {
                        outboxCount++;
                        GenerateOutboxtem();
                    }
                }

                for (int i = 0; i < outboxResponse.outbox.Length; i++)
                {
                    outboxContent.GetChild(i).GetChild(1).GetComponent<Text>().text = outboxResponse.outbox[i].receiver[0].receiver_username.Trim() + "....+" + " ";
                    outboxContent.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>().text = (outboxResponse.outbox[i].receiver.Length - 1).ToString();
                    outboxContent.GetChild(i).GetChild(2).GetComponent<Text>().text = outboxResponse.outbox[i].receiver[0].message;
                    string date = outboxResponse.outbox[i].createdAt;
                    outboxContent.GetChild(i).GetChild(3).GetComponent<Text>().text = ConvertDateTime(date);
                    outboxContent.GetChild(i).GetChild(4).GetComponent<Text>().text = string.Empty;

                    for (int j = 0; j < outboxResponse.outbox[i].receiver.Length; j++)
                    {
                        if (j != outboxResponse.outbox[i].receiver.Length - 1)
                        {
                            string name = outboxResponse.outbox[i].receiver[j].receiver_username.Trim();
                            outboxContent.GetChild(i).GetChild(4).GetComponent<Text>().text += name + ", ";
                        }
                        else
                        {
                            string name = outboxResponse.outbox[i].receiver[j].receiver_username.Trim();
                            outboxContent.GetChild(i).GetChild(4).GetComponent<Text>().text += name;
                        }
                    }


                    outboxContent.GetChild(i).gameObject.SetActive(true);

                }
            }
        }

    }


    void GenerateOutboxtem()
    {
        GameObject scrollItemObj = Instantiate(outboxPanel.gameObject);
        scrollItemObj.transform.SetParent(outboxContent, false);

        outboxItemList.Add(scrollItemObj);
    }

    public void DestroyOutBoxItem()
    {
        if (outboxItemList.Count > 0)
        {
            for (int i = 0; i < outboxItemList.Count; i++)
            {
                Destroy(outboxItemList[i]);
            }
            outboxItemList.Clear();
            outboxCount = 1;
        }
    }

    #endregion

    #region Filters In outbox

    public void ClickClubFilter()
    {
        highlightFiltersUI[0].SetActive(true);
        highlightFiltersUI[1].SetActive(false);
    }

    public void ClickSystemFilter()
    {
        highlightFiltersUI[0].SetActive(false);
        highlightFiltersUI[1].SetActive(true);
    }

    #endregion
}
