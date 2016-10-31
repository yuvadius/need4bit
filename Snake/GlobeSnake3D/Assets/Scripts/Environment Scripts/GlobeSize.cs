using System.Collections;
using UnityEngine;

public class GlobeSize : MonoBehaviour {

	public static GlobeSize instance;

	public GameObject globe;
	public MeshRenderer globeMesh;

	public float minSurface, maxSurface;
	public float increaseSpeed = 1;
	public System.Action<float> radiusChangedAction;

	float starterRadius = 1f; //this must the size of the globe in the scene metrics. by radius.

	[SerializeField, HideInInspector]
	float _destinationRadius;
	public float destinationRadius {
		get {
			return _destinationRadius;
		}
		private set {
			_destinationRadius = value;
			if(!Application.isPlaying) {
				radius = value;
			}
		}
	}

	[SerializeField, HideInInspector]
	float _radius;
	public float radius {
		get {
			return _radius;
		}
		private set {
			_radius = value;
			scale();
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
			destinationRadius = (Mathf.Sqrt(value / (4 * Mathf.PI)));
			if(!Application.isPlaying) {
				radius = destinationRadius;
			} else if(PhotonNetwork.isMasterClient) {
				FindObjectOfType<GlobeSync>().SetNewSize(value);
			}
		}
	}

	void Awake() {
		if(instance) {
			DestroyImmediate(gameObject);
			return;
		}

		instance = this;
		_destinationRadius = _radius = globe.transform.localScale.x;
		globe.transform.localScale = Vector3.one;
		starterRadius = globeMesh.bounds.extents.y;
		scale();
	}

	void FixedUpdate() {
		if(surface < destinationSurface) {
			surface = Mathf.Clamp(surface + Time.deltaTime * increaseSpeed, surface, destinationSurface);
		} else if(radius > destinationRadius) {
			surface = Mathf.Clamp(surface - Time.deltaTime * increaseSpeed, destinationSurface, surface);
		}
	}

	public void SyncGlobe(float surface, float destinationSurface) {
		this.surface = surface;
		this.destinationSurface = destinationSurface;
    }

	public void DestinationChanged(float destination) {
		destinationSurface = destination;
	}

	void scale() {
		float newScale = radius / starterRadius;
		globe.transform.localScale = new Vector3(newScale, newScale, newScale);
		if(radiusChangedAction != null) {
			radiusChangedAction(radius);
		}
	}

}
