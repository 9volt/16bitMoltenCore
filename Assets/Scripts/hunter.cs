using UnityEngine;
using System.Collections;

public class hunter : MonoBehaviour {
	public GameObject arrow;
	public float shot_speed;
	public float multi_shot_speed;
	private float next_shot;
	private float next_multishot;
	private PlayerControl pc;

	// Use this for initialization
	void Start () {
		next_shot = Time.time;
		next_multishot = Time.time;
		pc = gameObject.GetComponent<PlayerControl>();
	}

	void Multishot(){
		Vector3 place = transform.position;
		Vector3 offset = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.extents;
		if(pc.dir == 0){
			place.x = place.x + offset.x;
		} else if(pc.dir == 90){
			place.y = place.y + offset.y;
		} else if(pc.dir == 180){
			place.x = place.x - offset.x;
		} else if(pc.dir == 270){
			place.y = place.y - offset.y;
		}
		float new_dir = Random.Range(-15, 15) + pc.dir;
		Quaternion q = Quaternion.Euler(0f, 0f, new_dir);
		Network.Instantiate(arrow, place, q, 0);
		//g.GetComponent<shot>().player = gameObject;
	}

	void ShootArrow(){
		Vector3 place = transform.position;
		Network.Instantiate(arrow, place, TowardsClosestTarget(), 0);
	}

	Quaternion TowardsClosestTarget(){
		float closest_distance = 999f;
		GameObject closest_enemy = null;
		foreach(GameObject go in GameObject.FindGameObjectsWithTag("enemy")){
			float dist = Vector3.Distance(go.transform.position, transform.position);
			if(dist < closest_distance){
				closest_enemy = go;
				closest_distance = dist;
			}
		}
		if(closest_enemy == null){
			return Quaternion.Euler(0f, 0f, pc.dir);
		} else {
			// How you get an angle towards a target in 2D
			Vector3 target = closest_enemy.transform.position - transform.position;
			target.Normalize();
			return Quaternion.AngleAxis(Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg, Vector3.forward);
		}
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetButton("A")){
			if(Time.time > next_shot){
				next_shot = shot_speed + Time.time;
				ShootArrow();
			}
		}
		if(Input.GetButton("B")){
			if(Time.time > next_multishot){
				Multishot();
				next_multishot = multi_shot_speed + Time.time;
			}
		}
	}
}
