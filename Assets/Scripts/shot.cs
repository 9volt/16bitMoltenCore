using UnityEngine;
using System.Collections;

public class shot : MonoBehaviour {
	public int damage;
	public GameObject player;
	public float speed;

	// Use this for initialization
	void Start () {
		Destroy(gameObject, 20);
	}
	
	// Update is called once per frame
	void Update () {
		rigidbody2D.AddForce(transform.right * speed);
	}
}
