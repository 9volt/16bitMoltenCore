﻿using UnityEngine;
using System.Collections;

public class mage : MonoBehaviour {
	public GameObject frostbolt;
	public GameObject arcane_explosion;
	private bool casting = false;
	public float frostbolt_cast_time;
	private float end_cast;
	private float start_cast;
	public Texture blue;
	private Camera cam;
	private PlayerControl pc;
	public float ae_recast;
	private float next_ae;


	// Use this for initialization
	void Start () {
		next_ae = Time.time;
		pc = gameObject.GetComponent<PlayerControl>();
		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}

	void ShootFrostbolt(){
		Quaternion q = Quaternion.Euler(0f, 0f, pc.dir);
		Network.Instantiate(frostbolt, transform.position, q, 0);
		//g.GetComponent<shot>().player = gameObject;
	}

	void ArcaneExplosion(){
		if(Time.time > next_ae){
			next_ae = Time.time + ae_recast;
			arcane_explosion.GetComponent<arcane_explosion>().player = gameObject;
			Network.Instantiate(arcane_explosion, transform.position, transform.rotation, 0);
		}
	}

	// Update is called once per frame
	void Update () {
		if(casting  && Time.time > end_cast){
			ShootFrostbolt();
			casting = false;
		}
		if(pc.interrupt_casting){
			casting = false;
		}
		if(!casting && Input.GetButton("A")){
			casting = true;
			start_cast = Time.time;
			end_cast = frostbolt_cast_time + Time.time;
		}
		if(!casting && Input.GetButton("B")){
			ArcaneExplosion();
		}
	}

	void OnGUI(){
		Vector3 g = cam.WorldToScreenPoint(transform.position);
		Rect offset = gameObject.GetComponent<SpriteRenderer>().sprite.rect;
		GUI.DrawTexture(new Rect(g.x, g.y, 5, 5), blue);
		if(casting){
			Vector3 p = cam.WorldToScreenPoint(transform.position);
			GUI.DrawTexture(new Rect(p.x - 15, p.y - (offset.height / 1.25f), ((Time.time-start_cast)/frostbolt_cast_time) * 30, 5), blue);
		}
	}
}
