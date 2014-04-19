using UnityEngine;
using System.Collections;

public class mage : MonoBehaviour {
	public GameObject frostbolt;
	private bool casting = false;
	public float frostbolt_cast_time;
	private float end_cast;
	private float start_cast;
	public Texture blue;
	private Camera camera;
	private Vector2 last_heading;
	private int playerNum;
	private PlayerControl pc;

	// Use this for initialization
	void Start () {
		playerNum = gameObject.GetComponent<player_health>().playerNum;
		pc = gameObject.GetComponent<PlayerControl>();
		camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}

	void ShootFrostbolt(){
		Quaternion q = Quaternion.Euler(0f, 0f, pc.dir);
		GameObject g = (GameObject)Instantiate(frostbolt, transform.position, q);
		g.GetComponent<shot>().player = gameObject;
	}

	void ArcaneExplosion(){

	}

	// Update is called once per frame
	void Update () {
		if(casting  && Time.time > end_cast){
			ShootFrostbolt();
			casting = false;
		}
		if(pc.interrupt_casting){
			casting = false;
			last_heading = rigidbody2D.velocity;
		}

		if(!casting && Input.GetButton("A" + playerNum)){
			casting = true;
			start_cast = Time.time;
			end_cast = frostbolt_cast_time + Time.time;
		}
		if(!casting && Input.GetButton("B" + playerNum)){
			ArcaneExplosion();
		}
	}

	void OnGUI(){
		Vector3 g = camera.WorldToScreenPoint(transform.position);
		GUI.DrawTexture(new Rect(g.x, g.y, 5, 5), blue);
		if(casting){
			Vector3 p = camera.WorldToScreenPoint(transform.position);
			GUI.DrawTexture(new Rect(p.x - 20, p.y - 40, ((Time.time-start_cast)/frostbolt_cast_time) * 30, 5), blue);
		}
	}
}
