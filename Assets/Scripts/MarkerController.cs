using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MarkerController : NetworkBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("Current player is " + FindObjectOfType<GameManager>().currentPlayer + " And  characteridentifier is " + GetComponent<CharacterIdentifier>().playerId);
        if(FindObjectOfType<GameManager>().currentPlayer == GetComponent<CharacterIdentifier>().playerId)
        {
            gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
