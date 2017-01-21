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
	private Rigidbody2D rBody;
	// Use this for initialization
	void Start () {
		rBody = GetComponent<Rigidbody2D> ();
        if (isServer)
        {
            CmdJoinPlayer();
            RpcSetSpawnPoint(GetComponent<CharacterIdentifier>().playerId);
        }

        if(isLocalPlayer)
        {
            GetComponent<SpriteRenderer>().material.color = Color.blue;
            FindObjectOfType<GameManager>().currentPlayer = GetComponent<CharacterIdentifier>().playerId;
        }
    }

	void FixedUpdate(){

        if (!isLocalPlayer)
        {
            return;
        }

        float x = Input.GetAxis("Horizontal") * Time.deltaTime * this.velocity;
		float y = Input.GetAxis("Vertical") * Time.deltaTime * this.velocity;
		rBody.MovePosition (rBody.position + new Vector2(x, y));
	}

	// Update is called once per frame
	void Update () {

        if (!isLocalPlayer)
        {
            return;
        }

        

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
        GetComponent<CharacterIdentifier>().playerId = GameObject.FindObjectOfType<GameManager>().registerPlayer();
    }

    [ClientRpc]
    void RpcSetSpawnPoint(string playerId)
    {
        if(isLocalPlayer)
        {
            // Get a spawn point
            gameObject.transform.position = GameObject.FindObjectOfType<GameManager>().findSpawnPointByPlayerId(playerId).transform.position;
        }
    }
}