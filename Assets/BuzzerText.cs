using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BuzzerText : MonoBehaviour
{
    public ArcadeButtons buttons;
    public TMP_Text text;
    public bool displayTurnText = false;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(displayTurnText)
        {
            string value;
            buttons.ReadTurn(out value);
            text.text = value;
        }
    }
}
