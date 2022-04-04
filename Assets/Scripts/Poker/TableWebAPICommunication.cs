using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;



public class TableWebAPICommunication : MonoBehaviour
{
    public static TableWebAPICommunication instance;
    public string playerToken;
    //public string showTableStartURL, currentObserverListUrl;



    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {



        StartCoroutine(InitializeURLs());
        StartGetObserverCoroutine();

        Invoke("StopGetObserverCoroutine", 5);


    }

    // Update is called once per frame
    void Update()
    {

    }

    //IEnumerator InitializeURLs()
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    showTableStartURL = ServerChanger.instance.domainURL + "api/v1/pokertable/is-creater-board";
    //    currentObserverListUrl = ServerChanger.instance.domainURL + "api/v1/regulartable/current-observer-list";

    //}

    IEnumerator InitializeURLs()
    {
        yield return new WaitForSeconds(0.5f);
        //showTableStartURL = ServerChanger.instance.domainURL + "api/v1/pokertable/is-creater-board";
        //currentObserverListUrl = ServerChanger.instance.domainURL + "api/v1/pokertable/current-observer-list";

    }

    public void PostData(string url, string body, System.Action<string> _callback)
    {
        StartCoroutine(PostDataCoroutine(url, body, _callback));

    }


    string response = "";
    public IEnumerator PostDataCoroutine(string url, string body, System.Action<string> _callback)
    {
        UnityWebRequest wwwWebRequest = UnityWebRequest.Post(url, body);

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(body);
        wwwWebRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        wwwWebRequest.SetRequestHeader("Content-Type", "application/json");
        if (playerToken != "")
        {
            //print("TOKENNNN " + playerToken);
            wwwWebRequest.SetRequestHeader("Authorization", "JWT " + playerToken);
        }

        yield return wwwWebRequest.SendWebRequest();

        //Debug.Log(": Execute :");

        if (wwwWebRequest.isDone)
        {
            //Debug.Log("success");
            response = wwwWebRequest.downloadHandler.text;

            print("response..........." + response);
            _callback(response);
        }
        else
        {
            Debug.Log(": Error: " + wwwWebRequest.error);
            yield break;
        }
    }

   // #region Trace Player Observer table list



    [Serializable]
    class CurrentObserverResponse
    {
        public bool error;
        public Observers[] observers;

    }

    [Serializable]
    class Observers
    {
        public CurrentObservers[] current_observers;
    }

    [Serializable]
    class CurrentObservers
    {
        public string playerName;

    }

    [SerializeField] CurrentObserverResponse currentObserverResponse;

    public void ClickCurrentObserverRequest(string tableId)
    {

        GameSerializeClassesCollection.instance.tableID.table_id = tableId;
        string body = JsonUtility.ToJson(GameSerializeClassesCollection.instance.tableID);
        print(body);
        // ClubManagement.instance.loadingPanel.SetActive(true);
        //if (!string.IsNullOrEmpty(currentObserverListUrl))
        //{
        //    PostData(currentObserverListUrl, body, CurrentObserverProcess);
        //}
    }

    void CurrentObserverProcess(string response)
    {
        //ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("Some error ...!");
        }
        else
        {
            //print("response" + response);

            currentObserverResponse = JsonUtility.FromJson<CurrentObserverResponse>(response);
            GameManagerScript.instance.observerNumber[0].text = "0";
            GameManagerScript.instance.observerNumber[1].text = "0";
            if (!currentObserverResponse.error)
            {
                if (currentObserverResponse.observers[0].current_observers.Length > 0)
                {
                    DeleteObserverUI();

                    for (int i = 0; i < currentObserverResponse.observers[0].current_observers.Length; i++)
                    {
                        GameManagerScript.instance.UpdateObserver(currentObserverResponse.observers[0].current_observers[i].playerName);

                    }
                }
                GameManagerScript.instance.observerNumber[0].text = "" + currentObserverResponse.observers[0].current_observers.Length;
                GameManagerScript.instance.observerNumber[1].text = "" + currentObserverResponse.observers[0].current_observers.Length;

            }
            else
            {
                DeleteObserverUI();
            }
        }
    }


    public void StartGetObserverCoroutine()
    {
        StartCoroutine("GetObserver");

    }


    public void StopGetObserverCoroutine()
    {
        StopCoroutine("GetObserver");
        DeleteObserverUI();
    }

    IEnumerator GetObserver()
    {
        yield return null;
        ClickCurrentObserverRequest(GameSerializeClassesCollection.instance.observeTable.ticket);
            
    }

    void DeleteObserverUI()
    {
        for (int i = 0; i < GameManagerScript.instance.observerContent.transform.childCount; i++)
        {
            Destroy(GameManagerScript.instance.observerContent.transform.GetChild(i).gameObject);

        }
    }
}
