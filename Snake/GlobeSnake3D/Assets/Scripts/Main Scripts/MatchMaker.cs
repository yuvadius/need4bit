using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using Photon;

enum skins { Red, RedGreen, Blue, Yellow };

public class MatchMaker : PunBehaviour
{
    [HideInInspector]
    public SnakeSync mySync;
    public static MatchMaker instance;
    private static GameObject snake;
    private static bool isSnake = false;
    private static int playerNumber;
    private static string skin = null;
    public int maxScores;//Max amount of scores in scoreboard
    public int disconnectTimeout;//Amount of time before disconnect in milliseconds

	public int segmentsStart = 0;

	public string lobby = "America";

    void Awake()
    {
        //SetOfflineMode();
        if(instance)
            DestroyImmediate(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }

    /// <summary>
    /// Puts your game on Offline/SinglePlayer mode
    /// </summary>
    void SetOfflineMode()
    {
        PhotonNetwork.offlineMode = true;
        if(PhotonNetwork.room == null)
            PhotonNetwork.CreateRoom("Offline Mode");
    }

    void Start()
    {
        PhotonNetwork.networkingPeer.DisconnectTimeout = disconnectTimeout; // milliseconds
        if(PhotonNetwork.connectionState != ConnectionState.Connected)
            PhotonNetwork.ConnectUsingSettings("0.1");
    }

    void OnGUI()
    {
        //Make text responsive to screen size
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / 480.0f, Screen.height / 320.0f, 1));
        if (PhotonNetwork.offlineMode)
            GUILayout.Label("Offline Mode");
        else if (PhotonNetwork.connectionStateDetailed == ClientState.Joined)
            GUILayout.Label("Connected/" + PhotonNetwork.GetPing().ToString());
        else
            GUILayout.Label("Connecting");
        //Show top [maxScores] players in room(including urself)
        if (PhotonNetwork.connectionStateDetailed == ClientState.Joined && !PhotonNetwork.offlineMode)
        {
            KeyValuePair<KeyValuePair<string, int>, bool>[] scores = new KeyValuePair<KeyValuePair<string, int>, bool>[PhotonNetwork.playerList.Count()];
            int counter = 0;
            foreach (var player in PhotonNetwork.otherPlayers)
            {
                scores[counter] = new KeyValuePair<KeyValuePair<string, int>, bool>(new KeyValuePair<string, int>(player.name, player.GetScore()), false);
                counter++;
            }
            scores[counter] = new KeyValuePair<KeyValuePair<string, int>, bool>(new KeyValuePair<string, int>(PhotonNetwork.playerName, PhotonNetwork.player.GetScore()), true);
            var sortedDict = from entry in scores orderby entry.Key.Value descending select entry;//Sorts dict by descending score
            counter = 0;
            int index = 0;
            bool showedLocal = false;
            foreach (KeyValuePair<KeyValuePair<string, int>, bool> entry in sortedDict)
            {
                index++;
                if (counter < maxScores)
                {
                    if (showedLocal == true || counter != maxScores - 1 || (counter == maxScores - 1 && entry.Value))
                    {
                        counter++;
                        GUI.contentColor = Color.white;
                        if (entry.Value)
                        {
                            showedLocal = true;
                            GUI.contentColor = Color.yellow;
                        }
                        GUILayout.Label("#" + index + "    " + entry.Key.Key + ": " + entry.Key.Value);
                    }
                }
            }
        }
    }

    public override void OnDisconnectedFromPhoton()
    {
        MainController.instance.gameOver();
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinOrCreateRoom(lobby, null, null);
        SetPlayerProperties("Skin", -1);//Only here u can create new properties, strange i know
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.playerName = "Player" + PhotonNetwork.playerList.Count();
    }

    public static void SetPlayerProperties(string key, int value)
    {
        ExitGames.Client.Photon.Hashtable style = new ExitGames.Client.Photon.Hashtable();
        style.Add(key, value);
        PhotonNetwork.player.SetCustomProperties(style);
    }

    public static bool CreatePlayer()
    {
        if (!isSnake && PhotonNetwork.connectionStateDetailed == ClientState.Joined)
        {
            if (skin == null)
            {
                int skinNumber = GetSkin();
                skin = Enum.GetName(typeof(skins), skinNumber);
                SetPlayerProperties("Skin", skinNumber);
                PhotonNetwork.player.SetScore(0);
            }
            Debug.Log("Your skin is: " + skin);

            snake = PhotonNetwork.Instantiate("Remote Snake " + skin, new Vector3(), Quaternion.identity, 0);
            instance.mySync = snake.GetComponent<SnakeSync>();
            SnakeController.instance.skin.sharedMaterial = instance.mySync.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial;
            SnakeController.instance.trail.segment = instance.mySync.GetComponentInChildren<Trail>().segment;
            
			for(int i = 0; i<instance.segmentsStart; ++i) {
				SnakeController.instance.trail.AddSegment(true);
            }
			
			//Why are you destroying what you just instantiated? dont do that.
            foreach (Transform child in snake.transform)
                GameObject.Destroy(child.gameObject);
            snake.name = "Snake Syncer";
            isSnake = true;
            return true;
        }
        else if (PhotonNetwork.offlineMode)
            return true;
        else
            return false;
    }

    private static int GetSkin()
    {
        int[] colors = new int[Enum.GetValues(typeof(skins)).Length];
        foreach (var player in PhotonNetwork.otherPlayers)
            if ((int)player.customProperties["Skin"] != -1)//Makes sure if the player has a skin yet
                colors[(int)player.customProperties["Skin"]]++;
        return colors.ToList().IndexOf(colors.Min());
    } 

    public static void DestroyPlayer()
    {
        if (isSnake)
        {
            PhotonNetwork.Destroy(snake);
            isSnake = false;
            PhotonNetwork.player.SetScore(0);
        }
    }

    public override void OnCreatedRoom()
    {
        //PhotonNetwork.InstantiateSceneObject("Globe", new Vector3(), Quaternion.identity, 0, null);
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer other)
    {
        Debug.Log("OnPhotonPlayerConnected() " + other.name); // not seen if you're the player connecting
        if (isSnake)
        {
            mySync.syncTrail(other);
        }
    }
}