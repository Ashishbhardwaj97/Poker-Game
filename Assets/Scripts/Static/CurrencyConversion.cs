using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CurrencyConversion : MonoBehaviour
{

    public static CurrencyConversion instance;

    public List<GameObject> chipsAndCurrencyUIList;


    #region Currency Related Data..
    [System.Serializable]
    public class ChipsCurrency
    {

        public bool error;
        public List<CurrencyData> data;
        public string message;
        public float statusCode;


    }
    [System.Serializable]
    public class CurrencyData
    {
        public bool status;
        public string _id;
        public float chips;
        public List<ChipsPrice> chips_price;
        public string createdAt;
        public string updatedAt;
        public string __v;

    }
    [System.Serializable]
    public class ChipsPrice
    {
        public float discounted_amount;
        public string _id;
        public string currency;
        public float amount;


    }

     public string currencyConversionUrl;
   
     public ChipsCurrency chipCurrRef;

    #endregion

    void Awake() 
    {
        instance = this;
    }

    void Start()
    {
        currencyConversionUrl= ServerChanger.instance.domainURL + "api/v1/admin/shop-chips";
    }

    public void BuyChipRequest() 
    {
        ClubManagement.instance.loadingPanel.SetActive(true);
        Communication.instance.GetData(currencyConversionUrl, BuyChipsProcess);
    }

    public void BuyChipsProcess(string response) 
    {
        print(response);

        ClubManagement.instance.loadingPanel.SetActive(false);

        if (string.IsNullOrEmpty(response))
        {
            print("Some error in login! Please try after some time.");
        }
        else
        {
            print("response" + response);

            chipCurrRef = JsonUtility.FromJson<ChipsCurrency>(response);


            if (!chipCurrRef.error)
            {
                    print("login correct...");
                CurrencyDataToUI();

            }
            else
            {
                    print("login incorrect...");

            }

            Debug.Log(chipCurrRef.data[0].chips);
            Debug.Log(chipCurrRef.data[0].chips_price[0].currency);
            Debug.Log(chipCurrRef.data[0].chips_price[0].amount);
            Debug.Log(chipCurrRef.data[0].chips_price[0].discounted_amount);
        }

    }

    public void CurrencyDataToUI() 
    {
        if (chipCurrRef.data.Count != chipsAndCurrencyUIList.Count)
        {
            Debug.Log("currency and api data mismatch..."+ chipCurrRef.data.Count +"  " + chipsAndCurrencyUIList.Count);
            return;
        }


        for (int i = 0; i < chipsAndCurrencyUIList.Count; i++) 
        {
            chipsAndCurrencyUIList[i].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "$"+ chipCurrRef.data[i].chips_price[0].amount.ToString() ;
            chipsAndCurrencyUIList[i].transform.GetChild(1).GetChild(1).GetComponent<Text>().color = Color.black;
            chipsAndCurrencyUIList[i].transform.GetChild(1).GetChild(0).GetComponent<Text>().color = Color.black;
            chipsAndCurrencyUIList[i].transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "$"+ chipCurrRef.data[i].chips_price[0].discounted_amount.ToString() ;
            chipsAndCurrencyUIList[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = ChangeValuestoMillions(chipCurrRef.data[i].chips) +" chips";
        }
    }


    public string ChangeValuestoMillions(float value) 
    {

        if (value > 999 && value < 1000000)
        {
            return (value / 1000f).ToString("F0") + "K"; // convert to K for number from > 1000 < 1 million 
        }
        else if (value >= 1000000)
        {
            return (value / 1000000f).ToString("F0") + "M"; // convert to M for number from > 1 million 
        }
        else if (value < 900)
        {
            return value+""; // if value < 1000, nothing to do
        }
        else
            return "";
    }

    public void CallSocialBuyChipsMethod()
    {
        int buttonValue = int.Parse( EventSystem.current.currentSelectedGameObject.name);
        int chipsValue = (int)chipCurrRef.data[buttonValue].chips;
        SocialProfile._instance.BuyChips(chipsValue);
        Debug.Log("chips Value" + chipsValue);
    }
}
