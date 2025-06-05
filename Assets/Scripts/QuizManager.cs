using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class QuizManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Button[] answerButtons;
    [SerializeField] private List<Question> questions = new List<Question>();

    private int currentQuestionIndex = 0;
    private bool canAnswer = true;

    private void Start()
    {
        DisplayQuestion();
    }

    private void DisplayQuestion()
    {
        if (currentQuestionIndex < questions.Count)
        {
            canAnswer = true;
            Question q = questions[currentQuestionIndex];
            questionText.text = q.questionText;

            for (int i = 0; i < answerButtons.Length; i++)
            {
                int index = i;
                answerButtons[i].interactable = true;
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = q.answers[i];
                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() => CheckAnswer(index));
                answerButtons[i].GetComponent<Image>().color = Color.white; // Renk sıfırlama
            }
        }
        else
        {
            questionText.text = "Tebrikler! Tüm soruları tamamladınız.";
            foreach (var btn in answerButtons)
                btn.gameObject.SetActive(false);
        }
    }

    private void CheckAnswer(int selectedIndex)
    {
        if (!canAnswer) return;
        canAnswer = false;

        bool isCorrect = questions[currentQuestionIndex].correctAnswerIndex == selectedIndex;
        ColorBlock cb = answerButtons[selectedIndex].colors;

        if (isCorrect)
        {
            answerButtons[selectedIndex].GetComponent<Image>().color = Color.green;
            currentQuestionIndex++;
            StartCoroutine(WaitAndShowNextQuestion(0.5f));
        }
        else
        {
            answerButtons[selectedIndex].GetComponent<Image>().color = Color.red;
            StartCoroutine(ShowWrongAnswerMessage());
        }
    }

    private IEnumerator ShowWrongAnswerMessage()
    {
        string originalText = questionText.text;
        questionText.text = "Yanlış cevap! Lütfen tekrar deneyin.";
        yield return new WaitForSeconds(1f);
        DisplayQuestion();
    }

    private IEnumerator WaitAndShowNextQuestion(float delay)
    {
        yield return new WaitForSeconds(delay);
        DisplayQuestion();
    }
}
