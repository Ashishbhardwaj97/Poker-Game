using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Raycaster1 : MonoBehaviour
{
    public GameObject endTimeRayCaster;
    public GameObject endTimeRayCaster1;
    public GameObject endSetTimePanel;

    public string endTimeValue;
    public string endTimevalue1;
    RaycastHit hit;
    public static Raycaster1 instance;
    //public GameObject setTimePanel;

    private void Awake()
    {
        instance = this;
    }

    void FixedUpdate()
    {
        if (Physics.Raycast(endTimeRayCaster.transform.position, endTimeRayCaster.transform.TransformDirection(Vector3.forward), out hit, 100))
        {
            Debug.DrawLine(endTimeRayCaster.transform.position, endTimeRayCaster.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            endTimeValue = hit.collider.transform.GetComponent<Text>().text;
            if (endTimeValue == "01")
            {
                endSetTimePanel.transform.GetChild(5).GetComponent<Text>().text = " ";
                endSetTimePanel.transform.GetChild(6).GetComponent<Text>().text = (int.Parse(endTimeValue) + 1).ToString();
            }

            else if (endTimeValue == "23")
            {
                endSetTimePanel.transform.GetChild(5).GetComponent<Text>().text = (int.Parse(endTimeValue) - 1).ToString();
                endSetTimePanel.transform.GetChild(6).GetComponent<Text>().text = "00";
            }

            else if (endTimeValue == "00")
            {
                endSetTimePanel.transform.GetChild(5).GetComponent<Text>().text = "23";
                endSetTimePanel.transform.GetChild(6).GetComponent<Text>().text = " ";
            }

            else
            {
                endSetTimePanel.transform.GetChild(5).GetComponent<Text>().text = (int.Parse(endTimeValue) - 1).ToString();
                endSetTimePanel.transform.GetChild(6).GetComponent<Text>().text = (int.Parse(endTimeValue) + 1).ToString();
            }
        }

        if (Physics.Raycast(endTimeRayCaster1.transform.position, endTimeRayCaster1.transform.TransformDirection(Vector3.forward), out hit, 100))
        {
            Debug.DrawLine(endTimeRayCaster1.transform.position, endTimeRayCaster1.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            endTimevalue1 = hit.collider.transform.GetComponent<Text>().text;
            if (endTimevalue1 == "00")
            {
                endSetTimePanel.transform.GetChild(7).GetComponent<Text>().text = " ";
                endSetTimePanel.transform.GetChild(8).GetComponent<Text>().text = (int.Parse(endTimevalue1) + 1).ToString();
            }

            else if (endTimevalue1 == "59")
            {
                endSetTimePanel.transform.GetChild(7).GetComponent<Text>().text = (int.Parse(endTimevalue1) - 1).ToString();
                endSetTimePanel.transform.GetChild(8).GetComponent<Text>().text = " ";
            }

            else
            {
                endSetTimePanel.transform.GetChild(7).GetComponent<Text>().text = (int.Parse(endTimevalue1) - 1).ToString();
                endSetTimePanel.transform.GetChild(8).GetComponent<Text>().text = (int.Parse(endTimevalue1) + 1).ToString();
            }
        }
    }

}
