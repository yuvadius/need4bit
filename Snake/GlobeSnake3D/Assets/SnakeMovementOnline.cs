using UnityEngine;
using System.Collections;

public class SnakeMovementOnline : MonoBehaviour {
    public Transform snake;
    public Transform subSnake;
    public static Transform snakeTransform;
    public static Transform subSnakeTransform;

    void Update () {
        snakeTransform = snake;
        subSnakeTransform = subSnake;
    }
}
