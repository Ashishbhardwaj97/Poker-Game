using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalendarController3 : MonoBehaviour
{
    public GameObject _calendarPanel;
    public GameObject _calendarBG;
    public GameObject clubData;
    public Text _yearNumText;
    public Text _monthNumText;
    public string month;

    public GameObject _item;

    public List<GameObject> _dateItems = new List<GameObject>();
    const int _totalDateNum = 42;

    private DateTime _dateTime;
    public static CalendarController3 _calendarInstance;
    public int clickCount = 0;
    public Scrollbar scrollBar;

    string currentMonth;
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

        month = DateTime.Now.ToString("MM");
        currentMonth = month;
        print("month.........." + month);
        if (month.Substring(0, 1) == "0")
        {
            month = month.Substring(1);
            
        }
        else
        {
            month = DateTime.Now.ToString("MM");
        }

        if((_dateTime.Month).ToString() == month)
        {
            _calendarBG.transform.GetChild(3).GetComponent<Button>().interactable = false;
        }

        firstDate = "0";
        secondDate = "0";
        allDatesList = new List<GameObject>();
        allDatesList2 = new List<GameObject>();
        if(_item.activeInHierarchy)
        {
            allDatesList.Add(_item);
        }
        for (int i = 19; i < 59; i++)
        {
            if (_calendarBG.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                allDatesList.Add(_calendarBG.transform.GetChild(i).gameObject);
            }
        }
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
        if (firstDate != null && secondDate != "0")
        {
            StartCoroutine(AddDatesInListOnNextAndPrev());
        }
        if (firstDate != null && secondDate == "0")
        {
            StartCoroutine(AddDatesInAnotherListOnNextAndPrev());
        }
        _dateTime = _dateTime.AddMonths(-1);
        CreateCalendar();
        StartCoroutine(HighlightAllDates());

        if ((_dateTime.Month).ToString() != month)
        {
            print("PREV1...");
            _calendarBG.transform.GetChild(3).GetComponent<Button>().interactable = true;
        }

        if (firstDate != "0" & secondDate != "0" && (_dateTime.Month).ToString() != month)
        {
            print("PREV2...");
            _calendarBG.transform.GetChild(2).GetComponent<Button>().interactable = false;
            _calendarBG.transform.GetChild(3).GetComponent<Button>().interactable = true;
        }

        allDatesList.Clear();
        if (_item.activeInHierarchy)
        {
            allDatesList.Add(_item);
        }
        for (int i = 19; i < 59; i++)
        {
            if (_calendarBG.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                allDatesList.Add(_calendarBG.transform.GetChild(i).gameObject);
            }
        }
    }

    public void MonthNext()
    {
        if (firstDate != null && secondDate != "0")
        {
            StartCoroutine(AddDatesInListOnNextAndPrev());
        }
        if(firstDate != null && secondDate == "0")
        {

        }
        _dateTime = _dateTime.AddMonths(1);
        CreateCalendar();
        StartCoroutine(HighlightAllDates());
        print("_dateTime.Month " + _dateTime.Month);
        if ((_dateTime.Month).ToString() == firstDateMonth.ToString())
        {
            print("NEXT1...");
            _calendarBG.transform.GetChild(2).GetComponent<Button>().interactable = true;
        }

        if (firstDate != "0" & secondDate != "0" && (_dateTime.Month).ToString() == firstDateMonth.ToString())
        {
            print("NEXT2...");
            _calendarBG.transform.GetChild(2).GetComponent<Button>().interactable = true;
            _calendarBG.transform.GetChild(3).GetComponent<Button>().interactable = false;
        }

        allDatesList.Clear();
        if (_item.activeInHierarchy)
        {
            allDatesList.Add(_item);
        }
        for (int i = 19; i < 59; i++)
        {
            if (_calendarBG.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                allDatesList.Add(_calendarBG.transform.GetChild(i).gameObject);
            }
        }

        if (int.Parse(currentMonth) == _dateTime.Month)
        {
            _calendarBG.transform.GetChild(3).GetComponent<Button>().interactable = false;
        }
    }

    public void CloseCalenderBackPanel()
    {
        _calendarPanel.SetActive(false);
        //calenderUpperPanelBtn.SetActive(false);
    }

    public void ShowCalendar(Text target)
    {
        _calendarPanel.SetActive(true);
        //calenderUpperPanelBtn.SetActive(true);
        scrollBar.value = 0;
        _target = target.ToString();
        print("SHOW..."+_target);
    }

    #region Club-Data DateRange Selection

    public string _target;
    public string _target1;
    public string _day;
    public void OnDateItemClick(string day)
    {
        _day = day;
        if(clickCount == 0)
        {
            ResetCalender();
            StartCoroutine(FirstDateDelay());
        }
        else
        {
            SecondDate();
            clickCount--;
        }
    }

    public IEnumerator FirstDateDelay()
    {
        yield return new WaitForSeconds(0.1f);
        FirstDate();
        clickCount++;
    }

    public string firstDate;
    public int firstDateMonth;
    public string secondDate;
    public int secondDateMonth;
    internal bool dateRange;
    public void FirstDate()
    {
        firstDate = _day;
        firstDateMonth = _dateTime.Month;

        if(_dateTime.Month == int.Parse(month))
        {
            _calendarBG.transform.GetChild(3).GetComponent<Button>().interactable = false;
        }
    }

    public void SecondDate()
    {
        secondDate = _day;
        secondDateMonth = _dateTime.Month;
        if (firstDate != null && secondDate != null)
        {
            StartCoroutine(HighlightAllDates());
            _calendarBG.transform.GetChild(2).GetComponent<Button>().interactable = false;
            //_calendarBG.transform.GetChild(3).GetComponent<Button>().interactable = false;
        }
    }
    public List<GameObject> allDatesList;
    public List<GameObject> allDatesList2;
    public IEnumerator HighlightAllDates()
    {
        yield return new WaitForSeconds(0.3f);
        if (secondDate != "0") 
        {
            if (allDatesList2.Count == 0)
            {
                print("enter1....");
                if (int.Parse(firstDate) < int.Parse(secondDate))
                {
                    print("FIRSTDATE");
                    _calendarBG.transform.GetChild(3).GetComponent<Button>().interactable = false;
                    for (int i = int.Parse(firstDate); i <= int.Parse(secondDate); i++)
                    {
                        allDatesList[i - 1].transform.GetChild(0).gameObject.SetActive(true);
                    }

                    for (int i = (int.Parse(firstDate)) + 1; i <= (int.Parse(secondDate)) - 1; i++)
                    {
                        allDatesList[i - 1].transform.GetChild(0).GetComponent<Image>().color = new Color32(3, 168, 124, 40);
                    }
                }
                else
                {
                    print("SecondDATE");
                    for (int i = int.Parse(firstDate); i >= int.Parse(secondDate); i--)
                    {
                        allDatesList[i - 1].transform.GetChild(0).gameObject.SetActive(true);
                    }

                    for (int i = (int.Parse(firstDate)) - 1; i >= (int.Parse(secondDate)) + 1; i--)
                    {
                        allDatesList[i - 1].transform.GetChild(0).GetComponent<Image>().color = new Color32(3, 168, 124, 40);
                    }
                }
            }

            else if (Math.Abs(firstDateMonth - _dateTime.Month) == 1) 
            {
                print("2nd....");
                for (int i = 0; i < allDatesList.Count; i++)
                {
                    allDatesList[i].transform.GetChild(0).gameObject.SetActive(false);
                }

                for (int i = int.Parse(secondDate); i <= allDatesList2.Count; i++)
                {
                    allDatesList2[i - 1].transform.GetChild(0).gameObject.SetActive(true);
                }

                for (int i = int.Parse(secondDate); i <= allDatesList2.Count; i++)
                {
                    allDatesList2[i - 1].transform.GetChild(0).GetComponent<Image>().color = new Color32(3, 168, 124, 40);
                }
            }

            else if (firstDateMonth == 1 || firstDateMonth == 2)
            {
                if (firstDateMonth == 1)
                {
                    print("3rd....");
                    if ((_dateTime.Month != 1) && (_dateTime.Month != 12))
                    {
                        for (int i = 0; i < allDatesList.Count; i++)
                        {
                            allDatesList[i].transform.GetChild(0).gameObject.SetActive(false);
                        }

                        for (int i = 0; i < allDatesList2.Count; i++)
                        {
                            allDatesList2[i].transform.GetChild(0).gameObject.SetActive(false);
                        }
                    }

                    if(_dateTime.Month == 1)
                    {
                        for (int i = 0; i < allDatesList2.Count; i++)
                        {
                            allDatesList2[i].transform.GetChild(0).gameObject.SetActive(false);
                        }

                        for (int i = int.Parse(firstDate); i >= 1; i--)
                        {
                            allDatesList[i - 1].transform.GetChild(0).gameObject.SetActive(true);
                        }

                        for (int i = (int.Parse(firstDate) - 1); i >= 1; i--)
                        {
                            allDatesList[i - 1].transform.GetChild(0).GetComponent<Image>().color = new Color32(3, 168, 124, 40);
                        }
                    }

                    if(_dateTime.Month == 12)
                    {
                        for (int i = 0; i < allDatesList.Count; i++)
                        {
                            allDatesList[i].transform.GetChild(0).gameObject.SetActive(false);
                        }

                        for (int i = int.Parse(secondDate); i <= allDatesList2.Count; i++)
                        {
                            allDatesList2[i - 1].transform.GetChild(0).gameObject.SetActive(true);
                        }

                        for (int i = int.Parse(secondDate); i <= allDatesList2.Count; i++)
                        {
                            allDatesList2[i - 1].transform.GetChild(0).GetComponent<Image>().color = new Color32(3, 168, 124, 40);
                        }
                    }
                }

                else if (firstDateMonth == 2)
                {
                    print("4th....");
                    if (_dateTime.Month != 1 && _dateTime.Month != 2)
                    {
                        for (int i = 0; i < allDatesList.Count; i++)
                        {
                            allDatesList[i].transform.GetChild(0).gameObject.SetActive(false);
                        }

                        for (int i = 0; i < allDatesList2.Count; i++)
                        {
                            allDatesList2[i].transform.GetChild(0).gameObject.SetActive(false);
                        }
                    }

                    if (_dateTime.Month == 1)
                    {
                        for (int i = 0; i < allDatesList.Count; i++)
                        {
                            allDatesList[i].transform.GetChild(0).gameObject.SetActive(false);
                        }

                        for (int i = int.Parse(secondDate); i <= allDatesList2.Count; i++)
                        {
                            allDatesList2[i - 1].transform.GetChild(0).gameObject.SetActive(true);
                        }

                        for (int i = int.Parse(secondDate); i <= allDatesList2.Count; i++)
                        {
                            allDatesList2[i - 1].transform.GetChild(0).GetComponent<Image>().color = new Color32(3, 168, 124, 40);
                        }
                    }

                    if (_dateTime.Month == 2)
                    {
                        for (int i = 0; i < allDatesList2.Count; i++)
                        {
                            allDatesList2[i].transform.GetChild(0).gameObject.SetActive(false);
                        }

                        for (int i = int.Parse(firstDate); i >= 1; i--)
                        {
                            allDatesList[i - 1].transform.GetChild(0).gameObject.SetActive(true);
                        }

                        for (int i = (int.Parse(firstDate) - 1); i >= 1; i--)
                        {
                            allDatesList[i - 1].transform.GetChild(0).GetComponent<Image>().color = new Color32(3, 168, 124, 40);
                        }
                    }
                }
            }

            else if (firstDateMonth-2 >= (_dateTime.Month))
            {
                print("5th....");
                for (int i = 0; i < allDatesList.Count; i++)
                {
                    allDatesList[i].transform.GetChild(0).gameObject.SetActive(false);
                }

                for (int i = 0; i < allDatesList2.Count; i++)
                {
                    allDatesList2[i].transform.GetChild(0).gameObject.SetActive(false);
                }
            }

            else
            {
                print("6th....");
                print("Enter2...");
                if ((_dateTime.Month).ToString() == (_dateTime.Month).ToString()/*_yearNumText.text == DateTime.Now.ToString("yyyy")*/)
                {
                    print("1st....");
                    for (int i = 0; i < allDatesList2.Count; i++)
                    {
                        allDatesList2[i].transform.GetChild(0).gameObject.SetActive(false);
                    }

                    for (int i = int.Parse(firstDate); i >= 1; i--)
                    {
                        allDatesList[i - 1].transform.GetChild(0).gameObject.SetActive(true);
                    }

                    for (int i = (int.Parse(firstDate) - 1); i >= 1; i--)
                    {
                        allDatesList[i - 1].transform.GetChild(0).GetComponent<Image>().color = new Color32(3, 168, 124, 40);
                    }
                }
            }
        }
    }

    public void ConfirmDates()
    {
        for (int i = 0; i < Admin.instance.daysImagesList.Count; i++)
        {
            Admin.instance.daysImagesList[i].transform.GetChild(0).gameObject.SetActive(false);
        }

        dateRange = true;
        if (allDatesList2.Count == 0)
        {
            for (int i = 0; i < allDatesList.Count; i++)
            {
                if (allDatesList[i].transform.GetChild(0).gameObject.activeInHierarchy)
                {
                    if (_dateTime.Month < 10)
                    {
                        _target = allDatesList[i].transform.GetChild(1).GetComponent<Text>().text + "/" + "0" + _dateTime.Month + "/" + _yearNumText.text;
                    }
                    else
                    {
                        _target = allDatesList[i].transform.GetChild(1).GetComponent<Text>().text + "/" + _dateTime.Month + "/" + _yearNumText.text;
                    }
                    print("_target " + _target);
                    break;
                }
            }

            for (int i = allDatesList.Count - 1; i >= 0; i--)
            {
                if (allDatesList[i].transform.GetChild(0).gameObject.activeInHierarchy)
                {
                    if (_dateTime.Month < 10)
                    {
                        _target1 = allDatesList[i].transform.GetChild(1).GetComponent<Text>().text + "/" + "0" + _dateTime.Month + "/" + _yearNumText.text;
                    }
                    else
                    {
                        _target1 = allDatesList[i].transform.GetChild(1).GetComponent<Text>().text + "/" + _dateTime.Month + "/" + _yearNumText.text;
                    }
                    print("_target1 " + _target1);
                    break;
                }
            }
        }

        else
        {
            for (int i = 0; i < allDatesList2.Count; i++)
            {
                if (firstDateMonth > secondDateMonth && firstDateMonth < 10/*_dateTime.Month < 10*/) 
                {
                    print("<10");
                    _target = firstDate + "/" + "0" + firstDateMonth + "/" + _yearNumText.text;
                }
                else if(firstDateMonth > secondDateMonth && firstDateMonth >= 10)
                {
                    print(">10");
                    _target = firstDate + "/" + firstDateMonth + "/" + _yearNumText.text;
                }
                 print("_target " + _target);
                 break;
            }
            for (int i = allDatesList.Count - 1; i >= 0; i--)
            {
                if (allDatesList[i].transform.GetChild(0).gameObject.activeInHierarchy)
                {
                    print("A...");
                    if (firstDateMonth == 1)
                    {
                        print("B...");
                        _target1 = allDatesList[i].transform.GetChild(1).GetComponent<Text>().text + "/" + "0" + _dateTime.Month + "/" + (int.Parse(_yearNumText.text) + 1);
                    }
                    else
                    {
                        print("C...");
                        if (_dateTime.Month < 10)
                        {
                            print("D...");
                            _target1 = allDatesList[i].transform.GetChild(1).GetComponent<Text>().text + "/" + "0" + _dateTime.Month + "/" + _yearNumText.text;
                        }
                        else
                        {
                            print("E...");
                            _target1 = allDatesList[i].transform.GetChild(1).GetComponent<Text>().text + "/" + _dateTime.Month + "/" + _yearNumText.text;
                        }
                    }
                    print("_target1 " + _target1);
                    break;
                }
            }
            //if (_dateTime.Month == 12)
            //{
            //    print("F...");
            //    _target1 = firstDate + "/" + "0" + 1 + "/" + (int.Parse(_yearNumText.text) + 1);
            //}
            //else
            //{
            //    print("G...");
            //    _target1 = firstDate + "/" + (_dateTime.Month + 1) + "/" + _yearNumText.text;
            //}
        }
        clubData.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = _target + " " + "-" + " " + _target1;

        print("_Finaltarget " + clubData.transform.GetChild(3).GetChild(0).GetComponent<Text>().text);

        totalDays = 4;
        Admin.instance.ClubDataRequest(totalDays);
        _calendarPanel.SetActive(false);
        ResetCalender();
    }

    public void ResetCalender()
    {
        for (int i = 0; i <= allDatesList.Count - 1; i++)
        {
            allDatesList[i].transform.GetChild(0).gameObject.SetActive(false);
            allDatesList[i].transform.GetChild(0).GetComponent<Image>().color = new Color32(3, 168, 124, 255);
        }

        for (int i = 0; i <= allDatesList2.Count - 1; i++)
        {
            allDatesList2[i].transform.GetChild(0).gameObject.SetActive(false);
            allDatesList2[i].transform.GetChild(0).GetComponent<Image>().color = new Color32(3, 168, 124, 255);
        }

        _calendarBG.transform.GetChild(2).GetComponent<Button>().interactable = true;
        _calendarBG.transform.GetChild(3).GetComponent<Button>().interactable = true;
        firstDate = "0";
        secondDate = "0";
        clickCount = 0;
        firstDateMonth = 0;
        secondDateMonth = 0;

        allDatesList2.Clear();
        allDatesList.Clear();

        if ((_dateTime.Month).ToString() == month)
        {
            _calendarBG.transform.GetChild(3).GetComponent<Button>().interactable = false;
        }
    }

    public IEnumerator AddDatesInListOnNextAndPrev()
    {
        yield return new WaitForSeconds(0.1f);
        allDatesList.Clear();
        yield return new WaitForSeconds(0.1f);
        if (_item.activeInHierarchy)
        {
            allDatesList.Add(_item);
        }
        for (int i = 19; i < 59; i++)
        {
            if (_calendarBG.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                allDatesList.Add(_calendarBG.transform.GetChild(i).gameObject);
            }
        }
    }

    public void OpenCalender()
    {
        _calendarBG.transform.GetChild(2).GetComponent<Button>().interactable = true;
    }

    public IEnumerator AddDatesInAnotherListOnNextAndPrev()
    {
        print("NEW FUNC");
        yield return new WaitForSeconds(0.1f);
        if (allDatesList2.Count == 0)
        {
            if (_item.activeInHierarchy)
            {
                allDatesList2.Add(_item);
            }
            for (int i = 19; i < 59; i++)
            {
                if (_calendarBG.transform.GetChild(i).gameObject.activeInHierarchy)
                {
                    allDatesList2.Add(_calendarBG.transform.GetChild(i).gameObject);
                }
            }
        }
    }
    #endregion

    public string todaysDate;
    public string prevDate;
    public string prevMonthDate;
    public int totalDays;
    

    public void NoOfDays(int days)
    {
        if(days == 1)
        {
            totalDays = 1;
        }
        else if(days == 7)
        {
            totalDays = 2;
        }
        else if (days == 14)
        {
            totalDays = 3;
        }
        print(totalDays);
        Admin.instance.ClubDataRequest(totalDays);

        todaysDate = DateTime.Now.ToString("dd");
        prevDate = (int.Parse(todaysDate) - days).ToString();
        if ((int.Parse(todaysDate) - days) > 0)
        {
            clubData.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = prevDate + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("yyyy") + " " + "-" + " " + todaysDate + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("yyyy");
        }
        else
        {
            if (DateTime.Now.ToString("MM") == "03" || DateTime.Now.ToString("MM") == "05" || DateTime.Now.ToString("MM") == "07" || DateTime.Now.ToString("MM") == "08" || DateTime.Now.ToString("MM") == "10" || DateTime.Now.ToString("MM") == "12")
            {
                print("31st....");
                prevMonthDate = (31 - days + int.Parse(todaysDate)).ToString();
                clubData.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = prevMonthDate + "/" + (int.Parse(DateTime.Now.ToString("MM")) - 1).ToString() + "/" + DateTime.Now.ToString("yyyy") + " " + "-" + " " + todaysDate + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("yyyy");
            }
            else if(DateTime.Now.ToString("MM") == "04" || DateTime.Now.ToString("MM") == "06" || DateTime.Now.ToString("MM") == "09" || DateTime.Now.ToString("MM") == "11")
            {
                print("30th....");
                prevMonthDate = (30 - days + int.Parse(todaysDate)).ToString();
                clubData.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = prevMonthDate + "/" + (int.Parse(DateTime.Now.ToString("MM")) - 1).ToString() + "/" + DateTime.Now.ToString("yyyy") + " " + "-" + " " + todaysDate + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("yyyy");
            }
            else if(DateTime.Now.ToString("MM") == "02")
            {
                print("28th....");
                prevMonthDate = (28 - days + int.Parse(todaysDate)).ToString();
                clubData.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = prevMonthDate + "/" + (int.Parse(DateTime.Now.ToString("MM")) - 1).ToString() + "/" + DateTime.Now.ToString("yyyy") + " " + "-" + " " + todaysDate + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("yyyy");
            }
            else if (DateTime.Now.ToString("MM") == "01")
            {
                print("31th....1st");
                prevMonthDate = (31 - days + int.Parse(todaysDate)).ToString();
                clubData.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = prevMonthDate + "/" + "12" + "/" + (int.Parse(DateTime.Now.ToString("yyyy")) - 1).ToString() + " " + "-" + " " + todaysDate + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("yyyy");
            }
        }
    }
}