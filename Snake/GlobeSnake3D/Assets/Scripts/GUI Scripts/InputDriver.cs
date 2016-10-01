using UnityEngine;
using System.Collections;

public class InputDriver : MonoBehaviour {

    public float changeMove = 3f;

    public float horizontalMove = 0;
    public float verticalMove = 0;  

    public Camera snakeCamera;
    public Transform snakeHead;

    bool isDetecting = false;

    Vector2 center;
    float maxDis;

    void Start() {
        center.x = Screen.width / 2;
        center.y = Screen.height / 2;

        maxDis = center.magnitude;
    }

    void Update() {

        float verticalAim;
        float horizontalAim;

        if (isDetecting == true) {
            //verticalAim = (float)Input.mousePosition.y * 2f / (float)Screen.height - 1f;
            //horizontalAim = (float)Input.mousePosition.x * 2f / (float)Screen.width - 1f;

            Vector2 mousePos = Input.mousePosition;
            Vector2 snakeVec = getSnakeForwardPlaceOnScreen() - getSnakePlaceOnScreen();
            Vector2 mouseVec = mousePos - getSnakePlaceOnScreen();

            float mouseAng = getAngle(mouseVec);
            float snakeAng = getAngle(snakeVec);

            horizontalAim = getAngDelta(snakeAng, mouseAng);

            verticalAim = (mousePos - center).magnitude * 2 / maxDis - 1f;
        } else {
            verticalAim = horizontalAim = 0;
        }
    
        horizontalMove = horizontalAim > horizontalMove ? 
            Mathf.Clamp(horizontalMove + Time.deltaTime * changeMove, horizontalMove, horizontalAim) : 
            Mathf.Clamp(horizontalMove - Time.deltaTime * changeMove, horizontalAim, horizontalMove);
        
        verticalMove = verticalAim > verticalMove ? 
            Mathf.Clamp(verticalMove + Time.deltaTime * changeMove, verticalMove, verticalAim) : 
            Mathf.Clamp(verticalMove - Time.deltaTime * changeMove, verticalAim, verticalMove);

#if UNITY_EDITOR || UNITY_STANDALONE
		float inputHorizontalMove = Input.GetAxis("Horizontal");
        float inputVerticalMove = Input.GetAxis("Vertical");

        if (inputHorizontalMove != 0) {
            horizontalMove = inputHorizontalMove;
        }

        if (inputVerticalMove != 0) {
            verticalMove = inputVerticalMove;
        }
#endif
        //Debug.Log("horizontalMove: " + horizontalMove + ", verticalMove: " + verticalMove);

    }

    float getAngDelta(float ang1, float ang2) {

        float positive = 0;

        if (ang2 >= ang1) {
            positive =  ang2 - ang1;
        } else {
            positive = (180f - ang1) - (-180f - ang2);
        }

        if (positive < 180) {
            return positive / 180;
        } else {
            return (positive - 360) / 180;
        }
    }

    float getAngle(Vector2 vec) {

        return Mathf.Atan2(vec.x, vec.y) * Mathf.Rad2Deg;

    }

    Vector2 getSnakePlaceOnScreen() {
        return snakeCamera.WorldToScreenPoint(snakeHead.position);
    }

    Vector2 getSnakeForwardPlaceOnScreen() {
        return snakeCamera.WorldToScreenPoint(snakeHead.position + snakeHead.forward * 3);
    }


    Vector2 getSnakeDirection() {
        
        //Vector3 headRightProject = Vector3.Project(snakeHead.forward, snakeCamera.transform.right);
        //Vector3 headForwardProject = Vector3.Project(snakeHead.forward, snakeCamera.transform.forward);

        //print(snakeHead.InverseTransformVector(snakeHead.forward) + " " + snakeHead.forward);
        

        //float rightAim = (snakeCamera.transform.right - headRightProject).sqrMagnitude;
        //float forwardAim = (snakeCamera.transform.forward - headForwardProject).sqrMagnitude;

        //print(rightAim + " " + forwardAim);

        //print( + " " + snakeCamera.transform.right);




        return Vector2.one;
    }

    public void DetectMouseInput() {
        isDetecting = true;
    }

    public void StopDetection() {
        isDetecting = false;
    }

}
