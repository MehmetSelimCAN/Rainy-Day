using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    private Button playButton;
    private Button levelSelectMenuButton;
    private Button backButton;

    private Transform mainMenu;
    private Transform levelSelectMenu;

    private void Awake() {
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        levelSelectMenuButton = GameObject.Find("LevelSelectButton").GetComponent<Button>();
        backButton = GameObject.Find("BackButton").GetComponent<Button>();

        mainMenu = GameObject.Find("MainMenu").transform;
        levelSelectMenu = GameObject.Find("LevelSelectMenu").transform;

        playButton.onClick.AddListener(() => {
            Play();
        });

        levelSelectMenuButton.onClick.AddListener(() => {
            LevelSelectMenu();
        });

        backButton.onClick.AddListener(() => {
            BackToMenu();
        });

        levelSelectMenu.gameObject.SetActive(false);
    }


    private void Play() {
        SceneManager.LoadScene(PlayerPrefs.GetInt("LastLevel"));
    }

    private void LevelSelectMenu() {
        mainMenu.gameObject.SetActive(false);
        levelSelectMenu.gameObject.SetActive(true);
    }

    private void BackToMenu() {
        levelSelectMenu.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }
}
