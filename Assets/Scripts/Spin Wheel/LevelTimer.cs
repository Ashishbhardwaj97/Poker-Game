using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    //public static LevelTimer instance;
    //public Text timerText;

    //public float gameTime = 0f;
    //public float elapsedTime;
    //float startTime;

    //void Awake()
    //{
    //    startTime = Time.time;
    //    instance = this;
    //}

    //void Start()
    //{
    //    Application.runInBackground = true;
    //    gameTime = 0f;
    //}


    //void Update()
    //{
    //    //if(!SpinRotate.instance.isSpin)
    //    //{
    //    //    gameTime = 3600f;
    //    //    SpinRotate.instance.isSpin = true;
    //    //    print(gameTime);
    //    //}

    //    elapsedTime = Time.time - startTime;

    //    int hours = (int)((gameTime - elapsedTime) / (60*24)) % 60;

    //    int minutes = (int)((gameTime - elapsedTime) / 60) % 60;
    //    int seconds = (int)((gameTime - elapsedTime) % 60);
    //    string gameTimerString = string.Format("{00:00}:{01:00}",hours, minutes);

    //    timerText.text = gameTimerString;

    //    //Debug.Log(gameTimerString);

    //    if (elapsedTime >= gameTime)
    //    {
    //        if (SpinRotate.instance.isSpin)
    //        {
    //            SpinRotate.instance.tapToSpinBtn.GetComponent<Button>().interactable = true;
    //            SpinRotate.instance.isSpin = false;
    //        }
    //        // Times up         //Enable free spin              //Hide time strip           //gametime = 3600
    //    }
    //}
}

