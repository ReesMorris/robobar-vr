using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour {

    public ParticleSystem particles;
	
	void Update () {
        float rot = gameObject.transform.eulerAngles.z;
        if (rot > 100f && rot < 260f) {
            if(!particles.isPlaying)
                particles.Play();
        } else {
            if (particles.isPlaying)
                particles.Stop();
        }
	}
}
