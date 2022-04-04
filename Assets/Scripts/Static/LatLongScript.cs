using UnityEngine;
using System.Collections;
using UnityEngine.Android;
public class LatLongScript : MonoBehaviour
{
    public static LatLongScript instance;
    internal float latitude;
    internal float longitude;

    internal bool islatlong;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }
        //#elif UNITY_IOS
        //PlayerSettings.iOS.locationUsageDescription = "Details to use location";
#endif
        GettingLatLong();
    }

    public void GettingLatLong()
    {
        Debug.Log("GettingLatLong..........");
        StartCoroutine(StartLocationService());
    }
    private IEnumerator StartLocationService()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("User has not enabled location");
            yield break;
        }
        Input.location.Start();
        while (Input.location.status == LocationServiceStatus.Initializing)
        {
            yield return new WaitForSeconds(1);
        }
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            yield break;
        }
        latitude = Input.location.lastData.latitude;
        longitude = Input.location.lastData.longitude;
        Debug.Log("Latitude : " + Input.location.lastData.latitude);
        Debug.Log("Longitude : " + Input.location.lastData.longitude);
        Debug.Log("Altitude : " + Input.location.lastData.altitude);
        islatlong = true;
    }
}