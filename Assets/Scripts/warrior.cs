using UnityEngine;
using System.Collections;

public class warrior : MonoBehaviour {
	public GameObject sword;
	public float sword_reswing;
	private float next_swing;
	public Texture blue;
	private Camera camera;
	private Vector2 last_heading;
	private PlayerControl pc;

	// Use this for initialization
	void Start () {
		next_swing = Time.time;
		pc = gameObject.GetComponent<PlayerControl>();
		camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}

	void SwingSword(){
		Quaternion q = Quaternion.Euler(0f, 0f, pc.dir);
		Network.Instantiate(sword, transform.position, q, 0);
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

	void OnGUI(){
		Vector3 g = camera.WorldToScreenPoint(transform.position);
	}
}
