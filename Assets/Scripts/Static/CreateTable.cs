using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class CreateTable : MonoBehaviour
{
    //public string createTableUrl;

    [Header("GameObject Reference")]
    public GameObject regularImage;
    public GameObject tournamentImage;
    public GameObject gameType;
    public GameObject nlhImage;
    public GameObject ploImage;
    public GameObject firstScreen;
    public GameObject secondScreen;
    public GameObject firstScreenBackButton;
    public GameObject RepeatTextPanel;
    public GameObject clubHome;
    public GameObject createTable;
    public GameObject back;
    public GameObject startSetTime;
    public GameObject startSetTimeBG;
    public GameObject endSetTime;
    public GameObject endSetTimeBG;
    public GameObject startTimeBtn;
    public GameObject endTimeBtn;
    public GameObject selectMembers;
    public GameObject maintainVpipBtn;
    public GameObject limitWinPlayerBtn;

    [Header("Text References")]
    public Text leftHandle;
    public Text rightHandle;
    public Text sendInvitation;
    Text tableSizeValue;
    Text actionTimeValue;
    Text smallBlindValue;
    Text bigBlindValue;
    Text buyInValue;
    Text maintainVPIPValue;
    Text handThresholdValue;
    Text limitWinningValue;
    Text feeValue;
    Text feeCapValue;
    Text autoStartValue;
    Text buyInValue1;

    [Header("InputField References")]
    public InputField tableNameInputField;

    //.............................................................//
    [Header("Slider References")]
    public RangeSlider rangeSlider;
    public Slider tableSizeSlider;
    public Slider actionTimeSlider;
    public Slider smallBlindSlider;
    public Slider bigBlindSlider;
    public Slider buyInSlider;
    public Slider maintainVPIPSlider;
    public Slider handThresholdSlider;
    public Slider limitWinningSlider;
    public Slider feeSlider;
    public Slider feeCapSlider;
    public Slider autoStartSlider;
    public Slider buyInSlider1;

    internal bool videoModebool;
    internal bool mississipiebool;
    internal bool maintainVPIPbool;
    internal bool limitWinningbool;
    internal bool autoStartbool;
    internal bool buyInbool;
    internal bool repeatbool;
    public int addMinute = 30;

    [SerializeField]
    public ClubInfo clubInfo;

    private void Start()
    {
        //createTableUrl = ServerChanger.instance.domainURL + "api/v1/pokertable/create-rules";

        tableSizeValue = secondScreen.transform.GetChild(0).GetChild(7).GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>();
        actionTimeValue = secondScreen.transform.GetChild(0).GetChild(11).GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>();
        smallBlindValue = secondScreen.transform.GetChild(0).GetChild(15).GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>();
        bigBlindValue = secondScreen.transform.GetChild(0).GetChild(21).GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>();
        buyInValue = secondScreen.transform.GetChild(0).GetChild(71).GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>();
        maintainVPIPValue = secondScreen.transform.GetChild(0).GetChild(33).GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>();
        handThresholdValue = secondScreen.transform.GetChild(0).GetChild(38).GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>();
        limitWinningValue = secondScreen.transform.GetChild(0).GetChild(46).GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>();
        feeValue = secondScreen.transform.GetChild(0).GetChild(50).GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>();
        feeCapValue = secondScreen.transform.GetChild(0).GetChild(55).GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>();
        autoStartValue = secondScreen.transform.GetChild(0).GetChild(60).GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>();

        buyInValue1 = secondScreen.transform.GetChild(0).GetChild(72).GetChild(2).GetChild(0).GetComponent<Text>();

        smallBlindValue.text = "0.05";
        bigBlindValue.text = "0.1";
        leftHandle.text = "2";
        autoStartValue.text = "2";
        rightHandle.text = "20";
        actionTimeValue.text = "15";
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (secondScreen.activeInHierarchy)
            {
                secondScreen.SetActive(false);
                firstScreen.SetActive(true);
                transform.GetChild(1).GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).GetChild(1).GetChild(1).GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
            }

            else if(firstScreen.activeInHierarchy)
            {
                ClubManagement.instance.ClickOnBackButtonCreateTableHomePage();
            }
        }
    }

    public void OpenStartSetTime()
    {
        startSetTime.SetActive(true);
        startSetTimeBG.SetActive(true);
    }

    public void closeStartSetTime()
    {
        startSetTime.SetActive(false);
        startSetTimeBG.SetActive(false);
    }

    public void OpenEndSetTime()
    {
        endSetTime.SetActive(true);
        endSetTimeBG.SetActive(true);
    }

    public void closeEndSetTime()
    {
        endSetTime.SetActive(false);
        endSetTimeBG.SetActive(false);
    }

    public void SetTime()
    {
        startTimeBtn.transform.GetChild(0).GetComponent<Text>().text = Raycaster.instance.startTimeValue.ToString() + ":" + Raycaster.instance.startTimeValue1.ToString();
        startSetTime.SetActive(false);
        startSetTimeBG.SetActive(false);
    }

    public void SetTime1()
    {
        endTimeBtn.transform.GetChild(0).GetComponent<Text>().text = Raycaster1.instance.endTimeValue.ToString() + ":" + Raycaster1.instance.endTimevalue1.ToString();
        endSetTime.SetActive(false);
        endSetTimeBG.SetActive(false);
    }

    public void RegularTable()
    {
        regularImage.SetActive(true);
        tournamentImage.SetActive(false);
        gameType.SetActive(true);
    }

    public void TournamentTable()
    {
        regularImage.SetActive(false);
        tournamentImage.SetActive(true);
        gameType.SetActive(true);
    }

    public void NlhType()
    {
        nlhImage.SetActive(true);
        ploImage.SetActive(false);


        if (regularImage.activeInHierarchy)
        {
            secondScreen.SetActive(true);
        }
        else
        {
            TournamentScript.instance.tournamentObj.SetActive(true);
            gameObject.SetActive(false);
        }
        firstScreen.SetActive(false);
    }

    public void PloType()
    {
        nlhImage.SetActive(false);
        ploImage.SetActive(true);
        firstScreen.SetActive(false);
        secondScreen.SetActive(true);
    }

    public void Back()
    {
        firstScreen.SetActive(true);
        secondScreen.SetActive(false);
        transform.GetChild(1).GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).GetChild(1).GetChild(1).GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).GetChild(1).gameObject.SetActive(false);

        tableSizeSlider.maxValue = 9;
        secondScreen.transform.GetChild(0).GetChild(9).GetComponent<Text>().text = "9";
    }

    public void UpdateTableSizeVal()
    {
        tableSizeValue.text = Convert.ToInt32(tableSizeSlider.value).ToString();
    }

    public void UpdateActionTimeVal()
    {
        if (actionTimeSlider.value == 0)
        {
            actionTimeValue.text = "15";
        }
        else if (actionTimeSlider.value == 1)
        {
            actionTimeValue.text = "18";
        }
        else if (actionTimeSlider.value == 2)
        {
            actionTimeValue.text = "20";
        }
        else if (actionTimeSlider.value == 3)
        {
            actionTimeValue.text = "22";
        }
        else if (actionTimeSlider.value == 4)
        {
            actionTimeValue.text = "25";
        }
        else if (actionTimeSlider.value == 5)
        {
            actionTimeValue.text = "28";
        }
        else if (actionTimeSlider.value == 6)
        {
            actionTimeValue.text = "30";
        }
        else if (actionTimeSlider.value == 7)
        {
            actionTimeValue.text = "32";
        }
        else if (actionTimeSlider.value == 8)
        {
            actionTimeValue.text = "35";
        }
        else if (actionTimeSlider.value == 9)
        {
            actionTimeValue.text = "38";
        }
        else if (actionTimeSlider.value == 10)
        {
            actionTimeValue.text = "40";
        }
        else if (actionTimeSlider.value == 11)
        {
            actionTimeValue.text = "45";
        }
    }

    public void UpdateSmallBlindVal()
    {
        if (smallBlindSlider.value == 0)
        {
            smallBlindValue.text = "0.05";
        }
        else if (smallBlindSlider.value == 1)
        {
            smallBlindValue.text = "0.1";
        }
        else if (smallBlindSlider.value == 2)
        {
            smallBlindValue.text = "0.2";
        }
        else if (smallBlindSlider.value == 3)
        {
            smallBlindValue.text = "0.3";
        }
        else if (smallBlindSlider.value == 4)
        {
            smallBlindValue.text = "0.4";
        }
        else if (smallBlindSlider.value == 5)
        {
            smallBlindValue.text = "0.5";
        }
        else if (smallBlindSlider.value == 6)
        {
            smallBlindValue.text = "1";
        }
        else if (smallBlindSlider.value == 7)
        {
            smallBlindValue.text = "2";
        }
        else if (smallBlindSlider.value == 8)
        {
            smallBlindValue.text = "3";
        }
        else if (smallBlindSlider.value == 9)
        {
            smallBlindValue.text = "4";
        }
        else if (smallBlindSlider.value == 10)
        {
            smallBlindValue.text = "5";
        }
        else if (smallBlindSlider.value == 11)
        {
            smallBlindValue.text = "10";
        }
        else if (smallBlindSlider.value == 12)
        {
            smallBlindValue.text = "15";
        }
        else if (smallBlindSlider.value == 13)
        {
            smallBlindValue.text = "25";
        }
        else if (smallBlindSlider.value == 14)
        {
            smallBlindValue.text = "50";
        }
        else if (smallBlindSlider.value == 15)
        {
            smallBlindValue.text = "100";
        }
        else if (smallBlindSlider.value == 16)
        {
            smallBlindValue.text = "250";
        }
        secondScreen.transform.GetChild(0).GetChild(19).GetComponent<Text>().text = ((float.Parse(smallBlindValue.text)) * 2).ToString();
        secondScreen.transform.GetChild(0).GetChild(20).GetComponent<Text>().text = ((float.Parse(smallBlindValue.text)) * 4).ToString();
        bigBlindSlider.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = secondScreen.transform.GetChild(0).GetChild(19).GetComponent<Text>().text;

        secondScreen.transform.GetChild(0).GetChild(23).GetComponent<Text>().text = ((float.Parse(bigBlindValue.text)) * 20).ToString();
        secondScreen.transform.GetChild(0).GetChild(24).GetComponent<Text>().text = ((float.Parse(bigBlindValue.text)) * 200).ToString();
        leftHandle.text = secondScreen.transform.GetChild(0).GetChild(23).GetComponent<Text>().text;
        rightHandle.text = secondScreen.transform.GetChild(0).GetChild(24).GetComponent<Text>().text;
    }

    public void UpdatBigBlindVal()
    {
        if (bigBlindSlider.value == 0)
        {
            bigBlindValue.text = (float.Parse(smallBlindValue.text)*2).ToString("F1");
        }

        if (bigBlindSlider.value == 1)
        {
            bigBlindValue.text = (float.Parse(smallBlindValue.text) * 2.5).ToString("F2");
        }

        if (bigBlindSlider.value == 2)
        {
            bigBlindValue.text = (float.Parse(smallBlindValue.text) * 3).ToString("F2");
        }

        if (bigBlindSlider.value == 3)
        {
            bigBlindValue.text = (float.Parse(smallBlindValue.text) * 3.5).ToString("F2");
        }

        if (bigBlindSlider.value == 4)
        {
            bigBlindValue.text = (float.Parse(smallBlindValue.text) * 4).ToString("F1");
        }
        secondScreen.transform.GetChild(0).GetChild(23).GetComponent<Text>().text = ((float.Parse(bigBlindValue.text)) * 20).ToString(); 
        secondScreen.transform.GetChild(0).GetChild(24).GetComponent<Text>().text = ((float.Parse(bigBlindValue.text)) * 200).ToString();
        leftHandle.text = secondScreen.transform.GetChild(0).GetChild(23).GetComponent<Text>().text;
        rightHandle.text = secondScreen.transform.GetChild(0).GetChild(24).GetComponent<Text>().text;
    }

    public void UpdateBuyInLeftVal()
    {
        if (rangeSlider.LowValue == 1)
        {
            leftHandle.text = (float.Parse(bigBlindValue.text) * 20).ToString();
        }

        if (rangeSlider.LowValue == 2)
        {
            leftHandle.text = (float.Parse(bigBlindValue.text) * 20 * rangeSlider.LowValue).ToString();
        }

        if (rangeSlider.LowValue == 3)
        {
            leftHandle.text = (float.Parse(bigBlindValue.text) * 20 * rangeSlider.LowValue).ToString();
        }

        if (rangeSlider.LowValue == 4)
        {
            leftHandle.text = (float.Parse(bigBlindValue.text) * 20 * rangeSlider.LowValue).ToString();
        }

        if (rangeSlider.LowValue == 5)
        {
            leftHandle.text = (float.Parse(bigBlindValue.text) * 20 * rangeSlider.LowValue).ToString();
        }

        if (rangeSlider.LowValue == 6)
        {
            leftHandle.text = (float.Parse(bigBlindValue.text) * 20 * rangeSlider.LowValue).ToString();
        }

        if (rangeSlider.LowValue == 7)
        {
            leftHandle.text = (float.Parse(bigBlindValue.text) * 20 * rangeSlider.LowValue).ToString();
        }

        if (rangeSlider.LowValue == 8)
        {
            leftHandle.text = (float.Parse(bigBlindValue.text) * 20 * rangeSlider.LowValue).ToString();
        }

        if (rangeSlider.LowValue == 9)
        {
            leftHandle.text = (float.Parse(bigBlindValue.text) * 20 * rangeSlider.LowValue).ToString();
        }

        if (rangeSlider.LowValue == 10)
        {
            leftHandle.text = (float.Parse(bigBlindValue.text) * 20 * rangeSlider.LowValue).ToString();
        }
    }

    public void UpdateBuyInRightVal()
    {
        if (rangeSlider.HighValue == 1)
        {
            rightHandle.text = (float.Parse(bigBlindValue.text) * 20 * 1).ToString();
        }

        if (rangeSlider.HighValue == 2)
        {
            rightHandle.text = (float.Parse(bigBlindValue.text) * 20 * 2).ToString();
        }

        if (rangeSlider.HighValue == 3)
        {
            rightHandle.text = (float.Parse(bigBlindValue.text) * 20 * 3).ToString();
        }

        if (rangeSlider.HighValue == 4)
        {
            rightHandle.text = (float.Parse(bigBlindValue.text) * 20 * 4).ToString();
        }

        if (rangeSlider.HighValue == 5)
        {
            rightHandle.text = (float.Parse(bigBlindValue.text) * 20 * 5).ToString();
        }

        if (rangeSlider.HighValue == 6)
        {
            rightHandle.text = (float.Parse(bigBlindValue.text) * 20 * 6).ToString();
        }

        if (rangeSlider.HighValue == 7)
        {
            rightHandle.text = (float.Parse(bigBlindValue.text) * 20 * 7).ToString();
        }

        if (rangeSlider.HighValue == 8)
        {
            rightHandle.text = (float.Parse(bigBlindValue.text) * 20 * 8).ToString();
        }

        if (rangeSlider.HighValue == 9)
        {
            rightHandle.text = (float.Parse(bigBlindValue.text) * 20 * 9).ToString();
        }

        if (rangeSlider.HighValue == 10)
        {
            rightHandle.text = (float.Parse(bigBlindValue.text) * 20 * 10).ToString();
        }
    }

    public void UpdateMaintainVPIPVal()
    {
        maintainVPIPValue.text = Convert.ToInt32(maintainVPIPSlider.value * 5).ToString() + "%";
    }

    public void UpdateHandThresholdVal()
    {
        handThresholdValue.text = Convert.ToInt32(handThresholdSlider.value * 5).ToString();
    }
    public void UpdateLimitWinningVal()
    {
        limitWinningValue.text = Convert.ToInt32(limitWinningSlider.value).ToString();
    }
    public void UpdateFeeVal()
    {
        feeValue.text = (feeSlider.value * 0.5).ToString("F1") + " " + "%"; 
    }
    public void UpdateFeeCapVal()
    {
        feeCapValue.text = Convert.ToInt32(feeCapSlider.value * 8).ToString() +" "+ "x";
    }
    public void UpdateAutoStartVal()
    {
        if (autoStartSlider.value == 10)
        {
            autoStartValue.text = "Off";
        }
        else
        {
            autoStartValue.text = autoStartSlider.value.ToString();
        }
    }

    public void VideoModeImage(GameObject gameObject)
    {
        if(!gameObject.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
            videoModebool = false;
        }
        else
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
            videoModebool = true;
        }
        TableSize();
    }

    public void TableSize()
    {
        if(videoModebool)
        {
            tableSizeSlider.maxValue = 8;
            secondScreen.transform.GetChild(0).GetChild(9).GetComponent<Text>().text = "8";
        }
        else
        {
            tableSizeSlider.maxValue = 9;
            secondScreen.transform.GetChild(0).GetChild(9).GetComponent<Text>().text = "9";
        }
    }

    public void MississipieImage(GameObject gameObject)
    {
        if (!gameObject.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
            mississipiebool = false;
        }
        else
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
            mississipiebool = true;
        }
    }

    public void MaintainVPIPImage(GameObject gameObject)
    {
        if (!gameObject.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
            maintainVPIPbool = false;
            maintainVPIPSlider.interactable = false;
            handThresholdSlider.interactable = false;
            handThresholdSlider.value = 0;
            maintainVPIPSlider.value = 0;
            maintainVPIPSlider.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = "0%";
            handThresholdSlider.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = "5";
        }
        else
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
            maintainVPIPbool = true;
            maintainVPIPSlider.interactable = true;
            handThresholdSlider.interactable = true;

            //..........Disable LimitWinPlayer.....//
            limitWinPlayerBtn.transform.GetChild(0).gameObject.SetActive(true);
            limitWinPlayerBtn.transform.GetChild(1).gameObject.SetActive(false);
            limitWinningbool = false;
            limitWinningSlider.interactable = false;
            limitWinningSlider.value = 0;
            limitWinningSlider.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = "1";
        }
    }

    public void LimitWinningImage(GameObject gameObject)
    {
        if (!gameObject.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
            limitWinningbool = false;
            limitWinningSlider.interactable = false;
            limitWinningSlider.value = 0;
            limitWinningSlider.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = "1";
        }
        else
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
            limitWinningbool = true;
            limitWinningSlider.interactable = true;

            //.......Disable maintain VPIP and Hand threshold ......//
            maintainVpipBtn.transform.GetChild(0).gameObject.SetActive(true);
            maintainVpipBtn.transform.GetChild(1).gameObject.SetActive(false);
            maintainVPIPbool = false;
            maintainVPIPSlider.interactable = false;
            handThresholdSlider.interactable = false;
            handThresholdSlider.value = 0;
            maintainVPIPSlider.value = 0;
            maintainVPIPSlider.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = "0%";
            handThresholdSlider.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = "5";
        }
    }

    public void AutoStartImage(GameObject gameObject)
    {
        if (!gameObject.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
            autoStartbool = false;
            autoStartSlider.interactable = false;
            autoStartSlider.value = 0;
            autoStartSlider.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = "2";
        }
        else
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
            autoStartbool = true;
            autoStartSlider.interactable = true;
           
        }
    }

    public void BuyInImage(GameObject gameObject)
    {
        if (!gameObject.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
            buyInbool = false;

            sendInvitation.color = new Color32(94,101,111,255);
            selectMembers.transform.GetComponent<Image>().color = new Color32(60, 67, 78, 255);
            selectMembers.transform.GetChild(0).GetComponent<Text>().color = new Color32(94, 101, 111, 255);
            selectMembers.transform.GetComponent<Button>().interactable = false;
        }
        else
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
            buyInbool = true;

            sendInvitation.color = new Color32(255, 255, 255, 255);
            selectMembers.transform.GetComponent<Image>().color = new Color32(3, 168, 124, 255);
            selectMembers.transform.GetChild(0).GetComponent<Text>().color = new Color32(255, 255, 255, 255);
            selectMembers.transform.GetComponent<Button>().interactable = true;
        }
    }

    public void RepeatImage(GameObject gameObject)
    {
        if (!gameObject.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);

            RepeatTextPanel.SetActive(false);
            repeatbool = false;
        }
        else
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(1).gameObject.SetActive(true);

            RepeatTextPanel.SetActive(true);
            repeatbool = true;
        }
    }

    public void AllButtonImage(GameObject val)
    {
        if (repeatbool)
        {
            if (!val.transform.GetChild(0).gameObject.activeInHierarchy)
            {
                for (int i = 0; i <= 7; i++)
                {
                    RepeatTextPanel.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
                }
            }
            else
            {
                for (int i = 0; i <= 7; i++)
                {
                    RepeatTextPanel.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
                }
            }
        }
    }

    public void AllButtonImage2(GameObject val)
    {
        if (repeatbool)
        {
            if (!val.transform.GetChild(0).gameObject.activeInHierarchy)
            {
                val.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                val.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public void SelectMemberTick(GameObject image)
    {
            if (image.activeInHierarchy)
            {
                image.SetActive(false);
            }
            else
            {
                image.SetActive(true);
            }
    }

    [Serializable]
    public class Users
    {
        public string user_id;
        public string client_id;
    }

    int usersCount;
    public void ClickSubmitInMembersDuringCreateTable()
    {
        clubInfo.users.Clear();
        usersCount = 0;

        for (int i = 0; i < ClubManagement.instance.membersInCreateTableContent.transform.childCount; i++)
        {
            if (ClubManagement.instance.membersInCreateTableContent.transform.GetChild(i).GetChild(4).GetChild(1).gameObject.activeInHierarchy)
            {
                clubInfo.users.Add(new Users());
                clubInfo.users[usersCount].user_id = ClubManagement.instance.membersInCreateTableContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text;
                clubInfo.users[usersCount].client_id = ClubManagement.instance.membersInCreateTableContent.transform.GetChild(i).GetChild(2).GetComponent<Text>().text;
                usersCount = usersCount + 1;
            }
        }

        secondScreen.gameObject.SetActive(true);
        ClubManagement.instance.createTablePage.transform.GetChild(4).gameObject.SetActive(false);

        sendInvitation.text = "Send invitation (" + (usersCount).ToString() + ")";
    }

    public void ClickOnBackButtonFromMemberList()
    {
        createTable.transform.GetChild(4).gameObject.SetActive(false);
        secondScreen.SetActive(true);

        for (int i = 0; i < ClubManagement.instance.membersInCreateTableContent.transform.childCount; i++)
        {
            ClubManagement.instance.membersInCreateTableContent.transform.GetChild(i).GetChild(4).GetChild(1).gameObject.SetActive(false);
        }

        if (clubInfo.users.Count > 0)
        {
            for (int i = 0; i < clubInfo.users.Count; i++)
            {
                for (int j = 0; j < ClubManagement.instance.membersInCreateTableContent.transform.childCount; j++)
                {
                    if (clubInfo.users[i].client_id == ClubManagement.instance.membersInCreateTableContent.transform.GetChild(j).GetChild(2).GetComponent<Text>().text)
                    {
                        ClubManagement.instance.membersInCreateTableContent.transform.GetChild(j).GetChild(4).GetChild(1).gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    public void ClubInfoValues()
    {
        clubInfo.club_id = ClubManagement.instance.clubId.text;

        clubInfo.game_type = secondScreen.transform.GetChild(0).GetChild(0).GetComponent<Text>().text;
        clubInfo.table_name = secondScreen.transform.GetChild(0).GetChild(3).GetComponent<InputField>().textComponent.text;
        clubInfo.video_mode = videoModebool;
        clubInfo.table_size = Convert.ToInt32(secondScreen.transform.GetChild(0).GetChild(7).GetComponent<Slider>().value);
        clubInfo.action_time = actionTimeValue.text; 
        clubInfo.small_blind = float.Parse(smallBlindValue.text); 
        clubInfo.big_blind = float.Parse(bigBlindValue.text); 
        clubInfo.min_buy_in = float.Parse(leftHandle.text); 
        clubInfo.max_buy_in = float.Parse(rightHandle.text); 

        clubInfo.mississippi_straddle = mississipiebool;
        clubInfo.min_vpip = Convert.ToInt32(secondScreen.transform.GetChild(0).GetChild(33).GetComponent<Slider>().value * 9);
        //clubInfo.maintainVPIP_button = maintainVPIPbool;
        
        clubInfo.vpip_level = 1;
        clubInfo.hands = Convert.ToInt32(secondScreen.transform.GetChild(0).GetChild(38).GetComponent<Slider>().value * 45);

        clubInfo.winning_players = Convert.ToInt32(secondScreen.transform.GetChild(0).GetChild(46).GetComponent<Slider>().value);

        clubInfo.limit_win_player = limitWinningbool;
                                                                                                                                      
        clubInfo.fee = Convert.ToInt32(secondScreen.transform.GetChild(0).GetChild(50).GetComponent<Slider>().value * 10);
        clubInfo.fee_cap = Convert.ToInt32(secondScreen.transform.GetChild(0).GetChild(55).GetComponent<Slider>().value * 8);
        clubInfo.auto_start = autoStartbool;
        clubInfo.min_auto_start = autoStartValue.text;
        clubInfo.buy_in_authorization = buyInbool;
        clubInfo.start_time = secondScreen.transform.GetChild(0).GetChild(64).GetChild(0).GetComponent<Text>().text;
        clubInfo.end_time = secondScreen.transform.GetChild(0).GetChild(66).GetChild(0).GetComponent<Text>().text;
        clubInfo.repeat = repeatbool;
        clubInfo.zone = Registration.instance.timeZone;
        clubInfo.repeat_days = new List<string>();
        if (repeatbool)
        {
            for (int i = 1; i <= 7; i ++)
            {
                if (RepeatTextPanel.transform.GetChild(i).GetChild(0).gameObject.activeInHierarchy)
                {
                    clubInfo.repeat_days.Add(RepeatTextPanel.transform.GetChild(i).GetChild(1).GetComponent<Text>().text);
                }
               
            }
        }
        string body = JsonUtility.ToJson(clubInfo);

        print("body : "+body);
        ClubManagement.instance.loadingPanel.SetActive(true);
        //Communication.instance.PostData(createTableUrl, body, CreateTableProcess);
        Uimanager.instance.createTablePage.SetActive(false);
        Uimanager.instance.clubHomePage.SetActive(true);

        for (int i = 0; i < ClubManagement.instance.membersInCreateTableContent.transform.childCount; i++)
        {
            ClubManagement.instance.membersInCreateTableContent.transform.GetChild(i).GetChild(4).GetChild(1).gameObject.SetActive(false);
        }
        sendInvitation.text = "Send invitation (0)";
    }

    public void ResetCreateTableUI()
    {
        firstScreen.SetActive(true);
        gameType.SetActive(false);
        nlhImage.SetActive(false);
        ploImage.SetActive(false);
        regularImage.SetActive(false);
        tournamentImage.SetActive(false);
    }

    public void ResetClubInfoValues()
    {
        secondScreen.SetActive(false);
        firstScreen.SetActive(true);
        RepeatTextPanel.SetActive(false);
        firstScreen.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(false);
        firstScreen.transform.GetChild(1).gameObject.SetActive(false);
        firstScreen.transform.GetChild(1).gameObject.SetActive(false);
        firstScreen.transform.GetChild(1).GetChild(1).GetChild(0).gameObject.SetActive(false);
        secondScreen.transform.GetChild(0).localPosition = new Vector3(0, -3469.1f, 0);
        videoModebool = false;
        mississipiebool = false;
        maintainVPIPbool = false;
        limitWinningbool = false;
        autoStartbool = false;
        buyInbool = false;
        repeatbool = false;

        secondScreen.transform.GetChild(0).GetChild(7).GetComponent<Slider>().value = 0;
        tableNameInputField.text = "";
        secondScreen.transform.GetChild(0).GetChild(5).GetChild(0).gameObject.SetActive(true);
        secondScreen.transform.GetChild(0).GetChild(5).GetChild(1).gameObject.SetActive(false);
        secondScreen.transform.GetChild(0).GetChild(11).GetComponent<Slider>().value = 0;
        secondScreen.transform.GetChild(0).GetChild(15).GetComponent<Slider>().value = 0;
        secondScreen.transform.GetChild(0).GetChild(21).GetComponent<Slider>().value = 0;
        secondScreen.transform.GetChild(0).GetChild(25).GetComponent<Slider>().value = 12;
        rangeSlider.LowValue = 0;
        rangeSlider.HighValue = 10;
        secondScreen.transform.GetChild(0).GetChild(27).GetChild(0).gameObject.SetActive(true);
        secondScreen.transform.GetChild(0).GetChild(27).GetChild(1).gameObject.SetActive(false);
        secondScreen.transform.GetChild(0).GetChild(33).GetComponent<Slider>().value = 0;
        secondScreen.transform.GetChild(0).GetChild(34).GetChild(0).gameObject.SetActive(true);
        secondScreen.transform.GetChild(0).GetChild(34).GetChild(1).gameObject.SetActive(false);
        secondScreen.transform.GetChild(0).GetChild(38).GetComponent<Slider>().value = 0;
        secondScreen.transform.GetChild(0).GetChild(46).GetComponent<Slider>().value = 0;
        secondScreen.transform.GetChild(0).GetChild(43).GetChild(0).gameObject.SetActive(true);
        secondScreen.transform.GetChild(0).GetChild(43).GetChild(1).gameObject.SetActive(false);
        secondScreen.transform.GetChild(0).GetChild(50).GetComponent<Slider>().value = 0;
        secondScreen.transform.GetChild(0).GetChild(55).GetComponent<Slider>().value = 0;
        secondScreen.transform.GetChild(0).GetChild(60).GetComponent<Slider>().value = 0;
        secondScreen.transform.GetChild(0).GetChild(59).GetChild(0).gameObject.SetActive(true);
        secondScreen.transform.GetChild(0).GetChild(59).GetChild(1).gameObject.SetActive(false);
        secondScreen.transform.GetChild(0).GetChild(62).GetChild(0).gameObject.SetActive(true);
        secondScreen.transform.GetChild(0).GetChild(62).GetChild(1).gameObject.SetActive(false);
        secondScreen.transform.GetChild(0).GetChild(64).GetChild(0).GetComponent<Text>().text = "HH/MM";
        secondScreen.transform.GetChild(0).GetChild(66).GetChild(0).GetComponent<Text>().text = "HH/MM";
        secondScreen.transform.GetChild(0).GetChild(75).GetChild(0).GetComponent<Text>().text = "DD/MM/YYYY";
        secondScreen.transform.GetChild(0).GetChild(76).GetChild(0).GetComponent<Text>().text = "DD/MM/YYYY";
        secondScreen.transform.GetChild(0).GetChild(68).GetChild(0).gameObject.SetActive(true);
        secondScreen.transform.GetChild(0).GetChild(68).GetChild(1).gameObject.SetActive(false);
        maintainVPIPSlider.interactable = false;
        handThresholdSlider.interactable = false;
        limitWinningSlider.interactable = false;
        autoStartSlider.interactable = false;
        //................................SelecMember......................................//
        sendInvitation.color = new Color32(94, 101, 111, 255);
        selectMembers.transform.GetComponent<Image>().color = new Color32(60, 67, 78, 255);
        selectMembers.transform.GetChild(0).GetComponent<Text>().color = new Color32(94, 101, 111, 255);
        selectMembers.transform.GetComponent<Button>().interactable = false;
        //.................................................................................//
        for (int i = 0; i <= 7; i++)
        {
            RepeatTextPanel.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }
    }

    [SerializeField]
    public CreateTableCheck createTableCheck;

    void CreateTableProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("error");
        }
        else
        {
            print("" + response);

           createTableCheck = JsonUtility.FromJson<CreateTableCheck>(response);

            if (!createTableCheck.error)
            {
                print("table create successful.......");
                print("ClubManagement.instance.clubId........" + ClubManagement.instance.clubId.text);

                ClubManagement.instance.DestroyGeneratedTableItem();
                ClubManagement.instance.ResetRegularTableItem();
                ClubManagement.instance.ResetTournamentTableItem();

                ClubManagement.instance.ClickOnClubDetails(ClubManagement.instance.clubId);
                ClubManagement.instance.myTableScrollContent.SetActive(true);
                ClubManagement.instance.clubHomeScreenScrollList.SetActive(true);
            }
            else
            {
                print("table create Unsuccessful.......");
            }

            
        }

    }

    #region Default Date Time from server

    private string dateTimeZoneUrl;

    [Serializable]
    public class DateTimeReq
    {
        public string zone;
    }

    [Serializable]
    public class DateTimeRes
    {
        public bool error;
        public string date;
        public string time;
    }

    [SerializeField] DateTimeReq dateTimeReq;
    [SerializeField] DateTimeRes dateTimeRes;

    public void DefaultDateTimeReq()
    {
        dateTimeZoneUrl = ServerChanger.instance.domainURL + "api/v1/pokertable/date-time-zone";

        dateTimeReq.zone = Registration.instance.timeZone;

        string body = JsonUtility.ToJson(dateTimeReq);

        print(body);

        ClubManagement.instance.loadingPanel.SetActive(true);
        Communication.instance.PostData(dateTimeZoneUrl, body, DefaultDateTimeProcess);

    }


    void DefaultDateTimeProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);

        if (!string.IsNullOrEmpty(response))
        {
            print(response);
            dateTimeRes = JsonUtility.FromJson<DateTimeRes>(response);

            if (!dateTimeRes.error)
            {
                print("success.........!!");

                string startTime = dateTimeRes.time.Substring(0, dateTimeRes.time.Length);
                string currentDate = dateTimeRes.date;

                secondScreen.transform.GetChild(0).GetChild(64).GetChild(0).GetComponent<Text>().text = startTime;
                secondScreen.transform.GetChild(0).GetChild(75).GetChild(0).GetComponent<Text>().text = currentDate;
                secondScreen.transform.GetChild(0).GetChild(76).GetChild(0).GetComponent<Text>().text = currentDate;

                string endTime = AddMin(currentDate, startTime, addMinute);

                secondScreen.transform.GetChild(0).GetChild(66).GetChild(0).GetComponent<Text>().text = endTime;  //end time

            }
        }
    }


    string AddMin(string dateDMY, string time, int addMin)
    {
        string dateMDY = Admin.instance.DMYToMDY(dateDMY);

        DateTime dateTime = DateTime.Parse(dateMDY + " " + time);

        dateTime = dateTime.AddMinutes(addMin);

        return dateTime.ToString("HH:mm");
    }

    #endregion

}

[Serializable]
public class CreateTableCheck
{
    public bool error;
}

[Serializable]
public class ClubInfo
{
    public string club_id;
    public string game_type;
    //public string table_type;
    public string table_name;
    public bool video_mode;
    public int table_size;
    public string action_time;
    public float small_blind;
    public float big_blind;
    public float min_buy_in;
    public float max_buy_in;
    public bool mississippi_straddle;
    public int min_vpip;
    public int vpip_level;
    //public bool maintainVPIP_button;
    
    public int hands;
    public int winning_players;
    public bool limit_win_player;
    public float fee;
    public float fee_cap;
    public bool auto_start;
    public string min_auto_start;
    public bool buy_in_authorization;
   // public string start_date;
    public string start_time;
    //public string end_date;
    public string end_time;
    public string zone;
    public bool repeat; 
    public List<string> repeat_days;
    public List<CreateTable.Users> users;
}
