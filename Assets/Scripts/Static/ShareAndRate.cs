using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;

public class ShareAndRate : MonoBehaviour
{

    string subject = "Poker";
    string body = string.Empty;

  
    public void OnClickTextSharing() 
    {
        ShareSheet shareSheet = ShareSheet.CreateInstance();
        shareSheet.AddText(ConnectToSocialMediaScript.instance.shareLinkMessage);
        
        shareSheet.AddURL(URLString.URLWithPath(ConnectToSocialMediaScript.instance.shareLinkString));
        shareSheet.SetCompletionCallback((result, error) => {
            Debug.Log("Share Sheet was closed. Result code: " + result.ResultCode);
        });
        shareSheet.Show();
    }


    public void RateUs()
    {
#if UNITY_ANDROID
        Application.OpenURL("market://details?id=YOUR_ID");
#elif UNITY_IPHONE
  Application.OpenURL("itms-apps://itunes.apple.com/app/idYOUR_ID");
#endif
    }

}

