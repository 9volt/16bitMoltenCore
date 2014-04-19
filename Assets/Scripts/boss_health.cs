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

	// Use this for initialization
	void Start () {
		current_health = health;
		aggro_table = new Dictionary<GameObject, float>();
	}
	
	// Update is called once per frame
	void Update () {
		if(aggro_table.Count > 0){
			GameObject target = null;
			float max_threat = 0;
			foreach(KeyValuePair<GameObject, float> kvp in aggro_table){
				if(kvp.Value > max_threat){
					target = kvp.Key;
					max_threat = kvp.Value;
				}
	        }
			if(Vector3.Distance(transform.position, target.transform.position) > 2f){
	        	transform.position = Vector3.MoveTowards(transform.position,  target.transform.position , 2 * Time.deltaTime);
			}
		}
	}

	void damage(int damage, GameObject player){
		current_health -= damage;
		if(current_health < 0) current_health = 0;
		float threat = 0;
		if(aggro_table.TryGetValue(player, out threat)){
			aggro_table[player] = threat + damage;
		} else {
			aggro_table.Add(player, damage);
		}
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
