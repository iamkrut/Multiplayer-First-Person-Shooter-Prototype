using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ScoreManager : NetworkBehaviour {

	public static ScoreManager instance;

	public Text redScoreText;
	public Text blueScoreText;

	[SyncVar(hook="RpcOnRedScore")]
	private int redScore;
	public int RedScore{
		set{
			redScore = value;
		}
		get{
			return redScore;
		}
	}

	[SyncVar(hook="RpcOnBlueScore")]
	private int blueScore;
	public int BlueScore{
		set{
			blueScore = value;
		}
		get{
			return blueScore;
		}
	}

	void Awake(){
		instance = this;
		redScore = 0;
		blueScore = 0;
	}

	[Command]
	public void CmdUpdateTeamScore(int i){
		if (i == 0) { // blue team player dead
			// increase red score
			//int val = Int32.Parse(GameObject.Find("RedText").GetComponent<Text>().text);
			//val++;
			//GameObject.Find ("RedText").GetComponent<Text> ().text = val.ToString ();
			instance.redScore+=1;
		} else if(i == 1){ // red team
			// increase blue score
			//int val = Int32.Parse(GameObject.Find("BlueText").GetComponent<Text>().text);
			//val++;
			//GameObject.Find ("BlueText").GetComponent<Text> ().text = val.ToString ();
			instance.blueScore+=1;
		}
	}

	//[ClientRpc]
	private void RpcOnRedScore(int score){
		redScoreText.text = score.ToString();
	}

	//[ClientRpc]
	private void RpcOnBlueScore(int score){
		blueScoreText.text = score.ToString();
	}

	private void OnStartClient(){
		RpcOnRedScore (redScore);
		RpcOnBlueScore (blueScore);
	}
}
