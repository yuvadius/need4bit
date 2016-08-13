using UnityEngine;
using System.Collections.Generic;
using Photon;

public class MatchMaker : PunBehaviour
{
    public static MatchMaker instance;
    private static GameObject snake = null;

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
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        CreatePlayer();
    }

    public static void CreatePlayer()
    {
        if (snake == null && PhotonNetwork.connectionStateDetailed == ClientState.Joined)
        {
            snake = PhotonNetwork.Instantiate("Remote Snake", new Vector3(), Quaternion.identity, 0);
            Destroy(snake.transform.GetChild(0).gameObject);
            snake.name = "Snake Syncer";
        }
    }

    public static void DestroyPlayer()
    {
        PhotonNetwork.Destroy(snake);
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
        if ( SnakeController.instance.trail.segmentList.Count != 0)
        {
            Vector3[] positions = new Vector3[SnakeController.instance.trail.segmentList.Count];
            Quaternion[] rotations = new Quaternion[SnakeController.instance.trail.segmentList.Count];
            int counter = 0;
            foreach (SegmentScript segment in SnakeController.instance.trail.segmentList)
            {
                positions[counter] = segment.transform.position;
                rotations[counter] = segment.transform.rotation;
                counter++;
            }
            SnakeSync.instance.CreateSegment(other, positions, rotations);
        }
    }
}