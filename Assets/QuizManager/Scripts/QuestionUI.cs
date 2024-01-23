using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

class QuestionUI : MonoBehaviour
{
    public UIDocument UIDocument;
    [SerializeField] private QuizManager QuizManager;
    public VisualElement Root { get => UIDocument.rootVisualElement; }
    public event EventHandler<bool> OnAnswerSelected;
    private bool QuestionRuning = false;
    private readonly string[] difficultiesPtBr = { "Fácil", "Média", "Difícil" };
    private readonly int[] timesForDifficulties = { 15, 30, 60 };

    private void Start()
    {
        Hide();
        QuizManager.OnChoseQuestion += (sender, question) => Render(question);
    }

    private void Update()
    {
        if (QuestionRuning) UpdateTimer(Time.deltaTime);
    }

    private void Render(Question question)
    {
        PrepareQuestion(question.question);
        PrepareAnswers(question.answers);
        PrepareDifficulty(question.difficulty);
        PrepareTimer(question.difficulty);
        Show();
    }

    private void PrepareQuestion(string questionText)
    {
        Label questionElement = Root.Q<Label>("QuestionLabel");
        questionElement.text = questionText;
    }

    private void PrepareAnswers(List<Answer> answers)
    {
        foreach ((string answerNumber, Answer answer) in answers.Select((answer, index) => ((index + 1).ToString(), answer)))
        {
            Button answerButton = Root.Q<Button>("AnswerButton" + answerNumber);
            TextElement buttonText = answerButton.GetFirstOfType<TextElement>();
            buttonText.text = answer.text;
            buttonText.userData = answer.correct;
            answerButton.clickable.clicked += () =>
            {
                OnAnswerSelected?.Invoke(this, answer.correct);
                Hide();
            };
        }
    }

    private void PrepareDifficulty(int difficulty)
    {
        Label questionDifficulty = Root.Q<Label>("DifficultyLabel");
        questionDifficulty.text = difficultiesPtBr[difficulty];
        questionDifficulty.ClearClassList();
        questionDifficulty.AddToClassList("question-difficulty");
        questionDifficulty.AddToClassList($"{(Difficulty)difficulty}-difficulty");
    }

    private void PrepareTimer(int difficulty)
    {
        ProgressBar progressBar = Root.Q<ProgressBar>("TimerBar");
        progressBar.highValue = timesForDifficulties[difficulty];
        progressBar.value = timesForDifficulties[difficulty];

    }

    private void UpdateTimer(float timeElapsedInSecconds)
    {
        ProgressBar progressBar = Root.Q<ProgressBar>("TimerBar");
        progressBar.value -= timeElapsedInSecconds;
        progressBar.title = $"Tempo Restante: {(int)progressBar.value}s";
        if (progressBar.value <= 0)
        {
            Debug.Log("Tempo excedido");
            Hide();
        }
    }

    public void Show()
    {
        Root.style.display = DisplayStyle.Flex;
        QuestionRuning = true;
    }

    public void Hide()
    {
        Root.style.display = DisplayStyle.None;
        QuestionRuning = false;
    }
}