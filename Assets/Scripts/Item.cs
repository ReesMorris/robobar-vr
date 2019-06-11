using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public bool canSmash;
    public GameObject collisionEffect;

    Quaternion startRot;
    Rigidbody rigidbody;
    ItemManager itemManager;
    GameObject respawnLoc;

    void Start() {
        respawnLoc = new GameObject(gameObject.name + " Spawner");
        respawnLoc.transform.parent = GameObject.Find("World").transform;
        respawnLoc.transform.rotation = gameObject.transform.rotation;
        respawnLoc.transform.position = gameObject.transform.position;
        startRot = transform.rotation;
        rigidbody = GetComponent<Rigidbody>();
        ItemManager.RespawnItems += Respawn;
    }

    public void Respawn() {
        rigidbody.Sleep();
        transform.position = respawnLoc.transform.position;
        transform.rotation = startRot;

        // Glass
        Glass glass = GetComponent<Glass>();
        if (glass != null) {
            glass.Empty(0f);
            glass.SetIce(0f);
            glass.lemonSlice.SetActive(false);
            glass.limeSlice.SetActive(false);
        }
    }

    void Update() {
        // Just incase it falls out of the world
        if(transform.position.y < -10) {
            Respawn();
        }
    }

    void Smash() {
        if (canSmash) {
            Instantiate(collisionEffect, transform.position, Quaternion.identity);
            Respawn();
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (rigidbody == null)
            rigidbody = GetComponent<Rigidbody>();
        if(rigidbody.velocity.magnitude > 1.6f) {
            Smash();
        }
    }
}