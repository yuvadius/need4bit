using UnityEngine;
using System.Collections;

public class PlayerController : Photon.MonoBehaviour
{
    public float speed;				//Floating point variable to store the player's movement speed.
    public SpriteRenderer sprite;   //Store a reference to the SpriteRenderer component required to flip the player.
    public Rigidbody2D rb2d;		//Store a reference to the Rigidbody2D component required to use 2D Physics.
    public bool isPlayer;           //Is this a player or an enemy AI.

    // Use this for initialization
    void Start()
	{
        if (!isPlayer)
        {
            transform.parent = GameObject.Find("Enemy Manager").transform;
            name = NameGenerator.GetRandomName();
        }
        else
        {
            name = photonView.name;
            if (!photonView.isMine)
                transform.parent = GameObject.Find("Enemy Players").transform;
        }
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
	{
        if (photonView.isMine && isPlayer)
        {
            //Store the current horizontal input in the float moveHorizontal.
            float moveHorizontal = Input.GetAxis("Horizontal");

            //flip fish to the acceleration direction
            if (moveHorizontal > 0)
                sprite.flipX = true;
            else if (moveHorizontal < 0)
                sprite.flipX = false;

            //Store the current vertical input in the float moveVertical.
            float moveVertical = Input.GetAxis("Vertical");

            //Use the two store floats to create a new Vector2 variable movement.
            Vector2 movement = new Vector2(moveHorizontal, moveVertical);

            //Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
            rb2d.AddForce(movement * speed);
        }
        else if (rb2d.velocity.x > 0)
            sprite.flipX = true;
        else if (rb2d.velocity.x < 0)
            sprite.flipX = false;
    }

	//OnTriggerEnter2D is called whenever this object overlaps with a trigger collider.
	void OnTriggerEnter2D(Collider2D other) 
	{
        if (photonView.isMine)
        {
            //Check the provided Collider2D parameter other to see if it is tagged "PickUp", if it is...
            if (other.gameObject.CompareTag("PickUp"))
            {
                if (PhotonNetwork.isMasterClient)
                    PhotonNetwork.Destroy(other.gameObject);
                else
                    photonView.RPC("DestroyPickup", PhotonTargets.MasterClient, other.gameObject.GetPhotonView().viewID);
                //Grow because you just ate a pickup
                Grow(other.GetComponent<PickupController>().GetVolume());
            }
            else if (other.gameObject.CompareTag("Player"))
            {
                if (other.gameObject.transform.localScale.x > transform.localScale.x)
                    DestroyPlayer();
                else if (transform.localScale.x > other.gameObject.transform.localScale.x)
                    Grow(other.transform.GetComponent<PlayerController>().GetGrow());
            }
            else if (other.gameObject.CompareTag("Boundaries"))
            {
                DestroyPlayer();
            }
        }
	}

    void Grow(float volume)
    {
        if (photonView.isMine)
        {
            float grow = Mathf.Sqrt(GetVolume() + volume);
            transform.localScale = new Vector3(grow, grow, grow);
        }
    }

    /// <summary>
    /// How much you get to grow by eating this fish
    /// </summary>
    /// <returns>A float representing the volume tthat is your growth</returns>
    float GetGrow()
    {
        return GetVolume();
    }

    void DestroyPlayer()
    {
        PhotonNetwork.Destroy(gameObject);
        if (isPlayer)
            GameObject.Find("Main Menu").GetComponent<TutorialInfo>().ShowLaunchScreen();
    } 

    [PunRPC]
    void DestroyPickup(int pvID)
    {
        PhotonNetwork.Destroy(PhotonView.Find(pvID));
    }

    float GetVolume()
    {
        return transform.localScale.x * transform.localScale.y;
    }

    public int GetScore()
    {
        return Matchmaker.VolumeToScore(GetVolume());
    }
}
