using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public List<QnA> QnA;

    public GameObject[] options;
    public int currentQuestion = 0;
    public GameObject Model;
    public GameObject ReviewModel;
    public Transform SpawnPosition;

    private Vector3 modelPos = new Vector3 (0f, -1.2f, 0.5f);
    private Quaternion modelRot = Quaternion.Euler(0, 180, 0);
    // private Vector3 modelScale = ();

    public GameObject QuizPanel;
    public GameObject ReviewPanel;

    // public TextAsset QuestionTxt;
    public TextMeshProUGUI QuestionTxt;
    public TextMeshProUGUI ReviewTxt;
    public TextMeshProUGUI PatternTxt;
    
    private int tries = 0;
    public float score;

    public Button reviewGarment;
    public Button proceedButton;

    public AudioSource source;
    public AudioClip correctSoundClip;
    public AudioClip wrongSoundClip;

    private void Start() {
        ReviewPanel.SetActive(false);
        QuizPanel.SetActive(true);
        generateQuestion();
    }

    public void toMainMenu()
    {
        SceneManager.LoadScene("Menu");
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
            options[i].GetComponent<Image>().color = options[i].GetComponent<TutorialAnswerScript>().startColor;

            options[i].GetComponent<TutorialAnswerScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<Image>().sprite = QnA[currentQuestion].Answers[i];

            if (QnA[currentQuestion].CorrectAnswer == i)
            {
                options[i].GetComponent<TutorialAnswerScript>().isCorrect = true;
            }
        }
    }

    void generateQuestion()
    {
        QuizPanel.SetActive(true);

        if (QnA.Count > 0)
        {
            Debug.Log(currentQuestion);

            QuestionTxt.text = QnA[currentQuestion].Question.text;
            Model = Instantiate(QnA[currentQuestion].Model, modelPos, modelRot);
            // Model.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            GameObject child = Model.transform.GetChild(0).gameObject;
            child.layer = LayerMask.NameToLayer("ReactToMask1");
            
            SetAnswers();
        }
        else
        {
            Debug.Log("Out of Questions");
            toMainMenu();
        }
    }
    void generateReview()
    {
        Destroy(Model);
        QuizPanel.SetActive(false);
        ReviewPanel.SetActive(true);

        Vector3 delta = new Vector3(3, 0, 0);


        Model = Instantiate(QnA[currentQuestion].Model, modelPos-delta, modelRot);
        GameObject modelChild = Model.transform.GetChild(0).gameObject;
        modelChild.layer = LayerMask.NameToLayer("ReactToMask1");


        ReviewTxt.text = QnA[currentQuestion].Review.text;
        ReviewModel = Instantiate(QnA[currentQuestion].reviewModel, modelPos+delta, modelRot);
        // ReviewModel.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        GameObject reviewModelChild = ReviewModel.transform.GetChild(0).gameObject;
        reviewModelChild.layer = LayerMask.NameToLayer("ReactToMask2");

        int answerIndex = QnA[currentQuestion].CorrectAnswer;
        reviewGarment.transform.GetChild(0).GetComponent<Image>().sprite = QnA[currentQuestion].Answers[answerIndex];
        
        QnA.RemoveAt(currentQuestion);
    }

    IEnumerator WaitForReview()
    {
        yield return new WaitForSeconds(1);
        generateReview();
    }

    public void proceed()
    {
        Destroy(Model);
        generateQuestion();
    }
}
