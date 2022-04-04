using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    
    private Animation anim;
    public float desiredSpeed;

    void Start()
    {
        if (PokerSceneManagement.instance.isSceneRestart)
        {
            SocialGame.instance.pokerUICanvas.GetComponent<Canvas>().enabled = false;
            ApiHitScript.instance.LoadPlayerData();
        }
        else
        {
            StartCoroutine(Reset());
        }
    }

    public IEnumerator Reset()
    {
        ApiHitScript.instance.CheckAppVersion();

        anim = GetComponent<Animation>();
        anim.Play();
        if (PlayerPrefs.GetInt("SoundOffOn") == 0)
        {
            SoundManager.instance.PlayPomeSound(AudioClipCollection.instance.shuffleSFX);
        }
        yield return new WaitForSeconds(anim.clip.length);

        ClubManagement.instance.loadingPanel.SetActive(true);

        //........Wait For Check App Version Callback......//
        while (true)
        {
            if (!ApiHitScript.instance.isCheckAppVersionCallback)
            {
                yield return new WaitForSeconds(1f);
            }
            else
            {
                break;
            }
        }

        //.................................................//

        ClubManagement.instance.loadingPanel.SetActive(false);

        if (ApiHitScript.instance.isAppUpdate)
        {
            ApiHitScript.instance.updatePanel.SetActive(true);
        }
        else
        {
            ApiHitScript.instance.updatePanel.SetActive(false);
            ApiHitScript.instance.LoadPlayerData();
        }

    }
}
