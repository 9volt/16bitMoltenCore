using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ranged_add_health : MonoBehaviour {
	public int health;
	public int current_health;
	public bool attacking = true;
	public float attack_speed;
	private float next_attack;
	public GameObject shot;

	// Use this for initialization
	void Start () {
		next_attack = Time.time;
		current_health = health;
	}
	
	// Update is called once per frame
	void Update () {
		if(networkView.isMine){
			if(Time.time > next_attack){
				next_attack = Time.time + attack_speed;
				Network.Instantiate(shot, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0, 360)), 0);
			}
		}
	}
	

	void damage(int damage){
		if(networkView.isMine){
			current_health -= damage;
			networkView.RPC("set_health", RPCMode.AllBuffered, current_health);
		}
	}

	[RPC]
	void set_health(int health){
		current_health = health;
		if(current_health <= 0){
			gameObject.SetActive(false);
		}
	}

	void OnTriggerEnter2D(Collider2D c){
		if(c.gameObject.tag == "boss_damaging"){
			if(networkView.isMine){
				damage(c.gameObject.GetComponent<shot>().damage);
			}
			Destroy(c.gameObject);
		}
	}
}
