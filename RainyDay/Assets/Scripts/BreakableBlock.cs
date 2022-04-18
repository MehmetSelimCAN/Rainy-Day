using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : MonoBehaviour {

    private IEnumerator WaitForDeathEffect() {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().isTrigger = true;
        if (transform.Find("pfDoor") != null) {
            transform.Find("pfDoor").gameObject.SetActive(false);
        }
        transform.Find("breakEffect").GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.collider.tag == "Player") {
            StartCoroutine(WaitForDeathEffect());
        }
    }
}
