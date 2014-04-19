using UnityEngine;
using System.Collections;

public class mage : MonoBehaviour {
	public GameObject frostbolt;
	private bool casting;
	public float frostbolt_cast_time;
	private float end_cast;
	private float start_cast;
	public Texture blue;
	public Camera camera;
	private Vector2 last_heading;
	private int playerNum;
	// Use this for initialization
	void Start () {
		playerNum = gameObject.GetComponent<player_health>().playerNum;
	}

	void ShootFrostbolt(){
		Debug.Log(last_heading);
		Quaternion q = Quaternion.LookRotation(last_heading, Vector3.forward);
		GameObject g = (GameObject)Instantiate(frostbolt, transform.position, transform.rotation);
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
		if(rigidbody2D.velocity != Vector2.zero){
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
		if(casting){
			Vector3 p = camera.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y * -1, 0));
			GUI.DrawTexture(new Rect(p.x - 20, p.y - 40, ((Time.time-start_cast)/frostbolt_cast_time) * 30, 5), blue);
		}
	}
}
