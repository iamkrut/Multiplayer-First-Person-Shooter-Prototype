using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prototype.NetworkLobby;
using UnityEngine.SceneManagement;

public class SelectMapScript : MonoBehaviour {

	public void OnMap1(){
		GameObject.Find ("LobbyManager").GetComponent<LobbyManager> ().playScene = "Prototype";
	}

	public void OnMap2(){
		GameObject.Find ("LobbyManager").GetComponent<LobbyManager> ().playScene = "Prototype_Map_2";
	}

}
