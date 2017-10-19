using UnityEngine;
using System.Collections;


public class crosshairscript : MonoBehaviour {
	static public bool center =true;
	static  public bool cursorOff =false;


	public float cursorAdjustY;
	public float cursorAdjustX;
	public Texture2D crosshairTexture;
	public int W;
	public int H;
	public bool on=true;
	/*
	void OnGUI(){
		if(on==true){
			GUI.DrawTexture(new Rect(Screen.width/2+cursorAdjustX, Screen.height/2+cursorAdjustY, W, H), crosshairTexture, ScaleMode.ScaleToFit);

		}
	}
*/
	void OnGUI(){

		GUI.Label(new Rect(Screen.width/2 - 25 ,Screen.height/2 - 25,50,50),crosshairTexture);
	}

	// Update is called once per frame, this will hide your      //original mouse, but still keep its functions. On low fps
	//this will cause mouse flicker.
	void Update () {
		if(TimerScript.instance.showGameOverCanvas){
			return;
		}
		Screen.lockCursor = cursorOff;
		Screen.lockCursor = center;

	}
}
