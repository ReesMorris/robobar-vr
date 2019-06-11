using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour {

    public Transform target;

	void Update () {
        // Lookat by itself was backwards; code below is from [https://answers.unity.com/questions/132592/lookat-in-opposite-direction.html]; accessed December 6, 2018
        transform.LookAt(2 * transform.position - target.position);
    }
}
