using SmartLocalization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TablePreferenceScript : MonoBehaviour
{
    public static TablePreferenceScript instance;

    [Header("GameObject Reference")]
    public GameObject preferencePanel;
    public GameObject preferenceScrollContent;

    public GameObject preferenceValuePanel;
    public GameObject preferenceValueParent;

    public GameObject myAgePanel;
    public GameObject myAgeScrollContent;
    public GameObject myAgeParentObj;
    public GameObject tableMatchObj;

    [Header("Set Table Configuration")]
    public int maxAge = 100;
    public int minAge = 17;
    public int maxChar = 15;
    public int maxSelection = 4;
    public string maxSelectionInst = "You can select maximum";
    public string defaultSelectedText = "Select";

    private GameObject panel;
    private GameObject scrollContent;

    private string tablePreferenceUrl;
    private string saveTablePreferenceUrl;

    private int totalPreferenceCount;
    private int totalPreferenceValueCount;
    private int valueCount;
    private int myAgeCount;

    public List<GameObject> preferenceList;
    public List<GameObject> preferenceValueList;

    public List<GameObject> myAgeList;

    public List<string> selectedList;

    [Serializable]
    public class PValueList
    {
        public List<GameObject> valueList;
    }

    public List<PValueList> pValueList;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        tablePreferenceUrl = ServerChanger.instance.domainURL + "api/v1/user/table-preference-setting";
        saveTablePreferenceUrl = ServerChanger.instance.domainURL + "api/v1/user/save-table-preference-setting";

        totalPreferenceCount = 1;
        totalPreferenceValueCount = 1;
        valueCount = 1;
        myAgeCount = 1;

        preferenceList = new List<GameObject>();
        preferenceValueList = new List<GameObject>();
        myAgeList = new List<GameObject>();
        selectedList = new List<string>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            for (int i = 0; i < preferenceValueParent.transform.childCount; i++)
            {
                preferenceValueParent.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    #region Table Preference Data

    [Serializable]
    public class TablePreference
    {
        public bool error;
        public PreferenceData data;

    }

    [Serializable]
    public class PreferenceData
    {
        public PreferenceOption[] preferenceOption;
        public PreferenceOptionValues[] preferenceOptionValues;
        public UserSetting userSetting;
    }

    [Serializable]
    public class PreferenceOption
    {
        public string optionKeyName;
        public string myOptionTitle;
        public string myOptionSelect;
        public string othersOptionTitle;
        public string othersOptionSelect;
        public string optionIcon;
    }

    [Serializable]
    public class PreferenceOptionValues
    {
        public string preferenceTitle;
        public string preferenceKey;
        public PreferenceValue[] preferenceValue;
    }

    [Serializable]
    public class PreferenceValue
    {
        public string value;
        public int rank;
        public string title;
    }

    [Serializable]
    public class UserSetting
    {
        public UserPersonalSetting[] userPersonalSetting;
        public OthersPreferenceSetting[] othersPreferenceSetting;

    }

    [SerializeField] TablePreference tablePreference;
    
    public void TablePreferenceRequest()
    {
        ResetMyAgeItem();
        ResetTablePreferenceValueItem();
        ResetTablePreferenceItem();
        ResetAllValuesForImage();
        ResetValueItems();

        preferencePanel.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("Select");//defaultSelectedText;
        preferencePanel.transform.GetChild(1).GetChild(2).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue("Select");//defaultSelectedText;

        ClubManagement.instance.loadingPanel.SetActive(true);
        Communication.instance.GetData(tablePreferenceUrl, TablePreferenceCallback);
    }

    void TablePreferenceCallback(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        print("TablePreferenceCallback : " + response);

        if (!string.IsNullOrEmpty(response))
        {
            tablePreference = JsonUtility.FromJson<TablePreference>(response);

            if (!tablePreference.error)
            {
                InstantiateMyAgeItems();
                
                if (tablePreference.data.preferenceOption.Length != totalPreferenceCount)
                {
                    for (int i = totalPreferenceCount; i < tablePreference.data.preferenceOption.Length; i++)
                    {
                        totalPreferenceCount++;
                        GenerateTablePreferenceItem();
                    }
                }

                if (tablePreference.data.preferenceOptionValues.Length != totalPreferenceValueCount)
                {
                    for (int i = totalPreferenceValueCount; i < tablePreference.data.preferenceOptionValues.Length; i++)
                    {
                        totalPreferenceValueCount++;
                        GenerateTablePreferenceValueItem();
                    }
                }

                for (int i = 0; i < tablePreference.data.preferenceOption.Length; i++)
                {
                    preferenceScrollContent.transform.GetChild(i).GetChild(3).GetComponent<Text>().text = tablePreference.data.preferenceOption[i].optionKeyName;

                    preferenceScrollContent.transform.GetChild(i).GetChild(0).GetChild(1).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue(tablePreference.data.preferenceOption[i].myOptionTitle);
                    preferenceScrollContent.transform.GetChild(i).GetChild(1).GetChild(1).GetComponent<Text>().text = LanguageManager.Instance.GetTextValue(tablePreference.data.preferenceOption[i].othersOptionTitle);

                    preferenceScrollContent.transform.GetChild(i).GetChild(0).GetChild(3).GetComponent<Text>().text = tablePreference.data.preferenceOption[i].myOptionSelect;
                    preferenceScrollContent.transform.GetChild(i).GetChild(1).GetChild(3).GetComponent<Text>().text = tablePreference.data.preferenceOption[i].othersOptionSelect;

                    for (int j = 0; j < tablePreference.data.userSetting.userPersonalSetting.Length; j++)
                    {
                        if (!string.IsNullOrEmpty(tablePreference.data.userSetting.userPersonalSetting[j].key))
                        {
                            if (tablePreference.data.preferenceOption[i].optionKeyName == tablePreference.data.userSetting.userPersonalSetting[j].key)
                            {
                                for (int k = 0; k < tablePreference.data.userSetting.userPersonalSetting[j].title.Count; k++)
                                {
                                    if (k == 0)
                                    {
                                        preferenceScrollContent.transform.GetChild(i).GetChild(0).GetChild(2).GetComponent<Text>().text = string.Empty;
                                        preferenceScrollContent.transform.GetChild(i).GetChild(0).GetChild(2).GetComponent<Text>().text += tablePreference.data.userSetting.userPersonalSetting[j].title[k];
                                    }
                                    else
                                    {
                                        preferenceScrollContent.transform.GetChild(i).GetChild(0).GetChild(2).GetComponent<Text>().text += ", " + tablePreference.data.userSetting.userPersonalSetting[j].title[k];
                                    }
                                    if (preferenceScrollContent.transform.GetChild(i).GetChild(0).GetChild(2).GetComponent<Text>().text.Length > maxChar)
                                    {
                                        preferenceScrollContent.transform.GetChild(i).GetChild(0).GetChild(2).GetComponent<Text>().text = preferenceScrollContent.transform.GetChild(i).GetChild(0).GetChild(2).GetComponent<Text>().text.Remove(maxChar) + " ...";
                                    }
                                }

                            }
                        }
                        
                    }

                    for (int j = 0; j < tablePreference.data.userSetting.othersPreferenceSetting.Length; j++)
                    {
                        if (!string.IsNullOrEmpty(tablePreference.data.userSetting.othersPreferenceSetting[j].key))
                        {
                            if (tablePreference.data.preferenceOption[i].optionKeyName == tablePreference.data.userSetting.othersPreferenceSetting[j].key)
                            {
                                for (int k = 0; k < tablePreference.data.userSetting.othersPreferenceSetting[j].title.Count; k++)
                                {
                                    if (k == 0)
                                    {
                                        preferenceScrollContent.transform.GetChild(i).GetChild(1).GetChild(2).GetComponent<Text>().text = string.Empty;
                                        preferenceScrollContent.transform.GetChild(i).GetChild(1).GetChild(2).GetComponent<Text>().text += tablePreference.data.userSetting.othersPreferenceSetting[j].title[k];
                                    }
                                    else
                                    {
                                        preferenceScrollContent.transform.GetChild(i).GetChild(1).GetChild(2).GetComponent<Text>().text += ", " + tablePreference.data.userSetting.othersPreferenceSetting[j].title[k];
                                    }
                                    if (preferenceScrollContent.transform.GetChild(i).GetChild(1).GetChild(2).GetComponent<Text>().text.Length > maxChar)
                                    {
                                        preferenceScrollContent.transform.GetChild(i).GetChild(1).GetChild(2).GetComponent<Text>().text = preferenceScrollContent.transform.GetChild(i).GetChild(1).GetChild(2).GetComponent<Text>().text.Remove(maxChar) + " ...";
                                    }
                                }

                            }
                        }
                    }

                }

                pValueList = new List<PValueList>();

                for (int i = 0; i < tablePreference.data.preferenceOptionValues.Length; i++)
                {
                    
                    preferenceValueParent.transform.GetChild(i).GetChild(0).GetChild(2).GetComponent<Text>().text = tablePreference.data.preferenceOptionValues[i].preferenceKey;
                    
                    panel = preferenceValueParent.transform.GetChild(i).GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetChild(0).gameObject;
                    scrollContent = preferenceValueParent.transform.GetChild(i).GetChild(1).GetChild(1).GetChild(0).GetChild(0).gameObject;

                    pValueList.Add(new PValueList());

                    if (tablePreference.data.preferenceOptionValues[i].preferenceValue.Length != valueCount)
                    {
                        pValueList[i].valueList = new List<GameObject>();

                        for (int j = valueCount; j < tablePreference.data.preferenceOptionValues[i].preferenceValue.Length; j++)
                        {
                            valueCount++;

                            scrollItemObj = Instantiate(panel);
                            scrollItemObj.transform.SetParent(scrollContent.transform, false);
                            
                            pValueList[i].valueList.Add(scrollItemObj);
                        }
                        
                        valueCount = 1;
                    }

                    for (int j = 0; j < tablePreference.data.preferenceOptionValues[i].preferenceValue.Length; j++)
                    {
                        scrollContent.transform.GetChild(j).GetChild(0).GetComponent<Text>().text = tablePreference.data.preferenceOptionValues[i].preferenceValue[j].title;
                        scrollContent.transform.GetChild(j).GetChild(0).GetChild(0).GetComponent<Text>().text = tablePreference.data.preferenceOptionValues[i].preferenceValue[j].value;
                    }
                }

                iconImage.Clear();

                for (int i = 0; i < tablePreference.data.preferenceOption.Length; i++)
                {
                    iconImage.Add(tablePreference.data.preferenceOption[i].optionIcon);
                }
                UpdateImage();
            }
        }
    }

    private GameObject scrollItemObj;
    
    void GenerateTablePreferenceItem()
    {
        scrollItemObj = Instantiate(preferencePanel);
        scrollItemObj.transform.SetParent(preferenceScrollContent.transform, false);
        preferenceList.Add(scrollItemObj);
    }

    void ResetTablePreferenceItem()
    {
        if (preferenceList.Count > 0)
        {
            for (int i = 0; i < preferenceList.Count; i++)
            {
                Destroy(preferenceList[i]);
            }
            preferenceList.Clear();
        }
        totalPreferenceCount = 1;

        
    }

    void GenerateTablePreferenceValueItem()
    {
        scrollItemObj = Instantiate(preferenceValuePanel);
        scrollItemObj.transform.SetParent(preferenceValueParent.transform, false);
        preferenceValueList.Add(scrollItemObj);
    }

    void ResetTablePreferenceValueItem()
    {
        if (preferenceValueList.Count > 0)
        {
            for (int i = 0; i < preferenceValueList.Count; i++)
            {
                Destroy(preferenceValueList[i]);
            }
            preferenceValueList.Clear();
        }
        totalPreferenceValueCount = 1;
    }

    void ResetValueItems()
    {
        if (pValueList.Count > 0)
        {
            for (int i = 0; i < pValueList.Count; i++)
            {
                for (int j = 0; j < pValueList[i].valueList.Count; j++)
                {
                    Destroy(pValueList[i].valueList[j]);
                }
            }
            
            pValueList.Clear();
        }
        valueCount = 1;
    }

    public void InstantiateMyAgeItems()
    {
        if (maxAge - minAge + 1 != myAgeCount)
        {
            for (int i = myAgeCount; i < maxAge - minAge + 1; i++)
            {
                myAgeCount++;
                GenerateMyAgeItem();
            }
        }

        for (int i = 0; i < myAgeScrollContent.transform.childCount; i++)
        {
            myAgeScrollContent.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "" + (i + minAge).ToString();
        }
    }

    void GenerateMyAgeItem()
    {
        scrollItemObj = Instantiate(myAgePanel);
        scrollItemObj.transform.SetParent(myAgeScrollContent.transform, false);
        myAgeList.Add(scrollItemObj);
    }

    void ResetMyAgeItem()
    {
        if (myAgeList.Count > 0)
        {
            for (int i = 0; i < myAgeList.Count; i++)
            {
                Destroy(myAgeList[i]);
            }
            myAgeList.Clear();
        }
        myAgeCount = 1;
    }

    Transform selectedPanel;
    public void ClickPreferenceBtn(Transform panel)
    {
        for (int i = 0; i < preferenceValueParent.transform.childCount; i++)
        {
            if(panel.parent.GetChild(3).GetComponent<Text>().text == preferenceValueParent.transform.GetChild(i).GetChild(0).GetChild(2).GetComponent<Text>().text)
            {
                selectedPanel = panel;
                maxSelectCount = 0;

                SearchingScript.instance.listName.Clear();
                selectedList.Clear();

                if (panel.GetChild(1).GetComponent<Text>().text == "My Age")
                {
                    preferenceValueParent.SetActive(false);
                    myAgeParentObj.SetActive(true);

                    for (int j = 0; j < myAgeScrollContent.transform.childCount; j++)
                    {
                        SearchingScript.instance.listName.Add(myAgeScrollContent.transform.GetChild(j));
                    }

                    for (int j = 0; j < tablePreference.data.userSetting.userPersonalSetting.Length; j++)
                    {
                        if (!string.IsNullOrEmpty(tablePreference.data.userSetting.userPersonalSetting[j].key))
                        {
                            if (tablePreference.data.userSetting.userPersonalSetting[j].key == panel.parent.GetChild(3).GetComponent<Text>().text)
                            {
                                for (int k = 0; k < tablePreference.data.userSetting.userPersonalSetting[j].value.Count; k++)
                                {
                                    selectedList.Add(tablePreference.data.userSetting.userPersonalSetting[j].value[k]);
                                }
                            }
                        }
                    }
                    for (int j = 0; j < selectedList.Count; j++)
                    {
                        for (int k = 0; k < myAgeScrollContent.transform.childCount; k++)
                        {
                            if (selectedList[j] == myAgeScrollContent.transform.GetChild(k).GetChild(0).GetComponent<Text>().text)
                            {
                                myAgeScrollContent.transform.GetChild(k).GetChild(2).GetChild(0).gameObject.SetActive(true);
                            }
                        }
                       
                    }


                }
                else
                {
                    myAgeParentObj.SetActive(false);
                    preferenceValueParent.SetActive(true);
    
                    for (int j = 0; j < preferenceValueParent.transform.GetChild(i).GetChild(1).GetChild(1).GetChild(0).GetChild(0).childCount; j++)
                    {
                        SearchingScript.instance.listName.Add(preferenceValueParent.transform.GetChild(i).GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetChild(j));
                    }
                    
                    preferenceValueParent.transform.GetChild(i).GetChild(0).GetChild(1).GetComponent<Text>().text = panel.GetChild(1).GetComponent<Text>().text;

                    for (int j = 0; j < preferenceValueParent.transform.GetChild(i).GetChild(1).GetChild(1).GetChild(0).GetChild(0).childCount; j++)
                    {
                        if (panel.GetChild(3).GetComponent<Text>().text == "Single")
                        {
                            preferenceValueParent.transform.GetChild(i).GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetChild(j).GetChild(2).gameObject.SetActive(true);
                            preferenceValueParent.transform.GetChild(i).GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetChild(j).GetChild(1).gameObject.SetActive(false);

                            preferenceValueParent.transform.GetChild(i).GetChild(0).GetChild(3).GetComponent<Text>().text = "Single";

                        }
                        else if (panel.GetChild(3).GetComponent<Text>().text == "Multiple")
                        {
                            preferenceValueParent.transform.GetChild(i).GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetChild(j).GetChild(1).gameObject.SetActive(true);
                            preferenceValueParent.transform.GetChild(i).GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetChild(j).GetChild(2).gameObject.SetActive(false);

                            preferenceValueParent.transform.GetChild(i).GetChild(0).GetChild(3).GetComponent<Text>().text = "Multiple";
                        }

                        preferenceValueParent.transform.GetChild(i).GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetChild(j).gameObject.SetActive(true);
                    }

                    preferenceValueParent.transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<InputField>().text = string.Empty;

                    for (int j = 0; j < preferenceValueParent.transform.GetChild(i).GetChild(1).GetChild(1).GetChild(0).GetChild(0).childCount; j++)
                    {
                        preferenceValueParent.transform.GetChild(i).GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetChild(j).GetChild(1).GetChild(0).gameObject.SetActive(false);
                        preferenceValueParent.transform.GetChild(i).GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetChild(j).GetChild(2).GetChild(0).gameObject.SetActive(false);
                    }

                    // default selected button ui
                    
                    if (panel.CompareTag("Self"))
                    {
                        for (int j = 0; j < tablePreference.data.userSetting.userPersonalSetting.Length; j++)
                        {
                            if (!string.IsNullOrEmpty(tablePreference.data.userSetting.userPersonalSetting[j].key))
                            {
                                if(tablePreference.data.userSetting.userPersonalSetting[j].key== panel.parent.GetChild(3).GetComponent<Text>().text)
                                {
                                    for (int k = 0; k < tablePreference.data.userSetting.userPersonalSetting[j].title.Count; k++)
                                    {
                                        selectedList.Add(tablePreference.data.userSetting.userPersonalSetting[j].title[k]);
                                    }
                                }
                            }
                        }
                    }
                    else if (panel.CompareTag("Other"))
                    {
                        for (int j = 0; j < tablePreference.data.userSetting.othersPreferenceSetting.Length; j++)
                        {
                            if (!string.IsNullOrEmpty(tablePreference.data.userSetting.othersPreferenceSetting[j].key))
                            {
                                if (tablePreference.data.userSetting.othersPreferenceSetting[j].key == panel.parent.GetChild(3).GetComponent<Text>().text)
                                {
                                    for (int k = 0; k < tablePreference.data.userSetting.othersPreferenceSetting[j].title.Count; k++)
                                    {
                                        selectedList.Add(tablePreference.data.userSetting.othersPreferenceSetting[j].title[k]);
                                    }
                                }
                            }
                        }
                    }
                    maxSelectCount = selectedList.Count;
                    for (int j = 0; j < selectedList.Count; j++)
                    {
                        for (int k = 0; k < preferenceValueParent.transform.GetChild(i).GetChild(1).GetChild(1).GetChild(0).GetChild(0).childCount; k++)
                        {
                            if (selectedList[j] == preferenceValueParent.transform.GetChild(i).GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetChild(k).GetChild(0).GetComponent<Text>().text)
                            {
                                if (panel.GetChild(3).GetComponent<Text>().text == "Single")
                                {
                                    preferenceValueParent.transform.GetChild(i).GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetChild(k).GetChild(2).GetChild(0).gameObject.SetActive(true);
                                    
                                }
                                else if (panel.GetChild(3).GetComponent<Text>().text == "Multiple")
                                {
                                    preferenceValueParent.transform.GetChild(i).GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetChild(k).GetChild(1).GetChild(0).gameObject.SetActive(true);
                                    
                                }
                                    
                            }
                        }
                    }

                    preferenceValueParent.transform.GetChild(i).gameObject.SetActive(true);
                }

            }

        }
    }

    public void ClickRadioBtn(Transform radioPanel)
    {
        for (int i = 0; i < radioPanel.parent.parent.childCount; i++)
        {
            radioPanel.parent.parent.GetChild(i).GetChild(2).GetChild(0).gameObject.SetActive(false);
        }
        if (radioPanel.GetChild(0).gameObject.activeInHierarchy)
        {
            radioPanel.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            radioPanel.GetChild(0).gameObject.SetActive(true);
            if (!tableMatchObj.activeInHierarchy)
            {
                Registration.instance.selectCountryText = radioPanel.parent.GetChild(1).GetComponent<Text>().text;
            }
        }
    }

    public int maxSelectCount;
    public void ClickMultipleCheckBtn(Transform panel)
    {
        
        if (maxSelectCount == maxSelection)
        {
            if (panel.GetChild(0).gameObject.activeInHierarchy)
            {
                panel.GetChild(0).gameObject.SetActive(false);
                maxSelectCount--;
            }
            else
            {
                Cashier.instance.toastMsgPanel.SetActive(true);
                Cashier.instance.toastMsg.text = LanguageManager.Instance.GetTextValue("You can select maximum") + " " + maxSelectCount;
                
                return;
            }
        }

        else if (maxSelectCount < maxSelection)
        {
            if (panel.GetChild(0).gameObject.activeInHierarchy)
            {
                panel.GetChild(0).gameObject.SetActive(false);
                maxSelectCount--;
            }
            else
            {
                panel.GetChild(0).gameObject.SetActive(true);
                maxSelectCount++;
            }
        }

        //print(maxSelectCount);

    }

    #region update Preference Image

    public List<string> iconImage;

    [SerializeField]
    private List<IconImageInSequence> iconImageInSequence;

    private int k = 0;
    private int totalImageCount;
    private int count = 0;

    private int previousCountForMemberList;
    void UpdateImage()
    {
        if (iconImage.Count == previousCountForMemberList)
        {
            return;
        }

        k = 0;
        previousCountForMemberList = 0;
        totalImageCount = 0;
        iconImageInSequence.Clear();

        iconImageInSequence = new List<IconImageInSequence>();

        for (int i = 0; i < iconImage.Count; i++)
        {
            if (!string.IsNullOrEmpty(iconImage[i]))
            {
                iconImageInSequence.Add(new IconImageInSequence());
                ClubManagement.instance.loadingPanel.SetActive(true);
                iconImageInSequence[k].imgUrl = iconImage[i];
                iconImageInSequence[k].ImageProcess(iconImage[i]);

                k = k + 1;

            }
            previousCountForMemberList++;
        }
    }

    public void ApplyImage()
    {
        if (k == totalImageCount)
        {
            count = 0;
            ClubManagement.instance.loadingPanel.SetActive(false);

            for (int i = 0; i < iconImage.Count; i++)
            {
                if (!string.IsNullOrEmpty(iconImage[i]))
                {
                    preferenceScrollContent.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().sprite = iconImageInSequence[count].imgPic;
                    preferenceScrollContent.transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<Image>().sprite = iconImageInSequence[count].imgPic;
                    
                    count++;
                }
            }
        }
    }

    [Serializable]
    public class IconImageInSequence
    {
        public string imgUrl;
        public Sprite imgPic;

        public void ImageProcess(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                Communication.instance.GetImage(url, ImageResponse);
            }
        }

        void ImageResponse(Sprite response)
        {
            if (response != null)
            {
                imgPic = response;

                instance.totalImageCount++;
                instance.ApplyImage();

            }
        }
    }

    public void ResetAllValuesForImage()
    {
        totalImageCount = 0;
        k = 0;
        count = 0;
        previousCountForMemberList = 0;
        iconImage.Clear();
        iconImageInSequence.Clear();

    }

    #endregion

    #endregion

    #region Save Table Preference Data
    
    [Serializable]
    public class SaveOthersPreferenceSetting
    {
        public OthersPreferenceSetting othersPreferenceSetting;
    }

    [Serializable]
    public class SaveUserPersonalSetting
    {
        public UserPersonalSetting userPersonalSetting;
    }

    [Serializable]
    public class OthersPreferenceSetting
    {
        public string key;
        public List<string> value;
        public List<string> title;
    }

    [Serializable]
    public class UserPersonalSetting
    {
        public string key;
        public List<string> value;
        public List<string> title;
    }

    [SerializeField] SaveOthersPreferenceSetting saveOthersPreferenceSetting;
    [SerializeField] SaveUserPersonalSetting saveUserPersonalSetting;

    [Serializable]
    public class SaveTablePreferenceResponse
    {
        public bool error;
    }

    [SerializeField] SaveTablePreferenceResponse saveTablePreferenceResponse;

    public void SaveUserTablePreferenceRequest(GameObject panel)
    {
        saveUserPersonalSetting.userPersonalSetting.key = panel.transform.GetChild(0).GetChild(2).GetComponent<Text>().text;

        saveUserPersonalSetting.userPersonalSetting.value.Clear();
        saveUserPersonalSetting.userPersonalSetting.value = new List<string>();
        saveUserPersonalSetting.userPersonalSetting.value.AddRange(SearchSelectedObj(panel.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(0), panel.transform.GetChild(0).GetChild(3).GetComponent<Text>().text, panel.transform.GetChild(0).GetChild(2).GetComponent<Text>().text));

        saveUserPersonalSetting.userPersonalSetting.title.Clear();
        saveUserPersonalSetting.userPersonalSetting.title = new List<string>();
        saveUserPersonalSetting.userPersonalSetting.title.AddRange(SearchSelectedObjByTitle(panel.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(0), panel.transform.GetChild(0).GetChild(3).GetComponent<Text>().text));

        string body = JsonUtility.ToJson(saveUserPersonalSetting);
        print("Save User Table Preference Request......" + body);

        ClubManagement.instance.loadingPanel.SetActive(true);
        Communication.instance.PostData(saveTablePreferenceUrl, body, SaveTablePreferenceCallback);

    }

    public void SaveOtherTablePreferenceRequest(GameObject panel)
    {
        saveOthersPreferenceSetting.othersPreferenceSetting.key = panel.transform.GetChild(0).GetChild(2).GetComponent<Text>().text;

        saveOthersPreferenceSetting.othersPreferenceSetting.value.Clear();
        saveOthersPreferenceSetting.othersPreferenceSetting.value = new List<string>();
        saveOthersPreferenceSetting.othersPreferenceSetting.value.AddRange(SearchSelectedObj(panel.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(0), panel.transform.GetChild(0).GetChild(3).GetComponent<Text>().text, panel.transform.GetChild(0).GetChild(2).GetComponent<Text>().text));

        saveOthersPreferenceSetting.othersPreferenceSetting.title.Clear();
        saveOthersPreferenceSetting.othersPreferenceSetting.title = new List<string>();
        saveOthersPreferenceSetting.othersPreferenceSetting.title.AddRange(SearchSelectedObjByTitle(panel.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(0), panel.transform.GetChild(0).GetChild(3).GetComponent<Text>().text));

        string body = JsonUtility.ToJson(saveOthersPreferenceSetting);
        print("Save Other Table Preference Request......" + body);

        ClubManagement.instance.loadingPanel.SetActive(true);
        Communication.instance.PostData(saveTablePreferenceUrl, body, SaveTablePreferenceCallback);
    }

    
    void SaveTablePreferenceCallback(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);

        if (!string.IsNullOrEmpty(response))
        {
            saveTablePreferenceResponse = JsonUtility.FromJson<SaveTablePreferenceResponse>(response);

            if (!saveTablePreferenceResponse.error)
            {
                print("success ......" + response);
                TablePreferenceRequest();
                selectedObj.SetActive(false);

                //for (int i = 0; i < stringList.Count; i++)
                //{
                //    if (i == 0)
                //    {
                //        selectedPanel.GetChild(2).GetComponent<Text>().text = string.Empty;
                //        selectedPanel.GetChild(2).GetComponent<Text>().text += stringList[i];
                //    }
                //    else
                //    {
                //        selectedPanel.GetChild(2).GetComponent<Text>().text += ", " + stringList[i];
                //    }
                //}

                //if (selectedPanel.GetChild(2).GetComponent<Text>().text.Length > maxChar)
                //{
                //    selectedPanel.GetChild(2).GetComponent<Text>().text = selectedPanel.GetChild(2).GetComponent<Text>().text.Remove(maxChar) + "...";
                //}
            }
        }
    }

    public List<string> stringList;
    
    public List<string> SearchSelectedObj(Transform scrollObj, string type, string key)
    {
        if (type == "Single")                      // For Single Selection UI
        {
            stringList.Clear();
            stringList = new List<string>();
            for (int i = 0; i < scrollObj.childCount; i++)
            {
                if (scrollObj.GetChild(i).GetChild(2).GetChild(0).gameObject.activeInHierarchy)
                {
                    stringList.Add(scrollObj.GetChild(i).GetChild(0).GetComponent<Text>().text);
                }
            }
        }
        else if (type == "Multiple")              // For Multiple Selection UI
        {
            stringList.Clear();
            stringList = new List<string>();
            for (int i = 0; i < scrollObj.childCount; i++)
            {
                if (scrollObj.GetChild(i).GetChild(1).GetChild(0).gameObject.activeInHierarchy)
                {
                    stringList.Add(scrollObj.GetChild(i).GetChild(0).GetComponent<Text>().text);
                }
            }

            if (key == "age")
            {
                stringList.Clear();
                stringList = new List<string>();
                for (int i = 0; i < scrollObj.childCount; i++)
                {
                    if (scrollObj.GetChild(i).GetChild(1).GetChild(0).gameObject.activeInHierarchy)
                    {
                        string str = scrollObj.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text;

                        string str2 = str.Substring(str.LastIndexOf(',') + 1);
                        string str1 = str.Substring(0, str.IndexOf(","));
                        print(str1);
                        print(str2);
                        print(str);

                        int n1 = int.Parse(str1);
                        int n2 = int.Parse(str2);

                        print(n1);
                        print(n2);

                        for (int j = n1; j <= n2; j++)
                        {
                            stringList.Add(j.ToString());
                        }

                    }
                }
            }
        }

        return stringList;
    }

    public List<string> SearchSelectedObjByTitle(Transform scrollObj, string type)
    {
        if (type == "Single")                      // For Single Selection UI
        {
            stringList.Clear();
            stringList = new List<string>();
            for (int i = 0; i < scrollObj.childCount; i++)
            {
                if (scrollObj.GetChild(i).GetChild(2).GetChild(0).gameObject.activeInHierarchy)
                {
                    stringList.Add(scrollObj.GetChild(i).GetChild(0).GetComponent<Text>().text);
                }
            }
        }
        else if (type == "Multiple")              // For Multiple Selection UI
        {
            stringList.Clear();
            stringList = new List<string>();
            for (int i = 0; i < scrollObj.childCount; i++)
            {
                if (scrollObj.GetChild(i).GetChild(1).GetChild(0).gameObject.activeInHierarchy)
                {
                    stringList.Add(scrollObj.GetChild(i).GetChild(0).GetComponent<Text>().text);
                }
            }

        }

        return stringList;
    }

    GameObject selectedObj;

    public void DoneBtnInTablePreference(GameObject panel)
    {
        if (selectedPanel.CompareTag("Self"))
        {
            SaveUserTablePreferenceRequest(panel);
        }
        else if (selectedPanel.CompareTag("Other"))
        {
            SaveOtherTablePreferenceRequest(panel);
        }
        selectedObj = panel;
    }


    #endregion
}
