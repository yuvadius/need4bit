using UnityEngine;

public class GlobeSize : MonoBehaviour {

	public static GlobeSize instance;

	public MeshRenderer globeMesh;

	float starterRadius = 1f; //this must the size of the globe in the scene metrics. by radius.
	private float _radius = 1;
	[HideInInspector]
	public float radius {
		get {
			return _radius;
		}
		set {
			if(PhotonNetwork.offlineMode || PhotonNetwork.isMasterClient) {
				_radius = value;
				scale();
				_circumference = 2 * Mathf.PI * radius;
			}
		}
	}

	public float surface {
		get {
			return 4 * Mathf.PI * radius * radius;
		}
		set {
			if(PhotonNetwork.offlineMode || PhotonNetwork.isMasterClient)
				SetRadius(Mathf.Sqrt(value / (4 * Mathf.PI)));
		}
	}

	private float _circumference;
	public float circumference {
		get {
			return _circumference;
		}
	}

	void Awake() {
		instance = this;
		transform.localScale = Vector3.one;
		starterRadius = globeMesh.bounds.extents.y;
	}

	void Start() {
		SetRadius(radius);
	}

	public void SetRadius(float radius) {
		this.radius = radius;
	}

	void scale() {
		float newScale = radius / starterRadius;
		transform.localScale = new Vector3(newScale, newScale, newScale);
#if UNITY_EDITOR
		if(Application.isPlaying)
#endif
			AppleController.instance.setHeight(radius);
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if(stream.isWriting)
			stream.SendNext(radius);
		else {
			_radius = (float)stream.ReceiveNext();
			scale();
			_circumference = 2 * Mathf.PI * radius;
		}
	}
}
