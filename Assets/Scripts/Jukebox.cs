using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jukebox : MonoBehaviour {

    public AudioSource audio;
    public string[] faces;
    public string[] hitMessages;
    public string[] offlineMessages;
    public Text facesUI;
    public Text textUI;

    bool isTalking;
    int timesHit;

    void Start() {
        StartCoroutine(Faces());
    }

    IEnumerator Faces() {
        while(true) {
            if(!isTalking) {
                textUI.text = "";
                facesUI.text = faces[Random.Range(0, faces.Length)];
                yield return new WaitForSeconds(Random.Range(2f, 6f));
            }
            yield return new WaitForSeconds(1f);
        }
    }

    void OnCollisionEnter(Collision collision) {
        Item item = collision.gameObject.GetComponent<Item>();
        if (item != null) {
            item.Respawn();
            if (!isTalking) {
                timesHit++;
                if (timesHit == 3)
                    StartCoroutine(Talk(offlineMessages[Random.Range(0, offlineMessages.Length)], true));
                else
                    StartCoroutine(Talk(hitMessages[Random.Range(0, hitMessages.Length)], false));
            }
        }
    }

    IEnumerator Talk(string message, bool stopMusic) {
        isTalking = true;
        facesUI.text = "";
        for(int i = 0; i <= message.Length; i++) {
            textUI.text = message.Substring(0, i);
            yield return new WaitForSeconds(0.05f);
        }
        if (stopMusic) {
            yield return new WaitForSeconds(2f);
            audio.Stop();
            textUI.text = "";
        } else {
            yield return new WaitForSeconds(2f);
            isTalking = false;
        }
    }
}
