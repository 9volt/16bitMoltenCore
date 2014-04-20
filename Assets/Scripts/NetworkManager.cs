using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
	private float btnX;
	private float btnY;
	private float btnW;
	private float btnH;
	private string game_name = "16bitMoltenCore_thenmal";
	private bool refreshing = false;
	private float end_time;
	private HostData[] servers = new HostData[0];
	public GameObject mage;
	public GameObject priest;
	public GameObject warrior;
	public GameObject hunter;
	public GameObject spawn;
	public GameObject[] bosses;
	public GameObject[] boss_spawns;
	private bool spawning = false;

	// Use this for initialization
	void Start () {
		btnX = Screen.width * 0.05f;
		btnY = Screen.width * 0.075f;
		btnW = Screen.width * 0.1f;
		btnH = Screen.width * 0.05f;
	}
	
	// Update is called once per frame
	void Update () {
		if(refreshing){
			if(MasterServer.PollHostList().Length > 0){
				refreshing = false;
				Debug.Log(MasterServer.PollHostList().Length);
				servers = MasterServer.PollHostList();
			} else if(Time.time > end_time){
				Debug.Log("No server found");
				refreshing = false;
			}
		}
	}

	void StartServer(){
		Network.InitializeServer(4, 25025, !Network.HavePublicAddress());
		MasterServer.RegisterHost(game_name, "16bitMoltenCore", "DO NOT EAT");
	}


	void RefreshHostList(){
		MasterServer.RequestHostList(game_name);
		refreshing = true;
		end_time = Time.time + 10f;
		MasterServer.PollHostList();
	}

	void OnServerInitialized(){
		spawning = true;
		for(int i = 0; i < bosses.Length; i++){
			Network.Instantiate(bosses[i], boss_spawns[i].transform.position, boss_spawns[i].transform.rotation, 0);
		}
	}

	void OnPlayerConnected(){

	}

	void SpawnPlayer(GameObject prefab){
		spawning = false;
		Network.Instantiate(prefab, spawn.transform.position, spawn.transform.rotation, 0);
	}
	
	void OnConnectedToServer(){
		spawning = true;
	}

	bool IsThereA(string class_name){
		foreach(GameObject go in GameObject.FindGameObjectsWithTag("Player")){
			if(go.name == class_name + "(Clone)" && go.activeSelf){
				return true;
			}
		}
		return false;
	}

	void OnGUI(){
		if(spawning){
			if(!IsThereA("mage")){
				if(GUI.Button(new Rect(btnX, btnY, btnW, btnH), "Mage")){
					SpawnPlayer(mage);
				}
			}
			if(!IsThereA("priest")){
				if(GUI.Button(new Rect(btnX, btnY * 1.2f + btnH, btnW, btnH), "Priest")){
					SpawnPlayer(priest);
				}
			}
			if(!IsThereA("warrior")){
				if(GUI.Button(new Rect(btnX, btnY * 1.4f + (btnH * 2), btnW, btnH), "Warrior")){
					SpawnPlayer(warrior);
				}
			}
			if(!IsThereA("hunter")){
				if(GUI.Button(new Rect(btnX, btnY * 1.6f + (btnH * 3), btnW, btnH), "Hunter")){
					SpawnPlayer(hunter);
				}
			}
		}

		if(!Network.isClient && !Network.isServer){
			if(GUI.Button(new Rect(btnX, btnY, btnW, btnH), "Start Server")){
				StartServer();
			}
			if(GUI.Button(new Rect(btnX, btnY * 1.2f + btnH, btnW, btnH), "Find Servers")){
				RefreshHostList();
			}
			for(int i = 0; i < servers.Length; i++){
				if(GUI.Button(new Rect(btnX * 1.5f + btnW, btnY * 1.2f + btnH + (btnH * i), btnW * 3f, btnH), string.Join(".", servers[i].ip) + " : " + servers[i].connectedPlayers)){
					Network.Connect(servers[i]);
				}
			}
		}
	}
}
