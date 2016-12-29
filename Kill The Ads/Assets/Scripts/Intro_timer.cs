using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Intro_timer : MonoBehaviour 
{
    private float curr_time;
    private int curr_sprite;
    public GameObject curr_state;
    private Vector3 num_place;

	// Use this for initialization
	void Start () 
    {
        curr_time = 0f;
        curr_sprite = 3;
        curr_state = GameObject.FindGameObjectWithTag("3");
        num_place = curr_state.transform.position;
	}
	
	// Update is called once per frame
	void Update () 
    {
        curr_time += Time.deltaTime;

        if(curr_time > 1f)
        {
            if (curr_sprite != 1)
            {
                curr_time = 0f;
                Change_Sprite();
            }
            else
            {
                SceneManager.LoadScene("main");
            }
        }
	}

    void Change_Sprite()
    {
        Destroy(curr_state);
        if( curr_sprite == 3 )
        {
            curr_state = GameObject.FindGameObjectWithTag("2");
            curr_sprite = 2;
            curr_state.transform.position = num_place;
        }

        else if (curr_sprite == 2)
        {
            curr_state = GameObject.FindGameObjectWithTag("1");
            curr_sprite = 1;
            curr_state.transform.position = num_place;
        }
    }
}
