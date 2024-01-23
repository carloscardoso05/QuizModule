using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizManagerTester : MonoBehaviour
{
    private QuizManager QuizManager;
    int difficulty = 0;
    // Start is called before the first frame update
    void Start()
    {   
        QuizManager = GetComponentInChildren<QuizManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Selecionar Quiz (Q)
        if (Input.GetKeyDown(KeyCode.Q)) {
            QuizManager.SelectQuiz();
        }
        //Carregar questão (W)
        if (Input.GetKeyDown(KeyCode.W)) {
            QuizManager.SelectQuestion(difficulty);
            difficulty = (difficulty + 1) % 3;
        }
    }
}
