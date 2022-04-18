using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeWriterEffect : MonoBehaviour {

    private float delay = 0.05f;
    [SerializeField] private string fullText;
    [SerializeField] private string currentText = "";

    public void Type() {
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText() {
        for (int i = 0; i < fullText.Length + 1; i++) {
            currentText = fullText.Substring(0, i);
            this.GetComponent<Text>().text = currentText;
            yield return new WaitForSeconds(delay);
        }
    }

}
