using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TeamSelectionCanvasManager : NetworkBehaviour{

	public GameObject teamSelectionCanvas;
	public GameObject playerObject;
	public GameObject playerRed;
	public GameObject playerBlue;
	public GameObject camera;
	public GameObject cameraGun;
	public GameObject soundManager;
	public GameObject inGameUICanvas;

	public GameObject playerModel;

	private bool ifSelected;
	private int no;

	void OnEnable(){
		if (ifSelected) {
			teamSelectionCanvas.SetActive (false);
		}
	}

	// Use this for initialization
	void Awake () {
		ifSelected = false;
		Cursor.visible = true;
		cameraGun.SetActive (false);
		soundManager.SetActive (false);
		inGameUICanvas.SetActive (false);
		playerModel.GetComponent<PlayerControllerPrototype> ().enabled = false;
		playerModel.GetComponent<PlayerMotor> ().enabled = false;
		playerModel.GetComponent<PlayerSetup> ().enabled = false;
		playerModel.GetComponent<PlayerShoot> ().enabled = false;
		playerModel.GetComponent<Player> ().enabled = false;
	}

	[Client]
	public void CmdSelectRed(){
		playerRed.SetActive (true);
		no = 1;
		//CmdShowToOthers ();
		ifSelected = true;
		teamSelectionCanvas.SetActive (false);
		cameraGun.SetActive (true);
		soundManager.SetActive (true);
		inGameUICanvas.SetActive (true);
		playerModel.GetComponent<PlayerControllerPrototype> ().enabled = true;
		playerModel.GetComponent<PlayerMotor> ().enabled = true;
		playerModel.GetComponent<PlayerSetup> ().enabled = true;
		playerModel.GetComponent<PlayerShoot> ().enabled = true;
		playerModel.GetComponent<Player> ().enabled = true;
		camera.GetComponent<crosshairscript> ().enabled = true;
	}

	[Client]
	public void CmdSelectBlue(){
		playerBlue.SetActive (true);
		no = 2;
		//CmdShowToOthers ();
		ifSelected = true;
		teamSelectionCanvas.SetActive (false);
		cameraGun.SetActive (true);
		soundManager.SetActive (true);
		inGameUICanvas.SetActive (true);
		playerModel.GetComponent<PlayerControllerPrototype> ().enabled = true;
		playerModel.GetComponent<PlayerMotor> ().enabled = true;
		playerModel.GetComponent<PlayerSetup> ().enabled = true;
		playerModel.GetComponent<PlayerShoot> ().enabled = true;
		playerModel.GetComponent<Player> ().enabled = true;
		camera.GetComponent<crosshairscript> ().enabled = true;
	}

	[Command]
	public void CmdShowToOthers(){
		Debug.Log ("called");
		GameManager.ShowNewPlayer (playerObject.transform.name, no);
	}
}
