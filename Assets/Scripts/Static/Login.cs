using SmartLocalization;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public static Login instance;

    public string generateCodeUrl,verifyOtpUrl,resetPasswordUrl;
    public GameObject loginPanel, login_FB_Google_Panel;
    public GameObject forgotPasswordPanel;
    //public TMP_InputField forgotPasswordInputField;
    public InputField forgotPasswordInputField;

    //public InputField otp1InputField;
    //public InputField otp2InputField;
    //public InputField otp3InputField;
    //public InputField otp4InputField;
    //public TMP_InputField setNewPassword1InputField;
    //public TMP_InputField setNewPassword2InputField;

    public InputField setNewPassword1InputField;
    public InputField setNewPassword2InputField;

    public TMP_InputField otpInputField;

    public GameObject verificationPanel;
    public GameObject setNewPasswordPanel;
    public GameObject loginCanvas;
    public GameObject registrationCanvas;

    public GameObject eyeBall_On, eyeBall_Off;

    public Text otpError;

    [SerializeField]
    public ForgotPassword forgotPassword;

    [SerializeField]
    public ResetPassword resetPassword;

    [SerializeField]
    public VerifyOTP verifyOTP;

    [SerializeField]
    public VerifyOTPResponse verifyOTPResponse;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        generateCodeUrl = ServerChanger.instance.domainURL + "api/v1/user/generate-code";
        verifyOtpUrl = ServerChanger.instance.domainURL + "api/v1/user/verify-otp";
        resetPasswordUrl = ServerChanger.instance.domainURL + "api/v1/user/reset-password";
    }

    public GameObject rememberMeTick;
    public bool isRememberMe;
    public void ToggleTick()
    {
        if (rememberMeTick.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            rememberMeTick.transform.GetChild(0).gameObject.SetActive(false);
            isRememberMe = false;
        }
        else
        {
            rememberMeTick.transform.GetChild(0).gameObject.SetActive(true);
            isRememberMe = true;
        }
    }

    public void ForgotPassword()
    {
        loginPanel.SetActive(false);
        forgotPasswordPanel.SetActive(true);
        ApiHitScript.instance.incorrectResponse.SetActive(false);
        forgotPasswordInputField.text = string.Empty;
    }

    public void Back()
    {
        loginPanel.SetActive(true);

        ApiHitScript.instance.userNameInputField.text = "";
        ApiHitScript.instance.passwordInputField .text = "";

        forgotPasswordPanel.SetActive(false);
    }

    public void GetOTP()
    {
        verificationPanel.SetActive(true);
        forgotPasswordPanel.SetActive(false);
    }

    public void VerficationBack()
    {
        verificationPanel.SetActive(false);
        forgotPasswordPanel.SetActive(true);
    }

    public string otp;
    public void Verify()
    {
        if (string.IsNullOrEmpty(otpInputField.text) || otpInputField.text.Length < 4)
        {
            otpError.text = LanguageManager.Instance.GetTextValue("otp blank");
        }

        else
        {
            otp = otpInputField.text;
            
            verifyOTP.code = otp;

            string body = JsonUtility.ToJson(verifyOTP);
            ClubManagement.instance.loadingPanel.SetActive(true);
            Communication.instance.PostData(verifyOtpUrl, body, VerifyProcess);

            
        }
    }
    public Text verifyOTPResponseMessage;
    void VerifyProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("error");
        }
        else
        {
            print("response..." + response);
            verifyOTPResponse = JsonUtility.FromJson<VerifyOTPResponse>(response);

            if(verifyOTPResponse.error == "false")
            {
                setNewPasswordPanel.SetActive(true);
                verificationPanel.SetActive(false);
                verifyOTPResponseMessage.text = verifyOTPResponse.message;
                verifyOTPResponseMessage.color = new Color32(50, 200, 50, 255);
            }
            else
            {
                verifyOTPResponseMessage.text = LanguageManager.Instance.GetTextValue("otp invalid");
                verifyOTPResponseMessage.color = new Color32(255, 122, 89, 255);
            }
            
        }
    }

    public void SetNewPasswordBack()
    {
        setNewPasswordPanel.SetActive(false);
        verificationPanel.SetActive(true);
    }
    //....................//
    public void MovetoLogin()
    {
        login_FB_Google_Panel.SetActive(false);
        loginPanel.SetActive(true);
    }

    public void BacktoLogin_FB_Google()
    {
        login_FB_Google_Panel.SetActive(true);
        loginPanel.SetActive(false);
    }

    //.......................//
    public void SignUp()
    {
        
        loginCanvas.SetActive(false);
        registrationCanvas.SetActive(true);
        ApiHitScript.instance.ClearLoginInputFields();
        Registration.instance.cityInputField.text = Registration.instance.userCity;
        Registration.instance.countryCodeText.text = Registration.instance.userContryCode;

        //StartCoroutine(Registration.instance.DetectCountry());
    }

    public void ForgotPasswordValues()
    {

        if (string.IsNullOrEmpty(forgotPasswordInputField.text) || !forgotPasswordInputField.GetComponent<ValidateInputField>().isValidInput)
        {
            forgotPasswordInputField.GetComponent<ValidateInputField>().Validate(forgotPasswordInputField.text);
            forgotPasswordInputField.Select();
        }
        else
        {
            forgotPassword.email = forgotPasswordInputField.textComponent.text;
            resetPassword.email = forgotPassword.email;
            verifyOTP.email = forgotPassword.email;

            string body = JsonUtility.ToJson(forgotPassword);
            print("body......" + body);
            
            ClubManagement.instance.loadingPanel.SetActive(true);
            Communication.instance.PostData(generateCodeUrl, body, ForgotPasswordProcess);
            otpInputField.text = string.Empty;
        }
    }

    [Serializable]
    public class ForgotPasswordResponse
    {
        public bool error;
        public Errors errors;
    }
    [Serializable]
    public class Errors
    {
        public Username username;
    }

    [Serializable]
    public class Username
    {
        public Properties properties;
    }

    [Serializable]
    public class Properties
    {
        public string message;
    }

    [SerializeField] Properties properties;
    [SerializeField] Username username;
    [SerializeField] Errors errors;
    [SerializeField] ForgotPasswordResponse forgotPasswordResponse;

    public Text wrongEmailResponse;
    void ForgotPasswordProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("Network error..!!");
            Cashier.instance.toastMsg.text = LanguageManager.Instance.GetTextValue("internet check");//SocialProfile._instance.networkErrorMsg;
            Cashier.instance.toastMsgPanel.SetActive(true);
        }
        else
        {
            print("response..." + response);

            forgotPasswordResponse = JsonUtility.FromJson<ForgotPasswordResponse>(response);

            if (!forgotPasswordResponse.error)
            {
                GetOTP();
                wrongEmailResponse.text = string.Empty;
            }
            else
            {
                wrongEmailResponse.text = LanguageManager.Instance.GetTextValue("email already exist");
            }
        }
    }

    public bool CheckMinCharcterInPassword()
    {
        if (Convert.ToInt32(setNewPassword1InputField.text.Length) < Registration.instance.minLengthInPassword)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    public void SetNewPasswordValues()
    {
        if (string.IsNullOrEmpty(setNewPassword1InputField.text) || !setNewPassword1InputField.GetComponent<ValidateInputField>().isValidInput)
        {
            setNewPassword1InputField.GetComponent<ValidateInputField>().Validate(setNewPassword1InputField.text);
            setNewPassword1InputField.Select();
        }
        else if (string.IsNullOrEmpty(setNewPassword2InputField.text) || !setNewPassword2InputField.GetComponent<ValidateInputField>().isValidInput)
        {
            setNewPassword2InputField.GetComponent<ValidateInputField>().Validate(setNewPassword2InputField.text);
            setNewPassword2InputField.Select();
        }
        else
        {
            
            resetPassword.new_password = setNewPassword1InputField.textComponent.text;
            resetPassword.confirm_password = setNewPassword2InputField.textComponent.text;

            string body = JsonUtility.ToJson(resetPassword);
            ClubManagement.instance.loadingPanel.SetActive(true);
            Communication.instance.PostData(resetPasswordUrl, body, SetNewPasswordValuesProcess);
            print(body);
        }
        
    }

    void SetNewPasswordValuesProcess(string response)
    {
        ClubManagement.instance.loadingPanel.SetActive(false);
        if (string.IsNullOrEmpty(response))
        {
            print("error");
        }
        else
        {
            print("response..." + response);
            setNewPasswordPanel.SetActive(false);
            loginPanel.SetActive(true);
        }
    }

    public bool MatchPassword()
    {
        if (setNewPassword1InputField.textComponent.text == setNewPassword2InputField.textComponent.text)
        {
            return true;
        }
        return false;
    }
}
[Serializable]
public class ForgotPassword
{
    public string email;
}

[Serializable]
public class ResetPassword
{
    public string new_password;
    public string confirm_password;
    public string email;
}

[Serializable]
public class VerifyOTP
{
    public string email;
    public string code;
}

[Serializable]
public class VerifyOTPResponse
{
    public string error;
    public string message;
}