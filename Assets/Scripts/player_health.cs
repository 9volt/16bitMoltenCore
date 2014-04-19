using UnityEngine;
using System.Collections;

public class player_health : MonoBehaviour {
	public string name;
	public int health;
	public int current_health;
	public Texture green;
	public Texture red;
	public bool active = true;

	// Use this for initialization
	void Start () {
		current_health = health;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void damage(int damage){
		current_health -= damage;
		if(current_health < 0) current_health = 0;
	}

	void OnGUI(){
		if(active){
			GUI.DrawTexture(new Rect(10, Screen.height - 20, 50, 20), red);
			GUI.DrawTexture(new Rect(10, Screen.height - 20, ((float)current_health/health) * 50, 20), green);
			GUI.Label(new Rect(15, Screen.height - 20, 50, 20), name);
		}
	}
}
