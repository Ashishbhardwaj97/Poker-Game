using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SmartLocalization;

public class ValidateInputField : MonoBehaviour
{
    [HideInInspector] public bool isValidInput = false;

    [SerializeField] private string errorMessage;
    [SerializeField] private string blankErrorMessage;

    [SerializeField] private string errorMessageKey;
    [SerializeField] private string blankErrorMessageKey;

    private InputField inputToValidate;
    [SerializeField] private bool isEmail, isPasswordMatch, isResetPasswordMatch, isChangePasswordMatch, isClubIDMatch, isDiamondMatch;
    [SerializeField] private bool isMinCharInSignUp, isMinCharInResetPassword, isMinCharInChangePassword;

    [SerializeField] public Text validationInfo;

    private void OnEnable()
    {
        validationInfo.text = string.Empty;
    }

    void Start()
    {
        inputToValidate = GetComponent<InputField>();
        inputToValidate.onValueChanged.AddListener(OnValueChange);
        inputToValidate.onEndEdit.AddListener(Validate);
    }

    private void EndEdit(string value)
    {
        inputToValidate.text = value;
        Validate(value);
    }

    string oldEditText = "";
    string editText = "";

    private void OnValueChange(string arg0)
    {
        validationInfo.text = string.Empty;
    }

    public void Validate(string arg0)
    {
        print("Validate " + arg0);
        if (arg0.Length <= 0)
        {
            validationInfo.text = LanguageManager.Instance.GetTextValue(blankErrorMessageKey);
            isValidInput = false;
        }
        else if (isEmail && !ValidateEmail(arg0))
        {

            validationInfo.text = LanguageManager.Instance.GetTextValue(errorMessageKey);
            isValidInput = false;
        }
        else if (isPasswordMatch && !Registration.instance.MatchPassword())
        {
            validationInfo.text = LanguageManager.Instance.GetTextValue(errorMessageKey); ;
            isValidInput = false;
        }
        else if (isResetPasswordMatch && !Login.instance.MatchPassword())
        {
            validationInfo.text = LanguageManager.Instance.GetTextValue(errorMessageKey); ;
            isValidInput = false;
        }
        else if (isChangePasswordMatch && !SocialProfile._instance.MatchPassword())
        {
            validationInfo.text = LanguageManager.Instance.GetTextValue(errorMessageKey); ;
            isValidInput = false;
        }
        else if (isClubIDMatch && !ClubManagement.instance.MatchClubID())
        {
            validationInfo.text = LanguageManager.Instance.GetTextValue(errorMessageKey); ;
            isValidInput = false;
        }
        else if (isDiamondMatch && !ClubManagement.instance.CheckDiamondVal())
        {
            validationInfo.text = LanguageManager.Instance.GetTextValue(errorMessageKey); ;
            isValidInput = false;
        }
        else if (isMinCharInSignUp && !Registration.instance.CheckMinCharcterInPassword())
        {
            validationInfo.text = LanguageManager.Instance.GetTextValue(errorMessageKey);
            isValidInput = false;
        }
        else if (isMinCharInChangePassword && !SocialProfile._instance.CheckMinCharcterInPassword())
        {
            validationInfo.text = LanguageManager.Instance.GetTextValue(errorMessageKey);
            isValidInput = false;
        }
        else if (isMinCharInResetPassword && !Login.instance.CheckMinCharcterInPassword())
        {
            validationInfo.text = LanguageManager.Instance.GetTextValue(errorMessageKey);
            isValidInput = false;
        }
        else
        {
            isValidInput = true;
        }
    }

    public static bool ValidateEmail(string emailString)
    {
        return Regex.IsMatch(emailString, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
    }

}
