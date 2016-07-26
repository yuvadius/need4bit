using UnityEngine;
using System.Collections;

public class Apple : MonoBehaviour
{

	public Leaf[] leaves;
	public SphereCollider spherCollider;

	bool isBeforeSet = true;
	Vector3 randomVector;
	bool started = false;
	AppleController controller;



	public void OnDisable()
	{
		started = false;
		for (int i = 0; i < leaves.Length; ++i)
		{
			leaves[i].AttachBack();
			leaves[i].gameObject.SetActive(false);	
		}
    }

    /// <summary>
    /// Set a random vector value to the property "randomVector"
    /// </summary>
    private void SetRandomVector()
    {
        //pick a random normalized vector;
        randomVector = new Vector3(
            Random.Range(-10, 10),
            Random.Range(-10, 10),
            Random.Range(-10, 10)
            );
        randomVector = randomVector.normalized;
        transform.position = Vector3.zero;
        transform.LookAt(randomVector * 100);
    }
    
	//TODO: turn recursion into a loop
    private void AvoidOverlap(float height, int retry = 10)
    {
		LayerMask mask = LayerMask.GetMask("apples", "head", "segments");
        for (int i = 0; i < retry; ++i)
		{
			
			if( !Physics.CheckSphere(randomVector * height, spherCollider.radius, mask))
			{
				return; //found a place where we dont collide with anything
			}

			SetRandomVector();
		}

		Debug.LogError("something went wrong, we did not find space for our apple to spawn in");
	}	

	void OnEnable()
	{
		if (started == false)
		{
            SetRandomVector();
			started = true;
		}

		int leafAmount = Random.Range(0, (int)Mathf.Pow(2, leaves.Length));
		for (int i = 0; i < leaves.Length; ++i)
		{
			leaves[i].gameObject.SetActive((leafAmount & (int)Mathf.Pow(2, i)) != 0);
		}
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
            AvoidOverlap(height);

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











