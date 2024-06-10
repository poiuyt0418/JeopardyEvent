using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestionWindow : MonoBehaviour
{
    public TMP_Text text;
    public CanvasGroup cg;
    // Start is called before the first frame update
    void Start()
    {
        if (cg == null)
        {
            cg = GetComponent<CanvasGroup>();
        }
        if (cg != null)
        {
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }

    public void Toggle()
    {
        cg.alpha = -cg.alpha + 1;
        cg.interactable = !cg.interactable;
        cg.blocksRaycasts = !cg.blocksRaycasts;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
