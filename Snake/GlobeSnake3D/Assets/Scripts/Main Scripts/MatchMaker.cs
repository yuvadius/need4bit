using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using Photon;

enum skins { Red, Green, Blue, Yellow };

public class MatchMaker : PunBehaviour
{
    public SnakeSync mySync;
    public static MatchMaker instance;
    private static GameObject snake;
    private static bool isSnake = false;
    private static int playerNumber;
    private static string skin = null;
    private static int maxScores = 3;//max amount of scores in scoreboard

    void Awake()
    {
        if(instance)
            DestroyImmediate(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }

    void Start()
    {
        if(PhotonNetwork.connectionState != ConnectionState.Connected)
            PhotonNetwork.ConnectUsingSettings("0.1");
    }

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString() + "/" + PhotonNetwork.GetPing().ToString());
        //Show top 3 players in room
        if (PhotonNetwork.connectionStateDetailed == ClientState.Joined)
        {
            KeyValuePair<string, int>[] scores = new KeyValuePair<string, int>[PhotonNetwork.playerList.Count()];
            int counter = 0;
            foreach (var player in PhotonNetwork.playerList)
            {
                scores[counter] = new KeyValuePair<string, int>(player.name, player.GetScore());
                counter++;
            }
            var sortedDict = from entry in scores orderby entry.Value descending select entry;//Sorts dict by descending score
            counter = 0;
            foreach (KeyValuePair<string, int> entry in sortedDict)
            {
                counter++;
                GUILayout.Label("#" + counter + "    " + entry.Key + ": " + entry.Value);
                if (counter == maxScores)
                    break;
            }
        }
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinOrCreateRoom("Europe", null, null);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.playerName = "Player" + PhotonNetwork.playerList.Count();
    }

    public static bool CreatePlayer()
    {
        if (!isSnake && PhotonNetwork.connectionStateDetailed == ClientState.Joined)
        {
            if (skin == null)
            {
                ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
                int skinNumber = GetSkin();
                skin = Enum.GetName(typeof(skins), skinNumber);
                properties.Add("Skin", skinNumber);
                PhotonNetwork.player.SetCustomProperties(properties);
                PhotonNetwork.player.SetScore(0);
            }
            Debug.Log("Your skin is: " + skin);
            snake = PhotonNetwork.Instantiate("Remote Snake " + skin, new Vector3(), Quaternion.identity, 0);

			//Why are you destroying what you just instantiated? dont do that.
			foreach (Transform child in snake.transform)
                GameObject.Destroy(child.gameObject);

            snake.name = "Snake Syncer";
            instance.mySync = snake.GetComponent<SnakeSync>();
            isSnake = true;
            return true;
        }
        return false;
    }

    private static int GetSkin()
    {
        return 1;
        int[] colors = new int[Enum.GetValues(typeof(skins)).Length];
        foreach (var player in PhotonNetwork.otherPlayers)
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
        // PhotonNetwork.InstantiateSceneObject("Globe", new Vector3(), Quaternion.identity, 0, null);
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer other)
    {
        Debug.Log("OnPhotonPlayerConnected() " + other.name); // not seen if you're the player connecting
        if (SnakeController.instance.trail.segmentList.Count != 0 && isSnake)
        {
            mySync.syncTrail(other);
        }
    }
}