using System.Collections;
using UnityEngine;

public class GlobeSize : MonoBehaviour {

	public static GlobeSize instance;

	public MeshRenderer globeMesh;

	public float increaseSpeed = 1;

	public System.Action<float> radiusChangedAction;

	float starterRadius = 1f; //this must the size of the globe in the scene metrics. by radius.

	[SerializeField]
	[HideInInspector]
	private float _destinationRadius;
	public float destinationRadius {
		get {
			return _destinationRadius;
		}
		set {
			_destinationRadius = value;
			if(!Application.isPlaying) {
				radius = value;
			}
		}
	}

	[SerializeField]
	[HideInInspector]
	float _radius;
	public float radius {
		get {
			return _radius;
		}
		set {
			_radius = value;
			innerRadiusChanged();
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

	public float destinationSurface {
		get {
			return 4 * Mathf.PI * destinationRadius * destinationRadius;
		}
		set {
			destinationRadius = (Mathf.Sqrt(value / (4*Mathf.PI)));
			if(!Application.isPlaying) {
				radius = destinationRadius;
			}
        }
	}

	void Awake() {
		if(instance) {
			DestroyImmediate(gameObject);
			return;
		} else {
			instance = this;
			_destinationRadius = _radius = transform.localScale.x;
            transform.localScale = Vector3.one;
			starterRadius = globeMesh.bounds.extents.y;
			DontDestroyOnLoad(gameObject);
			scale();
			StartCoroutine(scaleSmoothlyCo());
        }
	}

	void scale() {
		float newScale = radius / starterRadius;
		transform.localScale = new Vector3(newScale, newScale, newScale);
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if(stream.isWriting) {
			stream.SendNext(destinationSurface);
		} else {
			destinationSurface = (float)stream.ReceiveNext();
			innerRadiusChanged();
		}
	}

	void innerRadiusChanged() {
		scale();
		if(radiusChangedAction != null) {
			radiusChangedAction(radius);
        }
	}

	//Shouldn't this somehow be in fixed update?
	IEnumerator scaleSmoothlyCo() {
		while(true) {
			if( surface < destinationSurface) {

				surface = Mathf.Clamp(surface + Time.deltaTime * increaseSpeed, surface, destinationSurface);

			}else if( radius > destinationRadius) {

				surface = Mathf.Clamp(surface - Time.deltaTime * increaseSpeed, destinationSurface, surface);

			}
			yield return null;
		}
	}
}
