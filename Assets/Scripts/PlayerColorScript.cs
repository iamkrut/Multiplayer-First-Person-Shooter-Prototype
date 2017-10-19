using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PlayerColorScript : NetworkBehaviour {
	[SyncVar]
	public Color color;

	MeshRenderer[] rends;

	void Start(){
		rends = GetComponentsInChildren<MeshRenderer> ();
		foreach (MeshRenderer rend in rends) {
			rend.material.color = color;
		}
	}
}
