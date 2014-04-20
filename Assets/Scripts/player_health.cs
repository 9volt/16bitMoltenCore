using UnityEngine;
using System.Collections;

public class player_health : MonoBehaviour {
	private float barW;
	private float barH;
	private float barX;
	private float barY;

	public string name;
	public int health;
	public int current_health;
	public Texture green;
	public Texture red;
	public bool active = true;
	public int playerNum;
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

	public void damage(int damage){
		current_health -= damage;
		networkView.RPC("set_health", RPCMode.AllBuffered, current_health);
		if(current_health < 0) current_health = 0;
	}
	
	[RPC]
	void set_health(int new_health){
		current_health = new_health;
		if(current_health <= 0){
			gameObject.SetActive(false);
		}
	}


	void OnGUI(){
		if(active){
			GUI.DrawTexture(new Rect(barX, barY, barW, barH), red);
			GUI.DrawTexture(new Rect(barX, barY, ((float)current_health/health) * barW, barH), green);
			GUI.Label(new Rect(barX, barY, barW, barH), name);
		}
	}
}
