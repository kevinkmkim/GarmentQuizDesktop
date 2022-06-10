using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    public List<QnA> QnA;
    public GameObject[] options;
    public int currentQuestion;
    public GameObject Model;

    private Vector3 modelPos = new Vector3 (-5, -4, 50);
    private Quaternion modelRot = Quaternion.Euler(0, 180, 0);
    // private Vector3 modelScale = ();

    public GameObject QuizPanel;
    public GameObject GOPanel;

    // public TextAsset QuestionTxt;
    public TextMeshProUGUI QuestionTxt;
    public TextMeshProUGUI ScoreTxt;
    
    int totalQuestions = 0;
    private int tries = 0;
    public float score;

    private void Start() {
        totalQuestions = QnA.Count;
        GOPanel.SetActive(false);
        generateQuestion();
    }

    public void retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        QuizPanel.SetActive(false);
        GOPanel.SetActive(true);

        ScoreTxt.text = score + "/" + totalQuestions;
    }

    public void correct()
    {
        if (tries == 0)
        {
            score += 1.0f;
        }
        else if (tries == 1)
        {
            score += 0.5f;
        }
        tries = 0;
        QnA.RemoveAt(currentQuestion);
        StartCoroutine(WaitForNext());
    }

    public void wrong()
    {
        if (tries < 2)
        {
            tries += 1;
        }
        else
        {
            tries = 0;
            QnA.RemoveAt(currentQuestion);
            StartCoroutine(WaitForNext());
        }
    }

    void SetAnswers()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<Image>().color = options[i].GetComponent<AnswerScript>().startColor;

            options[i].GetComponent<AnswerScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<Image>().sprite = QnA[currentQuestion].Answers[i];

            if (QnA[currentQuestion].CorrectAnswer == i+1)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }
    }

    void generateQuestion()
    {
        if (QnA.Count > 0)
        {
            // currentQuestion = Random.Range(0,QnA.Count);
            Debug.Log(currentQuestion);

            QuestionTxt.text = QnA[currentQuestion].Question.text;
            Model = Instantiate(QnA[currentQuestion].Model, modelPos, modelRot);
            SetAnswers();
        }
        else
        {
            Debug.Log("Out of Questions");
            GameOver();
        }
    }

    IEnumerator WaitForNext()
    {
        yield return new WaitForSeconds(1);
        Destroy(Model);
        generateQuestion();
    }
}
