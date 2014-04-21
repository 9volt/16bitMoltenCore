using UnityEngine;
using System.Collections;

public class lucifron : MonoBehaviour {
	private boss_health health;
	private int curse_count = 0;
	public GameObject lucifron_curse;
	public GameObject lucifron_add;

	// Use this for initialization
	void Start () {
		if(networkView.isMine){
			health = (boss_health)gameObject.GetComponent<boss_health>();
			Vector3 offset = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size;
			Vector3 pos = transform.position;
			pos.x += offset.x * 2;
			Network.Instantiate(lucifron_add, pos, transform.rotation, 0);
			pos = transform.position;
			pos.x -= offset.x * 2;
			Network.Instantiate(lucifron_add, pos, transform.rotation, 0);
		}
	}

	void CursePlayers(){
		curse_count++;
		foreach(GameObject player in health.GetPlayers()){
			lucifron_curse.GetComponent<lucifrons_curse>().target = player;
			Network.Instantiate(lucifron_curse, player.transform.position, player.transform.rotation, 0);
		}
	}

	// Update is called once per frame
	void Update () {
		if(networkView.isMine){
			if(health.current_health < health.health * .75f && curse_count < 1){
				CursePlayers();
			}
			if(health.current_health < health.health * .50f && curse_count < 2){
				CursePlayers();
			}
			if(health.current_health < health.health * .25f && curse_count < 3){
				CursePlayers();
			}
			if(health.current_health < health.health * .10f && curse_count < 4){
				CursePlayers();
			}
		}
	}
}
