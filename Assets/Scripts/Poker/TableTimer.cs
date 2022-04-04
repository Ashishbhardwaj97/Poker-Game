using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TableTimer : MonoBehaviour
{
    public static TableTimer instance;

    public Text runningTimeText, remainingTimeText; // Table Menu Time Text UI
    public Text tableName, tableBlinds; // Table Menu Time Text UI
    public string tableStartTimeString, tableEndTimeString;  // To Save the variable from Table Property

    string[] splitStartTimeStrings, splitEndTimeStrings; // String Splitters for Time string
    int totalTableRunningTime, totalTableRemainingTime; // Total Time Variables


    public Text runningTimeTextVideo, remainingTimeTextVideo; // Table Menu Time Text UI
    public Text tableNameVideo, tableBlindsVideo; // Table Menu Time Text UI

    public Text runningTimeTextNonVideo, remainingTimeTextNonVideo; // Table Menu Time Text UI
    public Text tableNameNonVideo, tableBlindsNonVideo; // Table Menu Time Text UI


    [Serializable]
    public class TableTimeHoursAndMinutes
    {
        public int hours;
        public int minutes;
    }

    [HideInInspector]
    public TableTimeHoursAndMinutes tableStartTime;
    [HideInInspector]
    public TableTimeHoursAndMinutes tableEndTime;
    [HideInInspector]
    public TableTimeHoursAndMinutes localSytemTime;

    public Text tournamentTimerText;
    public bool activeTournamentTimer;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

        //// Call this to start Total Running Timer while selecting a table to play
        //StartRunningTimeCoroutine();
        //// Call this to Stop Total Running Timer while exiting the table
        //StopRunningTimeCoroutine();



        //// Call this to start Total Remaining Timer while selecting a table to play
        //StartRemainingTimeCoroutine();
        //// Call this to Stop Total Remaining Timer while exiting the table
        //StopRemainingTimeCoroutine();


    }


    // Call this to start Total Running Timer while selecting a table to play
    public void StartRunningTimeCoroutine()
    {
        //try
        //{
            localSytemTime.hours = System.DateTime.Now.Hour;
            localSytemTime.minutes = System.DateTime.Now.Minute;
            
            splitStartTimeStrings = tableStartTimeString.Split(new string[] { ":" }, StringSplitOptions.None);
            print("0th"+splitStartTimeStrings[0]);
            print("1st"+splitStartTimeStrings[1]);
            tableStartTime.hours = int.Parse(splitStartTimeStrings[0]);
            tableStartTime.minutes = int.Parse(splitStartTimeStrings[1]);

            totalTableRunningTime = (localSytemTime.hours - tableStartTime.hours) * 3600;
            print("Remaining time Seconds 1111 " + totalTableRunningTime);
            totalTableRunningTime = totalTableRunningTime + (localSytemTime.minutes - tableStartTime.minutes) * 60;
            print("Remaining time Seconds " + totalTableRunningTime + (localSytemTime.minutes - tableStartTime.minutes) * 60);
            StartCoroutine("RunningTimerValue");
        
        //catch
        //{
        //    print("Exception");
        //}
    }

    // Call this to Stop Total Running Timer while exiting the table
    public void StopRunningTimeCoroutine()
    {
        StopCoroutine("RunningTimerValue");
    }


    IEnumerator RunningTimerValue()
    {
        RunningDisplayTime(totalTableRunningTime);
        yield return new WaitForSeconds(2f);
        while (true)
        {
            totalTableRunningTime += 1;
            RunningDisplayTime(totalTableRunningTime);
            yield return new WaitForSeconds(1);
            if (totalTableRemainingTime <= 0)
            {
                Debug.Log("Table has Ended!");
                break;
            }

        }

    }


    // To Display the Time
    void RunningDisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float hours = Mathf.FloorToInt(timeToDisplay / 3600);
        float minutes = Mathf.FloorToInt(timeToDisplay / 60) % 60;
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        runningTimeText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }


    // Call this to start Total Remaining Timer while selecting a table to play
    public void StartRemainingTimeCoroutine()
    {
        localSytemTime.hours = System.DateTime.Now.Hour;
        localSytemTime.minutes = System.DateTime.Now.Minute;

        splitEndTimeStrings = tableEndTimeString.Split(new string[] { ":" }, StringSplitOptions.None);
        tableEndTime.hours = int.Parse(splitEndTimeStrings[0]);
        tableEndTime.minutes = int.Parse(splitEndTimeStrings[1]);

        totalTableRemainingTime = (tableEndTime.hours - localSytemTime.hours) * 3600;
        print("Remaining time Seconds 1111 " + totalTableRemainingTime);
        totalTableRemainingTime = totalTableRemainingTime + (tableEndTime.minutes - localSytemTime.minutes) * 60;
        print("Remaining time Seconds " + totalTableRemainingTime + (tableEndTime.minutes - localSytemTime.minutes) * 60);
        StartCoroutine("RemainingTimerValue");
    }

    // Call this to Stop Total Remaining Timer while exiting the table
    public void StopRemainingTimeCoroutine()
    {
        StopCoroutine("RemainingTimerValue");
    }



    IEnumerator RemainingTimerValue()
    {
        while (true)
        {
            if (totalTableRemainingTime > 0)
            {
                totalTableRemainingTime -= 1;
                RemainingDisplayTime(totalTableRemainingTime);
                yield return new WaitForSeconds(1);
            }

            else
            {
                Debug.Log("Table has Ended!");
                totalTableRemainingTime = 0;
                break;
            }
        }
    }

    // To Display the Time
    //void RemainingDisplayTime(float timeToDisplay)
    void RemainingDisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float hours = Mathf.FloorToInt(timeToDisplay / 3600);
        float minutes = Mathf.FloorToInt(timeToDisplay / 60) % 60;
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        remainingTimeText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        if (activeTournamentTimer)
        {
            tournamentTimerText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        }
    }

}