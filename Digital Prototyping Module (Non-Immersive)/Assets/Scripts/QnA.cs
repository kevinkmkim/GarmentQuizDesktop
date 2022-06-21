using UnityEngine;

[System.Serializable]

public class QnA
{
    public TextAsset Question;
    public TextAsset Review;
    public GameObject Model;
    public GameObject reviewModel;
    public Sprite[] Answers;
    public int CorrectAnswer;
}
