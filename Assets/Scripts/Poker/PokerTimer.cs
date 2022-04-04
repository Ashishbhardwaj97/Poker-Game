using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokerTimer : MonoBehaviour
{
    public float timeRemaining;
    public Text timeText;
    public Image timerImage;
    public float waitTime;
    public GameObject subjectPanel;
    public Button subjectBtn;

    private void OnEnable()
    {
        timeText.text = "" + timeRemaining; 
        StartTimer();
    }

    private void OnDisable()
    {
        timerImage.fillAmount = 1;
        StopCoroutine(popUpTime);

        timeRemaining = 10;
        waitTime = 10;
    }

    IEnumerator popUpTime;
    void StartTimer()
    {
        popUpTime = TimerCoro();
        StartCoroutine(popUpTime);
    }

    IEnumerator TimerCoro()
    {
        yield return new WaitForSeconds(0.1f);
        print(timeRemaining);
        while (timeRemaining >= 0) 
        {
            timeRemaining -= 0.1f;
            timerImage.fillAmount -= 0.1f / waitTime;
            int seconds = (int)timeRemaining;
            timeText.text = "" + seconds;

            yield return new WaitForSeconds(0.1f);
        }
        timeRemaining = 0;
        if (subjectPanel != null)
        {
            TimeUp();
        }
    }

    void TimeUp()
    {

        if(subjectPanel.name == "GameLeftPanel")
        {
            subjectBtn.onClick.Invoke();
        }
        subjectPanel.SetActive(false);


    }
}
