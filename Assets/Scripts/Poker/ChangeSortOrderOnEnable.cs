using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSortOrderOnEnable : MonoBehaviour
{
    public int time;
    private void OnEnable()
    {
        StartCoroutine("TurnOffObject");
    }

    IEnumerator TurnOffObject()
    {
        //yield return new WaitForSeconds(time);
        yield return new WaitForSeconds(0.1f);
        transform.GetComponent<Canvas>().overrideSorting = true;
        transform.GetComponent<Canvas>().sortingOrder = 3;
        //transform.gameObject.SetActive(false);
    }
}
