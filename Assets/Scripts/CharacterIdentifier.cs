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
                animationClips = new int[] { 1, 3, 5 };
                break;
            case ("Bob"):
                charAnimator.SetTrigger("Bob");
                animationClips = new int[] { 8, 10, 12 };
                break;
            case ("Slasher"):
                charAnimator.SetTrigger("Slasher");
                animationClips = new int[] { 13, 15, 17 };
                break;
        }

        AnimationEventSoundPlayer animationSoundPlayer = gameObject.GetComponentInChildren<AnimationEventSoundPlayer>();
        Debug.Log("Animation event player?" + animationSoundPlayer.ToString());
        Debug.Log("Locally, the player is " + playerId);
        AkSoundEngine.SetState("player1_state", "is" + playerId);

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
        Animator charAnimator = GetComponent<Animator>();
        //FindObjectOfType<GameManager> ().currentPlayer
        switch (playerId)
        {
            case ("Alice"):
                charAnimator.SetTrigger("Alice");
                break;
            case ("Bob"):
                charAnimator.SetTrigger("Bob");
                break;
            case ("Slasher"):
                charAnimator.SetTrigger("Slasher");
                break;
        }
    }
}
