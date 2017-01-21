using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    public GameObject markerPrefab;

	// Use this for initialization
	void Start () {
        if(isServer)
        {
            CmdJoinPlayer();
        }
        
    }
	
	// Update is called once per frame
	void Update () {

        if (!isLocalPlayer)
        {
            return;
        }

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 3.0f;
        var y = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Translate(x, y, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdDisclosePosition();
        }
    }

    public override void OnStartLocalPlayer()
    {
        Debug.Log("Player ID is " + GetComponent<CharacterIdentifier>().playerId);
        GetComponent<SpriteRenderer>().material.color = Color.blue;
    }


    [Command]
    void CmdDisclosePosition()
    {
        // Spawn a sprite on current location
        GameObject marker = (GameObject)Instantiate(
            markerPrefab,
            transform.position,
            transform.rotation
        );

        marker.GetComponent<CharacterIdentifier>().playerId = GetComponent<CharacterIdentifier>().playerId;

        NetworkServer.Spawn(marker);
    }

    [Command]
    void CmdJoinPlayer()
    {
        GetComponent<CharacterIdentifier>().playerId = GameObject.FindObjectOfType<GameManager>().registerPlayer();
    }
}
