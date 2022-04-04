using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using agora_gaming_rtc;
using agora_utilities;
using System;

public class PlayersGenerator : MonoBehaviour
{

    public static PlayersGenerator instance;

    public List<Transform> videoPanelsForAllClient, videoPanelPlayers;
    public Transform videoPanelForNewClient2;

    private void Awake()
    {
        instance = this;
    }

    public void InstantiateAllOtherPlayer(GameSerializeClassesCollection.Player players)
    {
        if (GameObject.Find(players.playerName) == null )
        {
            if(GameManagerScript.instance.playersParent.transform.GetChild(players.seatId - 1).childCount == 1)
            {
                GameManagerScript.instance.totalPlayersSitting++;
                GameManagerScript.instance.CheckTotalPlayersSitting();
                GameObject remotePlayerObj = SpawnManager.instance.Spawn("PlayerPool", "PlayerPrefab", Vector3.zero, Vector3.zero) as GameObject;
                if (!remotePlayerObj.activeInHierarchy)
                {
                    remotePlayerObj.SetActive(true);
                }
                remotePlayerObj.transform.SetParent(GameManagerScript.instance.playersParent.transform.GetChild(players.seatId - 1));
                //GameManagerScript.instance.playersParent.transform.GetChild(players.seatId - 1).GetChild(0).GetChild(0).gameObject.SetActive(false);
                PokerPlayerController playerController = remotePlayerObj.GetComponent<PokerPlayerController>();
                playerController.isLocalPlayer = false;
                playerController.isBeforeLocalPlayer = true;
                playerController.isAfterLocalPlayer = false;
                //playerController.timesAnimation = GameManagerScript.instance.timesAnimBeforeLocalPlayer;
                remotePlayerObj.name = players.playerName;
                playerController.player.playerName = players.playerName;
                playerController.player.chips = players.initialChips;
                playerController.player.seatId = players.seatId;
                playerController.player.clientId = players.clientId;
                print("Client ID 1111 " + players.clientId);
                playerController.player.user_image = players.user_image;
                videoPanelPlayers.Add(remotePlayerObj.transform);
            }
            else
            {
                string playerName = GameManagerScript.instance.playersParent.transform.GetChild(players.seatId - 1).GetChild(0).name;

                GameSerializeClassesCollection.instance.onError.token = Communication.instance.playerToken;
                GameSerializeClassesCollection.instance.onError.eventname = " __all_client";
                GameSerializeClassesCollection.instance.onError.message = " Seatid Already Occupied by " + playerName;
                string data2 = JsonUtility.ToJson(GameSerializeClassesCollection.instance.onError);
                print(data2);
                if (!GameManagerScript.instance.isTournament && GameManagerScript.instance.networkManager.activeInHierarchy)
                {
                    PokerNetworkManager.instance.socket.Emit("custom_error", new JSONObject(data2));
                    //UIManagerScript.instance.TableToPokerUI(5);
                }
            }
           
        }
        else
        {
            GameSerializeClassesCollection.instance.onError.token = Communication.instance.playerToken;
            GameSerializeClassesCollection.instance.onError.eventname = " __all_client";
            GameSerializeClassesCollection.instance.onError.message = " Player Already Sitting ";
            string data2 = JsonUtility.ToJson(GameSerializeClassesCollection.instance.onError);
            print(data2);
            if (!GameManagerScript.instance.isTournament && GameManagerScript.instance.networkManager.activeInHierarchy)
            {
                    PokerNetworkManager.instance.socket.Emit("custom_error", new JSONObject(data2));
                    //UIManagerScript.instance.TableToPokerUI(5);
            }
        }
    }

    public void InstantiateLocalPlayer(GameSerializeClassesCollection.Players localPlayer, bool isResume)
    {
        StartCoroutine(InstantiateLocalPlayerCoroutine(localPlayer,isResume));
    }

    IEnumerator InstantiateLocalPlayerCoroutine(GameSerializeClassesCollection.Players localPlayer, bool isResume)
    {
        //.........................................HOLD ON IF ANOTHER PLAYER IS GENERATING..................................................................//
        while (true)
        {
            if (ChairAnimation.instance.isChairAnimComplete && !GameManagerScript.instance.isPlayerGenerating)
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        //.................................................................................................................................................//

        if (GameObject.Find(localPlayer.player.playerName) == null )
        {
            if(GameManagerScript.instance.playersParent.transform.GetChild(localPlayer.player.seatId - 1).childCount == 1)
            {

                GameManagerScript.instance.totalPlayersSitting++;
                GameManagerScript.instance.CheckTotalPlayersSitting();
//#if !UNITY_EDITOR
                if (GameManagerScript.instance.isVideoTable && !isResume)
                {
                    if (GameManagerScript.instance.isTournament)
                    {
                        //GameManagerScript.instance.OnVideoEngine(TournamentManagerScript.instance.tableNo + GameSerializeClassesCollection.instance.objMTTJoinClass.tournament_id, localPlayer.player.clientId);
                        GameManagerScript.instance.OnVideoEngine(TournamentManagerScript.instance.videoChannel, localPlayer.player.clientId);
                    }
                    else
                    {
                        GameManagerScript.instance.OnVideoEngine(GameSerializeClassesCollection.instance.playerData.ticket, localPlayer.player.clientId);
                    }
                }
                //#endif

                UIManagerScript.instance.OnPlayerJoin();
                print("local player name " + localPlayer.player.playerName);
                GameObject localPlayerObj = SpawnManager.instance.Spawn("PlayerPool", "PlayerPrefab", Vector3.zero, Vector3.zero) as GameObject;
                if (!localPlayerObj.activeInHierarchy)
                {
                    localPlayerObj.SetActive(true);
                }
                GameManagerScript.instance.localPlayerSeatid = localPlayer.player.seatId;
                //localPlayerObj.transform.SetParent(GameManagerScript.instance.playersParent.transform.GetChild(0));
                localPlayerObj.transform.SetParent(GameManagerScript.instance.playersParent.transform.GetChild(localPlayer.player.seatId - 1));
                PokerPlayerController playerController = localPlayerObj.GetComponent<PokerPlayerController>();
                playerController.isLocalPlayer = true;
                playerController.isBeforeLocalPlayer = false;
                playerController.isAfterLocalPlayer = false;
                //playerController.timesAnimation = GameManagerScript.instance.timesAnimLocalPlayer;
                localPlayerObj.name = localPlayer.player.playerName;
                playerController.player.playerName = localPlayer.player.playerName;
                playerController.player.chips = localPlayer.player.initialChips;
                playerController.player.seatId = localPlayer.player.seatId;
                playerController.player.clientId = localPlayer.player.clientId;
                playerController.player.user_image = localPlayer.player.user_image;

                print("local player " + localPlayer.player.playerName);
                print("Client ID local player " + localPlayer.player.clientId);
                // UIManagerScript.instance.OnPlayerJoin();
                //GameManagerScript.instance.ShuffleStart();
                GameManagerScript.instance.localSeatID = localPlayer.player.seatId;
                if(!GameManagerScript.instance.isTournament)
                {
                    PokerNetworkManager.instance.localPlayer = localPlayerObj;
                }
                else
                {
                    TournamentManagerScript.instance.localPlayer = localPlayerObj;
                }
                StartCoroutine(GameManagerScript.instance.ShuffleStart());
                GameManagerScript.instance.isObserver = false;
                CardShuffleAnimation.instance.shuffleCardCount = 0;
                //if (Table.instance.table.status != 0 && GameManagerScript.instance.isVideoTable && !GameManagerScript.instance.isDirectLogIn)
                //{
                //AssignVideoPanelsToPlayers();
                //}
                //else
                //{
                //    AssignVideoPanelsToPlayers();
                //}
               
            }
            else
            {
                string playerName = GameManagerScript.instance.playersParent.transform.GetChild(localPlayer.player.seatId - 1).GetChild(0).name;

                GameSerializeClassesCollection.instance.onError.token = Communication.instance.playerToken;
                GameSerializeClassesCollection.instance.onError.eventname = " __new_client";
                GameSerializeClassesCollection.instance.onError.message = "Seatid Already Occupied by " + playerName;
                string data2 = JsonUtility.ToJson(GameSerializeClassesCollection.instance.onError);
                print(data2);
                if (!GameManagerScript.instance.isTournament && GameManagerScript.instance.networkManager.activeInHierarchy)
                {
                    PokerNetworkManager.instance.socket.Emit("custom_error", new JSONObject(data2));
                    //UIManagerScript.instance.TableToPokerUI(5);
                }
            }
            
        }
        else
        {

            GameSerializeClassesCollection.instance.onError.token = Communication.instance.playerToken;
            GameSerializeClassesCollection.instance.onError.eventname = " __new_client";
            GameSerializeClassesCollection.instance.onError.message = "Player Already Sitting ";
            string data2 = JsonUtility.ToJson(GameSerializeClassesCollection.instance.onError);
            print(data2);
            if (!GameManagerScript.instance.isTournament && GameManagerScript.instance.networkManager.activeInHierarchy)
            {
                PokerNetworkManager.instance.socket.Emit("custom_error", new JSONObject(data2));
                //UIManagerScript.instance.TableToPokerUI(5);
            }
        }

        yield return null;

    }
    //bool threadEntryLock;
    public void InstantiateNewRemotePlayer(GameSerializeClassesCollection.Players newRemotePlayer)
    {
        if (!GameManagerScript.instance.isTournament)
        {
            lock (PokerNetworkManager.instance.entry)
            {
                StartCoroutine(InstantiateNewRemotePlayerCoroutine(newRemotePlayer));
            }
        }
        else
        {
            lock (TournamentManagerScript.instance.entry)
            {
                StartCoroutine(InstantiateNewRemotePlayerCoroutine(newRemotePlayer));
            }
        }
    }

    int timedOut;
    IEnumerator InstantiateNewRemotePlayerCoroutine(GameSerializeClassesCollection.Players newRemotePlayer)
    {
        float time = 2;
        //.........................................HOLD ON IF ANOTHER PLAYER IS GENERATING..................................................................//
        while (true)
        {
            if(time < 0.5f)
            {
                break;
            }
            if (GameManagerScript.instance.isShuffflingComplete && !GameManagerScript.instance.isPlayerGenerating)
            {
                break;
            } 
            print("Holding player........."+newRemotePlayer.player.playerName+ "............ new client 2");
            time -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        //.................................................................................................................................................//

        int actualSeatId = GameManagerScript.instance.GetActualSeatID(newRemotePlayer.player.seatId);

        if (GameObject.Find(newRemotePlayer.player.playerName) == null)
        {
            if (GameManagerScript.instance.playersParent.transform.GetChild(actualSeatId).childCount == 1)
            {

                GameManagerScript.instance.totalPlayersSitting++;
                GameManagerScript.instance.CheckTotalPlayersSitting();
                GameObject remotePlayerObj = SpawnManager.instance.Spawn("PlayerPool", "PlayerPrefab", Vector3.zero, Vector3.zero) as GameObject;
                if (!remotePlayerObj.activeInHierarchy)
                {
                    remotePlayerObj.SetActive(true);
                }

                actualSeatId = GameManagerScript.instance.GetActualSeatID(newRemotePlayer.player.seatId);
                remotePlayerObj.transform.SetParent(GameManagerScript.instance.playersParent.transform.GetChild(actualSeatId));
                PokerPlayerController playerController = remotePlayerObj.GetComponent<PokerPlayerController>();
                playerController.isLocalPlayer = false;
                playerController.isBeforeLocalPlayer = false;
                playerController.isAfterLocalPlayer = true;
                //playerController.timesAnimation = GameManagerScript.instance.timesAnimAfterLocalPlayer;
                remotePlayerObj.name = newRemotePlayer.player.playerName;
                playerController.player.playerName = newRemotePlayer.player.playerName;
                playerController.player.chips = newRemotePlayer.player.initialChips;
                playerController.player.seatId = newRemotePlayer.player.seatId;
                playerController.player.clientId = newRemotePlayer.player.clientId;
                playerController.player.user_image = newRemotePlayer.player.user_image;
                print("Client ID 3333 " + newRemotePlayer.player.clientId);
                videoPanelPlayers.Add(remotePlayerObj.transform);
                //if (Table.instance.table.status != 0 && GameManagerScript.instance.isVideoTable && !GameManagerScript.instance.isDirectLogIn)
                //{
                //    AssignVideoPanelsToPlayers();
                //}
                //else
                //{
                //    AssignVideoPanelsToPlayers();
                //}
            }
            else
            {
                string playerName = GameManagerScript.instance.playersParent.transform.GetChild(actualSeatId).GetChild(0).name;

                GameSerializeClassesCollection.instance.onError.token = Communication.instance.playerToken;
                GameSerializeClassesCollection.instance.onError.eventname = " __new_client_2";
                GameSerializeClassesCollection.instance.onError.message = " Seatid Already Occupied "+ playerName;
                string data2 = JsonUtility.ToJson(GameSerializeClassesCollection.instance.onError);
                print(data2);
                if (!GameManagerScript.instance.isTournament && GameManagerScript.instance.networkManager.activeInHierarchy)
                {
                    PokerNetworkManager.instance.socket.Emit("custom_error", new JSONObject(data2));
                    //UIManagerScript.instance.TableToPokerUI(5);
                }
            }
        }
        else
        {
            GameSerializeClassesCollection.instance.onError.token = Communication.instance.playerToken;
            GameSerializeClassesCollection.instance.onError.eventname = " __new_client_2";
            GameSerializeClassesCollection.instance.onError.message = " Player Already Sitting ";
            string data2 = JsonUtility.ToJson(GameSerializeClassesCollection.instance.onError);
            print(data2);
            if (!GameManagerScript.instance.isTournament && GameManagerScript.instance.networkManager.activeInHierarchy)
            {
                PokerNetworkManager.instance.socket.Emit("custom_error", new JSONObject(data2));
                //UIManagerScript.instance.TableToPokerUI(5);
            }
        }

        yield return null;

    }

    public void AssignVideoToLocalPlayer()
    {
        StartCoroutine(AssignVideoToLocalPlayerCoroutine());
    }

    public IEnumerator AssignVideoToLocalPlayerCoroutine() 
    {
        yield return new WaitForSeconds(1f);

        AssignVideoPanelToHostPlayer(GameManagerScript.instance.playersParent.transform.GetChild(GameManagerScript.instance.localPlayerSeatid - 1).GetChild(0).transform);
    }

    public void AssignVideoPanelToHostPlayer(Transform player)
    {
        GameManagerScript.instance.hostPlayerVideoPanel.transform.SetParent(player.transform.GetChild(1));
        GameManagerScript.instance.hostPlayerVideoPanel.transform.localPosition = Vector3.zero;
        GameManagerScript.instance.hostPlayerVideoPanel.transform.localEulerAngles = new Vector3(0, 0, 180);
        GameManagerScript.instance.hostPlayerVideoPanel.transform.localScale = new Vector3(1, 1, 1);
        GameManagerScript.instance.hostPlayerVideoPanel.transform.GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
        GameManagerScript.instance.hostPlayerVideoPanel.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(440f, 260f);
        //GameManagerScript.instance.hostPlayerVideoPanel.transform.GetChild(0).localEulerAngles = new Vector3(0, 0, 180);
        //GameManagerScript.instance.hostPlayerVideoPanel.transform.GetChild(0).GetComponent<RawImage>().texture = UIManagerScript.instance.videoPoorConnectionTexture;
        //GameManagerScript.instance.hostPlayerVideoPanel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(440f, 260f);
    }

    IEnumerator Assign()
    {
        //yield return new WaitForSeconds(4f);
        //AssignVideoPanelToHostPlayer(GameManagerScript.instance.playersParent.transform.GetChild(GameManagerScript.instance.localPlayerSeatid - 1).GetChild(0).transform);
      
        //AssignVideoPanelToHostPlayer(GameManagerScript.instance.playersParent.transform.GetChild(0).GetChild(0).transform);
        yield return new WaitForSeconds(2f);
        if (videoPanelsForAllClient.Count > 0)
        {
            //if (videoPanelPlayers.Count == videoPanelsForAllClient.Count)
            //{
            for (int i = 0; i < videoPanelsForAllClient.Count; i++)
            {
                for (int j = 0; j < videoPanelPlayers.Count; j++)
                {
                    try
                    {
                        if (videoPanelsForAllClient[i].name == videoPanelPlayers[j].GetComponent<PokerPlayerController>().player.clientId.ToString())
                        {
                            videoPanelsForAllClient[i].SetParent(videoPanelPlayers[j].transform.GetChild(1));
                            videoPanelsForAllClient[i].localPosition = Vector3.zero;
                            videoPanelsForAllClient[i].localEulerAngles = new Vector3(0, 0, 180);
                            videoPanelsForAllClient[i].localScale = new Vector3(1, 1, 1);
                            videoPanelsForAllClient[i].GetComponent<RectTransform>().sizeDelta = new Vector2(440f, 260f);
                            videoPanelsForAllClient[i].GetChild(0).localEulerAngles = new Vector3(0, 0, 180);
                            videoPanelsForAllClient[i].GetChild(0).GetComponent<RawImage>().texture = UIManagerScript.instance.videoPoorConnectionTexture;
                            videoPanelsForAllClient[i].GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(440f, 260f);
                            //yield return new WaitForSeconds(0.1f);
                            break;
                        }

                    }
                    catch
                    {
                        print("Error found...........");
                    }
                }
            }
        }
        //}
    }

    public void AssignVideoPanelsToPlayers()
    {
        try
        {
            StartCoroutine(Assign());
        }
        catch
        {
            print("Problem in Assign Video Panels To Players");
        }
    }

    Transform micParent;
    //bool islocalplayer;
    public void FindPlayerToMuteAudioByUid(int uid, bool mute)
    {
        try
        {
            if (GameObject.Find(uid.ToString()) != null)
            {
                if (GameObject.Find(uid.ToString()).transform.parent != null)
                {
                    //AgoraInit.instance.muteTest.text = " Test111 " + uid + mute;
                    // micParent = GameObject.Find(uid.ToString()).transform.parent.parent.GetComponent<PokerPlayerController>().micImage;
                    micParent = GameObject.Find(uid.ToString()).transform.parent.parent.GetComponent<PokerPlayerController>().commonUi.GetChild(15).GetChild(15);

                    if (mute)
                    {
                        GameObject.Find(uid.ToString()).transform.parent.parent.GetComponent<PokerPlayerController>().muteOn.SetActive(true);
                    }
                    else
                    {
                        GameObject.Find(uid.ToString()).transform.parent.parent.GetComponent<PokerPlayerController>().muteOn.SetActive(false);
                    }

                }
                else
                {
                    for (int i = 0; i < videoPanelPlayers.Count; i++)
                    {
                        if (videoPanelPlayers[i].GetComponent<PokerPlayerController>().player.clientId == uid)
                        {
                            // micParent = videoPanelPlayers[i].GetComponent<PokerPlayerController>().micImage;
                            micParent = videoPanelPlayers[i].GetComponent<PokerPlayerController>().commonUi.GetChild(15).GetChild(15);
                            // AgoraInit.instance.muteTest.text = " Test2222 " + uid + mute;
                            break;
                        }
                    }
                }
            }
            //if(GameObject.Find(uid.ToString()).transform.parent.parent.GetComponent<PokerPlayerController>().isLocalPlayer)
            //{
            //    islocalplayer = true;
            //}

            if (mute /*&& islocalplayer*/)
            {
                micParent.GetChild(0).gameObject.SetActive(false);
                micParent.GetChild(1).gameObject.SetActive(true);
            }
            else /*if(islocalplayer)*/
            {
                micParent.GetChild(0).gameObject.SetActive(true);
                micParent.GetChild(1).gameObject.SetActive(false);
            }
            //islocalplayer = false;
        }
        catch
        {
            print("Some Error in Mic Off/ON");
        }

    }


    public void OnPlayerLeft(GameSerializeClassesCollection.Players newRemotePlayer)
    {
        int seatID = GameManagerScript.instance.GetActualSeatID(newRemotePlayer.player.seatId);
        GameManagerScript.instance.playersParent.transform.GetChild(seatID).GetChild(0).GetComponent<PokerPlayerController>().PlayerDeactivate(true);
        SpawnManager.instance.Despawn("PlayerPool", GameManagerScript.instance.playersParent.transform.GetChild(seatID).GetChild(0));
    }

    public void CurrentPlayerLeft()
    {
        //if (GameObject.Find(GameSerializeClassesCollection.instance.onCurrentPlayerLeft.playerName) != null)
        //{
        //    GameObject currplayer = GameObject.Find(GameSerializeClassesCollection.instance.onCurrentPlayerLeft.playerName);
        //    currplayer.GetComponent<PokerPlayerController>().PlayerDeactivate();
        //    SpawnManager.instance.Despawn("PlayerPool", currplayer.transform);
        //    print("CurrentPlayerLeft complete");
        //}
        StartCoroutine(CurrentPlayerLeftCoroutine());
    }

    IEnumerator CurrentPlayerLeftCoroutine() 
    {

        while (true)
        {
            if (ChairAnimation.instance.isChairAnimComplete &&  CardShuffleAnimation.instance.isAnimationComplete)
            {
                break;
            }
            yield return new WaitForSeconds(0.2f);
        }

        //if (GameObject.Find(GameSerializeClassesCollection.instance.onCurrentPlayerLeft.playerName) == null)
        //{
        //    yield return new WaitForSeconds(2f);
        //}

        if (GameObject.Find(GameSerializeClassesCollection.instance.onCurrentPlayerLeft.playerName) != null)
        {
            GameManagerScript.instance.totalPlayersSitting--;
            GameManagerScript.instance.CheckTotalPlayersSitting();
            AgoraInit.instance.ResetUserJoinCount(1);
            GameObject currplayer = GameObject.Find(GameSerializeClassesCollection.instance.onCurrentPlayerLeft.playerName);
            if (currplayer.GetComponent<PokerPlayerController>().isLocalPlayer)
            {
                //GameSerializeClassesCollection.instance.localPlayerConfirmation.playerName = GameSerializeClassesCollection.instance.onCurrentPlayerLeft.playerName;
                //GameSerializeClassesCollection.instance.localPlayerConfirmation.ticket = GameSerializeClassesCollection.instance.onCurrentPlayerLeft.tableNumber;
                //GameSerializeClassesCollection.instance.localPlayerConfirmation.seatId = GameSerializeClassesCollection.instance.onCurrentPlayerLeft.seatId;
                //string data = JsonUtility.ToJson(GameSerializeClassesCollection.instance.localPlayerConfirmation);
                //print("local Player Exit Confirmation " + data);
                //PokerNetworkManager.instance.socket.Emit("__left_player_details", new JSONObject(data));
                UIManagerScript.instance.TableToPokerUI(4);
            }
            else
            {
                currplayer.GetComponent<PokerPlayerController>().PlayerDeactivate(true);
                SpawnManager.instance.Despawn("PlayerPool", currplayer.transform);
                print("CurrentPlayerLeft complete");
            }
        }
        else
        {
           
        
            GameSerializeClassesCollection.instance.onError.token = Communication.instance.playerToken;
            GameSerializeClassesCollection.instance.onError.eventname = " __current_left_player";
            GameSerializeClassesCollection.instance.onError.message = "NO PLAYER "+ GameSerializeClassesCollection.instance.onCurrentPlayerLeft.playerName  + " FOUND ON THE TABLE -> Unnecessary event";
            string data2 = JsonUtility.ToJson(GameSerializeClassesCollection.instance.onError);
            print(data2);
            PokerNetworkManager.instance.socket.Emit("custom_error", new JSONObject(data2));
            //UIManagerScript.instance.TableToPokerUI(5);
        }
        yield return null;
    }

    //IEnumerator CurrentPlayerLeftCoroutine21111()
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    while (true)
    //    {
    //        if (GameManagerScript.instance.isHandEnd)
    //        {
    //            if (GameObject.Find(GameSerializeClassesCollection.instance.onCurrentPlayerLeft.playerName) != null)
    //            {
    //                GameObject currplayer = GameObject.Find(GameSerializeClassesCollection.instance.onCurrentPlayerLeft.playerName);
    //                currplayer.GetComponent<PokerPlayerController>().PlayerDeactivate();
    //                SpawnManager.instance.Despawn("PlayerPool", currplayer.transform);
    //            }
    //        }
    //    }
    //}

 

}
