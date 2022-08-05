using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InstructionManager : MonoBehaviour
{
    public GameObject[] instructions;
    public Button correctButton;
    public Button[] incorrectButtons;
    public int instructionIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && instructionIndex < 2)
        {
            NextInstruction();
            if (instructionIndex == 2)
            {
                foreach (Button incorrectButton in incorrectButtons)
                {
                    incorrectButton.interactable = true;
                }
            }

        }
    }

    void NextInstruction()
    {
        instructions[instructionIndex].SetActive(false);
        instructions[instructionIndex+1].SetActive(true);
        instructionIndex++;
    }

    public void IncorrectButtonDown()
    {
        correctButton.interactable = true;
        instructions[2].SetActive(false);
        instructions[3].SetActive(true);
    }

    IEnumerator WaitForReview()
    {
        yield return new WaitForSeconds(1);
        instructions[3].SetActive(false);
        instructions[4].SetActive(true);
    }

    public void CorrectButtonDown()
    {
        StartCoroutine(WaitForReview());
    }

    public void toMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
