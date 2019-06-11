using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public Renderer[] bodyParts;
    public Renderer[] tieParts;

    void Start() {
        Recolour();
    }

    public void Recolour() {
        // Body
        Color col = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        foreach (Renderer renderer in bodyParts) {
            renderer.material.color = col;
        }
        // Tie
        col = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        foreach (Renderer renderer in tieParts) {
            if (Random.Range(0f, 100f) < 40f)
                renderer.enabled = false;
            else
                renderer.material.color = col;
        }
    }
}
