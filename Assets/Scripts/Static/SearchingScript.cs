using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchingScript : MonoBehaviour
{
    public static SearchingScript instance;
    public List<Transform> memberListName;
    public List<Transform> memberListNameInTrade;
    public List<Transform> memberListNameInAssignDownline;
    public List<Transform> memberListNameInDownlineMember;
    public List<Transform> memberListNameInCreateTable;
    public List<Transform> memberListNameInChipReqDetail;
    public List<Transform> memberListNameInTradeHistory;
    public List<Transform> memberListNameAfterSearch;
    public List<Transform> memberListNameInComposeMsg;

    public List<Transform> listName;

    private List<Transform> newList;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        listName = new List<Transform>();
        memberListName = new List<Transform>();
        memberListNameInTrade = new List<Transform>();
        memberListNameInTradeHistory = new List<Transform>();
        memberListNameInChipReqDetail = new List<Transform>();
        memberListNameAfterSearch = new List<Transform>();
        memberListNameInAssignDownline = new List<Transform>();
        memberListNameInDownlineMember = new List<Transform>();
        memberListNameInCreateTable = new List<Transform>();
        memberListNameInCreateTable = new List<Transform>();
        memberListNameInComposeMsg = new List<Transform>();
        countryListName = new List<Transform>();
        languageListName = new List<Transform>();

        newList = new List<Transform>();
    }

    public void FindNames(InputField input)
    {
        StartCoroutine(FindNamesCorotine(input, listName));

    }
    public void FindName(InputField input)
    {
        StartCoroutine(FindNameCorotine(input, memberListName, 1, 2));
    }

    public void FindNameInTrade(InputField input)
    {
        StartCoroutine(FindNameCorotine(input, memberListNameInTrade, 0, 1));
    }

    public void FindNameInTradeHistory(InputField input)
    {
        StartCoroutine(FindNameCorotine(input, memberListNameInTradeHistory, 0, 1, 3, 4));
    }

    public void FindNameInChipReqDetails(InputField input)
    {
        StartCoroutine(FindNameCorotine(input, memberListNameInChipReqDetail, 1, 3));
    }

    public void FindNameInAssignDownline(InputField input)
    {
        StartCoroutine(FindNameCorotine(input, memberListNameInAssignDownline, 1, 2));
    }

    public void FindNameInDownlineMember(InputField input)
    {
        StartCoroutine(FindNameCorotine(input, memberListNameInDownlineMember, 1, 2));
    }

    public void FindNameInCreateTable(InputField input)
    {
        StartCoroutine(FindNameCorotine(input, memberListNameInCreateTable, 1, 2));
    }

    public void FindNameInComposeMsg(InputField input)
    {
        StartCoroutine(FindNameCorotine(input, memberListNameInComposeMsg, 1, 2));
    }

    #region Searching Logic

    IEnumerator FindNamesCorotine(InputField input, List<Transform> _nameList)
    {
        yield return new WaitForSeconds(0.1f);
        memberListNameAfterSearch = SearchByName(input.textComponent.text, _nameList);
        NewObjList(_nameList, memberListNameAfterSearch);

    }

    public List<Transform> SearchByName(string str, List<Transform> nameList)
    {
        newList.Clear();
        for (int i = 0; i < nameList.Count; i++)
        {
            if (nameList[i].GetChild(0).GetComponent<Text>().text.ToLower().Contains(str.ToLower()) )
            {
                newList.Add(nameList[i]);
            }
        }

        return newList;
    }


    IEnumerator FindNameCorotine(InputField input, List<Transform> _nameList, int _nameIndex, int idIndex)
    {
        yield return new WaitForSeconds(0.1f);
        memberListNameAfterSearch = SearchByName(input.textComponent.text, _nameList, _nameIndex,idIndex);
        NewObjList(_nameList, memberListNameAfterSearch);

    }

   

    public List<Transform> SearchByName(string str, List<Transform> nameList, int _nameIndex,int idIndex)
    {
        newList.Clear();
        for (int i = 0; i < nameList.Count; i++)
        {
            if (nameList[i].GetChild(_nameIndex).GetComponent<Text>().text.ToLower().Contains(str.ToLower()) || nameList[i].GetChild(idIndex).GetComponent<Text>().text.ToLower().Contains(str.ToLower()))
            {
                newList.Add(nameList[i]);
            }
        }

        return newList;
    }

    IEnumerator FindNameCorotine(InputField input, List<Transform> _nameList, int _nameIndex, int idIndex, int _nameIndex1, int idIndex1)
    {
        yield return new WaitForSeconds(0.1f);
        memberListNameAfterSearch = SearchByName(input.textComponent.text, _nameList, _nameIndex, idIndex, _nameIndex1, idIndex1);
        NewObjList(_nameList, memberListNameAfterSearch);
        
    }

    public List<Transform> SearchByName(string str, List<Transform> nameList, int _nameIndex, int idIndex, int _nameIndex1, int idIndex1)
    {
        newList.Clear();
        for (int i = 0; i < nameList.Count; i++)
        {
            
            if (nameList[i].GetChild(_nameIndex).GetComponent<Text>().text.ToLower().Contains(str.ToLower()) || nameList[i].GetChild(idIndex).GetComponent<Text>().text.ToLower().Contains(str.ToLower()) ||

                nameList[i].GetChild(_nameIndex1).GetComponent<Text>().text.ToLower().Contains(str.ToLower()) || nameList[i].GetChild(idIndex1).GetComponent<Text>().text.ToLower().Contains(str.ToLower())
                )
            {
                newList.Add(nameList[i]);
            }
        }

        return newList;
    }

    public void NewObjList(List<Transform> oldList, List<Transform> newList)
    {
        for (int i = 0; i < oldList.Count; i++)
        {
            oldList[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < newList.Count; i++)
        {
            newList[i].gameObject.SetActive(true);
        }

        if (newList.Count == 0)
        {
            if (Profile._instance.dropDownCountryPanel.activeInHierarchy)
            {
                Profile._instance.dropDownCountryPanel.transform.GetChild(3).GetComponent<Button>().interactable = false;
            }
            else if (Registration.instance.dropDownCountryPanel.activeInHierarchy)
            {
                Registration.instance.dropDownCountryPanel.transform.GetChild(3).GetComponent<Button>().interactable = false;
            }
            else if (TablePreferenceScript.instance.myAgeParentObj.activeInHierarchy)
            {
                TablePreferenceScript.instance.myAgeParentObj.transform.GetChild(2).GetChild(0).GetComponent<Button>().interactable = false;
            }
            else if (TablePreferenceScript.instance.tableMatchObj.activeInHierarchy)
            {
                for (int i = 0; i < TablePreferenceScript.instance.preferenceValueParent.transform.childCount; i++)
                {
                    if (TablePreferenceScript.instance.preferenceValueParent.transform.GetChild(i).gameObject.activeInHierarchy)
                    {
                        TablePreferenceScript.instance.preferenceValueParent.transform.GetChild(i).GetChild(2).GetChild(0).GetComponent<Button>().interactable = false;
                    }
                }
            }


        }
        else
        {
            if (Profile._instance.dropDownCountryPanel.activeInHierarchy)
            {
                Profile._instance.dropDownCountryPanel.transform.GetChild(3).GetComponent<Button>().interactable = true;
            }
            else if (Registration.instance.dropDownCountryPanel.activeInHierarchy)
            {
                Registration.instance.dropDownCountryPanel.transform.GetChild(3).GetComponent<Button>().interactable = true;
            }
            else if (TablePreferenceScript.instance.myAgeParentObj.activeInHierarchy)
            {
                TablePreferenceScript.instance.myAgeParentObj.transform.GetChild(2).GetChild(0).GetComponent<Button>().interactable = true;
            }
            else if (TablePreferenceScript.instance.tableMatchObj.activeInHierarchy)
            {
                for (int i = 0; i < TablePreferenceScript.instance.preferenceValueParent.transform.childCount; i++)
                {
                    if (TablePreferenceScript.instance.preferenceValueParent.transform.GetChild(i).gameObject.activeInHierarchy)
                    {
                        TablePreferenceScript.instance.preferenceValueParent.transform.GetChild(i).GetChild(2).GetChild(0).GetComponent<Button>().interactable = true;
                    }
                }
            }
        }
    }
    #endregion

    #region Search With Name Only

    public List<Transform> countryListName;
    public List<Transform> languageListName;
    public List<Transform> changeLanguageListName;

    public void FindCountryName(InputField input)
    {
        StartCoroutine(SearchByNameOnlyCorotine(input, countryListName, 0));
    }

    public void FindLanguageName(InputField input)
    {
        StartCoroutine(SearchByNameOnlyCorotine(input, languageListName, 0));
    }

    public void FindChangeLanguageName(InputField input)
    {
        StartCoroutine(SearchByNameOnlyCorotine(input, changeLanguageListName, 0));
    }

    IEnumerator SearchByNameOnlyCorotine(InputField input, List<Transform> _nameList, int _nameIndex)
    {
        yield return new WaitForSeconds(0.1f);
        memberListNameAfterSearch = SearchByNameOnly(input.textComponent.text, _nameList, _nameIndex);
        NewObjList(_nameList, memberListNameAfterSearch);

    }

    public List<Transform> SearchByNameOnly(string str, List<Transform> nameList, int _nameIndex)
    {
        newList.Clear();
        for (int i = 0; i < nameList.Count; i++)
        {
            if (nameList[i].GetChild(_nameIndex).GetComponent<Text>().text.ToLower().Contains(str.ToLower()))
            {
                newList.Add(nameList[i]);
            }
        }

        return newList;
    }

    #endregion
}
