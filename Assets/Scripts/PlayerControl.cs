﻿using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	[HideInInspector]
	
	public float moveForce = 35f;			// Amount of force added to move the player left and right.
	public float maxSpeed =5f;				// The fastest the player can travel in the x axis.
	// Use this for initialization
	public bool interrupt_casting = false;
	private Camera cam;
	public float dir = 0f;

	void Start () {
		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}

	// Update is called once per frame
	void Update () {
		if(networkView.isMine){
			interrupt_casting = false;
			// Cache the horizontal input.
			float h = Input.GetAxis("Horizontal");
			// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
			if(h * rigidbody2D.velocity.x < maxSpeed){
				// ... add a force to the player.
				if(h < 0 && cam.WorldToScreenPoint(transform.position).x > 0){
					rigidbody2D.AddForce(Vector2.right * h * moveForce);
					dir = 180f;
				} else if(h > 0 && cam.WorldToScreenPoint(transform.position).x < Screen.width){
					rigidbody2D.AddForce(Vector2.right * h * moveForce);
					dir = 0f;
				}
			}

			// Cache the horizontal input.
			float v = Input.GetAxis("Vertical");
			// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
			if(v * rigidbody2D.velocity.y < maxSpeed){
				// ... add a force to the player.
				if(v > 0 && cam.WorldToScreenPoint(transform.position).y < Screen.height){
					dir = 90f;
					rigidbody2D.AddForce(Vector2.up * v * moveForce);
				} else if(v < 0 && cam.WorldToScreenPoint(transform.position).y > 0){
					rigidbody2D.AddForce(Vector2.up * v * moveForce);
					dir = 270f;
				}
			}
			if(h != 0 || v != 0){
				interrupt_casting = true;
			}
		} else {
			this.enabled = false;
			if(this.name == "mage(Clone)"){
				gameObject.GetComponent<mage>().enabled = false;
			} else if(this.name == "priest(Clone)"){
				gameObject.GetComponent<priest>().enabled = false;
			} else if(this.name == "warrior(Clone)"){
				gameObject.GetComponent<warrior>().enabled = false;
			} else if(this.name == "hunter(Clone)"){
				gameObject.GetComponent<hunter>().enabled = false;
			}
		}
	}
}
