using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
using System.Collections;
using UnityEngine.Android;
#endif

/// <summary>
///    TestHome serves a game controller object for this application.
/// </summary>
public class AgoraInit : MonoBehaviour
{

    public static AgoraInit instance;

    //public Text leaveChannelTest;

    // Use this for initialization
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
    private ArrayList permissionList = new ArrayList();
#endif
    static PokerAgoraVideo app = null;

    private string HomeSceneName = "SceneHome";

    // private string PlaySceneName = "SceneHelloVideo";

    // PLEASE KEEP THIS App ID IN SAFE PLACE
    // Get your own App ID at https://dashboard.agora.io/
    [SerializeField]
    private string AppID = "your_appid";
    // public Text muteTest;

    void Awake()
    {

        instance = this;
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
        permissionList.Add(Permission.Microphone);
        permissionList.Add(Permission.Camera);
#endif

        // keep this alive across scenes
        DontDestroyOnLoad(this.gameObject);

        //if (instance != null)
        //{
        //    Destroy(this.gameObject);
        //}
        //else
        //{

        //    instance = this;

        //    DontDestroyOnLoad(this.gameObject);
        //}
    }

    void Start()
    {
        CheckAppId();
    }

    void Update()
    {
        CheckPermissions();
    }

    private void CheckAppId()
    {
        Debug.Assert(AppID.Length > 10, "Please fill in your AppId first on Game Controller object.");
    }

    /// <summary>
    ///   Checks for platform dependent permissions.
    /// </summary>
    private void CheckPermissions()
    {
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
        foreach (string permission in permissionList)
        {
            if (!Permission.HasUserAuthorizedPermission(permission))
            {
                Permission.RequestUserPermission(permission);
            }
        }
#endif
    }

    public void onJoinButtonClicked(string tableId, int clientId)
    {
        // get parameters (channel name, channel profile, etc.)
        //   GameObject go = GameObject.Find("ChannelName");
        // InputField field = go.GetComponent<InputField>();

        // create app if nonexistent
        if (ReferenceEquals(app, null))
        {
            app = new PokerAgoraVideo(); // create app
            app.loadEngine(AppID); // load engine
        }

        app.SetVideoConfig();       // TO change video configuration          //Update

        // join channel and jump to next scene
        app.join(tableId, clientId);
        OnLevelFinishedLoading();
        //("unity3d");
        // SceneManager.sceneLoaded += OnLevelFinishedLoading; // configure GameObject after scene is loaded
        // SceneManager.LoadScene(PlaySceneName, LoadSceneMode.Single);
    }

    public void onLeaveButtonClicked()
    {
        if (!ReferenceEquals(app, null))
        {
            app.leave(); // leave channel
            app.unloadEngine(); // delete engine
            app = null; // delete app
            SceneManager.LoadScene(HomeSceneName, LoadSceneMode.Single);
        }
        Destroy(gameObject);
    }

    public void OnLevelFinishedLoading()
    {
        //  if (scene.name == PlaySceneName)
        //  {
        if (!ReferenceEquals(app, null))
        {
            app.onSceneHelloVideoLoaded(); // call this after scene is loaded

        }
        // SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        // }
    }

    void OnApplicationPause(bool paused)
    {
        if (!ReferenceEquals(app, null))
        {
            app.EnableVideo(paused);
        }
    }

    void OnApplicationQuit()
    {
        if (!ReferenceEquals(app, null))
        {
            app.unloadEngine();
        }
    }


    public void MuteUnmuteLocalPlayer(bool mute)
    {
        if (!ReferenceEquals(app, null))
        {
            Debug.Log("HGGHGKJG22222");
            app.MuteLocalUser(mute);
        }
    }

    public void MuteUnmuteAllRemotePlayer(bool mute)
    {
        if (!ReferenceEquals(app, null))
        {
            Debug.Log("HGGHGKJG22222");
            app.MuteAllAudioRemoteUser(mute);
        }
    }

    public void LeaveChannel()
    {
        if (!ReferenceEquals(app, null))
        {
            Debug.Log("leave");
            app.leave();
        }
    }

    public void MuteVideo(bool mute)
    {
        if (!ReferenceEquals(app, null))
        {
            Debug.Log("video mute " + mute);
            app.MuteLocalVideo(mute);
        }
    }

    public void MuteAllRemoteVideo(bool mute)
    {
        if (!ReferenceEquals(app, null))
        {
            Debug.Log("all remote video mute " + mute);
            app.MuteAllRemoteVideoStreamsUser(mute);
        }
    }

    public void ResetUserJoinCount(int response)
    {
        if (!ReferenceEquals(app, null))
        {
            if (response == 0)
            {
                app.onUserJoinCounter = 0;
            }
            else if (response == 1)
            {
                app.onUserJoinCounter--;
            }
        }
    }

    public void TurnUserVOllOnOff(uint userId, int volume)
    {
        if (!ReferenceEquals(app, null))
        {
            app.MuteRemoteVolume(userId, volume);
        }
    }
}
