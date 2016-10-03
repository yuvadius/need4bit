using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TimeLabel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//GetComponent<Text>().text = DateTime.UtcNow.Second.ToString() + "." + DateTime.UtcNow.Millisecond.ToString();
		//GetComponent<Text>().text = PhotonNetwork.time.ToString();
		//PhotonNetwork.FetchServerTimestamp();
		GetComponent<Text>().text = "Time: " + PhotonNetwork.ServerTimestamp.ToString();
    }
}
