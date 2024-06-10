using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TeamPoints : MonoBehaviour
{
    public TMP_InputField input;
    public TMP_Text teamNumberText;
    public int teamNumber;
    // Start is called before the first frame update
    void Start()
    {
        teamNumberText.text = "Team " + teamNumber + ":";
    }

    public void ChangePoints()
    {
        QuestionManager.Instance.ChangePoints(teamNumber, input.text);
        QuestionManager.Instance.RefreshPoints();
    }

    public void RefreshPoints(string points)
    {
        input.text = points;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
