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
	public GameObject spawn;
	public GameObject[] bosses;
	public GameObject[] boss_spawns;

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
		MasterServer.RegisterHost(game_name, "Thenmal Networking Test", "DO NOT EAT");
	}


	void RefreshHostList(){
		MasterServer.RequestHostList(game_name);
		refreshing = true;
		end_time = Time.time + 10f;
		MasterServer.PollHostList();
	}

	void OnServerInitialized(){
		Debug.Log("Server initialized");
		SpawnPlayer();
		for(int i = 0; i < bosses.Length; i++){
			Network.Instantiate(bosses[i], boss_spawns[i].transform.position, boss_spawns[i].transform.rotation, 0);
		}
	}

	void OnPlayerConnected(){

	}

	void SpawnPlayer(){
		Network.Instantiate(mage, spawn.transform.position, spawn.transform.rotation, 0);
	}

	void SpawnPlayer2(){
		Network.Instantiate(priest, spawn.transform.position, spawn.transform.rotation, 0);
	}
	
	void OnConnectedToServer(){
		SpawnPlayer2();
	}

	void OnGUI(){
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
