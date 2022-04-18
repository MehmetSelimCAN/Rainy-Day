using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour {

    private void Awake() {
        if (PlayerPrefs.GetInt("LastLevel") == 0) {
            PlayerPrefs.SetInt("LastLevel", 1);
        }

        Transform buttonTemplate = transform.Find("levelButtonTemplate");
        buttonTemplate.gameObject.SetActive(false);

        int xIndex = 0;
        int yIndex = 0;

        for (int levelNumber = 1; levelNumber < 15; levelNumber++) {
            Transform buttonTransform = Instantiate(buttonTemplate, transform);
            buttonTransform.gameObject.SetActive(true);

            if (xIndex % 7 == 0 && xIndex != 0) {
                xIndex = 0;
                yIndex++;
            }
            xIndex++;

            float xOffsetAmount = 100f;
            float yOffsetAmount = -125f;
            buttonTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-400f + xOffsetAmount * xIndex, 125f + yOffsetAmount * yIndex);

            buttonTransform.Find("levelNumber").GetComponent<Text>().text = levelNumber.ToString();
            buttonTransform.name = "Level" + levelNumber.ToString();

            if (levelNumber <= PlayerPrefs.GetInt("LastLevel")) {
                buttonTransform.Find("levelNumber").GetComponent<Text>().color = new Color32(0, 129, 204, 255);
            }
            else {
                buttonTransform.Find("levelNumber").GetComponent<Text>().color = Color.black;
            }

            buttonTransform.GetComponent<Button>().onClick.AddListener(() => {
                if (buttonTransform.Find("levelNumber").GetComponent<Text>().color == new Color32(0, 129, 204, 255)) {
                    SceneManager.LoadScene(buttonTransform.name);
                }
            });
        }
    }

}
