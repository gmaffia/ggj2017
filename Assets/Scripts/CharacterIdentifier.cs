using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterIdentifier : NetworkBehaviour
{

	[SyncVar(hook="OnCharChange")]
    public string playerId;

    public int lastSet = 0;
    public string[] players = new string[3];

	void OnCharChange(string newPlayerId){
		Debug.Log ("->>>"+newPlayerId);
		playerId = newPlayerId;

        if (isLocalPlayer)
        {
            FindObjectOfType<GameManager>().currentPlayer = newPlayerId;
        }
        
        Animator charAnimator = GetComponent<Animator> ();
        //FindObjectOfType<GameManager> ().currentPlayer
        //FindObjectOfType<GameManager> ().currentPlayer
        int[] animationClips = new int[3];
        switch (playerId)
        {
            case ("Alice"):
                charAnimator.SetTrigger("Alice");
				tag = "Alice";
                animationClips = new int[] { 1, 3, 5 };
                break;
            case ("Bob"):
                charAnimator.SetTrigger("Bob");
				tag = "Bob";
                animationClips = new int[] { 8, 10, 12 };
                break;
            case ("Slasher"):
                charAnimator.SetTrigger("Slasher");
				tag = "Slasher";
                animationClips = new int[] { 13, 15, 17 };
                break;
        }

        if (isLocalPlayer)
        {
            Debug.Log("The current roster is");
            FindObjectOfType<GameManager>().printPlayers();
            SyncListString roster = FindObjectOfType<GameManager>().players;

            if (roster.Count == 1 && roster.Contains(playerId))
            {
                Debug.Log("Locally, the player is " + playerId);
                lastSet++;
                setSounds(animationClips, lastSet, playerId);
                GameObject soundBank = GameObject.Find("WwiseGlobal").GetComponent<AkBank>().gameObject;
                AkSoundEngine.PostEvent("play_ambient", soundBank);
            }
            
           
            for(int i = 0; i < roster.Count; i++)
            {                
                if(roster[i] != playerId)
                {
                    Debug.Log("Adding sounds for Already logged in player " + roster[i]);
                    lastSet++;
                    setSounds(animationClips, lastSet, playerId);
                }
            }
        }
        else
        {
            Debug.Log("A remote Player Joined" + newPlayerId);
            Debug.Log("Current Roster");
            FindObjectOfType<GameManager>().printPlayers();
            SyncListString roster = FindObjectOfType<GameManager>().players;
            lastSet++;
            setSounds(animationClips, lastSet, playerId);
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if(!isLocalPlayer)
        {
            Debug.Log("Old->>>" + playerId);
        }        
		UpdateCharacterSkin ();
    }

	public void UpdateCharacterSkin(){
		Animator charAnimator = GetComponent<Animator>();

		switch (playerId)
		{
		case ("Alice"):
			charAnimator.SetTrigger("Alice");
			tag = "Alice";
			break;
		case ("Bob"):
			charAnimator.SetTrigger("Bob");
			tag = "Bob";
			break;
		case ("Slasher"):
			charAnimator.SetTrigger("Slasher");
			tag = "Slasher";
			break;
		}
	}

    private void setSounds(int[] animationClips, int lastSet, string playerId)
    {
        AnimationEventSoundPlayer animationSoundPlayer = gameObject.GetComponentInChildren<AnimationEventSoundPlayer>();
        GameObject soundBank = GameObject.Find("WwiseGlobal").GetComponent<AkBank>().gameObject;
        Debug.Log("Setting sounds for player# " + lastSet.ToString() + " which is " + playerId);
        AkSoundEngine.SetState("player" + lastSet.ToString() + "_state", "is" + playerId);

        if (animationSoundPlayer != null)
        {
            for (int i = 0; i < animationClips.Length; i++)
            {
                animationSoundPlayer.addSoundEvent(animationClips[i], 0.2f, "player" + lastSet.ToString() + "_step");
                animationSoundPlayer.addSoundEvent(animationClips[i], 0.5f, "player" + lastSet.ToString() + "_step");
                animationSoundPlayer.addSoundEvent(animationClips[i], 0.8f, "player" + lastSet.ToString() + "_step");
                animationSoundPlayer.addSoundEvent(animationClips[i], 1.0f, "player" + lastSet.ToString() + "_step");
            }
        }

    }
}
