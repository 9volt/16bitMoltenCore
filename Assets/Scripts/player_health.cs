﻿using UnityEngine;
using System.Collections;

public class player_health : MonoBehaviour {
	private float barW;
	private float barH;
	private float barX;
	private float barY;

	public string player_name;
	public int health;
	public int current_health;
	public Texture green;
	public Texture red;
	public bool player_active = true;
	public int playerNum;
	private GameObject heal_duplication_check = null;
	// Use this for initialization
	void Start () {
		int player_count = GameObject.FindGameObjectsWithTag("Player").Length;
		barW = Screen.width * .15f;
		barH = 15f;
		barX = barW * player_count * ((player_count * .1f) + 1f);
		barY = (Screen.height * .9f) - barH;
		current_health = health;
	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D c){
		if(c.gameObject.tag == "player_damaging"){
			if(networkView.isMine){
				damage(c.gameObject.GetComponent<shot>().damage);
			}
			Destroy(c.gameObject);
		}
		if(c.gameObject.tag == "player_healing" && c.gameObject != heal_duplication_check){
			heal_duplication_check = c.gameObject;
			if(networkView.isMine){
				damage(c.gameObject.GetComponent<shot>().damage);
			}
		}
	}

	public void damage(int damage){
		current_health -= damage;
		networkView.RPC("set_health", RPCMode.AllBuffered, current_health);
		if(current_health < 0) current_health = 0;
		if(current_health > health) current_health = health;
	}
	
	[RPC]
	void set_health(int new_health){
		current_health = new_health;
		if(current_health <= 0){
			gameObject.SetActive(false);
		}
	}


	void OnGUI(){
		if(player_active){
			GUI.DrawTexture(new Rect(barX, barY, barW, barH), red);
			GUI.DrawTexture(new Rect(barX, barY, ((float)current_health/health) * barW, barH), green);
			GUI.Label(new Rect(barX, barY, barW, barH), player_name);
		}
	}
}
