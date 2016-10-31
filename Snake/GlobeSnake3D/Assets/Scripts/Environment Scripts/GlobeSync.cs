using UnityEngine;
using System.Collections;
using Photon;

public class GlobeSync : PunBehaviour {

	public void SetNewSize(float destination) {
		if(PhotonNetwork.isMasterClient) {
			photonView.RPC("SyncNewSize", PhotonTargets.Others, destination);
		}
	}

	public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer) {
		if(PhotonNetwork.isMasterClient) {
			photonView.RPC("InitializeGlobe", newPlayer, GlobeSize.instance.surface, GlobeSize.instance.destinationSurface);
        }
	}

	[PunRPC]
	public void InitializeGlobe(float surface, float destinationSurface) {
		GlobeSize.instance.SyncGlobe(surface, destinationSurface);
	}

	[PunRPC]
	public void SyncNewSize(float destinationSurface) {
		GlobeSize.instance.DestinationChanged(destinationSurface);
	}

}
