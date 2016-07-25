using UnityEngine;
using System.Collections;

public class SpeedController : MonoBehaviour {

    public float speedUpRate = 1;
    public float slowDownRate = 2;

    SnakeController snake;
    RotateForward speedLocker;
    InputDriver inputDriver;

    void Start() {
        snake = FindObjectOfType<SnakeController>();
        speedLocker = FindObjectOfType<RotateForward>();
        inputDriver = FindObjectOfType<InputDriver>();
    }

    void Update() {
        if (speedLocker.IsStarted() == false) {
            return; //because we have a slow start mechanism
        }

        if (inputDriver.verticalMove > 0) {
            moveFaster();
        } else if (inputDriver.verticalMove < 0) {
            moveSlower();
        } else {
            stabilize();
        }
    }

    void moveFaster() {
        float newMoveSpeed = snake.moveSpeed + speedUpRate * Time.deltaTime;
        if (newMoveSpeed > snake.constSpeed + snake.moveExtraUp * inputDriver.verticalMove) {
            newMoveSpeed = snake.constSpeed + snake.moveExtraUp * inputDriver.verticalMove;
        }

        snake.SetNewSpeed(newMoveSpeed);
    }

    void moveSlower() {
        float newMoveSpeed = snake.moveSpeed - snake.moveExtraDown * -inputDriver.verticalMove * Time.deltaTime;
        if (newMoveSpeed < snake.constSpeed - snake.moveExtraDown * -inputDriver.verticalMove) {
            newMoveSpeed = snake.constSpeed - snake.moveExtraDown * -inputDriver.verticalMove;
        }

        snake.SetNewSpeed(newMoveSpeed);
    }

    void stabilize() {
        if (snake.moveSpeed < snake.constSpeed) {
            float newMoveSpeed = snake.moveSpeed + speedUpRate * Time.deltaTime;
            if (newMoveSpeed > snake.constSpeed) {
                newMoveSpeed = snake.constSpeed;
            }

            snake.SetNewSpeed(newMoveSpeed);
        } else if (snake.moveSpeed > snake.constSpeed) {
            float newMoveSpeed = snake.moveSpeed - slowDownRate * Time.deltaTime;
            if (newMoveSpeed < snake.constSpeed) {
                newMoveSpeed = snake.constSpeed;
            }

            snake.SetNewSpeed(newMoveSpeed);
        }

    }
}
