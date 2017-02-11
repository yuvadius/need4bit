using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;		//Public variable to store a reference to the player game object
	private Vector3 offset;			//Private variable to store the offset distance between the player and camera

    public GameObject objectTarget;
    public Vector3 screenPosition = new Vector3(0, 0, 20);

    // Use this for initialization
    void Start () 
	{
		//Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position - Vector3.zero;

        //objectTarget.transform.position = GetComponent<Camera>().ScreenToWorldPoint(screenPosition);
    }
	
	// LateUpdate is called after Update each frame
	void LateUpdate () 
	{
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        if (player != null)
            transform.position = player.transform.position + offset;
	}
}
