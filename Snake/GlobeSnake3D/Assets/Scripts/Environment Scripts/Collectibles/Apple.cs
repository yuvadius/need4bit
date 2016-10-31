using UnityEngine;
using System.Collections;

public class Apple : MonoBehaviour
{

	public Leaf[] leaves;
	public SphereCollider spherCollider;

    public bool isNetworkApple;
	public CollectibleController controller;


    void Awake()
    {
        controller = GameObject.Find("Apple System").GetComponent<CollectibleController>();
        transform.parent = controller.transform;
        controller.appleList.Add(this);

		int leafAmount = Random.Range(0, (int)Mathf.Pow(2, leaves.Length));
		for(int i = 0; i < leaves.Length; ++i) {
			leaves[i].gameObject.SetActive((leafAmount & (int)Mathf.Pow(2, i)) != 0);
		}
	}

	void Start() {
		setHeight(GlobeSize.instance.radius);
	}

    void OnDestroy()
    {
        controller.destroy(this);
	}

	public void OnDisable()
	{
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

    public void setController(CollectibleController controller)
	{
		this.controller = controller;
	}

    /// <summary>
    /// Sets the apples position on the globe
    /// </summary>
    /// <param name="height">The globes radius</param>
	public void setHeight(float height)
	{
        transform.position = transform.position.normalized * height;
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











