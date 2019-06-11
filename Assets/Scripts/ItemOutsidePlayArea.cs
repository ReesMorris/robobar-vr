using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOutsidePlayArea : MonoBehaviour {

    void Start() {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
    }

    void OnCollisionEnter(Collision collision) {
        Item item = collision.gameObject.GetComponent<Item>();
        if(item) {
            item.Respawn();
        }
    }
}
