using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Category
{
    public string name;
    public GameObject originalModel;
    public Sprite originalPattern;
    public List<QnA> QnA;
    public GameObject finalModel;
    public Sprite finalPattern;
}
