using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;



/// <summary>
/// This will create a recording structure for a given snake
/// </summary>
public class SnakeRecorder : MonoBehaviour {

	public RemoteRecordingData recording;

	public Transform recordingPivot;

	int frameCount = 0;

	void Update()
	{
		frameCount++;
		recording.data.pointsToRecord.Add(new RemotePoint(recordingPivot.position, recordingPivot.rotation, frameCount));
	}

	[ContextMenu("Record")]
	public void Record()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream fs = File.Create(Application.persistentDataPath + "/dataRecord.dat");

		bf.Serialize(fs, recording.data);
		fs.Close();
		fs.Dispose();

		Debug.Log("Path to file is: " + Application.persistentDataPath);
	}

}
