using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialAnswerScript : MonoBehaviour
{
    public bool isCorrect = false;
    public TutorialManager tutorialManager;

    public Color startColor;

    private void Start() 
    {
        startColor = GetComponent<Image>().color;
    }

    public void Answer()
    {
        if (isCorrect)
        {
            GetComponent<Image>().color = Color.green;
            Debug.Log("Correct Answer");
            tutorialManager.correct();
        }
        else
        {
            GetComponent<Image>().color = Color.red;
            Debug.Log("Wrong Answer");
            tutorialManager.wrong();
        }
    }
}
