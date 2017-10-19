using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;

public class TimerScript : NetworkBehaviour {

	public static TimerScript instance;

	public Text timerText;
	public bool showGameOverCanvas;

	public float msToWait = 5000.0f;

	private ulong initialTimerValue;

	private ulong diff;
	private ulong m;

	private float secondsLeft;

	[SyncVar(hook="RpcUpdateTimer")]
	private string r;

	[SyncVar(hook="RpcShowGameOverScreen")]
	private bool gameOver;

	public bool GameOver{
		set{
			gameOver = value;
		}
		get{
			return gameOver;
		}
	}

	public GameObject GameOverPanel;
	public Text GameOverText;

	void Awake(){
		instance = this;
		gameOver = false;
	}

	void Start(){
		gameOver = false;
		Debug.Log ("Timer started");
		initialTimerValue = (ulong) DateTime.Now.Ticks;
	}

	/*
	[ServerCallback]
	void Update(){
		if (!isServer) {
			//Debug.Log ("Timer ret");
			return;
		}

		CmdIsTimerOver ();
	}
	*/

	[Command]
	public void CmdIsTimerOver(){

		//Debug.Log ("RPCTimer started");

		diff = ((ulong) DateTime.Now.Ticks - initialTimerValue);
		m = diff / TimeSpan.TicksPerMillisecond;
		secondsLeft = (float) (msToWait - m) / 1000.0f;

		if (secondsLeft <= 0) {
			//gameOver = true;
			//r = "00:00";
			//timerText.gameObject.SetActive (false);
			//if (ScoreManager.instance.RedScore > ScoreManager.instance.BlueScore) {
			//	GameOverPanel.SetActive (true);
			//} else if (ScoreManager.instance.RedScore > ScoreManager.instance.BlueScore) {
			//	GameOverPanel.SetActive (true);
			//} else {
			//	GameOverPanel.SetActive (true);
			//}
			//RpcUpdateTimer("00:00");
		} else {
			r = "";
			secondsLeft -= ((int)secondsLeft / 3600) * 3600;
			r += ((int) ((double)secondsLeft / 60)).ToString ("00") + ":";
			r += ((int)Math.Ceiling ((double)secondsLeft % 60)).ToString ("00");
			//timerText.text = r;
		}
	}

	//[ClientRpc]
	private void RpcUpdateTimer(string r){
		timerText.text = r;
	}

	public void RpcShowGameOverScreen(bool gameOver){
		showGameOverCanvas = gameOver;
		/*
		GameOverPanel.SetActive (true);

		if (ScoreManager.instance.RedScore > ScoreManager.instance.BlueScore) {
			GameOverText.color = Color.red;
			GameOverText.text = "Red Team Wins";
		} else if (ScoreManager.instance.RedScore < ScoreManager.instance.BlueScore) {
			GameOverText.color = Color.blue;
			GameOverText.text = "Blue Team Wins";
		} else {
			GameOverText.color = Color.white;
			GameOverText.text = "Draw Game";
		}
		*/
	}

	[Command]
	public void CmdTimerZero(){
		if (secondsLeft <= 0) {
			r = "00:00";
			gameOver = true;
		}
	}

	private void OnStartClient(){
		RpcUpdateTimer (r);
		RpcShowGameOverScreen (gameOver);
	}
}
