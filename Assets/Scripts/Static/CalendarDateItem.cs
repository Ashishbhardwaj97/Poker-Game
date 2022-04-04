using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class CalendarDateItem : MonoBehaviour {

    public void OnDateItemClick()
    {
        CalendarController._calendarInstance.OnDateItemClick(gameObject.GetComponentInChildren<Text>().text);
    }

    public void OnDateItemClick1()
    {
        CalendarController1._calendarInstance.OnDateItemClick(gameObject.GetComponentInChildren<Text>().text);
    }

    public void OnDateItemClick2()
    {
        CalendarController2._calendarInstance.OnDateItemClick(gameObject.GetComponentInChildren<Text>().text);
        string dayNo = gameObject.name.Substring(4);        
        CalendarController2._calendarInstance.day = CalendarController2._calendarInstance.SetDay(Convert.ToInt32(dayNo));

        TournamentScript.instance.ResetDayColor();

        var colors = GetComponent<Button>().colors;
        colors.normalColor = new Color32(3, 166, 123, 255);
        GetComponent<Button>().colors = colors;

    }

    public void OnDateItemClick3()
    {
        CalendarController3._calendarInstance.OnDateItemClick(gameObject.GetComponentInChildren<Text>().text);
    }
}
