using UnityEngine;
using Photon;
using System;
using System.Linq;
using System.Collections;

public class Matchmaker : PunBehaviour
{
    public string lobby = "Europe";
    public int maxScoreboardCount;//Max players to show on scoreboard

    void Awake()
    {
        SetOfflineMode();
    }

    /// <summary>
    /// Puts your game on Offline/SinglePlayer mode
    /// </summary>
    void SetOfflineMode()
    {
        PhotonNetwork.offlineMode = true;
        if (PhotonNetwork.room == null)
            PhotonNetwork.CreateRoom("Offline Mode");
    }

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString() + "/" + PhotonNetwork.GetPing().ToString());
        GUI.contentColor = Color.yellow;
        GUILayout.Label("Score: " + PhotonNetwork.player.GetScore());
        PhotonPlayer[] playerList = PhotonNetwork.playerList;
        //Sort array by descending order of score
        Array.Sort(playerList, delegate (PhotonPlayer x, PhotonPlayer y) { return y.GetScore().CompareTo(x.GetScore()); });
        for (int i = 0; i < playerList.Length && i < maxScoreboardCount; i++)
            GUILayout.Label("#" + (i + 1) + "    " + playerList[i].NickName + ": " + playerList[i].GetScore());
    }

    void Start()
    {
        if (PhotonNetwork.connectionState != ConnectionState.Connected)
            PhotonNetwork.ConnectUsingSettings("0.1");
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinOrCreateRoom(lobby, null, null);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.playerName = "Player" + PhotonNetwork.playerList.Count();
    }

    public static void CreatePlayer()
    {
        GameObject player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0);
        GameObject.Find("Main Camera").GetComponent<CameraController>().player = player;
        //GameObject.Find("Main Camera").transform.parent = player.transform;
    }

    /// <summary>
    /// Converts volume size to score
    /// </summary>
    /// <param name="volume">The volume that you need to convert</param>
    /// <returns>The score that the volume represents</returns>
    public static int VolumeToScore(float volume)
    {
        return (int)(volume * 100);
    }

    /// <summary>
    /// Converts score to volume size
    /// </summary>
    /// <param name="score">The score that you need to convert</param>
    /// <returns>The volume that the score represents</returns>
    public static float ScoreToVolume(int score)
    {
        return score / 100f;
    }
}