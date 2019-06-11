using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpawner : MonoBehaviour {

    public int spawnAmount;
    public GameObject icePrefab;
    public Transform barSpoonSpawnPoint;

    void OnTriggerExit(Collider other) {
        if(other.name == "Bar Spoon") {
            for (int i = 0; i < spawnAmount; i++) {
                GameObject ice = Instantiate(icePrefab, barSpoonSpawnPoint.position, Quaternion.identity);
                ice.name = icePrefab.name;
            }
        }
    }


}
