using UnityEngine;
using System.Collections;

public class priest : MonoBehaviour {
	public GameObject heal;
	public GameObject dispel;
	public float dispel_recast;
	private float next_dispel;
	private bool casting = false;
	public float heal_cast_time;
	private float end_cast;
	private float start_cast;
	public Texture blue;
	private Camera cam;
	private PlayerControl pc;

	// Use this for initialization
	void Start () {
		next_dispel = Time.time;
		pc = gameObject.GetComponent<PlayerControl>();
		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}

	void ShootHeal(){
		Quaternion q = Quaternion.Euler(0f, 0f, pc.dir);
		Network.Instantiate(heal, transform.position, q, 0);
		//g.GetComponent<shot>().player = gameObject;
	}

	void Dispel(){
		if(Time.time > next_dispel){
			Network.Instantiate(dispel, transform.position, transform.rotation, 0);
			next_dispel = Time.time + dispel_recast;
		}
	}


	// Update is called once per frame
	void Update () {
		if(casting  && Time.time > end_cast){
			ShootHeal();
			casting = false;
		}
		if(pc.interrupt_casting){
			casting = false;
		}
		if(!casting && Input.GetButton("A")){
			casting = true;
			start_cast = Time.time;
			end_cast = heal_cast_time + Time.time;
		}
		if(!casting && Input.GetButton("B")){
			Dispel();
		}
	}

	void OnGUI(){
		Vector3 g = cam.WorldToScreenPoint(transform.position);
		Rect offset = gameObject.GetComponent<SpriteRenderer>().sprite.rect;
		GUI.DrawTexture(new Rect(g.x, g.y, 5, 5), blue);
		if(casting){
			Vector3 p = cam.WorldToScreenPoint(transform.position);
			GUI.DrawTexture(new Rect(p.x - 15, p.y - (offset.height / 1.25f), ((Time.time-start_cast)/heal_cast_time) * 30, 5), blue);
		}
	}
}
