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
        Debug.Log("Animation event player?" + animationSoundPlayer.ToString());
        Debug.Log("Locally, the player is " + playerId);
        AkSoundEngine.SetState("player1_state", "is" + playerId);
        GameObject soundBank = GameObject.Find("WwiseGlobal").GetComponent<AkBank>().gameObject;
        AkSoundEngine.PostEvent("play_ambient", soundBank);

        if (animationSoundPlayer != null)
        {
            for (int i = 0; i < animationClips.Length; i++)
            {
                animationSoundPlayer.addSoundEvent(animationClips[i], 0.2f, "player1_step");
                animationSoundPlayer.addSoundEvent(animationClips[i], 0.5f, "player1_step");
                animationSoundPlayer.addSoundEvent(animationClips[i], 0.8f, "player1_step");
                animationSoundPlayer.addSoundEvent(animationClips[i], 1.0f, "player1_step");
            }
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log("Old->>>" + playerId);       
		UpdateCharacterSkin ();
        Debug.Log("Current Player roster");
        FindObjectOfType<GameManager>().printPlayers();
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
