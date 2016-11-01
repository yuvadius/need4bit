using UnityEngine;
using System.Collections;

public class GameStarter : MonoBehaviour {

    public WelcomeTextFader welcomeFader;
    public TipTextFader tipFader;
    public float planetRotationTime = 5f;
	public float waitTime = 0.3f;
	public float slowTime = 1f;
	public float cameraStartTime = 3f;

    bool hasStarted = false;
    CameraController cameraController;
    RotateForward snakeStarter;
    LerpToCameraPoint cameraLerper;


	void Start() {
		cameraController = FindObjectOfType<CameraController>();
        snakeStarter = FindObjectOfType<RotateForward>();
        cameraLerper = FindObjectOfType<LerpToCameraPoint>();
        StartCoroutine(rotateGlobe());
	}

	void Update () {
        if (hasStarted == false) {
            if (Input.GetMouseButtonDown(0)) {
                startGame();
            }
        }
	}

    void startGame() {
		if (MatchMaker.CreatePlayer())
        {
			hasStarted = true;
			FindObjectOfType<ScriptOrderController>().StartGame();
            welcomeFader.StopFading();
            snakeStarter.ManualStart(waitTime, slowTime);
            StartCoroutine(setCameraLerpHigh());
            tipFader.StartTipping();
        }
    }

    IEnumerator rotateGlobe() {

		while(hasStarted == false) {
			PlanetRotator.instance.SetTime(Time.time);
            yield return null;
        }

        float currentTime = Time.time;

        float timePassed = 0;

        while (timePassed < planetRotationTime) {
            float ratio = timePassed / planetRotationTime;
            currentTime += (1-ratio) * Time.deltaTime;
			PlanetRotator.instance.SetTime(currentTime);
            timePassed += Time.deltaTime;
            yield return null;
        }

    }

    IEnumerator setCameraLerpHigh() {
        float timePassed = 0;

        while (timePassed < cameraStartTime) {
            float ratio = timePassed / cameraStartTime;
            cameraController.lerpSpeed = Mathf.Lerp(2f, 30f, ratio);
            cameraLerper.lookAtLerpCoefficient = Mathf.Lerp(2f, 30f, ratio);
            timePassed += Time.deltaTime;
            yield return null;
        }
        
        cameraController.lerpSpeed = 30;
        Destroy(gameObject);
    }
}
