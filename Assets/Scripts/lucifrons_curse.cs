using UnityEngine;
using System.Collections;

public class lucifrons_curse : MonoBehaviour {
	public GameObject target;
	public float countdown;
	private float death_time;
	public int damage;
	// Use this for initialization
	void Start () {
		death_time = Time.time + countdown;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 offset = target.GetComponent<SpriteRenderer>().sprite.bounds.extents;
		Vector3 new_pos = target.transform.position;
		new_pos.y += (offset.y * 2);
		transform.position = new_pos;
		if(Time.time > death_time){
			if(networkView.isMine){
				target.GetComponent<player_health>().damage(damage);
			}
			Destroy(gameObject);
		}
	}
}
