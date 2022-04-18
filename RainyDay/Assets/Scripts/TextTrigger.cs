using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTrigger : MonoBehaviour {

    private bool triggered;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player" && !triggered) {
            triggered = true;
            GetComponentInParent<TypeWriterEffect>().Type();
        }
    }


}
