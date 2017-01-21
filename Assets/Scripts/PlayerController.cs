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

	private Animator charAnimator;

	// Use this for initialization
	void Start () {
		sprite = GetComponent<SpriteRenderer> ();
		rBody = GetComponent<Rigidbody2D> ();
        if (isServer)
        {
            CmdJoinPlayer();
            RpcSetSpawnPoint(GetComponent<CharacterIdentifier>().playerId);
        }

        if(isLocalPlayer)
        {
            //GetComponent<SpriteRenderer>().material.color = Color.blue;
            FindObjectOfType<GameManager>().currentPlayer = GetComponent<CharacterIdentifier>().playerId;
        }

		charAnimator = GetComponent<Animator> ();
		switch (FindObjectOfType<GameManager> ().currentPlayer) {
			case ("Alice"):
				charAnimator.SetBool("Alice",true);
			break;
			case ("Bob"):
				charAnimator.SetBool("Bob",true);
			break;
			case ("Slasher"):
				charAnimator.SetBool("Slasher",true);
			break;
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
	enum facingDirection {Front,Left,Right,Up}
	Vector3 lastPostion = Vector3.zero;
	SpriteRenderer sprite;
	void Animate(){
		if (isLocalPlayer) {
			float x = Input.GetAxis("Horizontal");
			float y = Input.GetAxis("Vertical");
			if (x!=0) {				
				charAnimator.SetBool ("Walking", true);
				charAnimator.SetBool ("FacingSide",true);
				charAnimator.SetBool ("FacingFront",false);
				charAnimator.SetBool ("FacingBack",false);
				if (x < 0) {
					sprite.flipX = true;
				} else {
					sprite.flipX = false;
				}
			} else {
				if (y != 0) {
					sprite.flipX = false;
					charAnimator.SetBool ("FacingSide", false);
					charAnimator.SetBool ("Walking", true);
					if (y > 0) {
						charAnimator.SetBool ("FacingFront", false);
						charAnimator.SetBool ("FacingBack", true);
					} else {
						charAnimator.SetBool ("FacingFront", true);
						charAnimator.SetBool ("FacingBack", false);
					}
				} else {
					charAnimator.SetBool ("Walking", false);
				}
			}
		} else {
			if (transform.position.x != lastPostion.x) {
				//Moving H
				if (transform.position.x > lastPostion.x) {
					sprite.flipX = true;
					//Moving left
				}
				if (transform.position.x < lastPostion.x) {
					sprite.flipX = false;
					//MovingRight
				}
			} else {
				if (transform.position.y != lastPostion.y) {
					sprite.flipX = false;
					//Moving V
					if (transform.position.y > lastPostion.y) {
						//Moving Up
					}
					if (transform.position.y < lastPostion.y) {
						//Moving Down
					}
				} else {
					//Not Moving
				}
			}
		}
	}
	// Update is called once per frame
	void Update () {
		Animate ();
		if (isLocalPlayer && Input.GetKeyDown(KeyCode.Space))
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