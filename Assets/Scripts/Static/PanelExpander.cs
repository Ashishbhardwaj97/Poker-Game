using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelExpander : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        Invoke("ChangeHeight", .2f);
        

    }

    void ChangeHeight()
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 275f);
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
    }

   
}
