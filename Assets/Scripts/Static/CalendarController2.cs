using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalendarController2 : MonoBehaviour
{
    public GameObject _calendarPanel;
    public Text _yearNumText;
    public Text _monthNumText;

    public GameObject _item;

    public List<GameObject> _dateItems = new List<GameObject>();
    const int _totalDateNum = 42;

    private DateTime _dateTime;
    public static CalendarController2 _calendarInstance;
    public int calenderCount = 0;

    public GameObject calenderPanelBtn;             //Panel Background
    //public GameObject calenderUpperPanelBtn;

    public Scrollbar scrollBar;

    void Start()
    {
        _calendarInstance = this;
        Vector3 startPos = _item.transform.localPosition;
        _dateItems.Clear();
        _dateItems.Add(_item);

        for (int i = 1; i < _totalDateNum; i++)
        {
            GameObject item = GameObject.Instantiate(_item) as GameObject;
            item.name = "Item" + (i + 1).ToString();
            item.transform.SetParent(_item.transform.parent);
            item.transform.localScale = Vector3.one;
            item.transform.localRotation = Quaternion.identity;
            item.transform.localPosition = new Vector3((i % 7) * 100 + startPos.x, startPos.y - (i / 7) * 80, startPos.z);

            _dateItems.Add(item);
        }

        _dateTime = DateTime.Now;

        CreateCalendar();

        _calendarPanel.SetActive(false);

        //allDayButtonList = new GameObject[50];
    }

    void CreateCalendar()
    {
        DateTime firstDay = _dateTime.AddDays(-(_dateTime.Day - 1));
        int index = GetDays(firstDay.DayOfWeek);

        int date = 0;
        for (int i = 0; i < _totalDateNum; i++)
        {
            Text label = _dateItems[i].GetComponentInChildren<Text>();
            _dateItems[i].SetActive(false);

            if (i >= index)
            {
                DateTime thatDay = firstDay.AddDays(date);
                if (thatDay.Month == firstDay.Month)
                {
                    _dateItems[i].SetActive(true);

                    label.text = (date + 1).ToString();
                    date++;
                }
            }
        }
        _yearNumText.text = _dateTime.Year.ToString();
        if (_dateTime.Month == 1)
        {
            _monthNumText.text = "January" + ",";
        }
        if (_dateTime.Month == 2)
        {
            _monthNumText.text = "February" + ",";
        }
        if (_dateTime.Month == 3)
        {
            _monthNumText.text = "March" + ",";
        }
        if (_dateTime.Month == 4)
        {
            _monthNumText.text = "April" + ",";
        }
        if (_dateTime.Month == 5)
        {
            _monthNumText.text = "May" + ",";
        }
        if (_dateTime.Month == 6)
        {
            _monthNumText.text = "June" + ",";
        }
        if (_dateTime.Month == 7)
        {
            _monthNumText.text = "July" + ",";
        }
        if (_dateTime.Month == 8)
        {
            _monthNumText.text = "August" + ",";
        }
        if (_dateTime.Month == 9)
        {
            _monthNumText.text = "September" + ",";
        }
        if (_dateTime.Month == 10)
        {
            _monthNumText.text = "October" + ",";
        }
        if (_dateTime.Month == 11)
        {
            _monthNumText.text = "November" + ",";
        }
        if (_dateTime.Month == 12)
        {
            _monthNumText.text = "December" + ",";
        }
    }

    int GetDays(DayOfWeek day)
    {
        switch (day)
        {
            case DayOfWeek.Monday: return 1;
            case DayOfWeek.Tuesday: return 2;
            case DayOfWeek.Wednesday: return 3;
            case DayOfWeek.Thursday: return 4;
            case DayOfWeek.Friday: return 5;
            case DayOfWeek.Saturday: return 6;
            case DayOfWeek.Sunday: return 0;
        }

        return 0;
    }
    public void YearPrev()
    {
        _dateTime = _dateTime.AddYears(-1);
        CreateCalendar();
    }

    public void YearNext()
    {
        _dateTime = _dateTime.AddYears(1);
        CreateCalendar();
    }

    public void MonthPrev()
    {
        _dateTime = _dateTime.AddMonths(-1);
        CreateCalendar();
        TournamentScript.instance.ResetDayColor();
    }

    public void MonthNext()
    {
        _dateTime = _dateTime.AddMonths(1);
        CreateCalendar();
        TournamentScript.instance.ResetDayColor();
    }

    public void CloseCalenderBackPanel()
    {
        calenderPanelBtn.SetActive(false);
        _calendarPanel.SetActive(false);
        
        startSetTimeObj.SetActive(false);
        lineImageObj.SetActive(false);
    }

    public GameObject startSetTimeObj;
    public GameObject lineImageObj;

    public void ShowCalendar(Text target)
    {
        calenderPanelBtn.SetActive(true);
        _calendarPanel.SetActive(true);
        
        scrollBar.value = 0;
        _target = target;
        
        calenderCount++;
        startSetTimeObj.SetActive(true);
        lineImageObj.SetActive(true);
    }

    Text _target;
    string previousSelectedDate;
    public void OnDateItemClick(string day)
    {        
        if (day.Length == 2)
        {
            if (_dateTime.Month == 10 || _dateTime.Month == 11 || _dateTime.Month == 12)
            {
                _target.text = day + "/" /*+ "0"*/ + _dateTime.Month + "/" + _yearNumText.text;
            }
            else
            {
                _target.text = day + "/" + "0" + _dateTime.Month + "/" + _yearNumText.text;
            }
        }
        else
        {
            if (_dateTime.Month == 10 || _dateTime.Month == 11 || _dateTime.Month == 12)
            {
                _target.text = "0" + day + "/" /*+ "0"*/ + _dateTime.Month + "/" + _yearNumText.text;
            }
            else
            {
                _target.text = "0" + day + "/" + "0" + _dateTime.Month + "/" + _yearNumText.text;
            }
        }
        if (TournamentScript.instance.tournamentObj.activeInHierarchy)
        {
            TournamentScript.instance.startDateVal = _target.text;
        }
        previousSelectedDate = _target.text;
    }
    public string day;

    public string SetDay(int dayNo)
    {
        dayNo = dayNo % 7;

        if (dayNo == 1)
        {
            return "SUN";
        }
        else if (dayNo == 2)
        {
            return "MON";
        }
        else if (dayNo == 3)
        {
            return "TUE";
        }
        else if (dayNo == 4)
        {
            return "WED";
        }
        else if (dayNo == 5)
        {
            return "THU";
        }
        else if (dayNo == 6)
        {
            return "FRI";
        }
        else if (dayNo == 0)
        {
            return "SAT";
        }
        else
        {
            return "";
        }
    }
    public void SetTime()
    {
        _target.text = previousSelectedDate;
        string date = _target.text;
        string _time = Raycaster.instance.startTimeValue.ToString() + ":" + Raycaster.instance.startTimeValue1.ToString();

        string dateTime = _target.text + "    " + _time+":"+"00";
        string dateTime1 = _target.text + " " + _time;
        string dateTime2 = _target.text + " " + _time + ":" + "00";
        string time = _time +":"+ "00";
        _target.text = _target.text + " " + day + "     " + _time;

        _calendarPanel.SetActive(false);
        calenderPanelBtn.SetActive(false);
        startSetTimeObj.SetActive(false);
        lineImageObj.SetActive(false);

        TournamentScript.instance.ConvertTimeToSecondsForCalender(_time);
        if (TournamentScript.instance.tournamentObj.activeInHierarchy)
        {
            TournamentScript.instance.startTimeVal = _time;
        }

        if (TournamentGameDetail.instance.tournamentCanvas.activeInHierarchy)
        {
            ClubManagement.instance.resetStartTime = true;


            TournamentGameDetail.instance.detailImage.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = dateTime;

            TournamentGameDetail.instance.StopCurrentCountDownCorotine();

            ClubManagement.instance.StopCountDown();

            double remainingSec = ClubManagement.instance.TimeDifference(dateTime2);
            TournamentGameDetail.instance.currentCountDownCorotine = ClubManagement.instance.RemainingTimerValue(remainingSec, TournamentGameDetail.instance.timerCountDownText);
            TournamentGameDetail.instance.StartCurrentCountDownCorotine();
            //............Api call for update start time...........//
            TournamentGameDetail.instance.ClickUpdateDateTime(date, _time);

            TournamentGameDetail.instance.mttAdminObj.gameObject.SetActive(false);
        }
    }
    
}
