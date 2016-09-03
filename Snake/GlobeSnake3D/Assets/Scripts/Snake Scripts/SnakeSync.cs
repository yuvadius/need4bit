using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnakeSync : Photon.MonoBehaviour {
	//[PunRPC] 

	public Transform following;
	public GameObject pathPointGizmo;
	public bool isMine;

	void Awake() {
		isMine = photonView.isMine;
		if( isMine == true) {
			following = GameObject.FindGameObjectWithTag("GameController").GetComponent<SnakeController>().trail.transform;
		} else {
			pathPointGizmo = GameObject.FindGameObjectWithTag("GameController").GetComponent<MainController>().pathPointGizmo;
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if(stream.isWriting) {
			stream.SendNext(following.position);
		} else {
			Vector3 pathPoint = (Vector3)stream.ReceiveNext();
			GameObject newPoint = Instantiate(pathPointGizmo, pathPoint, Quaternion.identity) as GameObject;
			newPoint.SetActive(true);
		}
	}

}