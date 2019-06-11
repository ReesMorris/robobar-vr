using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionParticles : MonoBehaviour {

    ParticleSystem particleSystem;

	void Start () {
        particleSystem = GetComponent<ParticleSystem>();
        Destroy(gameObject, particleSystem.main.duration + 1f);
	}
}
