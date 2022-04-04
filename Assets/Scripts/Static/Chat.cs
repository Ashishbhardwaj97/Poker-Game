using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    public static Chat instance;

    [Header("GameObject Reference")]
    public GameObject localPlayerMessageInfo;
    public GameObject remotePlayerMessageInfo;
    public GameObject chatPanel;
    public GameObject chatScrollview;
    public GameObject chatBtn;
    public GameObject chatCloseBtn;

    [Space]
    public Transform chatContent;
    public TMP_Text inputfieldText;
    public TMP_InputField inputfield;

    public string localPlayerMessage;
    public string localPlayerName;
    public bool isFocus;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
    }

    void Update()
    {
        if (inputfield.isFocused == true && isFocus)
        {
            //print("Update");
            //chatPanel.transform.GetChild(6).gameObject.SetActive(false);
            chatPanel.transform.localPosition = new Vector3(transform.localPosition.x, 300, 0);
            chatCloseBtn.SetActive(true);
            isFocus = false;
        }

        if(Input.GetKey(KeyCode.Escape))
        {
            chatPanel.transform.localPosition = new Vector3(transform.localPosition.x, -230, 0);
        }
    }

    public void OpenAndUpdateChat()
    {
        print("OpenChat");
        chatBtn.transform.GetChild(0).gameObject.SetActive(false);
        DestroyChat();
        FindLocalPlayer();
        if (GameManagerScript.instance.isTournament)
        {
            TournamentManagerScript.instance.ChatHistoryEmitter();
        }
        else
        {
            PokerNetworkManager.instance.ChatHistoryEmitter();
        }
        chatPanel.SetActive(true);
        chatCloseBtn.SetActive(true);
        chatBtn.SetActive(false);
        isFocus = true;
    }

    public void CloseChat()
    {
        chatPanel.transform.GetChild(5).gameObject.SetActive(false);
        chatCloseBtn.SetActive(false);
        chatPanel.SetActive(false);
        chatBtn.SetActive(true);
        chatPanel.transform.localPosition = new Vector3(transform.localPosition.x, -230, 0);
        isFocus = false;
        //chatPanel.transform.GetChild(6).gameObject.SetActive(true);
    }
   
    IEnumerator UpdateScrollBar()
    {
        yield return new WaitForSeconds(0.2f);
        chatScrollview.GetComponent<ScrollRect>().verticalNormalizedPosition = 0.0f;
        print(chatScrollview.GetComponent<ScrollRect>().verticalNormalizedPosition);
    }

    public void CreateChatForLocalUser()
    {
        if (inputfieldText.text.Length <= 1)
        {
            print("Empty Text");
        }
        else if(string.IsNullOrWhiteSpace(inputfield.text))
        {
            print("Spaces in Text");
        }
        else
        {
            print("Text not empty");
            FindLocalPlayer();
            localPlayerMessageInfo.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = localPlayerName;
            //localPlayerMessageInfo.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute;
            localPlayerMessage = inputfieldText.text;
            localPlayerMessageInfo.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = localPlayerMessage;
            GameObject gameObject = Instantiate(localPlayerMessageInfo, chatContent);
            gameObject.transform.GetChild(0).localPosition = new Vector3(20, 0, 0);
            StartCoroutine(UpdateScrollBar());

            inputfield.text = "";
            if (GameManagerScript.instance.isTournament)
            {
                TournamentManagerScript.instance.ChatEmitter(localPlayerMessage);
            }
            else
            {
                PokerNetworkManager.instance.ChatEmitter(localPlayerMessage);
            }
        }
    }

    public void FindLocalPlayer()
    {
        for (int i = 0; i < GameManagerScript.instance.playersParent.transform.childCount; i++)
        {
            if (GameManagerScript.instance.playersParent.transform.GetChild(i).childCount == 2)
            {
                if (GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).GetComponent<PokerPlayerController>().isLocalPlayer)
                {
                    localPlayerName = GameManagerScript.instance.playersParent.transform.GetChild(i).GetChild(0).gameObject.name;
                }
            }
        }
    }

    public void CreateChatForRemoteUser()
    {
        if (chatPanel.activeInHierarchy)
        {
            if (GameManagerScript.instance.isTournament)
            {
                //remotePlayerMessageInfo.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute;
                remotePlayerMessageInfo.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = GameSerializeClassesCollection.instance.tourneyChatListener.playerName;
                remotePlayerMessageInfo.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = GameSerializeClassesCollection.instance.tourneyChatListener.message;
                GameObject gameObject = Instantiate(remotePlayerMessageInfo, chatContent);
                gameObject.transform.GetChild(0).localPosition = new Vector3(-20, 0, 0);
            }
            else
            {
                //remotePlayerMessageInfo.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute;
                remotePlayerMessageInfo.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = GameSerializeClassesCollection.instance.chatListener.playerName;
                remotePlayerMessageInfo.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = GameSerializeClassesCollection.instance.chatListener.message;
                GameObject gameObject = Instantiate(remotePlayerMessageInfo, chatContent);
                gameObject.transform.GetChild(0).localPosition = new Vector3(-20, 0, 0);
            }
            StartCoroutine(UpdateScrollBar());
        }

        else
        {
            chatBtn.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void CreateChatHistory()
    {
        if (GameManagerScript.instance.isTournament)
        {
            for (int i = 0; i < GameSerializeClassesCollection.instance.tourneyChatHistoryListener.chat_message.Length; i++)
            {
                if (GameSerializeClassesCollection.instance.tourneyChatHistoryListener.chat_message[i].playerName == localPlayerName)
                {
                    localPlayerMessageInfo.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = GameSerializeClassesCollection.instance.tourneyChatHistoryListener.chat_message[i].playerName;
                    localPlayerMessageInfo.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = GameSerializeClassesCollection.instance.tourneyChatHistoryListener.chat_message[i].message;
                    GameObject gameObject = Instantiate(localPlayerMessageInfo, chatContent);
                    gameObject.transform.GetChild(0).localPosition = new Vector3(20, 0, 0);
                }
                else
                {
                    remotePlayerMessageInfo.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = GameSerializeClassesCollection.instance.tourneyChatHistoryListener.chat_message[i].playerName;
                    remotePlayerMessageInfo.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = GameSerializeClassesCollection.instance.tourneyChatHistoryListener.chat_message[i].message;
                    GameObject gameObject = Instantiate(remotePlayerMessageInfo, chatContent);
                    gameObject.transform.GetChild(0).localPosition = new Vector3(-20, 0, 0);
                }
            }
        }
        else
        {
            //print("Length: " + GameSerializeClassesCollection.instance.chatHistoryListener.chat_message.Length);
            for (int i = 0; i < GameSerializeClassesCollection.instance.chatHistoryListener.chat_message.Length; i++)
            {
                if (GameSerializeClassesCollection.instance.chatHistoryListener.chat_message[i].playerName == localPlayerName)
                {
                    localPlayerMessageInfo.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = GameSerializeClassesCollection.instance.chatHistoryListener.chat_message[i].playerName;
                    localPlayerMessageInfo.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = GameSerializeClassesCollection.instance.chatHistoryListener.chat_message[i].message;
                    GameObject gameObject = Instantiate(localPlayerMessageInfo, chatContent);
                    gameObject.transform.GetChild(0).localPosition = new Vector3(20, 0, 0);
                }
                else
                {
                    remotePlayerMessageInfo.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = GameSerializeClassesCollection.instance.chatHistoryListener.chat_message[i].playerName;
                    remotePlayerMessageInfo.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = GameSerializeClassesCollection.instance.chatHistoryListener.chat_message[i].message;
                    GameObject gameObject = Instantiate(remotePlayerMessageInfo, chatContent);
                    gameObject.transform.GetChild(0).localPosition = new Vector3(-20, 0, 0);
                }
            }
        }
        StartCoroutine(UpdateScrollBar());
    }

    public void DestroyChat()
    {
        for (int i = 0; i < chatContent.childCount; i++)
        {
            Destroy(chatContent.transform.GetChild(i).gameObject);
        }
    }

    public void InbuildChatMessages(Text msg)
    {
        inputfield.text = "";

        chatPanel.transform.GetChild(5).gameObject.SetActive(false);
        inputfield.text = msg.text;
    }
}