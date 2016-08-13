using UnityEngine;
using System.Collections.Generic;
using Photon;

public class MatchMaker : PunBehaviour
{
    public static MatchMaker instance;
    private static GameObject snake;
    private static bool isSnake = false;
    private static int playerNumber;

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
        playerNumber = PhotonNetwork.playerList.Length;
        //switch (playerNumber % 4)
        //{
        //    case 0:
        //        break;
        //    case 1:
        //        SnakeSync.instance.SetPlayerColor(SnakeColor.RED);
        //        break;
        //    case 2:
        //        SnakeSync.instance.SetPlayerColor(SnakeColor.GREEN);
        //        break;
        //    case 3:
        //        SnakeSync.instance.SetPlayerColor(SnakeColor.YELLOW);
        //        break;
        //}
    }

    public static void CreatePlayer()
    {
        if (!isSnake && PhotonNetwork.connectionStateDetailed == ClientState.Joined)
        {
            playerNumber = PhotonNetwork.playerList.Length;
            switch (playerNumber % 4)
            {
                case 0:
                    snake = PhotonNetwork.Instantiate("Remote Snake Blue", new Vector3(), Quaternion.identity, 0);
                    break;
                case 1:
                    snake = PhotonNetwork.Instantiate("Remote Snake Red", new Vector3(), Quaternion.identity, 0);
                    break;
                case 2:
                    snake = PhotonNetwork.Instantiate("Remote Snake Yellow", new Vector3(), Quaternion.identity, 0);
                    break;
                case 3:
                    snake = PhotonNetwork.Instantiate("Remote Snake Green", new Vector3(), Quaternion.identity, 0);
                    break;
            }
            
            Destroy(snake.transform.GetChild(0).gameObject);
            snake.name = "Snake Syncer";
            isSnake = true;
        }
    }

    public static void DestroyPlayer()
    {
        if (isSnake)
        {
            PhotonNetwork.Destroy(snake);
            isSnake = false;
        }
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
        if (SnakeController.instance.trail.segmentList.Count != 0 && isSnake)
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