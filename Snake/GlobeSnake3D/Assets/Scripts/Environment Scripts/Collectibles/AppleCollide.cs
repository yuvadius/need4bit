using UnityEngine;
using System.Collections;

public class AppleCollide : Photon.MonoBehaviour {

	Apple me;
	SphereCollider collider2;
	bool isStarted = false;

	void OnEnable(){
		if ( isStarted == false ){
			me = GetComponent<Apple>();
			collider2 = GetComponent<SphereCollider>();
			isStarted = true;
		}

		collider2.enabled = true;
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "head" && other.gameObject.GetComponent<HeadEat>().isRemote == false) {
			if(isStarted == false) {
				OnEnable();
			}
            if (me.isNetworkApple)
            {
                if (photonView.isMine)
                    PhotonNetwork.Destroy(gameObject);
                else if (!PhotonNetwork.isMasterClient)
                    photonView.RPC("NetworkDestroyApple", PhotonTargets.MasterClient, photonView.viewID);
            }
            else
            {
                me.destroy(other.transform);
                GameObject.FindObjectOfType<UIScore>().AddApple();
                collider2.enabled = false;
            }
        }
        else if (other.gameObject.GetComponent<HeadEat>().isRemote == false)
        {
			Debug.LogError("Not Head touched Apple");
		}
	}

    [PunRPC]
    void NetworkDestroyApple(int pvID)
    {
        Debug.Log(pvID);
        PhotonNetwork.Destroy(PhotonView.Find(pvID));
    }
}
