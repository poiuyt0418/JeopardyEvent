using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoardQuestion : MonoBehaviour
{
    public BoardCategory category;
    public QuestionType type;
    public string points;
    public TMP_Text text;
    public CanvasGroup cg;
    QuestionWindow qw, gqw;
    AnswerWindow gaw;
    int pointTest;
    public AudioClip readySound;
    // Start is called before the first frame update
    void Start()
    {
        text.text = points;
        text.text = points.ToString().Replace('_', '\n');
        cg = GetComponent<CanvasGroup>();
        qw = QuestionManager.Instance.questionWindow.GetComponent<QuestionWindow>();
        gqw = QuestionManager.Instance.gmQuestionWindow.GetComponent<QuestionWindow>();
        gaw = QuestionManager.Instance.gmAnswerWindow.GetComponent<AnswerWindow>();
    }

    public void GetQuestion()
    {
        qw.Toggle();
        gqw.Toggle();
        gaw.Toggle();
        QuestionManager.Instance.gmboard.cg.interactable = false;
        gqw.text.text = qw.text.text = QuestionManager.Instance.GetQuestion(type, int.Parse(points) / 100);
        gaw.text.text = QuestionManager.Instance.GetAnswer(type, int.Parse(points) / 100);
        QuestionManager.Instance.buttons.StartInputRead();
        cg.interactable = false;
        QuestionManager.Instance.activeQuestion = this;
        if (readySound != null)
        {
            QuestionManager.Instance.PlaySound(readySound);
        }
    }

    public void Continue()
    {
        qw.Toggle();
        gqw.Toggle();
        gaw.Toggle();
        QuestionManager.Instance.gmboard.cg.interactable = true;
        cg.alpha = 0;
        QuestionManager.Instance.availableQuestions[(int)type][int.Parse(points) / 100 - 1] = false;
        QuestionManager.Instance.activeQuestion = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(QuestionManager.Instance.startQuestionLink && int.TryParse(points, out pointTest))
        {
            if (cg.alpha != 0 && !QuestionManager.Instance.availableQuestions[(int)type][pointTest / 100 - 1])
            {
                cg.alpha = 0;
                cg.blocksRaycasts = false;
                cg.interactable = false;
                if (category != null)
                    category.CheckEmpty();
            }
        }
    }
}
