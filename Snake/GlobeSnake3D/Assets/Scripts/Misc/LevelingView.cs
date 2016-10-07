using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelingView : MonoBehaviour {

    public GameObject container;
    public Text freePoints;

    LevelingSystem system;
    SnakeController snake;

    bool isPointAvailable = false;

    void Start() {
        system = FindObjectOfType<LevelingSystem>();
        snake = FindObjectOfType<SnakeController>();
        checkNExecuteDeactivationCondition();
    }

    void Update() {
        if (isPointAvailable ==  true) {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                PressedGlobe();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                PressedMove();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                PressedTurn();
            }
        }
    }

    public void SetUpView() {
        isPointAvailable = true;
        container.SetActive(true);
        setFreePointsText();
    }

    public void PressedGlobe() {
        system.ConsumePoint();
        checkNExecuteDeactivationCondition();
    }

    public void PressedMove() {
        snake.IncreaseMovement();
        system.ConsumePoint();
        checkNExecuteDeactivationCondition();
    }

    public void PressedTurn() {
        snake.IncrementTurn();
        system.ConsumePoint();
        checkNExecuteDeactivationCondition();
    }

    void checkNExecuteDeactivationCondition() {
        if (system.availablePoints == 0) {
            container.SetActive(false);
            isPointAvailable = false;
        } else {
            setFreePointsText();
        }
    }

    void setFreePointsText() {
        if (system.availablePoints == 1) {
            freePoints.text = system.availablePoints.ToString() + Translator.GetTranslation(" Очко");
        } else {
            freePoints.text = system.availablePoints.ToString() + Translator.GetTranslation(" Очка");
        }
    }

}
