using UnityEngine;
using System.Collections;

public class hunter : MonoBehaviour {
	public GameObject sword;
	public float sword_reswing;
	private float next_swing;
	private Camera camera;
	private PlayerControl pc;

	// Use this for initialization
	void Start () {
		next_swing = Time.time;
		pc = gameObject.GetComponent<PlayerControl>();
		camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}

	void SwingSword(){
		Quaternion q = Quaternion.Euler(0f, 0f, pc.dir);
		Vector3 place = transform.position;
		Vector3 offset = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.extents;
		if(pc.dir == 0){
			place.x = place.x + offset.x;
		} else if(pc.dir == 90){
			place.y = place.y + offset.y;
		} else if(pc.dir == 180){
			place.x = place.x - offset.x;
		} else if(pc.dir == 270){
			place.y = place.y - offset.y;
		}
		Network.Instantiate(sword, place, q, 0);
		//g.GetComponent<shot>().player = gameObject;
	}

	void ThreateningShout(){

	}

	// Update is called once per frame
	void Update () {
		if(Input.GetButton("A")){
			if(Time.time > next_swing){
				next_swing = sword_reswing + Time.time;
				SwingSword();
			}
		}
		if(Input.GetButton("B")){
			ThreateningShout();
		}
	}
}
