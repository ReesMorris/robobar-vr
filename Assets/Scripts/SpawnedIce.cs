using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedIce : MonoBehaviour {

    Transform spatula;

    void Start() {
        spatula = GameObject.FindGameObjectWithTag("BarSpoon").transform;
    }

    void Update() {
        //transform.position = spatula.transform.position;
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.name != "Bar Spoon" && collision.gameObject.name != "Glass" && collision.gameObject.name != "SpawnedIce") {
            StartCoroutine(Despawn());
        }
    }

    IEnumerator Despawn() {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}