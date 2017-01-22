using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterIdentifier : NetworkBehaviour
{

	[SyncVar(hook="OnCharChange")]
    public string playerId;

    public int lastSet = 0;

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

        AnimationEventSoundPlayer animationSoundPlayer = gameObject.GetComponentInChildren<AnimationEventSoundPlayer>();
        GameObject soundBank = GameObject.Find("WwiseGlobal").GetComponent<AkBank>().gameObject;
        if (isLocalPlayer)
        {            
            Debug.Log("Animation event player?" + animationSoundPlayer.ToString());
            Debug.Log("Locally, the player is " + playerId);
            lastSet++;
            AkSoundEngine.SetState("player" + lastSet.ToString() + "_state", "is" + playerId);            
            AkSoundEngine.PostEvent("play_ambient", soundBank);

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

            // Are there players that are here before me?
            SyncListString roster = FindObjectOfType<GameManager>().players;
            for(int i = 0; i < roster.Count; i++)
            {
                Debug.Log("Adding sounds for Already logged in player " + roster[i]);
                if(roster[i] != playerId)
                {
                    lastSet++;
                    AkSoundEngine.SetState("player" + lastSet.ToString() + "_state", "is" + roster[i]);                    
                    if (animationSoundPlayer != null)
                    {
                        for (int j = 0; j < animationClips.Length; j++)
                        {
                            animationSoundPlayer.addSoundEvent(animationClips[i], 0.2f, "player" + lastSet.ToString() + "_step");
                            animationSoundPlayer.addSoundEvent(animationClips[i], 0.5f, "player" + lastSet.ToString() + "_step");
                            animationSoundPlayer.addSoundEvent(animationClips[i], 0.8f, "player" + lastSet.ToString() + "_step");
                            animationSoundPlayer.addSoundEvent(animationClips[i], 1.0f, "player" + lastSet.ToString() + "_step");
                        }
                    }
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
}
