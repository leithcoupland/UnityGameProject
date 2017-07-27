using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using Prototype.NetworkLobby;

public class NetworkLobbyHook : LobbyHook {

	public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject _lobbyPlayer, GameObject gamePlayer){
		LobbyPlayer lobbyPlayer = _lobbyPlayer.GetComponent<LobbyPlayer> ();
		SetupLocalPlayer localPlayer = gamePlayer.GetComponent<SetupLocalPlayer> ();

		localPlayer.playerName = lobbyPlayer.playerName;
		localPlayer.playerColor = lobbyPlayer.playerColor;
	}

}
