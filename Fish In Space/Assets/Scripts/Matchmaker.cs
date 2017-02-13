using UnityEngine;
using Photon;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Matchmaker : PunBehaviour
{
    public static bool startedGame;//Is the game in process
    public string lobby = "Europe";

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