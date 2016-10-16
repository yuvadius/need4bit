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

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "head" && other.gameObject.GetComponent<HeadEat>().isRemote == false)
        {
            if (isStarted == false)
            {
                OnEnable();
            }
            if (me.isNetworkApple)
            {
                if (photonView.isMine)
                {
                    GameObject.FindObjectOfType<UIScore>().AddApple();
                    NetworkDestroyApple(photonView.viewID);
                }
                else if (!PhotonNetwork.isMasterClient)
                {
                    GameObject.FindObjectOfType<UIScore>().AddApple();
                    photonView.RPC("NetworkDestroyApple", PhotonTargets.MasterClient, photonView.viewID);
                }
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
        PhotonNetwork.Destroy(PhotonView.Find(pvID));
    }
}
