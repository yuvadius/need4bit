using UnityEngine;
using System.Collections;

/// <summary>
/// Head eat. Bad name, cause this script is for collision with the head.
/// 
/// responsible for the biting animation, and triggering adding segment events in the trail.
/// 
/// responsible for sending game over message to main controller upon impact with segment.
/// </summary>
public class HeadEat : MonoBehaviour {

	public bool isRemote;

	public Collider myCollider;
	public GameObject snakeHead;
	public GameObject testerHead;

	BiteScript bite;
	Trail trail;
    LevelingSystem levelingSystem;

	void Start(){
		if(isRemote == false) {
			bite = GetComponent<BiteScript>();
			trail = transform.parent.gameObject.GetComponent<Trail>();
			levelingSystem = GameObject.FindObjectOfType<LevelingSystem>();
		}

		BenchmarkController benchmarker = BenchmarkController.instance;
		if(benchmarker != null) {
			benchmarker.SettleSnakeHead(this);
		}
	}

    float DistanceFromPeak(Vector3 contact, GameObject gameObject)
    {
        CapsuleCollider collider = gameObject.GetComponent<CapsuleCollider>();
        Vector3 peak = gameObject.transform.rotation * Vector3.forward;
        peak = gameObject.transform.TransformPoint(collider.center) + (Vector3.ClampMagnitude(peak, collider.height / 2) * gameObject.transform.parent.localScale.z);
        return Vector3.Distance(contact, peak);
    }

    void OnTriggerEnter(Collider other)
    {
        if (isRemote == false)
        {
            if (other.tag == "apple")
            {
                bite.bite();
                trail.addSegment();
                levelingSystem.AddApple();
            }
            else if (other.tag == "bannana")
            {
                StartCoroutine(chew(1.5f, Random.Range(1, 3)));
                levelingSystem.AddBannana();
            }
            else if (other.tag == "segment")
            {
                GetComponent<Collider>().enabled = false;
                MainController.instance.gameOver();
            }
            else if (other.tag != "head")
            {
                Debug.LogError("Hit something that not supposed to be " + other.tag);
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "head" || isRemote == false)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit))
                {
                    if (DistanceFromPeak(hit.point, gameObject) < DistanceFromPeak(hit.point, other.gameObject))
                    {
                        GetComponent<Collider>().enabled = false;
                        MainController.instance.gameOver();
                    }
                }
            }
    }

	IEnumerator chew(float seconds, int times){
		bite.bite();
		float time = seconds;
		float fraction = 0;
		float period = seconds/times;

		while( time > 0 ){
			time -= Time.deltaTime;
			fraction += Time.deltaTime;

			if( fraction > period ){
				bite.bite ();
				fraction = 0;
			}

			yield return null;
		}

		trail.addSegment();
	}
}
