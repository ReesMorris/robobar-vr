using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class FridgeGlass : MonoBehaviour {

    void OnTriggerEnter(Collider other) {
        Item item = other.gameObject.GetComponent<Item>();
        if(item != null) {
            item.transform.SetParent(GameObject.Find("World").transform);
            item.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
