using UnityEngine;
using System.Collections;

public class dispel : MonoBehaviour {
	public float range;

	// Use this for initialization
	void Start () {
		foreach(GameObject go in GameObject.FindGameObjectsWithTag("curse")){
			if(Vector3.Distance(transform.position, go.transform.position) < range){
				Destroy(go);
			}
		}
		Destroy(gameObject, .667f);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
