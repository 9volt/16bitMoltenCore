using UnityEngine;
using System.Collections;

public class boss_health : MonoBehaviour {
	public string name;
	public int health;
	public int current_health;
	public Texture green;
	public Texture red;
	public bool active = false;

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
			GUI.DrawTexture(new Rect(Screen.width / 2 - 100, 0, 200, 20), red);
			GUI.DrawTexture(new Rect(Screen.width / 2 - 100, 0, ((float)current_health/health) * 200, 20), green);
			GUI.Label(new Rect(Screen.width / 2 - (name.Length / 2) * 5, 0, 100, 20), name);
		}
	}
}
