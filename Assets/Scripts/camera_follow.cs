using UnityEngine;
using System.Collections;

public class camera_follow : MonoBehaviour {
	private GameObject[] players;

	// Use this for initialization
	void Start () {

	}

	Vector3 FindCenter(){
		Vector3 sum = Vector3.zero;
		if(players == null || players.Length == 0 ){
			return sum;
		}
		
		foreach(GameObject go in players){
			sum += go.transform.position;
		}
		return sum / players.Length;
	}

	// Update is called once per frame
	void Update () {
		players = GameObject.FindGameObjectsWithTag("Player");
		Vector3 newPos = FindCenter();
		newPos.z = -10;
		transform.position = newPos;
	}
}
