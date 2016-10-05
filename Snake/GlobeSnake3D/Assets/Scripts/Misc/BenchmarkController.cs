using UnityEngine;
using System.Collections;


/// <summary>
/// The purpose of this controller is to conduct benchmarking tests.
/// </summary>
public class BenchmarkController : MonoBehaviour {

	public static BenchmarkController instance;

	[Tooltip("Should we activate the rotation benchmark test?")]
	public bool shouldRotateTest;
	public GameObject pathLandmark, extrapLandmark, emulationLandmark;

	[Tooltip("should we activate the forward benchmark test?")]
	public bool shouldForwardTest;
	public Material forwardTestGlobeMat;
	public int testConsistency = 30;
	public DistanceTest forwardTestLabel;
	public TimeLabel timeLabel;

	[Tooltip("Should we print the time and inspect that time is not offset between to clients?")]
	public bool shouldTimePrint;

	Transform myPivot, remotePivot;
	float medium = 0;



	void Awake() {
		instance = this;

		if( shouldForwardTest) {
			PlanetRotator planet = FindObjectOfType<PlanetRotator>();
			if( planet != null) {
				planet.shouldSpin = false;
				planet.globeRenderer.material = forwardTestGlobeMat;
            }
		}

		if(timeLabel != null) {
			timeLabel.gameObject.SetActive(shouldTimePrint);
		}
	}

	void Start() {
		if(shouldForwardTest) {
			myPivot = MainController.instance.snakeTilt.transform;
		}
		if(forwardTestLabel) {
			forwardTestLabel.gameObject.SetActive(shouldForwardTest);
		}
    }

	public void SettleSnakeHead(HeadEat snakeHead) {
		if(shouldForwardTest) {
			if(snakeHead.isRemote)
				remotePivot = snakeHead.transform;
            snakeHead.myCollider.enabled = false;
			snakeHead.snakeHead.SetActive(false);
			snakeHead.testerHead.SetActive(true);

			StartCoroutine(forwardTestCo());
		}
	}

	IEnumerator forwardTestCo() {		
        while(true) {
			if(remotePivot) {
				float distance = (myPivot.position - remotePivot.position).magnitude;
				medium = (medium * testConsistency + distance) / (testConsistency + 1);
				forwardTestLabel.label.text = "Off: " + medium.ToString();
			}
			yield return null;
		}
	}

	public void CreateLandmark(Quaternion rotation, int type) {
		GameObject newLandmark = Instantiate(getLandmark(type)) as GameObject;
		newLandmark.transform.SetParent(transform);
		newLandmark.transform.position = Vector3.zero;
		newLandmark.transform.rotation = rotation;
		newLandmark.SetActive(true);
	}

	GameObject getLandmark(int type) {
		if(type == 1)
			return pathLandmark;
		else if(type == 2)
			return extrapLandmark;
		else
			return emulationLandmark;
	}

}
