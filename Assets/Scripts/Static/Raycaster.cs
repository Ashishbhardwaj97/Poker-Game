using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Raycaster : MonoBehaviour
{
    public GameObject startTimeRayCaster;
    public GameObject startTimeRayCaster1;
    public GameObject startSetTimePanel;

    public string startTimeValue;
    public string startTimeValue1;

    RaycastHit hit;
    public static Raycaster instance;

    private void Awake()
    {
        instance = this;
    }

    void FixedUpdate()
    {
        if (Physics.Raycast(startTimeRayCaster.transform.position, startTimeRayCaster.transform.TransformDirection(Vector3.forward), out hit, 100))
        {
            Debug.DrawLine(startTimeRayCaster.transform.position, startTimeRayCaster.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            startTimeValue = hit.collider.transform.GetComponent<Text>().text;
            if (startTimeValue == "01")
            {
                startSetTimePanel.transform.GetChild(5).GetComponent<Text>().text = " ";
                startSetTimePanel.transform.GetChild(6).GetComponent<Text>().text = (int.Parse(startTimeValue) + 1).ToString();
            }

            else if(startTimeValue == "23")
            {
                startSetTimePanel.transform.GetChild(5).GetComponent<Text>().text = (int.Parse(startTimeValue) - 1).ToString();
                startSetTimePanel.transform.GetChild(6).GetComponent<Text>().text = "00";
            }

            else if (startTimeValue == "00")
            {
                startSetTimePanel.transform.GetChild(5).GetComponent<Text>().text = "23";
                startSetTimePanel.transform.GetChild(6).GetComponent<Text>().text = " ";
            }

            else
            {
                startSetTimePanel.transform.GetChild(5).GetComponent<Text>().text = (int.Parse(startTimeValue) - 1).ToString();
                startSetTimePanel.transform.GetChild(6).GetComponent<Text>().text = (int.Parse(startTimeValue) + 1).ToString();
            }
        }

        if (Physics.Raycast(startTimeRayCaster1.transform.position, startTimeRayCaster1.transform.TransformDirection(Vector3.forward), out hit, 100))
        {
            Debug.DrawLine(startTimeRayCaster1.transform.position, startTimeRayCaster1.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            startTimeValue1 = hit.collider.transform.GetComponent<Text>().text;
            if (startTimeValue1 == "00")
            {
                startSetTimePanel.transform.GetChild(7).GetComponent<Text>().text = " ";
                startSetTimePanel.transform.GetChild(8).GetComponent<Text>().text = (int.Parse(startTimeValue1) + 1).ToString();
            }

            else if(startTimeValue1 == "59")
            {
                startSetTimePanel.transform.GetChild(7).GetComponent<Text>().text = (int.Parse(startTimeValue1) - 1).ToString();
                startSetTimePanel.transform.GetChild(8).GetComponent<Text>().text = " ";
            }

            else
            {
                startSetTimePanel.transform.GetChild(7).GetComponent<Text>().text = (int.Parse(startTimeValue1) - 1).ToString();
                startSetTimePanel.transform.GetChild(8).GetComponent<Text>().text = (int.Parse(startTimeValue1) + 1).ToString();
            }
        }
        //else
        //{
        //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        //}
    }
}
