using UnityEngine;

public class GlobeSize : MonoBehaviour {

	public static GlobeSize instance;

	public MeshRenderer globeMesh;

	public System.Action<float> radiusChangedAction;

	float starterRadius = 1f; //this must the size of the globe in the scene metrics. by radius.
	[SerializeField]
	private float m_radiusMM = 1;
	public float radius {
		get {
			return m_radiusMM;
		}
		set {
			if(!Application.isPlaying || PhotonNetwork.isMasterClient) {
				m_radiusMM = value;
				innerRadiusChanged();
            }
		}
	}

	public float surface {
		get {
			return 4 * Mathf.PI * radius * radius;
		}
		set {
			radius = (Mathf.Sqrt(value / (4 * Mathf.PI)));
		}
	}

	private float _circumference;
	public float circumference {
		get {
			return _circumference;
		}
	}

	void Awake() {
		if(instance) {
			DestroyImmediate(gameObject);
			return;
		} else {
			instance = this;
			transform.localScale = Vector3.one;
			starterRadius = globeMesh.bounds.extents.y;
			DontDestroyOnLoad(gameObject);
		}
	}

	void Start() {
		float newScale = radius / starterRadius;
		transform.localScale = new Vector3(newScale, newScale, newScale);
		_circumference = 2 * Mathf.PI * radius;
	}

	void scale() {
		float newScale = radius / starterRadius;
		transform.localScale = new Vector3(newScale, newScale, newScale);
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if(stream.isWriting)
			stream.SendNext(radius);
		else {
			m_radiusMM = (float)stream.ReceiveNext();
			innerRadiusChanged();
        }
	}

	void innerRadiusChanged() {
		scale();
		_circumference = 2 * Mathf.PI * radius;
		if(radiusChangedAction != null) {
			radiusChangedAction(radius);
        }
	}
}
