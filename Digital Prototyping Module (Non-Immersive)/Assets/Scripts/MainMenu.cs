using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void LoadQuiz (int buttonIndex) {
        QuizManager.categoryIndex = buttonIndex;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        // [equivalent to] SceneManager.LoadScene("Game");
    }

    public void LoadTutorial () {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        // [equivalent to] SceneManager.LoadScene("Tutorial");
    }
}
