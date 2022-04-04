using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TracePlayer : MonoBehaviour
{
    
    public void ExpandPanel(Transform panel)
    {

        if (!panel.transform.GetChild(1).gameObject.activeInHierarchy)
        {
            panel.transform.GetChild(0).gameObject.SetActive(false);
            panel.transform.GetChild(1).gameObject.SetActive(true);
            panel.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 475f);

        }
        else
        {
            panel.transform.GetChild(0).gameObject.SetActive(true);
            panel.transform.GetChild(1).gameObject.SetActive(false);
            panel.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 275f);

        }

        if (ClubManagement.instance.onlinePlayingListContent.activeInHierarchy)
        {
            ClubManagement.instance.CalculateTime();
        }
        if (ClubManagement.instance.onlineObservingListContent.activeInHierarchy)
        {
            ClubManagement.instance.CalculateTime1();
        }

        if (DownlineManagementScript.instance.tracePlayerContent.activeInHierarchy)
        {
            DownlineManagementScript.instance.CalculateTime();
        }
        if (DownlineManagementScript.instance.observingTableContent.activeInHierarchy)
        {
            DownlineManagementScript.instance.CalculateTime1();
        }

    }

    
}
