using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnableScript : MonoBehaviour
{
    public int time;

    private void OnEnable()
    {
        StartCoroutine("TurnOffObject");
    }

    IEnumerator TurnOffObject()
    {
        yield return new WaitForSeconds(time);
        transform.gameObject.SetActive(false);
    }
}
