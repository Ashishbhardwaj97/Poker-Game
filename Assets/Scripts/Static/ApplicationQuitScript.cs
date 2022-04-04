using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApplicationQuitScript : MonoBehaviour
{
    public GameObject loginPanel;
    public GameObject forgotPasswordPanel;
    
    public GameObject verificationPanel;
    public GameObject setNewPasswordPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (forgotPasswordPanel.activeInHierarchy)
            {
                loginPanel.SetActive(true);
                forgotPasswordPanel.SetActive(false);
            }

            else if (verificationPanel.activeInHierarchy)
            {
                forgotPasswordPanel.SetActive(true);
                verificationPanel.SetActive(false);
            }

            else if (setNewPasswordPanel.activeInHierarchy)
            {
                verificationPanel.SetActive(true);
                setNewPasswordPanel.SetActive(false);
            }

            Application.Quit();
            
        }
    }
}
