using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 
/// made as singleton
/// 
/// It is responsible for holding the seeSkeleton bool which shows the skeleton of the rotation device.
/// 
/// It is responsible for making the cursur dissapear.
/// 
/// It is responsible for checking for exit input
/// 
/// It is responsible for finishing the level.
/// </summary>
public class MainController : MonoBehaviour {
	public static MainController instance;

    public CanvasGroup gameCanvasGroup, endCanvasGroup;
    public Text applesAteText, applesRecordText;
    public Text gameOverHeader;
    public GameObject[] turnOffOnOver;
    public float gameOverTime = 5;
    public float endCanvasStrength = 0.7f;
    public string recordKey = "apple_record";
    public bool CR_running = false;

    public TiltSide snakeTilt;
    public LevelingSystem levelSystem;
    public Animator snakeAnim;
   
	void Awake(){
		if( instance == null ){
			instance = this;
		}
        Cursor.visible = false;

	}

    void Start() {
        gameCanvasGroup.alpha = 1;
        gameCanvasGroup.blocksRaycasts = gameCanvasGroup.interactable = true;
        endCanvasGroup.alpha = 0;
        endCanvasGroup.blocksRaycasts = endCanvasGroup.interactable = false;
    }

	public void gameOver(){
        if (!CR_running)
        {
            MatchMaker.DestroyPlayer();
            StartCoroutine(makeGameStop());
        }
	}

    public void Restart() {
        if (PhotonNetwork.connectionStateDetailed == ClientState.Joined)
            SceneManager.LoadScene("main");
        else
            print("Can't restart if disconnected");
    }

    [ContextMenu("Clean Record")]
    public void CleanRecord() {
        PlayerPrefs.DeleteKey(recordKey);
    }

    public void ExitGame() {
        Application.Quit();
    }

	IEnumerator makeGameStop(){
        CR_running = true;
        snakeTilt.DieTilt();
        snakeAnim.SetTrigger("Die");
        SnakeController.instance.stop(gameOverTime);
        applesAteText.text = applesAteText.text + levelSystem.applesAte.ToString();

        if (PlayerPrefs.HasKey(recordKey)) {
            int recored = PlayerPrefs.GetInt(recordKey);
            if (recored >= levelSystem.applesAte) {
				gameOverHeader.text = Translator.GetTranslation("): Проиграли :(");
                applesRecordText.text = applesRecordText.text + recored.ToString();
            } else {
				gameOverHeader.text = Translator.GetTranslation("(: Новый Рекорд!!! :)");
                applesRecordText.text = applesRecordText.text + levelSystem.applesAte.ToString();
                PlayerPrefs.SetInt(recordKey, levelSystem.applesAte);
                PlayerPrefs.Save();
            }
        } else {
			gameOverHeader.text = Translator.GetTranslation("(: Новый Рекорд!!! :)");
            applesRecordText.text = applesRecordText.text + levelSystem.applesAte.ToString();
            PlayerPrefs.SetInt(recordKey, levelSystem.applesAte);
        }

        float timePassed = 0;
        gameCanvasGroup.blocksRaycasts = false;
        gameCanvasGroup.interactable = false;
        while (timePassed < gameOverTime) {

            float ratio = timePassed / gameOverTime;

            gameCanvasGroup.alpha = 1 - ratio;
            endCanvasGroup.alpha = endCanvasStrength * ratio;

            timePassed += Time.deltaTime;
            yield return null;
        }
        gameCanvasGroup.alpha = 0;
        for (int i=0; i<turnOffOnOver.Length; ++i) {
            turnOffOnOver[i].SetActive(false);
        }
            endCanvasGroup.alpha = endCanvasStrength;
        endCanvasGroup.blocksRaycasts = true;
        endCanvasGroup.interactable = true;
	}
}
