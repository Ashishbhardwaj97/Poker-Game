using UnityEngine;
using UnityEngine.UI;

using agora_gaming_rtc;
using agora_utilities;


public class PokerAgoraVideo
{

    // instance of agora engine
    private IRtcEngine mRtcEngine;

    // load agora engine
    public void loadEngine(string appId)
    {
        // start sdk
        Debug.Log("initializeEngine");

        if (mRtcEngine != null)
        {
            Debug.Log("Engine exists. Please unload it first!");
            return;
        }

        // init engine
        mRtcEngine = IRtcEngine.GetEngine(appId);



        // enable log
        mRtcEngine.SetLogFilter(LOG_FILTER.DEBUG | LOG_FILTER.INFO | LOG_FILTER.WARNING | LOG_FILTER.ERROR | LOG_FILTER.CRITICAL);
    }

    public void join(string channel, int clientId)
    {
        Debug.Log("calling join (channel = " + channel + ")");

        if (mRtcEngine == null)
            return;

        // set callbacks (optional)
        mRtcEngine.OnJoinChannelSuccess = onJoinChannelSuccess;
        mRtcEngine.OnUserJoined = onUserJoined;
        mRtcEngine.OnUserOffline = onUserOffline;
        mRtcEngine.OnUserMutedAudio = OnAudioMuteRemoteUser;
        mRtcEngine.OnLeaveChannel = OnLeaveChannel;
        mRtcEngine.OnRemoteVideoStateChanged = OnRemotePlayerVideoStateChanged;
        mRtcEngine.OnUserMuteVideo = OnVideoMuteRemoteUser;
        mRtcEngine.OnConnectionStateChanged = OnConnectionStateChange;
        mRtcEngine.OnNetworkQuality = OnNetworkQuality;
        mRtcEngine.OnRemoteVideoStats = OnVideoStats;
        //mRtcEngine.on = OnMuteAllRemoteVideoStreams;
        // mRtcEngine.MuteLocalAudioStream = OnMuteLocalUser;

        // enable video
        mRtcEngine.EnableVideo();
        // allow camera output callback
        mRtcEngine.EnableVideoObserver();

        // join channel
        mRtcEngine.JoinChannel(channel, null, (uint)clientId);

        // Optional: if a data stream is required, here is a good place to create it
        int streamID = mRtcEngine.CreateDataStream(true, true);
        Debug.Log("initializeEngine done, data stream id = " + streamID);
    }

    public void SetVideoConfig()
    {
        // Create a VideoEncoderConfiguration instance. See the descriptions of the parameters in API Reference.
        VideoEncoderConfiguration config = new VideoEncoderConfiguration();

        // Sets the video resolution.
        config.dimensions.width = 320;
        config.dimensions.height = 180;

        // Sets the video frame rate.
        config.frameRate = FRAME_RATE.FRAME_RATE_FPS_15;

        // Sets the video encoding bitrate (Kbps).
        config.bitrate = 150;

        // Sets the adaptive orientation mode. See the description in API Reference.
        config.orientationMode = ORIENTATION_MODE.ORIENTATION_MODE_ADAPTIVE;

        // Sets the video encoding degradation preference under limited bandwidth. MIANTAIN_QUALITY means to degrade the frame rate to maintain the video quality.
        config.degradationPreference = DEGRADATION_PREFERENCE.MAINTAIN_FRAMERATE;

        // Sets the video encoder configuration.
        mRtcEngine.SetVideoEncoderConfiguration(config);
        Debug.Log("Video Config Applied");
    }

    public string getSdkVersion()
    {
        string ver = IRtcEngine.GetSdkVersion();
        if (ver == "2.9.1.45")
        {
            ver = "2.9.2";  // A conversion for the current internal version#
        }
        else
        {
            if (ver == "2.9.1.46")
            {
                ver = "2.9.2.2";  // A conversion for the current internal version#
            }
        }
        return ver;
    }

    public void leave()
    {
        Debug.Log("calling leave");

        if (mRtcEngine == null)
            return;

        // leave channel
        mRtcEngine.LeaveChannel();
        // deregister video frame observers in native-c code
        mRtcEngine.DisableVideoObserver();
        //unloadEngine();
    }

    // unload agora engine
    public void unloadEngine()
    {
        Debug.Log("calling unloadEngine");

        // delete
        if (mRtcEngine != null)
        {
            IRtcEngine.Destroy();  // Place this call in ApplicationQuit
            mRtcEngine = null;
        }
    }


    public void EnableVideo(bool pauseVideo)
    {
        if (mRtcEngine != null)
        {
            if (!pauseVideo)
            {
                mRtcEngine.EnableVideo();
            }
            else
            {
                mRtcEngine.DisableVideo();
            }
        }
    }

    // accessing GameObject in Scnene1
    // set video transform delegate for statically created GameObject
    public void onSceneHelloVideoLoaded()
    {
        if (GameManagerScript.instance.isObserver && GameManagerScript.instance.isTournament)
        {
            MuteLocalUser(true);
            MuteLocalVideo(true);
            MuteAllAudioRemoteUser(true);
            if(SocialTournamentScript.instance.isObserverAudio)
            {
                MuteAllAudioRemoteUser(false);
            }
            Debug.Log("Observer......... true");
            return;
        }
        GameObject quad = GameObject.Find("HostPlayerVideoPanel");
        if (ReferenceEquals(quad, null))
        {
            Debug.Log("BBBB: failed to find Quad");
            return;
        }
        else
        {
            quad.AddComponent<VideoSurface>();
            GameManagerScript.instance.hostPlayerVideoPanel = quad;
            //PlayersGenerator.instance.AssignVideoPanelToHostPlayer(GameManagerScript.instance.playersParent.transform.GetChild(GameManagerScript.instance.localPlayerSeatid - 1).GetChild(0).transform);
            onUserJoinCounter = 0;
        }

        PlayersGenerator.instance.AssignVideoToLocalPlayer();

    }

    //// implement engine callbacks
    //private void onJoinChannelSuccess(string channelName, uint uid, int elapsed)
    //{
    //    Debug.Log("JoinChannelSuccessHandler: uid = " + uid);
    //    // PlayersGenerator.instance.testVideo.text = uid.ToString();
    //    GameObject textVersionGameObject = GameObject.Find("VersionText");
    //    textVersionGameObject.GetComponent<Text>().text = "SDK Version : " + getSdkVersion();
    //}

    // implement engine callbacks
    private void onJoinChannelSuccess(string channelName, uint uid, int elapsed)
    {
        Debug.Log("JoinChannelSuccessHandler: uid = " + uid);
        // PlayersGenerator.instance.testVideo.text = uid.ToString();
        // GameObject textVersionGameObject = GameObject.Find("VersionText");
        // textVersionGameObject.GetComponent<Text>().text = "SDK Version : " + getSdkVersion();
    }

    // When a remote user joined, this delegate will be called. Typically
    // create a GameObject to render video on it
    private void onUserJoined(uint uid, int elapsed)
    {
        Debug.Log("onUserJoined: uid = " + uid + " elapsed = " + elapsed);
        if (GameManagerScript.instance.isTournament)
        {
            if (TournamentManagerScript.instance.observerList.Contains((int)uid))
            {
                MuteRemoteVolume(uid, 0);
                Debug.Log("Observer.........Entered..");
                return;
            }

        }
        // this is called in main thread

        // find a game object to render video stream from 'uid'
        GameObject go = GameObject.Find(uid.ToString());
        if (!ReferenceEquals(go, null))
        {
            return; // reuse
        }

        // create a GameObject and assign to this new user
        VideoSurface videoSurface = makeImageSurface(uid.ToString());
        if (!ReferenceEquals(videoSurface, null))
        {
            // configure videoSurface
            videoSurface.SetForUser(uid);
            videoSurface.SetEnable(true);
            videoSurface.SetVideoSurfaceType(AgoraVideoSurfaceType.Renderer);
            videoSurface.SetGameFps(30);
        }
        GameObject.Find(uid.ToString()).transform.GetChild(0).gameObject.SetActive(true);

        //PlayersGenerator.instance.videoPanelsForAllClient.Add(videoSurface.transform);

        //PlayersGenerator.instance.testVideo.text = videoSurface.transform.gameObject.name.ToString();
        //if (PlayersGenerator.instance.videoPanelsForAllClient.Count == PlayersGenerator.instance.videoPanelPlayers.Count)
        //{
        //    PlayersGenerator.instance.AssignVideoPanelsToPlayers();
        //}
        //else
        //{

        //    PlayersGenerator.instance.videoPanelForNewClient2 = videoSurface.transform;

        //}
    }

    private const float Offset = 100;
    public int onUserJoinCounter;
    // GameObject videoObj;
    public VideoSurface makeImageSurface(string goName)
    {
        GameObject go = new GameObject();
        if (go == null)
        {
            return null;
        }

        go.name = goName;
        go.tag = "PlayerVideoPanel";
        go.AddComponent<RawImage>();
        // go.transform.Rotate(0f, 0.0f, 180.0f);
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = new Vector3(1f, 1f, 1f);

        GameObject goChild = new GameObject();
        goChild.name = "Poor Connection Panel";
        goChild.AddComponent<RawImage>();
        goChild.transform.SetParent(go.transform);
        goChild.transform.localPosition = Vector3.zero;
        goChild.transform.localScale = new Vector3(1f, 1f, 1f);
        goChild.SetActive(false);
        onUserJoinCounter++;
        VideoSurface videoSurface = go.AddComponent<VideoSurface>();
        PlayersGenerator.instance.videoPanelsForAllClient.Add(go.transform);
        Debug.Log("SHUFFFFLLLLE");
        if (GameManagerScript.instance.totalPlayersSitting - 1 >= onUserJoinCounter)
        {
            PlayersGenerator.instance.AssignVideoPanelsToPlayers();
        }
        return videoSurface;
    }

    // When remote user is offline, this delegate will be called. Typically
    // delete the GameObject for this user
    private void onUserOffline(uint uid, USER_OFFLINE_REASON reason)
    {
        // remove video stream
        Debug.Log("onUserOffline: uid = " + uid + " reason = " + reason);

        // this is called in main thread
        if (reason == USER_OFFLINE_REASON.QUIT)
        {
            GameObject go = GameObject.Find(uid.ToString());
            //if (GameManagerScript.instance.isTournament)
            //{
            if (!ReferenceEquals(go, null))
            {
                for (int i = 0; i < PlayersGenerator.instance.videoPanelsForAllClient.Count; i++)
                {
                    if (int.Parse(go.name) == int.Parse(PlayersGenerator.instance.videoPanelsForAllClient[i].name))
                    {
                        PlayersGenerator.instance.videoPanelsForAllClient.Remove(PlayersGenerator.instance.videoPanelsForAllClient[i]);
                    }
                }
                Object.Destroy(go);
                onUserJoinCounter--;
                Debug.Log("onUserJoinCounter " + onUserJoinCounter);
            }
        }
        //}

    }

    //.............. All Emitter Events to Agora...........//

    #region Agora Emitter Events

    public void MuteLocalUser(bool mute)
    {
        //Debug.Log("HGGHGKJG1111");
        mRtcEngine.MuteLocalAudioStream(mute);
    }

    public void MuteAllAudioRemoteUser(bool mute) 
    {
        //Debug.Log("HGGHGKJG1111");
        mRtcEngine.MuteAllRemoteAudioStreams(mute);
    }

    public void MuteAllRemoteVideoStreamsUser(bool mute) 
    {
        mRtcEngine.MuteAllRemoteVideoStreams(mute);
    }

    public void MuteRemoteVolume(uint uid, int vol)
    {
        mRtcEngine.AdjustUserPlaybackSignalVolume(uid, vol);
    }

    public void MuteLocalVideo(bool mute)
    {
        Debug.Log("Local User Video Mute Success " + mute);
        // AgoraInit.instance.leaveChannelTest.text = "Test " + stats;
        mRtcEngine.MuteLocalVideoStream(mute);
    }
    #endregion

    //.............................Ends..........................//

    //.............. All CallBacks Received from agora...........//

    #region Agora Callbacks

    private void OnAudioMuteRemoteUser(uint uid, bool muted)
    {
        Debug.Log("Remote User Audio Mute Success " + uid);
       // AllButtons.instance.testUI.text = uid.ToString() + " " + muted.ToString();
        PlayersGenerator.instance.FindPlayerToMuteAudioByUid((int)uid, muted);
    }

    private void OnLeaveChannel(RtcStats stats)
    {
        Debug.Log("On Remote player leave " + stats);
       // AgoraInit.instance.leaveChannelTest.text = "Test " + stats;
       
    }

    private void OnRemotePlayerVideoStateChanged(uint uid, REMOTE_VIDEO_STATE state , REMOTE_VIDEO_STATE_REASON reason, int elapsed)
    {
        //Debug.Log("REMOTE_VIDEO_UID " + uid + " REMOTE_VIDEO_STATE " + (int)state + " REMOTE_VIDEO_STATE_REASON " + (int)reason );
        //Debug.Log();
        //Debug.Log("REMOTE_VIDEO_STATE_REASON" + reason);
        //Debug.Log("REMOTE_VIDEO_elapsed" + elapsed);

        if ((int)reason == 5 /*state == 0 */ /*|| (int)state == 3*/ || (int)state == 4 )
        {

            if (GameObject.Find(uid.ToString()) != null)
            {
                if (!GameObject.Find(uid.ToString()).transform.GetChild(0).gameObject.activeInHierarchy)
                {
                    GameObject.Find(uid.ToString()).transform.GetChild(0).gameObject.SetActive(true);
                }

            }

        }
        else if ((int)state == 1 || (int)state == 2 || (int)reason == 6)
        {
            if (GameObject.Find(uid.ToString()) != null)
            {
                if (GameObject.Find(uid.ToString()).transform.GetChild(0).gameObject.activeInHierarchy)
                {
                    GameObject.Find(uid.ToString()).transform.GetChild(0).gameObject.SetActive(false);
                }

            }
        }

    }

    private void OnVideoMuteRemoteUser(uint uid, bool muted)
    {
        Debug.Log("Remote User Video Mute Success " + uid);
        // AllButtons.instance.testUI.text = uid.ToString() + " " + muted.ToString();
        PlayersGenerator.instance.FindPlayerToMuteAudioByUid((int)uid, muted);
    }

    private void OnConnectionStateChange(CONNECTION_STATE_TYPE state, CONNECTION_CHANGED_REASON_TYPE reason)
    {
        //Debug.Log("CONNECTION_STATE_TYPE " + state);
        //Debug.Log("CONNECTION_CHANGED_REASON_TYPE"+ reason);
        //// AllButtons.instance.testUI.text = uid.ToString() + " " + muted.ToString();
        //PlayersGenerator.instance.FindPlayerToMuteAudioByUid((int)uid, muted);
    }

    private void OnMuteAllRemoteVideoStreams(CONNECTION_STATE_TYPE state, CONNECTION_CHANGED_REASON_TYPE reason)
    {
        //Debug.Log("CONNECTION_STATE_TYPE " + state);
        //Debug.Log("CONNECTION_CHANGED_REASON_TYPE" + reason);
        //// AllButtons.instance.testUI.text = uid.ToString() + " " + muted.ToString();
        //PlayersGenerator.instance.FindPlayerToMuteAudioByUid((int)uid, muted);
    }

    private void OnNetworkQuality(uint uid, int txQuality, int rxQuality)
    {
        //Debug.Log("uid " + uid + " txQuality " + txQuality + " rxQuality " + rxQuality);
    }

    private void OnVideoStats(RemoteVideoStats remoteVideoStats)
    {
        //Debug.Log(" uid " + remoteVideoStats.uid + " receivedBitrate " + remoteVideoStats.receivedBitrate);/*+ " totalFrozenTime " + remoteVideoStats.totalFrozenTime + " delay " + remoteVideoStats.delay + " rxStreamType " + remoteVideoStats.rxStreamType + " packetLossRate " + remoteVideoStats.packetLossRate + " frozenRate " + remoteVideoStats.frozenRate);*/

        if (GameObject.Find(remoteVideoStats.uid.ToString()) != null && remoteVideoStats.receivedBitrate < 1)
        {
            if (!GameObject.Find(remoteVideoStats.uid.ToString()).transform.GetChild(0).gameObject.activeInHierarchy)
            {
                //GameObject.Find(remoteVideoStats.uid.ToString()).transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        if (GameObject.Find(remoteVideoStats.uid.ToString()) != null && remoteVideoStats.receivedBitrate > 1)
        {
            if (GameObject.Find(remoteVideoStats.uid.ToString()).transform.GetChild(0).gameObject.activeInHierarchy)
            {
                //GameObject.Find(remoteVideoStats.uid.ToString()).transform.GetChild(0).gameObject.SetActive(false);
            }

        }
    }
    #endregion

    //.............................Ends..........................//
}
