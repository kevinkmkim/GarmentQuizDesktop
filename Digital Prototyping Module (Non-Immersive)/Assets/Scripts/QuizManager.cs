using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    public List<Category> category;
    public int category_index;

    // public List<QnA> QnA;
    public GameObject[] options;
    public int currentQuestion = 0;
    public GameObject Model;
    public GameObject ReviewModel;
    public Transform SpawnPosition;

    private Vector3 modelPos = new Vector3 (-5, -4, 50);
    private Quaternion modelRot = Quaternion.Euler(0, 180, 0);
    // private Vector3 modelScale = ();

    public GameObject QuizPanel;
    public GameObject ReviewPanel;
    public GameObject GOPanel;

    // public TextAsset QuestionTxt;
    public TextMeshProUGUI QuestionTxt;
    public TextMeshProUGUI ReviewTxt;
    public TextMeshProUGUI ScoreTxt;
    
    int totalQuestions = 0;
    private int tries = 0;
    public float score;

    public Button proceedButton;

    private void Start() {
        totalQuestions = category[category_index].QnA.Count;
        GOPanel.SetActive(false);
        generateQuestion();
    }

    public void toMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
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

        StartCoroutine(WaitForReview());
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
            StartCoroutine(WaitForReview());
        }
    }

    void SetAnswers()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<Image>().color = options[i].GetComponent<AnswerScript>().startColor;

            options[i].GetComponent<AnswerScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<Image>().sprite = category[category_index].QnA[currentQuestion].Answers[i];

            if (category[category_index].QnA[currentQuestion].CorrectAnswer == i+1)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }
    }

    void generateQuestion()
    {
        QuizPanel.SetActive(true);
        ReviewPanel.SetActive(false);

        if (category[category_index].QnA.Count > 0)
        {
            Debug.Log(currentQuestion);

            QuestionTxt.text = category[category_index].QnA[currentQuestion].Question.text;
            Model = Instantiate(category[category_index].QnA[currentQuestion].Model, SpawnPosition.position, modelRot);
            SetAnswers();
        }
        else
        {
            Debug.Log("Out of Questions");
            GameOver();
        }
    }


    void generateReview()
    {
        QuizPanel.SetActive(false);
        ReviewPanel.SetActive(true);

        ReviewTxt.text = category[category_index].QnA[currentQuestion].Review.text;
        ReviewModel = Instantiate(category[category_index].QnA[currentQuestion].reviewModel, modelPos, modelRot);
        
        category[category_index].QnA.RemoveAt(currentQuestion);
    }

    IEnumerator WaitForReview()
    {
        yield return new WaitForSeconds(1);
        generateReview();
    }

    public void Proceed()
    {
        Destroy(Model);
        Destroy(ReviewModel);
        generateQuestion();
    }
}
