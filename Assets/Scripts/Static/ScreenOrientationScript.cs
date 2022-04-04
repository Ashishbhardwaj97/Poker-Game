using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenOrientationScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Menupage()
    {

        SceneManager.LoadScene("main");
        //Screen.orientation = ScreenOrientation.Portrait;

        PlayerPrefs.SetInt("SceneChange", 1);
    }
}
