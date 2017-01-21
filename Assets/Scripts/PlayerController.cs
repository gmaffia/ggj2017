using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    public GameObject markerPrefab;
    public float destroyAfter = 3.0f;
    public float velocity = 3.0f;

    private GameObject currentMarker = null;

	// Use this for initialization
	void Start () {
        if (isServer)
        {
            CmdJoinPlayer();
        }

        Debug.Log("Player ID is " + GetComponent<CharacterIdentifier>().playerId);
        if(isLocalPlayer)
        {
            GetComponent<SpriteRenderer>().material.color = Color.blue;
            FindObjectOfType<GameManager>().currentPlayer = GetComponent<CharacterIdentifier>().playerId;
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (!isLocalPlayer)
        {
            return;
        }

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * this.velocity;
        var y = Input.GetAxis("Vertical") * Time.deltaTime * this.velocity;

        transform.Translate(x, y, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdDisclosePosition();
        }
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

        Destroy(marker, destroyAfter);
    }

    [Command]
    void CmdJoinPlayer()
    {
        Debug.Log("Joining Player Command");
        GetComponent<CharacterIdentifier>().playerId = GameObject.FindObjectOfType<GameManager>().registerPlayer();
    }
}
