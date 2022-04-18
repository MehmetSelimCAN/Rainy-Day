using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalDoor : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            if (transform.name == "menuDoor") {
                SceneManager.LoadScene("MainMenu");
            }
            else if (transform.name == "quitDoor") {
                Application.Quit();
            }
        }
    }

}
