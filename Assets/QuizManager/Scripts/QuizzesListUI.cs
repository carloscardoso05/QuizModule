using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

class QuizzesListUI : MonoBehaviour
{
    public UIDocument UIDocument;
    [SerializeField] private QuizManager QuizManager;
    [SerializeField] private QuizProvider QuizProvider;
    public VisualElement Root { get => UIDocument.rootVisualElement; }
    private ListView ListView { get => Root.Q<ListView>("QuizzesList"); }
    public List<Quiz> Quizzes = new();
    public event EventHandler<Quiz> OnQuizSelected;

    private void Start()
    {
        Hide();
        QuizProvider.OnGetQuizzes += LoadQuizzes;
        ListView.selectionType = SelectionType.Single;
        ListView.onItemsChosen += OnQuizChosen;
    }

    private void OnQuizChosen(IEnumerable<object> objects)
    {
        int i = ListView.selectedIndex;
        Quiz quiz = Quizzes[i];
        OnQuizSelected?.Invoke(this, quiz);
        Hide();
    }

    void LoadQuizzes(object sender, Dictionary<string, Quiz> quizzes)
    {
        Quizzes = quizzes.Values.ToList();
        ListView.itemsSource = Quizzes.Select((quiz) => quiz.title).ToList();
    }

    public void Show()
    {
        Root.style.display = DisplayStyle.Flex;
    }
    public void Hide()
    {
        Root.style.display = DisplayStyle.None;
    }
}