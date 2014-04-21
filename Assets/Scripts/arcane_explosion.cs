using UnityEngine;
using System.Collections;

public class arcane_explosion : MonoBehaviour {
	public float range;
	public int damage;
	public GameObject player;

	// Use this for initialization
	void Start () {
		foreach(GameObject go in GameObject.FindGameObjectsWithTag("enemy")){
			if(Vector3.Distance(transform.position, go.transform.position) < range){
				if(go.GetComponents<boss_health>().Length > 0){
					go.GetComponent<boss_health>().damage(damage, player);
				}
				if(go.GetComponents<ranged_add_health>().Length > 0){
					go.GetComponent<ranged_add_health>().damage(damage);
				}
			}
		}
		Destroy(gameObject);
	}
}
