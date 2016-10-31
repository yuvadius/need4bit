using UnityEngine;
using System.Collections;

public class DontDie : MonoBehaviour {
	void Awake() {
		DontDestroyOnLoad(gameObject);
	}
}
