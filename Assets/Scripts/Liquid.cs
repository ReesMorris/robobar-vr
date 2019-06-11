using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : MonoBehaviour {

    public string name;
    public Color colour;

    ParticleSystem.MainModule main;

    void Start() {
        Color full = new Color(colour.r, colour.g, colour.b, 1);
        main = GetComponent<ParticleSystem>().main;
        main.startColor = full;
    }
}
