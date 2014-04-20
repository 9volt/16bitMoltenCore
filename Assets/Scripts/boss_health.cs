using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class boss_health : MonoBehaviour {
	public string name;
	public int health;
	public int current_health;
	public Texture green;
	public Texture red;
	public bool active = false;
	private Dictionary<GameObject, float> aggro_table;
	private GameObject current_aggro;
	public bool attacking = true;
	public float attack_speed;
	public int attack_damage;
	private float next_attack;

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
						current_aggro = kvp.Key;
						max_threat = kvp.Value;
					}
		        }
				if(attacking){
					if(Vector3.Distance(transform.position, current_aggro.transform.position) > 2f){
						transform.position = Vector3.MoveTowards(transform.position,  current_aggro.transform.position , 2 * Time.deltaTime);
					} else if(Time.time > next_attack){
						next_attack = Time.time + attack_speed;
						current_aggro.GetComponent<player_health>().damage(attack_damage);
					}
				}
			}
		}
	}

	void damage(int damage, GameObject player){
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

	[RPC]
	void set_health(int health){
		current_health = health;
	}

	void OnTriggerEnter2D(Collider2D c){
		if(c.gameObject.tag == "boss_damaging"){
			damage(c.gameObject.GetComponent<shot>().damage, c.gameObject.GetComponent<shot>().player);
			Destroy(c.gameObject);
		}
	}

	void OnGUI(){
		if(active){
			GUI.DrawTexture(new Rect(Screen.width / 2 - 100, 0, 200, 20), red);
			GUI.DrawTexture(new Rect(Screen.width / 2 - 100, 0, ((float)current_health/health) * 200, 20), green);
			GUI.Label(new Rect(Screen.width / 2 - (name.Length / 2) * 5, 0, 100, 20), name);
		}
	}
}
