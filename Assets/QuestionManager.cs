using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager instance;
    public Question questions;
    public TextAsset json;
    public GameObject questionWindow;
    public GameObject gmQuestionWindow;
    public GameObject gmAnswerWindow;
    public ArcadeButtons buttons;
    public BoardQuestion activeQuestion;
    public QuestionBoard board;
    public GMQuestionBoard gmboard;
    public GameObject gmCategory;
    public Image background;
    public Color defaultPlateColor;
    public Color restColor;
    public Color readyColor;
    public Color wrongColor;
    public List<List<bool>> availableQuestions;
    public Canvas display;
    public Canvas gmView;
    public TMP_Text timer;
    public TMP_Text gmTimer;
    public Coroutine coroutine;
    public bool coroutineRunning;
    public bool startQuestionLink;
    public bool skipGame;
    public string[] points;
    GameObject[] teamPlates;
    public int totalQuestionCount;
    public int questionCountPerCategory;
    public int maxGames;
    public int gameNumber;
    public int timerDuration;

    public static QuestionManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject().AddComponent<QuestionManager>();
            }

            return instance;
        }
    }

    void LoadQuestion()
    {
        questions = JsonUtility.FromJson<Question>(json.text);
    }

    public void PlaySound(AudioClip clip)
    {
        AudioSource source = GetComponent<AudioSource>();
        source.clip = clip;
        source.Play();
    }

    public void ResetBoard()
    {
        if (activeQuestion != null)
        {
            activeQuestion.Continue();
        }
        if (!skipGame)
        {
            points = new string[] { "0", "0", "0", "0" };
        }
        skipGame = false;
        startQuestionLink = false;
        availableQuestions = new List<List<bool>>();
    }

    public string GetQuestion(QuestionType type, int points)
    {
        return questions.GetQuestion(gameNumber, type, points);
    }

    public string GetAnswer(QuestionType type, int points)
    {
        return questions.GetAnswer(gameNumber, type, points);
    }

    void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        ResetBoard();
        LoadQuestion();
        teamPlates = GameObject.FindGameObjectsWithTag("TeamPlate");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SwapDisplay()
    {
        int temp = display.targetDisplay;
        display.targetDisplay = gmView.targetDisplay;
        gmView.targetDisplay = temp;
    }

    public void QuestionWrongBackground()
    {
        questionWindow.GetComponent<Image>().color = wrongColor;
    }

    public void ResetQuestionBackground()
    {
        questionWindow.GetComponent<Image>().color = restColor;
    }

    public void ChangePoints(int team, string input)
    {
        points[team-1] = input;
    }

    public void RefreshPoints()
    {
        for (int i = 0; i < teamPlates.Length; i++)
        {
            teamPlates[i].GetComponent<TeamPoints>().RefreshPoints(points[teamPlates[i].GetComponent<TeamPoints>().teamNumber - 1]);
        }
    }

    public void SkipGame()
    {
        buttons.SkipAnswer();
        background.color = restColor;
        skipGame = true;
    }

    public bool PossibleGame()
    {
        return gameNumber < maxGames;
    }

    public bool CheckFinish()
    {
        if (skipGame)
        {
            return true;
        }
        for (int i = 0; i < availableQuestions.Count;i++)
        {
            for (int j = 0; j < availableQuestions[i].Count; j++)
            {
                if (availableQuestions[i][j])
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void StopTimer()
    {
        timer.text = "";
        gmTimer.text = "";
        if (coroutineRunning)
        {
            StopCoroutine(coroutine);
            coroutineRunning = false;
        }
    }

    public void TimeQuestion()
    {
        coroutine = StartCoroutine(Timer());
    }

    public IEnumerator Timer()
    {
        coroutineRunning = true;
        for (int i = 0; i < timerDuration; i++)
        {
            timer.text = (timerDuration - i).ToString();
            gmTimer.text = (timerDuration - i).ToString();
            yield return new WaitForSeconds(1);
        }
        gmTimer.text = "";
        coroutineRunning = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public class Question
{
    public string[] questions1;
    public string[] questions2;
    public string[] answers1;
    public string[] answers2;
    public string[] allQuestions;
    public string[] allAnswers;

    public string GetQuestion(int gameNumber, QuestionType type,int points)
    {
        if (allQuestions == null || allQuestions.Length <= 0)
        {
            allQuestions = new string[questions1.Length + questions2.Length];
            for (int i = 0; i < questions1.Length; i++)
            {
                allQuestions[i] = questions1[i];
            }
            for (int i = questions1.Length; i < questions1.Length + questions2.Length; i++)
            {
                allQuestions[i] = questions2[i - questions1.Length];
            }
        }
        return allQuestions[gameNumber * QuestionManager.Instance.totalQuestionCount + (int)type * 5 + points - 1];
    }

    public string GetAnswer(int gameNumber, QuestionType type, int points)
    {
        if (allAnswers == null || allAnswers.Length <= 0)
        {
            allAnswers = new string[answers1.Length + answers2.Length];
            for (int i = 0; i < answers1.Length; i++)
            {
                allAnswers[i] = answers1[i];
            }
            for (int i = answers1.Length; i < answers1.Length + answers2.Length; i++)
            {
                allAnswers[i] = answers2[i - questions1.Length];
            }
        }
        return allAnswers[gameNumber * QuestionManager.Instance.totalQuestionCount + (int)type * 5 + points - 1];
    }
}

public enum QuestionType
{
    Bible, Redemptive_History, American_History, Korean_History, Complete_Lyrics
}