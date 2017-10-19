using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MuzzleFlashControllerScript : NetworkBehaviour {

	private float age;
	private float muzzleFlashLifetime = 1;

	[ServerCallback]
	void Update () {
		age += Time.deltaTime;
		if (age > muzzleFlashLifetime) {
			NetworkServer.Destroy (gameObject);
		}
	}
}
