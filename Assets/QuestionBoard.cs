using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBoard : MonoBehaviour
{
    public List<BoardCategory> categories;
    public GameObject questionPrefab;
    public List<BoardQuestion> categoriesGM;
    public List<BoardQuestion> questionsGM;
    bool possible;
    // Start is called before the first frame update
    void Start()
    {
        categoriesGM = new List<BoardQuestion>();
        questionsGM = new List<BoardQuestion>();
        ResetBoard();
    }

    public void ResetBoard()
    {
        foreach (BoardCategory category in categories)
        {
            category.ResetQuestions();
        }
        for (int i = categoriesGM.Count - 1; i >= 0; i--)
        {
            Destroy(categoriesGM[i].gameObject);
            categoriesGM.RemoveAt(i);
        }
        for (int i = questionsGM.Count - 1; i >= 0; i--)
        {
            Destroy(questionsGM[i].gameObject);
            questionsGM.RemoveAt(i);
        }
        if (QuestionManager.Instance.PossibleGame())
        {
            List<List<bool>> availableQuestions = QuestionManager.Instance.availableQuestions;
            for (int i = 0; i < categories.Count; i++)
            {
                availableQuestions.Add(new List<bool>());
                categoriesGM.Add(Instantiate(questionPrefab, QuestionManager.Instance.gmCategory.transform).GetComponent<BoardQuestion>());
                categoriesGM[i].type = categories[i].type;
                categoriesGM[i].points = categoriesGM[i].type.ToString();
                categoriesGM[i].GetComponent<CanvasGroup>().interactable = false;
                for (int j = 0; j < QuestionManager.Instance.questionCountPerCategory; j++)
                {
                    questionsGM.Add(Instantiate(questionPrefab, QuestionManager.Instance.gmboard.transform).GetComponent<BoardQuestion>());
                    questionsGM[i * 5 + j].points = (j + 1).ToString() + "00";
                    questionsGM[i * 5 + j].type = categories[i].type;
                    questionsGM[i * 5 + j].category = categories[i];
                    availableQuestions[i].Add(true);
                }
            }
            possible = true;
            QuestionManager.Instance.startQuestionLink = true;
        }
        else
        {
            possible = false;
            Debug.Log("reached max games");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (possible && QuestionManager.Instance.CheckFinish())
        {
            QuestionManager.Instance.gameNumber++;
            QuestionManager.Instance.ResetBoard();
            ResetBoard();
        }
    }
}
