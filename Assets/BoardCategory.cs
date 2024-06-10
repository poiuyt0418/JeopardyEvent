using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BoardCategory : MonoBehaviour
{
    public TMP_Text text;
    public GameObject[] questions;
    public QuestionType type;
    public GameObject questionPrefab;
    public CanvasGroup cg;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ResetQuestions()
    {
        foreach (GameObject question in questions)
        {
            Destroy(question);
        }
        if (QuestionManager.Instance.PossibleGame())
        {
            if (cg == null)
                cg = GetComponent<CanvasGroup>();
            cg.alpha = 1;
            text.text = type.ToString().Replace('_', '\n');
            questions = new GameObject[QuestionManager.Instance.questionCountPerCategory];
            for (int i = 0; i < QuestionManager.Instance.questionCountPerCategory; i++)
            {
                questions[i] = Instantiate(questionPrefab, transform.parent);
                questions[i].GetComponent<BoardQuestion>().points = (i + 1).ToString() + "00";
                questions[i].GetComponent<BoardQuestion>().type = type;
                questions[i].GetComponent<BoardQuestion>().category = this;
            }
        }
    }

    public void CheckEmpty()
    {
        if (QuestionManager.Instance.PossibleGame())
        {
            for (int i = 0; i < questions.Length; i++)
            {
                if (questions[i].GetComponent<CanvasGroup>().alpha > 0)
                {
                    break;
                }
                if (i == questions.Length - 1)
                {
                    cg.alpha = 0;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
