using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SnakeRecordingData
{
	public List<RemotePoint> pointsToRecord = new List<RemotePoint>();
}

public class RemoteRecordingData : MonoBehaviour
{
	public SnakeRecordingData data;
}
