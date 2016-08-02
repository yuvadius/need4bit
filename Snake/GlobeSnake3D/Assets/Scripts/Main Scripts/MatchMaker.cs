using UnityEngine;
using Photon;

public class MatchMaker : PunBehaviour
{
    public GameObject snake;
    public Transform snakeTransformPrefab;

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
        GameObject tempSnake = PhotonNetwork.Instantiate("Remote Snake", new Vector3(), Quaternion.identity, 0);
        //CreatePlayer(tempSnake);
    }

    void CreatePlayer(GameObject tempSnake)
    {
        PhotonView photonView = tempSnake.GetComponent<PhotonView>();
        //Destroy(tempSnake);
        //photonView.RPC("SpawnOnNetwork", PhotonTargets.OthersBuffered, snakeTransformPrefab.position, snakeTransformPrefab.rotation, id1);

        PhotonView view = CopyComponent(tempSnake.GetComponent<PhotonView>(), snake);
        PhotonView[] nViews = snakeTransformPrefab.GetComponentsInChildren<PhotonView>();
        nViews[0].viewID = 1001;
        Destroy(tempSnake);
        /*Debug.Log(view.viewID.ToString());
        PhotonView[] nViews = snakeTransformPrefab.GetComponentsInChildren<PhotonView>();
        Debug.Log(nViews[0].viewID.ToString() + " yoyoyo");
        nViews[0].viewID = id1;
        Debug.Log(nViews[0].viewID.ToString() + " yoyoyo");*/
    }

    [PunRPC]
    void SpawnOnNetwork(Vector3 pos, Quaternion rot, int id1)
    {
        Debug.LogError("yay");
        Transform newPlayer = Instantiate(snakeTransformPrefab, pos, rot) as Transform;
        // Set player's PhotonView
        PhotonView[] nViews = newPlayer.GetComponentsInChildren<PhotonView>();
        nViews[0].viewID = id1;
        //PhotonView nView = newPlayer.GetComponent<PhotonView>();
        //nView.viewID = id1;
        //Debug.Log(nView.viewID.ToString());
    }

    T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            Debug.Log(field.ToString() + ": " + field.GetValue(original));
            /*if (field.ToString().Contains("ownerId") || (int)field.GetValue(original) == 0)
            {
                Debug.Log("yo");
                field.SetValue(copy, 1001);
            }
            else*/
                field.SetValue(copy, field.GetValue(original));
        }
        return copy as T;
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