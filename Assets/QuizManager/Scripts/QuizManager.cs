using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class QuizManager : MonoBehaviour
{
    private Quiz Quiz;
    [SerializeField] private QuestionUI QuestionUI;
    [SerializeField] private QuizzesListUI QuizzesListUI;
    public event EventHandler<Question> OnChoseQuestion;
    private List<string>[] availableQuestions = new List<string>[3];

    private void Start()
    {
        QuizzesListUI.OnQuizSelected += (sender, quiz) =>
        {
            Quiz = quiz;
            for (int i = 0; i < 3; i++)
            {
                availableQuestions[i] = Quiz.questions.Keys.Where((id) => Quiz.questions[id].difficulty == i).ToList();
            }
        };
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

    public void SelectQuestion(int difficulty)
    {
        if (Quiz is null) throw new Exception("Nenhum Quiz foi selecionado ainda");
        List<string> difficultyQuestions = availableQuestions[difficulty];
        if (difficultyQuestions.Count == 0)
        {
            difficultyQuestions = Quiz.questions.Keys.Where((id) => Quiz.questions[id].difficulty == difficulty).ToList();
            Debug.Log("Resetado difficuldade " + difficulty);
        }
        int index = UnityEngine.Random.Range(0, difficultyQuestions.Count);
        string questionId = difficultyQuestions.ElementAt(index);
        Question question = Quiz.questions[questionId];
        difficultyQuestions.RemoveAt(index);

        OnChoseQuestion?.Invoke(this, question);
    }

}
