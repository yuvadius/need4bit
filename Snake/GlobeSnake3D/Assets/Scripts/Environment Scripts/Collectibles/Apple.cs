using UnityEngine;
using System.Collections;

public class Apple : MonoBehaviour
{

	public Leaf[] leaves;
	

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
	

	void OnEnable()
	{
		if (started == false)
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

	public void setHeight(float height)
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











