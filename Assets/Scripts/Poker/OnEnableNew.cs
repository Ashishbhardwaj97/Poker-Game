using SmartLocalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnEnableNew : MonoBehaviour
{
    public Text subjectText;

    void OnEnable()
    {
        CheckOnEnable();
    }

    void CheckOnEnable()
    {
        if(gameObject.name == "Quit Panel_new")
        {
            if(GameManagerScript.instance.isObserver)
            {
                subjectText.text = LanguageManager.Instance.GetTextValue("leave detail1");//"Are you sure you want to exit.";
            }
            else
            {
                subjectText.text = LanguageManager.Instance.GetTextValue("leave detail"); //"Your cards will be folded if you are in this hand.";
            }
        }
    }
}
