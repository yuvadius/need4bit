using UnityEngine;
using Photon;

public class MatchMaker : PunBehaviour
{
    public Transform subSnake;//This will be the parent of the local remote snake

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
        CreatePlayer();
    }

    void CreatePlayer()
    {
        GameObject tempSnake = PhotonNetwork.Instantiate("Remote Snake", new Vector3(), Quaternion.identity, 0);
        Destroy(tempSnake.transform.GetChild(0).gameObject);
        tempSnake.transform.SetParent(subSnake);
        tempSnake.name = "Snake Syncer";
    }

    public override void OnCreatedRoom()
    {
        // PhotonNetwork.InstantiateSceneObject("Globe", new Vector3(), Quaternion.identity, 0, null);
    }

    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("Can't join random room!");
        PhotonNetwork.CreateRoom(null);
    }
}