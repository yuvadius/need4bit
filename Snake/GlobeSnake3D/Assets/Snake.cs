using UnityEngine;
using System.Collections;

public class Snake : MonoBehaviour {
    public static Transform playerPrefab; //set this in the inspector

    public static void CreatePlayer()
    {
        // You must be in a Room already
        // Marco Polo tutorial shows how to connect and join room
        // See: http://doc.photonengine.com/en/pun/current/tutorials/tutorial-marco-polo

        // Manually allocate PhotonViewID
        int id1 = PhotonNetwork.AllocateViewID();

        PhotonView photonView = playerPrefab.GetComponent<PhotonView>();
        photonView.RPC("SpawnOnNetwork", PhotonTargets.AllBuffered, playerPrefab.position, playerPrefab.rotation, id1);
    }

    [PunRPC]
    static void SpawnOnNetwork(Vector3 pos, Quaternion rot, int id1)
    {
        Transform newPlayer = Instantiate(playerPrefab, pos, rot) as Transform;
        // Set player's PhotonView
        PhotonView[] nViews = newPlayer.GetComponentsInChildren<PhotonView>();
        nViews[0].viewID = id1;
    }
}
