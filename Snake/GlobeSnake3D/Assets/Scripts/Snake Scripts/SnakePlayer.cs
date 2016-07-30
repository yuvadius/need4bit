using UnityEngine;
using System.Collections;

public class SnakePlayer : MonoBehaviour {

	public RemoteRecordingData data;
	public SnakeRemoteDevice snakeRemoteDevide;

	int frameCount = 0;

	void Update()
	{
		frameCount++;
		if (data.data.pointsToRecord.Count > frameCount)
		{
			print(data.data.pointsToRecord[frameCount]);
			snakeRemoteDevide.SetNewRemotePoint(data.data.pointsToRecord[frameCount]);
        }
	}
}
