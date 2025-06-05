using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class QuizQuestion
{
    public string Question;
    public string[] Answers;
    public int CorrectIndex;
}

public class QuizLoader : MonoBehaviour
{
    public TextMeshProUGUI QuestionText;
    public TextMeshProUGUI[] AnswerTexts;
    public Button[] AnswerButtons;
    public Button mainMenuButton;

    public Animator characterAnimator; 

    private List<QuizQuestion> questions = new List<QuizQuestion>();
    private int currentQuestionIndex = 0;
    private Color defaultButtonColor;

    /*void Start()
    {
        LoadQuestionsFromText("questions");
        if (AnswerButtons.Length > 0)
            defaultButtonColor = AnswerButtons[0].GetComponent<Image>().color;

        DisplayCurrentQuestion();
    }*/

    void Start()
    {
        string fileName = GameData.SelectedCategoryFile;
        LoadQuestionsFromText(fileName);

        if (AnswerButtons.Length > 0)
            defaultButtonColor = AnswerButtons[0].GetComponent<Image>().color;

        DisplayCurrentQuestion();
    }


    void LoadQuestionsFromText(string filename)
    {
        TextAsset file = Resources.Load<TextAsset>(filename);
        if (file == null)
        {
            Debug.LogError("Text file not found!");
            return;
        }

        string[] lines = file.text.Split(new[] { "\n", "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
        QuizQuestion currentQuestion = null;

        foreach (string rawLine in lines)
        {
            string line = rawLine.Trim();

            if (line.Length >= 2 && char.IsDigit(line[0]) && line[1] == ')')
            {
                if (currentQuestion != null)
                    questions.Add(currentQuestion);

                currentQuestion = new QuizQuestion();
                currentQuestion.Answers = new string[4];
                currentQuestion.Question = line.Substring(2).Trim();
            }
            else if (line.Length >= 2 && char.IsLetter(line[0]) && line[1] == ')')
            {
                char letter = line[0];
                bool isCorrect = char.IsUpper(letter);
                int index = char.ToLower(letter) - 'a';

                if (currentQuestion != null && index >= 0 && index < 4)
                {
                    currentQuestion.Answers[index] = line.Substring(2).Trim();
                    if (isCorrect)
                        currentQuestion.CorrectIndex = index;
                }
            }
        }

        if (currentQuestion != null)
            questions.Add(currentQuestion);
    }

    public void AnswerButtonClicked(int index)
    {
        StopAllCoroutines(); 
        bool correct = index == questions[currentQuestionIndex].CorrectIndex;

        if (correct)
        {
            SetButtonColor(index, Color.green);
            if (characterAnimator != null)
                characterAnimator.SetTrigger("Correct"); 

            StartCoroutine(ProceedToNextQuestionAfterDelay(1f));
        }
        else
        {
            SetButtonColor(index, Color.red);
            if (characterAnimator != null)
                characterAnimator.SetTrigger("Wrong"); 

            StartCoroutine(ShowWrongAnswerMessage());
        }
    }

    IEnumerator ProceedToNextQuestionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentQuestionIndex++;

        if (currentQuestionIndex < questions.Count)
        {
            DisplayCurrentQuestion();
        }
        else
        {
            QuestionText.text = "Testi Tamamladın!";

            if (characterAnimator != null)
                characterAnimator.SetTrigger("Finish"); 

            foreach (var btn in AnswerButtons)
                btn.gameObject.SetActive(false);

            mainMenuButton.gameObject.SetActive(true);
        }
    }

    IEnumerator ShowWrongAnswerMessage()
    {
        string originalText = questions[currentQuestionIndex].Question;
        QuestionText.text = "Yanlış cevap! Bir daha dene.";
        yield return new WaitForSeconds(1.3f);
        QuestionText.text = originalText;
        ResetButtonColors();
    }

    void DisplayCurrentQuestion()
    {
        ResetButtonColors();
        QuizQuestion q = questions[currentQuestionIndex];
        QuestionText.text = q.Question;

        for (int i = 0; i < 4; i++)
        {
            AnswerTexts[i].text = q.Answers[i];
        }
    }

    void SetButtonColor(int index, Color color)
    {
        AnswerButtons[index].GetComponent<Image>().color = color;
    }

    void ResetButtonColors()
    {
        foreach (var btn in AnswerButtons)
        {
            btn.GetComponent<Image>().color = defaultButtonColor;
        }
    }
}
