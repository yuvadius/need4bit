using UnityEngine;
using Photon;

public class Matchmaker : PunBehaviour
{
    public int numBarrels = 20;

    // Use this for initialization
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("Player", new Vector3(-10.78f, -10.78f), Quaternion.identity, 0);
    }

    public override void OnCreatedRoom()
    {
       PlaceBarrels.RandomBarrels(numBarrels);
    }

    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("Can't join random room!");
        PhotonNetwork.CreateRoom(null);
    }
}