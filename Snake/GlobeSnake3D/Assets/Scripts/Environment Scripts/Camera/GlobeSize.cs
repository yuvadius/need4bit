using UnityEngine;

public class GlobeSize : MonoBehaviour {

	public static GlobeSize instance;

	public MeshRenderer globeMesh;

    float starterRadius = 1f; //this must the size of the globe in the scene metrics. by radius.
	[HideInInspector]
	public float radius;

	public float surface {
		get {
			return 4 * Mathf.PI * radius * radius;
		}
		set {
			SetRadius(Mathf.Sqrt(value / (4 * Mathf.PI)));
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

    public void SetRadius(float radius){
        this.radius = radius;
        scale();
    }

    void scale() {
        float newScale = radius / starterRadius;
        transform.localScale= new Vector3(newScale, newScale, newScale);
#if UNITY_EDITOR
		if(Application.isPlaying)
#endif
		AppleController.instance.setHeight(radius);
	}

}
