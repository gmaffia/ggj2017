using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterController : NetworkBehaviour
{
    public GameObject markerPrefab;

	// Use this for initialization
	void Start () {
		
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

        NetworkServer.Spawn(marker);
    }
}
