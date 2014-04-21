using UnityEngine;
using System.Collections;

public class shout : MonoBehaviour {
	public float range;
	public float threat;
	public GameObject player;

	// Use this for initialization
	void Start () {
		foreach(GameObject go in GameObject.FindGameObjectsWithTag("enemy")){
			if(Vector3.Distance(transform.position, go.transform.position) < range){
				if(go.GetComponents<boss_health>().Length > 0){
					go.GetComponent<boss_health>().increase_threat(threat, player);
				}
			}
		}
		Destroy(gameObject);
	}
}
