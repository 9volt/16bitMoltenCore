using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class boss_health : MonoBehaviour {
	public string boss_name;
	public int health;
	public int current_health;
	public Texture green;
	public Texture red;
	public bool boss_active = true;
	private Dictionary<GameObject, float> aggro_table;
	private GameObject current_aggro;
	public bool attacking = true;
	public float attack_speed;
	public int attack_damage;
	private float next_attack;
	private bool touching = false;

	// Use this for initialization
	void Start () {
		next_attack = Time.time;
		current_health = health;
		aggro_table = new Dictionary<GameObject, float>();
	}
	
	// Update is called once per frame
	void Update () {
		if(networkView.isMine){
			if(aggro_table.Count > 0){
				float max_threat = 0;
				foreach(KeyValuePair<GameObject, float> kvp in aggro_table){
					if(kvp.Value > max_threat){
						if(current_aggro != kvp.Key){
							touching = false;
						}
						current_aggro = kvp.Key;
						max_threat = kvp.Value;
					}
					if(!kvp.Key.activeSelf){
						aggro_table.Remove(kvp.Key);
					}
		        }
				if(attacking){
					if(!touching){
						transform.position = Vector3.MoveTowards(transform.position,  current_aggro.transform.position , 2 * Time.deltaTime);
					}
				}
			}
		}
	}

	public void damage(int damage, GameObject player){
		if(networkView.isMine){
			current_health -= damage;
			networkView.RPC("set_health", RPCMode.AllBuffered, current_health);
			if(current_health < 0) current_health = 0;
			float threat = 0;
			if(aggro_table.TryGetValue(player, out threat)){
				aggro_table[player] = threat + damage;
			} else {
				aggro_table.Add(player, damage);
			}
		}
	}

	public void increase_threat(float inc_threat, GameObject player){
		if(networkView.isMine){
			float threat;
			if(aggro_table.TryGetValue(player, out threat)){
				aggro_table[player] = threat + inc_threat;
			} else {
				aggro_table.Add(player, inc_threat);
			}
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
				damage(c.gameObject.GetComponent<shot>().damage, c.gameObject.GetComponent<shot>().player);
			}
			Destroy(c.gameObject);
		}
		if(networkView.isMine){
			if(c.gameObject == current_aggro){
				touching = true;
			}
		}
	}

	void OnTriggerExit2D(Collider2D c){
		if(networkView.isMine){
			if(c.gameObject == current_aggro){
				touching = false;
			}
		}
	}

	
	void OnTriggerStay2D(Collider2D c){
		if(c.gameObject == current_aggro){
			touching = true;
			if(Time.time > next_attack){
				next_attack = Time.time + attack_speed;
				current_aggro.GetComponent<player_health>().damage(attack_damage);
			}
		}
	}

	void OnGUI(){
		if(boss_active){
			GUI.DrawTexture(new Rect(Screen.width / 2 - 100, 0, 200, 20), red);
			GUI.DrawTexture(new Rect(Screen.width / 2 - 100, 0, ((float)current_health/health) * 200, 20), green);
			GUI.Label(new Rect(Screen.width / 2 - (boss_name.Length / 2) * 5, 0, 100, 20), boss_name);
		}
	}
}
