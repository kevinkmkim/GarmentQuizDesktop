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

    private Vector3[] modelPos = new [] { new Vector3 (-2.58f, -14.2f, 10), new Vector3 (-2.58f, -5.4f, 10), new Vector3 (-2.58f, -12f, 10), new Vector3 (-2.58f, -10.5f, 10), new Vector3 (-2.58f, -9f, 10) };
    private Quaternion modelRot = Quaternion.Euler(0, 180, 0);
    // private Vector3 modelScale = ();

    public GameObject PreviewPanel;
    public GameObject QuizPanel;
    public GameObject ReviewPanel;
    public GameObject GOPanel;

    // public TextAsset QuestionTxt;
    public TextMeshProUGUI QuestionTxt;
    public TextMeshProUGUI ReviewTxt;
    public TextMeshProUGUI ScoreTxt;
    public TextMeshProUGUI PatternTxt;
    
    int totalQuestions = 0;
    private int tries = 0;
    public float score;

    public Button previewGarment;
    public Button reviewGarment;
    public Button proceedButton;

    public AudioSource source;
    public AudioClip correctSoundClip;
    public AudioClip wrongSoundClip;

    private void Start() {
        Debug.Log(categoryIndex);
        totalQuestions = category[categoryIndex].QnA.Count;
        GOPanel.SetActive(false);
        generatePattern("Initial");
    }


    public void toMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void GameOver()
    {
        QuizPanel.SetActive(false);
        PreviewPanel.SetActive(false);
        GOPanel.SetActive(true);

        // Destroy(proceedButton);

        ScoreTxt.text = score + "/" + totalQuestions;
    }

    public void correct()
    {
        source.PlayOneShot(correctSoundClip);

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
        source.PlayOneShot(wrongSoundClip);
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

    void generatePattern(string pattern_type)
    {
        ReviewPanel.SetActive(false);
        QuizPanel.SetActive(false);
        PreviewPanel.SetActive(true);
        if (pattern_type == "Initial")
        {

            PatternTxt.text = "Original Pattern";

            Model = Instantiate(category[categoryIndex].originalModel, modelPos[categoryIndex], modelRot);
            GameObject modelChild = Model.transform.GetChild(0).gameObject;
            modelChild.layer = LayerMask.NameToLayer("ReactToMask1");

            previewGarment.transform.GetChild(0).GetComponent<Image>().sprite = category[categoryIndex].originalPattern;

            var button = proceedButton;
            button.onClick.AddListener(() => startQuiz());
        }
        if (pattern_type == "Final")
        {

            PatternTxt.text = "Final Pattern";

            Model = Instantiate(category[categoryIndex].finalModel, modelPos[categoryIndex], modelRot);
            GameObject modelChild = Model.transform.GetChild(0).gameObject;
            modelChild.layer = LayerMask.NameToLayer("ReactToMask1");

            previewGarment.transform.GetChild(0).GetComponent<Image>().sprite = category[categoryIndex].finalPattern;

            var button = proceedButton;
            button.onClick.AddListener(() => GameOver());
        }
    }

    public void startQuiz()
    {
        // Destroy(proceedButton);
        PreviewPanel.SetActive(false);
        Destroy(Model);
        generateQuestion();
    }

    void generateQuestion()
    {
        ReviewPanel.SetActive(false);
        QuizPanel.SetActive(true);

        if (category[categoryIndex].QnA.Count > 0)
        {
            Debug.Log(currentQuestion);

            QuestionTxt.text = category[categoryIndex].QnA[currentQuestion].Question.text;
            Model = Instantiate(category[categoryIndex].QnA[currentQuestion].Model, modelPos[categoryIndex], modelRot);
            // Model.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            GameObject child = Model.transform.GetChild(0).gameObject;
            child.layer = LayerMask.NameToLayer("ReactToMask1");
            
            SetAnswers();
        }
        else
        {
            Debug.Log("Out of Questions");
            generatePattern("Final");
        }
    }


    void generateReview()
    {
        Destroy(Model);
        QuizPanel.SetActive(false);
        ReviewPanel.SetActive(true);

        Vector3 delta = new Vector3(3, 0, 0);


        Model = Instantiate(category[categoryIndex].QnA[currentQuestion].Model, modelPos[categoryIndex]-delta, modelRot);
        GameObject modelChild = Model.transform.GetChild(0).gameObject;
        modelChild.layer = LayerMask.NameToLayer("ReactToMask1");


        ReviewTxt.text = category[categoryIndex].QnA[currentQuestion].Review.text;
        ReviewModel = Instantiate(category[categoryIndex].QnA[currentQuestion].reviewModel, modelPos[categoryIndex]+delta, modelRot);
        // ReviewModel.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
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

    public void proceed()
    {
        Destroy(Model);
        Destroy(ReviewModel);
        generateQuestion();
    }
}
