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
    public static int categoryIndex;

    public GameObject[] options;
    public int currentQuestion = 0;
    public GameObject Model;
    public GameObject ReviewModel;
    public Transform SpawnPosition;

    private Vector3 modelPos = new Vector3 (-2.58f, -14.2f, 10);
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

    public Button reviewGarment;
    public Button proceedButton;

    private void Start() {
        Debug.Log(categoryIndex);
        totalQuestions = category[categoryIndex].QnA.Count;
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
            options[i].transform.GetChild(0).GetComponent<Image>().sprite = category[categoryIndex].QnA[currentQuestion].Answers[i];

            if (category[categoryIndex].QnA[currentQuestion].CorrectAnswer == i)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }
    }

    void generateQuestion()
    {
        QuizPanel.SetActive(true);
        ReviewPanel.SetActive(false);

        if (category[categoryIndex].QnA.Count > 0)
        {
            Debug.Log(currentQuestion);

            QuestionTxt.text = category[categoryIndex].QnA[currentQuestion].Question.text;
            Model = Instantiate(category[categoryIndex].QnA[currentQuestion].Model, modelPos, modelRot);
            Model.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            GameObject child = Model.transform.GetChild(0).gameObject;
            child.layer = LayerMask.NameToLayer("ReactToMask1");
            
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
        Destroy(Model);
        QuizPanel.SetActive(false);
        ReviewPanel.SetActive(true);

        Vector3 delta = new Vector3(3, 0, 0);


        Model = Instantiate(category[categoryIndex].QnA[currentQuestion].Model, modelPos-delta, modelRot);
        GameObject modelChild = Model.transform.GetChild(0).gameObject;
        modelChild.layer = LayerMask.NameToLayer("ReactToMask1");


        ReviewTxt.text = category[categoryIndex].QnA[currentQuestion].Review.text;
        ReviewModel = Instantiate(category[categoryIndex].QnA[currentQuestion].reviewModel, modelPos+delta, modelRot);
        ReviewModel.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        GameObject reviewModelChild = ReviewModel.transform.GetChild(0).gameObject;
        reviewModelChild.layer = LayerMask.NameToLayer("ReactToMask2");

        int answerIndex = category[categoryIndex].QnA[currentQuestion].CorrectAnswer;
        reviewGarment.transform.GetChild(0).GetComponent<Image>().sprite = category[categoryIndex].QnA[currentQuestion].Answers[answerIndex];


        
        category[categoryIndex].QnA.RemoveAt(currentQuestion);
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
