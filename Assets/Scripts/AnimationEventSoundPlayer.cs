using UnityEngine;
using System.Collections;

public class AnimationEventSoundPlayer : MonoBehaviour {

    private GameObject soundBank;

    // Use this for initialization
    void Start()
    {
        this.soundBank = GameObject.Find("WwiseGlobal").GetComponent<AkBank>().gameObject;
    }


    public void play(string playEvent)
    {
        if (playEvent != "")
        {
            AkSoundEngine.PostEvent(playEvent, this.soundBank);
        }
    }

    public void addSoundEvent(int animationClipIndex, float when, string playEvent)
    {
        Debug.Log("Adding Sound Event" + when.ToString() + " " + playEvent);
        Animator anim = gameObject.GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Using Animation Sond Event Player found no animation attached to GameObject");
            return;
        }

        AnimationClip clip = anim.runtimeAnimatorController.animationClips[animationClipIndex];
        Debug.Log("Setting for clip " + clip.name);
        if (clip == null)
        {
            Debug.LogError("Using Animation Sonud Event Player found no animation clip at specified index");
            return;
        }

        AnimationEvent evt = new AnimationEvent();
        evt.time = when;
        evt.stringParameter = playEvent;
        evt.functionName = "play";

        clip.AddEvent(evt);
    }

}