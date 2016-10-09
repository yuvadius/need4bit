using UnityEngine;
using System.Collections;

public class Apple : MonoBehaviour
{

	public Leaf[] leaves;
	public SphereCollider spherCollider;

	bool isBeforeSet = true;
    public bool isNetworkApple;
	Vector3 randomVector;
	bool started = false;
	public AppleController controller;


    void Awake()
    {
        controller = GameObject.Find("Apple System").GetComponent<AppleController>();
        transform.parent = controller.transform;
    }


	void OnEnable() {
		if(started == false && !isNetworkApple) {
			SetRandomVector();
			started = true;
		}

		int leafAmount = Random.Range(0, (int)Mathf.Pow(2, leaves.Length));
		for(int i = 0; i < leaves.Length; ++i) {
			leaves[i].gameObject.SetActive((leafAmount & (int)Mathf.Pow(2, i)) != 0);
		}
	}

	void Start() {
		if( started == false) {
			randomVector = transform.position.normalized;
			started = true;
		}
	}


	public void OnDisable()
	{
		started = false;
		for (int i = 0; i < leaves.Length; ++i)
		{
			leaves[i].AttachBack();
			leaves[i].gameObject.SetActive(false);	
		}
    }

    private static Vector3 GetRandomNormalVector()
    {
        Vector3 randomVector = new Vector3(
            Random.Range(-10, 10),
            Random.Range(-10, 10),
            Random.Range(-10, 10)
            );
        return randomVector.normalized;
    }

    /// <summary>
    /// Set a random vector value to the property "randomVector"
    /// </summary>
    private void SetRandomVector()
    {
        //pick a random normalized vector;
        randomVector = GetRandomNormalVector();
        transform.position = Vector3.zero;
        transform.LookAt(randomVector * 100);
    }
    
    public static Vector3 AvoidOverlap(float height, float appleRadius, int retry = 10)
    {
		LayerMask mask = LayerMask.GetMask("apples", "head", "segments");
        Vector3 random = GetRandomNormalVector();
        for (int i = 0; i < retry; ++i)
		{
            if (!Physics.CheckSphere(random * height, appleRadius, mask))
			{
				return random; //found a place where we dont collide with anything
			}
            random = GetRandomNormalVector();
		}
		Debug.LogError("something went wrong, we did not find space for our apple to spawn in");
        return random;
	}

	public void setController(AppleController controller)
	{
		this.controller = controller;
	}

    /// <summary>
    /// Sets the apples position on the globe
    /// </summary>
    /// <param name="height">The globes radius</param>
    /// <param name="radiusChanged">If the globe got bigger</param>
	public void setHeight(float height, bool radiusChanged = false)
	{
		if (isBeforeSet == true)
		{
			isBeforeSet = false;
			gameObject.SetActive(true);
		}
		if (started == false)
		{
			OnEnable();
		}
        if (!radiusChanged)
            randomVector = AvoidOverlap(height, spherCollider.radius);

        transform.position = randomVector * height;
	}

	public void destroy(Transform mouth)
	{
		controller.destroy(this);
		StartCoroutine(getEaten(0.4f, mouth));
	}

	IEnumerator getEaten(float seconds, Transform mouth)
	{
		float time = 0;
		Vector3 pos = transform.position;
		Vector3 pos2 = transform.position - (mouth.position - transform.position) / 2f;

		for (int i = 0; i < leaves.Length; ++i)
		{
			if (leaves[i].gameObject.activeSelf)
			{
				leaves[i].DropOff();
			}
		}

		while (seconds > time)
		{
			time += Time.deltaTime;
			transform.position = Vector3.Lerp(pos, pos2, time / seconds);
			yield return null;
		}
		pos = transform.position;
		time = 0;

		while (seconds > time * 2)
		{
			time += Time.deltaTime;
			transform.localScale = Vector3.one * (1 - (time * 2 / seconds));
			transform.position = Vector3.Lerp(pos, mouth.position, time * 2 / seconds);
			yield return null;
		}

		controller.addAppleToPool(this);
	}
}











