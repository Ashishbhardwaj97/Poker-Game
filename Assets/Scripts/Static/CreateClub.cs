using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateClub : MonoBehaviour
{
    public GameObject scrollView;
    public GameObject scheduleClubBtn;
    public GameObject scheduleClubPanel;
    public GameObject nextBtn;
    public GameObject scrollBar;
    int scheduleCount;

    public void ShowBtnPanel()
    {
        scheduleCount++;
        if (scheduleCount % 2 == 0)
        {
            scheduleClubBtn.SetActive(true);
            scheduleClubPanel.SetActive(false);
            nextBtn.transform.localPosition = new Vector3(0, transform.localPosition.y - 270);
            scrollBar.GetComponent<Scrollbar>().value = 1;
            scrollView.GetComponent<ScrollRect>().vertical = false;
        }
        else
        {
            scheduleClubBtn.SetActive(false);
            scheduleClubPanel.SetActive(true);
            nextBtn.transform.localPosition = new Vector3(0, transform.localPosition.y - 1300);
            scrollView.GetComponent<ScrollRect>().vertical = true;
        }
    }
}
