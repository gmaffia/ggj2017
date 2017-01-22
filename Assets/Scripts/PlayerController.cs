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
	private GameManager gameManager;
	// Use this for initialization
	void Start () {
		gameManager = FindObjectOfType<GameManager> ();
		charAnimator = GetComponent<Animator> ();
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
				charAnimator.SetBool ("Walking", true);
				charAnimator.SetBool ("FacingSide",true);
				charAnimator.SetBool ("FacingFront",false);
				charAnimator.SetBool ("FacingBack",false);
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
					//Moving V
					sprite.flipX = false;
					charAnimator.SetBool ("FacingSide", false);
					charAnimator.SetBool ("Walking", true);
					if (transform.position.y > lastPostion.y) {
						//Moving Up
						charAnimator.SetBool ("FacingFront", false);
						charAnimator.SetBool ("FacingBack", true);
					}
					if (transform.position.y < lastPostion.y) {
						//Moving Down
						charAnimator.SetBool ("FacingFront", true);
						charAnimator.SetBool ("FacingBack", false);
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
		if (isLocalPlayer && (tag == "Alice" || tag == "Bob") && Input.GetKeyDown(KeyCode.Space))
        {
            CmdDisclosePosition();
        }
		if (isServer) {
			//checkWinConditions ();
		}
	
    }
	void OnCollisionEnter2D(Collision2D coll) {
		if (isServer) {
			string collidedTo = coll.gameObject.tag;
			if (tag == "Alice" || tag == "Bob") {
				if (collidedTo == "Alice" || collidedTo == "Bob") {
					FindObjectOfType<GameManager> ().RpcHeroesWin ();
					Debug.Log ("Heroes Win");
				}
			}
			if (tag == "Slasher") {
				if (collidedTo == "Alice" || collidedTo == "Bob") {
					FindObjectOfType<GameManager> ().RpcSlasherWin ();
				}
			}
		}

	}
    [Command]
    void CmdDisclosePosition()
    {
        // Spawn a sprite on current location
        GameObject marker = (GameObject)Instantiate(
            markerPrefab,
			transform.position + Vector3.forward*-2f,
            transform.rotation
        );

        //marker.GetComponent<CharacterIdentifier>().playerId = GetComponent<CharacterIdentifier>().playerId;

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