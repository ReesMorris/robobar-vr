using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveCharacter : MonoBehaviour {

    public string[] animations;
    public Vector2 minMaxDelay;

    Animator animator;
    Character character;

	void Start () {
        character = GetComponent<Character>();
        animator = GetComponent<Animator>();
        PlayRandomAnimation();
        StartCoroutine(Animate());
	}
	
	IEnumerator Animate() {
        while(true) {
            AnimatorClipInfo[] currentClip = animator.GetCurrentAnimatorClipInfo(0);
            if (currentClip[0].clip.name == "Character") { // CODE TAKEN FROM [https://docs.unity3d.com/ScriptReference/Animator.GetCurrentAnimatorClipInfo.html]; accessed 9 December 2018
                yield return new WaitForSeconds(Random.Range(1f, 6f));
                PlayRandomAnimation();
            }
            yield return new WaitForSeconds(1f);
        }
    }

    void PlayRandomAnimation() {
        character.Recolour();
        animator.StopPlayback();
        animator.Play(animations[Random.Range(0, animations.Length)]);
    }
}
