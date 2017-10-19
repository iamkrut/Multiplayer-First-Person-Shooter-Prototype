using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class PlayerColorHook: LobbyHook {

	public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer){

		LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer> ();
		PlayerColorScript player = gamePlayer.GetComponent<PlayerColorScript> ();

		player.color = lobby.playerColor;

		if (player.color == Color.red) {
			player.GetComponent<Player> ().Team = 1;
		} else {
			player.GetComponent<Player> ().Team = 0;
		}
	}
}
