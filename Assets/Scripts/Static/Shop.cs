using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject shopCanvas;
    public GameObject shopPopUpCanvas;
    public GameObject myAccount;

    public  void AddSixtyDiamonds()
    {
        shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = (int.Parse(shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text) + 60).ToString();
        shopPopUpCanvas.transform.GetChild(6).GetChild(0).GetComponent<Text>().text = shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text;
        CreateClubMain.instance.homePage.transform.GetChild(0).GetChild(4).GetChild(0).GetComponent<Text>().text = shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text;
        ClubManagement.instance.clubPage.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text;
        ClubManagement.instance.clubHomePage.transform.GetChild(0).GetChild(7).GetChild(0).GetComponent<Text>().text = shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text;
        myAccount.transform.GetChild(0).GetChild(5).GetChild(0).GetComponent<Text>().text = shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text;
    }

    public void AddThreeHundredDiamonds()
    {
        shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = (int.Parse(shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text) + 300).ToString();
        shopPopUpCanvas.transform.GetChild(6).GetChild(0).GetComponent<Text>().text = shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text;
        CreateClubMain.instance.homePage.transform.GetChild(0).GetChild(4).GetChild(0).GetComponent<Text>().text = shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text;
        ClubManagement.instance.clubPage.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text;
        ClubManagement.instance.clubHomePage.transform.GetChild(0).GetChild(7).GetChild(0).GetComponent<Text>().text = shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text;
        myAccount.transform.GetChild(0).GetChild(5).GetChild(0).GetComponent<Text>().text = shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text;
    }

    public void AddSevenHundredDiamonds()
    {
        shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = (int.Parse(shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text) + 700).ToString();
        shopPopUpCanvas.transform.GetChild(6).GetChild(0).GetComponent<Text>().text = shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text;
        CreateClubMain.instance.homePage.transform.GetChild(0).GetChild(4).GetChild(0).GetComponent<Text>().text = shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text;
        ClubManagement.instance.clubPage.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text;
        ClubManagement.instance.clubHomePage.transform.GetChild(0).GetChild(7).GetChild(0).GetComponent<Text>().text = shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text;
        myAccount.transform.GetChild(0).GetChild(5).GetChild(0).GetComponent<Text>().text = shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text;
    }

    public void AddThirteenHundredDiamonds()
    {
        shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = (int.Parse(shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text) + 1300).ToString();
        shopPopUpCanvas.transform.GetChild(6).GetChild(0).GetComponent<Text>().text = shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text;
        CreateClubMain.instance.homePage.transform.GetChild(0).GetChild(4).GetChild(0).GetComponent<Text>().text = shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text;
        ClubManagement.instance.clubPage.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text;
        ClubManagement.instance.clubHomePage.transform.GetChild(0).GetChild(7).GetChild(0).GetComponent<Text>().text = shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text;
        myAccount.transform.GetChild(0).GetChild(5).GetChild(0).GetComponent<Text>().text = shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text;
    }

    public void AddThreeThousandDiamonds()
    {
        shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = (int.Parse(shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text) + 3000).ToString();
        shopPopUpCanvas.transform.GetChild(6).GetChild(0).GetComponent<Text>().text = shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text;
        CreateClubMain.instance.homePage.transform.GetChild(0).GetChild(4).GetChild(0).GetComponent<Text>().text = shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text;
        ClubManagement.instance.clubPage.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text;
        ClubManagement.instance.clubHomePage.transform.GetChild(0).GetChild(7).GetChild(0).GetComponent<Text>().text = shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text;
        myAccount.transform.GetChild(0).GetChild(5).GetChild(0).GetComponent<Text>().text = shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text;
    }

    public void AddSevenThousandDiamonds()
    {
        shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = (int.Parse(shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text) + 7000).ToString();
        shopPopUpCanvas.transform.GetChild(6).GetChild(0).GetComponent<Text>().text = shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text;
        CreateClubMain.instance.homePage.transform.GetChild(0).GetChild(4).GetChild(0).GetComponent<Text>().text = shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text;
        ClubManagement.instance.clubPage.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text;
        ClubManagement.instance.clubHomePage.transform.GetChild(0).GetChild(7).GetChild(0).GetComponent<Text>().text = shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text;
        myAccount.transform.GetChild(0).GetChild(5).GetChild(0).GetComponent<Text>().text = shopCanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text;
    }
}
