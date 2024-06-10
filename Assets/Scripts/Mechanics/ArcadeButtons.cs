using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ArcadeButtons : MonoBehaviour
{
    SerialPort stream = new SerialPort("COM5", 9600);
    public BuzzerText text;
    public bool waitForInput = false;
    bool portExists = false;
    public string val = "";
    public int playerTurn = 0;
    public Image[] teamPlates;
    public Image[] teamPlates2;
    public Color[] turnColor;
    public AudioClip buzzerSound;
    public AudioClip readySound;
    public AudioClip rightSound;
    public AudioClip wrongSound;
    // Start is called before the first frame update
    void Start()
    {
        if (SerialPort.GetPortNames().Length > 0)
            portExists = true;
        if (portExists)
        {
            stream.Open();
            stream.ReadTimeout = 1;
            stream.DiscardInBuffer();
        }
        int monitor_count = Display.displays.Length;
        for (int i = 1; i < monitor_count; i++)
        {
            Display.displays[i].Activate();
        }
    }

    public void StartInputRead()
    {
        QuestionManager.Instance.StopTimer();
        playerTurn = 0;
        val = "";
        waitForInput = true;
        text.displayTurnText = true;
        ChangePlateColor(0);
        if (portExists)
            stream.DiscardInBuffer();
    }

    public void ChangePlateColor(int turn)
    {
        for (int i = 0; i < teamPlates.Length; i++)
        {
            teamPlates[i].color = QuestionManager.Instance.defaultPlateColor;
        }
        for (int i = 0; i < teamPlates2.Length; i++)
        {
            teamPlates2[i].color = QuestionManager.Instance.defaultPlateColor;
        }
        if (turn > 0)
        {
            teamPlates[turn - 1].color = turnColor[turn-1];
            teamPlates2[turn - 1].color = turnColor[turn - 1];
        }
    }

    public void ReadTurn(out string returnValue)
    {
        
        if(playerTurn == 0)
        {
            if (waitForInput)
            {
                returnValue = "Waiting for Buzzer";
            }
            else
            {
                returnValue = "";
            }
            return;
        }
        returnValue = "Team " + playerTurn;
        text.displayTurnText = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CorrectAnswer()
    {
        //play correct sound
        QuestionManager.Instance.StopTimer();
        if (rightSound != null)
        {
            QuestionManager.Instance.PlaySound(rightSound);
        }
        if (QuestionManager.Instance.activeQuestion != null)
        {
            //if (playerTurn != 0)
            //{
            //    QuestionManager.Instance.points[playerTurn - 1] = (int.Parse(QuestionManager.Instance.points[playerTurn - 1]) + int.Parse(QuestionManager.Instance.activeQuestion.points)).ToString();
            //    QuestionManager.Instance.RefreshPoints();
            //}
            QuestionManager.Instance.activeQuestion.Continue();
        }
        text.text.text = "";
        waitForInput = false;
        ChangePlateColor(0);
        QuestionManager.Instance.background.color = QuestionManager.Instance.restColor;
        QuestionManager.Instance.ResetQuestionBackground();
    }

    public void WrongAnswer()
    {
        QuestionManager.Instance.QuestionWrongBackground();
        if (wrongSound != null)
        {
            QuestionManager.Instance.PlaySound(wrongSound);
        }
        if (QuestionManager.Instance.activeQuestion != null)
        {
            if (playerTurn != 0)
            {
                QuestionManager.Instance.points[playerTurn - 1] = (int.Parse(QuestionManager.Instance.points[playerTurn - 1]) - int.Parse(QuestionManager.Instance.activeQuestion.points)).ToString();
                QuestionManager.Instance.RefreshPoints();
            }
        }
        StartInputRead();
    }
    
    public void SkipAnswer()
    {
        QuestionManager.Instance.StopTimer();
        text.text.text = "";
        waitForInput = false;
        if (QuestionManager.Instance.activeQuestion != null)
            QuestionManager.Instance.activeQuestion.Continue();
        ChangePlateColor(0);
        QuestionManager.Instance.background.color = QuestionManager.Instance.restColor;
        QuestionManager.Instance.ResetQuestionBackground();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.F5))
        {
            RestartGame();
        }
        if (Input.GetKey(KeyCode.Space) && waitForInput == false)
        {
            StartInputRead();
        }
        if (Input.GetKey(KeyCode.Backspace))
        {
            SkipAnswer();
        }
        if (waitForInput)
        {
            QuestionManager.Instance.background.color = QuestionManager.Instance.readyColor;
            if (portExists)
                val = stream.ReadLine();
            if(val != "")
            {
                QuestionManager.Instance.TimeQuestion();
                if (buzzerSound != null)
                {
                    GetComponent<AudioSource>().clip = buzzerSound;
                    GetComponent<AudioSource>().Play();
                }
                Debug.Log(val);
                int numval = int.Parse(val);
                if (numval > 999)
                {
                    playerTurn = 1;
                }
                else if (numval > 99)
                {
                    playerTurn = 2;
                }
                else if (numval > 9)
                {
                    playerTurn = 3;
                }
                else if (numval > 0)
                {
                    playerTurn = 4;
                }
                ChangePlateColor(playerTurn);
                if (portExists)
                    stream.DiscardInBuffer();
                waitForInput = false;
            }
        }
    }

    void OnDestroy()
    {
        if (portExists)
        {
            stream.Close();
            stream.Dispose();
        }
    }
}