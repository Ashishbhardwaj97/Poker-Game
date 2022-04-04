using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LiftBottomPanel : MonoBehaviour
{
    public int liftValue;
    private void Start()
    {
#if UNITY_ANDROID || UNITY_EDITOR


        gameObject.transform.GetComponent<RectTransform>().localPosition = new Vector3(0, transform.localPosition.y, 0);
        print("localPosition" + gameObject.transform.GetComponent<RectTransform>().localPosition);

        #elif UNITY_IOS
        if (Screen.height >= 667)
        {
            gameObject.transform.GetComponent<RectTransform>().localPosition = new Vector3(0, transform.localPosition.y + liftValue, 0);
            print(gameObject.transform.GetComponent<RectTransform>().localPosition);
        }

        #endif
    }
}
