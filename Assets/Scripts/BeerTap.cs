using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using Valve.VR.InteractionSystem.Sample;

namespace Valve.VR.InteractionSystem.Sample {
    public class BeerTap : MonoBehaviour {

        public float minRestingPos;
        public float minPouringPos;
        public float maxPouringPos;
        public ParticleSystem particles;
        public Interactable handle;

        [Header("Return after release")]
        public bool autoRotateAfterRelease;
        [Range(0.1f, 1f)] public float rotateBackSpeed;

        CircularDrive circularDrive;
        ParticleSystem.MainModule main;

        void Start() {
            main = particles.main;
            circularDrive = handle.GetComponent<CircularDrive>();
            circularDrive.minAngle = minRestingPos;
            circularDrive.maxAngle = maxPouringPos;
        }

        void Update() {
            if (handle.gameObject.transform.localEulerAngles.x > minPouringPos) {
                if (!particles.isPlaying) {
                    particles.Play();
                    main.simulationSpeed = circularDrive.maxAngle / handle.transform.localEulerAngles.x;
                }
            } else {
                if(particles.isPlaying)
                    particles.Stop();
            }

            if (autoRotateAfterRelease) {
                if (Mathf.Floor(handle.gameObject.transform.eulerAngles.x) != minRestingPos) {
                    handle.transform.eulerAngles += (Vector3.left * rotateBackSpeed);
                }
            }
        }
    }
}