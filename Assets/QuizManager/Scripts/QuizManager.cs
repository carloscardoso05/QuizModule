using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class QuizManager : MonoBehaviour
{
    private Quiz Quiz;
    [SerializeField] private QuestionUI QuestionUI;
    [SerializeField] private QuizzesListUI QuizzesListUI;
    [SerializeField] private QuizProvider QuizProvider;
    public event EventHandler<Question> OnChoseQuestion;

    private void Start()
    {
        QuizProvider.GetQuizzes();
        QuizzesListUI.OnQuizSelected += (sender, quiz) => Quiz = quiz;
        QuestionUI.OnAnswerSelected += HandleAnswer;
    }

    private void HandleAnswer(object sender, bool correct)
    {
        Debug.Log(correct ? "Parabéns, você acertou" : "Você errou");
    }

    
    public void SelectQuiz()
    {
        QuizzesListUI.Show();
    }

    public void SelectQuestion()
    {
        if (Quiz is null) throw new Exception("Nenhum Quiz foi selecionado ainda");
        int index = UnityEngine.Random.Range(0, Quiz.questions.Count);
        Question question = Quiz.questions.Values.ElementAt(index);
        OnChoseQuestion?.Invoke(this, question);
    }

}
