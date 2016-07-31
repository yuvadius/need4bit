using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct Vector3Serializer
{
	public float x;
	public float y;
	public float z;

	public Vector3Serializer(Vector3 v3)
	{
		x = v3.x;
		y = v3.y;
		z = v3.z;
	}

	public Vector3 V3
	{ get { return new Vector3(x, y, z); } }

	public static implicit operator Vector3 (Vector3Serializer vec)
	{
		return vec.V3;
	}

	public static implicit operator Vector3Serializer(Vector3 vec)
	{
		return new Vector3Serializer(vec);
	}
}

[System.Serializable]
public struct QuaternionSerializer
{
	public float x;
	public float y;
	public float z;
	public float w;

	public QuaternionSerializer(Quaternion q)
	{
		x = q.x;
		y = q.y;
		z = q.z;
		w = q.w;
	}

	public Quaternion Q
	{ get { return new Quaternion(x, y, z, w); } }

	public static implicit operator Quaternion(QuaternionSerializer qua)
	{
		return qua.Q;
	}

	public static implicit operator QuaternionSerializer(Quaternion qua)
	{
		return new QuaternionSerializer(qua);
	}
}

/// <summary>
/// This is a representation of a single point in time of a remote snake, with index representing which frame it is.
/// </summary>
[System.Serializable]
public struct RemotePoint
{
	public Vector3Serializer position;
	public QuaternionSerializer rotation;
	public int index;

	public RemotePoint(Vector3 p, Quaternion r, int i)
	{
		this.position = p;
		this.rotation = r;
		this.index = i;
	}

	public override string ToString()
	{
		return position.V3.ToString() + " " + rotation.Q.ToString() + " " + index.ToString();
	}
}

/// <summary>
/// This is the main script to control a remote snake. It will be responsible to visually move the snake
/// on its trail as it receives the trail points from somewhere.
/// </summary>
public class SnakeRemoteDevice : MonoBehaviour {

	public GameObject originalRemoteSnake;

	List<RemotePoint> unprocessedPoints = new List<RemotePoint>(); //means that the remote snake still does not know that these points are in his trail
	List<RemotePoint> snakeTrail = new List<RemotePoint>(); //this are already processed remote points

	Transform remoteSnake;

	void Awake()
	{
		GameObject newSnake = Instantiate(originalRemoteSnake);
		newSnake.SetActive(true);
		remoteSnake = newSnake.transform;
	}

	void Update()
	{
		for(int i=0; i<unprocessedPoints.Count; ++i)
		{
			if( i == unprocessedPoints.Count - 1) //the last unprocessed point
			{
				print("Setting Point " + unprocessedPoints[i]);
				SetSnakeToPoint(unprocessedPoints[i]);
			}
			snakeTrail.Add(unprocessedPoints[i]);
		}
		unprocessedPoints.Clear();
	}

	public void SetNewRemotePoint(RemotePoint point)
	{
		unprocessedPoints.Add(point);
	}

	void SetSnakeToPoint(RemotePoint point)
	{
		remoteSnake.position = point.position;
		remoteSnake.rotation = point.rotation;
	}

}
