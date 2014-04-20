using UnityEngine;
using System.Collections;

public class shot : MonoBehaviour {
	public int damage;
	public GameObject player;
	public float speed;
	public float time_to_live;

	// Use this for initialization
	void Start () {
		//Debug.Log(gameObject.parent);
		float distance = 9999f;
		foreach(GameObject go in GameObject.FindGameObjectsWithTag("Player")){
			if(Vector3.Distance(go.transform.position, transform.position) < distance){
				player = go;
				distance = Vector3.Distance(go.transform.position, transform.position);
			}
		}
		Destroy(gameObject, time_to_live);
	}
	
	// Update is called once per frame
	void Update () {
		rigidbody2D.AddForce(transform.right * speed);
	}
}
