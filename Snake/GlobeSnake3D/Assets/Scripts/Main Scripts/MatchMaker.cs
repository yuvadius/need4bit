using UnityEngine;
using System.Collections.Generic;
using Photon;

public class MatchMaker : PunBehaviour
{
    void Start()
    {
        if(PhotonNetwork.connectionState != ConnectionState.Connected)
            PhotonNetwork.ConnectUsingSettings("0.1");
    }

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString() + "/" + PhotonNetwork.GetPing().ToString());
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
        tempSnake.name = "Snake Syncer";
    }

    public override void OnCreatedRoom()
    {
        // PhotonNetwork.InstantiateSceneObject("Globe", new Vector3(), Quaternion.identity, 0, null);
    }

    void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom(null);
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer other)
    {
        Debug.Log("OnPhotonPlayerConnected() " + other.name); // not seen if you're the player connecting
        if (Trail.instance.segmentList.Count != 0)
        {
            Vector3[] positions = new Vector3[Trail.instance.segmentList.Count];
            Quaternion[] rotations = new Quaternion[Trail.instance.segmentList.Count];
            int counter = 0;
            foreach (SegmentScript segment in Trail.instance.segmentList)
            {
                positions[counter] = segment.transform.position;
                rotations[counter] = segment.transform.rotation;
                counter++;
            }
            SnakeSync.instance.CreateSegment(other, positions, rotations);
        }
    }
}