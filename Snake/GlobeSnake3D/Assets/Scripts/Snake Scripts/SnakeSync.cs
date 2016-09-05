using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class CustomPayload {

	public static byte code = 0x33;

	public Vector3Serializer pos;
	public QuaternionSerializer quat;
	public float degsPerSecond;
	public double time;

	public CustomPayload(Vector3 pos, Quaternion quat, float degsPerSecond) {
		this.pos = pos;
		this.quat = quat;
		this.degsPerSecond = degsPerSecond;
		time = PhotonNetwork.time;
	}

	public static object Deserialize(byte[] arrBytes) {
		using(var memStream = new MemoryStream()) {
			var binForm = new BinaryFormatter();
			memStream.Write(arrBytes, 0, arrBytes.Length);
			memStream.Seek(0, SeekOrigin.Begin);
			var obj = binForm.Deserialize(memStream);
			return obj;
		}
	}

	public static byte[] Serialize(object customType) {
		BinaryFormatter bf = new BinaryFormatter();
		using(var ms = new MemoryStream()) {
			bf.Serialize(ms, customType);
			return ms.ToArray();
		}
	}
}

public class SnakeSync : Photon.MonoBehaviour {
	//[PunRPC] 

	public Transform following;
	public GameObject pathPointGizmo;
	public Extrapolator extrapolator;
	public GameObject rotationDeviceEmulator;
	public bool isMine;

	RotationDeviceEmulator emulator;

	void Awake() {
		isMine = photonView.isMine;
		if(isMine == true) {
			following = GameObject.FindGameObjectWithTag("GameController").GetComponent<SnakeController>().trail.transform;
		} else {
			pathPointGizmo = GameObject.FindGameObjectWithTag("GameController").GetComponent<MainController>().pathPointGizmo;
			emulator = (Instantiate(rotationDeviceEmulator, transform) as GameObject).GetComponent< RotationDeviceEmulator>();
		}

		extrapolator = FindObjectOfType<Extrapolator>();
    }

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if(stream.isWriting) {
			stream.SendNext(new CustomPayload(following.position, extrapolator.followingCenter.rotation, extrapolator.followingForwardRotator.degsPerSec));
		} else {
			CustomPayload payload = (CustomPayload)stream.ReceiveNext();
			//GameObject newPoint = Instantiate(pathPointGizmo, payload.pos, Quaternion.identity) as GameObject;
			//newPoint.SetActive(true);
			Vector3 extrapPoint, emulationPoint;
			Quaternion quat = extrapolator.ExtrapolateFrom(emulator.emulationOffset, payload, out extrapPoint, out emulationPoint);
			emulator.SetNextRot(quat, extrapPoint, emulationPoint);
		}
	}

}