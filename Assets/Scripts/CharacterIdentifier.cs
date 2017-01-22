using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterIdentifier : NetworkBehaviour
{

	[SyncVar(hook="OnCharChange")]
    public string playerId;
	void OnCharChange(string newPlayerId){
		Debug.Log ("->>>"+newPlayerId);
		playerId = newPlayerId;

		if(isLocalPlayer)
		{
			FindObjectOfType<GameManager>().currentPlayer = newPlayerId;
		}

		Animator charAnimator = GetComponent<Animator> ();
		//FindObjectOfType<GameManager> ().currentPlayer
		switch (playerId) {
			case ("Alice"):
				charAnimator.SetTrigger ("Alice");
				break;
			case ("Bob"):
				charAnimator.SetTrigger ("Bob");
				break;
			case ("Slasher"):
				charAnimator.SetTrigger ("Slasher");
				break;
		}
	}
}
