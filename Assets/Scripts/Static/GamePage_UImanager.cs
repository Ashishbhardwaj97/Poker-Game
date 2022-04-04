using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePage_UImanager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        print("startttttttttttt");
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Menupage()
    {
        
        SceneManager.LoadScene("main");
        Screen.orientation = ScreenOrientation.Portrait;

        PlayerPrefs.SetInt("SceneChange", 1);
    }

}
