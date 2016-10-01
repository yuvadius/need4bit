using UnityEngine;
using System.Collections;

public class GameStarter : MonoBehaviour {

    public MeshRenderer globeRenderer;
    public WelcomeTextFader welcomeFader;
    public TipTextFader tipFader;
    public float startTime = 5f;

    bool hasStarted = false;
    CameraController cameraController;
    RotateForward snakeStarter;
    LerpToCameraPoint cameraLerper;

	Material globeMat;

	void Start() {
		//This is so that git will not constantly want to commit the material of the globe
#if UNITY_EDITOR
		globeMat = globeRenderer.material;
#else
		globeMat = globeRenderer.sharedMaterial;
#endif


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
            snakeStarter.ManualStart();
            StartCoroutine(setCameraLerpHigh());
            tipFader.StartTipping();
        }
    }

    IEnumerator rotateGlobe() {

		while(hasStarted == false) {
			globeMat.SetFloat("_ManualTime", Time.time);
            yield return null;
        }

        float currentTime = Time.time;

        float timePassed = 0;

        while (timePassed < startTime) {
            float ratio = timePassed / startTime;
            currentTime += (1-ratio) * Time.deltaTime;
			globeMat.SetFloat("_ManualTime", currentTime);
            timePassed += Time.deltaTime;
            yield return null;
        }

    }

    IEnumerator setCameraLerpHigh() {

        float timePassed = 0;
        while (timePassed < startTime) {
            float ratio = timePassed / startTime;
            cameraController.lerpSpeed = Mathf.Lerp(0, 2f, ratio);
			globeMat.SetFloat("_ManualTime", 1 - ratio);
            timePassed += Time.deltaTime;
            yield return null;
        }

        timePassed = 0;
        while (timePassed < startTime) {
            float ratio = timePassed / startTime;
            cameraController.lerpSpeed = Mathf.Lerp(2f, 30f, ratio);
            cameraLerper.lookAtLerpCoefficient = Mathf.Lerp(2f, 30f, ratio);
            timePassed += Time.deltaTime;
            yield return null;
        }
        
        cameraController.lerpSpeed = 30;
        Destroy(gameObject);
    }
}
