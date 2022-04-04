using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllButtons : MonoBehaviour
{
    public static AllButtons instance;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartTable(GameObject startButton)
    {
       // print("Workinhg ");
        PokerNetworkManager.instance.StartTablePrepareGameEmitter();
        startButton.SetActive(false);
    }

    public void ManualTopUpButton()
    {
        if (Table.instance.table.status != 0)
        {
            PokerNetworkManager.instance.ManualTopUpEmitter();
        }
    }

    public void DisbandTableButton()
    {
        if (Table.instance.table.status != 0)
        {
            UIManagerScript.instance.disbandPanel.gameObject.SetActive(true);
            UIManagerScript.instance.tableMenuPanel.gameObject.SetActive(false);
        }
    }

    public void AcceptDeclineBuyInAuthRequest(Transform requestObj, bool accept)
    {
        GameSerializeClassesCollection.instance.buyInAuthAction.username = requestObj.GetChild(1).GetComponent<Text>().text.Replace(" ", string.Empty);
        GameSerializeClassesCollection.instance.buyInAuthAction.ticket_id = GameSerializeClassesCollection.instance.observeTable.ticket;
        GameSerializeClassesCollection.instance.buyInAuthAction.is_approved = accept;
        GameSerializeClassesCollection.instance.buyInAuthAction.token = GameSerializeClassesCollection.instance.observeTable.token;
       // GameSerializeClassesCollection.instance.buyInAuthAction.is_approved_all = false;



        print("Buy INnnnnn");
        PokerNetworkManager.instance.BuyInAuthActionEmitter(false);
        requestObj.gameObject.SetActive(false);
    }


    public void AcceptDeclineAllBuyInAuthRequest(bool isApprovedAll)
    {
       // GameSerializeClassesCollection.instance.buyInAuthAction.username = "";
        GameSerializeClassesCollection.instance.buyInAuthActionAcceptAll.ticket_id = GameSerializeClassesCollection.instance.observeTable.ticket;
        GameSerializeClassesCollection.instance.buyInAuthActionAcceptAll.token = GameSerializeClassesCollection.instance.observeTable.token;
      //  GameSerializeClassesCollection.instance.buyInAuthAction.is_approved = false;
        GameSerializeClassesCollection.instance.buyInAuthActionAcceptAll.is_approved_all = isApprovedAll;



        print("Buy INnnnnn ALL");
        PokerNetworkManager.instance.BuyInAuthActionEmitter(true);

        for (int i = 0; i < MailBoxScripts._instance.video_Nonvideo_Object[1].transform.childCount; i++)
        {
            MailBoxScripts._instance.video_Nonvideo_Object[1].transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < MailBoxScripts._instance.video_Nonvideo_Object[4].transform.childCount; i++)
        {
            MailBoxScripts._instance.video_Nonvideo_Object[4].transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void MississipieStraddle()
    {
        bool isStraddle;
        if (UIManagerScript.instance.straddlePanel.transform.GetChild(2).GetChild(1).GetChild(0).gameObject.activeInHierarchy)
        {
            isStraddle = false;
        }
        else
        {
            isStraddle = true;
        }

        if(isStraddle)
        {
            UIManagerScript.instance.straddlePanelSymbol.transform.GetChild(1).gameObject.SetActive(true);
            UIManagerScript.instance.straddlePanelSymbol.transform.GetChild(0).gameObject.SetActive(false);
        }

        else
        {
            UIManagerScript.instance.straddlePanelSymbol.transform.GetChild(1).gameObject.SetActive(false);
            UIManagerScript.instance.straddlePanelSymbol.transform.GetChild(0).gameObject.SetActive(true);
        }

        GameSerializeClassesCollection.instance.straddle.is_straddle = isStraddle;
        GameSerializeClassesCollection.instance.straddle.straddle_steps = UIManagerScript.instance.straddlePanel.transform.GetChild(4).GetComponent<Slider>().value.ToString();
        UIManagerScript.instance.straddlePanel.transform.parent.gameObject.SetActive(false);
        PokerNetworkManager.instance.StraddleEmitter();
    }
}
